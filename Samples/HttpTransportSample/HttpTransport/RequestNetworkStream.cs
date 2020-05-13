using System;
using System.Net.Sockets;
using System.Net;
using System.ServiceModel;
using System.Threading;

namespace HttpTransport
{
    /// <summary>
    /// Wrapper for NetworkStream
    /// </summary>
    class RequestNetworkStream
    {
        private NetworkStream _networkStream;
        private Socket _socket;

        private EndpointAddress _to;
        private EndPoint _endpoint;

        private bool _active;
        private bool _markedForReconnect;

        public RequestNetworkStream(EndpointAddress to)
        {
            _to = to;

        }

        public void Connect()
        {
            IPAddress[] hostAddresses = null;

            IPAddress deviceAddress;
            if (IPAddress.TryParse(_to.Uri.Host, out deviceAddress))
            {
                hostAddresses = new IPAddress[] { deviceAddress };
            }
            else
            {
                hostAddresses = Dns.GetHostAddresses(_to.Uri.Host);
            }

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
                        EndPoint endPoint = new IPEndPoint(address, _to.Uri.Port);
                        if (this._socket == null)
                        {
                            if ((address.AddressFamily == AddressFamily.InterNetwork) && (socketIp4 != null))
                            {
                                socketIp4.Connect(endPoint);
                                _endpoint = endPoint;
                                this._socket = socketIp4;
                                if (socketIp6 != null)
                                {
                                    socketIp6.Close();
                                }
                            }
                            else if (socketIp6 != null)
                            {
                                socketIp6.Connect(endPoint);
                                _endpoint = endPoint;
                                this._socket = socketIp6;
                                if (socketIp4 != null)
                                {
                                    socketIp4.Close();
                                }
                            }
                            this._active = true;
                            _networkStream = new NetworkStream(_socket);

                            return;
                        }
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            _socket.Connect(endPoint);
                            _endpoint = endPoint;
                            this._active = true;
                            _networkStream = new NetworkStream(_socket);
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

        public void Close(bool markForReconnect)
        {
            if (_networkStream != null)
            {
                _networkStream.Close();
            }
            if (_socket != null && _socket.Connected)
            {
                _socket.Close();
            }
            _active = false;
            _markedForReconnect = markForReconnect;

        }

        public void Close()
        {
            Close(false);
        }

        public void EnsureOpen()
        {
            if (_markedForReconnect)
            {
                _socket = null;
                Connect();
                _markedForReconnect = false;
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _networkStream.Write(buffer, offset, count);
        }

        public int ReadTimeout
        {
            get { return _networkStream.ReadTimeout; }
            set { _networkStream.ReadTimeout = value; }
        }

        public bool DataAvailable
        {
            get { return _networkStream.DataAvailable; }
        }

        public IAsyncResult BeginRead(byte[] buffer, int offset, int size)
        {
            return _networkStream.BeginRead(buffer, offset, size, null, null);
        }

        public int EndRead(IAsyncResult result)
        {
            return _networkStream.EndRead(result);
        }
    }
}
