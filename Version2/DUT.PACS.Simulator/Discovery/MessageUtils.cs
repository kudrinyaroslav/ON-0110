using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using System.Xml;
using DUT.PACS.Simulator.Discovery.Soap;

namespace DUT.PACS.Simulator.Discovery
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

        private void ReadSchemas()
        {
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DUT.PACS.Simulator.Discovery.Schemas.ws-discovery.xsd");
            XmlSchema schemaDiscovery = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DUT.PACS.Simulator.Discovery.Schemas.addressing.xsd");
            XmlSchema schemaAddressing = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            _discoverySchemas = new List<XmlSchema> { schemaDiscovery, schemaAddressing };
        }

        public MessageUtils()
        {
            ReadSchemas();
            Assembly asm = Assembly.GetExecutingAssembly();
            _enpointReference = "urn:uuid:" + asm.GetType().GUID.ToString();
        }

        private string BuildTypes(bool onvif20, ref XmlSerializerNamespaces namespaces)
        {
            DiscoveryType[] types = onvif20 ? GetOnvif20Type() : GetOnvif10Type();
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
                        prefix = string.Format("ns{0}", j);
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
                    strTypes += prefix + ":" + t;
                }
            }
            return strTypes;
        }

        private Proxies.WSDiscovery.EndpointReferenceType BuildEndpointReference()
        {
            Proxies.WSDiscovery.EndpointReferenceType enpointReference = new Proxies.WSDiscovery.EndpointReferenceType();
            enpointReference.Address = new Proxies.WSDiscovery.AttributedURI();
            enpointReference.Address.Value = _enpointReference;
            return enpointReference;
        }

        private Proxies.WSDiscovery.ScopesType BuildScopes(string[] scopeStrings, string matchBy)
        {
            Proxies.WSDiscovery.ScopesType scopes = new Proxies.WSDiscovery.ScopesType();
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
            Proxies.WSDiscovery.ProbeMatchesType probeMatches = new Proxies.WSDiscovery.ProbeMatchesType();

            probeMatches.ProbeMatch = new Proxies.WSDiscovery.ProbeMatchType[1];
            Proxies.WSDiscovery.ProbeMatchType probeMatch = new Proxies.WSDiscovery.ProbeMatchType();

            // Scopes
            probeMatch.Scopes = BuildScopes(scopes, null);

            // Types
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
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
            Proxies.WSDiscovery.HelloType hello = new Proxies.WSDiscovery.HelloType();

            // Scopes
            hello.Scopes = BuildScopes(scopes, null);

            // Types
            hello.Types = types;

            // EndpointReference
            hello.EndpointReference = BuildEndpointReference();

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
            Proxies.WSDiscovery.ByeType bye = new Proxies.WSDiscovery.ByeType();

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
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ParseProbe(byte[] packet, ref string messageId)
        {
            return ParseMessage<Proxies.WSDiscovery.ProbeType>(packet, ref messageId);
        }
    }
}
