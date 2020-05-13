///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ClientTestTool.TestCases.SoapValidation;
using ClientTestTool.Tests.SoapValidation.SchemaSets;

namespace ClientTestTool.Tests.SoapValidation
{
  public class SoapBuilder
  {
    private static Uri mSoapEnvelopeNamespace = new Uri("http://www.w3.org/2003/05/soap-envelope");
    private static Dictionary<Guid, XmlSerializer> mSerializersCache = new Dictionary<Guid, XmlSerializer>();

    public static String SoapEnvelopeUri
    {
      get
      {
        return mSoapEnvelopeNamespace.ToString();
      }
    }

    protected static XmlSerializer GetDeserializer<T>(XmlAttributeOverrides overrides)
        where T : class
    {
      XmlSerializer serializer = null;
      Type type = typeof(T);
      if (mSerializersCache.ContainsKey(type.GUID))
        serializer = mSerializersCache[type.GUID];
      else
      {
        serializer = new XmlSerializer(typeof(Envelope<T>), overrides);
        mSerializersCache[type.GUID] = serializer;
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
      if ((bom[0] != 0xEF) || (bom[1] != 0xBB) || (bom[2] != 0xBF))
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

      if (header != null)
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

    protected static XmlReader GetValidatingReader(TextReader content, ICollection<XmlSchema> validateSchemas)
    {
      List<XmlSchema> schemas = new List<XmlSchema>();
      schemas.AddRange(SoapSchemaSet.Instance.Schemas);

      if (null != validateSchemas)
        schemas.AddRange(validateSchemas);

      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ValidationType = ValidationType.Schema;
      settings.Schemas.XmlResolver = null;

      foreach (XmlSchema schema in schemas)
        settings.Schemas.Add(schema);

      return XmlReader.Create(content, settings);
    }

    private static String GetTypeElementName(Type type)
    {
      string ns = string.Empty;
      XmlRootAttribute[] attrs = type.GetCustomAttributes(typeof(XmlRootAttribute), false) as XmlRootAttribute[];
      if ((attrs != null) && (attrs.Length > 0))
      {
        ns = attrs[0].ElementName;
      }
      return !string.IsNullOrEmpty(ns) ? ns : type.Name;
    }

    private static String GetTypeNamespace(Type type)
    {
      string ns = string.Empty;
      XmlTypeAttribute[] attrs = type.GetCustomAttributes(typeof(XmlTypeAttribute), false) as XmlTypeAttribute[];

      if ((attrs != null) && (attrs.Length > 0))
        ns = attrs[0].Namespace;

      return ns;
    }

    protected static XmlAttributeOverrides GetAttributeOverrides<T>()
        where T : class
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
        where T : class
    {
      //override xml serialization attribute with actual values
      XmlAttributeOverrides overrides = GetAttributeOverrides<T>();

      XmlSerializer serializer = GetDeserializer<T>(overrides);

      Envelope<T> res = (Envelope<T>)serializer.Deserialize(reader);
      return res;
    }

    public static SoapMessage<T> ParseMessage<T>(TextReader message, ICollection<XmlSchema> validateSchemas)
        where T : class
    {
      XmlReader reader = GetValidatingReader(message, validateSchemas);

      //parse envelope
      Envelope<T> envelope = null;
      try
      {
        envelope = ParseEnvelope<T>(reader);
      }
      finally
      {
        message.Close();
      }

      if (null == envelope)
        return null;

      //create header elements list
      var headers = new List<XmlElement>();
      if ((null != envelope.Header) && (null != envelope.Header.Any))
        headers.AddRange(envelope.Header.Any);

      if (null == envelope.Body.Element)
      {
        if ((null != envelope.Body.Any) && (1 == envelope.Body.Any.Length))
        {
          //check if soap fault
          XmlElement element = envelope.Body.Any[0];
          Uri elementNs = !String.IsNullOrEmpty(element.NamespaceURI) ? new Uri(element.NamespaceURI) : null;
          if (element.LocalName == "Fault" && mSoapEnvelopeNamespace == elementNs)
          {
            //parse fault message
            SoapMessage<Fault> fault = ParseMessage<Fault>(message, null);
            throw new SoapFaultException(fault);
          }
        }
        throw new UnexpectedElementException(String.Format("Element <{0}> not found", GetTypeElementName(typeof(T))), headers);
      }
      return new SoapMessage<T>(headers, envelope.Body.Element);
    }
  }
}
