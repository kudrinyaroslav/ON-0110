///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Globalization;
using TestTool.Proxies.Onvif;
using WSD = TestTool.Proxies.WSDiscovery;
using Net = System.Net;

namespace TestTool.Tests.Common.Discovery
{
    public class DiscoveryUtils
    {
        public const string ONVIF_DISCOVER_TYPES = "NetworkVideoTransmitter";
        public const string WS_DISCOVER_ADDRESSING_NS = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
        public const string WS_DISCOVER_NS = "http://schemas.xmlsoap.org/ws/2005/04/discovery";
        public const string SOAP_ENVELOPE_NS = "http://www.w3.org/2003/05/soap-envelope";
        public const string ONVIF_NETWORK_WSDL_URL = "http://www.onvif.org/ver10/network/wsdl";
        private static string[] _mandatoryScopes = new string[] 
        { 
            "onvif://www.onvif.org/type",
            "onvif://www.onvif.org/location",
            "onvif://www.onvif.org/hardware",
            "onvif://www.onvif.org/name" 
        };

        public static string[] GetManadatoryScopes()
        {
            return _mandatoryScopes;
        }
        private static bool CheckDeviceType(byte[] message, string typesPath)
        {
            bool res = false;
            MemoryStream stream = new MemoryStream(message);
            try
            {
                XPathDocument doc = new XPathDocument(stream);
                XPathNavigator navigator = doc.CreateNavigator();
                XmlNamespaceManager nsManager = new XmlNamespaceManager(navigator.NameTable);
                nsManager.AddNamespace("s", SOAP_ENVELOPE_NS);
                nsManager.AddNamespace("wsd", WS_DISCOVER_NS);

                XPathNavigator typesNode = navigator.SelectSingleNode(typesPath, nsManager);
                if ((typesNode != null)&&(!string.IsNullOrEmpty(typesNode.InnerXml)))
                {
                    IDictionary<string, string> namespaces = typesNode.GetNamespacesInScope(XmlNamespaceScope.All);
                    string[] types = typesNode.InnerXml.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string type in types)
                    {
                        string[] splitType = type.Split(':');
                        if(splitType.Length == 2)
                        {
                            if (splitType[1] == ONVIF_DISCOVER_TYPES)
                            {
                                string ns = splitType[0];
                                if (namespaces.ContainsKey(ns))
                                {
                                    if(CompareUri(ONVIF_NETWORK_WSDL_URL, namespaces[ns]))
                                    {
                                        res = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                stream.Close();
            }
            return res;
        }
        public static bool CheckDeviceMatchType(SoapMessage<WSD.ProbeMatchesType> message, int matchNumber)
        {
            return CheckDeviceType(message.Raw, string.Format("/s:Envelope/s:Body/wsd:ProbeMatches/wsd:ProbeMatch/wsd:Types[{0}]", matchNumber + 1));
        }
        public static bool CheckDeviceHelloType(SoapMessage<WSD.HelloType> message)
        {
            return CheckDeviceType(message.Raw, "/s:Envelope/s:Body/wsd:Hello/wsd:Types");
        }
        /*public static bool CheckDiscoveryTypesNS(byte[] xml, out Uri nsUri)
        {
            bool res = true;
            nsUri = null;
            MemoryStream stream = new MemoryStream(xml);
            try
            {
                XPathDocument doc = new XPathDocument(stream);
                XPathNavigator navigator = doc.CreateNavigator();
                XPathNodeIterator iter = navigator.SelectDescendants("Types", WS_DISCOVER_NS, false);
                if (iter.Count == 1)
                {
                    iter.MoveNext();
                    XPathNavigator navTypes = iter.Current;
                    string types = navTypes.InnerXml;
                    if (!string.IsNullOrEmpty(types))
                    {
                        //TODO parse list of types
                        IDictionary<string, string> namespaces = navTypes.GetNamespacesInScope(XmlNamespaceScope.All);
                        string[] splitTypes = types.Split(':');
                        if(splitTypes.Length == 2)
                        {
                            string ns = splitTypes[0];
                            if(namespaces.ContainsKey(ns))
                            {
                                nsUri = new Uri(namespaces[ns]);
                                res = CompareUri(ONVIF_NETWORK_WSDL_URL, namespaces[ns]);
                            }
                            else
                            {
                                res = false;
                            }
                        }
                        else
                        {
                            res = false;
                        }
                    }
                    else
                    {
                        res = false;
                    }
                }
            }
            catch
            {
                stream.Close();
                res = false;
            }
            stream.Close();
            return res;
        }*/
        protected static Guid ParseUUID(string uuid)
        {
            Guid guid = Guid.Empty;
            if(uuid.StartsWith("uuid:") && (uuid.Length > 5))
            {
                guid = new Guid(uuid.Substring(5, uuid.Length - 5));
            }
            return guid;
        }
        public static bool CompareUUID(string a, string b)
        {
            Guid guidA = ParseUUID(a);
            Guid guidB = ParseUUID(b);
            return guidA == guidB;
        }
        public static bool CompareAddresses(Net.IPAddress a, Net.IPAddress b)
        {
            bool res = true;
            if ((a == null) && (b == null))
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            if (a.AddressFamily != b.AddressFamily)
            {
                return false;
            }
            if (a.AddressFamily == Net.Sockets.AddressFamily.InterNetworkV6)
            {
                //IPAddress.Equals works wrong for ipv6 if address has different scope id
                byte[] bytesA = a.GetAddressBytes();
                byte[] bytesB = b.GetAddressBytes();
                for (int i = 0; i < bytesA.Length; i++)
                {
                    if (bytesA[i] != bytesB[i])
                    {
                        return false;
                    }
                }

            }
            else
            {
                res = IPAddress.Equals(a, b);
            }
            return res;
        }
        public static bool CompareUri(string uriA, string uriB)
        {
            Uri uri1 = new Uri(uriA);
            Uri uri2 = new Uri(uriB);
            return Uri.Equals(uri1, uri2);
        }
        public static string ExtractMessageId(byte[] message)
        {
            return ExtractElementValue(message, "MessageID", WS_DISCOVER_ADDRESSING_NS);
        }
        public static string ExtractRelatesTo(ICollection<XmlElement> header)
        {
            string res = string.Empty;
            foreach (XmlElement element in header)
            {
                if((element.LocalName == "RelatesTo")&&CompareUri(element.NamespaceURI, WS_DISCOVER_ADDRESSING_NS))
                {
                    res = element.InnerText;
                    break;
                }
            }
            return res;
        }
        protected static string ExtractElementValue(byte[] xml, string element, string elementNSUri)
        {
            string res = string.Empty;
            MemoryStream stream = new MemoryStream(xml);
            XPathDocument doc = new XPathDocument(stream);
            XPathNavigator navigator = doc.CreateNavigator();
            XPathNodeIterator iter = navigator.SelectDescendants(element, elementNSUri, false);
            if (iter.Count == 1)
            {
                iter.MoveNext();
                res = iter.Current.InnerXml;
            }
            stream.Close();
            return res;
        }
        protected static bool FindScope(IEnumerable<string> scopes, string scope)
        {
            bool found = false;
            foreach (string value in scopes)
            {
                if (value.StartsWith(scope, true, CultureInfo.InvariantCulture))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
        public static string GetMissingMandatoryScope(Scope[] scopes)
        {
            string[] strScopes = new string[scopes.Length];
            int i = 0;
            foreach (Scope scope in scopes)
            {
                strScopes[i++] = scope.ScopeItem;
            }
            return GetMissingMandatoryScope(strScopes);
        }
        public static string GetMissingMandatoryScope(string[] scopes)
        {
            return GetMissingScope(scopes, _mandatoryScopes);
        }
        public static string GetMissingScope(string[] deviceScopes, string[] scopesToCheck)
        {
            List<string> devidedScopes = new List<string>();
            if (deviceScopes != null)
            {
                foreach (string scope in deviceScopes)
                {
                    string[] separators = new string[] {Environment.NewLine, " "};
                    devidedScopes.AddRange(scope.Split(separators, StringSplitOptions.RemoveEmptyEntries));
                }
                foreach (string scope in scopesToCheck)
                {
                    if (!FindScope(devidedScopes, scope))
                    {
                        return scope;
                    }
                }
            }
            return string.Empty;
        }
        public static Net.IPAddress GetIP(string hostName, bool ipv6)
        {
            Net.IPAddress address = null;
            if (!string.IsNullOrEmpty(hostName))
            {
                try
                {
                    if (!Net.IPAddress.TryParse(hostName, out address))
                    {
                        Net.IPHostEntry host = Net.Dns.GetHostEntry(hostName);
                        if (host != null)
                        {
                            foreach (Net.IPAddress ip in host.AddressList)
                            {
                                if(((ipv6)&&(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6))||
                                  ((!ipv6) && (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)))
                                {
                                    address = ip;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return address;
        }
        protected static XPathNavigator SelectChild(XPathNavigator navigator, string name, string namespaceURI)
        {
            XPathNavigator res = null;
            XPathNodeIterator iter = navigator.SelectDescendants(name, namespaceURI, false);
            if (iter.Count > 0)
            {
                iter.MoveNext();
                res = iter.Current;
            }
            return res;
        }
        protected static bool ValidateXMLValue(XPathNavigator navigator, string value, string expectedValue, string namespaceURI)
        {
            bool res = false;
            string[] splitValue = value.Split(':');
            if(splitValue.Length == 2)
            {
                string ns = splitValue[0];
                string text = splitValue[1];
                if(text == value)
                {
                    //check ns
                    IDictionary<string, string> namespaces = navigator.GetNamespacesInScope(XmlNamespaceScope.All);
                    if(namespaces.ContainsKey(ns))
                    {
                        res = CompareUri(namespaces[ns], namespaceURI);
                    }
                }
            }
            return res;
        }
        public static bool IsCorrectSoapFault(Fault fault, string code, string codeNS, string subcode, string subCodeNS, out string reason)
        {
            reason = null;
            if(fault.Code.Value.Name != code)
            {
                reason = string.Format("Invalid fault code returned. Expected: {0} - Actual: {1}", code, fault.Code.Value.Name);
                return false;
            }
            if (!CompareUri(fault.Code.Value.Namespace, codeNS))
            {
                reason = string.Format("Invalid fault code namespace. Expected: {0} - Actual: {1}", codeNS, fault.Code.Value.Namespace);
                return false;
            }
            if (fault.Code.Subcode.Value.Name != subcode)
            {
                reason = string.Format("Invalid fault subcode returned. Expected: {0} - Actual: {1}", subcode, fault.Code.Subcode.Value.Name);
                return false;
            }
            if (!CompareUri(fault.Code.Subcode.Value.Namespace, subCodeNS))
            {
                reason = string.Format("Invalid fault subcode namespace. Expected: {0} - Actual: {1}", subCodeNS, fault.Code.Subcode.Value.Namespace);
                return false;
            }
            return true;
        }
        public static string XmlToString(byte[] xml)
        {
            return Encoding.UTF8.GetString(xml);

            //string res = string.Empty;
            //MemoryStream ms = new MemoryStream();
            //ms.Write(xml, 0, xml.Length);
            //try
            //{
            //    XmlDocument doc = new XmlDocument();
            //    doc.Load(ms);
            //    res = doc.InnerXml;
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    ms.Close();
            //}
            //return res;
        }
        public static List<DeviceDiscoveryData> GetDevices(SoapMessage<WSD.ProbeMatchesType> message, Net.IPAddress sender)
        {
            List<DeviceDiscoveryData> devices = new List<DeviceDiscoveryData>();
            if (message.Object.ProbeMatch != null)
            {
                for (int i = 0; i < message.Object.ProbeMatch.Length; i++)
                {
                    WSD.ProbeMatchType match = message.Object.ProbeMatch[i];
                    if ((match.XAddrs != null)&&(CheckDeviceMatchType(message, i)))
                    {
                        DeviceDiscoveryData device = new DeviceDiscoveryData();
                        device.Type = match.Types;
                        device.Scopes = match.Scopes.Text[0];
                        device.EndPointAddress = sender.ToString();

                        string[] addresses = match.XAddrs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        device.ServiceAddresses.AddRange(addresses);
                        device.UUID = match.EndpointReference.Address.Value;
                        device.MetadataVersion = match.MetadataVersion;
                        devices.Add(device);
                    }
                }
            }
            return devices;
        }
        public static string GetDeviceId(WSD.ProbeMatchesType matches)
        {
            string id = string.Empty;
            if ((matches != null) &&
                (matches.ProbeMatch != null) && 
                (matches.ProbeMatch.Length > 0) && 
                (matches.ProbeMatch[0].EndpointReference != null)&&
                (matches.ProbeMatch[0].EndpointReference.Address != null))
            {
                id = matches.ProbeMatch[0].EndpointReference.Address.Value;
            }
            return id;
        }
        public static string GetDeviceId(WSD.HelloType hello)
        {
            string id = string.Empty;
            if ((hello != null) &&
                (hello.EndpointReference != null)&&
                (hello.EndpointReference.Address != null))
            {
                id = hello.EndpointReference.Address.Value;
            }
            return id;
        }
        public static string GetDeviceId(WSD.ByeType bye)
        {
            string id = string.Empty;
            if ((bye != null) &&
                (bye.EndpointReference != null) &&
                (bye.EndpointReference.Address != null))
            {
                id = bye.EndpointReference.Address.Value;
            }
            return id;
        }
    }
}
