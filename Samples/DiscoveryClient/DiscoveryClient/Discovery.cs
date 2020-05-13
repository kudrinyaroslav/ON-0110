using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using ONVIF_TestCases;
using RD = RemoteDiscovery;


namespace DiscoveryClient
{
    internal class DeviceDiscoveryEventArgs : EventArgs
    {
        public DeviceDiscovered Device { get; protected set; }

        public DeviceDiscoveryEventArgs(DeviceDiscovered device)
        {
            Device = device;
        }
    }
    internal class CloseConnectionParam 
    {
        public int Timeout { get; protected set; }
        public DiscoverySocket Socket { get; protected set; }
        public AutoResetEvent CloseEvent { get; set; }

        public CloseConnectionParam(DiscoverySocket socket, int timeout)
        {
            Socket = socket;
            Timeout = timeout;
        }
    }
    internal class Discovery
    {
        private IPAddress WS_DISCOVER_MULTICAST = IPAddress.Parse("239.255.255.250");
        private const int WS_DISCOVER_PORT = 3702;
        private const int WS_DISCOVER_MULTICAST_TIMEOUT = 2000;
        private const int WS_DISCOVER_UNICAST_TIMEOUT = 2000;

        private IPEndPoint endPointLocal;
        public event EventHandler<DeviceDiscoveryEventArgs> Discovered;
        public event EventHandler DiscoveryFinished;
       
        public Discovery(IPAddress local)
        {
            endPointLocal = new IPEndPoint(local, WS_DISCOVER_PORT);
        }

        public void Probe()
        {
            Probe(WS_DISCOVER_MULTICAST);
        }
        public void Probe(IPAddress address)
        {
            DiscoverySocket socket = new DiscoverySocket(endPointLocal);
            bool multicast = IsMulticast(address);
            if (multicast)
            {
                socket.JoinMulticastGroup(address);
            }
            string probe = BuildProbeMessage();
            IPEndPoint endpoint = new IPEndPoint(address, WS_DISCOVER_PORT);
            socket.Send(endpoint, probe);
            socket.MessageReceived += OnMessageReceived;
            socket.Listen();

            CloseConnectionParam task = new CloseConnectionParam(socket, WS_DISCOVER_MULTICAST_TIMEOUT);
            if(!multicast)
            {
                task.CloseEvent = socket.AnswerReceivedEvent;
            }
            ThreadPool.QueueUserWorkItem(CloseConnection, task);
        }
        protected void CloseConnection(object state)
        {
            CloseConnectionParam param = (CloseConnectionParam)state;
            if (param.CloseEvent == null)
            {
                Thread.Sleep(param.Timeout);
            }
            else
            {
                param.CloseEvent.WaitOne(param.Timeout);
            }
            param.Socket.Close();
            if(DiscoveryFinished != null)
            {
                DiscoveryFinished(this, EventArgs.Empty);
            }
        }
        protected DeviceDiscovered GetDevice(RD.ProbeMatchesType match, IPAddress from)
        {
            DeviceDiscovered res = null;
            if ((match.ProbeMatch != null) && (match.ProbeMatch.Length > 0) && (match.ProbeMatch[0].XAddrs != null))
            {
                // this is a problem since this is a required field
                // tell the user there was an error on the device but don't
                // add it to the list
                if (match.ProbeMatch[0].Types == "dn:NetworkVideoTransmitter")
                {
                    res = new DeviceDiscovered();
                    res.Type = match.ProbeMatch[0].Types;
                    res.Scopes = match.ProbeMatch[0].Scopes.Text[0];
                    res.IP = from.ToString();
                    res.ServiceAddress = match.ProbeMatch[0].XAddrs;
                    res.UUID = match.ProbeMatch[0].EndpointReference.Address.Value;
                    res.Metadata = match.ProbeMatch[0].MetadataVersion;
                }
            }
            return res;
        }
        protected void OnMessageReceived(object sender, DiscoveryMessageEventArgs e)
        {
            TestMessages TestMessage = new TestMessages();
            string message = e.Message;

            message = message.Replace('\n', ' ');
            message = message.Replace('\r', ' ');
            message = message.Replace('\t', ' ');
            message = message.Replace("                ", " ");
            message = message.Replace("        ", " ");
            message = message.Replace("    ", " ");
            message = message.Replace("  ", " ");
            message = message.Replace("> <", "><");
            message = message.Replace(">  <", "><");

            try
            {
                RD.ProbeMatchesType match = (RD.ProbeMatchesType)TestMessage.Parse_SoapMessage(message, typeof(RD.ProbeMatchesType));
                DeviceDiscovered device = GetDevice(match, e.Source.Address);
                if(device != null)
                {
                    DeviceDiscoveryEventArgs args = new DeviceDiscoveryEventArgs(device);
                    if(Discovered != null)
                    {
                        Discovered(this, args);
                    }
                }
            }
            catch
            {
            }

        }
        protected string BuildProbeMessage()
        {
            RemoteDiscovery.ScopesType Scope;
            TestMessages TestMessage = new TestMessages();
            Scope = new RemoteDiscovery.ScopesType();
            Scope.Text = new string[] { "" }; //"onvif://www.onvif.org/type/super" , " onvif://www.onvif.org/type/analytics ", " onvif://www.onvif.org/type/video", " onvif://www.onvif.org/name/Bosch", " onvif://www.onvif.org/location/city/Nuernberg", " onvif://www.onvif.org/hardware/Dinion-IP-NWC" };
            return TestMessage.Build_ProbeRequest("", Scope);
        }
        protected bool IsMulticast(IPAddress address)
        {
            Byte[] bytes = address.GetAddressBytes();
            // valid multicast is 224.0.0.0 to 239.255.255.255
            return (bytes[0] >= 224) && (bytes[0] <= 239);
        }

    }
}
