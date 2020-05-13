using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;
using TestTool.Tests.Common.Discovery;
using WSD = TestTool.Proxies.WSDiscovery;

namespace DiscoveryServer
{
    class DiscoverProbeEventArgs : EventArgs
    {
        public WSD.ProbeType Probe { get; set; }
        public object Response { get; set; }
        public IPEndPoint Sender { get; set; }
    }
    class DiscoveryServer : IDisposable
    {
        private const string _wsa = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
        private string _uuid = "uuid:7a973241-b089-4e33-9c6a-a3392ba04a06";
        private IPAddress WS_DISCOVER_MULTICAST_IP4 = IPAddress.Parse("239.255.255.250");
        private IPAddress WS_DISCOVER_MULTICAST_IP6 = IPAddress.Parse("FF02::C");
        private UdpClient _recvSocket;
        private UdpClient _sendSocket;
        private string[] _scopes = new string[] {
            "onvif://www.onvif.org/type/video_encoder ",
            "onvif://www.onvif.org/type/ptz ", 
            "onvif://www.onvif.org/type/audio_encoder ",
            "onvif://www.onvif.org/hardware/DUT ",
            "onvif://www.onvif.org/name/DUT ",
            "onvif://www.onvif.org/location/room"};

        protected string ServiceAddress { get; set; }
        public event EventHandler<DiscoverProbeEventArgs> OnProbeReceived;

        protected bool IPv6 { get; set; }

        public void Start(string address, string serviceAddress)
        {
            ServiceAddress = serviceAddress;
            IPAddress ip = IPAddress.Parse(address);
            IPv6 = ip.AddressFamily == AddressFamily.InterNetworkV6;

            IPEndPoint local = new IPEndPoint(ip, 3702);
            _recvSocket = new UdpClient(local);
            _recvSocket.JoinMulticastGroup(IPv6 ? WS_DISCOVER_MULTICAST_IP6 : WS_DISCOVER_MULTICAST_IP4);
            _recvSocket.BeginReceive(new AsyncCallback(OnReceived), null);
            _recvSocket.Client.ReceiveBufferSize = 4 * 1024;

            _sendSocket = new UdpClient();
            _sendSocket.Client.SendBufferSize = 4 * 1024;
            _sendSocket.Ttl = 10;

        }
        private void OnReceived(IAsyncResult result)
        {
            lock (_recvSocket)
            {
                if (_recvSocket.Client != null)
                {
                    IPEndPoint messageSource = null;
                    byte[] messageBytes = _recvSocket.EndReceive(result, ref messageSource);
                    OnDataReceived(messageBytes, messageSource);
                    _recvSocket.BeginReceive(OnReceived, null);
                }
            }
        }
        private string GetMessageId(List<XmlElement> header)
        {
            string id = string.Empty;
            foreach (XmlElement element in header)
            {
                if ((element.LocalName == "MessageID") && (element.NamespaceURI == _wsa))
                {
                    id = element.InnerText;
                    break;
                }
            }
            return id;
        }
        private void OnDataReceived(byte[] data, IPEndPoint sender)
        {
            try
            {
                SoapMessage<WSD.ProbeType> probe = SoapBuilder.ParseMessage<WSD.ProbeType>(data, null);
                if(OnProbeReceived != null)
                {
                    DiscoverProbeEventArgs args = new DiscoverProbeEventArgs();
                    args.Sender = sender;
                    args.Probe = probe.Object;
                    OnProbeReceived(this, args);
                    if(args.Response != null)
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("dn", DiscoveryUtils.ONVIF_NETWORK_WSDL_URL);
                        DiscoveryHeaderBuilder header = new DiscoveryHeaderBuilder();
                        header.OrigingMessageId = GetMessageId(probe.Header);
                        byte[] response = SoapBuilder.BuildMessage(args.Response, Encoding.UTF8, header, namespaces);
                        Send(response);
                    }
                }
            }
            catch
            {
            }
        }
        private void Send(byte[] data)
        {
            IPAddress address = IPv6 ? WS_DISCOVER_MULTICAST_IP6 : WS_DISCOVER_MULTICAST_IP4;
            _sendSocket.Send(data, data.Length, address.ToString(), 3702);
        }
        public void SendHello()
        {
            WSD.HelloType hello = new WSD.HelloType();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("dn", DiscoveryUtils.ONVIF_NETWORK_WSDL_URL);
            hello.EndpointReference = new WSD.EndpointReferenceType();
            hello.EndpointReference.Address = new WSD.AttributedURI();
            hello.EndpointReference.Address.Value = _uuid;
            hello.Types = "dn:NetworkVideoTransmitter";
            hello.Scopes = new WSD.ScopesType();
            hello.Scopes.Text = _scopes;
            hello.XAddrs = ServiceAddress;

            byte[] data = SoapBuilder.BuildMessage(hello, Encoding.UTF8, new DiscoveryHeaderBuilder(), namespaces);
            Send(data);
        }
        public void SendBye()
        {
            WSD.ByeType bye = new WSD.ByeType();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("dn", DiscoveryUtils.ONVIF_NETWORK_WSDL_URL);
            namespaces.Add("dn", DiscoveryUtils.ONVIF_NETWORK_WSDL_URL);
            bye.EndpointReference = new WSD.EndpointReferenceType();
            bye.EndpointReference.Address = new WSD.AttributedURI();
            bye.EndpointReference.Address.Value = _uuid;
            bye.Types = "dn:NetworkVideoTransmitter";
            bye.Scopes = new WSD.ScopesType();
            bye.Scopes.Text = _scopes;
            bye.XAddrs = ServiceAddress;

            byte[] data = SoapBuilder.BuildMessage(bye, Encoding.UTF8, new DiscoveryHeaderBuilder(), namespaces);
            Send(data);
        }
        public WSD.ProbeMatchesType BuildProbeMatches(WSD.ProbeType probe)
        {
            WSD.ProbeMatchType match = new WSD.ProbeMatchType();
            match.EndpointReference = new WSD.EndpointReferenceType();
            match.EndpointReference.Address = new WSD.AttributedURI();
            match.EndpointReference.Address.Value = _uuid;
            match.Types = "dn:NetworkVideoTransmitter";
            match.Scopes = new WSD.ScopesType();
            match.Scopes.Text = _scopes;
            match.XAddrs = ServiceAddress;

            WSD.ProbeMatchesType matches = new WSD.ProbeMatchesType();
            matches.ProbeMatch = new WSD.ProbeMatchType[] { match };
            return matches;
        }
        public void Shutdown()
        {
            lock (_recvSocket)
            {
                if (_recvSocket != null)
                {
                    _recvSocket.Close();
                }
            }
            lock (_sendSocket)
            {
                if (_sendSocket != null)
                {
                    _sendSocket.Close();
                }
            }
        }
        public void Dispose()
        {
            Shutdown();
        }
    }
}
