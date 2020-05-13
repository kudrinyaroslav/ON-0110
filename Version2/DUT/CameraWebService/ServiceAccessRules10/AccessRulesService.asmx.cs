using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.ServiceAccessRules10
{

    /// <summary>
    /// Summary description for Access Rules Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "AccessRulesBinding", Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class AccessRulesService : AccessRulesBinding
    {
        //TestSuit
        AccessRulesServiceTest AccessRulesServiceTest
        {
            get
            {
                if (Application[Base.AppVars.ACCESSRULESSERVICE] != null)
                {
                    return (AccessRulesServiceTest)Application[Base.AppVars.ACCESSRULESSERVICE];
                }
                else
                {
                    AccessRulesServiceTest serviceTest = new AccessRulesServiceTest(TestCommon);
                    Application[Base.AppVars.ACCESSRULESSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        #region XmlReplySubstituteExtension for bugs
          //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.GetServiceCapabilities_with_missed_attribute )]
          //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.GetServiceCapabilities_with_missed_boolean_attribute )]
          //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.GetServiceCapabilities_with_two_tags )]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.MultipleSkipped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AccessRulesCapabilitiesIncorrectResponseTag)]
        #endregion

        public override ServiceCapabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            ServiceCapabilities result = (ServiceCapabilities)ExecuteGetCommand(validation, AccessRulesServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfo", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessProfileInfo")]
        public override AccessProfileInfo[] GetAccessProfileInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            AccessProfileInfo[] result = (AccessProfileInfo[])ExecuteGetCommand(validation, AccessRulesServiceTest.GetAccessProfileInfoTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfoList", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAccessProfileInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AccessProfileInfo")] out AccessProfileInfo[] AccessProfileInfo)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            AccessProfileInfo[] infos = AccessRulesServiceTest.TakeAccessProfileInfoList();
            string result = (string)ExecuteGetCommand(validation, AccessRulesServiceTest.GetAccessProfileInfoListTest);
            AccessProfileInfo = infos;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessProfile")]
        public override AccessProfile[] GetAccessProfiles([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            AccessProfile[] result = (AccessProfile[])ExecuteGetCommand(validation, AccessRulesServiceTest.GetAccessProfilesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileList", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAccessProfileList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AccessProfile")] out AccessProfile[] AccessProfile)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            AccessProfile[] infos = AccessRulesServiceTest.TakeAccessProfileList();
            string result = (string)ExecuteGetCommand(validation, AccessRulesServiceTest.GetAccessProfileListTest);
            AccessProfile = infos;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/CreateAccessProfile", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateAccessProfile(AccessProfile AccessProfile)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AccessProfile.token", AccessProfile.token);
            validation.Add(ParameterType.String, "AccessProfile.Name", AccessProfile.Name);
            validation.Add(ParameterType.String, "AccessProfile.Description", AccessProfile.Description);

            if (AccessProfile.AccessPolicy != null && AccessProfile.AccessPolicy.Count() > 0)
            {
                int i = 0;
                string iStr;
                foreach (var accessPolicy in AccessProfile.AccessPolicy)
                {
                    iStr = i.ToString();
                    validation.Add(ParameterType.String, "AccessProfile.AccessPolicy" + iStr + ".ScheduleToken", accessPolicy.ScheduleToken);
                    validation.Add(ParameterType.String, "AccessProfile.AccessPolicy" + iStr + ".Entity", accessPolicy.Entity);
                    validation.Add(ParameterType.OptionalQName, "AccessProfile.AccessPolicy" + iStr + ".EntityType", accessPolicy.EntityType);
                    i++;
                }

            }

            //TODO: AccessProfile.AccessPolicy validation list
            
            string result = (string)ExecuteGetCommand(validation, AccessRulesServiceTest.CreateAccessProfileTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/ModifyAccessProfile", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifyAccessProfile(AccessProfile AccessProfile)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AccessProfile.token", AccessProfile.token);
            validation.Add(ParameterType.String, "AccessProfile.Name", AccessProfile.Name);
            validation.Add(ParameterType.String, "AccessProfile.Description", AccessProfile.Description);
            if (AccessProfile.AccessPolicy != null && AccessProfile.AccessPolicy.Count() > 0)
            {
                validation.Add(ParameterType.String, "AccessProfile.AccessPolicy0.ScheduleToken", AccessProfile.AccessPolicy[0].ScheduleToken);
                validation.Add(ParameterType.String, "AccessProfile.AccessPolicy0.Entity", AccessProfile.AccessPolicy[0].Entity);
                validation.Add(ParameterType.OptionalQName, "AccessProfile.AccessPolicy0.EntityType", AccessProfile.AccessPolicy[0].EntityType);
            }

            //TODO: AccessProfile.AccessPolicy validation list

            ExecuteVoidCommand(validation, AccessRulesServiceTest.ModifyAccessProfileTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accessrules/wsdl/DeleteAccessProfile", RequestNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteAccessProfile(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, AccessRulesServiceTest.DeleteAccessProfileTest);
        }
    }
}
