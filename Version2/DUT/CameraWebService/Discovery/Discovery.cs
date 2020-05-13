using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Xml;

namespace CameraWebService.Discovery
{
    public class Discovery : IDisposable
    {
        private IPAddress WS_DISCOVER_MULTICAST_IP4 = IPAddress.Parse("239.255.255.250");
        private const int WS_DISCOVER_PORT = 3702;

        private UdpClient _socket;
        private byte[] _buffer;
        private MessageUtils _messageUtils;
        private List<string> _scopes = new List<string>();
        private List<string> _scopes2 = new List<string>();
        private string _xAddr;
        private IPEndPoint _local;
        private bool scopesFlag = true;

        private IPAddress GetLocalIP()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipAddress in ipHostEntry.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }
            }
            return IPAddress.Any;
        }

        public Discovery(string xAddr)
        {
            _xAddr = xAddr;
            _local = new IPEndPoint(GetLocalIP(), WS_DISCOVER_PORT);

            _scopes.AddRange(new string[] {
                "onvif://www.onvif.org/Network_Video_Transmitter",
                "onvif://www.onvif.org/type/video_encoder",
                "onvif://www.onvif.org/name/DUT",
                "onvif://www.onvif.org/Profile/Q/FactoryDefault",
               //"onvif://www.onvif.org/Profile/Q/Operational",
                "onvif://www.onvif.org/type/ptz"});

            _scopes2.AddRange(new string[] {
                "onvif://www.onvif.org/Network_Video_Transmitter",
                "onvif://www.onvif.org/type/video_encoder",
                "onvif://www.onvif.org/name/DUT",
                //"onvif://www.onvif.org/Profile/Q/FactoryDefault",
               "onvif://www.onvif.org/Profile/Q/Operational",
                "onvif://www.onvif.org/type/ptz"});

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

        private void OnMessageReceived(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            System.Diagnostics.Debug.WriteLine("OnMessageReceived {0}", DateTime.Now.Millisecond);

            SocketFlags flags = new SocketFlags();
            IPPacketInformation ipInf = new IPPacketInformation();

            int packetLength = socket.EndReceiveMessageFrom(ar, ref flags, ref clientEndPoint, out ipInf);

            byte[] packet = new byte[packetLength];
            Array.Copy(_buffer, packet, packetLength);

            EndPoint newClientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            socket.BeginReceiveMessageFrom(
                _buffer, 0, _buffer.Length, SocketFlags.None, ref newClientEndPoint, OnMessageReceived, (object)socket);

            string messageId = string.Empty;
            if (_messageUtils.ParseProbe(packet, ref messageId))
            {
                if (scopesFlag)
                {
                    scopesFlag = !scopesFlag;
                    SendMessage(
                        _messageUtils.BuildProbeMatches(_scopes.ToArray(), false, new string[] { _xAddr }, messageId),
                       false, clientEndPoint);
                }
                else
                {
                    scopesFlag = !scopesFlag;
                    SendMessage(
                     _messageUtils.BuildProbeMatches(_scopes2.ToArray(), false, new string[] { _xAddr }, messageId),
                       false, clientEndPoint);
                }


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

        private void SendToLocal3702(byte[] msg)
        {
            // sending to all local IPv4 interfaces to make DUT emulator working
            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) ||
                        (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                    {
                        foreach (UnicastIPAddressInformation uinfo in adapter.GetIPProperties().UnicastAddresses)
                            if (uinfo.Address != null && uinfo.Address.AddressFamily == AddressFamily.InterNetwork)
                                sock.SendTo(msg, new IPEndPoint(uinfo.Address, 3702));
                    }
                }
            }
        }



        private void SendMessage(byte[] msg, bool multicast, EndPoint target)
        {
            UdpClient socketSend = new UdpClient(AddressFamily.InterNetwork);

            if (multicast)
            {
                socketSend.Client.SendTo(msg, target);  // sending directly to local interface to avoid local IP bug (doesn't get multicast messages)

                // preparing and sending multicast
                socketSend.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socketSend.Client.Bind(_local);
                socketSend.Client.SetSocketOption(
                    SocketOptionLevel.IP, SocketOptionName.MulticastInterface, _local.Address.GetAddressBytes());
                // все мулькаст-сообщения отправляем по адресу 239.255.255.250
                if (target is IPEndPoint)
                    ((IPEndPoint)target).Address = WS_DISCOVER_MULTICAST_IP4;

                socketSend.Client.SendTo(msg, target);
            }
            else
            {
                SendToLocal3702(msg);
                socketSend.Client.SendTo(msg, target);
            }
            socketSend.Close();
        }
    }
}
