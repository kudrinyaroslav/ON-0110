///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.Tests.Common.TestBase;
using System.ServiceModel;
using TestTool.Proxies.Device;
using TestTool.Tests.Common.TestEngine;

namespace TestTool.Tests.TestCases.Base
{
    public class DeviceManagementTest : BaseServiceTest<Device, DeviceClient>
    {
        public DeviceManagementTest(TestLaunchParam param)
            : base(param)
        {
        }

        protected override DeviceClient CreateClient()
        {
            HttpTransport.HttpBinding binding = (HttpTransport.HttpBinding)CreateBinding(true);
            DeviceClient client = new DeviceClient(binding, new EndpointAddress(_cameraAddress));
            return client;
        }
        
        protected void ValidateNoSuchServiceFault(FaultException exc)
        {
            bool fault = exc.IsValidOnvifFault("Receiver/ActionNotSupported/NoSuchService");
            SaveStepFault(exc);
            if (fault)
            {
                StepPassed();
            }
            else
            {
                throw exc;
            }
        }

        protected DiscoveryMode GetDiscoveryMode()
        {
            DiscoveryMode mode = DiscoveryMode.Discoverable;
            RunStep(() => { mode = Client.GetDiscoveryMode(); }, "Get Discovery Mode");
            return mode;
        }

        protected void SetDiscoveryMode(DiscoveryMode mode)
        {
            RunStep(() => { Client.SetDiscoveryMode(mode);}, "Set Discovery Mode");
        }

        protected string GetWsdlUrl()
        {
            string wsdlUrl = string.Empty;
            RunStep(() => { wsdlUrl = Client.GetWsdlUrl(); }, "Get WSDL URL"); 
            return wsdlUrl;
        }

        protected Capabilities GetCapabilities(CapabilityCategory[] categories, string stepName)
        {
            Capabilities capabilities = null;
            RunStep(() => { capabilities = Client.GetCapabilities(categories); }, stepName);
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
            return hostname;
        }

        protected void SetHostname(string name, string stepName)
        {
            RunStep(() => {Client.SetHostname(name); }, stepName); 
        }

        protected void SetHostname(string name)
        {
            SetHostname(name, "Set Hostname");
        }

        protected DNSInformation GetDnsConfiguration()
        {
            DNSInformation dnsInformation = null;
            RunStep(() => { dnsInformation = Client.GetDNS(); }, "Get DNS configuration");
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
        }

        protected NTPInformation GetNTP()
        {
            NTPInformation ntpInformation = null;
            RunStep(()=> {ntpInformation =  Client.GetNTP();}, "Get NTP information"); 
            return ntpInformation;
        }

        protected void SetNTP(NTPInformation ntpInformation, string stepName)
        {
            RunStep(() => { Client.SetNTP(ntpInformation.FromDHCP, ntpInformation.NTPManual); }, stepName);
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
            return dateTime;
        }

        protected void SetSystemDateAndTime(SystemDateTime dateTime)
        {
            RunStep( () => { Client.SetSystemDateAndTime(dateTime.DateTimeType, 
                dateTime.DaylightSavings, 
                dateTime.TimeZone, 
                dateTime.UTCDateTime);}, "Set system date and time");
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

            model = modelCopy;
            firmwareVersion = firmwareVersionCopy;
            serialNumber = serialNumberCopy;
            hardwareId = hardwareIdCopy;

            return information;
        }


        protected void SetSystemFactoryDefault(FactoryDefaultType type)
        {
             RunStep(() => { Client.SetSystemFactoryDefault(type); }, "Set System Factory Default");
        }

        protected string SystemReboot()
        {
            string message = null;
            RunStep( ()=> 
            {            
                message = Client.SystemReboot();
                LogStepEvent(string.Format("Response Message received: '{0}'", message));
            },  "Send System Reboot message");
            return message;
        }
    
        protected NetworkInterface[] GetNetworkInterfaces()
        {
            NetworkInterface[] interfaces = null; 
            RunStep(() => {interfaces = Client.GetNetworkInterfaces(); }, "Get network interfaces"); 
            return interfaces;
        }

        protected bool SetNetworkInterface(string interfaceToken, 
            NetworkInterfaceSetConfiguration configuration)
        {
#if true
            return Client.SetNetworkInterfaces(interfaceToken, configuration);
#else
            bool rebootNeeded = false;
            RunStep(() => { rebootNeeded = Client.SetNetworkInterfaces(interfaceToken, configuration); }
                , "Set network interfaces");
           
            return rebootNeeded;
#endif
        }

        protected NetworkZeroConfiguration GetZeroConfiguration()
        {
            NetworkZeroConfiguration zero = null;
            RunStep( () => { zero = Client.GetZeroConfiguration();}, "Get Network Zero configuration");
            return zero;
        }

        protected void SetZeroConfiguration(string interfaceToken, bool enabled)
        {
            RunStep( () => { Client.SetZeroConfiguration(interfaceToken, enabled);}, 
                "Set Network Zero configuration" );

        }

        protected NetworkProtocol[] GetNetworkProtocols()
        {
            NetworkProtocol[] protocols = null;
            RunStep(() => { protocols = Client.GetNetworkProtocols(); }, "Get Network Protocols");
            return protocols;
        }

        protected void SetNetworkProtocols(NetworkProtocol[] protocols)
        {
            RunStep(() => { Client.SetNetworkProtocols(protocols); }, "Set Network Protocols");
        }

        protected NetworkGateway GetNetworkDefaultGateway()
        {
            NetworkGateway result = null;
            RunStep(() => { result = Client.GetNetworkDefaultGateway(); }, "Get Network Default Gateway");
            return result;
        }

        protected void SetNetworkDefaultGateway(string[] ipv4Adddresses, string[] ipv6Addresses)
        {
            RunStep(() => { Client.SetNetworkDefaultGateway(ipv4Adddresses, ipv6Addresses ); }, "Set Network Default Gateway");
        }

        protected User[] GetUsers()
        {
            User[] response = null;
            RunStep(() => { response = Client.GetUsers(); }, "Get Users");
            return response;
        }

        protected void CreateUsers(User[] users)
        {
            RunStep(() => { Client.CreateUsers(users); }, "Create users");
        }

        protected void CreateUsers(User[] users, string stepName)
        {
            RunStep( () => { Client.CreateUsers(users);}, stepName);  
        }

        protected void DeleteUsers(string[] users)
        {
            RunStep(() => { Client.DeleteUsers(users); }, "Delete users");
        }

        protected void SetUser(User[] users)
        {
            RunStep( () => { Client.SetUser(users);}, "Set users");
        }
    }
}
