///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace TestTool.Tests.Common.Discovery
{
    public class DiscoverySocketEventArgs : EventArgs
    {
        public byte[] Message { get; set; }
        public IPEndPoint Source { get; set; }
    }
    public class DiscoverySocket : IDisposable
    {
        public UdpClient Socket { get; protected set; }
        private IPEndPoint _endpoint; 
        
        public event EventHandler<DiscoverySocketEventArgs> MessageReceived;

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="endpoint">Local binding endpoint</param>
        public DiscoverySocket(IPEndPoint endpoint)
        {
            _endpoint = new IPEndPoint(endpoint.Address, endpoint.Port);

            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} C-tor", GetHashCode()));
            System.Diagnostics.Trace.Flush();
#if true
            if (OSInfo.MajorVersion == 5 && OSInfo.MinorVersion == 1)
            {
                Socket = new UdpClient(endpoint);
            }
            else
            {
                Socket = new UdpClient(endpoint.AddressFamily);
            }
            
            Socket.Client.ReceiveBufferSize = 4 * 1024;
            Socket.Client.SendBufferSize = 4 * 1024;
            Socket.Ttl = 10;
            Socket.MulticastLoopback = false;
            Socket.Client.ReceiveBufferSize = 50000;

            if (OSInfo.MajorVersion == 5 && OSInfo.MinorVersion == 1)
            {
                //if ((endpoint.Address != IPAddress.Any) && (endpoint.Address != IPAddress.IPv6Any))
                //{
                //    Socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //    Socket.Client.Bind(endpoint);
                //}
            }
            else
            {
                if ((endpoint.Address != IPAddress.Any) && (endpoint.Address != IPAddress.IPv6Any))
                {
                    Socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    Socket.Client.Bind(endpoint);
                }
            }

#else
            Socket = new UdpClient(endpoint);
            Socket.Client.ReceiveBufferSize = 4 * 1024;
            Socket.Client.SendBufferSize = 4 * 1024;
            Socket.Ttl = 10;
            Socket.MulticastLoopback = false;
#endif
        }

        /// <summary>
        /// Joins to specified multicast group
        /// </summary>
        /// <param name="group">Multicast group address</param>
        public void JoinMulticastGroup(IPAddress group)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} JoinMulticastGroup", GetHashCode()));
            System.Diagnostics.Trace.Flush();

            bool isIp6 = group.AddressFamily == AddressFamily.InterNetworkV6;

            SocketOptionLevel level = isIp6 ? SocketOptionLevel.IPv6 : SocketOptionLevel.IP;
            object option = null;

            if (isIp6)
            {
                option = new IPv6MulticastOption(group);
            }
            else
            {
                option = new MulticastOption(group, _endpoint.Address);
            }

            if (Socket != null)
            {
                lock (Socket)
                {
                    Socket.Client.SetSocketOption(level, 
                        SocketOptionName.AddMembership, 
                        option);
                }
            }
        }

        /// <summary>
        /// Start listening for incoming UDP packets
        /// </summary>
        public void Listen()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} Listen", GetHashCode()));
            System.Diagnostics.Trace.Flush();

            if (Socket != null)
            {
                lock (Socket)
                {
                    IAsyncResult result = Socket.BeginReceive(new AsyncCallback(ReceiveCallback), null);
                }
            }
        }

        /// <summary>
        /// Sends data to specified endpoint
        /// </summary>
        /// <param name="destination">Endpoint to send to</param>
        /// <param name="sendBytes">Data to send</param>
        public void Send(IPEndPoint destination, byte[] sendBytes)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} Send", GetHashCode()));
            System.Diagnostics.Trace.Flush();

            if (Socket != null)
            {
                lock (Socket)
                {
                    Socket.Send(sendBytes, sendBytes.Length, destination);
                }
            }
        }

        public void Send(IPEndPoint destination, List<byte[]> sendBytes)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} Send", GetHashCode()));
            System.Diagnostics.Trace.Flush();

            if (Socket != null)
            {
                lock (Socket)
                {
                    foreach (byte[] message in sendBytes)
                    {
                        Socket.Send(message, message.Length, destination);
                    }
                }
            }
        }

        /// <summary>
        /// Processes incoming UDP data
        /// </summary>
        /// <param name="result">Async result</param>
        private void ReceiveCallback(IAsyncResult result)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} ReceiveCallback", GetHashCode()));
            try
            {
                lock (Socket)
                {
                    if ((Socket != null) && (Socket.Client != null))
                    {
                        IPEndPoint messageSource = null;
                        System.Diagnostics.Trace.WriteLine(string.Format("{0} DiscoverySocket::ReceiveCallback entry point",
                            DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff")));
                        byte[] messageBytes = Socket.EndReceive(result, ref messageSource);

                        System.Diagnostics.Trace.WriteLine(string.Format("{0} DiscoverySocket::ReceiveCallback from[{1}] message[{2}]",
                            DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"), messageSource.ToString(), Encoding.UTF8.GetString(messageBytes)));

                        DiscoverySocketEventArgs args = new DiscoverySocketEventArgs();
                        args.Message = messageBytes;
                        args.Source = messageSource;
                        MessageReceived(this, args);
                        System.Diagnostics.Trace.WriteLine(string.Format("{0} DiscoverySocket::ReceiveCallback begin receive",
                            DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff")));
                        System.Diagnostics.Trace.Flush();
                        Socket.BeginReceive(ReceiveCallback, null);
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0} DiscoverySocket::ReceiveCallback error [{1}]",
                    DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"), e.Message));
                System.Diagnostics.Trace.Flush();
            }
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} Dispose", GetHashCode()));
            System.Diagnostics.Trace.Flush();

            Close();
        }

        /// <summary>
        /// Closes socket
        /// </summary>
        public void Close()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Socket {0} Close", GetHashCode()));
            System.Diagnostics.Trace.Flush();

            if (Socket != null)
            {
                lock (Socket)
                {
                    Socket.Close();
                }
            }
        }
    }
}
