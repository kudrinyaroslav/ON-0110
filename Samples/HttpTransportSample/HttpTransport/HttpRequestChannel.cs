using System;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace HttpTransport
{
    public class HttpRequestChannel : ChannelBase, IRequestChannel
    {

        private const int READTIMEOUT = 500;

        private HttpChannelFactory _factory;
        
        private EndpointAddress _to;
        private Uri _via;
        
        MessageEncoder _encoder;
        BufferManager bufferManager;
        private HttpTransportBindingElement _bindingElement;

        private RequestNetworkStream _networkStream;

        private IEnumerable<IChannelController> _controllers;
        private List<ITrafficListener> _listeners;
        private IExecutionController _executionController;

        #region Copied from disassembled code

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
            foreach (IChannelController controller in _controllers)
            {
                if (controller is ITrafficListener)
                {
                    _listeners.Add((ITrafficListener) controller);
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

            this.bufferManager = BufferManager.CreateBufferManager(bindingElement.MaxBufferPoolSize, (int)bindingElement.MaxReceivedMessageSize);
        }

        private ChannelParameterCollection channelParameters;
        
        #endregion

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

            // check if underling stream has not been closed by the server
            _networkStream.EnsureOpen();

            base.ThrowIfDisposedOrNotOpen();
            
            // clear headers
            message.Headers.Clear();
            
            // send message
            WriteMessageToStream(message);

            // Start reading
            // Wait first byte for timeout.TotalMilliseconds
            int readTimeout = (int)timeout.TotalMilliseconds;

            // initialize variables
            MemoryStream responseStream = new MemoryStream();
            int bytes = 0;
            byte[] responseBuffer = new byte[2048];
            bool bContinue = false;
            HttpPacket header = null;

            // read response
            do
            {
                IAsyncResult result = _networkStream.BeginRead(responseBuffer, 0, responseBuffer.Length);

                // wait for bytes receied, stop event or timeout
                WaitHandle[] handles;
                if (_executionController != null && _executionController.StopEvent != null)
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle, _executionController.StopEvent };
                }
                else
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle};
                }
                int handle = System.Threading.WaitHandle.WaitAny(handles, readTimeout);

                if (handle == WaitHandle.WaitTimeout)
                {
                    _networkStream.Close(true);
                    throw new IOException("The HTTP request has exceeded the allotted timeout");
                }
                if (handle == 1)
                {
                    System.Diagnostics.Debug.WriteLine("Stop event");
                    throw new StopEventException("Stop waiting for the answer");
                }

                bytes = _networkStream.EndRead(result);
                
                responseStream.Write(responseBuffer, 0, bytes);

                // timeout for next part of answer
                readTimeout = READTIMEOUT;

                // parse response received by this moment.
                try
                {
                    bContinue = HttpHelper.ContinueReading(responseStream, out header);
                }
                catch (Exception exc)
                {
                    // clean resources somehow ?
                    throw new Exception("An error occurred while parsing HTTP packet", exc);
                }
            }
            while (_networkStream.DataAvailable || bContinue);

            int count = (int)responseStream.Length - header.BodyOffset;

            if (header.ContentLength < count)
            {
                throw new HttpProtocolException(
                    string.Format("An error occurred while receiving packet. Expected length: {0}, received: {1}",
                                  header.ContentLength, count));
            }
            
            // validate headers
            HttpHelper.ValidateHttpHeaders(header);

            // check if connection is to be closed
            if (header.Headers.ContainsKey("Connection"))
            {
                string connState = header.Headers["Connection"];
                if (connState.ToLower() == "close")
                {
                    _networkStream.Close(true);
                }
            }

            // parse response
            string response = Encoding.UTF8.GetString(responseStream.GetBuffer());
            
            foreach (ITrafficListener listener in _listeners)
            {
                listener.LogResponse(response);
            }

            int start = header.BodyOffset;

            if (start >= 0)
            {
                byte[] buffer = bufferManager.TakeBuffer(count);
                Array.Copy(responseStream.GetBuffer(), start, buffer, 0, count);
                
                message3 = _encoder.ReadMessage(new ArraySegment<byte>(buffer, 0, count), bufferManager);
            
                bufferManager.ReturnBuffer(buffer);

                if (message3.IsFault)
                {
                    Console.WriteLine("FAULT");
                    ThrowFaultException(message3);
                }
            }
            else
            {
                throw new ProtocolException(string.Format("The server returned unexpected reply: {0} {1}", header.StatusCode, header.StatusDescription));
            }
            return message3;
        }
        
        void WriteMessageToStream(Message message)
        {
            byte[] messageBytes = CreateRequest(message);

            foreach (ITrafficListener listener in  _listeners)
            {
                listener.LogRequest(Encoding.UTF8.GetString(messageBytes));
            }
            _networkStream.Write(messageBytes, 0, messageBytes.Length);
        }

        void ThrowFaultException(Message message)
        {
            string action = message.Headers.Action;
            MessageFault fault = MessageFault.CreateFault(message, (int)_bindingElement.MaxReceivedMessageSize);
            throw new FaultException(fault);
        }

        byte[] CreateRequest(Message message)
        {
            ArraySegment<byte> messageBuffer = EncodeMessage(message);

            byte[] messageBytes = messageBuffer.Array;

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

            byte[] httpHeaders = HttpHelper.CreateHttpHeaders(messageBytes.Length,
                                                   _to.Uri.AbsolutePath,
                                                   _to.Uri.Host);
            byte[] bytes = new byte[httpHeaders.Length + messageBytes.Length];

            Array.Copy(httpHeaders, bytes, httpHeaders.Length);

            Array.Copy(messageBytes, 0, bytes, httpHeaders.Length, messageBytes.Length);

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
