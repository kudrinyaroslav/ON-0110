///////////////////////////////////////////////////////////////////////////
//!  @author        Alexei Soloview
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using DateTime = System.DateTime;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;

namespace TestTool.Tests.TestCases.TestSuites.AdvancedSecurity
{
    /// <summary>
    ///     This file contains various utils used in implementation of testcase's bodies.
    /// </summary>
    partial class AdvancedSecurityTestSuit
    {
        #region Member's implementation

        private ServiceHolder<DeviceClient, Device> m_DeviceClient;

        private ServiceHolder<EventPortTypeClient, EventPortType> m_EventClient;

        public BaseOnvifTest Test
        {
            get { return this; }
        }

        private string GetAdvancedSecurityServiceAddress(FeaturesList feature)
        { return this.GetServiceAddress(OnvifService.ADVANCED_SECURITY); }

        private OnvifServiceClient<AdvancedSecurityService, AdvancedSecurityServiceClient> m_AdvancedSecurityServiceClient;
        OnvifServiceClient<AdvancedSecurityService, AdvancedSecurityServiceClient> IBaseOnvifService2<AdvancedSecurityService, AdvancedSecurityServiceClient>.ServiceClient
        {
            get
            {
                if (!m_AdvancedSecurityServiceClient.IsInitialized())
                {
                    m_AdvancedSecurityServiceClient = new OnvifServiceClient<AdvancedSecurityService, AdvancedSecurityServiceClient>(this, "Advanced Security", GetAdvancedSecurityServiceAddress);
                    m_AdvancedSecurityServiceClient.InitServiceClient(new [] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) });
                }

                return m_AdvancedSecurityServiceClient;
            }
        }

        private OnvifServiceClient<Keystore, KeystoreClient> m_KeystoreServiceClient;
        OnvifServiceClient<Keystore, KeystoreClient> IBaseOnvifService2<Keystore, KeystoreClient>.ServiceClient
        {
            get
            {
                if (!m_KeystoreServiceClient.IsInitialized())
                {
                    m_KeystoreServiceClient = new OnvifServiceClient<Keystore, KeystoreClient>(this, "Keystore", GetAdvancedSecurityServiceAddress);
                    m_KeystoreServiceClient.InitServiceClient(new [] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) });
                }

                return m_KeystoreServiceClient;
            }
        }

        private OnvifServiceClient<TLSServer, TLSServerClient> m_TLSServerServiceClient;
        OnvifServiceClient<TLSServer, TLSServerClient> IBaseOnvifService2<TLSServer, TLSServerClient>.ServiceClient
        {
            get
            {
                if (!m_TLSServerServiceClient.IsInitialized())
                {
                    m_TLSServerServiceClient = new OnvifServiceClient<TLSServer, TLSServerClient>(this, 
                                                                                                  "TLS Server", 
                                                                                                  GetAdvancedSecurityServiceAddress);
                    m_TLSServerServiceClient.InitServiceClient(new [] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) });
                }

                return m_TLSServerServiceClient;
            }
        }

        private OnvifServiceClient<Dot1X, Dot1XClient> m_Dot1XServiceClient;
        OnvifServiceClient<Dot1X, Dot1XClient> IBaseOnvifService2<Dot1X, Dot1XClient>.ServiceClient
        {
            get
            {
                if (!m_Dot1XServiceClient.IsInitialized())
                {
                    m_Dot1XServiceClient = new OnvifServiceClient<Dot1X, Dot1XClient>(this, "Dot1X", GetAdvancedSecurityServiceAddress);
                    m_Dot1XServiceClient.InitServiceClient(new [] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) });
                }

                return m_Dot1XServiceClient;
            }
        }

        public DistinguishedName DefaultSubject
        {
            get { return new DistinguishedName {Country = new[] {"US"}, CommonName = new[] {_cameraIp.ToString()}}; }
        }

        private OnvifServiceClient<Device, DeviceClient> m_DeviceServiceClient;
        OnvifServiceClient<Device, DeviceClient> IBaseOnvifService2<Device, DeviceClient>.ServiceClient        
        {
            get
            {
                if (!m_DeviceServiceClient.IsInitialized())
                {
                    m_DeviceServiceClient = new OnvifServiceClient<Device, DeviceClient>(this, "Device", this.GetDeviceServiceAddress);
                    m_DeviceServiceClient.InitServiceClient(new [] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });
                }

                return m_DeviceServiceClient;
            }
        }

        private OnvifServiceClient<EventPortType, EventPortTypeClient> m_EventServiceClient;
        OnvifServiceClient<EventPortType, EventPortTypeClient> IBaseOnvifService2<EventPortType, EventPortTypeClient>.ServiceClient        
        {
            get
            {
                if (!m_EventServiceClient.IsInitialized())
                {
                    m_EventServiceClient = new OnvifServiceClient<EventPortType, EventPortTypeClient>(this, "Event", this.GetEventServiceAddress);
                    m_EventServiceClient.InitServiceClient(new [] { new SoapValidator(EventsSchemasSet.GetInstance()) });
                }

                return m_EventServiceClient;
            }
        }
        #endregion

        #region Utils

        protected void DelayAfterSetNetworkProtocols()
        {
            Delay(string.Format("Waiting for the time {0} seconds while network protocols settings will be applied.", this.OperationDelay/1000));
        }

        protected string SecuredCameraAddress(int port)
        {
            var securedURI = new UriBuilder(CameraAddress);
            securedURI.Port = port;
            securedURI.Scheme = "https";

            return securedURI.Uri.ToString();
        }

        protected NetworkProtocol[] checkHTTPSAccess()
        {
            NetworkProtocol[] protocols = this.GetNetworkProtocols();
            NetworkProtocol https = protocols.FirstOrDefault(e => NetworkProtocolType.HTTPS == e.Name);

            Assert(null != https,
                   "Acess over HTTPS is not supported",
                   "Check access over HTTPS is supported");

            return protocols;
        }

        protected T ExtractCapabilities<T>(XmlElement element, string ns)
        {
            BeginStep("Parse Capabilities element in GetServices response");

            var xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Capabilities";
            xRoot.IsNullable = true;
            xRoot.Namespace = ns;

            var serializer = new XmlSerializer(typeof (T), xRoot);

            XmlReader reader = new XmlNodeReader(element);

            T capabilities;
            try
            {
                capabilities = (T) serializer.Deserialize(reader);
            }
            catch (Exception exc)
            {
                string message;
                if (exc.InnerException != null)
                {
                    message = string.Format("{0} {1}", exc.Message, exc.InnerException.Message);
                }
                else
                {
                    message = exc.Message;
                }
                throw new ApplicationException(message);
            }
            StepPassed();
            return capabilities;
        }

        protected AdvancedSecurityCapabilities ExtractAdvancedSecurityCapabilities(XmlElement element)
        {
            return ExtractCapabilities<AdvancedSecurityCapabilities>(element, OnvifService.ADVANCED_SECURITY);
        }

        private void ValidateCapabilitiesAttribute(string attributeName,
                                                   bool attributeSpecified1,
                                                   bool attributeSpecified2,
                                                   string attributeValue1,
                                                   string attributeValue2,
                                                   ref StringBuilder sb,
                                                   ref bool ok)
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1 != attributeValue2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
                else if (attributeSpecified1 &&
                         !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped", attributeName));
                }
            }
        }

        private void ValidateCapabilitiesAttribute(string attributeName,
                                                   bool attributeSpecified1,
                                                   bool attributeSpecified2,
                                                   bool attributeValue1,
                                                   bool attributeValue2,
                                                   ref StringBuilder sb,
                                                   ref bool ok)
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1 != attributeValue2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2 && attributeValue2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = {1}", attributeName, attributeValue2));
                }
                else if (attributeSpecified1 && attributeValue1 &&
                         !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = {1}", attributeName, attributeValue1));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped", attributeName));
                }
            }
        }

        private void ValidateCapabilitiesAttribute(string attributeName,
                                                   bool attributeSpecified1,
                                                   bool attributeSpecified2,
                                                   int[] attributeValue1,
                                                   int[] attributeValue2,
                                                   ref StringBuilder sb,
                                                   ref bool ok)
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1.Length != attributeValue2.Length)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(" ", attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(" ", attributeValue2)));
                }
                else
                {
                    var foundItems = new List<int>();
                    for (int i = 0; i < attributeValue1.Length; i++)
                    {
                        int index = Array.IndexOf(attributeValue2, attributeValue1[i]);

                        if (attributeValue2.Contains(attributeValue1[i]))
                        {
                            if (!foundItems.Contains(index))
                            {
                                foundItems.Add(index);
                            }
                            else
                            {
                                while (foundItems.Contains(index))
                                {
                                    index = Array.IndexOf(attributeValue2, attributeValue1[i], index + 1);
                                    if (index == -1)
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("{0} values are different.", attributeName));
                                        sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                                    attributeName,
                                                                    string.Join(" ", attributeValue1)));
                                        sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                                    attributeName,
                                                                    string.Join(" ", attributeValue2)));

                                        break;
                                    }
                                }

                                if (index != -1)
                                    foundItems.Add(index);
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine(string.Format("{0} values are different.", attributeName));
                            sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                        attributeName,
                                                        string.Join(" ", attributeValue1)));
                            sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                        attributeName,
                                                        string.Join(" ", attributeValue2)));

                            break;
                        }
                    }
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped or empty", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(" ", attributeValue2)));
                }
                else if (attributeSpecified1 &&
                         !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(" ", attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped or empty", attributeName));
                }
            }
        }

        private void ValidateCapabilitiesAttribute(string attributeName,
                                                   bool attributeSpecified1,
                                                   bool attributeSpecified2,
                                                   object[] attributeValue1,
                                                   object[] attributeValue2,
                                                   ref StringBuilder sb,
                                                   ref bool ok,
                                                   string separator = " ")
        {
            if (attributeSpecified1 &&
                attributeSpecified2)
            {
                if (attributeValue1.Length != attributeValue2.Length)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(separator, attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(separator, attributeValue2)));
                }
                else
                {
                    var foundItems = new List<int>();
                    for (int i = 0; i < attributeValue1.Length; i++)
                    {
                        int index = Array.IndexOf(attributeValue2, attributeValue1[i]);

                        if (attributeValue2.Contains(attributeValue1[i]))
                        {
                            if (!foundItems.Contains(index))
                            {
                                foundItems.Add(index);
                            }
                            else
                            {
                                while (foundItems.Contains(index))
                                {
                                    index = Array.IndexOf(attributeValue2, attributeValue1[i], index + 1);
                                    if (index == -1)
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("{0} values are different.", attributeName));
                                        sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                                    attributeName,
                                                                    string.Join(separator, attributeValue1)));
                                        sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                                    attributeName,
                                                                    string.Join(separator, attributeValue2)));

                                        break;
                                    }
                                }

                                if (index != -1)
                                    foundItems.Add(index);
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine(string.Format("{0} values are different.", attributeName));
                            sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                        attributeName,
                                                        string.Join(separator, attributeValue1)));
                            sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                        attributeName,
                                                        string.Join(separator, attributeValue2)));

                            break;
                        }
                    }
                }
            }
            else
            {
                if (!attributeSpecified1 &&
                    attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped or empty", attributeName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(separator, attributeValue2)));
                }
                else if (attributeSpecified1 &&
                         !attributeSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", attributeName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                attributeName,
                                                string.Join(separator, attributeValue1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped or empty", attributeName));
                }
            }
        }

        private bool CompareAlgorithmIdentifier(AlgorithmIdentifier algId1, AlgorithmIdentifier algId2)
        {
            bool ok = true;

            if (algId1.algorithm != algId2.algorithm)
            {
                ok = false;
            }
            else
            {
                byte[] parameters1 = null;
                byte[] parameters2 = null;

                bool parametersSpecified1 = algId1.parameters != null && algId1.parameters.Length != 0;
                bool parametersSpecified2 = algId2.parameters != null && algId2.parameters.Length != 0;

                if (parametersSpecified1)
                    parameters1 = algId1.parameters;

                if (parametersSpecified2)
                    parameters2 = algId2.parameters;

                if (parametersSpecified1 && parametersSpecified2)
                {
                    if (parameters1.Length != parameters2.Length)
                    {
                        ok = false;
                    }
                    else
                    {
                        for (int i = 0; i < parameters1.Length; i++)
                            if (parameters1[i] != parameters2[i])
                                ok = false;
                    }
                }
                else if (!parametersSpecified1 && parametersSpecified2)
                {
                    ok = false;
                }
                else if (parametersSpecified1 && !parametersSpecified2)
                {
                    ok = false;
                }
            }

            return ok;
        }

        private void ValidateCapabilitiesElement(string elementName,
                                                 bool elementSpecified1,
                                                 bool elementSpecified2,
                                                 AlgorithmIdentifier[] elementValue1,
                                                 AlgorithmIdentifier[] elementValue2,
                                                 ref StringBuilder sb,
                                                 ref bool ok)
        {
            if (elementSpecified1 &&
                elementSpecified2)
            {
                var algorithms1 = new List<string>();
                foreach (AlgorithmIdentifier item in elementValue1)
                    algorithms1.Add(item.algorithm);

                var algorithms2 = new List<string>();
                foreach (AlgorithmIdentifier item in elementValue2)
                    algorithms2.Add(item.algorithm);

                if (elementValue1.Length != elementValue2.Length)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", elementName));
                    sb.AppendLine(string.Format("From GetServices: {0} = \"{1}\"",
                                                elementName,
                                                string.Join(" ", algorithms1)));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} = \"{1}\"",
                                                elementName,
                                                string.Join(" ", algorithms2)));
                }
                else
                {
                    var foundItems = new List<int>();
                    for (int i = 0; i < elementValue1.Length; i++)
                    {
                        AlgorithmIdentifier algId =
                                elementValue2.FirstOrDefault(
                                        algid => CompareAlgorithmIdentifier(elementValue1[i], algid));
                        if (algId != null)
                        {
                            int index = Array.IndexOf(elementValue2, algId);
                            if (!foundItems.Contains(index))
                            {
                                foundItems.Add(index);
                            }
                            else
                            {
                                while (foundItems.Contains(index))
                                {
                                    index = Array.IndexOf(elementValue2, algId, index + 1);
                                    if (index == -1)
                                    {
                                        ok = false;
                                        sb.AppendLine(string.Format("{0} values are different.", elementName));

                                        string parameters = null;
                                        if (algId.parameters != null && algId.parameters.Length != 0)
                                            parameters = Convert.ToBase64String(algId.parameters,
                                                                                Base64FormattingOptions.InsertLineBreaks);

                                        sb.AppendLine(
                                                string.Format(
                                                        "{0} value = {{{1}{2}}} in capabilities from GetServices is missed in capabilities from GetServiceCapabilities.",
                                                        elementName,
                                                        algId.algorithm,
                                                        !string.IsNullOrEmpty(parameters)
                                                                ? (", \"" + parameters + "\"")
                                                                : ""));

                                        break;
                                    }
                                }

                                if (index != -1)
                                    foundItems.Add(index);
                            }
                        }
                        else
                        {
                            ok = false;
                            sb.AppendLine(string.Format("{0} values are different.", elementName));

                            string parameters = null;
                            if (elementValue1[i].parameters != null && elementValue1[i].parameters.Length != 0)
                                parameters = Convert.ToBase64String(elementValue1[i].parameters,
                                                                    Base64FormattingOptions.InsertLineBreaks);

                            sb.AppendLine(
                                    string.Format(
                                            "{0} value = {{{1}{2}}} in capabilities from GetServices is missed in capabilities from GetServiceCapabilities.",
                                            elementName,
                                            elementValue1[i].algorithm,
                                            !string.IsNullOrEmpty(parameters) ? (", \"" + parameters + "\"") : ""));

                            break;
                        }
                    }
                }
            }
            else
            {
                if (!elementSpecified1 &&
                    elementSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", elementName));
                    sb.AppendLine(string.Format("From GetServices: {0} is skipped or empty", elementName));
                }
                else if (elementSpecified1 &&
                         !elementSpecified2)
                {
                    ok = false;
                    sb.AppendLine(string.Format("{0} values are different.", elementName));
                    sb.AppendLine(string.Format("From GetServiceCapabilities: {0} is skipped or empty", elementName));
                }
            }
        }

        protected void CompareCapabilities(AdvancedSecurityCapabilities fromGetServices,
                                           AdvancedSecurityCapabilities fromGetServiceCapabilities)
        {
            var sb = new StringBuilder();
            bool ok = true;

            // SignatureAlgorithms
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool SignatureAlgorithmsSpecified1 = fromGetServices.KeystoreCapabilities.SignatureAlgorithms != null &&
                                                 fromGetServices.KeystoreCapabilities.SignatureAlgorithms.Length != 0;

            bool SignatureAlgorithmsSpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.SignatureAlgorithms !=
                                                 null &&
                                                 fromGetServiceCapabilities.KeystoreCapabilities.SignatureAlgorithms
                                                                           .Length != 0;

            AlgorithmIdentifier[] SignatureAlgorithms1 = fromGetServices.KeystoreCapabilities.SignatureAlgorithms;
            AlgorithmIdentifier[] SignatureAlgorithms2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.SignatureAlgorithms;

            ValidateCapabilitiesElement("SignatureAlgorithms",
                                        SignatureAlgorithmsSpecified1,
                                        SignatureAlgorithmsSpecified2,
                                        SignatureAlgorithms1,
                                        SignatureAlgorithms2,
                                        ref sb,
                                        ref ok);

            // MaximumNumberOfKeys
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfKeysSpecified1 = fromGetServices.KeystoreCapabilities.MaximumNumberOfKeysSpecified;
            bool MaximumNumberOfKeysSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfKeysSpecified;
            string MaximumNumberOfKeys1 = fromGetServices.KeystoreCapabilities.MaximumNumberOfKeys.ToString();
            string MaximumNumberOfKeys2 = fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfKeys.ToString();

            ValidateCapabilitiesAttribute("MaximumNumberOfKeys",
                                          MaximumNumberOfKeysSpecified1,
                                          MaximumNumberOfKeysSpecified2,
                                          MaximumNumberOfKeys1,
                                          MaximumNumberOfKeys2,
                                          ref sb,
                                          ref ok);

            // MaximumNumberOfCertificates 
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfCertificatesSpecified1 =
                    fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificatesSpecified;
            bool MaximumNumberOfCertificatesSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificatesSpecified;
            string MaximumNumberOfCertificates1 =
                    fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificates.ToString();
            string MaximumNumberOfCertificates2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificates.ToString();

            ValidateCapabilitiesAttribute("MaximumNumberOfCertificates",
                                          MaximumNumberOfCertificatesSpecified1,
                                          MaximumNumberOfCertificatesSpecified2,
                                          MaximumNumberOfCertificates1,
                                          MaximumNumberOfCertificates2,
                                          ref sb,
                                          ref ok);

            // MaximumNumberOfCertificationPaths  
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfCertificationPathsSpecified1 =
                    !string.IsNullOrEmpty(fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificationPaths);
            bool MaximumNumberOfCertificationPathsSpecified2 =
                    !string.IsNullOrEmpty(
                            fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths);
            string MaximumNumberOfCertificationPaths1 =
                    fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificationPaths;
            string MaximumNumberOfCertificationPaths2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificationPaths;

            ValidateCapabilitiesAttribute("MaximumNumberOfCertificationPaths",
                                          MaximumNumberOfCertificationPathsSpecified1,
                                          MaximumNumberOfCertificationPathsSpecified2,
                                          MaximumNumberOfCertificationPaths1,
                                          MaximumNumberOfCertificationPaths2,
                                          ref sb,
                                          ref ok);

            // RSAKeyPairGeneration 
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool RSAKeyPairGenerationSpecified1 = fromGetServices.KeystoreCapabilities.RSAKeyPairGenerationSpecified;
            bool RSAKeyPairGenerationSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyPairGenerationSpecified;
            bool RSAKeyPairGeneration1 = fromGetServices.KeystoreCapabilities.RSAKeyPairGeneration;
            bool RSAKeyPairGeneration2 = fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyPairGeneration;

            ValidateCapabilitiesAttribute("RSAKeyPairGeneration",
                                          RSAKeyPairGenerationSpecified1,
                                          RSAKeyPairGenerationSpecified2,
                                          RSAKeyPairGeneration1,
                                          RSAKeyPairGeneration2,
                                          ref sb,
                                          ref ok);


            // RSAKeyLengths 
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool RSAKeyLengthsSpecified1 = fromGetServices.KeystoreCapabilities.RSAKeyLengths != null &&
                                           fromGetServices.KeystoreCapabilities.RSAKeyLengths.Length != 0;

            bool RSAKeyLengthsSpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyLengths != null &&
                                           fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyLengths.Length != 0;

            uint[] RSAKeyLengths1 = fromGetServices.KeystoreCapabilities.RSAKeyLengths;
            uint[] RSAKeyLengths2 = fromGetServiceCapabilities.KeystoreCapabilities.RSAKeyLengths;

            ValidateCapabilitiesAttribute("RSAKeyLengths",
                                          RSAKeyLengthsSpecified1,
                                          RSAKeyLengthsSpecified2,
                                          (int[]) (object) RSAKeyLengths1,
                                          (int[]) (object) RSAKeyLengths2,
                                          ref sb,
                                          ref ok);

            // PKCS10ExternalCertificationWithRSA
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool PKCS10ExternalCertificationWithRSASpecified1 =
                    fromGetServices.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified;
            bool PKCS10ExternalCertificationWithRSASpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSASpecified;
            bool PKCS10ExternalCertificationWithRSA1 =
                    fromGetServices.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA;
            bool PKCS10ExternalCertificationWithRSA2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.PKCS10ExternalCertificationWithRSA;

            ValidateCapabilitiesAttribute("PKCS10ExternalCertificationWithRSA",
                                          PKCS10ExternalCertificationWithRSASpecified1,
                                          PKCS10ExternalCertificationWithRSASpecified2,
                                          PKCS10ExternalCertificationWithRSA1,
                                          PKCS10ExternalCertificationWithRSA2,
                                          ref sb,
                                          ref ok);

            // SelfSignedCertificateCreationWithRSA
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool SelfSignedCertificateCreationWithRSASpecified1 =
                    fromGetServices.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified;
            bool SelfSignedCertificateCreationWithRSASpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSASpecified;
            bool SelfSignedCertificateCreationWithRSA1 =
                    fromGetServices.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA;
            bool SelfSignedCertificateCreationWithRSA2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.SelfSignedCertificateCreationWithRSA;

            ValidateCapabilitiesAttribute("SelfSignedCertificateCreationWithRSA",
                                          SelfSignedCertificateCreationWithRSASpecified1,
                                          SelfSignedCertificateCreationWithRSASpecified2,
                                          SelfSignedCertificateCreationWithRSA1,
                                          SelfSignedCertificateCreationWithRSA2,
                                          ref sb,
                                          ref ok);

            // X509Versions
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool X509VersionsSpecified1 = fromGetServices.KeystoreCapabilities.X509Versions != null &&
                                          fromGetServices.KeystoreCapabilities.X509Versions.Length != 0;

            bool X509VersionsSpecified2 = fromGetServiceCapabilities.KeystoreCapabilities.X509Versions != null &&
                                          fromGetServiceCapabilities.KeystoreCapabilities.X509Versions.Length != 0;

            int[] X509Versions1 = fromGetServices.KeystoreCapabilities.X509Versions;
            int[] X509Versions2 = fromGetServiceCapabilities.KeystoreCapabilities.X509Versions;

            ValidateCapabilitiesAttribute("X509Versions",
                                          X509VersionsSpecified1,
                                          X509VersionsSpecified2,
                                          X509Versions1,
                                          X509Versions2,
                                          ref sb,
                                          ref ok);

            // MaximumNumberOfPassphrases
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfPassphrasesSpecified1 =
                    !string.IsNullOrEmpty(fromGetServices.KeystoreCapabilities.MaximumNumberOfPassphrases);
            bool MaximumNumberOfPassphrasesSpecified2 =
                    !string.IsNullOrEmpty(fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfPassphrases);
            string MaximumNumberOfPassphrases1 = fromGetServices.KeystoreCapabilities.MaximumNumberOfPassphrases;
            string MaximumNumberOfPassphrases2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfPassphrases;

            ValidateCapabilitiesAttribute("MaximumNumberOfPassphrases",
                                          MaximumNumberOfPassphrasesSpecified1,
                                          MaximumNumberOfPassphrasesSpecified2,
                                          MaximumNumberOfPassphrases1,
                                          MaximumNumberOfPassphrases2,
                                          ref sb,
                                          ref ok);

            // PKCS8RSAKeyPairUpload
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool PKCS8RSAKeyPairUploadSpecified1 = fromGetServices.KeystoreCapabilities.PKCS8RSAKeyPairUploadSpecified;
            bool PKCS8RSAKeyPairUploadSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.PKCS8RSAKeyPairUploadSpecified;
            bool PKCS8RSAKeyPairUpload1 = fromGetServices.KeystoreCapabilities.PKCS8RSAKeyPairUpload;
            bool PKCS8RSAKeyPairUpload2 = fromGetServiceCapabilities.KeystoreCapabilities.PKCS8RSAKeyPairUpload;

            ValidateCapabilitiesAttribute("PKCS8RSAKeyPairUpload",
                                          PKCS8RSAKeyPairUploadSpecified1,
                                          PKCS8RSAKeyPairUploadSpecified2,
                                          PKCS8RSAKeyPairUpload1,
                                          PKCS8RSAKeyPairUpload2,
                                          ref sb,
                                          ref ok);

            // PKCS12CertificateWithRSAPrivateKeyUpload
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool PKCS12CertificateWithRSAPrivateKeyUploadSpecified1 =
                    fromGetServices.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUploadSpecified;
            bool PKCS12CertificateWithRSAPrivateKeyUploadSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUploadSpecified;
            bool PKCS12CertificateWithRSAPrivateKeyUpload1 =
                    fromGetServices.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload;
            bool PKCS12CertificateWithRSAPrivateKeyUpload2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.PKCS12CertificateWithRSAPrivateKeyUpload;

            ValidateCapabilitiesAttribute("PKCS12CertificateWithRSAPrivateKeyUpload",
                                          PKCS12CertificateWithRSAPrivateKeyUploadSpecified1,
                                          PKCS12CertificateWithRSAPrivateKeyUploadSpecified2,
                                          PKCS12CertificateWithRSAPrivateKeyUpload1,
                                          PKCS12CertificateWithRSAPrivateKeyUpload2,
                                          ref sb,
                                          ref ok);

            // PasswordBasedEncryptionAlgorithms
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool PasswordBasedEncryptionAlgorithmsSpecified1 =
                    fromGetServices.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms != null &&
                    fromGetServices.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms.Length != 0;

            bool PasswordBasedEncryptionAlgorithmsSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms != null &&
                    fromGetServiceCapabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms.Length != 0;

            string[] PasswordBasedEncryptionAlgorithms1 = null;

            if (PasswordBasedEncryptionAlgorithmsSpecified1)
                PasswordBasedEncryptionAlgorithms1 =
                        fromGetServices.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms;

            string[] PasswordBasedEncryptionAlgorithms2 = null;
            if (PasswordBasedEncryptionAlgorithmsSpecified2)
                PasswordBasedEncryptionAlgorithms2 =
                        fromGetServiceCapabilities.KeystoreCapabilities.PasswordBasedEncryptionAlgorithms;

            ValidateCapabilitiesAttribute("PasswordBasedEncryptionAlgorithms",
                                          PasswordBasedEncryptionAlgorithmsSpecified1,
                                          PasswordBasedEncryptionAlgorithmsSpecified2,
                                          PasswordBasedEncryptionAlgorithms1,
                                          PasswordBasedEncryptionAlgorithms2,
                                          ref sb,
                                          ref ok);

            // PasswordBasedMACAlgorithms
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool PasswordBasedMACAlgorithmsSpecified1 =
                    fromGetServices.KeystoreCapabilities.PasswordBasedMACAlgorithms != null &&
                    fromGetServices.KeystoreCapabilities.PasswordBasedMACAlgorithms.Length != 0;

            bool PasswordBasedMACAlgorithmsSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.PasswordBasedMACAlgorithms != null &&
                    fromGetServiceCapabilities.KeystoreCapabilities.PasswordBasedMACAlgorithms.Length != 0;

            string[] PasswordBasedMACAlgorithms1 = null;

            if (PasswordBasedMACAlgorithmsSpecified1)
                PasswordBasedMACAlgorithms1 = fromGetServices.KeystoreCapabilities.PasswordBasedMACAlgorithms;

            string[] PasswordBasedMACAlgorithms2 = null;
            if (PasswordBasedMACAlgorithmsSpecified2)
                PasswordBasedMACAlgorithms2 = fromGetServiceCapabilities.KeystoreCapabilities.PasswordBasedMACAlgorithms;

            ValidateCapabilitiesAttribute("PasswordBasedMACAlgorithms",
                                          PasswordBasedMACAlgorithmsSpecified1,
                                          PasswordBasedMACAlgorithmsSpecified2,
                                          PasswordBasedMACAlgorithms1,
                                          PasswordBasedMACAlgorithms2,
                                          ref sb,
                                          ref ok);

            // MaximumNumberOfCRLs
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfCRLsSpecified1 =
                    !string.IsNullOrEmpty(fromGetServices.KeystoreCapabilities.MaximumNumberOfCRLs);
            bool MaximumNumberOfCRLsSpecified2 =
                    !string.IsNullOrEmpty(fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCRLs);
            string MaximumNumberOfCRLs1 = fromGetServices.KeystoreCapabilities.MaximumNumberOfCRLs;
            string MaximumNumberOfCRLs2 = fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCRLs;

            ValidateCapabilitiesAttribute("MaximumNumberOfCRLs",
                                          MaximumNumberOfCRLsSpecified1,
                                          MaximumNumberOfCRLsSpecified2,
                                          MaximumNumberOfCRLs1,
                                          MaximumNumberOfCRLs2,
                                          ref sb,
                                          ref ok);

            // MaximumNumberOfCertificationPathValidationPolicies
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfCertificationPathValidationPoliciesSpecified1 =
                    !string.IsNullOrEmpty(
                            fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies);
            bool MaximumNumberOfCertificationPathValidationPoliciesSpecified2 =
                    !string.IsNullOrEmpty(
                            fromGetServiceCapabilities.KeystoreCapabilities
                                                      .MaximumNumberOfCertificationPathValidationPolicies);
            string MaximumNumberOfCertificationPathValidationPolicies1 =
                    fromGetServices.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies;
            string MaximumNumberOfCertificationPathValidationPolicies2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.MaximumNumberOfCertificationPathValidationPolicies;

            ValidateCapabilitiesAttribute("MaximumNumberOfCertificationPathValidationPolicies",
                                          MaximumNumberOfCertificationPathValidationPoliciesSpecified1,
                                          MaximumNumberOfCertificationPathValidationPoliciesSpecified2,
                                          MaximumNumberOfCertificationPathValidationPolicies1,
                                          MaximumNumberOfCertificationPathValidationPolicies2,
                                          ref sb,
                                          ref ok);

            // EnforceTLSWebClientAuthExtKeyUsage
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool EnforceTLSWebClientAuthExtKeyUsageSpecified1 =
                    fromGetServices.KeystoreCapabilities.EnforceTLSWebClientAuthExtKeyUsageSpecified;
            bool EnforceTLSWebClientAuthExtKeyUsageSpecified2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.EnforceTLSWebClientAuthExtKeyUsageSpecified;
            bool EnforceTLSWebClientAuthExtKeyUsage1 =
                    fromGetServices.KeystoreCapabilities.EnforceTLSWebClientAuthExtKeyUsage;
            bool EnforceTLSWebClientAuthExtKeyUsage2 =
                    fromGetServiceCapabilities.KeystoreCapabilities.EnforceTLSWebClientAuthExtKeyUsage;

            ValidateCapabilitiesAttribute("EnforceTLSWebClientAuthExtKeyUsage",
                                          EnforceTLSWebClientAuthExtKeyUsageSpecified1,
                                          EnforceTLSWebClientAuthExtKeyUsageSpecified2,
                                          EnforceTLSWebClientAuthExtKeyUsage1,
                                          EnforceTLSWebClientAuthExtKeyUsage2,
                                          ref sb,
                                          ref ok);

            // TLSServerSupported
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool TLSServerSupportedSpecified1 = fromGetServices.TLSServerCapabilities != null &&
                                                fromGetServices.TLSServerCapabilities.TLSServerSupported != null &&
                                                fromGetServices.TLSServerCapabilities.TLSServerSupported.Length != 0;

            bool TLSServerSupportedSpecified2 = fromGetServiceCapabilities.TLSServerCapabilities != null &&
                                                fromGetServiceCapabilities.TLSServerCapabilities.TLSServerSupported !=
                                                null &&
                                                fromGetServiceCapabilities.TLSServerCapabilities.TLSServerSupported
                                                                          .Length != 0;

            string[] TLSServerSupported1 = null;

            if (TLSServerSupportedSpecified1)
                TLSServerSupported1 = fromGetServices.TLSServerCapabilities.TLSServerSupported;

            string[] TLSServerSupported2 = null;
            if (TLSServerSupportedSpecified2)
                TLSServerSupported2 = fromGetServiceCapabilities.TLSServerCapabilities.TLSServerSupported;

            ValidateCapabilitiesAttribute("TLSServerSupported",
                                          TLSServerSupportedSpecified1,
                                          TLSServerSupportedSpecified2,
                                          TLSServerSupported1,
                                          TLSServerSupported2,
                                          ref sb,
                                          ref ok);

            // MaximumNumberOfTLSCertificationPaths
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfTLSCertificationPathsSpecified1 = fromGetServices.TLSServerCapabilities != null &&
                                                                  !string.IsNullOrEmpty(
                                                                          fromGetServices.TLSServerCapabilities
                                                                                         .MaximumNumberOfTLSCertificationPaths);

            bool MaximumNumberOfTLSCertificationPathsSpecified2 = fromGetServiceCapabilities.TLSServerCapabilities !=
                                                                  null &&
                                                                  !string.IsNullOrEmpty(
                                                                          fromGetServiceCapabilities
                                                                                  .TLSServerCapabilities
                                                                                  .MaximumNumberOfTLSCertificationPaths);

            string MaximumNumberOfTLSCertificationPaths1 = null;
            if (MaximumNumberOfTLSCertificationPathsSpecified1)
                MaximumNumberOfTLSCertificationPaths1 =
                        fromGetServices.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths;

            string MaximumNumberOfTLSCertificationPaths2 = null;
            if (MaximumNumberOfTLSCertificationPathsSpecified2)
                MaximumNumberOfTLSCertificationPaths2 =
                        fromGetServiceCapabilities.TLSServerCapabilities.MaximumNumberOfTLSCertificationPaths;

            ValidateCapabilitiesAttribute("MaximumNumberOfTLSCertificationPaths",
                                          MaximumNumberOfTLSCertificationPathsSpecified1,
                                          MaximumNumberOfTLSCertificationPathsSpecified2,
                                          MaximumNumberOfTLSCertificationPaths1,
                                          MaximumNumberOfTLSCertificationPaths2,
                                          ref sb,
                                          ref ok);

            // TLSClientAuthSupported
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool TLSClientAuthSupportedSpecified1 = fromGetServices.TLSServerCapabilities != null &&
                                                    fromGetServices.TLSServerCapabilities.TLSClientAuthSupportedSpecified;
            bool TLSClientAuthSupportedSpecified2 = fromGetServices.TLSServerCapabilities != null &&
                                                    fromGetServiceCapabilities.TLSServerCapabilities
                                                                              .TLSClientAuthSupportedSpecified;
            bool TLSClientAuthSupported1 = fromGetServices.TLSServerCapabilities.TLSClientAuthSupported;
            bool TLSClientAuthSupported2 = fromGetServiceCapabilities.TLSServerCapabilities.TLSClientAuthSupported;

            ValidateCapabilitiesAttribute("TLSClientAuthSupported",
                                          TLSClientAuthSupportedSpecified1,
                                          TLSClientAuthSupportedSpecified2,
                                          TLSClientAuthSupported1,
                                          TLSClientAuthSupported2,
                                          ref sb,
                                          ref ok);

            // MaximumNumberOfTLSCertificationPathValidationPolicies
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfTLSCertificationPathValidationPoliciesSpecified1 =
                    fromGetServices.TLSServerCapabilities != null &&
                    !string.IsNullOrEmpty(
                            fromGetServices.TLSServerCapabilities.MaximumNumberOfTLSCertificationPathValidationPolicies);

            bool MaximumNumberOfTLSCertificationPathValidationPoliciesSpecified2 =
                    fromGetServiceCapabilities.TLSServerCapabilities != null &&
                    !string.IsNullOrEmpty(
                            fromGetServiceCapabilities.TLSServerCapabilities
                                                      .MaximumNumberOfTLSCertificationPathValidationPolicies);

            string MaximumNumberOfTLSCertificationPathValidationPolicies1 = null;
            if (MaximumNumberOfTLSCertificationPathValidationPoliciesSpecified1)
                MaximumNumberOfTLSCertificationPathValidationPolicies1 =
                        fromGetServices.TLSServerCapabilities.MaximumNumberOfTLSCertificationPathValidationPolicies;

            string MaximumNumberOfTLSCertificationPathValidationPolicies2 = null;
            if (MaximumNumberOfTLSCertificationPathValidationPoliciesSpecified2)
                MaximumNumberOfTLSCertificationPathValidationPolicies2 =
                        fromGetServiceCapabilities.TLSServerCapabilities
                                                  .MaximumNumberOfTLSCertificationPathValidationPolicies;

            ValidateCapabilitiesAttribute("MaximumNumberOfTLSCertificationPathValidationPolicies",
                                          MaximumNumberOfTLSCertificationPathValidationPoliciesSpecified1,
                                          MaximumNumberOfTLSCertificationPathValidationPoliciesSpecified2,
                                          MaximumNumberOfTLSCertificationPathValidationPolicies1,
                                          MaximumNumberOfTLSCertificationPathValidationPolicies2,
                                          ref sb,
                                          ref ok);

            // Temprary commented according ticket #971
            /*
            // MaximumNumberOfDot1XConfigurations
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool MaximumNumberOfDot1XConfigurationsSpecified1 = fromGetServices.Dot1XCapabilities != null &&
                                                                !string.IsNullOrEmpty(
                                                                        fromGetServices.Dot1XCapabilities
                                                                                       .MaximumNumberOfDot1XConfigurations);

            bool MaximumNumberOfDot1XConfigurationsSpecified2 = fromGetServiceCapabilities.Dot1XCapabilities != null &&
                                                                !string.IsNullOrEmpty(
                                                                        fromGetServiceCapabilities.Dot1XCapabilities
                                                                                                  .MaximumNumberOfDot1XConfigurations);

            string MaximumNumberOfDot1XConfigurations1 = null;
            if (MaximumNumberOfDot1XConfigurationsSpecified1)
                MaximumNumberOfDot1XConfigurations1 =
                        fromGetServices.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations;

            string MaximumNumberOfDot1XConfigurations2 = null;
            if (MaximumNumberOfDot1XConfigurationsSpecified2)
                MaximumNumberOfDot1XConfigurations2 =
                        fromGetServiceCapabilities.Dot1XCapabilities.MaximumNumberOfDot1XConfigurations;

            ValidateCapabilitiesAttribute("MaximumNumberOfDot1XConfigurations",
                                          MaximumNumberOfDot1XConfigurationsSpecified1,
                                          MaximumNumberOfDot1XConfigurationsSpecified2,
                                          MaximumNumberOfDot1XConfigurations1,
                                          MaximumNumberOfDot1XConfigurations2,
                                          ref sb,
                                          ref ok);

            // Dot1XMethods
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool Dot1XMethodsSpecified1 = fromGetServices.Dot1XCapabilities != null &&
                                          fromGetServices.Dot1XCapabilities.Dot1XMethods != null &&
                                          fromGetServices.Dot1XCapabilities.Dot1XMethods.Length != 0;

            bool Dot1XMethodsSpecified2 = fromGetServiceCapabilities.Dot1XCapabilities != null &&
                                          fromGetServiceCapabilities.Dot1XCapabilities.Dot1XMethods != null &&
                                          fromGetServiceCapabilities.Dot1XCapabilities.Dot1XMethods.Length != 0;

            string[] Dot1XMethods1 = null;

            if (Dot1XMethodsSpecified1)
                Dot1XMethods1 = fromGetServices.Dot1XCapabilities.Dot1XMethods.Where(d => !string.IsNullOrEmpty(d)).ToArray();

            string[] Dot1XMethods2 = null;
            if (Dot1XMethodsSpecified2)
                Dot1XMethods2 = fromGetServiceCapabilities.Dot1XCapabilities.Dot1XMethods.Where(d => !string.IsNullOrEmpty(d)).ToArray();

            ValidateCapabilitiesAttribute("Dot1XMethods",
                                          Dot1XMethodsSpecified1,
                                          Dot1XMethodsSpecified2,
                                          Dot1XMethods1,
                                          Dot1XMethods2,
                                          ref sb,
                                          ref ok,
                                          ", ");
            */

            string dump = sb.ToStringTrimNewLine();

            Assert(ok,
                   dump,
                   "Check that capabilities from GetServices and GetServiceCapabilities is equal to each other");
        }

        #endregion

        #region Polling Condition

        /// <summary>
        ///     Implements condition to wait until notification for specified keyID is received or timeout is expired.
        /// </summary>
        public class WaitNotificationForKeyPollingCondition : SubscriptionHandler.PollingConditionBase
        {
            private readonly string m_WaitingNotificationsFor;
            private bool m_StopPolling;

            public WaitNotificationForKeyPollingCondition(DateTime deadline, string waitingNotificationsFor)
                    : base(deadline)
            {
                m_WaitingNotificationsFor = waitingNotificationsFor;
            }

            public string KeyStatus { get; protected set; }

            public override bool StopPulling
            {
                get { return m_StopPolling; }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        log.AppendFormat("No notification for key with ID {0}", m_WaitingNotificationsFor);

                        return log.ToString();
                    }
                    else
                        return string.Format("Notification for key with ID '{0}' is received", m_WaitingNotificationsFor);
                }
            }

            public override void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (NotificationMessageHolderType msg in messages.Keys)
                    {
                        Dictionary<string, string> sourceSI = msg.Message.GetMessageSourceSimpleItems();

                        string keyID = null;
                        if (null != sourceSI && sourceSI.ContainsKey("KeyID"))
                            keyID = sourceSI["KeyID"];

                        Dictionary<string, string> dataSI = msg.Message.GetMessageDataSimpleItems();
                        if (null != keyID && m_WaitingNotificationsFor == keyID
                                          && dataSI.ContainsKey("NewStatus") &&
                            (dataSI["NewStatus"] == AdvancedSecurityExtensions.ksOK ||
                             dataSI["NewStatus"] == AdvancedSecurityExtensions.ksCORRUPT))
                        {
                            m_StopPolling = true;
                            KeyStatus = dataSI["NewStatus"];
                        }
                    }
            }
        }

        #endregion
    }
}