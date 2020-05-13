using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using DUT.PACS.Simulator.Events;

namespace DUT.PACS.Simulator.ServiceAccessControl10
{
    /// <summary>
    /// Summary description for PACSService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PACSBinding", Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PACSService : PACSServiceBinding
    {

        public PACSService()
        {
            ConfStorageLoad();
            EventServerLoad();

            EventServer.PACSService = this;

            EventServerSave();
            ConfStorageSave();
        }

        /***************************************************************************************/

        #region WebMethods

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessControlCapabilities;
            return capabilities;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/EnableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void EnableAccessPoint(string Token)
        {

            ConfStorageLoad();

            if (ConfStorage.AccessPointInfoList.ContainsKey(Token))
            {
                if (ConfStorage.AccessPointInfoList[Token].Capabilities.DisableAccessPoint)
                {
                    ConfStorage.AccessPointState[Token].Enabled = true;
                    EventServer.AccessPointEnabledEvent(this, "Changed", Token, true, "Access point enabled");
                    LoggingService.LogMessage(string.Format("Access Point with token '{0}' enabled", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                }
                else
                {
                    string message = string.Format("AccessPoint with token {0} does not does not support DisableAccessPoint capability.", Token);
                    LoggingService.LogMessage(string.Format("Access Point with token '{0}' cannot be enabled", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Receiver", "ActionNotSupported", "NotSupported" });
                    //throw FaultLib.GetSoapException(FaultType.General, "AccessPoint " + Token + " does not does not support DisableAccessPoint capability.");
                }
            }
            else
            {
                string message = string.Format("AccessPoint with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
                //throw FaultLib.GetSoapException(FaultType.General, "Token " + Token + " does not exist.");
            }

            ConfStorageSave();

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/DisableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DisableAccessPoint(string Token)
        {

            ConfStorageLoad();

            if (ConfStorage.AccessPointInfoList.ContainsKey(Token))
            {
                if (ConfStorage.AccessPointInfoList[Token].Capabilities.DisableAccessPoint)
                {
                    ConfStorage.AccessPointState[Token].Enabled = false;
                    EventServer.AccessPointEnabledEvent(this, "Changed", Token, false, "Access point disabled");
                    LoggingService.LogMessage(string.Format("Access Point with token '{0}' disabled", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
                }
                else
                {
                    LoggingService.LogMessage(string.Format("Access Point with token '{0}' cannot be disabled", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    string message = string.Format("AccessPoint with token {0} does not does not support DisableAccessPoint capability.", Token);
                    FaultLib.ReturnFault(message, new string[] { "Receiver", "ActionNotSupported", "NotSupported" });
                    //throw FaultLib.GetSoapException(FaultType.General, "AccessPoint " + Token + " does not does not support DisableAccessPoint capability.");
                }
            }
            else
            {
                string message = string.Format("AccessPoint with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
                //throw FaultLib.GetSoapException(FaultType.General, "Token " + Token + " does not exist.");
            }

            ConfStorageSave();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")]
        public override AccessPointInfo[] GetAccessPointInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessControlCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });
            return GetListByTokenList<AccessPointInfo>(Token, C => C.AccessPointInfoList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAccessPointInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")] out AccessPointInfo[] AccessPointInfo)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessControlCapabilities;

            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidStartReference" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ? 
                    int.MaxValue : (int)capabilities.MaxLimit;

            AccessPointInfo = GetList<AccessPointInfo>(offset, true, Limit, true, C => C.AccessPointInfoList);
            string newStartReferense = null;
            if (offset + AccessPointInfo.Length < ConfStorage.AccessPointInfoList.Count)
            {
                newStartReferense = Convert.ToString(offset + AccessPointInfo.Length);
            }
            return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AreaInfo")]
        public override AreaInfo[] GetAreaInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessControlCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });
            return GetListByTokenList<AreaInfo>(Token, C => C.AreaInfoList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetAreaInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AreaInfo")] out AreaInfo[] AreaInfo)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.AccessControlCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidStartReference" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            AreaInfo = GetList<AreaInfo>(offset, true, Limit, true, C => C.AreaInfoList);
            string newStartReferense = null;
            if (offset + AreaInfo.Length < ConfStorage.AreaInfoList.Count)
            {
                newStartReferense = Convert.ToString(offset + AreaInfo.Length);
            }
            return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointState", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessPointState")]
        public override AccessPointState GetAccessPointState(string Token)
        {
            return GetInfo(Token, C => C.AccessPointState);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/ExternalAuthorization", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ExternalAuthorization(string AccessPointToken, string CredentialToken, string Reason, Decision Decision)
        {
            Reason = "Other";
            AccessPointInfo accessPointInfo = null;
            try
            {
                accessPointInfo = GetInfo(AccessPointToken, C => C.AccessPointInfoList);
            }
            catch (SoapException ex)
            {
                FaultLib.ReturnFault(string.Format("Access point '{0}' not found. ", AccessPointToken),
                    new[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            AccessPointState accessPointState = GetInfo(AccessPointToken, C => C.AccessPointState);
            var capabilities = accessPointInfo.Capabilities;
            Requester requester = Requester.Anonymous;
            DUT.PACS.Simulator.ServiceCredential10.Credential credentials = new DUT.PACS.Simulator.ServiceCredential10.Credential();
            
            if (!string.IsNullOrEmpty(CredentialToken))
            {
                requester = Requester.Credential;
                
                try
                {
                    credentials = GetInfo(CredentialToken, C => C.CredentialList);
                }
                catch (SoapException)
                {
                    Reason = "Invalid credentials";
                    EventServer.AccessControlExternalEvent(this, "Access Response",
                                                           AccessPointToken, CredentialToken, null, Reason,
                                                           Decision.Denied, requester);
                    throw;
                }
                
            }
            if (!capabilities.AnonymousAccess && requester == Requester.Anonymous)
            {
                Reason = "AnonymousAccess is inaccessible";
                EventServer.AccessControlExternalEvent(this, "Access Response",
                                                       AccessPointToken, CredentialToken, credentials.CredentialHolderReference,
                                                       Reason, Decision.Denied, requester);
                FaultLib.ReturnFault(Reason, new string[] { "Sender", "ActionNotSupported", "NotSupported" });
            }

            if (accessPointState.Enabled)
            {
                if (capabilities.ExternalAuthorizationSpecified && capabilities.ExternalAuthorization)
                {

                    EventServer.AccessControlExternalEvent(this, "Access Response",
                                                           AccessPointToken, CredentialToken,
                                                           credentials.CredentialHolderReference,
                                                           Reason, Decision, requester);
                }
                else
                {
                    Reason = "External authorization is inaccessible";
                    EventServer.AccessControlExternalEvent(this, "Access Response",
                                                           AccessPointToken, CredentialToken, credentials.CredentialHolderReference,
                                                           Reason, Decision.Denied, requester);
                    FaultLib.ReturnFault(Reason, new string[] { "Sender", "ActionNotSupported", "NotSupported" });
                }
            }
            else
            {
                Reason = "Access point is disabled";
                EventServer.AccessControlExternalEvent(this, "Access Response",
                                                       AccessPointToken, CredentialToken, credentials.CredentialHolderReference, Reason,
                                                       Decision.Denied, requester);
                FaultLib.ReturnFault(Reason, new string[] { "Sender" });
            }
        }

        #endregion //WebMethods

        /***************************************************************************************/

        public void SynchronizationPoint()
        {
            ConfStorageLoad();
            EventServerLoad();

            //foreach (AccessControllerInfo info in ConfStorage.AccessControllerInfoList)
            //{
            //    //EventServer.TamperingEvent(this, "Initialized", info.token, info.);
            //}
            //DoorState state = ConfStorage.DoorStateList[Token];
            foreach (var accessPoint in ConfStorage.AccessPointInfoList.Values)
            {
                var capabilities = accessPoint.Capabilities;
                var token = accessPoint.token;
                if (capabilities.DisableAccessPoint)
                    EventServer.AccessPointEnabledEvent(this, "Initialized", token,
                    ConfStorage.AccessPointState[token].Enabled, string.Empty);
                if(capabilities.TamperSpecified && capabilities.Tamper)
                    EventServer.AccessPointTamperingEvent(this, "Initialized", token,
                    ConfStorage.AccessPointTamperingState[token], string.Empty);
            }

            EventServerSave();
            ConfStorageSave();
        }

















    }
}
