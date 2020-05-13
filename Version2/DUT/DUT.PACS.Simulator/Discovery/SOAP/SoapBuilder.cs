///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;

namespace DUT.PACS.Simulator.Discovery.Soap
{
    internal class UnxpectedElementException : XmlException
    {
        public List<XmlElement> Headers { get; protected set; }
        public UnxpectedElementException(string message, List<XmlElement> headers)
            : base(message)
        {
            Headers = headers;
        }
    }
    public class SoapBuilder
    {
        private static Uri _soapEnvelopeNS = new Uri("http://www.w3.org/2003/05/soap-envelope");
        protected static List<XmlSchema> _soapSchemas;
        private static Dictionary<Guid, XmlSerializer> _serializersCache = new Dictionary<Guid, XmlSerializer>();

        public static string SoapEnvelopeUri
        {
            get { return _soapEnvelopeNS.ToString(); }
        }

        static SoapBuilder()
        {
            //read schemas from assembly
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DUT.PACS.Simulator.Discovery.Schemas.xml.xsd");
            XmlSchema schemaXml = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DUT.PACS.Simulator.Discovery.Schemas.soap-envelope.xsd");
            XmlSchema schemaEnvelope = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            _soapSchemas = new List<XmlSchema> { schemaXml, schemaEnvelope };

            //put probematches type in cache to increase speed of discovery
            XmlAttributeOverrides overrides = GetAttributeOverrides<Proxies.WSDiscovery.ProbeMatchesType>();
            GetDeserializer<Proxies.WSDiscovery.ProbeMatchesType>(overrides);
        }
        
        protected static XmlSerializer GetDeserializer<T>(XmlAttributeOverrides overrides)
            where T : class
        {
            XmlSerializer serializer = null;
            Type type = typeof(T);
            if (_serializersCache.ContainsKey(type.GUID))
            {
                serializer = _serializersCache[type.GUID];
            }
            else
            {
                serializer = new XmlSerializer(typeof(Envelope<T>), overrides);
                _serializersCache[type.GUID] = serializer;
            }
            return serializer;
        }
        
        protected static byte[] ToXml(object obj, Encoding encoding, XmlSerializerNamespaces namespaces)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            XmlTextWriter writer = new XmlTextWriter(stream, encoding);

            if (namespaces != null)
            {
                serializer.Serialize(writer, obj, namespaces);
            }
            else
            {
                serializer.Serialize(writer, obj);
            }
            writer.Flush();
            
            //remove BOM if added
            byte[] bom = new byte[3];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bom, 0, 3);
            if ((bom[0] != 0xEF)||(bom[1] != 0xBB)||(bom[2] != 0xBF))
            {
                //it's not BOM - rewind stream to the begining
                stream.Seek(0, SeekOrigin.Begin);
            }

            byte[] message = new byte[stream.Length - stream.Position];
            stream.Read(message, 0, message.Length);
            writer.Close();

            return message;
        }
        
        public static byte[] BuildMessage(object obj, Encoding encoding, ISoapHeaderBuilder header)
        {
            return BuildMessage(obj, encoding, header, null);
        }
        
        public static byte[] BuildMessage(
            object obj, 
            Encoding encoding,
            ISoapHeaderBuilder header, 
            XmlSerializerNamespaces namespaces)
        {
            XmlDocument xmlBody = new XmlDocument();
            MemoryStream streamBody = new MemoryStream(ToXml(obj, encoding, null));
            xmlBody.Load(streamBody);
            streamBody.Close();

            Envelope envelope = new Envelope();
            envelope.Body = new Body();
            envelope.Body.Any = new XmlElement[] { xmlBody.DocumentElement };

            if(header != null)
            {
                MemoryStream streamHeader = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(streamHeader, Encoding.UTF8);
                writer.WriteStartDocument();
                writer.WriteStartElement("Header");
                header.WriteHeader(writer, obj);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                streamHeader.Seek(0, SeekOrigin.Begin);
                XmlDocument xmlHead = new XmlDocument();
                xmlHead.Load(streamHeader);
                streamHeader.Close();

                envelope.Header = new Header();
                List<XmlElement> elements = new List<XmlElement>();
                foreach (XmlElement element in xmlHead.DocumentElement.ChildNodes)
                {
                    elements.Add(element);
                }
                envelope.Header.Any = elements.ToArray();
            }

            return ToXml(envelope, encoding, namespaces);
        }
        
        protected static XmlReader GetValidatingReader(Stream stream, ICollection<XmlSchema> validateSchemas)
        {
            List<XmlSchema> schemas = new List<XmlSchema>();
            schemas.AddRange(_soapSchemas);
            if (validateSchemas != null)
            {
                schemas.AddRange(validateSchemas);
            }
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.XmlResolver = null; //disable resolver - all schemas should be in place
            foreach (XmlSchema schema in schemas)
            {
                settings.Schemas.Add(schema);
            }
            return XmlReader.Create(stream, settings);
        }
        
        private static string GetTypeElementName(Type type)
        {
            string ns = string.Empty;
            XmlRootAttribute[] attrs = type.GetCustomAttributes(typeof(XmlRootAttribute), false) as XmlRootAttribute[];
            if ((attrs != null) && (attrs.Length > 0))
            {
                ns = attrs[0].ElementName;
            }
            return !string.IsNullOrEmpty(ns) ? ns : type.Name;
        }
        
        private static string GetTypeNamespace(Type type)
        {
            string ns = string.Empty;
            XmlTypeAttribute[] attrs = type.GetCustomAttributes(typeof(XmlTypeAttribute), false) as XmlTypeAttribute[];
            if((attrs != null)&&(attrs.Length > 0))
            {
                ns = attrs[0].Namespace;
            }
            return ns;
        }
        
        private static XmlAttributeOverrides GetAttributeOverrides<T>()
            where T: class
        {
            XmlElementAttribute attr = new XmlElementAttribute();
            attr.ElementName = GetTypeElementName(typeof(T));
            attr.Namespace = GetTypeNamespace(typeof(T));
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlElements.Add(attr);
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            overrides.Add(typeof(Body<T>), "Element", attrs);
            return overrides;
        }

        private static Envelope<T> ParseEnvelope<T>(XmlReader reader)
            where T:class 
        {
            //override xml serialization attribute with actual values
            XmlAttributeOverrides overrides = GetAttributeOverrides<T>();

            XmlSerializer serializer = GetDeserializer<T>(overrides);
            Envelope<T> res = (Envelope<T>)serializer.Deserialize(reader);

            return res;
        }

        public static SoapMessage<T> ParseMessage<T>(byte[] message, ICollection<XmlSchema> validateSchemas)
            where T : class
        {
            MemoryStream streamMessage = new MemoryStream(message);
            XmlReader reader  = GetValidatingReader(streamMessage, validateSchemas);

            //parse envelope
            Envelope<T> envelope = null;
            try
            {
                envelope = ParseEnvelope<T>(reader);
            }
            finally
            {
                streamMessage.Close();
            }

            //create header elements list
            List<XmlElement> headers = new List<XmlElement>();
            if ((envelope.Header != null) && (envelope.Header.Any != null))
            {
                foreach (XmlElement header in envelope.Header.Any)
                {
                    headers.Add(header);
                }
            }
            if(envelope.Body.Element == null)
            {
                if((envelope.Body.Any != null)&&(envelope.Body.Any.Length == 1))
                {
                    //check if soap fault
                    XmlElement element = envelope.Body.Any[0];
                    Uri elementNS = !string.IsNullOrEmpty(element.NamespaceURI) ? new Uri(element.NamespaceURI) : null;
                    if ((element.LocalName == "Fault") && (Uri.Equals(_soapEnvelopeNS, elementNS)))
                    {
                        //parse fault message
                        SoapMessage<Fault> fault = ParseMessage<Fault>(message, null);
                        throw new SoapFaultException(fault); 
                    }
                }
                throw new UnxpectedElementException(
                    string.Format("Element <{0}> not found", GetTypeElementName(typeof(T))),
                    headers);
            }
            return new SoapMessage<T>(headers, envelope.Body.Element, message);
        }
    }
}
