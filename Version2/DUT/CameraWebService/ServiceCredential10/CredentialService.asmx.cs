using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.ServiceCredential10
{

    /// <summary>
    /// Summary description for Credential Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "AccessRulesBinding", Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class CredentialService : CredentialServiceBinding
    {
        //TestSuit
        CredentialServiceTest CredentialServiceTest
        {
            get
            {
                if (Application[Base.AppVars.CREDENTIALSERVICE] != null)
                {
                    return (CredentialServiceTest)Application[Base.AppVars.CREDENTIALSERVICE];
                }
                else
                {
                    CredentialServiceTest serviceTest = new CredentialServiceTest(TestCommon);
                    Application[Base.AppVars.CREDENTIALSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }



        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
       
        #region XmlReplySubstituteExtension for bugs
          //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket711_GetServiceCapabilities_with_missed_attribute )]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.Cr_111_GetServiceCapabilities_ValiditySupportsTimeValueisTrue)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.Cr_111_GetServiceCapabilities_ValiditySupportsTimeValueSkipped)]
            //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.CredentialAccessProfileValidityInvalid)]
           //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.CredentialValiditySkipped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AccessRulesCapabilitiesIncorrectResponseTag)]
        #endregion

        public override ServiceCapabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            ServiceCapabilities result = (ServiceCapabilities)ExecuteGetCommand(validation, CredentialServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfo", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CredentialInfo")]
        public override CredentialInfo[] GetCredentialInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            CredentialInfo[] result = (CredentialInfo[])ExecuteGetCommand(validation, CredentialServiceTest.GetCredentialInfoTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfoList", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetCredentialInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("CredentialInfo")] out CredentialInfo[] CredentialInfo)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            CredentialInfo[] infos = CredentialServiceTest.TakeCredentialInfoList();
            string result = (string)ExecuteGetCommand(validation, CredentialServiceTest.GetCredentialInfoListTest);
            CredentialInfo = infos;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentials", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Credential")]
        public override Credential[] GetCredentials([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            Credential[] result = (Credential[])ExecuteGetCommand(validation, CredentialServiceTest.GetCredentialsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialList", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetCredentialList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("Credential")] out Credential[] Credential)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            Credential[] infos = CredentialServiceTest.TakeCredentialList();
            string result = (string)ExecuteGetCommand(validation, CredentialServiceTest.GetCredentialListTest);
            Credential = infos;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/CreateCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateCredential(Credential Credential, CredentialState State)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Credential.token", Credential.token);
            validation.Add(ParameterType.String, "Credential.CredentialHolderReference", Credential.CredentialHolderReference);
            validation.Add(ParameterType.String, "Credential.Description", Credential.Description);
            if (Credential.ValidFromSpecified)
            {
                validation.Add(ParameterType.Log, "Credential.ValidFrom", Credential.ValidFrom);
            }
            if (Credential.ValidToSpecified)
            {
                validation.Add(ParameterType.Log, "Credential.ValidTo", Credential.ValidTo);
            }

            if (Credential.CredentialAccessProfile != null && Credential.CredentialAccessProfile.Length > 0)
            {
                if (Credential.CredentialAccessProfile[0].ValidFromSpecified)
                {
                    validation.Add(ParameterType.Log, "Credential.CredentialAccessProfile0.ValidFrom", Credential.CredentialAccessProfile[0].ValidFrom);
                }
                if (Credential.CredentialAccessProfile[0].ValidToSpecified)
                {
                    validation.Add(ParameterType.Log, "Credential.CredentialAccessProfile0.ValidTo", Credential.CredentialAccessProfile[0].ValidTo);
                }
            }

            
            //validation.Add(ParameterType.String, "Credential.CredentialAccessProfile[0].ValidFrom", Credential.CredentialAccessProfile[0].ValidFrom);
            //validation.Add(ParameterType.String, "Credential.CredentialAccessProfile[0].ValidTo", Credential.CredentialAccessProfile[0].ValidTo);

            //TODO: Credential.ValidFrom validation
            //TODO: Credential.ValidTo validation
            //TODO: Credential.CredentialAccessProfile validation
            //TODO: Credential.CredentialIdentifier validation

            if (Credential.CredentialIdentifier != null && Credential.CredentialIdentifier.Length > 0)
            {
                validation.Add(ParameterType.Log, "Credential.CredentialIdentifier0.Value", Credential.CredentialIdentifier[0].Value);
            }

            validation.Add(ParameterType.String, "State.Enabled", State.Enabled.ToString());
            if (State.AntipassbackState != null)
            {
                validation.Add(ParameterType.String, "State.AntipassbackState.AntipassbackViolated", State.AntipassbackState.AntipassbackViolated.ToString());
            }
            validation.Add(ParameterType.String, "State.Reason", State.Reason);


            string result = (string)ExecuteGetCommand(validation, CredentialServiceTest.CreateCredentialTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/ModifyCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifyCredential(Credential Credential)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Credential.token", Credential.token);
            validation.Add(ParameterType.String, "Credential.CredentialHolderReference", Credential.CredentialHolderReference);
            validation.Add(ParameterType.String, "Credential.Description", Credential.Description);
            if (Credential.ValidFromSpecified)
            {
                validation.Add(ParameterType.Log, "Credential.ValidFrom", Credential.ValidFrom);
            }
            if (Credential.ValidToSpecified)
            {
                validation.Add(ParameterType.Log, "Credential.ValidTo", Credential.ValidTo);
            }

            if (Credential.CredentialAccessProfile != null && Credential.CredentialAccessProfile.Length > 0)
            {
                if (Credential.CredentialAccessProfile[0].ValidFromSpecified)
                {
                    validation.Add(ParameterType.Log, "Credential.CredentialAccessProfile0.ValidFrom", Credential.CredentialAccessProfile[0].ValidFrom);
                }
                if (Credential.CredentialAccessProfile[0].ValidToSpecified)
                {
                    validation.Add(ParameterType.Log, "Credential.CredentialAccessProfile0.ValidTo", Credential.CredentialAccessProfile[0].ValidTo);
                }
            }
            //TODO: Credential.ValidFrom validation
            //TODO: Credential.ValidTo validation
            //TODO: Credential.CredentialAccessProfile validation
            //TODO: Credential.CredentialIdentifier validation

            ExecuteVoidCommand(validation, CredentialServiceTest.ModifyCredentialTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DeleteCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCredential(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, CredentialServiceTest.DeleteCredentialTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialState", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("State")]
        public override CredentialState GetCredentialState(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            CredentialState result = (CredentialState)ExecuteGetCommand(validation, CredentialServiceTest.GetCredentialStateTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/EnableCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void EnableCredential(string Token, string Reason)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            validation.Add(ParameterType.String, "Reason", Reason);
            ExecuteVoidCommand(validation, CredentialServiceTest.EnableCredentialTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DisableCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DisableCredential(string Token, string Reason)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            validation.Add(ParameterType.String, "Reason", Reason);
            ExecuteVoidCommand(validation, CredentialServiceTest.DisableCredentialTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/ResetAntipassbackViolation", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ResetAntipassbackViolation(string CredentialToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);
            ExecuteVoidCommand(validation, CredentialServiceTest.ResetAntipassbackViolationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialIdentifiers", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CredentialIdentifier")]
        public override CredentialIdentifier[] GetCredentialIdentifiers(string CredentialToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);
            CredentialIdentifier[] result = (CredentialIdentifier[])ExecuteGetCommand(validation, CredentialServiceTest.GetCredentialIdentifiersTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/SetCredentialIdentifier", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetCredentialIdentifier(string CredentialToken, CredentialIdentifier CredentialIdentifier)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);
            validation.Add(ParameterType.String, "CredentialIdentifier.Type", CredentialIdentifier.Type.Name);
            validation.Add(ParameterType.String, "CredentialIdentifier.ExemptedFromAuthentication", CredentialIdentifier.ExemptedFromAuthentication.ToString());
            //TODO: CredentialIdentifier.CredentialIdentifierValue validation
            ExecuteVoidCommand(validation, CredentialServiceTest.SetCredentialIdentifierTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialIdentifier", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCredentialIdentifier(string CredentialToken, string CredentialIdentifierTypeName)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);
            validation.Add(ParameterType.String, "CredentialIdentifierTypeName", CredentialIdentifierTypeName);

            ExecuteVoidCommand(validation, CredentialServiceTest.DeleteCredentialIdentifierTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CredentialAccessProfile")]
        public override CredentialAccessProfile[] GetCredentialAccessProfiles(string CredentialToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);

            CredentialAccessProfile[] result = (CredentialAccessProfile[])ExecuteGetCommand(validation, CredentialServiceTest.GetCredentialAccessProfilesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/SetCredentialAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetCredentialAccessProfiles(string CredentialToken, [System.Xml.Serialization.XmlElementAttribute("CredentialAccessProfile")] CredentialAccessProfile[] CredentialAccessProfile)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);
            validation.Add(ParameterType.OptionalString, "CredentialAccessProfile0/AccessProfileToken", CredentialAccessProfile[0].AccessProfileToken);
            if (CredentialAccessProfile[0].ValidFromSpecified)
            {
                validation.Add(ParameterType.Log, "CredentialAccessProfile0/ValidFrom", CredentialAccessProfile[0].ValidFrom);
            }
            if (CredentialAccessProfile[0].ValidToSpecified)
            {
                validation.Add(ParameterType.Log, "CredentialAccessProfile0/ValidTo", CredentialAccessProfile[0].ValidTo);
            }
            //TODO: CredentialAccessProfile validation for array
            ExecuteVoidCommand(validation, CredentialServiceTest.SetCredentialAccessProfilesTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCredentialAccessProfiles(string CredentialToken, [System.Xml.Serialization.XmlElementAttribute("AccessProfileToken")] string[] AccessProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialToken", CredentialToken);
            validation.Add(ParameterType.StringArray, "AccessProfileToken", AccessProfileToken);

            ExecuteVoidCommand(validation, CredentialServiceTest.DeleteCredentialAccessProfilesTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetSupportedFormatTypes", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("FormatTypeInfo")]
        public override CredentialIdentifierFormatTypeInfo[] GetSupportedFormatTypes(string CredentialIdentifierTypeName)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "CredentialIdentifierTypeName", CredentialIdentifierTypeName);

            CredentialIdentifierFormatTypeInfo[] result = (CredentialIdentifierFormatTypeInfo[])ExecuteGetCommand(validation, CredentialServiceTest.GetSupportedFormatTypesTest);
            return result;
        }
    }
}
