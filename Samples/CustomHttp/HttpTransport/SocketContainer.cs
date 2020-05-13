using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace CustomHttp
{
    class SocketContainer
    {
        private static Socket _socket = null;
        private static bool _socketInitialized = false;
        public static Socket Socket
        {
            get
            {
                if (!_socketInitialized)
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.3.144"), 80);
                    _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Unspecified);
                    _socket.Connect(endPoint);
                    _socketInitialized = true;
                }
                if (!_socket.Connected)
                {
                    _socket.Connect(IPAddress.Parse("192.168.3.144"), 80);
                }
                return _socket;
            }
        }
    }
}
