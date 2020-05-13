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
using TestTool.Tests.Common.Soap;
using WSD = TestTool.Proxies.WSDiscovery;
using Net = System.Net;
using System.Linq;

namespace TestTool.Tests.Common.Discovery
{
    public class DiscoveryUtils
    {
        public const string ONVIF_DISCOVER_TYPES = "NetworkVideoTransmitter";
        public const string ONVIF_20_DEVICE_TYPE = "Device"; 

        public const string WS_DISCOVER_ADDRESSING_NS = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
        public const string WS_DISCOVER_NS = "http://schemas.xmlsoap.org/ws/2005/04/discovery";
        public const string SOAP_ENVELOPE_NS = "http://www.w3.org/2003/05/soap-envelope";
        // tds = http://www.onvif.org/ver10/device/wsdl
        // dn  = http://www.onvif.org/ver10/network/wsdl/
        public const string ONVIF_NETWORK_WSDL_URL = "http://www.onvif.org/ver10/network/wsdl";
        public const string ONVIF_20_DEVICE_NS = "http://www.onvif.org/ver10/device/wsdl";

        public const string NVT_SCOPE = "onvif://www.onvif.org/type/Network_Video_Transmitter";
        
        private static DiscoveryType ONVIF_10_TYPE = new DiscoveryType(ONVIF_NETWORK_WSDL_URL, ONVIF_DISCOVER_TYPES);
        private static DiscoveryType ONVIF_20_TYPE = new DiscoveryType(ONVIF_20_DEVICE_NS, ONVIF_20_DEVICE_TYPE);

        public class DiscoveryType
        {
            public string Namespace { get; set; }
            public string Type { get; set; }
            public string Prefix { get; set; }

            public DiscoveryType(string ns, string type)
                : this(ns, type, string.Empty)
            {
            }

            public DiscoveryType(string ns, string type, string prefix)
            {
                Namespace = ns;
                Type = type;
                Prefix = prefix;
            }

        }

        private static string[] _mandatoryScopes = new string[] 
        { 
            //"onvif://www.onvif.org/type",
            //"onvif://www.onvif.org/location",
            "onvif://www.onvif.org/hardware",
            "onvif://www.onvif.org/name" 
        };

        public static string[] GetManadatoryScopes()
        {
            return _mandatoryScopes;
        }

        public static DiscoveryType[] GetOnvif10Type()
        {
            return new DiscoveryType[] { new DiscoveryType(ONVIF_NETWORK_WSDL_URL, ONVIF_DISCOVER_TYPES, "dn")};
        }

        public static DiscoveryType[] GetOnvif20Type()
        {
            return new DiscoveryType[] { new DiscoveryType(ONVIF_20_DEVICE_NS, ONVIF_20_DEVICE_TYPE, "tds") };
        }

        private static bool CheckDeviceType(
            byte[] message, string typesPath, WSD.ScopesType scopes, out string reason, bool mode1, bool mode2)
        {
            reason = string.Empty;
            bool ret = true;
            //AnnaT, 2011.11.18 : for 1.0 style
            if (mode1)
            {
                bool device10 = CheckDeviceType(message, typesPath, GetOnvif10Type());

                if (!device10)
                {
                    reason = string.Format("Device type is not {0} with namespace {1}",
                        ONVIF_DISCOVER_TYPES, ONVIF_NETWORK_WSDL_URL);
                }
                ret = ret && device10;
            }
            if (mode2)
            {
                bool device20 = CheckDeviceType(message, typesPath, GetOnvif20Type());

                if (!device20)
                {
                    if (!string.IsNullOrEmpty(reason))
                    {
                        reason += ", ";
                    }
                    reason += string.Format("Device type is not {0} with namespace {1}",
                        ONVIF_20_DEVICE_TYPE, ONVIF_20_DEVICE_NS);
                }
                ret = ret && device20;
            }
            return ret;

            // for latest core spec
            //if (!CheckDeviceType(message, typesPath, GetOnvif20Type()))
            //{
            //    reason = string.Format("Device type is not {0} with namespace {1}", ONVIF_20_DEVICE_TYPE, ONVIF_20_DEVICE_NS);
            //    return false;
            //}
            //bool hasNvtScope = ((scopes != null) && (scopes.Text != null)
            //    && (!string.IsNullOrEmpty(GetMissingScope(scopes.Text, new string[] { NVT_SCOPE }))));
            //if (hasNvtScope == CheckDeviceType(message, typesPath, GetOnvif10Type()))
            //{
            //    reason = string.Empty;
            //    return true;
            //}
            //else
            //{
            //    if (hasNvtScope)
            //    {
            //        reason = string.Format(
            //            "Device has scope {0} but device type is not {1} with namespace {2}",
            //            NVT_SCOPE, ONVIF_20_DEVICE_TYPE, ONVIF_20_DEVICE_NS);
            //    }
            //    else
            //    {
            //        reason = string.Format(
            //            "Device type is not {0} with namespace {1} but device scope {2} is missing",
            //            ONVIF_DISCOVER_TYPES, ONVIF_NETWORK_WSDL_URL, NVT_SCOPE);
            //    }
            //    return false;
            //}
        }

        private static bool CheckDeviceType(byte[] message, 
            string typesPath, 
            DiscoveryUtils.DiscoveryType[] requestTypes)
        {
            bool allFound = true;
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
                   
                    foreach (DiscoveryUtils.DiscoveryType requestType in requestTypes)
                    {
                        string typeName = requestType.Type;
                        string typeNs = requestType.Namespace;

                        bool res = false;

                        foreach (string type in types)
                        {
                            string[] splitType = type.Split(':');
                            if(splitType.Length == 2)
                            {
                                if (splitType[1] == typeName)
                                {
                                    string ns = splitType[0];
                                    if (namespaces.ContainsKey(ns))
                                    {
                                        if (CompareUri(typeNs, namespaces[ns]))
                                        {
                                            res = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        allFound = allFound && res;
                    }
                }
            }
            finally
            {
                stream.Close();
            }
            return allFound;
        }

        public static bool CheckDeviceMatchType(SoapMessage<WSD.ProbeMatchesType> message, int matchNumber, out string reason, bool mode1, bool mode2)
        {
            return CheckDeviceType(message.Raw, 
                string.Format("/s:Envelope/s:Body/wsd:ProbeMatches/wsd:ProbeMatch/wsd:Types[{0}]", matchNumber + 1),
                message.Object.ProbeMatch[matchNumber].Scopes, out reason, mode1, mode2);
        }

        public static bool CheckDeviceMatchType(SoapMessage<WSD.ProbeMatchesType> message, int matchNumber, bool mode1, bool mode2)
        {
            string reason;
            return CheckDeviceMatchType(message, matchNumber, out reason, mode1, mode2);
        }

        public static bool CheckDeviceMatchType(
            SoapMessage<WSD.ProbeMatchesType> message, int matchNumber, DiscoveryUtils.DiscoveryType[][] requestTypes)
        {
            string typesPath = string.Format("/s:Envelope/s:Body/wsd:ProbeMatches/wsd:ProbeMatch/wsd:Types[{0}]", matchNumber + 1);
            bool res = false;
            foreach (DiscoveryUtils.DiscoveryType[] types in requestTypes)
            {
                if (res = (res || CheckDeviceType(message.Raw, typesPath, types)))
                {
                    break;
                }
            }
            return res;
        }

        public static bool CheckDeviceHelloType(SoapMessage<WSD.HelloType> message, out string reason, bool mode1, bool mode2)
        {
            return CheckDeviceType(message.Raw, "/s:Envelope/s:Body/wsd:Hello/wsd:Types", 
                message.Object.Scopes, out reason, mode1, mode2);
        }
               
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

        protected static bool FindScopeExact(IEnumerable<string> scopes, string scope)
        {
            bool found = false;
            foreach (string value in scopes)
            {
                if (StringComparer.InvariantCulture.Compare(value, scope) == 0)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public static bool HasScope(IEnumerable<string> scopes, string scope)
        {
            return FindScopeExact(scopes, scope);
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
        }
        
        public static List<DeviceDiscoveryData> GetDevices(
            SoapMessage<WSD.ProbeMatchesType> message, Net.IPAddress sender, DiscoveryType[][] types)
        {
            List<DeviceDiscoveryData> devices = new List<DeviceDiscoveryData>();
            if (message.Object.ProbeMatch != null)
            {
                for (int i = 0; i < message.Object.ProbeMatch.Length; i++)
                {
                    WSD.ProbeMatchType match = message.Object.ProbeMatch[i];
                    if ((match.XAddrs != null) 
                        && (types == null 
                            //check devices according to Test Spec Annex A.1
                            ? CheckDeviceMatchType(message, i, true, false) 
                            //check if device contains at least one item from types array
                            : CheckDeviceMatchType(message, i, types)))
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
