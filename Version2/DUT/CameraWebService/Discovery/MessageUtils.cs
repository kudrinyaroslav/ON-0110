using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using WSD = TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.Discovery;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using System.Xml;

namespace CameraWebService.Discovery
{
    public class MessageUtils
    {
        private List<XmlSchema> _discoverySchemas;
        private string _enpointReference = string.Empty;

        public const string ONVIF_DISCOVER_TYPES = "NetworkVideoTransmitter";
        public const string ONVIF_20_DEVICE_TYPE = "Device";
        public const string ONVIF_NETWORK_WSDL_URL = "http://www.onvif.org/ver10/network/wsdl";
        public const string ONVIF_20_DEVICE_NS = "http://www.onvif.org/ver10/device/wsdl";
        private static DiscoveryType ONVIF_10_TYPE = new DiscoveryType(ONVIF_NETWORK_WSDL_URL, ONVIF_DISCOVER_TYPES);
        private static DiscoveryType ONVIF_20_TYPE = new DiscoveryType(ONVIF_20_DEVICE_NS, ONVIF_20_DEVICE_TYPE);
        private static DiscoveryType ONVIF_20_TYPE_WOPREFIX = new DiscoveryType(ONVIF_20_DEVICE_NS, ONVIF_20_DEVICE_TYPE, "");

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

        public static DiscoveryType[] GetOnvif10Type()
        {
            return new DiscoveryType[] { new DiscoveryType(ONVIF_NETWORK_WSDL_URL, ONVIF_DISCOVER_TYPES, "dn") };
        }

        public static DiscoveryType[] GetOnvif20Type()
        {
            return new DiscoveryType[] { new DiscoveryType(ONVIF_20_DEVICE_NS, ONVIF_20_DEVICE_TYPE, "tds") };
        }

        public static DiscoveryType[] GetOnvif20TypeWOPrefix()
        {
            return new DiscoveryType[] { new DiscoveryType(ONVIF_20_DEVICE_NS, ONVIF_20_DEVICE_TYPE, "") };
        }

        public static DiscoveryType[] GetOnvif10and20Type()
        {
            return new DiscoveryType[] { new DiscoveryType(ONVIF_20_DEVICE_NS, ONVIF_20_DEVICE_TYPE, "tds"), new DiscoveryType(ONVIF_NETWORK_WSDL_URL, ONVIF_DISCOVER_TYPES, "dn") };
        }

        private void ReadSchemas()
        {
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CameraWebService.Discovery.Schemas.ws-discovery.xsd");
            XmlSchema schemaDiscovery = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CameraWebService.Discovery.Schemas.addressing.xsd");
            XmlSchema schemaAddressing = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            _discoverySchemas = new List<XmlSchema> { schemaDiscovery, schemaAddressing };
        }

        public MessageUtils()
        {
            ReadSchemas();
            Assembly asm = Assembly.GetExecutingAssembly();
            //_enpointReference = "urn:uuid:" + asm.GetType().GUID.ToString();
            _enpointReference = "urn:uuid:00075f74-9d25-259d-745f-0700075f745f";
        }

        private string BuildTypes(bool onvif20, ref XmlSerializerNamespaces namespaces)
        {
            //DiscoveryType[] types = onvif20 ? GetOnvif20Type() : GetOnvif10Type();

            //DiscoveryType[] types = GetOnvif20Type(); //tds:Device
            DiscoveryType[] types = GetOnvif20TypeWOPrefix(); //Device
            //DiscoveryType[] types = GetOnvif10Type(); //dn:NVT
            //DiscoveryType[] types = GetOnvif10and20Type(); //tds:Device and n:NVT

            int j = 0;
            string strTypes = string.Empty;
            bool first = true;
            if (types != null)
            {
                foreach (DiscoveryType type in types)
                {
                    string ns = type.Namespace;
                    string t = type.Type;
                    string prefix;
                    if (string.IsNullOrEmpty(type.Prefix))
                    {
                        //prefix = string.Format("ns{0}", j);
                        prefix = "";
                        j++;
                    }
                    else
                    {
                        prefix = type.Prefix;
                    }

                    namespaces.Add(prefix, ns);
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        strTypes += " ";
                    }
                    if (prefix == "")
                    {
                        strTypes += t;
                    }
                    else
                    {
                        strTypes += prefix + ":" + t;
                    }
                }
            }
            return strTypes;
        }

        private WSD.EndpointReferenceType BuildEndpointReference()
        {
            WSD.EndpointReferenceType enpointReference = new WSD.EndpointReferenceType();
            enpointReference.Address = new WSD.AttributedURI();
            enpointReference.Address.Value = _enpointReference;
            return enpointReference;
        }

        private WSD.ScopesType BuildScopes(string[] scopeStrings, string matchBy)
        {
            WSD.ScopesType scopes = new WSD.ScopesType();
            scopes.MatchBy = matchBy;
            scopes.Text = new string[] { (scopeStrings != null ? string.Join(" ", scopeStrings) : string.Empty) };
            return scopes;
        }

        private string BuildXAddrs(string[] xAddrs)
        {
            return (xAddrs != null ? string.Join(" ", xAddrs) : string.Empty);
        }

        public byte[] BuildProbeMatches(string[] scopes, bool onvif20, string[] xAddrs, string relateTo)
        {
            WSD.ProbeMatchesType probeMatches = new WSD.ProbeMatchesType();

            probeMatches.ProbeMatch = new WSD.ProbeMatchType[1];
            WSD.ProbeMatchType probeMatch = new WSD.ProbeMatchType();

            probeMatches.Xmlns = new XmlSerializerNamespaces();
            probeMatches.Xmlns.Add("dis", "http://schemas.xmlsoap.org/ws/2005/04/discovery");
            probeMatch.Xmlns = new XmlSerializerNamespaces();
            probeMatch.Xmlns.Add("", "http://www.onvif.org/ver10/device/wsdl");

            // Scopes
            probeMatch.Scopes = BuildScopes(scopes, null);

            // Types
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("dis", "http://schemas.xmlsoap.org/ws/2005/04/discovery");
            probeMatch.Types = BuildTypes(onvif20, ref namespaces);

            // EndpointReference
            probeMatch.EndpointReference = BuildEndpointReference();

            // XAddrs
            probeMatch.XAddrs = BuildXAddrs(xAddrs);

            // MetadataVersion
            probeMatch.MetadataVersion = 0;

            probeMatches.ProbeMatch[0] = probeMatch;

            DiscoveryHeaderBuilder header = new DiscoveryHeaderBuilder();
            header.OrigingMessageId = relateTo;

            byte[] msg = SoapBuilder.BuildMessage(probeMatches, Encoding.UTF8, header, namespaces);

            return msg;
        }

        public byte[] BuildHello(
            string[] scopes, string types, XmlQualifiedName[] typesNamespaces, string[] xAddrs, uint metadataVersion)
        {
            WSD.HelloType hello = new WSD.HelloType();

            // Scopes
            hello.Scopes = BuildScopes(scopes, null);

            // Types
            hello.Types = types;

            // EndpointReference
            hello.EndpointReference = BuildEndpointReference();
            //hello.EndpointReference = new WSD.EndpointReferenceType();
            
            // XAddrs
            hello.XAddrs = BuildXAddrs(xAddrs);

            // MetadataVersion
            hello.MetadataVersion = metadataVersion;

            byte[] msg = SoapBuilder.BuildMessage(
                hello, Encoding.UTF8, new DiscoveryHeaderBuilder(), new XmlSerializerNamespaces(typesNamespaces));

            return msg;
        }

        public byte[] BuildHello(string[] scopes, bool onvif20, string[] xAddrs)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            return BuildHello(scopes, BuildTypes(onvif20, ref namespaces), namespaces.ToArray(), xAddrs, 0);
        }

        public byte[] BuildBye(string[] scopes, string types, XmlQualifiedName[] typesNamespaces, string[] xAddrs, uint metadataVersion)
        {
            WSD.ByeType bye = new WSD.ByeType();

            // Scopes
            bye.Scopes = BuildScopes(scopes, null);

            // Types
            bye.Types = types;

            // EndpointReference
            bye.EndpointReference = BuildEndpointReference();

            // XAddrs
            bye.XAddrs = BuildXAddrs(xAddrs);

            // MetadataVersion
            bye.MetadataVersion = metadataVersion;

            byte[] msg = SoapBuilder.BuildMessage(
                bye, Encoding.UTF8, new DiscoveryHeaderBuilder(), new XmlSerializerNamespaces(typesNamespaces));

            return msg;
        }

        public byte[] BuildBye(string[] scopes, bool onvif20, string[] xAddrs)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            return BuildBye(scopes, BuildTypes(onvif20, ref namespaces), namespaces.ToArray(), xAddrs, 0);
        }

        private bool ParseMessage<T>(byte[] packet, ref string messageId)
            where T : class
        {
            try
            {
                SoapMessage<T> message = SoapBuilder.ParseMessage<T>(packet, _discoverySchemas);
                messageId = string.Empty;
                foreach (XmlElement element in message.Header)
                {
                    if (element.LocalName == "MessageID")
                    {
                        messageId = element.InnerText;
                        break;
                    }
                }
                return !string.IsNullOrEmpty(messageId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ParseProbe(byte[] packet, ref string messageId)
        {
            return ParseMessage<WSD.ProbeType>(packet, ref messageId);
        }
    }
}
