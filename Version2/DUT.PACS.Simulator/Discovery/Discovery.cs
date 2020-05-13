using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Xml;

namespace DUT.PACS.Simulator.Discovery
{
    public class Discovery : IDisposable
    {
        private IPAddress WS_DISCOVER_MULTICAST_IP4 = IPAddress.Parse("239.255.255.250");
        private const int WS_DISCOVER_PORT = 3702;

        private UdpClient _socket;
        private byte[] _buffer;
        private MessageUtils _messageUtils;
        private List<string> _scopes = new List<string>();
        private List<string> _xAddr;
        private IPEndPoint _local;

        private IPAddress GetLocalIP()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipAddress in ipHostEntry.AddressList)
            {
                // skip IPv6 address
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    // skip Link-local address
                    if (!ipAddress.ToString().StartsWith("169.254"))
                    {
                        return ipAddress;
                    }
                }
            }
            return IPAddress.Any;
        }

        public Discovery(string xAddr)
        {
            _local = new IPEndPoint(GetLocalIP(), WS_DISCOVER_PORT);

            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());

            _xAddr = new List<string>();
            foreach (IPAddress ipAddress in ipHostEntry.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    _xAddr.Add(string.Format(xAddr, ipAddress.ToString()));
                }
                if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    _xAddr.Add(string.Format(xAddr, "[" + ipAddress.ToString().Replace("%" + ipAddress.ScopeId.ToString(), "") + "]"));
                }
            }

            System.IO.File.WriteAllText(Environment.GetEnvironmentVariable("TEMP") + @"\test.log", 
                                        "_xAddr: \n" + String.Join("\n", _xAddr.ToArray()) + "\n\n_local: " + _local + "\n");

            _scopes.AddRange(new string[] {
                "onvif://www.onvif.org/Profile/C",
                "onvif://www.onvif.org/name/Simulator",
                "onvif://www.onvif.org/hardware/PC",
                "onvif://www.onvif.org/location/scope1"});

            _messageUtils = new MessageUtils();

            _buffer = new byte[4 * 1024]; 

            _socket = new UdpClient();
            _socket.Client.ReceiveBufferSize = 48 * 1024;
            _socket.Client.SendBufferSize = 4 * 1024;

            _socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.Client.Bind(new IPEndPoint(IPAddress.Any, _local.Port));

            _socket.Client.SetSocketOption(
                SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(WS_DISCOVER_MULTICAST_IP4, _local.Address));

            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            _socket.Client.BeginReceiveMessageFrom(
                _buffer, 0, _buffer.Length, SocketFlags.None, ref clientEndPoint, 
                new AsyncCallback(OnMessageReceived), (object)_socket.Client); 
        }

        public void SetScopes(string[] scopes)
        {
            _scopes.Clear();
            _scopes.AddRange(scopes);
        }

        public List<String> Scopes
        {
            get { return _scopes; }
        }

        private void OnMessageReceived(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

                SocketFlags flags = new SocketFlags();
                IPPacketInformation ipInf = new IPPacketInformation();

                int packetLength = socket.EndReceiveMessageFrom(ar, ref flags, ref clientEndPoint, out ipInf);

                byte[] packet = new byte[packetLength];
                Array.Copy(_buffer, packet, packetLength);

                char[] packetChar = new char[packet.Length];
                Array.Copy(_buffer, packetChar, packet.Length);
                String packetStr = new String(packetChar);
                System.IO.File.AppendAllText(Environment.GetEnvironmentVariable("TEMP") + @"\test.log",
                                             "\n[" + DateTime.Now.ToString() + "]\nMessage: " + packetStr + "\n");

                string messageId = string.Empty;
                if (_messageUtils.ParseProbe(packet, ref messageId))
                {
                    System.IO.File.AppendAllText(Environment.GetEnvironmentVariable("TEMP") + @"\test.log", 
                                                 "Parsed Id: " + messageId + "\n");

                    SendMessage(
                        _messageUtils.BuildProbeMatches(_scopes.ToArray(), false, _xAddr.ToArray(), messageId), 
                        false, clientEndPoint);
                }
            }
            catch (Exception exc)
            {

            }
            finally
            {
                EndPoint newClientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                socket.BeginReceiveMessageFrom(
                    _buffer, 0, _buffer.Length, SocketFlags.None, ref newClientEndPoint, OnMessageReceived, (object)socket);

            }

        }

        public void Dispose()
        {
            _socket.Close();
        }

        public void SendHello(
            bool multicast, EndPoint target, 
            string[] scopes, string types, XmlQualifiedName[] typesNamespaces, string[] xAddrs, uint metadataVersion)
        {
            SendMessage(
                _messageUtils.BuildHello(scopes, types, typesNamespaces, xAddrs, metadataVersion), 
                multicast, target);
        }

        public void SendBye(
            bool multicast, EndPoint target, 
            string[] scopes, string types, XmlQualifiedName[] typesNamespaces, string[] xAddrs, uint metadataVersion)
        {
            SendMessage(
                _messageUtils.BuildBye(scopes, types, typesNamespaces, xAddrs, metadataVersion), 
                multicast, target);
        }

        private void SendMessage(byte[] msg, bool multicast, EndPoint target)
        {
            UdpClient socketSend = new UdpClient(AddressFamily.InterNetwork);
            socketSend.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            if (multicast)
            {
                socketSend.Client.Bind(_local);
                socketSend.Client.SetSocketOption(
                    SocketOptionLevel.IP, SocketOptionName.MulticastInterface, _local.Address.GetAddressBytes());
            }
            socketSend.Client.SendTo(msg, target);
            socketSend.Close();
        }
    }
}
