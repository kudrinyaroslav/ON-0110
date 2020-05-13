using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.WithLogic.Proxy;
using DUT.WithLogic.Base;
using DUT.WithLogic.Engine;

namespace DUT.WithLogic.Services.DeviceManagement
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DeviceBinding", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeviceEntity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensibleDocumented))]
    public class ONVIFDeviceManagement : DeviceBinding
    {

        protected ONVIFDeviceManagementConfiguration ONVIFDeviceManagementConfiguration
        {
            get
            {
                if (Application[AppVars.ONVIFDEVICEMANAGEMENTCONFIGURATION] != null)
                {
                    return (ONVIFDeviceManagementConfiguration)Application[AppVars.ONVIFDEVICEMANAGEMENTCONFIGURATION];
                }
                else
                {
                    ONVIFDeviceManagementConfiguration onvifDeviceManagementConfiguration = ONVIFDeviceManagementConfiguration.Load();
                    Application[AppVars.ONVIFDEVICEMANAGEMENTCONFIGURATION] = onvifDeviceManagementConfiguration;
                    return onvifDeviceManagementConfiguration;
                }
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        public void SaveConfiguration()
        {
            ONVIFDeviceManagementConfiguration.Serialize();
        }

        [System.Web.Services.WebMethodAttribute()]
        public void SaveCapabilities()
        {
            ONVIFDeviceManagementCapabilities.Serialize();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServices", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Service")]
        public override Service[] GetServices(bool IncludeCapability)
        {
            Service[] res;
            if (IncludeCapability)
            {
                res = ONVIFServiceList.ServicesWithCapabilities;
            }
            else
            {
                res = ONVIFServiceList.ServicesWithoutCapabilities;
            }
            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override DeviceServiceCapabilities GetServiceCapabilities()
        {
            return ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDeviceInformation", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Manufacturer")]
        public override string GetDeviceInformation(out string Model, out string FirmwareVersion, out string SerialNumber, out string HardwareId)
        {
            FirmwareVersion = ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FirmwareVersion;
            Model = ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Model;
            SerialNumber = ONVIFConfiguration.ONVIFDeviceManagementCapabilities.SerialNumber;
            HardwareId = ONVIFConfiguration.ONVIFDeviceManagementCapabilities.HardwareId;
            return ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Manufactorer;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetSystemDateAndTime", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSystemDateAndTime(SetDateTimeType DateTimeType, bool DaylightSavings, Proxy.TimeZone TimeZone, Proxy.DateTime UTCDateTime)
        {
            if (UTCDateTime != null)
            {
                try
                {
                    ONVIFDeviceManagementConfiguration.TimeOfset = DUTDataTime.GetOffset(UTCDateTime);
                }
                catch (Exception e)
                {
                    throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidDateTime(e);
                }
            }
            if ((TimeZone != null) && (TimeZone.TZ != null))
            {
                //TODO: "Sender/InvalidArgVal/InvalidTimeZone" fault is expected, but no SOAP fault returned
                ONVIFDeviceManagementConfiguration.TimeZone = TimeZone.TZ;
            }
            ONVIFDeviceManagementConfiguration.DaylightSavings = DaylightSavings;
            ONVIFDeviceManagementConfiguration.DateTimeType = DateTimeType;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemDateAndTime", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SystemDateAndTime")]
        public override SystemDateTime GetSystemDateAndTime()
        {
            SystemDateTime res = new SystemDateTime();

            if (ONVIFDeviceManagementConfiguration.DateTimeType == SetDateTimeType.Manual)
            {
                res.UTCDateTime = DUTDataTime.GetCurrentTime(ONVIFDeviceManagementConfiguration.TimeOfset);
            }
            else
            {
                res.UTCDateTime = DUTDataTime.GetCurrentTime(0);
            }
            res.TimeZone = new Proxy.TimeZone();
            res.TimeZone.TZ = ONVIFDeviceManagementConfiguration.TimeZone;
            res.DaylightSavings = ONVIFDeviceManagementConfiguration.DaylightSavings;
            res.DateTimeType = ONVIFDeviceManagementConfiguration.DateTimeType;

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetSystemFactoryDefault", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSystemFactoryDefault(FactoryDefaultType FactoryDefault)
        {
            if ((ONVIFDeviceManagementConfiguration.DiscoveryMode == DiscoveryMode.Discoverable) &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.DiscoveryByeSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.DiscoveryBye)
            {
                //TODO: Send Bye
            }
            if (FactoryDefault == FactoryDefaultType.Soft)
            {
                ONVIFDeviceManagementConfiguration onvifDeviceManagementConfiguration = ONVIFDeviceManagementConfiguration.Load();
                Application[AppVars.ONVIFDEVICEMANAGEMENTCONFIGURATION] = onvifDeviceManagementConfiguration;
            }
            else
            {
                ONVIFDeviceManagementConfiguration onvifDeviceManagementConfiguration = ONVIFDeviceManagementConfiguration.HardReset();
                Application[AppVars.ONVIFDEVICEMANAGEMENTCONFIGURATION] = onvifDeviceManagementConfiguration;
            }

            if (ONVIFDeviceManagementConfiguration.DiscoveryMode == DiscoveryMode.Discoverable)
            {
                //TODO: Send Hello
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/UpgradeSystemFirmware", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Message")]
        public override string UpgradeSystemFirmware(AttachmentData Firmware)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.FirmwareUpgradeSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.FirmwareUpgrade)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SystemReboot", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Message")]
        public override string SystemReboot()
        {
            if ((ONVIFDeviceManagementConfiguration.DiscoveryMode == DiscoveryMode.Discoverable) &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.DiscoveryByeSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.DiscoveryBye)
            {
                //TODO: Send Bye
            }
            if (ONVIFDeviceManagementConfiguration.DiscoveryMode == DiscoveryMode.Discoverable)
            {
                //TODO: Send Hello
            }
            return "Reboot will be started in 5 secunds.";
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/RestoreSystem", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RestoreSystem([System.Xml.Serialization.XmlElementAttribute("BackupFiles")] BackupFile[] BackupFiles)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.SystemBackupSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.SystemBackup)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemBackup", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("BackupFiles")]
        public override BackupFile[] GetSystemBackup()
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.SystemBackupSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.SystemBackup)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemLog", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SystemLog")]
        public override SystemLog GetSystemLog(SystemLogType LogType)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.SystemLoggingSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.SystemLogging)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetSystemSupportInformation", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SupportInformation")]
        public override SupportInformation GetSystemSupportInformation()
        {
            SupportInformation res = new SupportInformation();
            res.String = ONVIFConfiguration.ONVIFDeviceManagementCapabilities.ToString();
            res.Binary = new AttachmentData();
            //TODO: Binary

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Scopes")]
        public override Scope[] GetScopes()
        {
            List<Scope> res = new List<Scope>();
            res.AddRange(ONVIFDeviceManagementConfiguration.ConfigurableScopes);
            res.AddRange(ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FixedScopes);
            return res.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetScopes([System.Xml.Serialization.XmlElementAttribute("Scopes", DataType = "anyURI")] string[] Scopes)
        {
            ONVIFDeviceManagementConfiguration.ConfigurableScopes.Clear();
            foreach (string scope in Scopes)
            {
                ONVIFDeviceManagementConfiguration.ConfigurableScopes.Add(ONVIFDeviceManagementConfiguration.CreateConfigurableScope(scope));
            }
            if (ONVIFDeviceManagementConfiguration.DiscoveryMode == DiscoveryMode.Discoverable)
            {
                //TODO: Send Hello
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/AddScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddScopes([System.Xml.Serialization.XmlElementAttribute("ScopeItem", DataType = "anyURI")] string[] ScopeItem)
        {
            foreach (string scope in ScopeItem)
            {
                ONVIFDeviceManagementConfiguration.ConfigurableScopes.Add(ONVIFDeviceManagementConfiguration.CreateConfigurableScope(scope));
            }
            if (ONVIFDeviceManagementConfiguration.DiscoveryMode == DiscoveryMode.Discoverable)
            {
                //TODO: Send Hello
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/RemoveScopes", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveScopes([System.Xml.Serialization.XmlElementAttribute("ScopeItem", DataType = "anyURI")] ref string[] ScopeItem)
        {
            foreach (string scope in ScopeItem)
            {
                if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FixedScopes.Any(C => C.ScopeItem == scope))
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_FixedScope();
                }

                if (!(ONVIFDeviceManagementConfiguration.ConfigurableScopes.Any(C => C.ScopeItem == scope)))
                {
                    throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_NoScope();
                }
            }

            List<string> ScopeList = new List<string>();
            ScopeList.AddRange(ScopeItem);

            ONVIFDeviceManagementConfiguration.ConfigurableScopes = ONVIFDeviceManagementConfiguration.ConfigurableScopes.FindAll(
                delegate(Scope scope)
                {
                    return !ScopeList.Contains(scope.ScopeItem);
                }
            );

            if (ONVIFDeviceManagementConfiguration.DiscoveryMode == DiscoveryMode.Discoverable)
            {
                //TODO: Send Hello
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DiscoveryMode")]
        public override DiscoveryMode GetDiscoveryMode()
        {
            return ONVIFDeviceManagementConfiguration.DiscoveryMode;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDiscoveryMode(DiscoveryMode DiscoveryMode)
        {
            ONVIFDeviceManagementConfiguration.DiscoveryMode = DiscoveryMode;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetRemoteDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RemoteDiscoveryMode")]
        public override DiscoveryMode GetRemoteDiscoveryMode()
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscoverySpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscovery)
            {
                return ONVIFDeviceManagementConfiguration.RemoteDiscoveryMode;
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetRemoteDiscoveryMode", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRemoteDiscoveryMode(DiscoveryMode RemoteDiscoveryMode)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscoverySpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscovery)
            {
                ONVIFDeviceManagementConfiguration.RemoteDiscoveryMode = RemoteDiscoveryMode;
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDPAddresses", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DPAddress")]
        public override NetworkHost[] GetDPAddresses()
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscoverySpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscovery)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDPAddresses", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDPAddresses([System.Xml.Serialization.XmlElementAttribute("DPAddress")] NetworkHost[] DPAddress)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscoverySpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.System.RemoteDiscovery)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetEndpointReference", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("GUID")]
        public override string GetEndpointReference([System.Xml.Serialization.XmlAnyElementAttribute()] out System.Xml.XmlElement[] Any)
        {
            Any = null;
            return ONVIFConfiguration.ONVIFDeviceManagementCapabilities.EndpointReference;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetRemoteUser", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RemoteUser")]
        public override RemoteUser GetRemoteUser()
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.RemoteUserHandlingSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.RemoteUserHandling)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetRemoteUser", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRemoteUser(RemoteUser RemoteUser)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.RemoteUserHandlingSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.RemoteUserHandling)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("User")]
        public override User[] GetUsers()
        {
            List<User> res = new List<User>();

            res.AddRange(ONVIFDeviceManagementConfiguration.NotFixedUsers.Values.Select(C => C.CloneWithoutPassword()));
            res.AddRange(ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FixedUsers.Values.Select(C => C.CloneWithoutPassword()));

            return res.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/CreateUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void CreateUsers([System.Xml.Serialization.XmlElementAttribute("User")] User[] User)
        {
            //Check MaxUsers
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FixedUsers.Count +
                ONVIFDeviceManagementConfiguration.NotFixedUsers.Count +
                User.Count() > ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.MaxUsers)
            {
                throw ONVIFFault.GetDeviceManagementException_Action_TooManyUsers();
            }


            //Check if user list form request contains users with equal names
            if (User.GroupBy(n => n.Username).Any(g => g.Count() > 1))
            {
                throw ONVIFFault.GetDeviceManagementException_OperationProhibited_UsernameClash();
            }

            foreach (User user in User)
            {
                //Check that The password is too long
                if (user.Password.Length > ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.MaxPasswordLength)
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_UsernameClash();
                }

                //Check that The username is too long
                if (user.Username.Length > ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.MaxUserNameLength)
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_UsernameTooLong();
                }

                //Check that Username already exists
                if ((ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FixedUsers.ContainsKey(user.Username)) ||
                    (ONVIFDeviceManagementConfiguration.NotFixedUsers.ContainsKey(user.Username)))
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_UsernameClash();
                }

                //Check that the password is too weak
                if (string.IsNullOrEmpty(user.Password))
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_Password();
                }

                //Check that the username is too short 
                if (string.IsNullOrEmpty(user.Username))
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_UsernameTooShort();
                }
            }

            //add users to not fixed list
            foreach (User user in User)
            {
                ONVIFDeviceManagementConfiguration.NotFixedUsers.Add(user.Username, user);
            }

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/DeleteUsers", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteUsers([System.Xml.Serialization.XmlElementAttribute("Username")] string[] Username)
        {

            foreach (string username in Username)
            {
                //check if username is fixed
                if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FixedUsers.ContainsKey(username))
                {
                    throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_FixedUser();
                }

                //check if username exists
                if (!ONVIFDeviceManagementConfiguration.NotFixedUsers.ContainsKey(username))
                {
                    throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_UsernameMissing();
                }
            }

            foreach (string username in Username)
            {
                ONVIFDeviceManagementConfiguration.NotFixedUsers.Remove(username);
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetUser", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetUser([System.Xml.Serialization.XmlElementAttribute("User")] User[] User)
        {
            foreach (User user in User)
            {
                //check if username is fixed
                if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.FixedUsers.ContainsKey(user.Username))
                {
                    throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_FixedUser();
                }

                //check if username exists
                if (!ONVIFDeviceManagementConfiguration.NotFixedUsers.ContainsKey(user.Username))
                {
                    throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_UsernameMissing();
                }

                //check if the password is too long
                if (user.Password.ToString().Length > ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Security.MaxPasswordLength)
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_UsernameClash();
                }

                //check if the password is too weak
                if (user.Password.ToString().Length == 0)
                {
                    throw ONVIFFault.GetDeviceManagementException_OperationProhibited_Password();
                }
            }

            foreach (User user in User)
            {
                ONVIFDeviceManagementConfiguration.NotFixedUsers[user.Username].UserLevel = user.UserLevel;
                ONVIFDeviceManagementConfiguration.NotFixedUsers[user.Username].Password = user.Password;
                ONVIFDeviceManagementConfiguration.NotFixedUsers[user.Username].Extension = user.Extension;
                ONVIFDeviceManagementConfiguration.NotFixedUsers[user.Username].AnyAttr = user.AnyAttr;
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetWsdlUrl", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("WsdlUrl", DataType = "anyURI")]
        public override string GetWsdlUrl()
        {
            return ONVIFConfiguration.ONVIFDeviceManagementCapabilities.WsdlUri;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetCapabilities", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities1 GetCapabilities([System.Xml.Serialization.XmlElementAttribute("Category")] CapabilityCategory[] Category)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.GetCapabilitiesSupport)
            {
                Capabilities1 res = new Capabilities1();
                if ((Category == null) || (Category.Contains(CapabilityCategory.All)))
                {
                    res.Device = ONVIFConfiguration.ONVIFDeviceManagementCapabilities.DeviceOldCapabilities;
                    res.Device.XAddr = ONVIFServiceList.DeviceManagementUri;
                }
                else
                {

                    if (Category.Contains(CapabilityCategory.Device))
                    {
                        res.Device = ONVIFConfiguration.ONVIFDeviceManagementCapabilities.DeviceOldCapabilities;
                        res.Device.XAddr = ONVIFServiceList.DeviceManagementUri;
                    }

                    if (Category.Contains(CapabilityCategory.Events))
                    {
                        throw ONVIFFault.GetDeviceManagementException_ActionNotSupported_NoSuchService();
                    }

                    if (Category.Contains(CapabilityCategory.Analytics))
                    {
                        throw ONVIFFault.GetDeviceManagementException_ActionNotSupported_NoSuchService();
                    }

                    if (Category.Contains(CapabilityCategory.Imaging))
                    {
                        throw ONVIFFault.GetDeviceManagementException_ActionNotSupported_NoSuchService();
                    }

                    if (Category.Contains(CapabilityCategory.Media))
                    {
                        throw ONVIFFault.GetDeviceManagementException_ActionNotSupported_NoSuchService();
                    }

                    if (Category.Contains(CapabilityCategory.PTZ))
                    {
                        throw ONVIFFault.GetDeviceManagementException_ActionNotSupported_NoSuchService();
                    }
                }

                return res;

            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetHostname", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("HostnameInformation")]
        public override HostnameInformation GetHostname()
        {
            HostnameInformation res;

            if ((ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.HostnameFromDHCPSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.HostnameFromDHCP) &&
                (ONVIFDeviceManagementConfiguration.HostnameInformation.FromDHCP ||
                 String.IsNullOrEmpty(ONVIFDeviceManagementConfiguration.HostnameInformation.Name))) //TODO: && NetworkConfiguration use DHCP
            {
                res = new HostnameInformation();
                res.FromDHCP = ONVIFDeviceManagementConfiguration.HostnameInformation.FromDHCP;
                res.Name = ONVIFDeviceManagementConfiguration.HostnameFromDHCP;
            }
            else
            {
                res = ONVIFDeviceManagementConfiguration.HostnameInformation;
            }

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetHostname", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetHostname([System.Xml.Serialization.XmlElementAttribute(DataType = "token")] string Name)
        {
            if ((Uri.CheckHostName(Name) == UriHostNameType.Dns) &&
                !Name.Contains('_'))
            {
                ONVIFDeviceManagementConfiguration.HostnameInformation.Name = Name;
            }
            else
            {
                throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidHostname();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetHostnameFromDHCP", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RebootNeeded")]
        public override bool SetHostnameFromDHCP(bool FromDHCP)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.HostnameFromDHCPSpecified &&
               ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.HostnameFromDHCP)
            {
                ONVIFDeviceManagementConfiguration.HostnameInformation.FromDHCP = FromDHCP;
                return false;
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDNS", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DNSInformation")]
        public override DNSInformation GetDNS()
        {
            return ONVIFDeviceManagementConfiguration.DNSInformation;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDNS", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDNS(bool FromDHCP, [System.Xml.Serialization.XmlElementAttribute("SearchDomain", DataType = "token")] string[] SearchDomain, [System.Xml.Serialization.XmlElementAttribute("DNSManual")] IPAddress[] DNSManual)
        {
            System.Net.IPAddress address;
            foreach (IPAddress ip in DNSManual)
            {
                if (ip.Type == IPType.IPv4)
                {
                    if (String.IsNullOrEmpty(ip.IPv4Address) ||
                        (ip.IPv6Address != null) ||
                        !(System.Net.IPAddress.TryParse(ip.IPv4Address, out address)) ||
                        (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidIPv4Address();
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(ip.IPv6Address) ||
                        (ip.IPv4Address != null) ||
                        !(System.Net.IPAddress.TryParse(ip.IPv6Address, out address)) ||
                        (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6))
                    {
                        throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidIPv6Address();
                    }
                }
            }

            ONVIFDeviceManagementConfiguration.DNSInformation.FromDHCP = FromDHCP;
            ONVIFDeviceManagementConfiguration.DNSInformation.SearchDomain = SearchDomain;
            ONVIFDeviceManagementConfiguration.DNSInformation.DNSManual = DNSManual;
            if (FromDHCP)
            {
                ONVIFDeviceManagementConfiguration.DNSInformation.DNSFromDHCP = ONVIFDeviceManagementConfiguration.DNSFromDHCP.ToArray();
            }
            else
            {
                ONVIFDeviceManagementConfiguration.DNSInformation.DNSFromDHCP = null;
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNTP", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NTPInformation")]
        public override NTPInformation GetNTP()
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.NTPSpecified &&
                (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.NTP > 0))
            {
                return ONVIFDeviceManagementConfiguration.NTPInformation;
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNTP", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetNTP(bool FromDHCP, [System.Xml.Serialization.XmlElementAttribute("NTPManual")] NetworkHost[] NTPManual)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.NTPSpecified &&
                (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.NTP > 0))
            {
                if (NTPManual.Count() > ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.NTP)
                {
                    throw ONVIFFault.GetGeneralException_InvalidArgVal("Number of NTP servers exeeded.");
                }

                if ((ONVIFDeviceManagementConfiguration.DateTimeType == SetDateTimeType.NTP) &&
                    ((NTPManual == null) ||
                    (NTPManual.Count() == 0)) &&
                    !FromDHCP)
                {
                    throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_TimeSyncedToNtp();
                }

                System.Net.IPAddress address;
                foreach (NetworkHost networkHost in NTPManual)
                {
                    switch (networkHost.Type)
                    {
                        case NetworkHostType.IPv4:
                            if (String.IsNullOrEmpty(networkHost.IPv4Address) ||
                                (networkHost.IPv6Address != null) ||
                                (networkHost.DNSname != null) ||
                                !(System.Net.IPAddress.TryParse(networkHost.IPv4Address, out address)) ||
                                (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork))
                            {
                                throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidIPv4Address();
                            }
                            break;
                        case NetworkHostType.IPv6:
                            if (String.IsNullOrEmpty(networkHost.IPv6Address) ||
                                (networkHost.IPv4Address != null) ||
                                (networkHost.DNSname != null) ||
                                !(System.Net.IPAddress.TryParse(networkHost.IPv6Address, out address)) ||
                                (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6))
                            {
                                throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidIPv6Address();
                            }
                            break;
                        case NetworkHostType.DNS:
                            if (Uri.CheckHostName(networkHost.DNSname) != UriHostNameType.Dns)
                            {
                                throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidHostname();
                            }
                            break;
                    }
                }


                ONVIFDeviceManagementConfiguration.NTPInformation.FromDHCP = FromDHCP;
                ONVIFDeviceManagementConfiguration.NTPInformation.NTPManual = NTPManual;
                if (FromDHCP)
                {
                    ONVIFDeviceManagementConfiguration.NTPInformation.NTPFromDHCP = ONVIFDeviceManagementConfiguration.NTPFromDHCP.ToArray();
                }
                else
                {
                    ONVIFDeviceManagementConfiguration.NTPInformation.NTPFromDHCP = null;
                }
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetDynamicDNS", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DynamicDNSInformation")]
        public override DynamicDNSInformation GetDynamicDNS()
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.DynDNSSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.DynDNS)
            {
                return ONVIFDeviceManagementConfiguration.DynamicDNSInformation;
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetDynamicDNS", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDynamicDNS(DynamicDNSType Type, [System.Xml.Serialization.XmlElementAttribute(DataType = "token")] string Name, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string TTL)
        {
            if (ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.DynDNSSpecified &&
                ONVIFConfiguration.ONVIFDeviceManagementCapabilities.Capabilities.Network.DynDNS)
            {
                if (Uri.CheckHostName(Name) != UriHostNameType.Dns)
                {
                    throw ONVIFFault.GetGeneralException_InvalidArgVal("Invalid Name.");
                }

                ONVIFDeviceManagementConfiguration.DynamicDNSInformation.Name = Name;
                ONVIFDeviceManagementConfiguration.DynamicDNSInformation.TTL = TTL;
                ONVIFDeviceManagementConfiguration.DynamicDNSInformation.Type = Type;
            }
            else
            {
                throw ONVIFFault.GetGeneralException_ActionNotSupported();
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkInterfaces", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkInterfaces")]
        public override NetworkInterface[] GetNetworkInterfaces()
        {
            return ONVIFDeviceManagementConfiguration.NetworkInterface.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkInterfaces", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RebootNeeded")]
        public override bool SetNetworkInterfaces(string InterfaceToken, NetworkInterfaceSetConfiguration NetworkInterface)
        {
            if (!ONVIFDeviceManagementConfiguration.NetworkInterface.Any(C => C.token == InterfaceToken))
            {
                throw ONVIFFault.GetDeviceManagementException_InvalidArgVal_InvalidNetworkInterface(InterfaceToken);
            }
            
            //TODO validation
            //TODO real address change

            ONVIFDeviceManagementConfiguration.SetNetworkInterface(InterfaceToken, NetworkInterface);

            return false;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/GetNetworkProtocols", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NetworkProtocols")]
        public override NetworkProtocol[] GetNetworkProtocols()
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/device/wsdl/SetNetworkProtocols", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetNetworkProtocols([System.Xml.Serialization.XmlElementAttribute("NetworkProtocols")] NetworkProtocol[] NetworkProtocols)
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

        public override StorageConfiguration[] GetStorageConfigurations()
        {
            throw new NotImplementedException();
        }

        public override string CreateStorageConfiguration(StorageConfigurationData StorageConfiguration)
        {
            throw new NotImplementedException();
        }

        public override StorageConfiguration GetStorageConfiguration(string Token)
        {
            throw new NotImplementedException();
        }

        public override void SetStorageConfiguration(StorageConfiguration StorageConfiguration)
        {
            throw new NotImplementedException();
        }

        public override void DeleteStorageConfiguration(string Token)
        {
            throw new NotImplementedException();
        }
    }
}
