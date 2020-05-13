///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using TestTool.HttpTransport.Interfaces.Exceptions;
using TestTool.HttpTransport.Interfaces;
using TestTool.HttpTransport.Internals.Http;

namespace TestTool.HttpTransport
{
    class HttpRequestChannel : ChannelBase, IRequestChannel
    {
        /// <summary>
        /// Factory.
        /// </summary>
        private HttpChannelFactory _factory;
        
        /// <summary>
        /// Destination
        /// </summary>
        private EndpointAddress _to;

        /// <summary>
        /// 
        /// </summary>
        private Uri _via;
        
        MessageEncoder _encoder;
        BufferManager _bufferManager;

        private RequestNetworkStream _networkStream;

        /// <summary>
        /// Traffic listeners.
        /// </summary>
        private List<ITrafficListener> _listeners;
        /// <summary>
        /// Execution controller.
        /// </summary>
        private IExecutionController _executionController;

        private IEndpointController _endpointController;

        private ICredentialsProvider _credentialsProvider;

        /// <summary>
        /// authentication challenge, if once received
        /// </summary>
        private HttpPacket _digestAuthChallenge = null;

        /// <summary>
        /// current message encoded
        /// </summary>
        private byte[] _currentMessageBytes;

        internal HttpRequestChannel(HttpChannelFactory factory, 
            EndpointAddress to, 
            Uri via,
            MessageEncoder encoder, 
            HttpTransportBindingElement bindingElement)
            : base(factory)
        {
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            this._factory = factory;
            this._to = to;
            this._via = via;
            this._encoder = encoder;

            _listeners = new List<ITrafficListener>();

            foreach (IChannelController controller in bindingElement.Controllers)
            {
                if (controller is ITrafficListener)
                {
                    _listeners.Add((ITrafficListener) controller);
                }
                
                if (controller is IEndpointController)
                {
                    if (_endpointController != null)
                    {
                        throw new ApplicationException("Only one channel controller of type IAddressProvider can be set");
                    }
                    _endpointController = (IEndpointController) controller;
                }
                if (controller is IExecutionController)
                {
                    if (_executionController != null)
                    {
                        throw new ApplicationException("Only one channel controller of type IExecutionController can be set");
                    }
                    _executionController = (IExecutionController)controller;
                }
                if (controller is ICredentialsProvider)
                {
                    _credentialsProvider = controller as ICredentialsProvider;
                }
            }
            this._networkStream = new RequestNetworkStream(to);

            this._bufferManager = BufferManager.CreateBufferManager(bindingElement.MaxBufferPoolSize, (int)bindingElement.MaxReceivedMessageSize);
        }

        ~HttpRequestChannel()
        {
            // cannot close faulted channel
            if (State == CommunicationState.Opened) Close();
        }

        private ChannelParameterCollection channelParameters;

        #region Request

        public Message Request(Message message, TimeSpan timeout)
        {
            // check input parameters
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeout");
            }

            if (_endpointController != null)
            {
                _to = _endpointController.Address;

                if (_endpointController.WsaEnabled)
                {
                    message.Headers.To = _endpointController.Address.Uri;
                }
                else
                {
                    // clear headers since not all cameras understand them.
                    message.Headers.Action = null;
                    message.Headers.ReplyTo = null;
                    message.Headers.MessageId = null;
                }
            }
            else
            {
                // clear headers since not all cameras understand them.
                message.Headers.Action = null;
                message.Headers.ReplyTo = null;
                message.Headers.MessageId = null;
            }

            // check if underlying stream has not been closed by the server
            _networkStream.EnsureOpen(_to);

            base.ThrowIfDisposedOrNotOpen();

            _currentMessageBytes = null;

            // send message
            WriteMessageToStream(message);

            // Start reading
            // Wait first byte for timeout.TotalMilliseconds
            int readTimeout = (int)timeout.TotalMilliseconds;

            // initialize variables
            MemoryStream responseStream;
            HttpPacket header = null;

            do
            {
                responseStream = new MemoryStream();
                GetResponse(responseStream, out header, readTimeout);
            } while (header.StatusCode == 100);
            
            // check status 401
            bool digestEnabled = false;
            if (_credentialsProvider != null)
            {
                digestEnabled = _credentialsProvider.Security == Security.Digest;
            }

            if (header.StatusCode == 401)
            {
                if (digestEnabled)
                {
                    System.Diagnostics.Debug.WriteLine("HTTP 401 received");

                    foreach (string connectionHeader in header.Connection)
                    {
                        if (StringComparer.InvariantCultureIgnoreCase.Compare(connectionHeader, "close") == 0)
                        {
                            _networkStream.Close();
                            _networkStream.EnsureOpen(_to);
                            break;
                        }
                    }
                    
                    // Send request once more.
                    _digestAuthChallenge = header;

                    // uses _digestAuthChallenge (and _currentMessageBytes), so behaviour will be different
                    // (_digestAuthChallenge might be not null by the first attempt, but if we get status 401,
                    // it will be updated)
                    WriteMessageToStream(message);

                    do
                    {
                        responseStream = new MemoryStream();
                        GetResponse(responseStream, out header, readTimeout);
                    } while (header.StatusCode == 100);

                    if (header.StatusCode == 401)
                    {
                        LogResponse(responseStream, header);
                        _networkStream.Close();
                        throw new AccessDeniedException("Digest authentication FAILED (HTTP status 401 received)");
                    }
                }
                else
                {
                    _networkStream.Close();
                    LogResponse(responseStream, header);
                    throw new AccessDeniedException("Access denied (HTTP status 401 received)");
                }
            }

            foreach (string connectionHeader in header.Connection)
            {
                if (StringComparer.InvariantCultureIgnoreCase.Compare(connectionHeader, "close") == 0)
                {
                    _networkStream.Close();
                    break;
                }
            }
            
            int count = (int)responseStream.Length - header.BodyOffset;
            
            // parse response and notify listeners
            LogResponse(responseStream, header);

            if (header.ContentLength < count)
            {
                if (header.Headers.ContainsKey(HttpHelper.CONTENTLENGTH))
                {
                    throw new HttpProtocolException(
                        string.Format("An error occurred while receiving packet. Expected length: {0}, received: {1}",
                                      header.ContentLength, count));
                }
                else
                {
                    if (!header.NoBodySupposed)
                    {
                        if (header.StatusCode != 200)
                        {
                            throw new HttpProtocolException(
                                string.Format("An error returned. Error code: {0}, error description: {1}",
                                              header.StatusCode, header.StatusDescription));
                        }
                        else
                        {
                            throw new HttpProtocolException("Content-Length header is missing");
                        }
                    }
                }
            }
            
            // validate headers
            HttpHelper.ValidateHttpHeaders(header);

            return ReadMessage(responseStream, header);
        }

        private Message ReadMessage(MemoryStream responseStream, HttpPacket header)
        {
            var typeOfEncoding = GetTypeOfEncoding(header.ContentType);

            switch (typeOfEncoding)
            {
                case WSMessageEncoding.Text:
                    return ReadMessage(responseStream
                                      , header
                                      , header.BodyOffset
                                      , (int)(responseStream.Length - header.BodyOffset));
                case WSMessageEncoding.Mtom:

                    MemoryStream correctedResponse = new MemoryStream();
                    StreamWriter sw = new StreamWriter(correctedResponse);

                    MemoryStream streamCopy = new MemoryStream(responseStream.GetBuffer());
                    StreamReader rdr = new StreamReader(streamCopy);

                    bool emptyFound = false;
                    
                    while (!rdr.EndOfStream)
                    {
                        string nextLine = rdr.ReadLine();
                        if (!emptyFound)
                        {
                            bool write = nextLine.StartsWith(HttpHelper.CONTENTTYPE);

                            if (string.IsNullOrEmpty(nextLine ))
                            {
                                write = true;
                                emptyFound = true;
                            }

                            if (!string.IsNullOrEmpty(nextLine) && string.IsNullOrEmpty(nextLine.Trim('\0')))
                            {
                                write = true;
                                emptyFound = true; 
                            }

                            if (write)
                            {
                                sw.WriteLine(nextLine);
                            }

                        }
                        else
                        {
                            sw.WriteLine(nextLine);
                        }
                    }

                    rdr.Close();
                    sw.Flush();
                    correctedResponse.Seek(0, SeekOrigin.Begin);

                    return ReadMessage(correctedResponse
                                      , header
                                      , 0
                                      , (int)correctedResponse.Length);
                default:
                    throw new NotSupportedException(string.Format("\"{0}\" message encoding are not supported.", typeOfEncoding));
            }
        }

        private Message ReadMessage(MemoryStream responseStream, HttpPacket header, int start, int count)
        {
            Message result = null;

            if (start >= 0)
            {
                byte[] buffer = _bufferManager.TakeBuffer(count);
                Array.Copy(responseStream.GetBuffer(), start, buffer, 0, count);

                responseStream.Close();

                result = _encoder.ReadMessage(new ArraySegment<byte>(buffer, 0, count), _bufferManager, header.ContentType);

                _bufferManager.ReturnBuffer(buffer);
            }
            else
            {
                responseStream.Close();
                throw new ProtocolException(string.Format("The server returned unexpected reply: {0} {1}", header.StatusCode,
                                                          header.StatusDescription));
            }
            return result;
        }

        private WSMessageEncoding GetTypeOfEncoding(string contentType)
        {
            return Regex.IsMatch(contentType, "^multipart.*") 
                       ? WSMessageEncoding.Mtom 
                       : WSMessageEncoding.Text;
        }

        void WriteMessageToStream(Message message)
        {

            int bodyOffset;
            byte[] messageBytes = CreateRequest(message, out bodyOffset);
            
            _networkStream.Write(messageBytes, 0, messageBytes.Length);

            string formattedRequest = HttpHelper.GetFormattedMessage(messageBytes, bodyOffset);

            foreach (ITrafficListener listener in _listeners)
            {
                listener.LogRequest(formattedRequest);
            }
        }

        byte[] CreateRequest(Message message, out int bodyOffset)
        {
            byte[] messageBytes;

            if (_currentMessageBytes == null)
            {
                // Encode message using _encoder  
                ArraySegment<byte> messageBuffer = EncodeMessage(message);

                // Copy byte in order not to use Buffer
                messageBytes = new byte[messageBuffer.Count];
                Array.Copy(messageBuffer.Array, messageBytes, messageBuffer.Count);

                _currentMessageBytes = messageBytes;
            }
            else
            {
                messageBytes = _currentMessageBytes;
            }

            // xml directive to add
            string xmlDirective = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            string username = string.Empty;
            string password = string.Empty;
            HttpPacket header = null;
            if (_credentialsProvider != null)
            {
                username = _credentialsProvider.Username;
                password = _credentialsProvider.Password;
                if (_credentialsProvider.Security == Security.Digest)
                {
                    header = _digestAuthChallenge;
                }
            }

            // create headers
            byte[] httpHeaders = HttpHelper.CreateHttpHeaders(messageBytes.Length + xmlDirective.Length,
                                                   _to.Uri.PathAndQuery,
                                                   _to.Uri.Host, 
                                                   header, username, password);

            //whole message with headers
            byte[] bytes = new byte[httpHeaders.Length + messageBytes.Length + xmlDirective.Length];

            Array.Copy(httpHeaders, bytes, httpHeaders.Length);

            Array.Copy(Encoding.UTF8.GetBytes(xmlDirective), 0, bytes, httpHeaders.Length, xmlDirective.Length);

            Array.Copy(messageBytes, 0, bytes, httpHeaders.Length+xmlDirective.Length, messageBytes.Length);

            // body offset - for logger
            bodyOffset = httpHeaders.Length;

            return bytes;
        }

        ArraySegment<byte> EncodeMessage(Message message)
        {
            try
            {
                return _encoder.WriteMessage(message, int.MaxValue, _factory.BufferManager);
            }
            finally
            {
                // We have consumed the message by serializing it, so clean up
                message.Close();
            }
        }

        void GetResponse(MemoryStream responseStream, out HttpPacket header, int readTimeout)
        {
            int bytes = 0;
            byte[] responseBuffer = new byte[2048];
            bool bContinue = false;
            
            int readTimeoutLocal = readTimeout;

            DateTime startTime = DateTime.Now;

            // read response
            do
            {
                IAsyncResult result = _networkStream.BeginRead(responseBuffer, 0, responseBuffer.Length);

                // wait for bytes received, stop event or timeout
                WaitHandle[] handles;
                if (_executionController != null && _executionController.StopEvent != null)
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle, _executionController.StopEvent };
                }
                else
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle };
                }
                int handle = System.Threading.WaitHandle.WaitAny(handles, readTimeoutLocal);

                if (handle == WaitHandle.WaitTimeout)
                {
                    _networkStream.Close();
                    throw new TestTool.HttpTransport.Interfaces.Exceptions.TimeoutException("The HTTP request has exceeded the allotted timeout");
                }

                if (handle == 1)
                {
                    //Stop event
                    _networkStream.Close();
                    _executionController.ReportStop();
                }

                bytes = _networkStream.EndRead(result);

                DateTime endTime = DateTime.Now;

                //System.Diagnostics.Debug.WriteLine(string.Format("--- Bytes received: {0}", bytes));

                responseStream.Write(responseBuffer, 0, bytes);

                // timeout for next part of answer
                int ms = (int)((endTime - startTime).TotalMilliseconds);

                // parse response received by this moment.
                try
                {
                    bContinue = HttpHelper.ContinueReading(responseStream, out header);
                }
                catch (Exception exc)
                {
                    _networkStream.Close();
                    // clean resources somehow ?
                    throw new Exception("An error occurred while parsing HTTP packet", exc);
                }

                readTimeoutLocal = readTimeout - ms;
                //System.Diagnostics.Debug.WriteLine("Timeout: " + readTimeoutLocal);
                if (readTimeoutLocal < 0)
                {
                    _networkStream.Close();
                    throw new IOException("The HTTP request has exceeded the allotted timeout");
                }

                //System.Diagnostics.Debug.WriteLine(string.Format("DataAvailable: {0}, continue: {1} ", _networkStream.DataAvailable, bContinue));
            }
            while (_networkStream.DataAvailable || bContinue);

        }

        void LogResponse(MemoryStream responseStream, HttpPacket header)
        {
            string response = HttpHelper.GetFormattedMessage(responseStream.GetBuffer(), header.BodyOffset);
            foreach (ITrafficListener listener in _listeners)
            {
                listener.LogResponse(response);
            }            
        }

        #endregion

        #region IRequestChannel Members

        public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
        {
            return this.BeginRequest(message, DefaultSendTimeout, callback, state);

        }

        public Message EndRequest(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.EndpointAddress RemoteAddress
        {
            get { return _to; }
        }
        
        public Message Request(Message message)
        {
            return Request(message, DefaultSendTimeout);
        }

        public Uri Via
        {
            get { return _via; }
        }

        #endregion

        #region IChannel Members

        public T GetProperty<T>() where T : class
        {
            if (typeof(T) != typeof(ChannelParameterCollection))
            {
                if (typeof(T) == typeof(IRequestChannel))
                {
                    return (this as T);
                }
                T property = base.GetProperty<T>();
                if (property != null)
                {
                    return property;
                }
                return default(T);                
            }
            if (base.State == CommunicationState.Created)
            {
                lock (base.ThisLock)
                {
                    if (this.channelParameters == null)
                    {
                        this.channelParameters = new ChannelParameterCollection();
                    }
                }
            }
            return (this.channelParameters as T);
        }

        #endregion


        
        protected override void OnAbort()
        {
            _networkStream.Close();
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            _networkStream.Close();
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _networkStream.Connect();
        }
    }
}
