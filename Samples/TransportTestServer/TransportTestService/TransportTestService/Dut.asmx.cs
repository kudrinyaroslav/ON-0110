using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TransportTestService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : DeviceBinding
    {

        const string WRONGRESPONSE = "WRONG RESPONSE";

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNTP", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NTPInformation")]
        [XmlReplySubstituteExtensionAttribute("Hello World!")]
        public override NTPInformation GetNTP()
        {
            
            return new NTPInformation();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtensionAttribute(WRONGRESPONSE)]
        public override Capabilities GetCapabilities(CapabilityCategory[] Category)
        {
            return new Capabilities();
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetHostname", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("HostnameInformation")]
        [XmlReplySubstituteExtensionAttribute(WRONGRESPONSE)]
        public override HostnameInformation GetHostname()
        {
            return new HostnameInformation();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemLog", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SystemLog")]
        [XmlReplySubstituteExtensionAttribute("Hello World!")]
        public override SystemLog GetSystemLog(SystemLogType LogType)
        {
            return new SystemLog();
        }




        public override Service[] GetServices(bool IncludeCapability)
        {
            throw new NotImplementedException();
        }

        public override DeviceServiceCapabilities GetServiceCapabilities()
        {
            throw new NotImplementedException();
        }

        public override string GetDeviceInformation(out string Model, out string FirmwareVersion, out string SerialNumber, out string HardwareId)
        {
            throw new NotImplementedException();
        }

        public override void SetSystemDateAndTime(SetDateTimeType DateTimeType, bool DaylightSavings, TimeZone TimeZone, DateTime UTCDateTime)
        {
            throw new NotImplementedException();
        }

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


        public override SupportInformation GetSystemSupportInformation()
        {
            throw new NotImplementedException();
        }

        public override Scope[] GetScopes()
        {
            throw new NotImplementedException();
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

        public override DiscoveryMode GetDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        public override void SetDiscoveryMode(DiscoveryMode DiscoveryMode)
        {
            throw new NotImplementedException();
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

        public override string GetEndpointReference(out System.Xml.XmlElement[] Any)
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

        public override Dot11Capabilities GetDot11Capabilities(System.Xml.XmlElement[] Any)
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
