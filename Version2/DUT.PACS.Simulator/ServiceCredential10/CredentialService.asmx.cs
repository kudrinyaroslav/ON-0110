using System;
using System.Collections.Generic;
using System.Linq;
using DUT.PACS.Simulator.Common;
using System.Data.Linq;
using System.Collections;

namespace DUT.PACS.Simulator.ServiceCredential10
{
    /// <summary>
    /// Summary description for Credential Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "CredentialBinding", Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class CredentialService : CredentialServiceBinding
    {

        #region Members



        #endregion //Members


        /***************************************************************************************/

        #region Events


        public CredentialService()
        {
            ConfStorageLoad();
            EventServerLoad();

            EventServer.CredentialService = this;

            EventServerSave();
            ConfStorageSave();
        }

        #endregion //Events

        #region Sensors



        #endregion

        /***************************************************************************************/



        #region WebMethods

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;
            return capabilities;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfo", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CredentialInfo")]
        public override CredentialInfo[] GetCredentialInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });


            return Array.ConvertAll(GetListByTokenList<Credential>(Token, C => C.CredentialList), item => ToCredentialInfo(item));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfoList", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetCredentialInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("CredentialInfo")] out CredentialInfo[] CredentialInfo)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified || Limit < 1 || Limit > capabilities.MaxLimit)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            CredentialInfo = Array.ConvertAll(GetList<Credential>(offset, true, Limit, true, C => C.CredentialList), item => ToCredentialInfo(item));
            string newStartReferense = null;
            if (offset + CredentialInfo.Length < ConfStorage.CredentialList.Count)
            {
                newStartReferense = Convert.ToString(offset + CredentialInfo.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentials", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Credential")]
        public override Credential[] GetCredentials([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });
            return GetListByTokenList<Credential>(Token, C => C.CredentialList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialList", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetCredentialList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("Credential")] out Credential[] Credential)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified || Limit < 1 || Limit > capabilities.MaxLimit)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            Credential = GetList<Credential>(offset, true, Limit, true, C => C.CredentialList);
            string newStartReferense = null;
            if (offset + Credential.Length < ConfStorage.CredentialList.Count)
            {
                newStartReferense = Convert.ToString(offset + Credential.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/CreateCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateCredential(Credential Credential, CredentialState State)
        {

            ConfStorageLoad();
            EventServerLoad();

            if (Credential.token == "")
            {
                int i = 1;

                Credential.token = "credential" + i.ToString();

                while (ConfStorage.CredentialList.Keys.Contains(Credential.token))
                {
                    Credential.token = "credential" + i.ToString();
                    i++;
                }
            }
            else
            {
                string message = string.Format("Not empty token.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgs" });
            }

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;
            string res = Credential.token;

            //Check that there is no credential with the same token exists
            if (ConfStorage.CredentialList.ContainsKey(Credential.token))
            {
                string message = string.Format("Credential with token {0} already exists.", Credential.token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }


            //Check MaxCredentials capability
            if (ConfStorage.CredentialList.Count() >= capabilities.MaxCredentials)
            {
                string message = string.Format("There is not enough space to create new credential, see the MaxCredentials capability.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxCredentials" });
            }

            if (Credential.CredentialAccessProfile != null)
            {

                //Check MaxAccessProfilesPerCredential capability
                if (Credential.CredentialAccessProfile.Count() > capabilities.MaxAccessProfilesPerCredential)
                {
                    string message = string.Format("Max Access Profiles per Credential exeeded.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxAccessProfilesPerCredential" });
                }

                //Check that all profiles exists
                if (Credential.CredentialAccessProfile.Any(C => !(ConfStorage.AccessProfileList.Keys.Contains(C.AccessProfileToken))))
                {
                    string message = string.Format("Access Profile does not exist.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                }

                //Check for Duplicated Access Profiles tokens
                foreach (var group in Credential.CredentialAccessProfile.GroupBy(C => C.AccessProfileToken))
                {
                    if (group.Count() > 1)
                    {
                        string message = string.Format("Duplicated Access Profile.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                    }
                }

                //Check Access Profile validity support and validity interval
                foreach (CredentialAccessProfile credentialAccessProfile in Credential.CredentialAccessProfile)
                {
                    if (capabilities.CredentialAccessProfileValiditySupported)
                    {
                        if (credentialAccessProfile.ValidFromSpecified && credentialAccessProfile.ValidToSpecified && (credentialAccessProfile.ValidFrom > credentialAccessProfile.ValidTo))
                        {
                            string message = string.Format("Validity interval is wrong for at least one Access Profile.");
                            LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                            FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                        }
                    }
                    else
                    {
                        if (credentialAccessProfile.ValidFromSpecified || credentialAccessProfile.ValidToSpecified)
                        {
                            string message = string.Format("Validity is not supported for Access Profile.");
                            LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                            FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "CredentialValiditySupported" });
                        }
                    }
                }

            }

            if (Credential.CredentialIdentifier != null)
            {

                //Check SupportedIdentifierType capability
                if (Credential.CredentialIdentifier.Any(C => !(capabilities.SupportedIdentifierType.Any(A => A == C.Type.Name))))
                {
                    string message = string.Format("Device does not support Identifier Type.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "InvalidIdentifierType" });
                }

                //Check for Duplicated Identifier Types
                foreach (var group in Credential.CredentialIdentifier.GroupBy(C => C.Type.Name))
                {
                    if (group.Count() > 1)
                    {
                        string message = string.Format("Duplicated Identifier Type.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "DuplicatedIdentifierType" });
                    }
                }

                foreach (var ci in Credential.CredentialIdentifier)
                {

                    if (!IfValueCompliesToFormatType(ci))
                    {
                        string message = string.Format("Value does not comply to indicated FormatType.", Credential.token);
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "InvalidIdentifierValue" });
                    }
                }
            }
            else
            {
                string message = string.Format("CredentialIdentifier is expected.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgs" });
            }

            if (Credential.CredentialHolderReference == null)
            {
                string message = string.Format("CredentialHolderReference is expected.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgs" });
            }


            //Check Credential validity support and validity interval
            if (capabilities.CredentialValiditySupported)
            {
                if (Credential.ValidFromSpecified && Credential.ValidToSpecified && (Credential.ValidFrom > Credential.ValidTo))
                {
                    string message = string.Format("Validity interval is wrong.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                }
            }
            else
            {
                if (Credential.ValidFromSpecified || Credential.ValidToSpecified)
                {
                    string message = string.Format("Validity is not supported.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "CredentialValiditySupported" });
                }
            }




            ConfStorage.CredentialList.Add(Credential.token, Credential);
            ConfStorage.CredentialStateList.Add(Credential.token, State);

            EventServer.ConfigurationCredentialChangedEvent(this, Credential.token);

            EventServerSave();
            ConfStorageSave();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/ModifyCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifyCredential(Credential Credential)
        {
            ConfStorageLoad();
            EventServerLoad();

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;

            //Check that credential exists
            if (!ConfStorage.CredentialList.ContainsKey(Credential.token))
            {
                string message = string.Format("Credential with specified token {0} does not exists.", Credential.token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            if (Credential.CredentialAccessProfile != null)
            {

                //Check MaxAccessProfilesPerCredential capability
                if (Credential.CredentialAccessProfile.Count() > capabilities.MaxAccessProfilesPerCredential)
                {
                    string message = string.Format("Max Access Profiles per Credential exeeded.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxAccessProfilesPerCredential" });
                }

                //Check that all profiles exists
                if (Credential.CredentialAccessProfile.Any(C => !(ConfStorage.AccessProfileList.Keys.Contains(C.AccessProfileToken))))
                {
                    string message = string.Format("Access Profile does not exist.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                }

                //Check for Duplicated Access Profiles tokens
                foreach (var group in Credential.CredentialAccessProfile.GroupBy(C => C.AccessProfileToken))
                {
                    if (group.Count() > 1)
                    {
                        string message = string.Format("Duplicated Access Profile.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                    }
                }

                //Check Access Profile validity support and validity interval
                foreach (CredentialAccessProfile credentialAccessProfile in Credential.CredentialAccessProfile)
                {
                    if (capabilities.CredentialAccessProfileValiditySupported)
                    {
                        if (credentialAccessProfile.ValidFromSpecified && credentialAccessProfile.ValidToSpecified && (credentialAccessProfile.ValidFrom > credentialAccessProfile.ValidTo))
                        {
                            string message = string.Format("Validity interval is wrong for at least one Access Profile.");
                            LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                            FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                        }
                    }
                    else
                    {
                        if (credentialAccessProfile.ValidFromSpecified || credentialAccessProfile.ValidToSpecified)
                        {
                            string message = string.Format("Validity is not supported for Access Profile.");
                            LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                            FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "CredentialValiditySupported" });
                        }
                    }
                }
            }



            if (Credential.CredentialIdentifier != null)
            {

                //Check SupportedIdentifierType capability
                if (Credential.CredentialIdentifier.Any(C => !(capabilities.SupportedIdentifierType.Any(A => A == C.Type.Name))))
                {
                    string message = string.Format("Device does not support Identifier Type.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "InvalidIdentifierType" });
                }

                //Check for Duplicated Identifier Types
                foreach (var group in Credential.CredentialIdentifier.GroupBy(C => C.Type.Name))
                {
                    if (group.Count() > 1)
                    {
                        string message = string.Format("Duplicated Identifier Type.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "DuplicatedIdentifierType" });
                    }
                }

                //Check Value
                foreach (var ci in Credential.CredentialIdentifier)
                {

                    if (!IfValueCompliesToFormatType(ci))
                    {
                        string message = string.Format("Value does not comply to indicated FormatType.", Credential.token);
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "InvalidIdentifierValue" });
                    }
                }

            }
            else
            {
                string message = string.Format("CredentialIdentifier is expected.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgs" });
            }

            if (Credential.CredentialHolderReference == null)
            {
                string message = string.Format("CredentialHolderReference is expected.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgs" });
            }

            //Check Credential validity support and validity interval
            if (capabilities.CredentialValiditySupported)
            {
                if (Credential.ValidFromSpecified && Credential.ValidToSpecified && (Credential.ValidFrom > Credential.ValidTo))
                {
                    string message = string.Format("Validity interval is wrong.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                }
            }
            else
            {
                if (Credential.ValidFromSpecified || Credential.ValidToSpecified)
                {
                    string message = string.Format("Validity is not supported.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "CredentialValiditySupported" });
                }
            }



            ConfStorage.CredentialList.Remove(Credential.token);
            ConfStorage.CredentialList.Add(Credential.token, Credential);

            EventServer.ConfigurationCredentialChangedEvent(this, Credential.token);

            EventServerSave();
            ConfStorageSave();

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DeleteCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCredential(string Token)
        {
            ConfStorageLoad();

            if (ConfStorage.CredentialList.ContainsKey(Token))
            {
                ConfStorage.CredentialList.Remove(Token);
                ConfStorage.CredentialStateList.Remove(Token);
                EventServer.ConfigurationCredentialRemovedEvent(this, Token);
                LoggingService.LogMessage(string.Format("Credential with token '{0}' was deleted.", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
            }
            else
            {
                string message = string.Format("Credential with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialState", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("State")]
        public override CredentialState GetCredentialState(string Token)
        {
            return GetInfo(Token, (c) => c.CredentialStateList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/EnableCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void EnableCredential(string Token, string Reason)
        {

            ConfStorageLoad();

            if (ConfStorage.CredentialList.ContainsKey(Token))
            {
                ConfStorage.CredentialStateList[Token].Enabled = true;
                ConfStorage.CredentialStateList[Token].Reason = Reason;
                EventServer.CredentialEnabledEvent(this, Token, true, Reason, true);
                LoggingService.LogMessage(string.Format("Credential with token '{0}' enabled", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
            }
            else
            {
                string message = string.Format("Credential with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DisableCredential", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DisableCredential(string Token, string Reason)
        {
            ConfStorageLoad();

            if (ConfStorage.CredentialList.ContainsKey(Token))
            {
                ConfStorage.CredentialStateList[Token].Enabled = false;
                ConfStorage.CredentialStateList[Token].Reason = Reason;
                EventServer.CredentialEnabledEvent(this, Token, false, Reason, true);
                LoggingService.LogMessage(string.Format("Credential with token '{0}' enabled", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
            }
            else
            {
                string message = string.Format("Credential with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/ResetAntipassbackViolation", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ResetAntipassbackViolation(string CredentialToken)
        {
            ConfStorageLoad();
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;
            if (capabilities.ResetAntipassbackSupported)
            {
                if (ConfStorage.CredentialList.ContainsKey(CredentialToken))
                {
                    ConfStorage.CredentialStateList[CredentialToken].AntipassbackState.AntipassbackViolated = false;
                    EventServer.ConfigurationCredentialAntipassbackEvent(this, CredentialToken);
                    EventServer.CredentialResetAntipassbackViolationEvent(this, CredentialToken, false, true);
                    LoggingService.LogMessage(string.Format("Reset Antipassback Violation for Credential with token '{0}' was done.", CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                }
                else
                {
                    string message = string.Format("Credential with token {0} does not exist", CredentialToken);
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
                }
            }
            else
            {
                string message = string.Format("Reset of Antipassback Violation is not supported.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialIdentifiers", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CredentialIdentifier")]
        public override CredentialIdentifier[] GetCredentialIdentifiers(string CredentialToken)
        {
            return GetInfo(CredentialToken, (c) => c.CredentialList).CredentialIdentifier;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/SetCredentialIdentifier", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetCredentialIdentifier(string CredentialToken, CredentialIdentifier CredentialIdentifier)
        {
            ConfStorageLoad();

            if (ConfStorage.CredentialList.ContainsKey(CredentialToken))
            {
                if (ConfStorage.CredentialList[CredentialToken].CredentialIdentifier != null)
                {
                  //CI-TypeName alreay exists in Cr-token, and FormatType is supported by DUT
                    if (ConfStorage.CredentialList[CredentialToken].CredentialIdentifier.Any(C => C.Type.Name == CredentialIdentifier.Type.Name) && 
                        SystemCapabilities.Instance.SupportedFormatTypes[CredentialIdentifier.Type.Name].Where(credendialIdentifierFormatTypeInfo => credendialIdentifierFormatTypeInfo.FormatType == CredentialIdentifier.Type.FormatType).Count() > 0)
                    {
                        if (IfValueCompliesToFormatType(CredentialIdentifier))
                        {
                            ConfStorage.CredentialList[CredentialToken].CredentialIdentifier = ConfStorage.CredentialList[CredentialToken].CredentialIdentifier.Where(C => C.Type.Name != CredentialIdentifier.Type.Name).ToArray();
                            List<CredentialIdentifier> credentialIdentifierList = ConfStorage.CredentialList[CredentialToken].CredentialIdentifier.ToList();
                            credentialIdentifierList.Add(CredentialIdentifier);
                            ConfStorage.CredentialList[CredentialToken].CredentialIdentifier = credentialIdentifierList.ToArray();
                            EventServer.ConfigurationCredentialIdentifierEvent(this, CredentialToken);
                            LoggingService.LogMessage(string.Format("Credential Identifier type {0} was was replaced for Credential with token {1}.", CredentialIdentifier.Type.Name, CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                        }
                        else
                        {
                            string message = string.Format("Value does not comply to indicated FormatType.", CredentialToken);
                            LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                            FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "InvalidIdentifierValue" });
                        }
                    }
                    else
                    {
                        ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;
                        //DUT supports CI-TypeName and FormatType
                        if (capabilities.SupportedIdentifierType.Any(c => c == CredentialIdentifier.Type.Name) && SystemCapabilities.Instance.SupportedFormatTypes.ContainsKey(CredentialIdentifier.Type.Name) &&
                            SystemCapabilities.Instance.SupportedFormatTypes[CredentialIdentifier.Type.Name].Where(credendialIdentifierFormatTypeInfo => credendialIdentifierFormatTypeInfo.FormatType == CredentialIdentifier.Type.FormatType).Count() > 0)
                        {
                            if (IfValueCompliesToFormatType(CredentialIdentifier))
                            {
                                List<CredentialIdentifier> credentialIdentifierList = ConfStorage.CredentialList[CredentialToken].CredentialIdentifier.ToList();
                                credentialIdentifierList.Add(CredentialIdentifier);
                                ConfStorage.CredentialList[CredentialToken].CredentialIdentifier = credentialIdentifierList.ToArray();
                                EventServer.ConfigurationCredentialIdentifierEvent(this, CredentialToken);
                                LoggingService.LogMessage(string.Format("Credential Identifier type {0} was was added for Credential with token {1}.", CredentialIdentifier.Type.Name, CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                            }
                            else
                            {
                                string message = string.Format("Value does not comply to indicated FormatType.", CredentialToken);
                                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "InvalidIdentifierValue" });
                            }
                        }
                        else
                        {
                            string message = string.Format("Device does not support Identifier Type.", CredentialToken);
                            LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                            FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "InvalidIdentifierType" });
                        }
                    }
                }
                else
                {
                    //This case is impossible for now, as CI is obligatory field
                    List<CredentialIdentifier> credentialIdentifierList = new List<ServiceCredential10.CredentialIdentifier>();
                    credentialIdentifierList.Add(CredentialIdentifier);
                    ConfStorage.CredentialList[CredentialToken].CredentialIdentifier = credentialIdentifierList.ToArray();
                    EventServer.ConfigurationCredentialIdentifierEvent(this, CredentialToken);
                    LoggingService.LogMessage(string.Format("Credential Identifier type {0} was was added for Credential with token {1}.", CredentialIdentifier.Type.Name, CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                }

            }
            else
            {
                string message = string.Format("Credential with token {0} does not exist", CredentialToken);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialIdentifier", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCredentialIdentifier(string CredentialToken, string CredentialIdentifierTypeName)
        {
            ConfStorageLoad();

            if (ConfStorage.CredentialList.ContainsKey(CredentialToken))
            {
                if (ConfStorage.CredentialList[CredentialToken].CredentialIdentifier.Any(C => C.Type.Name == CredentialIdentifierTypeName))
                {
                    if (ConfStorage.CredentialList[CredentialToken].CredentialIdentifier.Count() > 1)
                    {
                        ConfStorage.CredentialList[CredentialToken].CredentialIdentifier = ConfStorage.CredentialList[CredentialToken].CredentialIdentifier.Where(C => C.Type.Name != CredentialIdentifierTypeName).ToArray();
                        EventServer.ConfigurationCredentialIdentifierEvent(this, CredentialToken);
                        LoggingService.LogMessage(string.Format("Credential Identifier type {0} was deleted for Credential with token {1}.", CredentialIdentifierTypeName, CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                    }
                    else
                    {
                        string message = string.Format("At least one credential identifier is required");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Receiver", "ConstraintViolated", "MinIdentifiersPerCredential" });
                    }
                }
                else
                {
                    string message = string.Format("Credential Identifier type {0} does not exist for Credential with token {1}.", CredentialIdentifierTypeName, CredentialToken);
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    //FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
                }
            }
            else
            {
                string message = string.Format("Credential with token {0} does not exist", CredentialToken);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetCredentialAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CredentialAccessProfile")]
        public override CredentialAccessProfile[] GetCredentialAccessProfiles(string CredentialToken)
        {
            return GetInfo(CredentialToken, (c) => c.CredentialList).CredentialAccessProfile;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/SetCredentialAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetCredentialAccessProfiles(string CredentialToken, [System.Xml.Serialization.XmlElementAttribute("CredentialAccessProfile")] CredentialAccessProfile[] CredentialAccessProfile)
        {
            ConfStorageLoad();
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;

            if (ConfStorage.CredentialList.ContainsKey(CredentialToken))
            {
                if (ConfStorage.CredentialList[CredentialToken].CredentialAccessProfile != null)
                {
                    if (CredentialAccessProfile.Any(C => !(ConfStorage.AccessProfileList.Keys.Contains(C.AccessProfileToken))))
                    {
                        string message = string.Format("Access Profile does not exist", CredentialToken);
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                    }
                    else
                    {
                        if (!capabilities.CredentialAccessProfileValiditySupported)
                        {
                            if (CredentialAccessProfile.Any(C => C.ValidFromSpecified || C.ValidToSpecified))
                            {
                                string message = string.Format("Access Profile Validity does not supported.");
                                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "CredentialAccessProfileValiditySupported" });
                            }
                            else
                            {
                                SetCredentialAccessProfilesHelper(capabilities, ConfStorage.CredentialList[CredentialToken], CredentialAccessProfile);
                            }
                        }
                        else
                        {
                            if (CredentialAccessProfile.Any(C => (C.ValidFromSpecified && C.ValidToSpecified) && (C.ValidFrom >= C.ValidTo)))
                            {
                                string message = string.Format("Access Profile Validity interval is wrong for at least one Access Profile.");
                                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                            }
                            else
                            {
                                SetCredentialAccessProfilesHelper(capabilities, ConfStorage.CredentialList[CredentialToken], CredentialAccessProfile);
                                EventServer.ConfigurationCredentialChangedEvent(this, CredentialToken);
                            }
                        }
                    }
                }
                else
                {
                    List<CredentialAccessProfile> credentialAccessProfileList = new List<ServiceCredential10.CredentialAccessProfile>();
                    foreach (CredentialAccessProfile credentialAccessProfile in CredentialAccessProfile)
                    {
                        credentialAccessProfileList.Add(credentialAccessProfile);
                    }
                    ConfStorage.CredentialList[CredentialToken].CredentialAccessProfile = credentialAccessProfileList.ToArray();
                    EventServer.ConfigurationCredentialChangedEvent(this, CredentialToken);
                    LoggingService.LogMessage(string.Format("Credential AccessProfiles were added for Credential with token {0}.", CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);

                }
            }
            else
            {
                string message = string.Format("Credential with token {0} does not exist", CredentialToken);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialAccessProfiles", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteCredentialAccessProfiles(string CredentialToken, [System.Xml.Serialization.XmlElementAttribute("AccessProfileToken")] string[] AccessProfileToken)
        {
            ConfStorageLoad();

            if (ConfStorage.CredentialList.ContainsKey(CredentialToken))
            {
                if (AccessProfileToken.Count() == 0)
                {
                    ConfStorage.CredentialList[CredentialToken].CredentialAccessProfile = new CredentialAccessProfile[] { };
                    EventServer.ConfigurationCredentialChangedEvent(this, CredentialToken);
                    LoggingService.LogMessage(string.Format("Access Profiles was deleted for Credential with token {0}.", CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                }
                else
                {
                    foreach (string accessProfileToken in AccessProfileToken)
                    {
                        if (ConfStorage.CredentialList[CredentialToken].CredentialAccessProfile.Any(C => C.AccessProfileToken == accessProfileToken))
                        {
                            ConfStorage.CredentialList[CredentialToken].CredentialAccessProfile = ConfStorage.CredentialList[CredentialToken].CredentialAccessProfile.Where(C => C.AccessProfileToken != accessProfileToken).ToArray();
                            EventServer.ConfigurationCredentialChangedEvent(this, CredentialToken);
                            LoggingService.LogMessage(string.Format("Access Profile with token {0} was deleted for Credential with token {1}.", accessProfileToken, CredentialToken), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                        }
                        else
                        {
                            string message = string.Format("Access Profile with token {0} does not exist for Credential with token {1}.", accessProfileToken, CredentialToken);
                            LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        }
                    }
                }
            }
            else
            {
                string message = string.Format("Credential with token {0} does not exist", CredentialToken);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/credential/wsdl/GetSupportedFormatTypes", RequestNamespace = "http://www.onvif.org/ver10/credential/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/credential/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("FormatTypeInfo")]
        public override CredentialIdentifierFormatTypeInfo[] GetSupportedFormatTypes(string CredentialIdentifierTypeName)
        {
            ConfStorageLoad();

            CredentialIdentifierFormatTypeInfo[] res = null;

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.CredentialCapabilities;


            if (capabilities.SupportedIdentifierType.Any(c => c == CredentialIdentifierTypeName))
            {
                if (Simulator.SystemCapabilities.Instance.SupportedFormatTypes.ContainsKey(CredentialIdentifierTypeName))
                {
                    res = Simulator.SystemCapabilities.Instance.SupportedFormatTypes[CredentialIdentifierTypeName].ToArray();
                }
                else
                {
                    CredentialIdentifierFormatTypeInfo credentialIdentifierFormatTypeInfo = new CredentialIdentifierFormatTypeInfo();
                    credentialIdentifierFormatTypeInfo.FormatType = "SIMPLE_ALPHA_NUMERIC";
                    credentialIdentifierFormatTypeInfo.Description = "Simple alpha numeric string.";

                    res = new CredentialIdentifierFormatTypeInfo[] { credentialIdentifierFormatTypeInfo };
                }
            }
            else
            {
                string message = string.Format("Credential Identifier Type {0} is not supported.", CredentialIdentifierTypeName);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }

            ConfStorageSave();

            return res;
        }

        #endregion //WebMethods

        /***************************************************************************************/

        #region Utils

        public static CredentialInfo ToCredentialInfo(Credential credential)
        {
            CredentialInfo credentialInfo = new CredentialInfo();

            credentialInfo.token = credential.token;
            credentialInfo.Description = credential.Description;
            credentialInfo.ValidFromSpecified = credential.ValidFromSpecified;
            credentialInfo.ValidFrom = credential.ValidFrom;
            credentialInfo.ValidToSpecified = credential.ValidToSpecified;
            credentialInfo.ValidTo = credential.ValidTo;
            credentialInfo.CredentialHolderReference = credential.CredentialHolderReference;

            return credentialInfo;
        }



        private void SetCredentialAccessProfilesHelper(ServiceCapabilities capabilities, Credential Credential, CredentialAccessProfile[] CredentialAccessProfile)
        {
            if (Credential.CredentialAccessProfile.Count() + CredentialAccessProfile.Count(C => Credential.CredentialAccessProfile.Any(A => A.AccessProfileToken == C.AccessProfileToken)) > capabilities.MaxAccessProfilesPerCredential)
            {
                string message = string.Format("Max Access Profiles per Credential exeeded for Credential with token {0}.", Credential.token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxAccessProfilesPerCredential" });
            }
            else
            {
                foreach (CredentialAccessProfile credentialAccessProfile in CredentialAccessProfile)
                {
                    if (Credential.CredentialAccessProfile.Any(C => C.AccessProfileToken == credentialAccessProfile.AccessProfileToken))
                    {
                        Credential.CredentialAccessProfile = Credential.CredentialAccessProfile.Where(C => C.AccessProfileToken != credentialAccessProfile.AccessProfileToken).ToArray();
                        List<CredentialAccessProfile> credentialAccessProfileList = Credential.CredentialAccessProfile.ToList();
                        credentialAccessProfileList.Add(credentialAccessProfile);
                        Credential.CredentialAccessProfile = credentialAccessProfileList.ToArray();
                        //TODO: Event
                        LoggingService.LogMessage(string.Format("Credential Access Profile with token {0} was was replaced for Credential with token {1}.", credentialAccessProfile.AccessProfileToken, Credential.token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                    }
                    else
                    {
                        List<CredentialAccessProfile> credentialAccessProfileList = Credential.CredentialAccessProfile.ToList();
                        credentialAccessProfileList.Add(credentialAccessProfile);
                        Credential.CredentialAccessProfile = credentialAccessProfileList.ToArray();
                        //TODO: Event
                        LoggingService.LogMessage(string.Format("Credential Access Profile with token {0} was was added for Credential with token {1}.", credentialAccessProfile.AccessProfileToken, Credential.token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);

                    }
                }
            }
        }

        private bool IfValueCompliesToFormatType(CredentialIdentifier credentialIdentifier)
        {
            bool temp = false;
            //BitArray bitArray1 = new BitArray(credentialIdentifier.Value);
            //BitArray bitArray;
            String binaryCode = "";
            for (int i = 0; i < credentialIdentifier.Value.Length; i++)
            {
                String temp1 = Convert.ToString(credentialIdentifier.Value[i], 2);
                if (temp1.Length < 8)
                    for (int j=0; j<8-temp1.Length; j++)
                        binaryCode += "0";
                binaryCode += temp1;
            }
            String trueBinaryCode = "";
            int one = 0;
            switch (credentialIdentifier.Type.FormatType) 
            {
                case "WIEGAND26":
                    //bitArray = new BitArray(26);
                    if (binaryCode.Length != 32)
                        break;

                    //trueBinaryCode = binaryCode.Remove(26);
                    //one = 0;
                    //for (int i = 1; i < 13; i++)
                    //{
                    //    if (trueBinaryCode[i] == '1')
                    //    one += 1;
                    //}
                    ////if the sum of 1-12 bit is odd, the bit number [0] should be equal to 1
                    //if (one % 2 == 1)
                    //{
                    //    if (trueBinaryCode[0] == '0')
                    //        break;
                    //}
                    //else //and vice versa
                    //{
                    //    if (trueBinaryCode[0] == '1')
                    //        break;
                    //}
                    //one = 0;
                    //for (int i = 13; i < 25; i++)
                    //{
                    //    if (trueBinaryCode[i] == '1')
                    //    one += 1;
                    //}
                    ////if the sum of 13-24 bit is odd, the bit number [25] should be equal to 0
                    //if (one % 2 == 1)
                    //{
                    //    if (trueBinaryCode[25] == '1')
                    //        break;
                    //}
                    //else //and vice versa
                    //{
                    //    if (trueBinaryCode[25] == '0')
                    //        break;
                    //}
                    temp = true;
                    break;
                case "WIEGAND37":
                case "WIEGAND37_FACILITY":
                    if (binaryCode.Length != 40)
                        break;
                    //trueBinaryCode = binaryCode.Remove(37);
                    //one = 0;
                    //for (int i = 1; i < 19; i++)
                    //{
                    //    if (trueBinaryCode[i] == '1')
                    //    one += 1;
                    //}
                    ////if the sum of 1-12 bit is odd, the bit number [0] should be equal to 1
                    //if (one % 2 == 1)
                    //{
                    //    if (trueBinaryCode[0] == '0')
                    //        break;
                    //}
                    //else //and vice versa
                    //{
                    //    if (trueBinaryCode[0] == '1')
                    //        break;
                    //}
                    //one = 0;
                    //for (int i = 18; i < 36; i++)
                    //{
                    //    if (trueBinaryCode[i] == '1')
                    //    one += 1;
                    //}
                    ////if the sum of 13-24 bit is odd, the bit number [25] should be equal to 0
                    //if (one % 2 == 1)
                    //{
                    //    if (trueBinaryCode[36] == '1')
                    //        break;
                    //}
                    //else //and vice versa
                    //{
                    //    if (trueBinaryCode[36] == '0')
                    //        break;
                    //}
                    temp = true;
                    break;
                case "SIMPLE_NUMBER32":
                    if (binaryCode.Length != 32)
                        break;
                    temp = true;
                    break;
                default:
                    break;
            }
            return temp;
        }

        #endregion

    }
}
