using System;
using System.Collections.Generic;
using System.Linq;
using DUT.PACS.Simulator.ServiceAccessRules10;
using DUT.PACS.Simulator.Common;

namespace DUT.PACS.Simulator.ServiceAccessRules10
{
    /// <summary>
    /// Summary description for Credential Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "AccessRulesBinding", Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class AccessRulesService : AccessRulesBinding
    {

        #region Members



        #endregion //Members


        /***************************************************************************************/

        #region Events

        public AccessRulesService()
        {
            ConfStorageLoad();
            EventServerLoad();

            EventServer.AccessRulesService = this;

            EventServerSave();
            ConfStorageSave();
        }



        #endregion //Events

        #region Sensors



        #endregion

        /***************************************************************************************/



        #region WebMethods

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessRulesCapabilities;
            return capabilities;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfo", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessProfileInfo")]
        public override AccessProfileInfo[] GetAccessProfileInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessRulesCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgVal", "TooManyItems" });


            return Array.ConvertAll(GetListByTokenList<AccessProfile>(Token, C => C.AccessProfileList), item => ToAccessProfileInfo(item));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfoList", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAccessProfileInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AccessProfileInfo")] out AccessProfileInfo[] AccessProfileInfo)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessRulesCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            AccessProfileInfo = Array.ConvertAll(GetList<AccessProfile>(offset, true, Limit, true, C => C.AccessProfileList), item => ToAccessProfileInfo(item));
            string newStartReferense = null;
            if (offset + AccessProfileInfo.Length < ConfStorage.AccessProfileList.Count)
            {
                newStartReferense = Convert.ToString(offset + AccessProfileInfo.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessProfile")]
        public override AccessProfile[] GetAccessProfiles([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessRulesCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgVal", "TooManyItems" });
            return GetListByTokenList<AccessProfile>(Token, C => C.AccessProfileList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileList", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAccessProfileList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AccessProfile")] out AccessProfile[] AccessProfile)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessRulesCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            AccessProfile = GetList<AccessProfile>(offset, true, Limit, true, C => C.AccessProfileList);
            string newStartReferense = null;
            if (offset + AccessProfile.Length < ConfStorage.AccessProfileList.Count)
            {
                newStartReferense = Convert.ToString(offset + AccessProfile.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/CreateAccessProfile", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateAccessProfile(AccessProfile AccessProfile)
        {
            ConfStorageLoad();
            EventServerLoad();

            if (AccessProfile.token == "")
            {
                int i = 1;

                AccessProfile.token = "accessprofile" + i.ToString();

                while (ConfStorage.AccessProfileList.Keys.Contains(AccessProfile.token))
                {
                    AccessProfile.token = "accessprofile" + i.ToString();
                    i++;
                }
            }

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessRulesCapabilities;
            string res = AccessProfile.token;

            //Check MaxAccessProfiles capability
            if (ConfStorage.AccessProfileList.Count() >= capabilities.MaxAccessProfiles)
            {
                string message = string.Format("There is not enough space to create new access profile, see the MaxAccessProfiles capability.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxAccessProfiles" });
            }

            //Check MaxAccessPoliciesPerAccessProfile capability
            if (AccessProfile.AccessPolicy.Count() > capabilities.MaxAccessPoliciesPerAccessProfile)
            {
                string message = string.Format("Max Access Polisies per AccessProfile exeeded.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxAccessPoliciesPerAccessProfile" });
            }

            //Check MultipleSchedulesPerAccessPointSupported capability
            if (!capabilities.MultipleSchedulesPerAccessPointSupported)
            {
                foreach (var group in AccessProfile.AccessPolicy.GroupBy(C => C.Entity))
                {
                    if (group.Count() > 1)
                    {
                        string message = string.Format("Multiple AccessPoints are not supported for the same schedule, see MultipleSchedulesPerAccessPointSupported capability.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MultipleSchedulesPerAccessPointSupported" });
                    }
                }
            }

            //Check that only access points are used
            if (AccessProfile.AccessPolicy.Any(C => (C.EntityType != null) && ((C.EntityType.Namespace != "http://www.onvif.org/ver10/accesscontrol/wsdl")||(C.EntityType.Name != "AccessPoint"))))
            {
                string message = string.Format("Specified Entity is not supported.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }

            //Check that all Access Pioints exists
            if (AccessProfile.AccessPolicy.Any(C => !(ConfStorage.AccessPointInfoList.Keys.Contains(C.Entity))))
            {
                string message = string.Format("Access Profile does not exist.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }

            //TODO: Check that all Schedules exists


            ConfStorage.AccessProfileList.Add(AccessProfile.token, AccessProfile);

            EventServer.ConfigurationAccessProfileChangedEvent(this, AccessProfile.token);

            EventServerSave();
            ConfStorageSave();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/ModifyAccessProfile", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifyAccessProfile(string Token, AccessProfile AccessProfile)
        {
            ConfStorageLoad();
            EventServerLoad();

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessRulesCapabilities;

            //Check that access profile exists
            if (!ConfStorage.AccessProfileList.ContainsKey(Token))
            {
                string message = string.Format("Access Profile with specified token {0} does not exists.", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }


            //Check MaxAccessPoliciesPerAccessProfile capability
            if (AccessProfile.AccessPolicy.Count() > capabilities.MaxAccessPoliciesPerAccessProfile)
            {
                string message = string.Format("Max Access Polisies per AccessProfile exeeded.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxAccessPoliciesPerAccessProfile" });
            }

            //Check MultipleSchedulesPerAccessPointSupported capability
            if (!capabilities.MultipleSchedulesPerAccessPointSupported)
            {
                foreach (var group in AccessProfile.AccessPolicy.GroupBy(C => C.Entity))
                {
                    if (group.Count() > 1)
                    {
                        string message = string.Format("Multiple AccessPoints are not supported for the same schedule, see MultipleSchedulesPerAccessPointSupported capability.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MultipleSchedulesPerAccessPointSupported" });
                    }
                }
            }

            //Check that only access points are used
            if (AccessProfile.AccessPolicy.Any(C => (C.EntityType != null) && ((C.EntityType.Namespace != "http://www.onvif.org/ver10/accesscontrol/wsdl") || (C.EntityType.Name != "AccessPoint"))))
            {
                string message = string.Format("Specified Entity is not supported.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }

            //Check that all Access Pioints exists
            if (AccessProfile.AccessPolicy.Any(C => !(ConfStorage.AccessPointInfoList.Keys.Contains(C.Entity))))
            {
                string message = string.Format("Access Profile does not exist.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }

            //TODO: Check that all Schedules exists

            ConfStorage.AccessProfileList.Remove(Token);
            ConfStorage.AccessProfileList.Add(AccessProfile.token, AccessProfile);

            EventServer.ConfigurationAccessProfileChangedEvent(this, AccessProfile.token);

            EventServerSave();
            ConfStorageSave();

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/DeleteAccessProfile", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteAccessProfile(string Token)
        {
            ConfStorageLoad();

            if (ConfStorage.AccessProfileList.ContainsKey(Token))
            {
                if (AccessProfileInUse(Token))
                {
                    string message = string.Format("Access Profile with token {0} is in use.", Token);
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "ReferenceInUse" });
                }
                else
                {
                    ConfStorage.AccessProfileList.Remove(Token);
                    //TODO: EventServer.CredentialEnabledEvent(this, "Changed", Token, true, "Credential enabled");
                    LoggingService.LogMessage(string.Format("Access Profile with token '{0}' was deleted.", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                }
            }
            else
            {
                string message = string.Format("Access Profile with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        #endregion //WebMethods

        /***************************************************************************************/

        #region Utils

        public static AccessProfileInfo ToAccessProfileInfo(AccessProfile accessProifle)
        {
            AccessProfileInfo accessProifleInfo = new AccessProfileInfo();

            accessProifleInfo.token = accessProifle.token;
            accessProifleInfo.Description = accessProifle.Description;
            accessProifleInfo.Name = accessProifle.Name;

            return accessProifleInfo;
        }

        public bool AccessProfileInUse(string accessProfileToken)
        {
            return ConfStorage.CredentialList.Any(C => (C.Value.CredentialAccessProfile != null) && (C.Value.CredentialAccessProfile.Any(A => A.AccessProfileToken == accessProfileToken)));
        }

        #endregion



      
    }
}
