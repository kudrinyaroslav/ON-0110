using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.CommonUtils.XmlTransformation;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.BaseOnvifService;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.OnvifServices
{
    [OnvifService(InitializationPriority = OnvifServiceInitializationPriority.DeviceService)]
    public interface IDeviceService: IBaseOnvifService2<Device, DeviceClient>
    {}

    public static class DeviceServiceExtensions
    {
        #region Initialization utils

        private static void InitializeGuard(this IDeviceService s)
        {
            if (null == s.ServiceClient.Port)
            {
                s.Test.Assert(false,
                              "Can't connect to Device Service",
                              "Check that Device Service is accessible");
            }
        }

        public static string GetDeviceServiceAddress(this IDeviceService s, FeaturesList featureList)
        {
            return s.Test.CameraAddress;
        }

        #endregion

        #region Service Commands

        public static DiscoveryMode GetDiscoveryMode(this IDeviceService s)
        {
            s.InitializeGuard();

            var mode = DiscoveryMode.Discoverable;
            s.Test.RunStep(() => mode = s.ServiceClient.Port.GetDiscoveryMode(), "Get Discovery Mode");
            s.Test.DoRequestDelay();
            return mode;
        }

        public static void SetDiscoveryMode(this IDeviceService s, DiscoveryMode mode)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetDiscoveryMode(mode), "Set Discovery Mode");
            s.Test.DoRequestDelay();
        }

        public static string GetWsdlUrl(this IDeviceService s)
        {
            s.InitializeGuard();

            string wsdlUrl = string.Empty;
            s.Test.RunStep(() => wsdlUrl = s.ServiceClient.Port.GetWsdlUrl(), "Get WSDL URL");
            s.Test.DoRequestDelay();
            return wsdlUrl;
        }

        public static Scope[] GetScopes(this IDeviceService s)
        {
            s.InitializeGuard();

            Scope[] r = null;
            s.Test.RunStep(() => r = s.ServiceClient.Port.GetScopes(), "Get Scopes");

            return r ?? new Scope[0];
        }

        public static void SetScopes(this IDeviceService s, IEnumerable<string> scopes)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetScopes(scopes.ToArray()), "Set Scopes");
        }

        public static Capabilities GetCapabilities(this IDeviceService s, CapabilityCategory[] categories, string stepName)
        {
            s.InitializeGuard();

            Capabilities capabilities = null;
            s.Test.RunStep(() => capabilities = s.ServiceClient.Port.GetCapabilities(categories), stepName);
            s.Test.DoRequestDelay();
            return capabilities;
        }

        public static Capabilities GetCapabilities(this IDeviceService s, CapabilityCategory[] categories)
        {
            s.InitializeGuard();

            return s.GetCapabilities(categories, "Get capabilities");
        }

        public static HostnameInformation GetHostname(this IDeviceService s)
        {
            s.InitializeGuard();

            HostnameInformation hostname = null;
            s.Test.RunStep(() => hostname = s.ServiceClient.Port.GetHostname(), "Get Hostname");
            s.Test.DoRequestDelay();
            return hostname;
        }

        public static void SetHostname(this IDeviceService s, string name, string stepName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetHostname(name), stepName);
            s.Test.DoRequestDelay();
        }

        public static void SetHostname(this IDeviceService s, string name)
        {
            s.InitializeGuard();

            s.SetHostname(name, "Set Hostname");
        }

        public static DNSInformation GetDnsConfiguration(this IDeviceService s)
        {
            s.InitializeGuard();

            DNSInformation dnsInformation = null;
            s.Test.RunStep(() => dnsInformation = s.ServiceClient.Port.GetDNS(), "Get DNS configuration");
            s.Test.DoRequestDelay();
            return dnsInformation;
        }

        public static void SetDnsConfiguration(this IDeviceService s, DNSInformation information)
        {
            s.InitializeGuard();

            s.SetDnsConfiguration(information, "Set DNS configuration"); 
        }

        public static void SetDnsConfiguration(this IDeviceService s, DNSInformation information, string stepName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetDNS(information.FromDHCP, information.SearchDomain, information.DNSManual), stepName);
            s.Test.DoRequestDelay();
        }

        public static NTPInformation GetNTP(this IDeviceService s)
        {
            s.InitializeGuard();

            NTPInformation ntpInformation = null;
            s.Test.RunStep(()=> ntpInformation =  s.ServiceClient.Port.GetNTP(), "Get NTP information");
            s.Test.DoRequestDelay();
            return ntpInformation;
        }

        public static void SetNTP(this IDeviceService s, NTPInformation ntpInformation, string stepName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetNTP(ntpInformation.FromDHCP, ntpInformation.NTPManual), stepName);
            s.Test.DoRequestDelay();
        }

        public static void SetNTP(this IDeviceService s, NTPInformation ntpInformation)
        {
            s.InitializeGuard();

            s.SetNTP(ntpInformation, "Set NTP configuration");
        }

        public static SystemDateTime GetSystemDateAndTime(this IDeviceService s)
        {
            s.InitializeGuard();

            SystemDateTime dateTime = null;
            s.Test.RunStep(() => dateTime = s.ServiceClient.Port.GetSystemDateAndTime(), "Get system date and time");
            s.Test.DoRequestDelay();
            return dateTime;
        }

        public static void SetSystemDateAndTime(this IDeviceService s, SystemDateTime dateTime)
        {
            s.InitializeGuard();

            s.SetSystemDateAndTime(dateTime, "Set system date and time");
        }

        public static void SetSystemDateAndTime(this IDeviceService s, SystemDateTime dateTime, string stepName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetSystemDateAndTime(dateTime.DateTimeType, 
                                                             dateTime.DaylightSavings, 
                                                             dateTime.TimeZone, 
                                                             dateTime.UTCDateTime), stepName);
            s.Test.DoRequestDelay();
        }

        public static string GetDeviceInformation(this IDeviceService s, out string model, 
                                                                         out string firmwareVersion, 
                                                                         out string serialNumber, 
                                                                         out string hardwareId)
        {
            s.InitializeGuard();

            string information = null;

            // Cannot use ref or out parameter inside an anonymous method, lambda expression, or query expression
            string modelCopy = null;
            string firmwareVersionCopy = null;
            string serialNumberCopy = null;
            string hardwareIdCopy = null;
            
            s.Test.RunStep(() => information = s.ServiceClient.Port.GetDeviceInformation(out modelCopy, out firmwareVersionCopy, out serialNumberCopy, out hardwareIdCopy), 
                           "Get device information");
            
            s.Test.DoRequestDelay();

            model = modelCopy;
            firmwareVersion = firmwareVersionCopy;
            serialNumber = serialNumberCopy;
            hardwareId = hardwareIdCopy;

            return information;
        }


        public static void SetSystemFactoryDefault(this IDeviceService s, FactoryDefaultType type)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetSystemFactoryDefault(type), "Set System Factory Default");
            s.Test.DoRequestDelay();
        }

        public static string SystemReboot(this IDeviceService s)
        {
            s.InitializeGuard();

            string message = null;
            s.Test.RunStep(()=> 
                           {            
                               message = s.ServiceClient.Port.SystemReboot();
                               s.Test.LogStepEvent(string.Format("Response Message received: '{0}'", message));
                           },  
                           "Send System Reboot message");
            s.Test.DoRequestDelay();
            return message;
        }
    
        public static NetworkInterface[] GetNetworkInterfaces(this IDeviceService s)
        {
            s.InitializeGuard();

            NetworkInterface[] interfaces = null; 
            s.Test.RunStep(() => interfaces = s.ServiceClient.Port.GetNetworkInterfaces(), "Get network interfaces");
            s.Test.DoRequestDelay();
            return interfaces;
        }

        public static NetworkInterface[] GetNetworkInterfaces(this IDeviceService s, BaseTest test)
        {
            s.InitializeGuard();

            NetworkInterface[] interfaces = null;
            test.RunStep(() => interfaces = s.ServiceClient.Port.GetNetworkInterfaces(), "Get Network Interfaces");
            s.Test.DoRequestDelay();
            return interfaces;
        }

        public static bool SetNetworkInterface(this IDeviceService s, string interfaceToken, NetworkInterfaceSetConfiguration configuration)
        {
            bool result = false;
            s.Test.RunStep(() => result = s.ServiceClient.Port.SetNetworkInterfaces(interfaceToken, configuration), "Set Network Interfaces");
            s.Test.DoRequestDelay();
            return result;
        }

        public static NetworkZeroConfiguration GetZeroConfiguration(this IDeviceService s)
        {
            s.InitializeGuard();

            NetworkZeroConfiguration zero = null;
            s.Test.RunStep(() => zero = s.ServiceClient.Port.GetZeroConfiguration(), "Get Network Zero configuration");
            s.Test.DoRequestDelay();
            return zero;
        }

        public static void SetZeroConfiguration(this IDeviceService s, string interfaceToken, bool enabled)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetZeroConfiguration(interfaceToken, enabled), "Set Network Zero configuration");
            s.Test.DoRequestDelay();
        }

        public static NetworkProtocol[] GetNetworkProtocols(this IDeviceService s)
        {
            s.InitializeGuard();

            NetworkProtocol[] protocols = null;
            s.Test.RunStep(() => protocols = s.ServiceClient.Port.GetNetworkProtocols(), "Get Network Protocols");
            s.Test.DoRequestDelay();

            s.Test.Assert(protocols != null,
                          "The DUT did not send Network Protocols",
                          "Check if network protocols returned from the DUT");

            return protocols;
        }

        public static void SetNetworkProtocols(this IDeviceService s, NetworkProtocol[] protocols)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetNetworkProtocols(protocols), "Set Network Protocols");
            s.Test.DoRequestDelay();
        }

        public static NetworkGateway GetNetworkDefaultGateway(this IDeviceService s)
        {
            s.InitializeGuard();

            NetworkGateway result = null;
            s.Test.RunStep(() => result = s.ServiceClient.Port.GetNetworkDefaultGateway(), "Get Network Default Gateway");
            s.Test.DoRequestDelay();
            return result;
        }

        public static void SetNetworkDefaultGateway(this IDeviceService s, string[] ipv4Adddresses, string[] ipv6Addresses)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetNetworkDefaultGateway(ipv4Adddresses, ipv6Addresses), "Set Network Default Gateway");
            s.Test.DoRequestDelay();
        }

        public static User[] GetUsers(this IDeviceService s)
        {
            s.InitializeGuard();

            User[] response = null;
            s.Test.RunStep(() => response = s.ServiceClient.Port.GetUsers(), "Get Users");
            s.Test.DoRequestDelay();
            return response ?? new User[0];
        }

        public static void CreateUsers(this IDeviceService s, User[] users)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.CreateUsers(users), "Create users");
            s.Test.DoRequestDelay();
        }

        public static BinaryData GetAccessPolicy(this IDeviceService s)
        {
            s.InitializeGuard();

            BinaryData r = null;
            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessPolicy(), "Get Access Policy");
            s.Test.DoRequestDelay();

            return r;
        }

        public static void CreateUsers(this IDeviceService s, User[] users, string stepName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.CreateUsers(users), stepName);
            s.Test.DoRequestDelay();
        }

        public static void DeleteUsers(this IDeviceService s, string[] users)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteUsers(users), "Delete users");
            s.Test.DoRequestDelay();
        }

        public static void SetUser(this IDeviceService s, User[] users)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetUser(users), "Set users");
            s.Test.DoRequestDelay();
        }

        public static RelayOutput[] GetRelayOutputs(this IDeviceService s)
        {
            return s.GetRelayOutputs("Get relay outputs");
        }

        public static RelayOutput[] GetRelayOutputs(this IDeviceService s, string stepName)
        {
            s.InitializeGuard();

            RelayOutput[] outputs = null;
            s.Test.RunStep(() => outputs = s.ServiceClient.Port.GetRelayOutputs(), stepName);
            s.Test.DoRequestDelay();
            return outputs;
        }

        public static void SetRelayOutputSettings(this IDeviceService s, string token, RelayOutputSettings settings)
        {
            s.InitializeGuard();

            s.SetRelayOutputSettings(token, settings,
                                     string.Format("Set relay output settings (IdleState = {0}, Mode = {1})", settings.IdleState, settings.Mode));
        }

        public static void SetRelayOutputSettings(this IDeviceService s, string token, RelayOutputSettings settings, string stepName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetRelayOutputSettings(token, settings), stepName);
            s.Test.DoRequestDelay();
        }

        public static void SetRelayOutputState(this IDeviceService s, string token, RelayLogicalState state)
        {
            s.InitializeGuard();

            s.SetRelayOutputState(token, state, "Set relay output state");
        }

        public static void SetRelayOutputState(this IDeviceService s, string token, RelayLogicalState state, string stepName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetRelayOutputState(token, state), stepName);
            s.Test.DoRequestDelay();
        }

        public static SystemLog GetSystemLog(this IDeviceService s, SystemLogType logType, string stepName)
        {
            s.InitializeGuard();

            SystemLog log = null;
            s.Test.RunStep(() => log = s.ServiceClient.Port.GetSystemLog(logType), stepName);
            s.Test.DoRequestDelay();
            return log;
        }

        public static SystemLog GetSystemLog(this IDeviceService s, SystemLogType logType, string stepName, string allowedFault)
        {
            s.InitializeGuard();

            SystemLog log = null;
            s.Test.RunStepAllowFault(() => log = s.ServiceClient.Port.GetSystemLog(logType), stepName, allowedFault);
            s.Test.DoRequestDelay();
            return log;
        }

        private static Tuple< bool, Service[] > receivedEarlierGetServices = null;
        
        public static void InitializeService(this IDeviceService s)
        {
            s.ResetService();
            s.Test.NetworkSettingsChangedEvent += address => s.ResetService();
        }

        public static void ResetService(this IDeviceService s)
        {
            receivedEarlierGetServices = null;
        }

        public static Service[] GetServices(this IDeviceService s, bool includeCapabilities, bool useReceivedEarlier = false)
        {
            s.InitializeGuard();

            if (null == receivedEarlierGetServices)
            {
                receivedEarlierGetServices = new Tuple<bool, Service[]>(includeCapabilities, 
                                                                        CommonMethodsProvider.GetServices(s.Test, s.ServiceClient.Port, includeCapabilities) ?? new Service[0]);

                return receivedEarlierGetServices.Item2;
            }

            if (useReceivedEarlier && 
                //In current request 'includeCapabilities' is the same as in previous request 
                //or in previous request 'includeCapabilities' was true.
                (includeCapabilities == receivedEarlierGetServices.Item1 || receivedEarlierGetServices.Item1))
                return receivedEarlierGetServices.Item2;

            receivedEarlierGetServices = new Tuple<bool, Service[]>(includeCapabilities,
                                                                    CommonMethodsProvider.GetServices(s.Test, s.ServiceClient.Port, includeCapabilities) ?? new Service[0]);

            return receivedEarlierGetServices.Item2;
        }
        
        public static DeviceServiceCapabilities GetServiceCapabilities(this IDeviceService s)
        {
            s.InitializeGuard();

            DeviceServiceCapabilities capabilities = null;
            s.Test.RunStep(() => capabilities = s.ServiceClient.Port.GetServiceCapabilities(), "Get service capabilities");
            s.Test.DoRequestDelay();
            return capabilities;
        }

        #endregion

        #region Utils

        public static string GetServiceAddress(this IDeviceService s, string serviceNs)
        {
            string address = string.Empty;

            Service[] services = s.GetServices(false, true);
            Service service = services.FindService(serviceNs);
            if (service != null)
            {
                address = service.XAddr;
            }

            return address;
        }
        
        public static string GetEventServiceAddress(this IDeviceService s)
        {
            return s.GetServiceAddress(OnvifService.EVENTS);
        }

        public static string GetEventServiceAddress(this IDeviceService s, FeaturesList featureList)
        {
            return s.GetEventServiceAddress();
        }

        #endregion

        #region Discovery utils

        /// <summary>
        /// Receives Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="afterStartAction">additional afterStartAction after begin waiting</param>
        /// <returns>Hello message</returns>
        public static SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(this IDeviceService s, 
                                                                                     bool checkIP, bool checkDeviceId, Action afterStartAction, Discovery.MessageFilterFunction messageFilter)
        {
            return s.ReceiveHelloMessage(checkIP, checkDeviceId, afterStartAction, messageFilter, s.Test.RebootTimeout + s.Test.MessageTimeout);
        }

        public static SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(this IDeviceService s, 
                                                                                     bool checkIP, bool checkDeviceId, Action afterStartAction)
        {
            return s.ReceiveHelloMessage(checkIP, checkDeviceId, afterStartAction, null, s.Test.RebootTimeout + s.Test.MessageTimeout);
        }

        public static SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(this IDeviceService s, 
                                                                                     bool checkIP, 
                                                                                     bool checkDeviceId,
                                                                                     Action afterStartAction,
                                                                                     int timeout)
        {
            return s.ReceiveHelloMessage(checkIP, checkDeviceId, afterStartAction, null, timeout);
        }

        /// <summary>
        /// Receives Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="afterStartAction">additional afterStartAction after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Hello message</returns>
        public static SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(this IDeviceService s, 
                                                                                     bool checkIP, 
                                                                                     bool checkDeviceId,
                                                                                     Action afterStartAction, 
                                                                                     Discovery.MessageFilterFunction messageFilter, 
                                                                                     int timeout)
        {
            var discovery = new DiscoveryInternal(s);

            return discovery.ReceiveHelloMessage(checkIP, checkDeviceId, afterStartAction, messageFilter, timeout);
        }

        public static string FirstNonLinkLocalIPv4AddressFromClientSubnetwork(this IDeviceService s, IEnumerable<string> addresses)
        {
            foreach (var address in addresses)
            {
                var IPs = Dns.GetHostAddresses(new Uri(address).Host);
                foreach (var ipAddress in IPs)
                {
                    if (AddressFamily.InterNetwork == ipAddress.AddressFamily && !ipAddress.IsIPv4LinkLocal() && s.Test.Nic.IP.IsInSameSubnet(ipAddress, s.Test.Nic.IPv4Mask))
                        return address;
                }
            }

            return string.Empty;
        }

        public static bool HasNonLinkLocalIPv4Address(this IDeviceService s, SoapMessage<HelloType> helloMsg)
        {
            if (null == helloMsg || null == helloMsg.Object)
                return false;

            try
            {
                var addresses = null == helloMsg.Object.XAddrs ? new string[0] : helloMsg.Object.XAddrs.Split(' ');
                if (s.Test.Nic.IP.AddressFamily == AddressFamily.InterNetwork)
                    //IPv4: return first non-link local interface from the same subnet as ODTT client
                    return !string.IsNullOrEmpty(s.FirstNonLinkLocalIPv4AddressFromClientSubnetwork(addresses));

                return false;
            }
            catch (Exception)
            { return false; }
        }

        public static SoapMessage<HelloType> ReceiveHelloWithNonLinkLocalIPv4AddressFromClientSubnetwork(this IDeviceService s)
        {
            return s.ReceiveHelloMessage(false, true, () => {}, msg => s.HasNonLinkLocalIPv4Address(msg.ToSoapMessage<HelloType>()));
        }

        /// <summary>
        /// Receives Bye message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional afterStartAction after begin waiting</param>
        /// <returns>Bye message</returns>
        public static SoapMessage<Proxies.WSDiscovery.ByeType> ReceiveByeMessage(this IDeviceService s, bool checkIP, bool checkDeviceId, Action action)
        {
            return s.ReceiveByeMessage(checkIP, checkDeviceId, action, s.Test.RebootTimeout + s.Test.MessageTimeout);
        }

        /// <summary>
        /// Receives Bye message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional afterStartAction after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Bye message</returns>
        public static SoapMessage<Proxies.WSDiscovery.ByeType> ReceiveByeMessage(this IDeviceService s, bool checkIP, bool checkDeviceId, Action afterStartAction, int timeout)
        {
            var discovery = new DiscoveryInternal(s);

            return discovery.ReceiveByeMessage(checkIP, checkDeviceId, afterStartAction, timeout);
        }

        /// <summary>
        /// Receives Bye or Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional afterStartAction after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Bye message</returns>
        public static SoapMessage<Proxies.WSDiscovery.ByeType> ReceiveByeOrHelloMessage(this IDeviceService s, 
                                                                                        bool checkIP, bool checkDeviceId, Action afterStartAction, Action<SoapMessage<object>> validationAction)
        {
            var discovery = new DiscoveryInternal(s);

            return discovery.ReceiveByeOrHelloMessage(checkIP, checkDeviceId, afterStartAction, validationAction);
        }


        /// <summary>
        /// Sends probes message and waits for answer from device
        /// </summary>
        /// <param name="multicast">if true, message will be sent to multicast group address</param>
        /// <param name="scopes">Scope to be probed</param>
        /// <param name="matchRule">Scope matching rule</param>
        /// <returns>ProbeMatches message from device</returns>
        public static SoapMessage<Proxies.WSDiscovery.ProbeMatchesType> ProbeDevice(this IDeviceService s,
                                                                                    bool multicast,
                                                                                    DiscoveryUtils.DiscoveryType[][] types,
                                                                                    string[] scopes,
                                                                                    string matchRule)
        {
            return s.ProbeDevice(multicast, types, scopes, matchRule, null);
        }

        public static SoapMessage<Proxies.WSDiscovery.ProbeMatchesType> ProbeDevice(this IDeviceService s,
                                                                                    bool multicast, 
                                                                                    DiscoveryUtils.DiscoveryType[][] types,
                                                                                    string[] scopes, 
                                                                                    string matchRule,
                                                                                    Discovery.ProcessMessage processMessageMethod) 
        {
            var discovery = new DiscoveryInternal(s);

            return discovery.ProbeDevice(multicast, types, scopes, matchRule);
        }

        #endregion
    }

    #region Discovery implementation

    internal class DiscoveryInternal
    {
        public enum WaitMessageType
        {
            Hello = 0x01,
            Bye = 0x02
        }

        private IDeviceService DeviceService { get; set; }
        private BaseOnvifTest Test { get { return DeviceService.Test; } }

        private SoapMessage<object> _message;
        private Fault _soapFault;
        private Exception _error;

        private AutoResetEvent _eventHelloReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventByeReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventProbeMatchReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventTimeout = new AutoResetEvent(false);
        private AutoResetEvent _eventFaultReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventDiscoveryError = new AutoResetEvent(false);

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="param">Test parameters</param>
        public DiscoveryInternal(IDeviceService deviceClient)
        {
            DeviceService = deviceClient;
        }

        /// <summary>
        /// Handles incoming discovery Hello message
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void OnHelloReceived(object sender, DiscoveryMessageEventArgs e)
        {
            _message = e.Message.Clone() as SoapMessage<object>;
            _eventHelloReceived.Set();
        }

        /// <summary>
        /// Handles incoming discovery Bye message
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void OnByeReceived(object sender, DiscoveryMessageEventArgs e)
        {
            _message = e.Message.Clone() as SoapMessage<object>;
            _eventByeReceived.Set();
        }

        /// <summary>
        /// Handles discovery finished event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void OnDiscoveryFinished(object sender, EventArgs e)
        {
            _eventTimeout.Set();
        }

        /// <summary>
        /// Handles discovery error
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void OnDiscoveryError(object sender, DiscoveryErrorEventArgs e)
        {
            _error = e.Exception;
            _eventDiscoveryError.Set();
        }

        /// <summary>
        /// Handles discovery soap fault
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void OnSoapFault(object sender, DiscoveryErrorEventArgs e)
        {
            _soapFault = e.Fault;
            _eventFaultReceived.Set();
        }

        /// <summary>
        /// Validates incoming Hello message
        /// </summary>
        /// <param name="message">Hello message</param>
        /// <param name="reason">reason why message is invalid, null if message is valid</param>
        /// <returns>true, if message is valid</returns>
        public bool ValidateHelloMessage(SoapMessage<Proxies.WSDiscovery.HelloType> message, string[] scopes, out string reason)
        {
            bool mode1 = Test.Features.Contains(Feature.DiscoveryTypesDnNetworkVideoTransmitter);
            bool mode2 = Test.Features.Contains(Feature.DiscoveryTypesTdsDevice);
            bool res = true;
            reason = null;
            try
            {
                //check Types namespace

                Proxies.WSDiscovery.HelloType hello = message.Object;
                if (hello.Types == null)
                {
                    reason = Resources.ErrorNoTypes_Text;
                    return false;
                }
                if (!DiscoveryUtils.CheckDeviceHelloType(message, out reason, mode1, mode2))
                {
                    return false;
                }
                if (hello.EndpointReference == null)
                {
                    reason = Resources.ErrorNoEndpointReference_Text;
                    return false;
                }
                if (hello.Scopes == null)
                {
                    reason = Resources.ErrorNoScopes_Text;
                    return false;
                }
                if(hello.Scopes.Text == null)
                {
                    reason = Resources.ErrorNoScopesText_Text;
                    return false;
                }
                //check mandatory scopes 
                string missingScope = DiscoveryUtils.GetMissingMandatoryScope(hello.Scopes.Text);
                if (!string.IsNullOrEmpty(missingScope))
                {
                    reason = string.Format(Resources.ErrorMissingMandatoryScope_Format, missingScope);
                    return false;
                }
                //check optional scopes
                if(scopes != null)
                {
                    missingScope = DiscoveryUtils.GetMissingScope(hello.Scopes.Text, scopes);
                    if (!string.IsNullOrEmpty(missingScope))
                    {
                        reason = string.Format(Resources.ErrorMissingScope_Format, missingScope);
                        return false;
                    }
                }
            }
            catch(Exception e)
            {
                reason = e.Message;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Receives Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="afterStartAction">additional afterStartAction after begin waiting</param>
        /// <returns>Hello message</returns>
        public SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(bool checkIP, bool checkDeviceId, Action afterStartAction, Discovery.MessageFilterFunction messageFilter)
        {
            return ReceiveHelloMessage(checkIP, checkDeviceId, afterStartAction, messageFilter, Test.RebootTimeout + Test.MessageTimeout);
        }

        public SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(bool checkIP, bool checkDeviceId, Action afterStartAction)
        {
            return ReceiveHelloMessage(checkIP, checkDeviceId, afterStartAction, null, Test.RebootTimeout + Test.MessageTimeout);
        }

        /// <summary>
        /// Receives Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="afterStartAction">additional afterStartAction after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Hello message</returns>
        public SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(bool checkIP, 
                                                                              bool checkDeviceId,
                                                                              Action afterStartAction, 
                                                                              Discovery.MessageFilterFunction messageFilter, 
                                                                              int timeout)
        {
            SoapMessage<object> res = ReceiveMessageInternal(DeviceDiscoveryTest.WaitMessageType.Hello, 
                                                             Resources.StepWaitHello_Title,
                                                             Resources.ErrorNoHelloMessage_Text,
                                                             checkIP,
                                                             checkDeviceId,
                                                             afterStartAction,
                                                             messageFilter,
                                                             null,
                                                             timeout);

            if(res != null)
                Test.RunStep(() => Thread.Sleep(5000), "5 seconds timeout after Hello");

            return res != null ? res.ToSoapMessage<Proxies.WSDiscovery.HelloType>() : null;
        }

        public SoapMessage<Proxies.WSDiscovery.HelloType> ReceiveHelloMessage(bool checkIP, 
                                                                              bool checkDeviceId,
                                                                              Action afterStartAction,
                                                                              int timeout)
        {
            return ReceiveHelloMessage(checkIP, checkDeviceId, afterStartAction, null, timeout);
        }

        /// <summary>
        /// Receives Bye message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional afterStartAction after begin waiting</param>
        /// <returns>Bye message</returns>
        public SoapMessage<Proxies.WSDiscovery.ByeType> ReceiveByeMessage(bool checkIP, bool checkDeviceId, Action action)
        {
            return ReceiveByeMessage(checkIP, checkDeviceId, action, Test.RebootTimeout + Test.MessageTimeout);
        }

        /// <summary>
        /// Receives Bye message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional afterStartAction after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Bye message</returns>
        public SoapMessage<Proxies.WSDiscovery.ByeType> ReceiveByeMessage(bool checkIP, bool checkDeviceId, Action action, int timeout)
        {
            SoapMessage<object> res = ReceiveMessageInternal(DeviceDiscoveryTest.WaitMessageType.Bye,
                                                             Resources.StepWaitBye_Title,
                                                             Resources.ErrorNoByeMessage_Text,
                                                             checkIP,
                                                             checkDeviceId,
                                                             action,
                                                             null,
                                                             timeout);

            return res != null ? res.ToSoapMessage<Proxies.WSDiscovery.ByeType>() : null;
        }

        /// <summary>
        /// Receives Bye or Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional afterStartAction after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Bye message</returns>
        public SoapMessage<Proxies.WSDiscovery.ByeType> ReceiveByeOrHelloMessage(bool checkIP, bool checkDeviceId, Action action, Action<SoapMessage<object>> validation)
        {
            SoapMessage<object> res = ReceiveMessageInternal(DeviceDiscoveryTest.WaitMessageType.Bye | DeviceDiscoveryTest.WaitMessageType.Hello,
                                                             Resources.StepWaitByeOrHello_Title,
                                                             null,
                                                             checkIP,
                                                             checkDeviceId,
                                                             action,
                                                             validation,
                                                             Test.RebootTimeout + Test.MessageTimeout);
            return res != null ? res.ToSoapMessage<Proxies.WSDiscovery.ByeType>() : null;
        }

        /// <summary>
        /// Receives message of specified type from DUT
        /// </summary>
        /// <param name="timeout">Time to wait</param>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="afterStartAction">additional afterStartAction after begin waiting</param>
        /// <returns>Message</returns>
        private SoapMessage<object> ReceiveMessageInternal(DeviceDiscoveryTest.WaitMessageType type, 
                                                           string stepName,
                                                           string noMessageText,
                                                           bool checkIP, 
                                                           bool checkDeviceId, 
                                                           Action afterStartAction,
                                                           Discovery.MessageFilterFunction messageFilter,
                                                           Action<SoapMessage<object>> validationAction,
                                                           int timeout)
        {
            var discovery = new Discovery(Test.Nic.IP) { MessageFilter = messageFilter };
            bool waitHello = (type & DeviceDiscoveryTest.WaitMessageType.Hello) != 0;
            bool waitBye = (type & DeviceDiscoveryTest.WaitMessageType.Bye) != 0;
            if (waitHello)
            {
                discovery.HelloReceived += OnHelloReceived;
            }
            if (waitBye)
            {
                discovery.ByeReceived += OnByeReceived;
            }
            discovery.ReceiveError += OnDiscoveryError;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            
            _eventTimeout.Reset();
            _eventHelloReceived.Reset();
            _eventByeReceived.Reset();
            _eventDiscoveryError.Reset();
            SoapMessage<object> response = null;
            try
            {
                var title = string.Format("Waiting for {0} message...", waitBye ? "Bye" : "Hello");
                if (waitBye && waitHello)
                    title = string.Format("Waiting for {0} message...", "Bye or Hello");

                {
                    Test.BeginStep(title);
                    if (waitBye && waitHello)
                    {
                        discovery.WaitByeOrHello(checkIP ? Test.CameraIP : null, checkDeviceId ? Test.CameraID : null, timeout);
                    }
                    else if (waitBye)
                    {
                        discovery.WaitBye(checkIP ? Test.CameraIP : null, checkDeviceId ? Test.CameraID : null, timeout);
                    }
                    else
                    {
                        discovery.WaitHello(checkIP ? Test.CameraIP : null, checkDeviceId ? Test.CameraID : null, timeout);
                    }
                    Test.StepPassed();
                }

            if (afterStartAction != null)
                {
                    afterStartAction();
                }
                Test.RunStep(() =>
                             {
                                 int res = Test.WaitForResponse(new WaitHandle[] { _eventHelloReceived, 
                                                                                   _eventByeReceived, 
                                                                                   _eventDiscoveryError, 
                                                                                   _eventTimeout });
                     
                                 if (((res == 0) && waitHello) || ((res == 1) && waitBye))
                                 {
                                      response = _message;
                                     if (_message != null)
                                     {
                                         string dump = System.Text.Encoding.UTF8.GetString(_message.Raw);
                                         Test.LogResponse(dump);
                                     }
                                 }
                                 else if(res == 2)
                                 {
                                     string message = _error.Message + _error.InnerException ?? " " + _error.InnerException.Message;
                                     throw new AssertException(message);
                                 }
                                else if(noMessageText != null)
                                 {
                                     throw new AssertException(noMessageText);
                                 }
                                 if(validationAction != null)
                                 {
                                     validationAction(response);
                                 }
                             }, 
                             stepName);
            }
            finally
            {
                discovery.Close();
            }
            return response;
        }
        private SoapMessage<object> ReceiveMessageInternal(DeviceDiscoveryTest.WaitMessageType type,
                                                           string stepName,
                                                           string noMessageText,
                                                           bool checkIP,
                                                           bool checkDeviceId,
                                                           Action afterStartAction,
                                                           Action<SoapMessage<object>> validationAction,
                                                           int timeout)
        {
            return ReceiveMessageInternal(type, stepName, noMessageText, checkIP, checkDeviceId, afterStartAction, null, validationAction, timeout);
        }

        /// <summary>
        /// Handles device discovered event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void OnDiscovered(object sender, DiscoveryMessageEventArgs e)
        {
            _message = e.Message.Clone() as SoapMessage<object>;
            _eventProbeMatchReceived.Set();
        }

        /// <summary>
        /// Sends probes message and waits for answer from device
        /// </summary>
        /// <param name="multicast">if true, message will be sent to multicast group address</param>
        /// <param name="scopes">Scope to be probed</param>
        /// <param name="matchRule">Scope matching rule</param>
        /// <returns>ProbeMatches message from device</returns>
        public SoapMessage<Proxies.WSDiscovery.ProbeMatchesType> ProbeDevice(bool multicast,
                                                                             DiscoveryUtils.DiscoveryType[][] types,
                                                                             string[] scopes,
                                                                             string matchRule)
        {
            return ProbeDevice(multicast, types, scopes, matchRule, null);
        }

        public SoapMessage<Proxies.WSDiscovery.ProbeMatchesType> ProbeDevice(bool multicast, 
                                                                             DiscoveryUtils.DiscoveryType[][] types,
                                                                             string[] scopes, 
                                                                             string matchRule,
                                                                             Discovery.ProcessMessage processMessageMethod) 
        {
            System.Diagnostics.Trace.WriteLine(string.Format("ProbeDevice: entry point"));
            System.Diagnostics.Trace.Flush();

            SoapMessage<Proxies.WSDiscovery.ProbeMatchesType> response = null;
            Discovery discovery = new Discovery(Test.Nic.IP, processMessageMethod);
            discovery.Discovered += OnDiscovered;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            discovery.SoapFaultReceived += OnSoapFault;
            discovery.ReceiveError += OnDiscoveryError;

            discovery.MessageSent += OnMessageSent; 

            _eventProbeMatchReceived = new AutoResetEvent(false);
            _eventTimeout = new AutoResetEvent(false);
            _eventFaultReceived = new AutoResetEvent(false);
            _eventDiscoveryError = new AutoResetEvent(false);
            int res = -1;
            try
            {
                discovery.Probe(multicast, Test.CameraIP, null, Test.MessageTimeout, types, scopes, matchRule);
                res = Test.WaitForResponse(new WaitHandle[] { _eventProbeMatchReceived, _eventTimeout, _eventFaultReceived, _eventDiscoveryError });
                if (res == 0)
                {
                    response = _message.ToSoapMessage<Proxies.WSDiscovery.ProbeMatchesType>();
                }
                else if (res == 2)
                {
                    throw new SoapFaultException(_soapFault);
                }
                else if (res == 3)
                {
                    string message = _error.Message + _error.InnerException ?? " " + _error.InnerException.Message;
                    throw new AssertException(message);
                }
            }
            finally
            {
                System.Diagnostics.Trace.WriteLine(string.Format("ProbeDevice: discovery.Dispose, res = {0}", res));
                System.Diagnostics.Trace.Flush();
                discovery.Dispose();
            }

            return response;
        }

        void OnMessageSent(object sender, MessageEventArgs e)
        {
            Test.LogRequest(e.Message);
        }

        /// <summary>
        /// Validates ProbeMatch element of ProbeMatches message
        /// </summary>
        /// <param name="match">Element to be validated</param>
        /// <param name="reason">Reason why element is invalid</param>
        /// <returns>true, if element is valid</returns>
        public bool ValidateProbeMatch(Proxies.WSDiscovery.ProbeMatchType match, out string reason)
        {
            reason = null;
            if (match.Scopes == null)
            {
                reason = Resources.ErrorNoScopes_Text; 
                return false;
            }
            if (match.Types == null)
            {
                //it looks like this check is unnecessary, 
                //because only ProbeMatch with valid type should be validated
                reason = Resources.ErrorNoTypes_Text; 
                return false;
            }
            if (match.EndpointReference == null)
            {
                reason = Resources.ErrorNoEndpointReference_Text;
                return false;
            }
            if(string.IsNullOrEmpty(match.XAddrs))
            {
                reason = Resources.ErrorNoXAddrs_Text;
                return false;
            }
            string[] addresses = match.XAddrs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string address in addresses)
            {
                Uri addr;
                if (!Uri.TryCreate(address, UriKind.Absolute, out addr))
                {
                    reason = string.Format(Resources.ErrorInvalidXAddrs_Format, address);
                    return false;
                }
                else if (string.Compare(addr.LocalPath, "/onvif/device_service", true) != 0)
                {
                    reason = string.Format(Resources.ErrorInvalidServicePath_Format, addr.LocalPath);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates ProbeMatches messages
        /// </summary>
        /// <param name="message">Message to be validated</param>
        /// <param name="reason">Reason why message is invalid</param>
        /// <returns>true, if message is valid</returns>
        public bool ValidateProbeMatchMessage(SoapMessage<Proxies.WSDiscovery.ProbeMatchesType> message, out string reason)
        {
            bool mode1 = Test.Features.Contains(Feature.DiscoveryTypesDnNetworkVideoTransmitter);
            bool mode2 = Test.Features.Contains(Feature.DiscoveryTypesTdsDevice);
            bool res = true;
            reason = null;
            try
            {
                //check Types namespace
                if(message.Object.ProbeMatch == null)
                {
                    reason = Resources.ErrorNoProbeMatch_Text; 
                    return false;
                }
                bool found = false;
                for(int i = 0; i < message.Object.ProbeMatch.Length; i++)
                {
                    Proxies.WSDiscovery.ProbeMatchType match = message.Object.ProbeMatch[i];
                    if (DiscoveryUtils.CheckDeviceMatchType(message, i, out reason, mode1, mode2))
                    {
                        if(!ValidateProbeMatch(match, out reason))
                        {
                            return false;
                        }
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                reason = e.Message;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Waits for device reboot timeout
        /// </summary>
        public void WaitDeviceReboot()
        {
            Test.RunStep(() => 
                         {
                             int res = WaitHandle.WaitAny(new WaitHandle[] { Test.Semaphore.StopEvent }, Test.RebootTimeout);
                             if(res == 0)
                                 throw new StopEventException();
                         }, 
                         Resources.StepWaitReboot_Title);
        }        
    }

    #endregion
}
