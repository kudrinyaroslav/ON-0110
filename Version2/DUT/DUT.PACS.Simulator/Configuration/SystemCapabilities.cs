using System.Collections.Generic;
using System.Xml;
using DUT.PACS.Simulator.Device10;
using DUT.PACS.Simulator.ServiceAccessControl10;
using DUT.PACS.Simulator.ServiceDoorControl10;
using System.Xml.Serialization;
using System.Text;

namespace DUT.PACS.Simulator
{
    /// <summary>
    /// Serialization-friendly capabilities storage.
    /// </summary>
    public class CapabilitiesWrapper
    {
        public CapabilitiesWrapper()
        {
        }

        [System.Xml.Serialization.XmlElement(Namespace = "http://www.onvif.org/ver10/device/wsdl", ElementName = "Capabilities")]
        public DeviceServiceCapabilities DeviceServiceCapabilities { get;  set; }

        [System.Xml.Serialization.XmlElement(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ElementName = "Capabilities")]
        public ServiceDoorControl10.ServiceCapabilities DoorServiceCapabilities { get; set; }

        [System.Xml.Serialization.XmlElement(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ElementName = "Capabilities")]
        public ServiceAccessControl10.ServiceCapabilities AccessControlCapabilities { get; set; }

        [System.Xml.Serialization.XmlElement(Namespace = "http://www.onvif.org/ver10/credential/wsdl", ElementName = "Capabilities")]
        public ServiceCredential10.ServiceCapabilities CredentialCapabilities { get; set; }

        [System.Xml.Serialization.XmlElement(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl", ElementName = "Capabilities")]
        public ServiceAccessRules10.ServiceCapabilities AccessRulesCapabilities { get; set; }
    }

    /// <summary>
    /// System capabilities
    /// </summary>
    public class SystemCapabilities
    {
        private static SystemCapabilities _instance;
        public static  SystemCapabilities Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SystemCapabilities();
                }
                return _instance;
            }
        }

        private SystemCapabilities()
        {
            // don't change service capabilities in other places to maintain synchronization of 
            // typed properties and XmlElement's

            //Device Management Service
            DeviceServiceCapabilities = new DeviceServiceCapabilities();
            DeviceServiceCapabilities.Network = new NetworkCapabilities();
            DeviceServiceCapabilities.Security = new SecurityCapabilities();
            DeviceServiceCapabilities.System = new Device10.SystemCapabilities();

            //Door Control Service
            DoorServiceCapabilities = new ServiceDoorControl10.ServiceCapabilities();
            DoorServiceCapabilities.MaxLimit = 7;

            //Access Control Service
            AccessControlCapabilities = new ServiceAccessControl10.ServiceCapabilities();
            //AccessControlCapabilities.DisableAccessPointSpecified = true;
            //AccessControlCapabilities.DisableAccessPoint = true;
            AccessControlCapabilities.MaxLimit = 1;

            //Credential Service
            CredentialCapabilities = new ServiceCredential10.ServiceCapabilities();
            CredentialCapabilities.MaxLimit = 3;
            CredentialCapabilities.MaxCredentials = 10;
            CredentialCapabilities.CredentialAccessProfileValiditySupported = true;
            CredentialCapabilities.CredentialValiditySupported = true;
            CredentialCapabilities.MaxAccessProfilesPerCredential = 5;
            CredentialCapabilities.ResetAntipassbackSupported = true;
            CredentialCapabilities.SupportedIdentifierType = new string[] {"ONVIFCard", "ONVIFPIN"};

            //Supported Format Types (probably refactoring requiered to be flexible)
            List<ServiceCredential10.CredentialIdentifierFormatTypeInfo> formatTypes;
            ServiceCredential10.CredentialIdentifierFormatTypeInfo credentialIdentifierFormatTypeInfo;

            SupportedFormatTypes = new Dictionary<string, List<ServiceCredential10.CredentialIdentifierFormatTypeInfo>>();
            
            formatTypes = new List<ServiceCredential10.CredentialIdentifierFormatTypeInfo>();

            credentialIdentifierFormatTypeInfo = new ServiceCredential10.CredentialIdentifierFormatTypeInfo();
            credentialIdentifierFormatTypeInfo.FormatType = "WIEGAND26";
            credentialIdentifierFormatTypeInfo.Description = "Standard 26 bit Wiegand format as defined by SIA standard (SIA AC-01).";
            formatTypes.Add(credentialIdentifierFormatTypeInfo);
            credentialIdentifierFormatTypeInfo = new ServiceCredential10.CredentialIdentifierFormatTypeInfo();
            credentialIdentifierFormatTypeInfo.FormatType = "WIEGAND37";
            credentialIdentifierFormatTypeInfo.Description = "Description";
            formatTypes.Add(credentialIdentifierFormatTypeInfo);


            SupportedFormatTypes.Add("ONVIFCard", formatTypes);

            formatTypes = new List<ServiceCredential10.CredentialIdentifierFormatTypeInfo>();

            credentialIdentifierFormatTypeInfo = new ServiceCredential10.CredentialIdentifierFormatTypeInfo();
            credentialIdentifierFormatTypeInfo.FormatType = "WIEGAND37";
            credentialIdentifierFormatTypeInfo.Description = "Description";
            formatTypes.Add(credentialIdentifierFormatTypeInfo);
            SupportedFormatTypes.Add("ONVIFPIN", formatTypes);

            //Access Rules Service
            AccessRulesCapabilities = new ServiceAccessRules10.ServiceCapabilities();
            AccessRulesCapabilities.MaxLimit = 2;
            AccessRulesCapabilities.MaxAccessPoliciesPerAccessProfile = 2;
            AccessRulesCapabilities.MaxAccessProfiles = 4;
            AccessRulesCapabilities.MultipleSchedulesPerAccessPointSupported = true;

            InitializeXmlElements();
        }

        public DeviceServiceCapabilities DeviceServiceCapabilities { get; private set; }
        public ServiceDoorControl10.ServiceCapabilities DoorServiceCapabilities { get; private set; }
        public ServiceAccessControl10.ServiceCapabilities AccessControlCapabilities { get; private set; }
        public ServiceCredential10.ServiceCapabilities CredentialCapabilities { get; private set; }
        public ServiceAccessRules10.ServiceCapabilities AccessRulesCapabilities { get; private set; }

        public Dictionary<string, List<ServiceCredential10.CredentialIdentifierFormatTypeInfo>> SupportedFormatTypes { get; private set; }

        private void InitializeXmlElements()
        {
            XmlDocument doc = new XmlDocument();
            
            StringBuilder sb = new StringBuilder();
            XmlSerializer ser = new XmlSerializer(typeof(CapabilitiesWrapper));

            XmlWriter sw = XmlWriter.Create(sb);
            
            CapabilitiesWrapper cw = new CapabilitiesWrapper();
            cw.AccessControlCapabilities = AccessControlCapabilities;
            cw.DeviceServiceCapabilities = DeviceServiceCapabilities;
            cw.DoorServiceCapabilities = DoorServiceCapabilities;
            cw.CredentialCapabilities = CredentialCapabilities;
            cw.AccessRulesCapabilities = AccessRulesCapabilities;
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

    }
}
