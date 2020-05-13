using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace DiscoveryClient
{
    internal class DiscoveryMessageEventArgs : EventArgs
    {
        public string Message { get; set; }
        public IPEndPoint Source { get; set; }
    }
    internal class DiscoverySocket : IDisposable
    {
        private IPEndPoint _local;

        public UdpClient Socket { get; protected set; }
        public object SocketSync { get; protected set; }
        public AutoResetEvent AnswerReceivedEvent { get; protected set; }
        public event EventHandler<DiscoveryMessageEventArgs> MessageReceived;

        public DiscoverySocket(IPEndPoint endpoint)
        {
            Socket = new UdpClient(endpoint);
            SocketSync = new object();
            AnswerReceivedEvent = new AutoResetEvent(false);
            Socket.Client.ReceiveBufferSize = 4 * 1024;
            Socket.Client.SendBufferSize = 4 * 1024;
            Socket.Ttl = 1;
            _local = endpoint;
        }
        public void JoinMulticastGroup(IPAddress group)
        {
            lock (SocketSync)
            {
                if (Socket != null)
                {
                    Socket.JoinMulticastGroup(group, _local.Address);
                }
            }

        }
        public void Listen()
        {
            lock (SocketSync)
            {
                if (Socket != null)
                {
                    Socket.BeginReceive(new AsyncCallback(ReceiveCallback), null);
                }
            }
        }
        public void Send(IPEndPoint destination, string message)
        {
            lock (SocketSync)
            {
                if (Socket != null)
                {
                    byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                    Socket.Send(sendBytes, sendBytes.Length, destination);
                }
            }
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            lock (SocketSync)
            {
                if ((Socket != null) && (Socket.Client != null))
                {
                    byte[] messageBytes;
                    IPEndPoint messageSource = null;
                    messageBytes = Socket.EndReceive(result, ref messageSource);
                    if (MessageReceived != null)
                    {
                        DiscoveryMessageEventArgs args = new DiscoveryMessageEventArgs();
                        args.Message = Encoding.ASCII.GetString(messageBytes);
                        args.Source = messageSource;
                        MessageReceived(this, args);
                    }
                    AnswerReceivedEvent.Set();
                    Socket.BeginReceive(ReceiveCallback, null);
                    //AnswerReceivedEvent.Reset();
                }
            }
        }
        public void Dispose()
        {
            Close();
        }
        public void Close()
        {
            lock (SocketSync)
            {
                if (Socket != null)
                {
                    Socket.Close();
                }
            }
        }
    }
}
