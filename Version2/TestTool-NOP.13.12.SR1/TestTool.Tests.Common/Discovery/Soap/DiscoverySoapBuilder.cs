using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;
using TestTool.Tests.Common.Soap;
using WSD = TestTool.Proxies.WSDiscovery;

namespace TestTool.Tests.Common.Discovery
{
    class DiscoverySoapBuilder : SoapBuilder
    {
        static DiscoverySoapBuilder()
        {
            //read schemas from assembly
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TestTool.Tests.Common.Discovery.Schemas.xml.xsd");
            XmlSchema schemaXml = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TestTool.Tests.Common.Discovery.Schemas.soap-envelope.xsd");
            XmlSchema schemaEnvelope = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            _soapSchemas = new List<XmlSchema> { schemaXml, schemaEnvelope };

            //put probematches type in cache to increase speed of discovery
            XmlAttributeOverrides overrides = GetAttributeOverrides<WSD.ProbeMatchesType>();
            GetDeserializer<WSD.ProbeMatchesType>(overrides);
        }

    }
}
