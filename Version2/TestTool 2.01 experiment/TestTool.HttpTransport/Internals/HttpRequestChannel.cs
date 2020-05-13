///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using TestTool.HttpTransport.Exceptions;
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
        private HttpTransportBindingElement _bindingElement;

        private RequestNetworkStream _networkStream;

        /// <summary>
        /// Controllers. 
        /// </summary>
        private IEnumerable<IChannelController> _controllers;
        /// <summary>
        /// Traffic listeners.
        /// </summary>
        private List<ITrafficListener> _listeners;
        /// <summary>
        /// Execution controller.
        /// </summary>
        private IExecutionController _executionController;

        private IEndpointController _endpointController;

        private List<IValidatingController> _validatingControllers;

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
            this._controllers = bindingElement.Contollers;

            _listeners = new List<ITrafficListener>();
            _validatingControllers = new List<IValidatingController>();

            foreach (IChannelController controller in _controllers)
            {
                if (controller is ITrafficListener)
                {
                    _listeners.Add((ITrafficListener) controller);
                }

                if (controller is IValidatingController)
                {
                    _validatingControllers.Add((IValidatingController)controller);
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
            }
            this._bindingElement = bindingElement;

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
            Message message3 = null;
            
            // check input parameters
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeout");
            }

            // check if underlying stream has not been closed by the server
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

            _networkStream.EnsureOpen(_to);

            base.ThrowIfDisposedOrNotOpen();
            

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

                if (header.StatusCode == 100)
                {
                    System.Diagnostics.Debug.WriteLine("100-Continue received");
                }

            } while (header.StatusCode == 100);

            System.Diagnostics.Debug.WriteLine("Real answer received");

            _networkStream.Close();

            int count = (int)responseStream.Length - header.BodyOffset;
            
            // parse response and notify listeners
            string response = HttpHelper.GetFormattedMessage(responseStream.GetBuffer(), header.BodyOffset);
            foreach (ITrafficListener listener in _listeners)
            {
                listener.LogResponse(response);
            }            

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
            
            int start = header.BodyOffset;

            if (start >= 0)
            {
                byte[] buffer = _bufferManager.TakeBuffer(count);
                Array.Copy(responseStream.GetBuffer(), start, buffer, 0, count);
                
                responseStream.Close();

                if (_validatingControllers.Count > 0)
                {
                    int offset = 0;
                    while (buffer[offset] != (byte)'<')
                    {
                        offset++;
                    }
                    
                    MemoryStream stream = new MemoryStream(buffer, offset, count-offset, false);
                    foreach (IValidatingController controller in _validatingControllers)
                    {
                        controller.Validate(stream);
                    }
                    stream.Close();
                }

                message3 = _encoder.ReadMessage(new ArraySegment<byte>(buffer, 0, count), _bufferManager);
            
                _bufferManager.ReturnBuffer(buffer);
            }
            else
            {
                responseStream.Close(); 
                throw new ProtocolException(string.Format("The server returned unexpected reply: {0} {1}", header.StatusCode, header.StatusDescription));
            }
            return message3;
        }
        
        void WriteMessageToStream(Message message)
        {
            int bodyOffset;
            byte[] messageBytes = CreateRequest(message, out bodyOffset);
            
            _networkStream.Write(messageBytes, 0, messageBytes.Length);

            string formattedRequest = HttpHelper.GetFormattedMessage(messageBytes, bodyOffset);

            foreach (ITrafficListener listener in  _listeners)
            {
                listener.LogRequest(formattedRequest);
            }
        }
        
        byte[] CreateRequest(Message message, out int bodyOffset)
        {
            // Encode message using _encoder  
            ArraySegment<byte> messageBuffer = EncodeMessage(message);

            // Copy byte in order not to use Buffer
            byte[] messageBytes;
            messageBytes = new byte[messageBuffer.Count];
            Array.Copy(messageBuffer.Array, messageBytes, messageBuffer.Count);
            
            // Modify message if necessary
            ISoapMessageMutator messageController = null;
            foreach (IChannelController controller in _bindingElement.Contollers)
            {
                if (controller is ISoapMessageMutator)
                {
                    messageController = (ISoapMessageMutator)controller;
                    break;
                }
            }
            
            if (messageController != null)
            {
                messageBytes = messageController.ProcessMessage(messageBytes);
            }

            // xml directive to add
            string xmlDirective = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            // create headers
            byte[] httpHeaders = HttpHelper.CreateHttpHeaders(messageBytes.Length + xmlDirective.Length,
                                                   _to.Uri.PathAndQuery,
                                                   _to.Uri.Host);

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

            // read response
            do
            {
                DateTime startTime = DateTime.Now;

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
                int handle = System.Threading.WaitHandle.WaitAny(handles, readTimeout);

                if (handle == WaitHandle.WaitTimeout)
                {
                    _networkStream.Close();
                    throw new IOException("The HTTP request has exceeded the allotted timeout");
                }

                if (handle == 1)
                {
                    //System.Diagnostics.Debug.WriteLine("Stop event");
                    _networkStream.Close();
                    _executionController.ReportStop();
                }

                bytes = _networkStream.EndRead(result);

                DateTime endTime = DateTime.Now;

                //System.Diagnostics.Debug.WriteLine(string.Format("--- Bytes received: {0}", bytes));

                responseStream.Write(responseBuffer, 0, bytes);

                // timeout for next part of answer
                readTimeout -= (int)((endTime - startTime).TotalMilliseconds);

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

                if (readTimeout < 0)
                {
                    _networkStream.Close();
                    throw new IOException("The HTTP request has exceeded the allotted timeout");
                }
                System.Diagnostics.Debug.WriteLine(string.Format("DataAvailable: {0}, continue: {1} ", _networkStream.DataAvailable, bContinue));
            }
            while (_networkStream.DataAvailable || bContinue);

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