using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using DUT.WithLogic.Base;

namespace DUT.WithLogic.Services.DeviceManagement
{
    public class ONVIFDeviceManagementCapabilities
    {
        Proxy.DeviceServiceCapabilities m_Capabilities;
        List<Proxy.Scope> m_FixedScopes;
        string m_manufactorer;
        string m_firmwareVersion;
        string m_serialNumber;
        string m_hardwareId;
        string m_model;
        string m_wsdlUri;
        string m_endpointReference;
        bool m_GetCapabilitiesSupport;
        List<Proxy.OnvifVersion> m_DeviceOldCapabilities_SupportedVersions;
        SerializableDictionary<string, Proxy.User> m_fixedUsers;

        public List<Proxy.OnvifVersion> DeviceOldCapabilities_SupportedVersions
        {
            get { return m_DeviceOldCapabilities_SupportedVersions; }
            set { m_DeviceOldCapabilities_SupportedVersions = value; }
        }

        public bool GetCapabilitiesSupport
        {
            get { return m_GetCapabilitiesSupport; }
            set { m_GetCapabilitiesSupport = value; }
        }

        public string EndpointReference
        {
            get { return m_endpointReference; }
            set { m_endpointReference = value; }
        }

        public string WsdlUri
        {
            get { return m_wsdlUri; }
            set { m_wsdlUri = value; }
        }

        public SerializableDictionary<string, Proxy.User> FixedUsers
        {
            get { return m_fixedUsers; }
            set { m_fixedUsers = value; }
        }

        public List<Proxy.Scope> FixedScopes
        {
            get { return m_FixedScopes; }
            set { m_FixedScopes = value; }
        }

        public string Model
        {
            get { return m_model; }
            set { m_model = value; }
        }

        public string Manufactorer
        {
            get { return m_manufactorer; }
            set { m_manufactorer = value; }
        }
        
        public string FirmwareVersion
        {
            get { return m_firmwareVersion; }
            set { m_firmwareVersion = value; }
        }
        
        public string SerialNumber
        {
            get { return m_serialNumber; }
            set { m_serialNumber = value; }
        }
        
        public string HardwareId
        {
            get { return m_hardwareId; }
            set { m_hardwareId = value; }
        }

        public Proxy.DeviceServiceCapabilities Capabilities
        {
            get { return m_Capabilities; }
            set { m_Capabilities = value; }
        }

        public Proxy.DeviceCapabilities DeviceOldCapabilities
        {
            get
            {
                Proxy.DeviceCapabilities res = new Proxy.DeviceCapabilities();

                res.System = new Proxy.SystemCapabilities1();
                res.System.DiscoveryBye = Capabilities.System.DiscoveryByeSpecified && Capabilities.System.DiscoveryBye;
                res.System.DiscoveryResolve = Capabilities.System.DiscoveryResolveSpecified && Capabilities.System.DiscoveryResolve;
                res.System.FirmwareUpgrade = Capabilities.System.FirmwareUpgradeSpecified && Capabilities.System.FirmwareUpgrade;
                res.System.RemoteDiscovery = Capabilities.System.RemoteDiscoverySpecified && Capabilities.System.RemoteDiscovery;
                res.System.SupportedVersions = DeviceOldCapabilities_SupportedVersions.ToArray();
                res.System.SystemBackup = Capabilities.System.SystemBackupSpecified && Capabilities.System.SystemBackup;
                res.System.SystemLogging = Capabilities.System.SystemLoggingSpecified && Capabilities.System.SystemLogging;
                res.System.Extension = new Proxy.SystemCapabilitiesExtension();
                res.System.Extension.HttpFirmwareUpgradeSpecified = Capabilities.System.HttpFirmwareUpgradeSpecified;
                res.System.Extension.HttpFirmwareUpgrade = Capabilities.System.HttpFirmwareUpgrade;
                res.System.Extension.HttpSupportInformationSpecified = Capabilities.System.HttpSupportInformationSpecified;
                res.System.Extension.HttpSupportInformation = Capabilities.System.HttpSupportInformation;
                res.System.Extension.HttpSystemBackupSpecified = Capabilities.System.HttpSystemBackupSpecified;
                res.System.Extension.HttpSystemBackup = Capabilities.System.HttpSystemBackup;
                res.System.Extension.HttpSystemLoggingSpecified = Capabilities.System.HttpSystemLoggingSpecified;
                res.System.Extension.HttpSystemLogging = Capabilities.System.HttpSystemLogging;
                res.System.Extension.Extension = new Proxy.SystemCapabilitiesExtension2();

                res.Security = new Proxy.SecurityCapabilities1();
                res.Security.AccessPolicyConfig = Capabilities.Security.AccessPolicyConfigSpecified && Capabilities.Security.AccessPolicyConfig;
                res.Security.KerberosToken = Capabilities.Security.KerberosTokenSpecified && Capabilities.Security.KerberosToken;
                res.Security.OnboardKeyGeneration = Capabilities.Security.OnboardKeyGenerationSpecified && Capabilities.Security.OnboardKeyGeneration;
                res.Security.RELToken = Capabilities.Security.RELTokenSpecified && Capabilities.Security.RELToken;
                res.Security.SAMLToken = Capabilities.Security.SAMLTokenSpecified && Capabilities.Security.SAMLToken;
                res.Security.TLS11 = Capabilities.Security.TLS11Specified && Capabilities.Security.TLS11;
                res.Security.TLS12 = Capabilities.Security.TLS12Specified && Capabilities.Security.TLS12;
                res.Security.X509Token = Capabilities.Security.X509TokenSpecified && Capabilities.Security.X509Token;
                res.Security.Extension = new Proxy.SecurityCapabilitiesExtension();
                res.Security.Extension.TLS10 = Capabilities.Security.TLS10Specified && Capabilities.Security.TLS10;
                res.Security.Extension.Extension = new Proxy.SecurityCapabilitiesExtension2();
                res.Security.Extension.Extension.Dot1X = Capabilities.Security.Dot1XSpecified && Capabilities.Security.Dot1X;
                res.Security.Extension.Extension.RemoteUserHandling = Capabilities.Security.RemoteUserHandlingSpecified && Capabilities.Security.RemoteUserHandling;
                res.Security.Extension.Extension.SupportedEAPMethod = Capabilities.Security.SupportedEAPMethods;

                res.Network = new Proxy.NetworkCapabilities1();
                res.Network.DynDNSSpecified = Capabilities.Network.DynDNSSpecified;
                res.Network.DynDNS = Capabilities.Network.DynDNS;
                res.Network.IPFilterSpecified = Capabilities.Network.IPFilterSpecified;
                res.Network.IPFilter = Capabilities.Network.IPFilter;
                res.Network.IPVersion6Specified = Capabilities.Network.IPVersion6Specified;
                res.Network.IPVersion6 = Capabilities.Network.IPVersion6;
                res.Network.ZeroConfigurationSpecified = Capabilities.Network.ZeroConfigurationSpecified;
                res.Network.ZeroConfiguration = Capabilities.Network.ZeroConfiguration;
                res.Network.Extension = new Proxy.NetworkCapabilitiesExtension();
                res.Network.Extension.Dot11ConfigurationSpecified = Capabilities.Network.Dot11ConfigurationSpecified;
                res.Network.Extension.Dot11Configuration = Capabilities.Network.Dot11Configuration;

                res.IO = new Proxy.IOCapabilities();
                //TODO: get from DeviceIO

                return res;
            }
        }

        public static void Serialize()
        {
            ONVIFDeviceManagementCapabilities temp = new ONVIFDeviceManagementCapabilities();
            temp.DeviceOldCapabilities_SupportedVersions = new List<Proxy.OnvifVersion>();
            Proxy.OnvifVersion supportedVersions = new Proxy.OnvifVersion();
            supportedVersions.Major = 1;
            supportedVersions.Minor = 02;
            temp.DeviceOldCapabilities_SupportedVersions.Add(supportedVersions);

            using (XmlWriter writer = XmlWriter.Create(@"D:\2.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFDeviceManagementCapabilities));
                serializer.Serialize(writer, temp);
            }
        }

        public override string ToString()
        {
            StringWriter stringWriter;
            using (stringWriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFDeviceManagementCapabilities));
                serializer.Serialize(stringWriter, this);
            }
            return stringWriter.ToString();
        }

        public static ONVIFDeviceManagementCapabilities Load()
        {
            using (XmlReader reader = XmlReader.Create(Engine.ONVIFServiceList.FullUri(Base.AppPaths.PATH_DEVICEMANAGEMENTCAPABILITIES)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFDeviceManagementCapabilities));
                return (ONVIFDeviceManagementCapabilities)serializer.Deserialize(reader);
            }

        }

    }
}