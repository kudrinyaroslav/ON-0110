using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.PACS12
{

    /// <summary>
    /// Summary description for Access Control Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PACSBinding", Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class PACSService : PACSBinding
    {
        //TestSuit
        PACSServiceTest PACSServiceTest
        {
            get
            {
                if (Application[Base.AppVars.PACSSERVICE] != null)
                {
                    return (PACSServiceTest)Application[Base.AppVars.PACSSERVICE];
                }
                else
                {
                    PACSServiceTest serviceTest = new PACSServiceTest(TestCommon);
                    Application[Base.AppVars.PACSSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AccessControlCapabilitiesIncorrectResponseTag)]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            ServiceCapabilities result = (ServiceCapabilities)ExecuteGetCommand(validation, PACSServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/EnableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void EnableAccessPoint(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, PACSServiceTest.EnableAccessPointTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/DisableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DisableAccessPoint(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, PACSServiceTest.DisableAccessPointTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/ExternalAuthorization", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ExternalAuthorization(string AccessPointToken, string CredentialToken, string Reason, Decision Decision)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AccessPointToken", AccessPointToken);
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);
            validation.Add(ParameterType.String, "Reason", Reason);
            validation.Add(ParameterType.String, "Decision", Decision.ToString());
            ExecuteVoidCommand(validation, PACSServiceTest.ExternalAuthorizationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AreaInfo")]
        public override AreaInfo[] GetAreaInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            AreaInfo[] result = (AreaInfo[])ExecuteGetCommand(validation, PACSServiceTest.GetAreaInfoTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAreaInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AreaInfo")] out AreaInfo[] AreaInfo)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            AreaInfo[] infos = PACSServiceTest.TakeAreaInfoList();
            string result = (string)ExecuteGetCommand(validation, PACSServiceTest.GetAreaInfoListTest);
            AreaInfo = infos;
            return result;
        }
        
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointState", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessPointState")]
        public override AccessPointState GetAccessPointState(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            AccessPointState result = (AccessPointState)ExecuteGetCommand(validation, PACSServiceTest.GetAccessPointStateTest);
            return result;

        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")]
        public override AccessPointInfo[] GetAccessPointInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            AccessPointInfo[] result = (AccessPointInfo[])ExecuteGetCommand(validation, PACSServiceTest.GetAccessPointInfoTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAccessPointInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")] out AccessPointInfo[] AccessPointInfo)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            AccessPointInfo[] infos = PACSServiceTest.TakeAccesssPointInfoList();
            string result = (string)ExecuteGetCommand(validation, PACSServiceTest.GetAccessPointInfoListTest);
            AccessPointInfo = infos;
            return result;
        }

        [return: XmlElement("AccessPoint")]
        public override AccessPoint[] GetAccessPoints([XmlElement("Token")] string[] Token)
        {
            throw new NotImplementedException();
        }

        [return: XmlElement("NextStartReference")]
        public override string GetAccessPointList(int Limit, [XmlIgnore] bool LimitSpecified, string StartReference, [XmlElement("AccessPoint")] out AccessPoint[] AccessPoint)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/CreateAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateAccessPoint(AccessPoint AccessPoint)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", AccessPoint.token);
            validation.Add(ParameterType.String, "Name", AccessPoint.Name);
            validation.Add(ParameterType.String, "Description", AccessPoint.Description);
            //TODO: validation of other fields
            string result = (string)ExecuteGetCommand(validation, PACSServiceTest.CreateAccessPointTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/SetAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAccessPoint(AccessPoint AccessPoint)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", AccessPoint.token);
            validation.Add(ParameterType.String, "Name", AccessPoint.Name);
            validation.Add(ParameterType.String, "Description", AccessPoint.Description);
            //TODO: validation of other fields
            ExecuteVoidCommand(validation, PACSServiceTest.SetAccessPointTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/ModifyAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifyAccessPoint(AccessPoint AccessPoint)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", AccessPoint.token);
            validation.Add(ParameterType.String, "Name", AccessPoint.Name);
            validation.Add(ParameterType.String, "Description", AccessPoint.Description);
            //TODO: validation of other fields
            ExecuteVoidCommand(validation, PACSServiceTest.ModifyAccessPointTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/DeleteAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteAccessPoint(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);           
            ExecuteVoidCommand(validation, PACSServiceTest.DeleteAccessPointTest);
        }

        public override void SetAccessPointAuthenticationProfile(string Token, string AuthenticationProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAccessPointAuthenticationProfile(string Token)
        {
            throw new NotImplementedException();
        }

        [return: XmlElement("Area")]
        public override Area[] GetAreas([XmlElement("Token")] string[] Token)
        {
            throw new NotImplementedException();
        }

        [return: XmlElement("NextStartReference")]
        public override string GetAreaList(int Limit, [XmlIgnore] bool LimitSpecified, string StartReference, [XmlElement("Area")] out Area[] Area)
        {
            throw new NotImplementedException();
        }

        [return: XmlElement("Token")]
        public override string CreateArea(Area Area)
        {
            throw new NotImplementedException();
        }

        public override void SetArea(Area Area)
        {
            throw new NotImplementedException();
        }

        public override void ModifyArea(Area Area)
        {
            throw new NotImplementedException();
        }

        public override void DeleteArea(string Token)
        {
            throw new NotImplementedException();
        }
    }
}
