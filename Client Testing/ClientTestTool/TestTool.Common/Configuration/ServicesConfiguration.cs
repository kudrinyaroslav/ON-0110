using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TestTool.Onvif;

namespace TestTool.Common.Configuration
{
    public class CapabilitiesWrapper
    {
        public CapabilitiesWrapper()
        {
        }

        [System.Xml.Serialization.XmlElement(Namespace = Definitions.OnvifService.DEVICE, ElementName = "Capabilities")]
        public DeviceServiceCapabilities DeviceServiceCapabilities { get; set; }

        [System.Xml.Serialization.XmlElement(Namespace = Definitions.OnvifService.DOORCONTROL, ElementName = "Capabilities")]
        public DoorControlServiceCapabilities DoorServiceCapabilities { get; set; }

        [System.Xml.Serialization.XmlElement(Namespace = Definitions.OnvifService.ACCESSCONTROL, ElementName = "Capabilities")]
        public AccessControlServiceCapabilities AccessControlCapabilities { get; set; }
    }

    public class ServicesConfiguration
    {

        // Device service capabilities;
        private DeviceServiceCapabilities _deviceServiceCapabilities;
        public DeviceServiceCapabilities DeviceServiceCapabilities
        {
            get
            {
                return _deviceServiceCapabilities;
            }
            set
            {
                _deviceServiceCapabilities = value;
                CreateOldStyleCapabilities();
            }
        }

        public void CreateOldStyleCapabilities()
        {
            _capabilities = new Capabilities();
            _capabilities.Device = new DeviceCapabilities();

            if (_deviceServiceCapabilities.Network != null)
            {
                NetworkCapabilities1 networkCapabilities = new NetworkCapabilities1();
                NetworkCapabilities network = _deviceServiceCapabilities.Network;
                networkCapabilities.DynDNS = network.DynDNS;
                networkCapabilities.DynDNSSpecified = network.DynDNSSpecified;
                networkCapabilities.IPFilter = network.IPFilter;
                networkCapabilities.IPFilterSpecified = network.IPFilterSpecified;
                networkCapabilities.IPVersion6 = network.IPVersion6;
                networkCapabilities.IPVersion6Specified = network.IPVersion6Specified;
                networkCapabilities.ZeroConfiguration = network.ZeroConfiguration;
                networkCapabilities.ZeroConfigurationSpecified = network.ZeroConfigurationSpecified;

                networkCapabilities.Extension = new NetworkCapabilitiesExtension();
                networkCapabilities.Extension.Dot11ConfigurationSpecified = network.Dot11ConfigurationSpecified;
                networkCapabilities.Extension.Dot11Configuration = network.Dot11Configuration;

                _capabilities.Device.Network = networkCapabilities;
            }

            if (_deviceServiceCapabilities.Security != null)
            {
                SecurityCapabilities1 securityCapabilities = new SecurityCapabilities1();
                SecurityCapabilities security = _deviceServiceCapabilities.Security;

                securityCapabilities.AccessPolicyConfig = security.AccessPolicyConfigSpecified ? security.AccessPolicyConfig : false;
                securityCapabilities.KerberosToken = security.KerberosTokenSpecified ? security.KerberosToken : false;
                securityCapabilities.OnboardKeyGeneration = security.OnboardKeyGenerationSpecified
                                                                ? security.OnboardKeyGeneration
                                                                : false;
                securityCapabilities.RELToken = security.RELTokenSpecified ? security.RELToken : false;
                securityCapabilities.SAMLToken = security.SAMLTokenSpecified ? security.SAMLToken : false;
                securityCapabilities.TLS11 = security.TLS11Specified ? security.TLS11 : false;
                securityCapabilities.TLS12 = security.TLS12Specified ? security.TLS12 : false;
                securityCapabilities.X509Token = security.X509TokenSpecified ? security.X509Token : false;

                _capabilities.Device.Security = securityCapabilities;
            }

            if (_deviceServiceCapabilities.System != null)
            {
                SystemCapabilities1 systemCapabilities = new SystemCapabilities1();
                SystemCapabilities system = _deviceServiceCapabilities.System;

                systemCapabilities.DiscoveryBye = system.DiscoveryByeSpecified ? system.DiscoveryBye : false;
                systemCapabilities.DiscoveryResolve = system.DiscoveryResolveSpecified ? system.DiscoveryResolve : false;
                systemCapabilities.FirmwareUpgrade = system.FirmwareUpgradeSpecified ? system.FirmwareUpgrade : false;
                systemCapabilities.RemoteDiscovery = system.RemoteDiscoverySpecified ? system.RemoteDiscovery : false;
                systemCapabilities.SupportedVersions = new OnvifVersion[]{new OnvifVersion(){Major = 2, Minor = 1}};
                systemCapabilities.SystemBackup = system.SystemBackupSpecified ? system.SystemBackup : false;
                systemCapabilities.SystemLogging = system.SystemLoggingSpecified ? system.SystemLogging : false;

                systemCapabilities.Extension = new SystemCapabilitiesExtension();
                systemCapabilities.Extension.HttpFirmwareUpgrade = system.HttpFirmwareUpgrade;
                systemCapabilities.Extension.HttpFirmwareUpgradeSpecified = system.HttpFirmwareUpgradeSpecified;
                systemCapabilities.Extension.HttpSupportInformation = system.HttpSupportInformation;
                systemCapabilities.Extension.HttpSupportInformationSpecified = system.HttpSupportInformationSpecified;
                systemCapabilities.Extension.HttpSystemBackup = system.HttpSystemBackup;
                systemCapabilities.Extension.HttpSystemBackupSpecified = system.HttpSystemBackupSpecified;
                systemCapabilities.Extension.HttpSystemLogging = system.HttpSystemLogging;
                systemCapabilities.Extension.HttpSystemLoggingSpecified = system.HttpSystemLoggingSpecified;

                _capabilities.Device.System = systemCapabilities;
            }

            _capabilities.Events = new EventCapabilities();

        }

        private Capabilities _capabilities;
        public Capabilities Capabilities
        {
            get { return _capabilities; }
        }


        public DoorControlServiceCapabilities DoorServiceCapabilities { get; set; }

        public AccessControlServiceCapabilities AccessControlCapabilities { get; set; }


        public void InitializeXmlElements()
        {
            XmlDocument doc = new XmlDocument();

            StringBuilder sb = new StringBuilder();
            XmlSerializer ser = new XmlSerializer(typeof(CapabilitiesWrapper));

            XmlWriter sw = XmlWriter.Create(sb);

            CapabilitiesWrapper cw = new CapabilitiesWrapper();
            cw.AccessControlCapabilities = AccessControlCapabilities;
            cw.DeviceServiceCapabilities = DeviceServiceCapabilities;
            cw.DoorServiceCapabilities = DoorServiceCapabilities;

            ser.Serialize(sw, cw);
            doc.LoadXml(sb.ToString());

            _elements = new Dictionary<string, XmlElement>();
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                XmlElement element = node as XmlElement;
                if (element != null)
                {
                    _elements.Add(element.NamespaceURI, element);
                }
            }
        }

        private Dictionary<string, XmlElement> _elements;

        public XmlElement GetServiceCapabilitiesElement(string ns)
        {
            if (_elements.ContainsKey(ns))
            {
                return _elements[ns];
            }
            else
            {
                return null;
            }
        }

        // Services
        public List<Service> Services { get; set; }

    }
}
