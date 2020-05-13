using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;

namespace Soap
{
    public class SoapFaultException : Exception
    {
        public SoapFaultException(Fault fault) : base()
        {
            Fault = fault;
        }
        public Fault Fault { get; set; }
    }
    class SoapBuilder
    {
        private static Uri _soapEnvelopeNS = new Uri("http://www.w3.org/2003/05/soap-envelope");
        protected static List<XmlSchema> _soapSchemas;

        static SoapBuilder()
        {
            //read schemas from assemby
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SoapSerializer.Schemas.xml.xsd");
            XmlSchema schemaXml = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SoapSerializer.Schemas.soap-envelope.xsd");
            XmlSchema schemaEnvelope = XmlSchema.Read(schemaStream, null);
            schemaStream.Close();
            _soapSchemas = new List<XmlSchema> { schemaXml, schemaEnvelope };
        }
        protected static T FromXml<T>(byte[] xml)
        {
            T res = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(xml);
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                res = (T)serializer.Deserialize(stream);
            }
            catch(Exception e)
            {
                stream.Close();
                throw e;
            }
            stream.Close();
            return res;
        }
        protected static T FromXmlString<T>(string xml)
        {
            T res = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(xml);
            try
            {
                res = (T)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                reader.Close();
                throw e;
            }
            reader.Close();
            return res;
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
            writer.Close();
            return stream.ToArray();
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

            return ToXml(envelope,encoding, namespaces);
        }
        protected static void Validate(byte[] message, ICollection<XmlSchema> validateSchemas)
        {
            MemoryStream streamMessage = new MemoryStream(message);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.XmlResolver = null; //disable resolver - all schemas should be in place
            foreach (XmlSchema schema in validateSchemas)
            {
                settings.Schemas.Add(schema);
            }
            XmlReader reader = XmlReader.Create(streamMessage, settings);
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(reader);
            }
            catch (Exception e)
            {
                streamMessage.Close();
                reader.Close();
                throw e;
            }
            streamMessage.Close();
            reader.Close();
        }
        public static T ParseMessage<T>(byte[] message, ICollection<XmlSchema> validateSchemas, out ICollection<XmlElement> headers)
        {
            //validate xml schemas
            List<XmlSchema> schemas = new List<XmlSchema>();
            schemas.AddRange(_soapSchemas);
            if(validateSchemas != null)
            {
                schemas.AddRange(validateSchemas);
            }
            Validate(message, schemas);

            T res = default(T);
            Envelope envelope = FromXml<Envelope>(message);
            headers = new List<XmlElement>();
            headers.Clear();
            if((envelope.Header != null)&&(envelope.Header.Any != null))
            {
                //save headers for future processing
                foreach (XmlElement header in envelope.Header.Any)
                {
                    headers.Add(header);
                }
            }
            if ((envelope.Body == null) || (envelope.Body.Any == null) || (envelope.Body.Any.Length != 1))
            {
                throw new XmlException("Invalid soap body");
            }
            XmlElement element = envelope.Body.Any[0];
            Uri elementNS = !string.IsNullOrEmpty(element.NamespaceURI) ? new Uri(element.NamespaceURI) : null;
            if ((element.LocalName == "Fault") && (Uri.Equals(_soapEnvelopeNS, elementNS)))
            {
                //fault returned by client
                Fault fault = FromXmlString<Fault>(element.OuterXml);
                throw new SoapFaultException(fault);
            }
            res = FromXmlString<T>(element.OuterXml);

            return res;
        }
    }
}
