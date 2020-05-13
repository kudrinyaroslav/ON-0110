using System;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.ServiceModel.Samples;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace CustomHttpTransport
{
    public class CustomHttpRequestChannel : ChannelBase, IRequestChannel
    {
        private CustomHttpChannelFactory _factory;
        private EndpointAddress _to;
        private Uri _via;
        private bool _manualAddressing;
        MessageEncoder _encoder;
        BufferManager bufferManager;

        private NetworkStream _networkStream;
        private Socket _socket;

        private bool _active;

        #region Copied from disassembled code

        internal CustomHttpRequestChannel(CustomHttpChannelFactory factory, 
            EndpointAddress to, 
            Uri via,
            bool manualAddressing,
            MessageEncoder encoder, 
            CustomHttpTransportBindingElement bindingElement)
            : base(factory)
        {
            if (!manualAddressing && (to == null))
            {
                throw new ArgumentNullException("to");
            }

            this._factory = factory;
            this._to = to;
            this._via = via;
            this._encoder = encoder;
            this._manualAddressing = manualAddressing;

            this.bufferManager = BufferManager.CreateBufferManager(bindingElement.MaxBufferPoolSize, (int)bindingElement.MaxReceivedMessageSize);

        }

        void Connect()
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses(_to.Uri.Host);

            Exception exception = null;
            Socket socketIp4 = null;
            Socket socketIp6 = null;
            try
            {
                if (this._socket == null)
                {
                    if (Socket.SupportsIPv4)
                    {
                        socketIp4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    }
                    if (Socket.OSSupportsIPv6)
                    {
                        socketIp6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                    }
                }
                foreach (IPAddress address in hostAddresses)
                {
                    try
                    {
                        if (this._socket == null)
                        {
                            if ((address.AddressFamily == AddressFamily.InterNetwork) && (socketIp6 != null))
                            {
                                socketIp6.Connect(address, _to.Uri.Port);
                                this._socket = socketIp6;
                                if (socketIp4 != null)
                                {
                                    socketIp4.Close();
                                }
                            }
                            else if (socketIp4 != null)
                            {
                                socketIp4.Connect(address, _to.Uri.Port);
                                this._socket = socketIp4;
                                if (socketIp6  != null)
                                {
                                    socketIp6.Close();
                                }
                            }
                            this._active = true;
                            return;
                        }
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            _socket.Connect(new IPEndPoint(address, _to.Uri.Port));
                            this._active = true;
                            return;
                        }
                    }
                    catch (Exception exception2)
                    {
                        if (((exception2 is ThreadAbortException) || (exception2 is StackOverflowException)) || (exception2 is OutOfMemoryException))
                        {
                            throw;
                        }
                        exception = exception2;
                    }
                }
            }
            catch (Exception exception3)
            {
                if (((exception3 is ThreadAbortException) || (exception3 is StackOverflowException)) || (exception3 is OutOfMemoryException))
                {
                    throw;
                }
                exception = exception3;
            }
            finally
            {
                if (!this._active)
                {
                    if (socketIp4 != null)
                    {
                        socketIp4.Close();
                    }
                    if (socketIp6 != null)
                    {
                        socketIp6.Close();
                    }
                    if (exception != null)
                    {
                        throw exception;
                    }
                    throw new SocketException((int)SocketError.NotConnected);
                }
            }

        }

        protected virtual void AddHeadersTo(Message message)
        {
            if (!this._manualAddressing && (this._to != null))
            {
                this._to.ApplyTo(message);
            }
        }

        private ChannelParameterCollection channelParameters;
 
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

        public Message Request(Message message, TimeSpan timeout)
        {
            Message message3 = null;
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeout");
            }
            base.ThrowIfDisposedOrNotOpen();
            this.AddHeadersTo(message);
            
            byte[] messageBytes = CreateRequest(message);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--> Sent: ");
            Console.ResetColor();
            Console.WriteLine(Encoding.UTF8.GetString(messageBytes));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("...");
            Console.ResetColor();


            _networkStream.Write(messageBytes, 0, messageBytes.Length);

            _networkStream.ReadTimeout = 60000;

            MemoryStream responseStream = new MemoryStream();

            int bytes = 0;
            byte[] responseBuffer = new byte[2048];

            System.Threading.Thread.Sleep(100);
            
            do
            {
                bytes = _networkStream.Read(responseBuffer, 0, responseBuffer.Length);
                responseStream.Write(responseBuffer, 0, bytes);

                //string responsePart = Encoding.UTF8.GetString(responseBuffer, 0, bytes);
                //Console.WriteLine(responsePart);

                _networkStream.ReadTimeout = 5000;
            }
            while (_networkStream.DataAvailable);

            //netStream.Close();

            string response = Encoding.UTF8.GetString(responseStream.GetBuffer());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("<-- Received: ");
            Console.ResetColor();
            Console.WriteLine(response);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***");
            Console.ResetColor();
            
            int start = response.IndexOf("<?xml");
            if (start >= 0)
            {
                int count = (int) responseStream.Length - start;
                byte[] buffer = bufferManager.TakeBuffer(count);
                Array.Copy(responseStream.GetBuffer(), start, buffer, 0, count);

                string strResponse = Encoding.UTF8.GetString(buffer);

                message3 = _encoder.ReadMessage(new ArraySegment<byte>(buffer, 0, count), bufferManager);
            
                if (message3.IsFault)
                {
                    Console.WriteLine("FAULT");
                }

            }
            return message3;
        }

        byte[] CreateRequest(Message message)
        {
            ArraySegment<byte> messageBuffer = EncodeMessage(message);

            byte[] httpHeaders = CreateHttpHeaders(messageBuffer.Count,
                                                   _to.Uri.AbsolutePath,
                                                   _to.Uri.Host);
            byte[] bytes = new byte[httpHeaders.Length + messageBuffer.Count];

            Array.Copy(httpHeaders, bytes, httpHeaders.Length);

            Array.Copy(messageBuffer.Array, messageBuffer.Offset, bytes, httpHeaders.Length, messageBuffer.Count);

            return bytes;
        }

        ArraySegment<byte> EncodeMessage(Message message)
        {
            try
            {
                //this.remoteAddress.ApplyTo(message);
                return _encoder.WriteMessage(message, int.MaxValue, _factory.BufferManager);
            }
            finally
            {
                // We have consumed the message by serializing it, so clean up
                message.Close();
            }
        }

        private byte[] CreateHttpHeaders(long size, string path, string address)
        {
            string xmlDirective = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            // Create HTTP headers and add content
            StringBuilder httpRequest = new StringBuilder();
            httpRequest.Append("POST " + path + " HTTP/1.1\r\n");
            httpRequest.Append("Host: " + address + "\r\n");
            httpRequest.Append("Content-Type: application/soap+xml; charset=utf-8\r\n");
            httpRequest.Append("Content-Length: " + (size + xmlDirective.Length).ToString() + "\r\n");
            httpRequest.Append("\r\n");

            httpRequest.Append(xmlDirective);
            // Convert HTTP request to byte array to send
            return (Encoding.UTF8.GetBytes(httpRequest.ToString()));
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            Connect();
            _networkStream = new NetworkStream(_socket);
        }
    }
}
