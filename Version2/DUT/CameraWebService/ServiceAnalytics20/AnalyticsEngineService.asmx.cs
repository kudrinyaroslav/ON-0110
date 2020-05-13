using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using DUT.CameraWebService.Common;
using System.Web.Services.Protocols;
using System.Xml;

namespace DUT.CameraWebService.ServiceAnalytics20
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver20/analytics/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "AnalyticsEngineBinding", Namespace = "http://www.onvif.org/ver20/analytics/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MessageDescription))]
    public class AnalyticsEngineService : AnalyticsEngineBinding
    {
        //TestSuit
        AnalyticsEngineServiceTest AnalyticsEngineServiceTest
        {
            get
            {
                if (Application[Base.AppVars.ANALYTSERVICE] != null)
                {
                    return (AnalyticsEngineServiceTest)Application[Base.AppVars.ANALYTSERVICE];
                }
                else
                {
                    AnalyticsEngineServiceTest serviceTest = new AnalyticsEngineServiceTest(TestCommon);
                    Application[Base.AppVars.ANALYTSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }


        public override void CreateAnalyticsModules(string ConfigurationToken, [XmlElement("AnalyticsModule")] Config[] AnalyticsModule)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAnalyticsModules(string ConfigurationToken, [XmlElement("AnalyticsModuleName")] string[] AnalyticsModuleName)
        {
            throw new NotImplementedException();
        }

        [return: XmlElement("AnalyticsModule")]
        public override Config[] GetAnalyticsModules(string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
       // [XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AnalyticsCapabilitiesIncorrectResponseTag)]
        public override Capabilities GetServiceCapabilities()
        {

            ParametersValidation validation = new ParametersValidation();
            Capabilities result = (Capabilities)ExecuteGetCommand(validation, AnalyticsEngineServiceTest.GetServiceCapabilitiesTest);
            return result;

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/GetSupportedAnalyticsModules", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SupportedAnalyticsModules")]
        public override SupportedAnalyticsModules GetSupportedAnalyticsModules(string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            SupportedAnalyticsModules result = (SupportedAnalyticsModules)ExecuteGetCommand(validation, AnalyticsEngineServiceTest.GetSupportedAnalyticsModulesTest);
            return result;
        }

        public override void ModifyAnalyticsModules(string ConfigurationToken, [XmlElement("AnalyticsModule")] Config[] AnalyticsModule)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/CreateRules", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void CreateRules(string ConfigurationToken, [System.Xml.Serialization.XmlElementAttribute("Rule")] Config[] Rule)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            ExecuteVoidCommand(validation, AnalyticsEngineServiceTest.CreateRulesTest);

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/DeleteRules", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Analytics_DeleteRulesIncorrectResponseTag)]
        public override void DeleteRules(string ConfigurationToken, [System.Xml.Serialization.XmlElementAttribute("RuleName")] string[] RuleName)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.StringArray, "RuleName", RuleName);
            ExecuteVoidCommand(validation, AnalyticsEngineServiceTest.DeleteRulesTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/GetRuleOptions", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RuleOptions")]
       //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1185_xsiType)]
        public override ConfigOptions[] GetRuleOptions(XmlQualifiedName RuleType, string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalQName, "RuleType", RuleType);
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            ConfigOptions[] result = (ConfigOptions[])ExecuteGetCommand(validation, AnalyticsEngineServiceTest.GetRuleOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/GetAnalyticsModuleOptions", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]        
        public override ConfigOptions[] GetAnalyticsModuleOptions(XmlQualifiedName Type, string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();            
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            ConfigOptions[] result = (ConfigOptions[])ExecuteGetCommand(validation, AnalyticsEngineServiceTest.GetAnalyticsModuleOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/GetRules", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Rule")]
        public override Config[] GetRules(string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            Config[] result = (Config[])ExecuteGetCommand(validation, AnalyticsEngineServiceTest.GetRulesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/GetSupportedRules", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SupportedRules")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket2047_GetSupportedRulesResponse)]
        public override SupportedRules GetSupportedRules(string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            SupportedRules result = (SupportedRules)ExecuteGetCommand(validation, AnalyticsEngineServiceTest.GetSupportedRulesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/analytics/wsdl/ModifyRules", RequestNamespace = "http://www.onvif.org/ver20/analytics/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/analytics/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifyRules(string ConfigurationToken, [System.Xml.Serialization.XmlElementAttribute("Rule")] Config[] Rule)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ConfigurationToken", ConfigurationToken);
            ExecuteVoidCommand(validation, AnalyticsEngineServiceTest.ModifyRulesTest);
        }
    }
}
