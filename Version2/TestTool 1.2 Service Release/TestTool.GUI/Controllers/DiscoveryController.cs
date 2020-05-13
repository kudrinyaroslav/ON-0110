///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.Tests.Common.Discovery;
//using Proxy = TestTool.Proxies.Device;
using TestTool.GUI.Utils;
using WSD = TestTool.Proxies.WSDiscovery;
using System.Net.Sockets;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Controller for device discovery tab
    /// </summary>
    class DiscoveryController : Controller<IDiscoveryView>
    {
        /// <summary>
        /// Wrapper for web-service proxy to make service calls.
        /// </summary>
        private ManagementServiceProvider _client;

        /// <summary>
        /// Is raised when discovery starts.
        /// </summary>
        public event Action DiscoveryStarted;
        /// <summary>
        /// Is raised when discovery is completed.
        /// </summary>
        public event Action DiscoveryCompleted;
        /// <summary>
        /// Is raised when device is discovered.
        /// </summary>
        public event Action<DeviceDiscoveryData> DeviceDiscovered;
        /// <summary>
        /// Is raised when an error occurs.
        /// </summary>
        public event Action<Exception> DiscoveryError;
        /// <summary>
        /// Is raised when device information is received.
        /// </summary>
        public event ManagementServiceProvider.DeviceInformationReceived DeviceInformationReceived;

        /// <summary>
        /// Indicates that time-consuming operation is pending.
        /// </summary>
        public override bool RequestPending
        {
            get
            {
                return _requestPending;
            }
        }

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="view">Discovery view control</param>
        public DiscoveryController(IDiscoveryView view)
            :base(view)
        {
        }

        /// <summary>
        /// Indicates that time-consuming operation is pending.
        /// </summary>
        private bool _requestPending = false;

        /// <summary>
        /// Initializes device management service client
        /// </summary>
        /// <param name="address">Address of device management service</param>
        void InitializeClient(string address)
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            _client = new ManagementServiceProvider(address, env.Timeouts.Message);
            _client.ExceptionThrown += new Action<string, Exception>(OnExceptionThrown);
            _client.FaultThrown += new Action<string, FaultException>(OnFaultThrown);
            _client.OnDeviceInformationReceived += DeviceInformationReceived;
            _client.OperationCompleted += new Action(ReportOperationCompleted);
            _client.OperationStarted += new Action(ReportOperationStarted);
            _client.Timeout = env.Timeouts.Message;
        }
        
        /// <summary>
        /// Handles client fault thrown event
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="exc">Thrown fault exception</param>
        void OnFaultThrown(string stage, FaultException exc)
        {
            View.ShowError(exc);
            _requestPending = false;
            ReportOperationCompleted();
        }

        /// <summary>
        /// Handles client exception event
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="exc">Exception</param>
        void OnExceptionThrown(string stage, Exception exc)
        {
            View.ShowError(exc);
            _requestPending = false;
            ReportOperationCompleted();
        }

        /// <summary>
        /// Returns list of available Ethernet network interfaces
        /// </summary>
        /// <returns>List of network interfaces</returns>
        public List<NetworkInterfaceDescription> GetNetworkInterfaces()
        {
            List<NetworkInterfaceDescription> interfaces = new List<NetworkInterfaceDescription>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            bool hasIpv6 = false;
            bool hasIpv4 = false;
            foreach (NetworkInterface adapter in adapters)
            {
                if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) ||
                    (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    foreach (UnicastIPAddressInformation uinfo in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (uinfo.Address != null)
                        {
                            hasIpv6 |= uinfo.Address.AddressFamily == AddressFamily.InterNetworkV6;
                            hasIpv4 |= uinfo.Address.AddressFamily == AddressFamily.InterNetwork;
                        }
                        interfaces.Add(new NetworkInterfaceDescription(adapter, uinfo.Address));
                    }
                }
            }
            if (hasIpv4)
            {
                interfaces.Add(new NetworkInterfaceDescription("Any IPv4", IPAddress.Any));
            }
            if (hasIpv6)
            {
                interfaces.Add(new NetworkInterfaceDescription("Any IPv6", IPAddress.IPv6Any));
            }
            return interfaces;
        }

        /// <summary>
        /// Sends multicast probe message and starts listening for answers
        /// </summary>
        /// <param name="networkInterface">Network interface to bind socket to</param>
        public void RunDiscovery(IPAddress networkInterface)
        {
            ProbeInternal(networkInterface, IPAddress.None);
        }

        /// <summary>
        /// Sends unicast probe message and starts listening for answers
        /// </summary>
        /// <param name="address"></param>
        public void ProbeDevice(IPAddress address)
        {
            //do not bind to specific interface while unicast probing
            IPAddress bindAddress = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 ?
                IPAddress.IPv6Any : IPAddress.Any;
            ProbeInternal(bindAddress, address);
        }

        /// <summary>
        /// Sends probe message to specified address from specified address, and starts listening for answer
        /// </summary>
        /// <param name="local">Local address to send message from</param>
        /// <param name="remote">Address to send message to, if IPAddress.None multicast message will be sent</param>
        protected void ProbeInternal(IPAddress local, IPAddress remote)
        {
            Discovery discovery = new Discovery(local);
            discovery.Discovered += OnDeviceDiscovered;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            discovery.ReceiveError += OnDiscoveryError;
            if (remote != IPAddress.None)
            {
                discovery.Probe(remote);
            }
            else
            {   //probe multicast address
                discovery.Probe();
            }

            if (DiscoveryStarted != null)
            {
                DiscoveryStarted();
            }
        }

        /// <summary>
        /// Handles discovery error event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event parameter</param>
        protected void OnDiscoveryError(object sender, DiscoveryErrorEventArgs e)
        {
            if(DiscoveryError != null)
            {
                DiscoveryError(e.Exception);
            }
        }

        /// <summary>
        /// Handles device discovered event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event parameter</param>
        protected void OnDeviceDiscovered(object sender, DiscoveryMessageEventArgs e)
        {
            if(DeviceDiscovered != null)
            {
                List<DeviceDiscoveryData> devices = DiscoveryUtils.GetDevices(e.Message.ToSoapMessage<WSD.ProbeMatchesType>(), e.Sender);
                if(devices.Count == 0)
                {
                    OnDiscoveryError(this, new DiscoveryErrorEventArgs(new Exception("Device type is not " + DiscoveryUtils.ONVIF_DISCOVER_TYPES), null));
                }
                else
                {
                    foreach (DeviceDiscoveryData device in devices)
                    {
                        DeviceDiscovered(device);
                    }
                }
                
            }
        }

        /// <summary>
        /// Handles discovery finished event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event parameter</param>
        protected void OnDiscoveryFinished(object sender, EventArgs e)
        {
            if(DiscoveryCompleted != null)
            {
                DiscoveryCompleted();
            }
        }

        /// <summary>
        /// Starts get device information operation
        /// </summary>
        /// <param name="address">Address of device management service</param>
        public void GetDeviceInformation(string address)
        {
            UpdateCredentials();
            _requestPending = true;
            InitializeClient(address);
            _client.GetDeviceInformation();
        }

        /// <summary>
        /// Updates credentials data saved in context.
        /// </summary>
        void UpdateCredentials()
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            env.Credentials.Password = View.Password;
            env.Credentials.UserName = View.UserName;
        }
        
        /// <summary>
        /// Applies saved application context
        /// </summary>
        /// <param name="context">Saved context</param>
        public override void LoadSavedContext(SavedContext context)
        {
            if(context.DiscoveryContext != null)
            {
                IPAddress address;
                if(IPAddress.TryParse(context.DiscoveryContext.DeviceAddress, out address))
                {
                    View.DeviceAddress = address;
                }
                View.ServiceAddress = context.DiscoveryContext.ServiceAddress;
                View.NICAddress = context.DiscoveryContext.InterfaceAddress;
            }
            if (context.DeviceEnvironment != null)
            {
                if (context.DeviceEnvironment.Credentials != null)
                {
                    View.UserName = context.DeviceEnvironment.Credentials.UserName;
                    View.Password = context.DeviceEnvironment.Credentials.Password;
                }
            }

            UpdateContext();
        }

        /// <summary>
        /// Updates discovery tab control
        /// </summary>
        public override void UpdateView()
        {
            View.UpdateButtons();
        }

        /// <summary>
        /// Updates application context
        /// </summary>
        public override void UpdateContext()
        {
            DiscoveredDevices devices = new DiscoveredDevices();

            devices.NIC = View.NIC;
            devices.Current = new DeviceInfoFull();
            devices.Current.ByDiscovery = View.Current;
            devices.ServiceAddress = View.ServiceAddress;
            devices.DeviceAddress = View.DeviceAddress;

            foreach (DeviceDiscoveryData data in View.Devices)
            {
                DeviceInfoFull info = new DeviceInfoFull();
                info.ByDiscovery = data;
                devices.Discovered.Add(info);
            }
            ContextController.UpdateDiscoveredDevices(devices);

            UpdateCredentials();

            View.UpdateFormTitle();
        }

        /// <summary>
        /// Stops long operation
        /// </summary>
        public override void Stop()
        {
            if (_requestPending)
            {
                _client.Stop();
            }
        }
    
    }
}
