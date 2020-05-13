﻿///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Reflection;
using WSD = TestTool.Proxies.WSDiscovery;
using System.Xml.Serialization;

namespace TestTool.Tests.Common.Discovery
{
    public class CloseConnectionState
    {
        public int Timeout { get; set; }
    }
    public class Discovery : IDisposable
    {
//        private const string SCOPE_MATCH_BY_URL = "http://schemas.xmlsoap.org/ws/2005/04/discovery/rfc2396"; 
        private const string SCOPE_MATCH_BY_URL = "http://schemas.xmlsoap.org/ws/2005/04/discovery/rfc3986"; 
        private IPAddress WS_DISCOVER_MULTICAST_IP4 = IPAddress.Parse("239.255.255.250");
        private IPAddress WS_DISCOVER_MULTICAST_IP6 = IPAddress.Parse("FF02::C");
        private const int WS_DISCOVER_PORT = 3702;
        private const int WS_DISCOVER_TIMEOUT = 3000;

        private DiscoverySocket _socket;
        private object _socketSync = new object();
        private Thread _connectionThread;
        private IPAddress _listenAddress;
        private string _listenDevice;
        private string _listenMessage;
        private AutoResetEvent _stopListenEvent = new AutoResetEvent(false);

        private List<XmlSchema> _discoverySchemas;
        private IPEndPoint _endPointLocal;
        public event EventHandler<DiscoveryMessageEventArgs> Discovered;
        public event EventHandler<DiscoveryMessageEventArgs> HelloReceived;
        public event EventHandler<DiscoveryMessageEventArgs> ByeReceived;
        public event EventHandler<DiscoveryErrorEventArgs> SoapFaultReceived;
        public event EventHandler<DiscoveryErrorEventArgs> ReceiveError;
        public event EventHandler DiscoveryFinished;

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="local">Local binding address</param>
        public Discovery(IPAddress local)
        {
            _endPointLocal = new IPEndPoint(local, WS_DISCOVER_PORT);

            //read schemas from assembly
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TestTool.Tests.Common.Discovery.Schemas.ws-discovery.xsd");
            XmlSchema schemaDiscovery = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TestTool.Tests.Common.Discovery.Schemas.addressing.xsd");
            XmlSchema schemaAddressing = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            _discoverySchemas = new List<XmlSchema> { schemaDiscovery, schemaAddressing };
        }

        /// <summary>
        /// Returns proper multicast group address for device discovery, depending on local address family
        /// </summary>
        /// <returns></returns>
        protected IPAddress GetDiscoveryMulticastAddress()
        {
            return _endPointLocal.AddressFamily == AddressFamily.InterNetworkV6 ? WS_DISCOVER_MULTICAST_IP6 : WS_DISCOVER_MULTICAST_IP4;
        }

        /// <summary>
        /// Joins to device discovery multicast group
        /// </summary>
        /// <param name="socket"></param>
        protected void JoinDiscoveryMutlicastGroup(DiscoverySocket socket)
        {
            IPAddress multicastAddress = GetDiscoveryMulticastAddress();
            socket.JoinMulticastGroup(multicastAddress);
        }

        /// <summary>
        /// Sends multicast probe message and begins listening for answer
        /// </summary>
        public void Probe()
        {
            //TODO
            Probe(true, null, null, WS_DISCOVER_TIMEOUT, null/*DiscoveryUtils.GetManadatoryScopes()*/);
        }

        /// <summary>
        /// Sends multicast probe message and begins listening for answer from specified address
        /// </summary>
        /// <param name="address">Address to listen</param>
        public void Probe(IPAddress address)
        {
            Probe(false, address, null, WS_DISCOVER_TIMEOUT, null/*DiscoveryUtils.GetManadatoryScopes()*/);
        }

        /// <summary>
        /// Sends multicast or unicast probe message and begins listening for answer from specified address and device
        /// </summary>
        /// <param name="multicast">if true, multicast message will be sent</param>
        /// <param name="address">Address to listen</param>
        /// <param name="deviceId">Device to listen</param>
        public void Probe(bool multicast, IPAddress address, string deviceId)
        {
            Probe(multicast, address, deviceId, WS_DISCOVER_TIMEOUT, null/*DiscoveryUtils.GetManadatoryScopes()*/);
        }

        /// <summary>
        /// Sends multicast or unicast probe message and begins listening for answer from specified address and device
        /// </summary>
        /// <param name="multicast">if true, multicast message will be sent</param>
        /// <param name="address">Address to listen</param>
        /// <param name="deviceId">Device to listen</param>
        /// <param name="timeout">Time to listen</param>
        /// <param name="scopes">Scopes to probe</param>
        public void Probe(bool multicast, IPAddress address, string deviceId, int timeout, string[] scopes)
        {
            Probe(multicast, address, deviceId, timeout, scopes, null);
        }

        /// <summary>
        /// Sends multicast or unicast probe message and begins listening for answer from specified address and device
        /// </summary>
        /// <param name="multicast">if true, multicast message will be sent</param>
        /// <param name="address">Address to listen</param>
        /// <param name="deviceId">Device to listen</param>
        /// <param name="timeout">>Time to listen</param>
        /// <param name="scopes">Scopes to probe</param>
        /// <param name="matchRule">Scope matching rule</param>
        public void Probe(bool multicast, IPAddress address, string deviceId, int timeout, string[] scopes, string matchRule)
        {
            _socket = new DiscoverySocket(_endPointLocal);
            _socket.MessageReceived += OnMessageReceived<WSD.ProbeMatchesType>;

            string messageId = string.Empty;
            try
            {
                if (multicast)
                {
                    JoinDiscoveryMutlicastGroup(_socket);
                }
                byte[] message = BuildProbeMessage(scopes, matchRule);
                messageId = DiscoveryUtils.ExtractMessageId(message);

                IPEndPoint target = multicast ? new IPEndPoint(GetDiscoveryMulticastAddress(), WS_DISCOVER_PORT) :
                    new IPEndPoint(address, WS_DISCOVER_PORT);

                _socket.Send(target, message);

                StartListen(timeout, address, deviceId, messageId);
            }
            catch
            {
                _socket.Close();
                throw;
            }
        }

        /// <summary>
        /// Starts listen for incoming udp messages from specified address and device
        /// </summary>
        /// <param name="timeout">Time to listen</param>
        /// <param name="address">Address to listen</param>
        /// <param name="device">Device to listen</param>
        /// <param name="messageId">Id of message to wait answer for</param>
        protected void StartListen(int timeout, IPAddress address, string device, string messageId)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("{0} Discovery StartListen", DateTime.Now));
            System.Diagnostics.Trace.Flush();

            _listenAddress = address;
            _listenDevice = device;
            _listenMessage = messageId;
            _stopListenEvent.Reset();
            _socket.Listen();
            _connectionThread = new Thread(CloseConnection);
            
            System.Diagnostics.Trace.WriteLine(string.Format("{0} StartListen, timeout={1}", DateTime.Now.ToLongTimeString(), timeout));
            System.Diagnostics.Trace.Flush();
            _connectionThread.Start(new CloseConnectionState() { Timeout = timeout });
        }

        /// <summary>
        /// Closes socket after timeout or answer from proper device
        /// </summary>
        /// <param name="state">Socket listening timeout</param>
        protected void CloseConnection(object state)
        {
            int timeout = ((CloseConnectionState)state).Timeout;
            System.Diagnostics.Trace.WriteLine(string.Format("{0} CloseConnection - started, timeout={1}", DateTime.Now.ToLongTimeString(), timeout));
            System.Diagnostics.Trace.Flush();
            bool timeoutOccured = false;
            if ((_listenAddress == null) && (_listenDevice == null))
            {
                //listen all devices
                Thread.Sleep(timeout);
                timeoutOccured = true;
            }
            else
            {
                //listen specified device only
                timeoutOccured = !_stopListenEvent.WaitOne(timeout);
            }
            System.Diagnostics.Trace.WriteLine(string.Format("{0} CloseConnection - awaken {1}", DateTime.Now.ToLongTimeString(), timeoutOccured ? "after timeout" : "by event"));
            System.Diagnostics.Trace.Flush();
            lock (_socketSync)
            {
                if (_socket != null)
                {
                    //close socket
                    _socket.Close();
                    _socket = null;
                }
            }
            if (DiscoveryFinished != null)
            {
                DiscoveryFinished(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns event handler for specified message type
        /// </summary>
        /// <param name="messageType">Type of message</param>
        /// <returns>Event handler</returns>
        protected EventHandler<DiscoveryMessageEventArgs> GetHandler(Type messageType)
        {
            EventHandler<DiscoveryMessageEventArgs> res = null;
            if(typeof(WSD.ProbeMatchesType).GUID == messageType.GUID)
            {
                res = Discovered;
            }
            else if(typeof(WSD.HelloType).GUID  == messageType.GUID)
            {
                res = HelloReceived;
            }
            else if(typeof(WSD.ByeType).GUID == messageType.GUID)
            {
                res = ByeReceived;
            }
            return res;
        }
        /// <summary>
        /// Tests if incoming message header has proper RelatedTo element
        /// </summary>
        /// <param name="header">Incoming SOAP message header</param>
        /// <returns>true, if incoming message header has proper RelatedTo element</returns>
        protected bool IsExpectedMessageHeader(ICollection<XmlElement> header)
        {
            if (!string.IsNullOrEmpty(_listenMessage))
            {
                string relatedTo = DiscoveryUtils.ExtractRelatesTo(header);
                if (!DiscoveryUtils.CompareUUID(_listenMessage, relatedTo))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Tests if incoming satisfies listening params - message id, device id and address
        /// </summary>
        /// <typeparam name="T">Type of message object</typeparam>
        /// <param name="message">Incoming SOAP message</param>
        /// <returns>true, if incoming satisfies listening params</returns>
        protected bool IsExpectedMessage<T>(SoapMessage<T> message)
            where T : class
        {
            if (!IsExpectedMessageHeader(message.Header))
            {
                return false;
            }
            if(!string.IsNullOrEmpty(_listenDevice))
            {
                string deviceId = string.Empty;
                if (message.Object is WSD.ProbeMatchesType)
                {
                    deviceId = DiscoveryUtils.GetDeviceId(message.Object as WSD.ProbeMatchesType);
                }
                else if(message.Object is WSD.HelloType)
                {
                    deviceId = DiscoveryUtils.GetDeviceId(message.Object as WSD.HelloType);
                }
                else if (message.Object is WSD.ByeType)
                {
                    deviceId = DiscoveryUtils.GetDeviceId(message.Object as WSD.ByeType);
                }
                if(_listenDevice != deviceId)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Processed incoming UDP packet
        /// </summary>
        /// <typeparam name="T">Type of expected object</typeparam>
        /// <param name="sender">Discovery socket</param>
        /// <param name="e">Event arguments, containing endpoint address and incoming bytes</param>
        protected void OnMessageReceived<T>(object sender, DiscoverySocketEventArgs e)
            where T : class
        {
            if ((_listenAddress == null) || DiscoveryUtils.CompareAddresses(e.Source.Address, _listenAddress))
            {
                try
                {
                    //try to parse message according to expected type
                    SoapMessage<T> message = SoapBuilder.ParseMessage<T>(e.Message, _discoverySchemas);
                    if(IsExpectedMessage<T>(message))
                    {
                        EventHandler<DiscoveryMessageEventArgs> handler = GetHandler(message.Object.GetType());
                        DiscoveryMessageEventArgs args = new DiscoveryMessageEventArgs(
                            message.ToSoapMessage<object>(),
                            e.Source.Address);
                        if (handler != null)
                        {
                            handler(this, args);
                        }
                        _stopListenEvent.Set();
                    }
                }
                catch (SoapFaultException ex)
                {
                    if(ex.Message != null)
                    {
                        if (IsExpectedMessage<Fault>(ex.FaultMessage))
                        {
                            if (SoapFaultReceived != null)
                            {
                                SoapFaultReceived(this, new DiscoveryErrorEventArgs(ex, ex.Fault));
                            }
                            _stopListenEvent.Set();
                        }
                    }
                }
                catch (UnxpectedElementException ex)
                {
                    if(!string.IsNullOrEmpty(_listenMessage) && IsExpectedMessageHeader(ex.Headers))
                    {
                        //throw this  exception only is message contains proper RelatedTo, otherwise just ignore message
                        if (ReceiveError != null)
                        {
                            ReceiveError(this, new DiscoveryErrorEventArgs(ex, null));
                        }
                        _stopListenEvent.Set();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (ReceiveError != null)
                    {
                        ReceiveError(this, new DiscoveryErrorEventArgs(ex, null));
                    }
                    System.Diagnostics.Trace.WriteLine(string.Format("Discovery::OnMessageReceived error [{0}]",
                        ex.Message));
                    System.Diagnostics.Trace.Flush();
                }
            }
        }

        /// <summary>
        /// Build device discovery probe message
        /// </summary>
        /// <param name="scopes">Scopes to probe</param>
        /// <param name="matchRule">Scope matching rule</param>
        /// <returns>Probe message</returns>
        protected byte[] BuildProbeMessage(string[] scopes, string matchRule)
        {
            WSD.ProbeType probe = new WSD.ProbeType();
            //Scope.MatchBy = string.IsNullOrEmpty(matchRule) ? SCOPE_MATCH_BY_URL : matchRule;
            probe.Scopes = new WSD.ScopesType();
            probe.Scopes.MatchBy = matchRule;
            if (scopes != null)
            {
                string strScopes = string.Empty;
                for (int i = 0; i < scopes.Length; i++)
                {
                    strScopes += scopes[i];
                    if (i < (scopes.Length - 1))
                    {
                        strScopes += " ";
                    }
                }
                probe.Scopes.Text = new string[] { strScopes };
            }
            probe.Types = "dn:" + DiscoveryUtils.ONVIF_DISCOVER_TYPES;
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("dn", DiscoveryUtils.ONVIF_NETWORK_WSDL_URL);
            return SoapBuilder.BuildMessage(probe, Encoding.UTF8, new DiscoveryHeaderBuilder(), namespaces);
        }

        /// <summary>
        /// Test if specified address is multicast
        /// </summary>
        /// <param name="address">Address to test</param>
        /// <returns>true, if specified address is multicast</returns>
        protected bool IsMulticast(IPAddress address)
        {
            Byte[] bytes = address.GetAddressBytes();
            // valid multicast is 224.0.0.0 to 239.255.255.255
            return address.IsIPv6Multicast || ((bytes[0] >= 224) && (bytes[0] <= 239));
        }

        /// <summary>
        /// Waits for Hello message from specified address and device
        /// </summary>
        /// <param name="from">Address to listen</param>
        /// <param name="deviceId">Device to listen</param>
        /// <param name="timeout">Wait timeout</param>
        public void WaitHello(IPAddress from, string deviceId, int timeout)
        {
            WaitMessage(true, from, deviceId, timeout, new EventHandler<DiscoverySocketEventArgs>[] { OnMessageReceived<WSD.HelloType> });
        }

        /// <summary>
        /// Waits for Bye message from specified address and device
        /// </summary>
        /// <param name="from">Address to listen</param>
        /// <param name="deviceId">Device to listen</param>
        /// <param name="timeout">Wait timeout</param>
        public void WaitBye(IPAddress from, string deviceId, int timeout)
        {
            WaitMessage(true, from, deviceId, timeout, new EventHandler<DiscoverySocketEventArgs>[] { OnMessageReceived<WSD.ByeType> });
        }

        /// <summary>
        /// Waits for Bye or Hello message from specified address and device
        /// </summary>
        /// <param name="from">Address to listen</param>
        /// <param name="deviceId">Device to listen</param>
        /// <param name="timeout">Wait timeout</param>
        public void WaitByeOrHello(IPAddress from, string deviceId, int timeout)
        {
            WaitMessage(true, from, deviceId, timeout, new EventHandler<DiscoverySocketEventArgs>[] { OnMessageReceived<WSD.HelloType>, OnMessageReceived<WSD.ByeType> });
        }
        
        /// <summary>
        /// Waits for message from specified address and device and processes it with specified handler
        /// </summary>
        /// <param name="multicast">if true wait for multicast message</param>
        /// <param name="from">Address to listen</param>
        /// <param name="deviceId">Device to listen</param>
        /// <param name="timeout">Wait timeout</param>
        /// <param name="callbacks">Handler to process message</param>
        protected void WaitMessage(bool multicast, IPAddress from, string deviceId, int timeout, EventHandler<DiscoverySocketEventArgs>[] callbacks)
        {
            _socket = new DiscoverySocket(_endPointLocal);
            
            if(multicast)
            {
                JoinDiscoveryMutlicastGroup(_socket);
            }

            foreach (EventHandler<DiscoverySocketEventArgs> callback in callbacks)
            {
                _socket.MessageReceived += callback;
            }
            StartListen(timeout, from, deviceId, null);
        }

        /// <summary>
        /// Close active socket
        /// </summary>
        public void Close()
        {
            if (_connectionThread != null)
            {
                _connectionThread.Abort();
                _connectionThread.Join();
            }
            lock (_socketSync)
            {
                if (_socket != null)
                {
                    _socket.Close();
                }
            }
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}