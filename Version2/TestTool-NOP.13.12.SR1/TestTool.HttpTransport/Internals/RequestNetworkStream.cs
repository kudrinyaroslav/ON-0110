///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Net.Sockets;
using System.Net;
using System.ServiceModel;
using System.Threading;
using TestTool.Tests.Common.InternalLogger;

namespace TestTool.HttpTransport
{
    /// <summary>
    /// Wrapper for NetworkStream
    /// </summary>
    class RequestNetworkStream
    {
        private bool InternalLog = false;

        private NetworkStream _networkStream;
        private Socket _socket;

        private EndpointAddress _to;
        private EndPoint _endpoint;

        private bool _active;

        public RequestNetworkStream(EndpointAddress to)
        {
            _to = to;
            if (!InternalLog)
                InternalLogger.GetInstance().SwitchOffForCurrentThread();
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

        public void Close()
        {
            if (_networkStream != null)
            {
                _networkStream.Close();
            }

            try
            {
                if (_socket != null)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
            }
            finally
            {
                _socket = null;
            }
            _active = false;
        }
        
        public void EnsureOpen(EndpointAddress address)
        {
            InternalLogger.GetInstance().LogMessage("Entered EnsureOpen");
            try
            {
                //bool close = false;
                bool close = _to != address;

                if (close && null != _to)
                    InternalLogger.GetInstance().LogMessage(string.Format("Endpoint address changed. Old: {0}. New: {1}.", _to, address));

                _to = address;

                var createFlag = false;
                if (_socket == null)
                {
                    close = true;
                    createFlag = true;
                    InternalLogger.GetInstance().LogMessage(string.Format("Socket for endpoint with address {0} does not exist.", _to));
                }
                else
                {
                    InternalLogger.GetInstance().LogMessage(string.Format("Socket exists and is{0} connected to remote host.", _socket.Connected ? "" : "n't"));
                    if (_socket.Connected)
                    {
                        var timeout = 500;
                        bool writePoll = _socket.Poll(timeout, SelectMode.SelectWrite);
                        InternalLogger.GetInstance().LogMessage(string.Format("Socket.Poll({0}, SelectMode.SelectWrite): {1}.", timeout, writePoll));
                        if (writePoll)
                        {
                            try
                            {
                                _socket.Send(new byte[0]);
                                InternalLogger.GetInstance().LogMessage("Socket.Send is called successfully.");
                            }
                            catch (SocketException e)
                            {
                                InternalLogger.GetInstance().LogMessage(string.Format("Socket.Send has thrown an exception: {0}", e.Message));
                                if (!e.NativeErrorCode.Equals(SocketError.WouldBlock))
                                {
                                    InternalLogger.GetInstance().LogMessage(string.Format("    The socket is dead because thrown exception isn't WSAEWOULDBLOCK."));
                                    close = true;
                                }
                                else
                                {
                                    InternalLogger.GetInstance().LogMessage(string.Format("    The socket is alive because thrown exception is WSAEWOULDBLOCK."));
                                }
                            }

                            var readPoll = _socket.Poll(timeout, SelectMode.SelectRead);
                            if (!close && readPoll)
                            {
                                InternalLogger.GetInstance().LogMessage(string.Format("Socket.Poll({0}, SelectMode.SelectRead): {1}.", timeout, readPoll));
                                var T = _socket.ReceiveTimeout;
                                try
                                {
                                    //By MSDN _socket.Poll(timeout, SelectMode.SelectRead) can return true if the connection has been closed, reset, or terminated.
                                    //For example, if socket receives TCP packet with flags [FIN, ACK] _socket.Poll(timeout, SelectMode.SelectRead) returns true.
                                    //To check this case we perform fake Receive with ReceiveTimeout = 1.
                                    //If ec == SocketError.Success then the connection has been closed, reset, or terminated.
                                    //For example, if connection is still alive then ec after fake Receive will be SocketError.TimedOut.
                                    SocketError ec;
                                    _socket.ReceiveTimeout = 1;
                                    _socket.Receive(new byte[0], 0, 0, SocketFlags.Peek, out ec);
                                    InternalLogger.GetInstance().LogMessage(string.Format("Socket.Receive is called successfully. Socket Error: {0}.", ec));
                                    close = SocketError.Success == ec;
                                }
                                catch (Exception e)
                                {
                                    InternalLogger.GetInstance().LogMessage(string.Format("    The socket is dead because Socket.Send has thrown an exception: {0}.", e.Message));
                                    close = true;
                                }
                                finally
                                {
                                    _socket.ReceiveTimeout = T;
                                }
                            }
                        }
                        else
                        {
                            close = true;
                        }
                    }
                    else
                    {
                        close = true;
                    }
                }
                if (close)
                {
                    InternalLogger.GetInstance().LogMessage(string.Format("Socket for endpoint with address {0} will be {1}created.", _to, createFlag ? "re" : ""));
                    
                    Close();
                    Connect();
                }
            }
            finally
            {
                InternalLogger.GetInstance().LogMessage("Exited EnsureOpen");
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

        internal bool Connected
        {
            get { return _socket != null ? _socket.Connected : false; }
        }
    }
}
