///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;
using TestTool.HttpTransport;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.Base
{
    public class DeviceManagementTest : BaseServiceTest<Device, DeviceClient>
    {

        /// <summary>
        /// Relay output delay.
        /// </summary>
        protected int _relayOutputDelayTimeMonostable;
        protected int _relayOutputDelayTimeBistable;
        
        public DeviceManagementTest(TestLaunchParam param)
            : base(param)
        {
            _relayOutputDelayTimeMonostable = param.RelayOutputDelayTimeMonostable;
            _relayOutputDelayTimeBistable = param.RelayOutputDelayTimeMonostable;
        }

        protected override DeviceClient CreateClient()
        {
            Binding binding = 
                CreateBinding(true, 
                new IChannelController[] {new SoapValidator(DeviceManagementSchemasSet.GetInstance())} );
            DeviceClient client = new DeviceClient(binding, new EndpointAddress(_cameraAddress));
            return client;
        }

        protected DiscoveryMode GetDiscoveryMode()
        {
            DiscoveryMode mode = DiscoveryMode.Discoverable;
            RunStep(() => { mode = Client.GetDiscoveryMode(); }, "Get Discovery Mode");
            DoRequestDelay();
            return mode;
        }

        protected void SetDiscoveryMode(DiscoveryMode mode)
        {
            RunStep(() => { Client.SetDiscoveryMode(mode);}, "Set Discovery Mode");
            DoRequestDelay();
        }

        protected string GetWsdlUrl()
        {
            string wsdlUrl = string.Empty;
            RunStep(() => { wsdlUrl = Client.GetWsdlUrl(); }, "Get WSDL URL");
            DoRequestDelay();
            return wsdlUrl;
        }

        protected Capabilities GetCapabilities(CapabilityCategory[] categories, string stepName)
        {
            Capabilities capabilities = null;
            RunStep(() => { capabilities = Client.GetCapabilities(categories); }, stepName);
            DoRequestDelay();
            return capabilities;
        }

        protected Capabilities GetCapabilities(CapabilityCategory[] categories)
        {
            return GetCapabilities(categories, "Get capabilities");
        }

        protected HostnameInformation GetHostname()
        {
            HostnameInformation hostname = null;
            RunStep(() => { hostname = Client.GetHostname(); }, "Get Hostname");
            DoRequestDelay();
            return hostname;
        }

        protected void SetHostname(string name, string stepName)
        {
            RunStep(() => {Client.SetHostname(name); }, stepName);
            DoRequestDelay();
        }

        protected void SetHostname(string name)
        {
            SetHostname(name, "Set Hostname");
        }

        protected DNSInformation GetDnsConfiguration()
        {
            DNSInformation dnsInformation = null;
            RunStep(() => { dnsInformation = Client.GetDNS(); }, "Get DNS configuration");
            DoRequestDelay();
            return dnsInformation;
        }

        protected void SetDnsConfiguration(DNSInformation information)
        {
            SetDnsConfiguration(information, "Set DNS configuration"); 
        }

        protected void SetDnsConfiguration(DNSInformation information, string stepName)
        {
            RunStep( () => 
            {
                Client.SetDNS(
                information.FromDHCP, 
                information.SearchDomain, 
                information.DNSManual); 
            }, stepName);
            DoRequestDelay();
        }

        protected NTPInformation GetNTP()
        {
            NTPInformation ntpInformation = null;
            RunStep(()=> {ntpInformation =  Client.GetNTP();}, "Get NTP information");
            DoRequestDelay();
            return ntpInformation;
        }

        protected void SetNTP(NTPInformation ntpInformation, string stepName)
        {
            RunStep(() => { Client.SetNTP(ntpInformation.FromDHCP, ntpInformation.NTPManual); }, stepName);
            DoRequestDelay();
        }

        protected void SetNTP(NTPInformation ntpInformation)
        {
            SetNTP(ntpInformation, "Set NTP configuration");
        }

        protected SystemDateTime GetSystemDateAndTime()
        {
            SystemDateTime dateTime = null;
            RunStep( () =>
            {
                dateTime = Client.GetSystemDateAndTime();
            }, 
            "Get system date and time");
            DoRequestDelay();
            return dateTime;
        }

        protected void SetSystemDateAndTime(SystemDateTime dateTime)
        {
            SetSystemDateAndTime(dateTime, "Set system date and time");
        }

        protected void SetSystemDateAndTime(SystemDateTime dateTime, string stepName)
        {
            RunStep( () => { Client.SetSystemDateAndTime(dateTime.DateTimeType, 
                dateTime.DaylightSavings, 
                dateTime.TimeZone, 
                dateTime.UTCDateTime);}, stepName);
            DoRequestDelay();
        }

        protected string GetDeviceInformation(out string model, 
            out string firmwareVersion, 
            out string serialNumber, 
            out string hardwareId)
        {
            string information = null;

            // Cannot use ref or out parameter inside an anonymous method, lambda expression, or query expression
            string modelCopy = null;
            string firmwareVersionCopy = null;
            string serialNumberCopy = null;
            string hardwareIdCopy = null;
            
            RunStep( () => 
            {
                information = Client.GetDeviceInformation(out modelCopy,
                 out firmwareVersionCopy,
                 out serialNumberCopy,
                 out hardwareIdCopy);
               
            }, "Get device information");
            
            DoRequestDelay();

            model = modelCopy;
            firmwareVersion = firmwareVersionCopy;
            serialNumber = serialNumberCopy;
            hardwareId = hardwareIdCopy;

            return information;
        }


        protected void SetSystemFactoryDefault(FactoryDefaultType type)
        {
             RunStep(() => { Client.SetSystemFactoryDefault(type); }, "Set System Factory Default");
             DoRequestDelay();
        }

        protected string SystemReboot()
        {
            string message = null;
            RunStep( ()=> 
            {            
                message = Client.SystemReboot();
                LogStepEvent(string.Format("Response Message received: '{0}'", message));
            },  "Send System Reboot message");
            DoRequestDelay();
            return message;
        }
    
        protected NetworkInterface[] GetNetworkInterfaces()
        {
            NetworkInterface[] interfaces = null; 
            RunStep(() => {interfaces = Client.GetNetworkInterfaces(); }, "Get network interfaces");
            DoRequestDelay();
            return interfaces;
        }

        protected bool SetNetworkInterface(string interfaceToken, 
            NetworkInterfaceSetConfiguration configuration)
        {
            bool result =  Client.SetNetworkInterfaces(interfaceToken, configuration);
            DoRequestDelay();
            return result;
        }

        protected NetworkZeroConfiguration GetZeroConfiguration()
        {
            NetworkZeroConfiguration zero = null;
            RunStep( () => { zero = Client.GetZeroConfiguration();}, "Get Network Zero configuration");
            DoRequestDelay();
            return zero;
        }

        protected void SetZeroConfiguration(string interfaceToken, bool enabled)
        {
            RunStep( () => { Client.SetZeroConfiguration(interfaceToken, enabled);}, 
                "Set Network Zero configuration" );
            DoRequestDelay();

        }

        protected NetworkProtocol[] GetNetworkProtocols()
        {
            NetworkProtocol[] protocols = null;
            RunStep(() => { protocols = Client.GetNetworkProtocols(); }, "Get Network Protocols");
            DoRequestDelay();
            return protocols;
        }

        protected void SetNetworkProtocols(NetworkProtocol[] protocols)
        {
            RunStep(() => { Client.SetNetworkProtocols(protocols); }, "Set Network Protocols");
            DoRequestDelay();
        }

        protected NetworkGateway GetNetworkDefaultGateway()
        {
            NetworkGateway result = null;
            RunStep(() => { result = Client.GetNetworkDefaultGateway(); }, "Get Network Default Gateway");
            DoRequestDelay();
            return result;
        }

        protected void SetNetworkDefaultGateway(string[] ipv4Adddresses, string[] ipv6Addresses)
        {
            RunStep(() => { Client.SetNetworkDefaultGateway(ipv4Adddresses, ipv6Addresses ); }, "Set Network Default Gateway");
            DoRequestDelay();
        }

        protected User[] GetUsers()
        {
            User[] response = null;
            RunStep(() => { response = Client.GetUsers(); }, "Get Users");
            DoRequestDelay();
            return response;
        }

        protected void CreateUsers(User[] users)
        {
            RunStep(() => { Client.CreateUsers(users); }, "Create users");
            DoRequestDelay();
        }

        protected void CreateUsers(User[] users, string stepName)
        {
            RunStep( () => { Client.CreateUsers(users);}, stepName);
            DoRequestDelay();
        }

        protected void DeleteUsers(string[] users)
        {
            RunStep(() => { Client.DeleteUsers(users); }, "Delete users");
            DoRequestDelay();
        }

        protected void SetUser(User[] users)
        {
            RunStep( () => { Client.SetUser(users);}, "Set users");
            DoRequestDelay();
        }

        protected RelayOutput[] GetRelayOutputs()
        {
            return GetRelayOutputs("Get relay outputs");
        }

        protected RelayOutput[] GetRelayOutputs(string stepName)
        {
            RelayOutput[] outputs = null;
            RunStep(() => { outputs = Client.GetRelayOutputs(); }, stepName);
            DoRequestDelay();
            return outputs;
        }

        protected void SetRelayOutputSettings(string token, RelayOutputSettings settings)
        {
            SetRelayOutputSettings(token, settings,
                                   string.Format("Set relay output settings (IdleState = {0}, Mode = {1})",
                                                 settings.IdleState, settings.Mode));
        }

        protected void SetRelayOutputSettings(string token, RelayOutputSettings settings, string stepName)
        {
            RunStep( ()=> { Client.SetRelayOutputSettings(token, settings);}, stepName);
            DoRequestDelay();
        }

        protected void SetRelayOutputState(string token, RelayLogicalState state)
        {
            SetRelayOutputState(token, state, "Set relay output state");
        }

        protected void SetRelayOutputState(string token, RelayLogicalState state, string stepName)
        {
            RunStep(()=> { Client.SetRelayOutputState(token, state); }, stepName);
            DoRequestDelay();
        }

        protected SystemLog GetSystemLog(SystemLogType logType, string stepName)
        {
            SystemLog log = null;
            RunStep(() => { log = Client.GetSystemLog(logType); }, stepName);
            DoRequestDelay();
            return log;
        }

        protected SystemLog GetSystemLog(SystemLogType logType, string stepName, string allowedFault)
        {
            SystemLog log = null;
            RunStepAllowFault(() => { log = Client.GetSystemLog(logType); }, stepName, allowedFault);
            DoRequestDelay();
            return log;
        }

        protected Service[] GetServices(bool includeCapabilities)
        {
            Service[] services = null;
            RunStep(() => { services = Client.GetServices(includeCapabilities); }, "Get Services");
            DoRequestDelay();
            return services;
        }

        protected DeviceServiceCapabilities GetServiceCapabilities()
        {
            DeviceServiceCapabilities capabilities = null;
            RunStep( () => { capabilities = Client.GetServiceCapabilities(); }, "Get service capabilities");
            DoRequestDelay();
            return capabilities;
        }


        protected T ExtractCapabilities<T>(XmlElement element, string ns)
        {
            BeginStep("Parse Capabilities element");

            System.Xml.Serialization.XmlRootAttribute xRoot = new System.Xml.Serialization.XmlRootAttribute();
            xRoot.ElementName = "Capabilities";
            xRoot.IsNullable = true;
            xRoot.Namespace = ns;

            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);

            XmlReader reader = new XmlNodeReader(element);

            T capabilities = (T)serializer.Deserialize(reader);
            StepPassed();
            return capabilities;
        }


    }
}