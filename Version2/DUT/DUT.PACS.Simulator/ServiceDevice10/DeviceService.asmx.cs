using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.PACS.Simulator.Configuration;

namespace DUT.PACS.Simulator.Device10
{
    /// <summary>
    /// Summary description for DeviceService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeviceServiceFake : DeviceServiceBinding
    {

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServices", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Service")]
        public override Service[] GetServices(bool IncludeCapability)
        {
            Service[] res = new Service[7];
            int serviceId = 0;
            string hostAndPort = HttpContext.Current.Request.Url.Authority;
            

            Simulator.SystemCapabilities capabilities = Simulator.SystemCapabilities.Instance;

            //Device Management Service
            res[serviceId] = new Service();
            res[serviceId].Namespace = @"http://www.onvif.org/ver10/device/wsdl";
            res[serviceId].XAddr = "http://" + hostAndPort + "/ServiceDevice10/DeviceService.asmx";

            if (IncludeCapability)
            {
                res[serviceId].Capabilities = capabilities.GetServiceCapabilitiesElement(res[serviceId].Namespace);
            }
            else
            {
                res[serviceId].Capabilities = null;
            }

            res[serviceId].Version = new OnvifVersion();
            res[serviceId].Version.Major = 1;
            res[serviceId].Version.Minor = 4;
            serviceId++;

            //Event Service
            res[serviceId] = new Service();
            res[serviceId].Namespace = @"http://www.onvif.org/ver10/events/wsdl";
            res[serviceId].XAddr = "http://" + hostAndPort + "/ServiceEvents10/EventService.asmx";

            if (IncludeCapability)
            { }
            else
            {
                res[serviceId].Capabilities = null;
            }

            res[serviceId].Version = new OnvifVersion();
            res[serviceId].Version.Major = 0;
            res[serviceId].Version.Minor = 1;
            serviceId++;

            //Access Control Service
            res[serviceId] = new Service();
            res[serviceId].Namespace = @"http://www.onvif.org/ver10/accesscontrol/wsdl";
            res[serviceId].XAddr = "http://" + hostAndPort + "/ServicePACS10/PACSService.asmx";

            if (IncludeCapability)
            {
                res[serviceId].Capabilities = capabilities.GetServiceCapabilitiesElement(res[serviceId].Namespace);
            }
            else
            {
                res[serviceId].Capabilities = null;
            }

            res[serviceId].Version = new OnvifVersion();
            res[serviceId].Version.Major = 0;
            res[serviceId].Version.Minor = 1;
            serviceId++;

            //Door Control Service
            res[serviceId] = new Service();
            res[serviceId].Namespace = @"http://www.onvif.org/ver10/doorcontrol/wsdl";
            res[serviceId].XAddr = "http://" + hostAndPort + "/ServiceDoorControl10/DoorControlService.asmx";

            if (IncludeCapability)
            {
                res[serviceId].Capabilities = capabilities.GetServiceCapabilitiesElement(res[serviceId].Namespace);
            }
            else
            {
                res[serviceId].Capabilities = null;
            }

            res[serviceId].Version = new OnvifVersion();
            res[serviceId].Version.Major = 0;
            res[serviceId].Version.Minor = 1;
            serviceId++;

            //Access Rules Service
            res[serviceId] = new Service();
            res[serviceId].Namespace = @"http://www.onvif.org/ver10/accessrules/wsdl";
            res[serviceId].XAddr = "http://" + hostAndPort + "/ServiceAccessRules/AccessRulesService.asmx";

            if (IncludeCapability)
            {
                res[serviceId].Capabilities = capabilities.GetServiceCapabilitiesElement(res[serviceId].Namespace);
            }
            else
            {
                res[serviceId].Capabilities = null;
            }

            res[serviceId].Version = new OnvifVersion();
            res[serviceId].Version.Major = 0;
            res[serviceId].Version.Minor = 1;
            serviceId++;

            //Credential Service
            res[serviceId] = new Service();
            res[serviceId].Namespace = @"http://www.onvif.org/ver10/credential/wsdl";
            res[serviceId].XAddr = "http://" + hostAndPort + "/ServiceCredential10/CredentialService.asmx";

            if (IncludeCapability)
            {
                res[serviceId].Capabilities = capabilities.GetServiceCapabilitiesElement(res[serviceId].Namespace);
            }
            else
            {
                res[serviceId].Capabilities = null;
            }

            res[serviceId].Version = new OnvifVersion();
            res[serviceId].Version.Major = 0;
            res[serviceId].Version.Minor = 1;
            serviceId++;

            //User Service
            res[serviceId] = new Service();
            res[serviceId].Namespace = @"http://www.onvif.org/v3/User/wsdl";
            res[serviceId].XAddr = "http://" + hostAndPort + "/ServiceUser03/UserService.asmx";

            if (IncludeCapability)
            {
                res[serviceId].Capabilities = capabilities.GetServiceCapabilitiesElement(res[serviceId].Namespace);
            }
            else
            {
                res[serviceId].Capabilities = null;
            }

            res[serviceId].Version = new OnvifVersion();
            res[serviceId].Version.Major = 0;
            res[serviceId].Version.Minor = 1;
            serviceId++;

            return res;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override DeviceServiceCapabilities GetServiceCapabilities()
        {
            DeviceServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.DeviceServiceCapabilities;
            return capabilities;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDeviceInformation", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Manufacturer")]
        public override string GetDeviceInformation(out string Model, out string FirmwareVersion, out string SerialNumber, out string HardwareId)
        {
            FirmwareVersion = "1.0";
            Model = "Simulator";
            SerialNumber = "123-456-789";
            HardwareId = "987-654-321";

            return "Monster, Inc";
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetSystemDateAndTime", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSystemDateAndTime(SetDateTimeType DateTimeType, bool DaylightSavings, TimeZone TimeZone, DateTime UTCDateTime)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemDateAndTime", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SystemDateAndTime")]
        public override SystemDateTime GetSystemDateAndTime()
        {
            throw new NotImplementedException();
        }

        public override void SetSystemFactoryDefault(FactoryDefaultType FactoryDefault)
        {
            throw new NotImplementedException();
        }

        public override string UpgradeSystemFirmware(AttachmentData Firmware)
        {
            throw new NotImplementedException();
        }

        public override string SystemReboot()
        {
            throw new NotImplementedException();
        }

        public override void RestoreSystem(BackupFile[] BackupFiles)
        {
            throw new NotImplementedException();
        }

        public override BackupFile[] GetSystemBackup()
        {
            throw new NotImplementedException();
        }

        public override SystemLog GetSystemLog(SystemLogType LogType)
        {
            throw new NotImplementedException();
        }

        public override SupportInformation GetSystemSupportInformation()
        {
            throw new NotImplementedException();
        }
        
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Scopes")]
        public override Scope[] GetScopes()
        {
            Discovery.Discovery discovery = (Discovery.Discovery) Application["Discovery"];
            List<Scope> scopes = new List<Scope>();

            foreach (string scope in discovery.Scopes)
            {
                Scope sc = new Scope();
                sc.ScopeDef = ScopeDefinition.Fixed;
                sc.ScopeItem = scope;
                scopes.Add(sc);
            }

            return scopes.ToArray();
        }

        public override void SetScopes(string[] Scopes)
        {
            throw new NotImplementedException();
        }

        public override void AddScopes(string[] ScopeItem)
        {
            throw new NotImplementedException();
        }

        public override void RemoveScopes(ref string[] ScopeItem)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DiscoveryMode")]
        public override DiscoveryMode GetDiscoveryMode()
        {
            ConfStorageLoad();
            EventServerLoad();

            return ConfStorage.DiscoveryMode;

            EventServerSave();
            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDiscoveryMode(DiscoveryMode DiscoveryMode)
        {
            ConfStorageLoad();
            EventServerLoad();

            ConfStorage.DiscoveryMode = DiscoveryMode;

            EventServerSave();
            ConfStorageSave();
        }

        public override DiscoveryMode GetRemoteDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        public override void SetRemoteDiscoveryMode(DiscoveryMode RemoteDiscoveryMode)
        {
            throw new NotImplementedException();
        }

        public override NetworkHost[] GetDPAddresses()
        {
            throw new NotImplementedException();
        }

        public override void SetDPAddresses(NetworkHost[] DPAddress)
        {
            throw new NotImplementedException();
        }

        public override string GetEndpointReference(out XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override RemoteUser GetRemoteUser()
        {
            throw new NotImplementedException();
        }

        public override void SetRemoteUser(RemoteUser RemoteUser)
        {
            throw new NotImplementedException();
        }

        public override User[] GetUsers()
        {
            throw new NotImplementedException();
        }

        public override void CreateUsers(User[] User)
        {
            throw new NotImplementedException();
        }

        public override void DeleteUsers(string[] Username)
        {
            throw new NotImplementedException();
        }

        public override void SetUser(User[] User)
        {
            throw new NotImplementedException();
        }

        public override string GetWsdlUrl()
        {
            throw new NotImplementedException();
        }


        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities GetCapabilities([System.Xml.Serialization.XmlElementAttribute("Category")] CapabilityCategory[] Category)
        {
            Capabilities res = new Capabilities();
            string hostAndPort = HttpContext.Current.Request.Url.Authority;

            //Device Management Service
            if ((Category == null) || Category.Contains(CapabilityCategory.All) || Category.Contains(CapabilityCategory.Device))
            {
                res.Device = new DeviceCapabilities();
                res.Device.XAddr = "http://" + hostAndPort + "/ServiceDevice10/DeviceService.asmx";
            }

            //Event Service
            if ((Category == null) || Category.Contains(CapabilityCategory.All) || Category.Contains(CapabilityCategory.Events))
            {
                res.Events = new EventCapabilities();
                res.Events.XAddr = "http://" + hostAndPort + "/ServiceEvents10/EventService.asmx";
            }

            return res;
        }

        public override HostnameInformation GetHostname()
        {
            throw new NotImplementedException();
        }

        public override void SetHostname(string Name)
        {
            throw new NotImplementedException();
        }

        public override bool SetHostnameFromDHCP(bool FromDHCP)
        {
            throw new NotImplementedException();
        }

        public override DNSInformation GetDNS()
        {
            throw new NotImplementedException();
        }

        public override void SetDNS(bool FromDHCP, string[] SearchDomain, IPAddress[] DNSManual)
        {
            throw new NotImplementedException();
        }

        public override NTPInformation GetNTP()
        {
            throw new NotImplementedException();
        }

        public override void SetNTP(bool FromDHCP, NetworkHost[] NTPManual)
        {
            throw new NotImplementedException();
        }

        public override DynamicDNSInformation GetDynamicDNS()
        {
            throw new NotImplementedException();
        }

        public override void SetDynamicDNS(DynamicDNSType Type, string Name, string TTL)
        {
            throw new NotImplementedException();
        }

        public override NetworkInterface[] GetNetworkInterfaces()
        {
            throw new NotImplementedException();
        }

        public override bool SetNetworkInterfaces(string InterfaceToken, NetworkInterfaceSetConfiguration NetworkInterface)
        {
            throw new NotImplementedException();
        }

        public override NetworkProtocol[] GetNetworkProtocols()
        {
            throw new NotImplementedException();
        }

        public override void SetNetworkProtocols(NetworkProtocol[] NetworkProtocols)
        {
            throw new NotImplementedException();
        }

        public override NetworkGateway GetNetworkDefaultGateway()
        {
            throw new NotImplementedException();
        }

        public override void SetNetworkDefaultGateway(string[] IPv4Address, string[] IPv6Address)
        {
            throw new NotImplementedException();
        }

        public override NetworkZeroConfiguration GetZeroConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void SetZeroConfiguration(string InterfaceToken, bool Enabled)
        {
            throw new NotImplementedException();
        }

        public override IPAddressFilter GetIPAddressFilter()
        {
            throw new NotImplementedException();
        }

        public override void SetIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public override void AddIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public override void RemoveIPAddressFilter(IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public override BinaryData GetAccessPolicy()
        {
            throw new NotImplementedException();
        }

        public override void SetAccessPolicy(BinaryData PolicyFile)
        {
            throw new NotImplementedException();
        }

        public override Certificate CreateCertificate(string CertificateID, string Subject, System.DateTime ValidNotBefore, bool ValidNotBeforeSpecified, System.DateTime ValidNotAfter, bool ValidNotAfterSpecified)
        {
            throw new NotImplementedException();
        }

        public override Certificate[] GetCertificates()
        {
            throw new NotImplementedException();
        }

        public override CertificateStatus[] GetCertificatesStatus()
        {
            throw new NotImplementedException();
        }

        public override void SetCertificatesStatus(CertificateStatus[] CertificateStatus)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCertificates(string[] CertificateID)
        {
            throw new NotImplementedException();
        }

        public override BinaryData GetPkcs10Request(string CertificateID, string Subject, BinaryData Attributes)
        {
            throw new NotImplementedException();
        }

        public override void LoadCertificates(Certificate[] NVTCertificate)
        {
            throw new NotImplementedException();
        }

        public override bool GetClientCertificateMode()
        {
            throw new NotImplementedException();
        }

        public override void SetClientCertificateMode(bool Enabled)
        {
            throw new NotImplementedException();
        }

        public override RelayOutput[] GetRelayOutputs()
        {
            throw new NotImplementedException();
        }

        public override void SetRelayOutputSettings(string RelayOutputToken, RelayOutputSettings Properties)
        {
            throw new NotImplementedException();
        }

        public override void SetRelayOutputState(string RelayOutputToken, RelayLogicalState LogicalState)
        {
            throw new NotImplementedException();
        }

        public override string SendAuxiliaryCommand(string AuxiliaryCommand)
        {
            throw new NotImplementedException();
        }

        public override Certificate[] GetCACertificates()
        {
            throw new NotImplementedException();
        }

        public override void LoadCertificateWithPrivateKey(CertificateWithPrivateKey[] CertificateWithPrivateKey)
        {
            throw new NotImplementedException();
        }

        public override CertificateInformation GetCertificateInformation(string CertificateID)
        {
            throw new NotImplementedException();
        }

        public override void LoadCACertificates(Certificate[] CACertificate)
        {
            throw new NotImplementedException();
        }

        public override void CreateDot1XConfiguration(Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        public override void SetDot1XConfiguration(Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        public override Dot1XConfiguration GetDot1XConfiguration(string Dot1XConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override Dot1XConfiguration[] GetDot1XConfigurations()
        {
            throw new NotImplementedException();
        }

        public override void DeleteDot1XConfiguration(string[] Dot1XConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override Dot11Capabilities GetDot11Capabilities(XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override Dot11Status GetDot11Status(string InterfaceToken)
        {
            throw new NotImplementedException();
        }

        public override Dot11AvailableNetworks[] ScanAvailableDot11Networks(string InterfaceToken)
        {
            throw new NotImplementedException();
        }

        public override SystemLogUri[] GetSystemUris(out string SupportInfoUri, out string SystemBackupUri, out GetSystemUrisResponseExtension Extension)
        {
            throw new NotImplementedException();
        }

        public override string StartFirmwareUpgrade(out string UploadDelay, out string ExpectedDownTime)
        {
            throw new NotImplementedException();
        }

        public override string StartSystemRestore(out string ExpectedDownTime)
        {
            throw new NotImplementedException();
        }
    }
}
