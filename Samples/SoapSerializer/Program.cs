using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteDiscovery;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Soap;
using System.Xml.Schema;
using System.Reflection;
using System.Text;

namespace SoapSerializer
{
    class Program
    {
        static void Main(string[] args)
        {
            ProbeType probe = new ProbeType();
            probe.Types = "dn:VideoTransmitter";
            probe.Scopes = new ScopesType();
            probe.Scopes.MatchBy = "trololo";

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("dn", "http://www.onvif.org/ver10/network/wsdl");
            DiscoveryHeaderBuilder header = new DiscoveryHeaderBuilder();
            byte[] soap = SoapBuilder.BuildMessage(probe, Encoding.UTF8, header, namespaces);

            ICollection<XmlElement> headers;
            ProbeType parsed = SoapBuilder.ParseMessage<ProbeType>(soap, GetSchemas(), out headers);
        }

        static List<XmlSchema> GetSchemas()
        {
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SoapSerializer.Schemas.ws-discovery.xsd");
            XmlSchema schemaDiscovery = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();

            schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SoapSerializer.Schemas.addressing.xsd");
            XmlSchema schemaAddressing = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            return new List<XmlSchema> { schemaDiscovery, schemaAddressing };
        }
    }
}
