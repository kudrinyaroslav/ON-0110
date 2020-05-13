/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSXML2;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Collections;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Net.NetworkInformation;



namespace ONVIF_TestCases
{
    #region Exception Code
    [Serializable]
    public class _MessageException : Exception
    {
        public string ErrorMessage
        {
            get
            {
                return base.Message.ToString();
            }
        }

        public _MessageException(string errorMessage)
            : base(errorMessage) { }

        public _MessageException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    public class XML_SerializerException : _MessageException
    {
    public XML_SerializerException(string errorMessage)
                                 : base(errorMessage) {}

    public XML_SerializerException(string errorMessage, Exception innerEx)
                                 : base(errorMessage, innerEx) {}
    }

    public class XML_DeserializerException : _MessageException
    {
        public XML_DeserializerException(string errorMessage)
            : base(errorMessage) { }

        public XML_DeserializerException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }

    }

    public class NetworkInterface_SOAPError : _MessageException
    {
        public NetworkInterface_SOAPError(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_SOAPError(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    #endregion

    

    public class TestMessages
    {
       

        #region Private constants

        /// <summary>
        /// Tag used to indentify the SOAP body area in the SOAP message
        /// </summary>
        private const string SoapBodyMessageTag = "%SOAPBODY_MSG%";
        private const string SoapNameSpaceTag = "%SOAPNAMESPACE_MSG%";
        private const string SoapHeaderTag = "%SOAPHEADER_MSG%";

        private const string SoapActionTag = "%SOAPACTION_MSG%";
        private const string SoapMessageIDTag = "%SOAPMSGID_MSG%";

        private const string SoapMessageAppSeqTag = "%SOAPAPPSEQ_MSG%";
        private const string SoapMessageNumberTag = "%SOAPMSGNUMBER_MSG%";
        private const string SoapInstanceIDTag = "%SOAPINSTANCEID_MSG%";

        /// <summary>
        /// XML encoding string used after the XML serialization
        /// </summary>
        private const string XML_EncodingString = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";

        /// <summary>
        /// Soap wrapper used for SOAP message construct
        /// </summary>
        //private const string GenericSoapWrapper =   "<?xml version='1.0' encoding='utf-8'?>" +
        //                                            "<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>" +
        //                                              "<soap:Body>" +
        //                                                    SoapBodyMessageTag +
        //                                              "</soap:Body>" +
        //                                            "</soap:Envelope>";


        //private const string GenericSoapWrapper =   "<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>" +
        //                                              "<soap:Body>" +
        //                                                    SoapBodyMessageTag +
        //                                              "</soap:Body>" +
        //                                            "</soap:Envelope>";



        private const string DiscoveryProbe_Action = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe";
        private const string DiscoveryHello_Action = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Hello";
        private const string DiscoveryBye_Action = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Bye";


        private const string DiscoveryMsg_GenericSoapHeader = "<soap:Header>" +
                                                                    "<wsadis:MessageID>" + SoapMessageIDTag + "</wsadis:MessageID>" +
                                                                    "<wsadis:To>urn:schemas-xmlsoap-org:ws:2005:04:discovery</wsadis:To>" +
                                                                    "<wsadis:Action>" + SoapActionTag + "</wsadis:Action>" +                                                                    
                                                                    SoapMessageAppSeqTag +
                                                              "</soap:Header>";

        private const string DiscoveryMsg_GenericAppSequence = "<d:AppSequence d:MessageNumber=\"" + SoapMessageNumberTag + "\" d:InstanceId=\"" + SoapInstanceIDTag + "\"/>";


        private const string GenericSoapWrapper =   "<?xml version='1.0' encoding='utf-8'?>" +
                                                    "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"" + SoapNameSpaceTag + ">" +
                                                        SoapHeaderTag +
                                                        "<soap:Body>" +
		                                                    SoapBodyMessageTag +
                                                        "</soap:Body>" +
                                                    "</soap:Envelope>";


        private const string DEFAULT_DEVICE_TYPE = "dn:NetworkVideoTransmitter";

        #endregion

        #region Private Variables

        private int MessageCount = 0;

        private XmlReaderSettings RS;
        private XmlSchemaCollection XML_SchemaCollection;
        private bool XmlSchema_ready = false;

        private string UUID_String = "";
        private string MAC_String = "";

        #endregion

        #region Namespaces and Schemas

        public struct Namespace_Type
        {
            private string _Prefix;
            private string _Namespace_URL;
            private string _Namespace_URI;

            public Namespace_Type(string Prefix, string Namespace_URL)
            {
                _Prefix = Prefix;
                _Namespace_URL = Namespace_URL;
                _Namespace_URI = "";
            }

            public Namespace_Type(string Prefix, string Namespace_URL, string Namespace_URI)
            {
                _Prefix = Prefix;
                _Namespace_URL = Namespace_URL;
                _Namespace_URI = Namespace_URI;
            }

            public string Prefix
            {
                get
                {
                    return _Prefix;
                }
                set
                {
                    _Prefix = value;
                }
            }

            public string Namespace
            {
                get
                {
                    return "xmlns:" + _Prefix + "=\"" + _Namespace_URL + "\"";
                }
            }

            public string Namespace_URL
            {
                get
                {
                    return _Namespace_URL;
                }
                set
                {
                    _Namespace_URL = value;
                }
            }

            public string Namespace_URI
            {
                get { return _Namespace_URI; }
                set { _Namespace_URI = value; }
            }
        }
        
#if !DEBUG
        static string runTimeLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
#else
        //static string runTimeLocation = "D:\\Dev\\VS Projects\\ONVIF\\TestApplication\\ONVIF_Tester\\ONVIF_TestCases";
        static string runTimeLocation = ".\\ONVIF_TestCases";
#endif

        /// <summary>
        /// The namespaces below are used to remove inline namespaces in the generated XML strings 
        /// and provide the building blocks for the XmlSchemaCollection
        /// </summary>
        Namespace_Type NS_XML_Media_Types = new Namespace_Type("xmime", "http://www.w3.org/2005/05/xmlmime", runTimeLocation + "\\xsd\\import\\xmlmime.xsd");
        Namespace_Type NS_WS_BaseFault = new Namespace_Type("wsrf-bf", "http://docs.oasis-open.org/wsrf/bf-2", runTimeLocation + "\\xsd\\import\\bf-2.xsd");
        

        Namespace_Type NS_ONVIF_Schema = new Namespace_Type("tt", "http://www.onvif.org/ver10/schema", runTimeLocation + "\\xsd\\onvif.xsd");
        Namespace_Type NS_Device_WSDL = new Namespace_Type("tds", "http://www.onvif.org/ver10/device/wsdl", runTimeLocation + "\\xsd\\devicemgmt.xsd");
        Namespace_Type NS_Media_WSDL = new Namespace_Type("trt", "http://www.onvif.org/ver10/media/wsdl", runTimeLocation + "\\xsd\\media.xsd");
        Namespace_Type NS_Imaging_WSDL = new Namespace_Type("timg", "http://www.onvif.org/ver10/imaging/wsdl");
        Namespace_Type NS_Event_WSDL = new Namespace_Type("tev", "http://www.onvif.org/ver10/event/wsdl");
        Namespace_Type NS_PTZ_WSDL = new Namespace_Type("tptz", "http://www.onvif.org/ver10/ptz/wsdl");
        Namespace_Type NS_Analytics_WSDL = new Namespace_Type("tan", "http://www.onvif.org/ver10/analytics/wsdl");
        Namespace_Type NS_Storage_WSDL = new Namespace_Type("tst", "http://www.onvif.org/ver10/storage/wsdl");
        Namespace_Type NS_Error_WSDL = new Namespace_Type("ter", "http://www.onvif.org/ver10/error");
        Namespace_Type NS_Network_WSDL = new Namespace_Type("dn", "http://www.onvif.org/ver10/network/wsdl", runTimeLocation + "\\xsd\\remotediscovery.xsd");
        Namespace_Type NS_Topics = new Namespace_Type("tns1", "http://www.onvif.org/ver10/topics"); 

        Namespace_Type NS_WSDL = new Namespace_Type("wsdl", "http://schemas.xmlsoap.org/wsdl/");   
        Namespace_Type NS_Soap_WSDL = new Namespace_Type("wsoap12", "http://schemas.xmlsoap.org/wsdl/soap12/");  
        Namespace_Type NS_HTTP_WSDL = new Namespace_Type("http", "http://schemas.xmlsoap.org/wsdl/http/");  
        Namespace_Type NS_Encoding = new Namespace_Type("soapenc", "http://www.w3.org/2003/05/soap-encoding");   
        Namespace_Type NS_Envelope = new Namespace_Type("soapenv", "http://www.w3.org/2003/05/soap-envelope");  
        Namespace_Type NS_Schema = new Namespace_Type("xs", "http://www.w3.org/2001/XMLSchema");  
        Namespace_Type NS_Schema_Instance = new Namespace_Type("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        Namespace_Type NS_Discovery = new Namespace_Type("d", "http://schemas.xmlsoap.org/ws/2005/04/discovery", runTimeLocation + "\\xsd\\import\\ws-discovery.xsd");
        Namespace_Type NS_WS_Discovery_Addressing = new Namespace_Type("wsadis", "http://schemas.xmlsoap.org/ws/2004/08/addressing", runTimeLocation + "\\xsd\\import\\addressing.xsd");
        Namespace_Type NS_WS_Addressing_Addressing = new Namespace_Type("wsa", "http://www.w3.org/2005/08/addressing", runTimeLocation + "\\xsd\\import\\ws-addr.xsd");
        Namespace_Type NS_Topics_Schema = new Namespace_Type("wstop", "http://docs.oasis-open.org/wsn/t-1", runTimeLocation + "\\xsd\\import\\t-1.xsd");
        Namespace_Type NS_WS_BaseNotification_Schema = new Namespace_Type("wsnt", "http://docs.oasis-open.org/wsn/b-2", runTimeLocation + "\\xsd\\import\\b-2.xsd");
        Namespace_Type NS_Optimized_Packaging = new Namespace_Type("xop", "http://www.w3.org/2004/08/xop/include");

        Namespace_Type NS_SOAP_Envelope = new Namespace_Type("SOAP-ENV", "http://www.w3.org/2003/05/soap-envelope");

        private Stack Namespace_Stack;

        #endregion

        public TestMessages()
        {
            this.InitSchemaCollection();

            // get the mac address now to speed things up
            if (MAC_String == "")
                MAC_String = GetMac();
        }

        #region Serializer and Deserializer code

        /// <summary>
        /// Soap deserializer, take a soap message and return the .Net object
        /// </summary>
        /// <param name="soapString">SOAP Message</param>
        /// <returns>.Net object</returns>
        private object FromSoap(string soapString)
        {
            IFormatter formatter;
            MemoryStream memStream = null;
            Object objectFromSoap = null;
            try
            {
                byte[] bytes = new byte[soapString.Length];

                Encoding.ASCII.GetBytes(soapString, 0,
                             soapString.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
                formatter = new SoapFormatter();
                objectFromSoap =
                     formatter.Deserialize(memStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
            return objectFromSoap;
        }

        /// <summary>
        /// Soap deserializer, uses Soap Formatter for deserialization, Still requires serializeable objects
        /// </summary>
        /// <param name="soapMessage">SOAP message</param>
        /// <returns>.Net object</returns>
        private object ParseSoap(string soapMessage)
        {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter formatter;
            MemoryStream memStream = null;
            Object objectFromSoap = null;
            try
            {
                byte[] bytes = new byte[soapMessage.Length];

                Encoding.ASCII.GetBytes(soapMessage, 0,
                             soapMessage.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
                formatter = new SoapFormatter();
                objectFromSoap =
                     formatter.Deserialize(memStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
            return objectFromSoap;

        }

        /// <summary>
        /// Take serializeable .Net object and create SOAP message
        /// </summary>
        /// <param name="objToSoap">Object to serialize</param>
        /// <returns>SOAP Message</returns>
        private string ToSoap(Object objToSoap)
        {
            IFormatter formatter;
            MemoryStream memStream = null;
            string strObject = "";
            try
            {
                memStream = new MemoryStream();
                formatter = new SoapFormatter();                
                formatter.Serialize(memStream, objToSoap);
                strObject =
                   Encoding.ASCII.GetString(memStream.GetBuffer());

                //Check for the null terminator character

                int index = strObject.IndexOf("\0");
                if (index > 0)
                {
                    strObject = strObject.Substring(0, index);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
            return strObject;
        }

        /// <summary>
        /// Serialize object into XML string
        /// </summary>
        /// <param name="objToSerialize">Serializealbe oject</param>
        /// <returns>XML string</returns>
        private string ToXML(object objToSerialize)
        {
            XmlSerializer serializer = null;

            FileStream stream = null;

            try
            {
                StringBuilder sb = new StringBuilder();

                StringWriter output = new StringWriter(sb);

                output.NewLine = String.Empty;
                serializer = new XmlSerializer(objToSerialize.GetType());
                     

                serializer.Serialize(output, objToSerialize);

                return output.ToString();
            }

            catch(Exception e)
            {                
                throw new XML_SerializerException("ToXML serializer failed - " + e.Message);
            }

            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
        
        /// <summary>
        /// Deserialize XML string into specified type object
        /// </summary>
        /// <param name="xmlString">XML string to deserialize</param>
        /// <param name="type">Oject type</param>
        /// <returns>Deserialized object</returns>
        public object FromXml(string xmlString, Type type)
        {
            XmlSerializer xmlSerializer;
            MemoryStream memStream = null;
            try
            {             
                xmlSerializer = new XmlSerializer(type);
                byte[] bytes = new byte[xmlString.Length];

                Encoding.ASCII.GetBytes(xmlString, 0, xmlString.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
                object objectFromXml = xmlSerializer.Deserialize(memStream);
                

                return objectFromXml;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
        }

        /// <summary>
        /// Lookup the namespace for a requested URI
        /// </summary>
        /// <param name="URI">URI of namespace</param>
        /// <returns>Namespace type</returns>
        private Namespace_Type? GetNamespace_FromURI(string URI)
        {
            if((URI == null) || (URI.Equals("")))
                return null;

            if (URI.Equals(NS_ONVIF_Schema.Namespace_URL))
                return NS_ONVIF_Schema;

            if (URI.Equals(NS_Device_WSDL.Namespace_URL))
                return NS_Device_WSDL;

            if (URI.Equals(NS_Media_WSDL.Namespace_URL))
                return NS_Media_WSDL;

            if (URI.Equals(NS_Imaging_WSDL.Namespace_URL))
                return NS_Imaging_WSDL;

            if (URI.Equals(NS_Event_WSDL.Namespace_URL))
                return NS_Event_WSDL;

            if (URI.Equals(NS_PTZ_WSDL.Namespace_URL))
                return NS_PTZ_WSDL;

            if (URI.Equals(NS_Analytics_WSDL.Namespace_URL))
                return NS_Analytics_WSDL;

            if (URI.Equals(NS_Storage_WSDL.Namespace_URL))
                return NS_Storage_WSDL;

            if (URI.Equals(NS_Error_WSDL.Namespace_URL))
                return NS_Error_WSDL;

            if (URI.Equals(NS_Network_WSDL.Namespace_URL))
                return NS_Network_WSDL;

            if (URI.Equals(NS_Topics.Namespace_URL))
                return NS_Topics;

            if (URI.Equals(NS_WSDL.Namespace_URL))
                return NS_WSDL;

            if (URI.Equals(NS_Soap_WSDL.Namespace_URL))
                return NS_Soap_WSDL;

            if (URI.Equals(NS_HTTP_WSDL.Namespace_URL))
                return NS_HTTP_WSDL;

            if (URI.Equals(NS_Encoding.Namespace_URL))
                return NS_Encoding;

            if (URI.Equals(NS_Envelope.Namespace_URL))
                return NS_Envelope;

            if (URI.Equals(NS_Schema.Namespace_URL))
                return NS_Schema;

            if (URI.Equals(NS_Schema_Instance.Namespace_URL))
                return NS_Schema_Instance;

            if (URI.Equals(NS_Discovery.Namespace_URL))
                return NS_Discovery;

            if (URI.Equals(NS_WS_Discovery_Addressing.Namespace_URL))
                return NS_WS_Discovery_Addressing;

            if (URI.Equals(NS_WS_Addressing_Addressing.Namespace_URL))
                return NS_WS_Addressing_Addressing;

            if (URI.Equals(NS_Topics_Schema.Namespace_URL))
                return NS_Topics_Schema;

            if (URI.Equals(NS_WS_BaseNotification_Schema.Namespace_URL))
                return NS_WS_BaseNotification_Schema;

            if (URI.Equals(NS_Optimized_Packaging.Namespace_URL))
                return NS_Optimized_Packaging;
            

            throw new XML_SerializerException("Namespace not found - " + URI);
        }

        /// <summary>
        /// Helper function for removing inline namespaces.  Replaces the namespace with an empty string
        /// </summary>
        /// <param name="XML_string"></param>
        /// <param name="aNamespace"></param>
        /// <returns></returns>
        private string ReplaceInlineNameSpaces(string XML_string, Namespace_Type aNamespace)
        {
            string newXMLstring = XML_string;

            // first replace the namespace with spaces before and after
            newXMLstring = newXMLstring.Replace(" xmlns:" + aNamespace.Prefix + "=\"" + aNamespace.Namespace_URL + "\" ", "");

            // now try it with just a space before
            newXMLstring = newXMLstring.Replace(" xmlns:" + aNamespace.Prefix + "=\"" + aNamespace.Namespace_URL + "\"", "");
            
            // just a space after
            newXMLstring = newXMLstring.Replace("xmlns:" + aNamespace.Prefix + "=\"" + aNamespace.Namespace_URL + "\" ", "");

            // no spaces
            newXMLstring = newXMLstring.Replace("xmlns:" + aNamespace.Prefix + "=\"" + aNamespace.Namespace_URL + "\"", "");

            return newXMLstring;

        }

        /// <summary>
        /// Helper function for removing inline namespaces
        /// </summary>
        /// <param name="XML_string"></param>
        /// <returns></returns>
        private string RemoveInlineNameSpaces(string XML_string)
        {
            string newXMLstring = XML_string;

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_ONVIF_Schema);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Device_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Media_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Imaging_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Event_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_PTZ_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Analytics_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Storage_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Error_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Network_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Topics);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Soap_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_HTTP_WSDL);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Encoding);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Envelope);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Schema);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Schema_Instance);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Discovery);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_WS_Discovery_Addressing);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_WS_Addressing_Addressing);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Topics_Schema);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_WS_BaseNotification_Schema);

            newXMLstring = ReplaceInlineNameSpaces(newXMLstring, NS_Optimized_Packaging);

            return newXMLstring;
        }

        /// <summary>
        /// Recursively go through the XML string and remove inline name spaces
        /// </summary>
        /// <param name="node">XML Node</param>
        /// <param name="NameSpace">Current namespace</param>
        /// <param name="included_Namespaces">Namespaces found</param>
        /// <returns>XML node</returns>
        private XmlNode RemoveInlineNameSpaces(XmlNode node, Namespace_Type? NameSpace, ref string included_Namespaces)
        {
            int x;
            Namespace_Type? tmpNamespace;
            Namespace_Type nonNull_Namespace;

            if (node.NodeType == XmlNodeType.Element)
            {
                // if this element has an inline name space remove it and start adding the namespace prefix
                tmpNamespace = GetNamespace_FromURI(node.NamespaceURI);
                if (tmpNamespace != null)
                {
                    NameSpace = tmpNamespace;
                    // a new namespace was found so add it to the stack
                    nonNull_Namespace = (Namespace_Type)NameSpace;
                    
                    // add the prefix
                    node.Prefix = nonNull_Namespace.Prefix;

                    // remember the namespace
                    included_Namespaces = SoapMessage_AddNamespace(nonNull_Namespace, included_Namespaces);
                 
                    // remove any namespace attributes
                    XmlAttributeCollection attributes = node.Attributes;
                    if (attributes != null)
                    {
                        for (x = 0; x < attributes.Count; x++)
                        {
                            if ((attributes[x].Prefix.Equals("xmlns", StringComparison.OrdinalIgnoreCase)) ||
                                (attributes[x].Name.Equals("xmlns", StringComparison.OrdinalIgnoreCase)))
                            {
                                // add the namespace if there were mulitple "types" in a element
                                tmpNamespace = GetNamespace_FromURI(attributes[x].Value);
                                if (tmpNamespace != null)
                                {
                                    nonNull_Namespace = (Namespace_Type)tmpNamespace;                                    
                                    included_Namespaces = SoapMessage_AddNamespace(nonNull_Namespace, included_Namespaces);
                                }

                                node.Attributes.Remove(attributes[x]);
                                x--;
                            }
                        }

                    }

                }
                else
                {
                    // if there has been a namespace already found apply its prefix                    
                    if (NameSpace != null)
                    {
                        nonNull_Namespace = (Namespace_Type)NameSpace;
                        // add the prefix
                        node.Prefix = nonNull_Namespace.Prefix;
                    }
                }

                // do this for all the children as well
                for (x = 0; x < node.ChildNodes.Count; x++)
                {
                    node.ReplaceChild(RemoveInlineNameSpaces(node.ChildNodes[x], NameSpace, ref included_Namespaces), node.ChildNodes[x]);
                }
            }

            return node;
        }
        
        /// <summary>
        /// The NVT's tested do not like the XMLDocument creators use of inline namespacing.  This 
        /// function and its helper functions remove the inline namespaces and create an import list
        /// </summary>
        /// <param name="XML_String">String to parse</param>
        /// <param name="included_Namespaces">Namespaces found</param>
        /// <returns>Updated XML string</returns>
        private string RemoveInlineNameSpaces(string XML_String, out string included_Namespaces)
        {
            string newXmlString = "";
            XmlTextReader reader;
            XmlTextWriter writer;
            XmlNode aNode1;
            byte[] byteBuffer;
            // reinilize the stack
            Namespace_Stack = new Stack(10);

            // default the included namespaces to nothing
            included_Namespaces = "";

            // now go through the XML string and remove any inline namespacing
            XmlDocument x_doc = new XmlDocument();
            x_doc.LoadXml(XML_String);

            MemoryStream memStream = null;
            try
            {
                byte[] bytes = new byte[XML_String.Length];

                Encoding.ASCII.GetBytes(XML_String, 0, XML_String.Length, bytes, 0);
                memStream = new MemoryStream(bytes);                
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            reader = new XmlTextReader(memStream);

            aNode1 = x_doc.ReadNode(reader);

            while (aNode1 != null)
            {
                aNode1 = RemoveInlineNameSpaces(aNode1, null, ref included_Namespaces);

                memStream = new MemoryStream();
                writer = new XmlTextWriter(memStream, Encoding.ASCII);

                aNode1.WriteTo(writer);

                writer.Flush();

                byteBuffer = memStream.GetBuffer();

                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();                
                newXmlString += enc.GetString(byteBuffer,0, (int)writer.BaseStream.Length);

                reader.Read();
                aNode1 = x_doc.ReadNode(reader);

            }
               
            // now manually go through the string and remove any namespace URI's listed
            newXmlString = RemoveInlineNameSpaces(newXmlString);


            return newXmlString;
        }

        private void InsertInlineNameSpace(XmlNode node, Namespace_Type? aNamespace)
        {
            int x;
            bool found = false;

            if(aNamespace == null)
                return;

            string namespaceString = "xmlns:" + ((Namespace_Type)aNamespace).Prefix;

            // make sure the namespace isn't already there
            XmlAttributeCollection attributes = node.Attributes;
            if (attributes != null)
            {
                for (x = 0; x < attributes.Count; x++)
                {
                    if (attributes[x].Name.Equals(namespaceString, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                    }
                }
            }

            if (!found)
            {
                // add the namespace to this node
                XmlElement aElment = (XmlElement)node;
                aElment.SetAttribute(namespaceString, ((Namespace_Type)aNamespace).Namespace_URL);
            }

        }

        private XmlNode AddInlineNameSpaces(XmlNode node, string searchString)
        {
            int x;
            string[] type_NS;

            XmlAttributeCollection child_attributes = node.Attributes;

            if (child_attributes != null)
            {
                for (x = 0; x < child_attributes.Count; x++)
                {
                    Console.WriteLine(child_attributes[x].Value);
                    if (child_attributes[x].Name.Equals(searchString, StringComparison.OrdinalIgnoreCase))
                    {
                        type_NS = child_attributes[x].Value.Split(new char[] { ':' });

                        if (type_NS.Length > 1)
                            InsertInlineNameSpace(node, GetNamespace_ByPrefix(type_NS[0]));
                        break;
                    }
                }
            }

            if (node.HasChildNodes)
            {
                for (x = 0; x < node.ChildNodes.Count; x++)
                {
                    node.ReplaceChild(AddInlineNameSpaces(node.ChildNodes[x], searchString), node.ChildNodes[x]);
                }
            }

            return node;
        }

        
        private string AddInlineNameSpaces(string XML_String, string searchString)
        {


            int x;            
            string newXmlString = "";
            XmlTextReader reader;
            XmlTextWriter writer;
            XmlNode aNode1;
            byte[] byteBuffer;
            // reinilize the stack
            Namespace_Stack = new Stack(10);

            // now go through the XML string and remove any inline namespacing
            XmlDocument x_doc = new XmlDocument();
            x_doc.LoadXml(XML_String);

            MemoryStream memStream = null;
            try
            {
                byte[] bytes = new byte[XML_String.Length];

                Encoding.ASCII.GetBytes(XML_String, 0, XML_String.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
                       
            reader = new XmlTextReader(memStream);

            aNode1 = x_doc.ReadNode(reader);

            while (aNode1 != null)
            {                
                aNode1 = AddInlineNameSpaces(aNode1, searchString);
              
                memStream = new MemoryStream();
                writer = new XmlTextWriter(memStream, Encoding.ASCII);

                aNode1.WriteTo(writer);

                writer.Flush();

                byteBuffer = memStream.GetBuffer();

                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                newXmlString = enc.GetString(byteBuffer, 0, (int)writer.BaseStream.Length);

                reader.Read();
                aNode1 = x_doc.ReadNode(reader);

            }

  
            return newXmlString;
        }

       


        /// <summary>
        /// Build Universal Unique Identifier
        /// </summary>
        /// <returns>UUID string</returns>
        private string Build_Message_UUID()
        {
            // uuid:aaaabbbb-cccc-dddd-eeee-ff ff ff ff ff ff
            string uuid = "uuid:";

            Guid aGuid = System.Guid.NewGuid();
            uuid += aGuid.ToString();


            return uuid;

        }

        /// <summary>
        /// Build the UUID string for used in the Address section of the Endpoint Reference
        /// </summary>
        /// <returns>UUID string</returns>
        public string Build_EndPointReference_UUID()
        {
            // uuid:aaaabbbb-cccc-dddd-eeee-ff ff ff ff ff ff
            string uuid = "uuid:";

            if (UUID_String == "")
            {
                // if the MAC address string is null try to fill it
                if (MAC_String == "")
                    MAC_String = GetMac();

                Guid aGuid = System.Guid.NewGuid();
                UUID_String = aGuid.ToString();

                string[] pieces = UUID_String.Split(new char[] { '-' });

                if (MAC_String != "")
                {
                    pieces[4] = MAC_String;
                    UUID_String = pieces[0] + "-" + pieces[1] + "-" + pieces[2] + "-" + pieces[3] + "-" + pieces[4];
                }
                else // if we didn't find a MAC address keep the last 6 bytes so the packet source can be identified
                    MAC_String = pieces[4];
            }

            uuid += UUID_String;

            return uuid;


        }
        

        /// <summary>
        /// Get the MAC address of a phisical adapter on this machine
        /// </summary>
        /// <returns>MAC address or empty string</returns>
        public string GetMac()
        {
            try
            {
                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();

                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                if (nics == null || nics.Length < 1)
                    return "";

                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties(); // .GetIPInterfaceProperties();
                    PhysicalAddress address = adapter.GetPhysicalAddress();

                    if (address.ToString() != null)
                    {
                        if (address.ToString().Length > 12)
                            return address.ToString().Substring(0, 12);
                        else
                            return address.ToString();
                    }
                }
            }
            catch { }
            return "";
        }


        /// <summary>
        /// Build Discovery message SOAP meassage header.  These are slightly unique compared to the others
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message_Number"></param>
        /// <param name="instance_ID"></param>
        /// <param name="probe"></param>
        /// <returns></returns>
        private string BuildDiscoveryMessage_SoapHeader(string action, int message_Number, int instance_ID, bool probe)
        {
            string soapHeader = DiscoveryMsg_GenericSoapHeader;

            soapHeader = soapHeader.Replace(SoapActionTag, action);

            soapHeader = soapHeader.Replace(SoapMessageIDTag, Build_Message_UUID());

            
            // special case for probe messages
            if (probe)
                soapHeader = soapHeader.Replace(SoapMessageAppSeqTag, "");
            else
            {
                soapHeader = soapHeader.Replace(SoapMessageAppSeqTag, DiscoveryMsg_GenericAppSequence);

                soapHeader = soapHeader.Replace(SoapMessageNumberTag, message_Number.ToString());
                soapHeader = soapHeader.Replace(SoapInstanceIDTag, instance_ID.ToString());
            }

            return soapHeader;
        }

        /// <summary>
        /// Add namespace to SOAP header
        /// </summary>
        /// <param name="aNamespace">namespace to add</param>
        /// <param name="currentNamesapceString">Current namespace string</param>
        /// <returns>Updated Namespace string</returns>
        private string SoapMessage_AddNamespace(Namespace_Type aNamespace, string currentNamesapceString)
        {
            string headerNamespace;
            // remember the namespace
            headerNamespace = " xmlns:" + aNamespace.Prefix + "=\"" + aNamespace.Namespace_URL + "\"";
            if (!currentNamesapceString.Contains(headerNamespace))
                currentNamesapceString += headerNamespace;

            return currentNamesapceString;
        }

        /// <summary>
        /// Get the correct action string for the Discovery message type being built 
        /// </summary>
        /// <param name="discoveryType"></param>
        /// <returns></returns>
        private string GetDiscoveryMesgAction(Type discoveryType)
        {
            if (discoveryType == typeof(RemoteDiscovery.ProbeType))
                return DiscoveryProbe_Action;

            if (discoveryType == typeof(RemoteDiscovery.HelloType))
                return DiscoveryHello_Action;

            if (discoveryType == typeof(RemoteDiscovery.ByeType))
                return DiscoveryBye_Action;

            throw new XML_SerializerException("Discovery type not found - " + discoveryType.ToString());

        }

        /// <summary>
        /// Build SOAP Message
        /// </summary>
        /// <param name="messageObject">Serializeable object for SOAP body</param>
        /// <returns>SOAP message string</returns>
        private string BuildSoap(object messageObject)
        {
            string SoapMessage = GenericSoapWrapper;
            string XmlMessage;
            string NameSpaces;
            string SoapHeader;

            XmlMessage = ToXML(messageObject);

            // remove the encoding line, it will be in the start of the SOAP message
            XmlMessage = XmlMessage.Replace(XML_EncodingString, "");

            // remoce inline namespaces in the XML string
            XmlMessage = RemoveInlineNameSpaces(XmlMessage, out NameSpaces);

            // replace the soap body tag with the new soap body message
            SoapMessage = SoapMessage.Replace(SoapBodyMessageTag, XmlMessage);
            
            // if this is a discovery command add the discovery header, otherwise blank the header out
            if ((messageObject.GetType() == typeof(RemoteDiscovery.ProbeType)) ||
                (messageObject.GetType() == typeof(RemoteDiscovery.HelloType)) ||
                (messageObject.GetType() == typeof(RemoteDiscovery.ByeType)))
            {
                SoapHeader = BuildDiscoveryMessage_SoapHeader(GetDiscoveryMesgAction(messageObject.GetType()), MessageCount++, messageObject.GetHashCode(), (messageObject.GetType() == typeof(RemoteDiscovery.ProbeType)));
                //SoapHeader = BuildDiscoveryMessage_SoapHeader(GetDiscoveryMesgAction(messageObject.GetType()), (System.DateTime.Now.ToUniversalTime().Ticks % 10000).ToString(), MessageCount++, messageObject.GetHashCode());
                SoapMessage = SoapMessage.Replace(SoapHeaderTag, SoapHeader);

                // add the neccissary namespace information due to the header
                NameSpaces = SoapMessage_AddNamespace(NS_WS_Discovery_Addressing, NameSpaces);
                NameSpaces = SoapMessage_AddNamespace(NS_Discovery, NameSpaces);
                NameSpaces = SoapMessage_AddNamespace(NS_Network_WSDL, NameSpaces);
                
            }
            else
            {
                // no header needed so just get rid of the tag
                SoapMessage = SoapMessage.Replace(SoapHeaderTag, "");
            }

            // now add the new namespaces to the soap message envelope
            SoapMessage = SoapMessage.Replace(SoapNameSpaceTag, NameSpaces);

            return SoapMessage;
        }

        
        /// <summary>
        /// Uses the XML Deserializer to deserialize SOAP message
        /// </summary>
        /// <param name="soapMessage">SOAP message to deserialize</param>
        /// <param name="type">Object type expected in SOAP body</param>
        /// <returns>Deserialized .Net object</returns>
        private object GetObjectFromSoap(string soapMessage, Type type)
        {          
            // because of problems with the SOAP deserializer it is neccissary to find a workaround for
            // some of non serializeable objects.  Messages that contain the XS:Any Element tag or
            // XS:AnyAttribute attribute are not deserialzieable using the SOAP deserializer.
            string nodeText = GetXMLFromSoap(soapMessage, type);

            return FromXml(nodeText, type);

            // if the body wasn't found there is a problem so throw an exception
            //throw new XML_SerializerException("SOAP Body not found");
        }

        /// <summary>
        /// Convert from SOAP message to XML string
        /// </summary>
        /// <param name="soapMessage"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetXMLFromSoap(string soapMessage, Type type)
        {
            // because of problems with the SOAP deserializer it is neccissary to find a workaround for
            // some of non serializeable objects.  Messages that contain the XS:Any Element tag or
            // XS:AnyAttribute attribute are not deserialzieable using the SOAP deserializer.
            string nodeText;

            byte[] bytes = new byte[soapMessage.Length];

            Encoding.ASCII.GetBytes(soapMessage, 0,
                         soapMessage.Length, bytes, 0);

            System.IO.MemoryStream memStream = new MemoryStream(bytes);
            XmlTextReader reader = new XmlTextReader(memStream);

            while (reader.Read())
            {
                // locate the soap body and deserialize the XML in there
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name.EndsWith(":body", StringComparison.OrdinalIgnoreCase)))
                {
                    nodeText = reader.ReadInnerXml();

                    // Add inline namespaces in the XML string
                    nodeText = AddInlineNameSpaces(nodeText, "xsi:type");

                    return nodeText;
                }

            }

            // if the body wasn't found there is a problem so throw an exception
            throw new XML_SerializerException("SOAP Body not found");
        }

        #endregion

       
        #region Build Messages

        /***************************************************************************
         * 
         *                          Build Message Functions
         *                          
         * These functions were designed return SOAP message containing the requested 
         * object types
         * 
         * They all work on the same basic design where the funciton has any parameters 
         * neccssary to build the object, the object is built, values added and the
         * SOAP wrapper is added.  This string is then returned.
         * 
         * Some functions have been overloaded to ease in building the messages with
         * simpler types instead of complex objects
         * 
         * 
         * *************************************************************************/


        #region Remote Discovery Requests

        /// <summary>
        /// Build a Probe Request
        /// </summary>
        /// <param name="Types">Types</param>
        /// <param name="Scope">Scope</param>
        /// <returns>SOAP Message</returns>
        public string Build_ProbeRequest(string Types, RemoteDiscovery.ScopesType Scope)
        {
            string soapMessage = "";

            // ProbeRequest is a dn:Probe is a d:ProbeType
            RemoteDiscovery.ProbeType ProbeRequest = new RemoteDiscovery.ProbeType();
            // add any perameters
            ProbeRequest.Types = Types;
            ProbeRequest.Scopes = Scope;
            // build the soap message
            soapMessage = BuildSoap(ProbeRequest);

            return soapMessage;
        }
 


        /// <summary>
        /// Build a Hello Request
        /// </summary>
        /// <param name="EndpointReference">Endpoint Reference</param>
        /// <param name="MetadataVersion">Metadata Version</param>
        /// <param name="Scope">Scope</param>
        /// <param name="Types">Types</param>
        /// <param name="XAddrs">XAddrs</param>
        /// <returns>SOAP Message</returns>
        public string Build_HelloRequest(RemoteDiscovery.EndpointReferenceType EndpointReference,
                                         uint MetadataVersion,
                                         RemoteDiscovery.ScopesType Scope, 
                                         string Types,
                                         string XAddrs)
        {
            string soapMessage = "";
                        
            // HelloRequest is a dn:Hello is a d:HelloType
            RemoteDiscovery.HelloType HelloRequest = new RemoteDiscovery.HelloType();
            // add any perameters
            HelloRequest.EndpointReference = EndpointReference;
            HelloRequest.MetadataVersion = MetadataVersion;
            HelloRequest.Scopes = Scope;
            HelloRequest.Types = Types;
            HelloRequest.XAddrs = XAddrs;
            // build the soap message
            soapMessage = BuildSoap(HelloRequest);

            return soapMessage;
        }

        /// <summary>
        /// Build a Bye Request
        /// </summary>
        /// <param name="EndpointReference">Endpoint Reference</param>
        /// <param name="MetadataVersion">Metadata Version</param>
        /// <param name="MetadataVersionSpecified">Metadata Version Specified</param>
        /// <param name="Scope">Scope</param>
        /// <param name="Types">Types</param>
        /// <param name="XAddrs">XAddrs</param>
        /// <returns>SOAP Message</returns>
        public string Build_ByeRequest(RemoteDiscovery.EndpointReferenceType EndpointReference,
                                       uint MetadataVersion,
                                       bool MetadataVersionSpecified,
                                       RemoteDiscovery.ScopesType Scope,
                                       string Types,
                                       string XAddrs)
        {
            string soapMessage = "";

            // ByeRequest is a dn:Bye is a d:ByeType
            RemoteDiscovery.ByeType ByeRequest = new RemoteDiscovery.ByeType();
            // add any perameters
            ByeRequest.EndpointReference = EndpointReference;
            ByeRequest.MetadataVersion = MetadataVersion;
            ByeRequest.MetadataVersionSpecified = MetadataVersionSpecified;
            ByeRequest.Scopes = Scope;
            ByeRequest.Types = Types;
            ByeRequest.XAddrs = XAddrs;
            // build the soap message
            soapMessage = BuildSoap(ByeRequest);

            return soapMessage;
        }

        #endregion

        #endregion

        /// <summary>
        /// Initlize the schema collection object.
        /// </summary>
        public void InitSchemaCollection()
        {
            string errorMsg = "";

            if (!XmlSchema_ready)
            {
                try
                {
                    Build_XmlSchemaCollection();
                }
                catch (Exception e)
                {
                    errorMsg = e.Message;
                }
            }
        }
        
        /// <summary>
        /// Build the XML Scheama Collection object, and/or
        /// the XmlReaderSettings if it is used.
        /// </summary>
        private void Build_XmlSchemaCollection()
        {
            

            this.RS = new XmlReaderSettings();
            this.XML_SchemaCollection = new XmlSchemaCollection();
#if true            
            
            
            this.XML_SchemaCollection.Add(NS_XML_Media_Types.Namespace_URL, NS_XML_Media_Types.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_WS_Addressing_Addressing.Namespace_URL, NS_WS_Addressing_Addressing.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_WS_BaseFault.Namespace_URL, NS_WS_BaseFault.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_Topics_Schema.Namespace_URL, NS_Topics_Schema.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_WS_BaseNotification_Schema.Namespace_URL, NS_WS_BaseNotification_Schema.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_WS_Discovery_Addressing.Namespace_URL, NS_WS_Discovery_Addressing.Namespace_URI);
            

            this.XML_SchemaCollection.Add(NS_ONVIF_Schema.Namespace_URL, NS_ONVIF_Schema.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_Device_WSDL.Namespace_URL, NS_Device_WSDL.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_Media_WSDL.Namespace_URL, NS_Media_WSDL.Namespace_URI);
            this.XML_SchemaCollection.Add(NS_Discovery.Namespace_URL, NS_Discovery.Namespace_URI);
            
#else
            this.RS.Schemas.Add(NS_XML_Media_Types.Namespace_URL, NS_XML_Media_Types.Namespace_URI);
            this.RS.Schemas.Add(NS_WS_Addressing_Addressing.Namespace_URL, NS_WS_Addressing_Addressing.Namespace_URI);
            this.RS.Schemas.Add(NS_WS_BaseFault.Namespace_URL, NS_WS_BaseFault.Namespace_URI);
            this.RS.Schemas.Add(NS_Topics_Schema.Namespace_URL, NS_Topics_Schema.Namespace_URI);
            this.RS.Schemas.Add(NS_WS_BaseNotification_Schema.Namespace_URL, NS_WS_BaseNotification_Schema.Namespace_URI);
            this.RS.Schemas.Add(NS_WS_Discovery_Addressing.Namespace_URL, NS_WS_Discovery_Addressing.Namespace_URI);
            

            this.RS.Schemas.Add(NS_ONVIF_Schema.Namespace_URL, NS_ONVIF_Schema.Namespace_URI);
            this.RS.Schemas.Add(NS_Device_WSDL.Namespace_URL, NS_Device_WSDL.Namespace_URI);
            this.RS.Schemas.Add(NS_Media_WSDL.Namespace_URL, NS_Media_WSDL.Namespace_URI);
            this.RS.Schemas.Add(NS_Discovery.Namespace_URL, NS_Discovery.Namespace_URI);
            
            
            //// add the various schemas used 
            //this.RS.Schemas.Add(null, NS_Discovery.Namespace_URI);            
            ////this.RS.Schemas.Add(null, NS_ONVIF_Schema.Namespace_URI);
            //this.RS.Schemas.Add(null, NS_WS_Addressing_Addressing.Namespace_URI);
            ////this.RS.Schemas.Add(null, NS_Discovery_Addressing.Namespace_URI); 

            //this.RS.Schemas.Add(null, NS_Device_WSDL.Namespace_URI);
            ////this.RS.Schemas.Add(null, NS_Media_WSDL.Namespace_URI);
            ////this.RS.Schemas.Add(null, NS_Mine.Namespace_URI);

            //this.RS.Schemas.Add(NS_ONVIF_Schema.Namespace_URL, NS_ONVIF_Schema.Namespace_URI);
            //this.RS.Schemas.Add(NS_Device_WSDL.Namespace_URL, NS_Device_WSDL.Namespace_URI);
            //this.RS.Schemas.Add(NS_WS_Addressing_Addressing.Namespace_URL, NS_WS_Addressing_Addressing.Namespace_URI);
            //this.RS.Schemas.Add(NS_WS_BaseNotification_Schema.Namespace_URL, NS_WS_BaseNotification_Schema.Namespace_URI);
            //this.RS.Schemas.Add(NS_Media_WSDL.Namespace_URL, NS_Media_WSDL.Namespace_URI);
            //this.RS.Schemas.Add(NS_Discovery.Namespace_URL, NS_Discovery.Namespace_URI);
            //this.RS.Schemas.Add(NS_Discovery_Addressing.Namespace_URL, NS_Discovery_Addressing.Namespace_URI);

#endif
            XmlSchema_ready = true;
        }
        
        /// <summary>
        /// Verify the XML message sent is correct.
        /// </summary>
        /// <param name="XML_String">String containing the XML data</param>
        /// <param name="response">Reference string for errros</param>
        /// <returns>Pass/Fail</returns>
        public bool Verify_XML(string XML_String, ref string response)
        {
            XmlParserContext context;
            bool validated_old, validated_new;
            string errorMsg = "";

            // build the XML schema collection which will be used to validate the XML
            if (!XmlSchema_ready)
            {
                try
                {
                    Build_XmlSchemaCollection();
                }
                catch (Exception e)
                {
                    errorMsg = e.Message;
                }
                if (!errorMsg.Equals(""))
                {
                    System.Windows.Forms.MessageBox.Show("Unexpected error - " + errorMsg + Environment.NewLine + "Please validated network connections", "Error", MessageBoxButtons.OK);
                    return false;
                }
            }


            // fix some bugs
            // replace dn:NetworkVideoTransmitter with NetworkVideoTransmitter, NetworkVideoTransmitter
            // is not defined in the dn namespace and this will cause an error.
            if (XML_String.Contains("dn:NetworkVideoTransmitter"))
                XML_String = XML_String.Replace("dn:NetworkVideoTransmitter", "NetworkVideoTransmitter");

            

            // old way first

            validated_old = true;
            validated_new = true;

            XmlValidatingReader reader = null;
#if true
            // the XmlValidatingReader is obsolete so it is recomended to use the XmlParserContext, but this
            // object does not like the elements defined with same name in the schemas used by ONVIF.  So
            // the XmlValidatingReader is still being used.  I've left the code in so I can test and see if
            // the problem can be worked around, but at this point I've not had any luck.
            

            try
            {
                //Create the XmlParserContext.
                context = new XmlParserContext(null, null, "", XmlSpace.None);

                //Implement the reader. 
                reader = new XmlValidatingReader(XML_String, XmlNodeType.Element, context);

                //Set the schema type and add the schema to the reader.
                reader.ValidationType = ValidationType.Schema;
                reader.Schemas.Add(this.XML_SchemaCollection);

                while (reader.Read())
                {
                }

            }
            catch (XmlException XmlExp)
            {
                response += "ERROR XmlException = " + XmlExp.Message + Environment.NewLine;
                validated_old = false;
            }
            catch (XmlSchemaException XmlSchExp)
            {
                response += "ERROR XmlSchemaException = " + XmlSchExp.Message + Environment.NewLine;
                validated_old = false;
            }
            catch (Exception GenExp)
            {
                response += "ERROR " + GenExp.GetType().ToString() + " = " + GenExp.Message + Environment.NewLine;
                validated_old = false;
            }
            

#else

            // New way

            XmlReader RD = null;
            
            try
            {
                //Create the XmlParserContext.
                context = new XmlParserContext(null, null, "", XmlSpace.None);

                byte[] bytes = new byte[XML_String.Length];

                Encoding.ASCII.GetBytes(XML_String, 0,
                             XML_String.Length, bytes, 0);

                System.IO.MemoryStream memStream = new MemoryStream(bytes);

                RS.ValidationType = ValidationType.Schema;
                RD = XmlReader.Create(memStream, RS, context);

                while (RD.Read())
                {
                }
            }
            catch (XmlException XmlExp)
            {
                response += "ERROR XmlException = " + XmlExp.Message + Environment.NewLine;
                validated_new = false;
            }
            catch (XmlSchemaException XmlSchExp)
            {
                response += "ERROR XmlSchemaException = " + XmlSchExp.Message + Environment.NewLine;
                validated_new = false;
            }
            catch (Exception GenExp)
            {
                response += "ERROR " + GenExp.GetType().ToString() + " = " + GenExp.Message + Environment.NewLine;
                validated_new = false;
            }
            
#endif
            return (validated_old & validated_new);

        }
  
        /// <summary>
        /// Convert Soap Message into deserilized .NET object
        /// </summary>
        /// <param name="message">SOAP message</param>
        /// <param name="TheObjectType">Type of object expected</param>
        /// <returns>New Object</returns>
        public object Parse_SoapMessage(string message, Type TheObjectType)
        {
            // deserialize the message
            // NewObject is a TheObjectType
            object NewObject = GetObjectFromSoap(message, TheObjectType);

            // return the object, cast to the correct type
            return NewObject;
        }

        /// <summary>
        /// Verify the XML in the message received is valid and is of the type specified.
        /// </summary>
        /// <param name="msg">Message string</param>
        /// <param name="type">Expected XML object type</param>
        /// <param name="response">Reference string for errors</param>
        /// <returns>Pass/Failed status of verfication</returns>
        private bool Verify_MessageResponse(string msg, Type type, ref string response)
        {
            bool verified = true;
            object response_Received;
            
            try
            {
                // remove the soap wrapper and return XML object
                response_Received = Parse_SoapMessage(msg, type);

                if (response_Received == null)
                {   
                    // if the message parser returned a null but the comparison object isn't there is a problem
                    // this should never happen, the Parse_SoapMessage should cause an error if no message is found
                    response += "No object found in SOAP message" + Environment.NewLine;
                    verified &= false;
                }
                else
                {
                    // verify the XML
                    verified &= Verify_XML(GetXMLFromSoap(msg, type), ref response);
                }
            }
            catch (Exception e)
            {
                string exceptionString = e.Message;
                // keep the inner excpetion message as well
                if (e.InnerException != null)
                {
                    Exception inner = e.InnerException;
                    exceptionString += " - " + inner.Message;
                }
                throw new XML_DeserializerException("Verify message response parse error - " + exceptionString);
            }

            return verified;

        }

        /// <summary>
        /// Check if there is a soap fault in the XML message, if so return it
        /// </summary>
        /// <param name="message">Message to check</param>
        /// <param name="faultMessage">Soap fault found, or empty string</param>
        /// <returns>True if fault found</returns>
        public bool Check_SoapFault(string message, out string faultMessage)
        {
            System.Xml.XmlNode q;
            System.Xml.XmlNode reason;
            XmlDocument rs = new XmlDocument();

            try
            {
                rs.LoadXml(message);
            }
            catch (Exception e)
            {
                faultMessage = "SOAP Fault Message incorrectly formed - " + e.Message;
                //throw new NetworkInterface_SOAPError("SOAP Fault Message incorrectly formed - " + e.Message);
                return true;
            }



            rs.NameTable.Add("SOAP-ENV");

            // Make sure we have the WSDL namespace declared
            XmlNamespaceManager nm2 = new XmlNamespaceManager(rs.NameTable);
            nm2.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            nm2.AddNamespace("SOAP-ENV", "http://www.w3.org/2003/05/soap-envelope");

            q = rs.SelectSingleNode("//soap:Fault", nm2);

            if (q == null)
                q = rs.SelectSingleNode("//SOAP-ENV:Fault", nm2);

            if (q != null)
            {
                reason = rs.SelectSingleNode("//soap:Fault//soap:Reason", nm2);
                if (reason == null)
                    reason = rs.SelectSingleNode("//SOAP-ENV:Fault//SOAP-ENV:Reason", nm2);

                if (reason != null)
                    faultMessage = reason.InnerText + " - " + rs.InnerText;
                else
                    faultMessage = rs.InnerText;

                if (faultMessage == "")
                {
                    q = rs.SelectSingleNode("//soap:Fault", nm2);
                    if (q == null)
                        q = rs.SelectSingleNode("//SOAP-ENV:Fault", nm2);

                    if (q != null)
                        faultMessage = q.InnerXml;
                }

                return true;
            }
            else
            {
                faultMessage = "";
                return false;
            }
        }

        /// <summary>
        /// Check if the soap fault found matches the type expected
        /// </summary>
        /// <param name="message">Message with soap fault</param>
        /// <param name="faultMessage">Fault message found or error string</param>
        /// <param name="SoapFaultType">Fault message expected</param>
        /// <returns>True if soap message found as expected</returns>
        public bool Verify_SoapErrorType(string message, out string faultMessage, string SoapFaultType)
        {

            System.Xml.XmlNode q;
            System.Xml.XmlNode reason;
            XmlDocument rs = new XmlDocument();
            string prefix = "";
            string allCodes = "";
            string errorCode = "";
            string NS_Error = "";
            string subCode1 = "";
            string subCode2 = "";
            string[] SoapFaults = SoapFaultType.Split(new char[] { '\\' });
                        
            faultMessage = "";

            if (SoapFaultType == "")
                return true;

            try
            {
                rs.LoadXml(message);
            }
            catch (Exception e)
            {
                faultMessage = "SOAP Fault Message incorrectly formed - " + e.Message;
                return false;
            }

            prefix = rs.DocumentElement.Prefix;

            rs.NameTable.Add("SOAP-ENV");

            // Make sure we have the WSDL namespace declared
            XmlNamespaceManager nm2 = new XmlNamespaceManager(rs.NameTable);
            nm2.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            nm2.AddNamespace(prefix, rs.DocumentElement.NamespaceURI);

            q = rs.SelectSingleNode("//" + prefix + ":Code//" + prefix + ":Value", nm2);
            if (q != null)
            {
                errorCode = q.InnerText;
                allCodes = errorCode;
            }

            q = rs.SelectSingleNode("//" + prefix + ":Code//" + prefix + ":Subcode//" + prefix + ":Value", nm2);
            if (q != null)
            {
                subCode1 = q.InnerText;
                allCodes += "\\" + subCode1;
            }

            q = rs.SelectSingleNode("//" + prefix + ":Code//" + prefix + ":Subcode//" + prefix + ":Subcode//" + prefix + ":Value", nm2);
            if (q != null)
            {
                subCode2 = q.InnerText;
                allCodes += "\\" + subCode2;
            }

            // now it is time to compare the soap faults received with the ones expected
            if (SoapFaults.Length > 0)
            {
                // remove the prefix since it will vary with the device and how they format the soap message
                string[] tmpAry = SoapFaults[0].Split(new char[] { ':' });
                if (tmpAry.Length > 1)
                    SoapFaults[0] = tmpAry[1];

                tmpAry = errorCode.Split(new char[] { ':' });
                if (tmpAry.Length > 1)
                    errorCode = tmpAry[1];


                if (SoapFaults[0] != errorCode)
                {
                    faultMessage = "SOAP error found \"" + allCodes + "\" but expected \"" + SoapFaultType + "\"";
                    return false;
                }
            }

            if (SoapFaults.Length > 1)
            {
                if ( (SoapFaults[1] != subCode1) && (! VerifySoapMessage_Namespaces(rs, SoapFaults[1], subCode1, out NS_Error)))
                {
                    faultMessage = "SOAP error found \"" + allCodes + "\" but expected \"" + SoapFaultType + "\"";

                    if (NS_Error != "")
                        faultMessage += ", " + NS_Error;

                    return false;
                }
            }

            if (SoapFaults.Length > 2)
            {
                if ((SoapFaults[2] != subCode2) && (!VerifySoapMessage_Namespaces(rs, SoapFaults[1], subCode1, out NS_Error)))
                {
                    faultMessage = "SOAP error found \"" + allCodes + "\" but expected \"" + SoapFaultType + "\"";

                    if (NS_Error != "")
                        faultMessage += ", " + NS_Error;

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Verify namespaces in SOAP fault message
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="ErrorExpected"></param>
        /// <param name="ErrorFound"></param>
        /// <param name="faultMessage"></param>
        /// <returns></returns>
        private bool VerifySoapMessage_Namespaces(XmlDocument rs, string ErrorExpected, string ErrorFound, out string faultMessage)
        {
            string prefix = "";
            System.Xml.XmlNode q;

            string[] NS_Found = ErrorFound.Split(new char[] { ':' });
            string[] NS_Expected = ErrorExpected.Split(new char[] { ':' });

            string found, expected;

            faultMessage = "";

            // if there isn't a namespace in one just look at the error and ignore the NS
            if ((NS_Found.Length < 2) || (NS_Expected.Length < 2))
            {
                found = NS_Found[0];
                expected = NS_Expected[0];

                if (NS_Found.Length > 1)
                    found = NS_Found[1];

                if (NS_Expected.Length > 1)
                    expected = NS_Expected[1];

                if (found != expected)
                    return false;
                else
                    return true;
            }

            // the value after the namespace should be correct in all occasions
            if (NS_Found[1] != NS_Expected[1])
            {
                faultMessage = "SOAP error found \"" + NS_Found[1] + "\", but expected  \"" + NS_Expected[1] + "\"";
                return false;
            }

            found = NS_Found[0];
            expected = NS_Expected[0];

            prefix = rs.DocumentElement.Prefix;

            rs.NameTable.Add("SOAP-ENV");

            // Make sure we have the WSDL namespace declared
            XmlNamespaceManager nm2 = new XmlNamespaceManager(rs.NameTable);
            nm2.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            nm2.AddNamespace(prefix, rs.DocumentElement.NamespaceURI);

            

            q = rs.SelectSingleNode("//" + prefix + ":Envelope", nm2);
            if (q != null)
            {
                string NameSpace_Name;
                string Correct_NS;
                Namespace_Type tmpNs;
                foreach (XmlAttribute attrib in q.Attributes)
                {
                    string[] tmpAry = attrib.Name.Split(new char[] { ':' });
                    if (tmpAry.Length > 1)
                    {
                        NameSpace_Name = tmpAry[1];
                        if (NameSpace_Name == found)
                        {
                            tmpNs = new Namespace_Type(expected, attrib.Value);

                            if (VerifyNameSpace(tmpNs, out Correct_NS))
                            {
                                return true;
                            }
                            else
                            {
                                //faultMessage = "SOAP error found \"" + ErrorFound + "\", namespace = " + attrib.Value + " but expected  \"" + Correct_NS + "\"";
                                faultMessage = Correct_NS;
                                return false;
                            }

                        }
                    }
                }
            }


            return true;
        }

        /// <summary>
        /// Verify Namespace is correct to the defined name spaces
        /// </summary>
        /// <param name="NS"></param>
        /// <param name="NS_Found"></param>
        /// <returns></returns>
        private bool VerifyNameSpace(Namespace_Type NS, out string NS_Found)
        {
            Namespace_Type? TmpNS = GetNamespace_ByPrefix(NS.Prefix);

            NS_Found = " Error - Namespace \"" + NS.Namespace + "\"not found";

            if (TmpNS != null)
            {
                NS_Found = ((Namespace_Type)TmpNS).Namespace_URL;

                if (NS.Namespace_URL == ((Namespace_Type)TmpNS).Namespace_URL)
                    return true;
                else
                {
                    NS_Found = " Error - Found Namespace \"" + NS.Namespace_URL + "\" but expected \"" + ((Namespace_Type)TmpNS).Namespace_URL + "\"";
                    return false;
                }
            }
            else
            {

                return false; // unknown namespace
            }
             
        }

        /// <summary>
        /// Get namespace type by namespace
        /// </summary>
        /// <param name="Namespace"></param>
        /// <returns></returns>
        private Namespace_Type? GetNamespace_ByNamespace(string Namespace)
        {
            if (Namespace == NS_XML_Media_Types.Namespace) return NS_XML_Media_Types;
            if (Namespace == NS_WS_BaseFault.Namespace) return NS_WS_BaseFault;
            if (Namespace == NS_ONVIF_Schema.Namespace) return NS_ONVIF_Schema;
            if (Namespace == NS_Device_WSDL.Namespace) return NS_Device_WSDL;
            if (Namespace == NS_Media_WSDL.Namespace) return NS_Media_WSDL;
            if (Namespace == NS_Imaging_WSDL.Namespace) return NS_Imaging_WSDL;
            if (Namespace == NS_Event_WSDL.Namespace) return NS_Event_WSDL;
            if (Namespace == NS_PTZ_WSDL.Namespace) return NS_PTZ_WSDL;
            if (Namespace == NS_Analytics_WSDL.Namespace) return NS_Analytics_WSDL;
            if (Namespace == NS_Storage_WSDL.Namespace) return NS_Storage_WSDL;
            if (Namespace == NS_Error_WSDL.Namespace) return NS_Error_WSDL;
            if (Namespace == NS_Network_WSDL.Namespace) return NS_Network_WSDL;
            if (Namespace == NS_Topics.Namespace) return NS_Topics;
            if (Namespace == NS_WSDL.Namespace) return NS_WSDL;
            if (Namespace == NS_Soap_WSDL.Namespace) return NS_Soap_WSDL;
            if (Namespace == NS_HTTP_WSDL.Namespace) return NS_HTTP_WSDL;
            if (Namespace == NS_Encoding.Namespace) return NS_Encoding;
            if (Namespace == NS_Envelope.Namespace) return NS_Envelope;
            if (Namespace == NS_Schema.Namespace) return NS_Schema;
            if (Namespace == NS_Schema_Instance.Namespace) return NS_Schema_Instance;
            if (Namespace == NS_Discovery.Namespace) return NS_Discovery;
            if (Namespace == NS_WS_Discovery_Addressing.Namespace) return NS_WS_Discovery_Addressing;
            if (Namespace == NS_WS_Addressing_Addressing.Namespace) return NS_WS_Addressing_Addressing;
            if (Namespace == NS_Topics_Schema.Namespace) return NS_Topics_Schema;
            if (Namespace == NS_WS_BaseNotification_Schema.Namespace) return NS_WS_BaseNotification_Schema;
            if (Namespace == NS_Optimized_Packaging.Namespace) return NS_Optimized_Packaging;
            if (Namespace == NS_SOAP_Envelope.Namespace) return NS_SOAP_Envelope;

            return null;
        }

        /// <summary>
        /// get Namespace type by prefix
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        private Namespace_Type? GetNamespace_ByPrefix(string Prefix)
        {
            if (Prefix == NS_XML_Media_Types.Prefix) return NS_XML_Media_Types;
            if (Prefix == NS_WS_BaseFault.Prefix) return NS_WS_BaseFault;
            if (Prefix == NS_ONVIF_Schema.Prefix) return NS_ONVIF_Schema;
            if (Prefix == NS_Device_WSDL.Prefix) return NS_Device_WSDL;
            if (Prefix == NS_Media_WSDL.Prefix) return NS_Media_WSDL;
            if (Prefix == NS_Imaging_WSDL.Prefix) return NS_Imaging_WSDL;
            if (Prefix == NS_Event_WSDL.Prefix) return NS_Event_WSDL;
            if (Prefix == NS_PTZ_WSDL.Prefix) return NS_PTZ_WSDL;
            if (Prefix == NS_Analytics_WSDL.Prefix) return NS_Analytics_WSDL;
            if (Prefix == NS_Storage_WSDL.Prefix) return NS_Storage_WSDL;
            if (Prefix == NS_Error_WSDL.Prefix) return NS_Error_WSDL;
            if (Prefix == NS_Network_WSDL.Prefix) return NS_Network_WSDL;
            if (Prefix == NS_Topics.Prefix) return NS_Topics;
            if (Prefix == NS_WSDL.Prefix) return NS_WSDL;
            if (Prefix == NS_Soap_WSDL.Prefix) return NS_Soap_WSDL;
            if (Prefix == NS_HTTP_WSDL.Prefix) return NS_HTTP_WSDL;
            if (Prefix == NS_Encoding.Prefix) return NS_Encoding;
            if (Prefix == NS_Envelope.Prefix) return NS_Envelope;
            if (Prefix == NS_Schema.Prefix) return NS_Schema;
            if (Prefix == NS_Schema_Instance.Prefix) return NS_Schema_Instance;
            if (Prefix == NS_Discovery.Prefix) return NS_Discovery;
            if (Prefix == NS_WS_Discovery_Addressing.Prefix) return NS_WS_Discovery_Addressing;
            if (Prefix == NS_WS_Addressing_Addressing.Prefix) return NS_WS_Addressing_Addressing;
            if (Prefix == NS_Topics_Schema.Prefix) return NS_Topics_Schema;
            if (Prefix == NS_WS_BaseNotification_Schema.Prefix) return NS_WS_BaseNotification_Schema;
            if (Prefix == NS_Optimized_Packaging.Prefix) return NS_Optimized_Packaging;
            if (Prefix == NS_SOAP_Envelope.Prefix) return NS_SOAP_Envelope;

            return null;
        }

        #region Validate Response Messages

        /*************************************************************************************
         * 
         *                      Message Validation Functions
         * 
         * All the message validation functions are basically the same thing.  They require a
         * message (in a string) and a reference to a response string.  The message will be 
         * parsed and validated and any issues found will be put in the response.
         * 
         * Each function has only a single line and that is to call Verify_MessageResponse with 
         * the correct type.  This was done to abstract the message from the message type and 
         * to keep from having to type the "typeof()" value each time.
         * 
         * **********************************************************************************/

        #region Remote Discovery Verify

        //public bool Verify_ProbeResponse(string message, ref string response)
        //{
        //    return Verify_MessageResponse(message, typeof(RemoteDiscovery.ProbeType), ref response);
        //}

        public bool Verify_ProbeMatchesResponse(string message, ref string response)
        {
            return Verify_MessageResponse(message, typeof(RemoteDiscovery.ProbeMatchesType), ref response);
        }

        public bool Verify_HelloResponse(string message, ref string response)
        {
            response = "";
            return Verify_MessageResponse(message, typeof(RemoteDiscovery.HelloType), ref response);
        }

        public bool Verify_ByeResponse(string message, ref string response)
        {
            return Verify_MessageResponse(message, typeof(RemoteDiscovery.ByeType), ref response);
        }

       
        #endregion
        #endregion


        #region Comparison Functions

        /*************************************************************************************
         * 
         *                      Comparison Functions
         *                      
         * These are a series of functions that were designed to break down the complex XML
         * objects into simpler pieces and perform the neccissary comparison and validate
         * NVT responses.  Unfortuntally it was found to take considerable time to implement and
         * it turned out that most of the test cases really only looked at one or two items so 
         * there wasn't much point in breaking down all of the objects.  Not only that the 
         * comparison object had to be completly (or nearly) assembled to be verified against 
         * and this added to the complexity and bugs found in the code were taking longer to fix
         * then just performing the neccissary validations on a per test basis.
         * 
         * There are a few tests that use these functions but for the most part they are just
         * a shadow of what was planned.
         * 
         * 
         * ***********************************************************************************/


        private bool Compare_StringPrefix(string source, string target, ref string response)
        {
            bool verified = true;

            if (source == null) // if source is null there isn't any reson to compare
                return verified;
            else if (target == null) // if the source is null but the target isn't there is a problem
                verified &= false;
            else
            {
                // neither are null so compare the values
                if (!source.Equals("")) // if source isn't blank compare
                    verified &= source.StartsWith(target, StringComparison.OrdinalIgnoreCase);
            }

            return verified;
        }

        private bool Compare_String(string source, string target, ref string response)
        {
            bool verified = true;

            if (source == null) // if source is null there isn't any reson to compare
                return verified;
            else if (target == null) // if the source is null but the target isn't there is a problem
                verified &= false;
            else
            {
                // neither are null so compare the values
                if(!source.Equals("")) // if source isn't blank compare
                    verified &= target.Equals(source);
            }

            return verified;
        }

        private bool Compare_Bool(bool? source, bool? target, ref string response)
        {
            bool verified = true;

            if (source == null) // if source is null there isn't any reson to compare
                return verified;
            else if (target == null) // if the source is null but the target isn't there is a problem
                verified &= false;
            else
            {
                // neither are null so compare the values
                verified &= target.Equals(source);
            }

            return verified;
        }

        private bool Compare_Uint(uint? source, uint? target, ref string response)
        {
            bool verified = true;

            if (source == null) // if source is null there isn't any reson to compare
                return verified;
            else if (target == null) // if the source is null but the target isn't there is a problem
                verified &= false;
            else
            {
                // neither are null so compare the values

                if(source != 0)  // if the source is zero (just initilized) don't compare              
                    verified &= target.Equals(source);
            }

            return verified;
        }
                    
        public bool Compare_RemoteDiscovery_ScopesType(RemoteDiscovery.ScopesType source, RemoteDiscovery.ScopesType target, ref string response)
        {
            int x, y;
            bool verified = true;
            string[] sourceScopeStrings = new string[0];
            string[] targetScopeStrings = new string[0];
            string[] tmpStrings;
            string[] holder;
            char[] splitter = new char[] {' '};

            if (source == null)
            {
                return verified;
            }
            else if (target == null) // if the source is null but the target isn't there is a problem
                verified &= false;
            else
            {
                // in accordinace to Appendex A of the ONVIF test spec, Scopes types are a little special, verify that
                // the URL specified has one of the prefixes specified.
                if (source.Text != null)
                {
                    for (x = 0; x < source.Text.Length; x++)
                    {
                        tmpStrings = source.Text[x].Split(splitter);

                        if (sourceScopeStrings.Length > 0)
                        {
                            holder = sourceScopeStrings;
                            sourceScopeStrings = new string[holder.Length + tmpStrings.Length];
                            Array.Copy(holder, sourceScopeStrings, holder.Length);
                            Array.Copy(tmpStrings, 0, sourceScopeStrings, holder.Length, tmpStrings.Length);
                        }
                        else
                        {
                            sourceScopeStrings = new string[tmpStrings.Length];
                            Array.Copy(tmpStrings, sourceScopeStrings, tmpStrings.Length);
                        }
                    }
                }
                else
                    sourceScopeStrings = null;

                if (target.Text != null)
                {
                    for (x = 0; x < target.Text.Length; x++)
                    {
                        tmpStrings = target.Text[x].Split(splitter);

                        if (targetScopeStrings.Length > 0)
                        {
                            holder = targetScopeStrings;
                            targetScopeStrings = new string[holder.Length + tmpStrings.Length];
                            Array.Copy(holder, targetScopeStrings, holder.Length);
                            Array.Copy(tmpStrings, 0, targetScopeStrings, holder.Length, tmpStrings.Length);
                        }
                        else
                        {
                            targetScopeStrings = new string[tmpStrings.Length];
                            Array.Copy(tmpStrings, targetScopeStrings, tmpStrings.Length);
                        }
                    }
                }
                else
                    targetScopeStrings = null;

                if(verified)
                    verified &= Compare_StringPrefixArray(sourceScopeStrings, targetScopeStrings, ref response);
                
            }

            return verified;

        }

        private bool Compare_StringPrefixArray(string[] source, string[] target, ref string response)
        {
            bool verified = true;
            bool objectFound = false;
            int x, y;

            if (source == null)
                return verified;
            else if (target == null) // if the source is null but the target isn't there is a problem
                verified &= false;
            else
            {

                for (x = 0; x < source.Length; x++)
                {
                    objectFound = false;
                    for (y = 0; y < target.Length; y++)
                    {
                        if (Compare_StringPrefix(target[y], source[x], ref response))
                        {
                            objectFound = true;
                            break;
                        }
                    }

                    if (!objectFound)
                    {
                        response += "Scope type - " + source[x] + " not found" + Environment.NewLine;
                    }

                    verified &= objectFound;
                }


                //// go through all the target objects and compare to all of the
                //// source objects.  A target may have more or less elements then the 
                //// source but each target prefix must be valid according to the list
                //for (x = 0; x < target.Length; x++)
                //{
                //    objectFound = false;
                //    for (y = 0; y < source.Length; y++)
                //    {
                //        if (Compare_StringPrefix(source[y], target[x], ref response))
                //        {
                //            objectFound = true;
                //            break;
                //        }
                //    }
                //    verified &= objectFound;
                //}
            }

            return verified;
        }

        private bool Compare_ObjectArrays(object[] source, object[] target, Type type, ref string response)
        {
            bool verified = true;
            bool objectFound = false;
            int x, y;

            if (source == null)
                return verified;
            else if (target == null) // if the source is null but the target isn't there is a problem
                verified &= false;
            else if (source.Length > target.Length)
                verified &= false;
            else
            {
                // go through all the target objects and compare to all of the
                // source objects.  A target may have more elements then the 
                // source but each source object must be found in the target.
                for (x = 0; x < source.Length; x++)
                {
                    objectFound = false;
                    for (y = 0; y < target.Length; y++)
                    {
                        if (Compare_Object(source[x], target[y], type, ref response))
                        {
                            objectFound = true;
                            break;
                        }
                    }
                    verified &= objectFound;
                }
            }

            return verified;
        }

        private bool Compare_Object(object source, object target, Type type, ref string response)
        {
            bool verified = true;

            if (type == typeof(string))
                verified &= Compare_String((string)source, (string)target, ref response);

            if (type == typeof(bool))
                verified &= Compare_Bool((bool)source, (bool)target, ref response);

            if (type == typeof(uint))
                verified &= Compare_Uint((uint)source, (uint)target, ref response);
                
            else
                throw new XML_DeserializerException("Compare Object failed to find type - " + type.ToString());

            return verified;
        }


        #endregion

        /// <summary>
        /// Parse the IP address out of a a Target URI string
        /// </summary>
        /// <param name="targetURI">URI to be parsed</param>
        /// <returns>IP addres in a string or "" if no address found</returns>
        public string ParseIpAddress(string targetURI)
        {
            int index = 0;
            int x, y;
            string[] pieces = targetURI.Split(new char[] { '.' });
            byte[] parts = new byte[4];
            string tmpQuad;
            char[] tmpAry;

            //Uri streamUri = new Uri(targetURI);

            // try to locate a IP address

            for (x = 0; x < pieces.Length; x++)
            {
                // if this is the first piece, make sure to remove any extra text in fron to the quad
                if (index == 0)
                {
                    if (pieces[x].Length > 3)
                        tmpQuad = pieces[x].Substring(pieces[x].Length - 3, 3);
                    else
                        tmpQuad = pieces[x];

                    // now check the digits, it may be a 1, 2 or 3 digit number
                    tmpAry = tmpQuad.ToCharArray();
                    tmpQuad = "";
                    for (y = tmpAry.Length; y > 0; y--)
                    {
                        if ((tmpAry[y - 1] >= '0') && (tmpAry[y - 1] <= '9'))
                            tmpQuad = tmpQuad.Insert(0, "" + tmpAry[y - 1]);
                        else
                            break; // startin from the end and working down, any non number stops the loop
                    }

                }
                else if (index == 3)
                {
                    // if this is the last peice trim off any extra text on end
                    if (pieces[x].Length > 3)
                        tmpQuad = pieces[x].Substring(0, 3);
                    else
                        tmpQuad = pieces[x];

                    // now check the digits, it may be a 1, 2 or 3 digit number
                    tmpAry = tmpQuad.ToCharArray();
                    tmpQuad = "";
                    for (y = 0; y < tmpAry.Length; y++)
                    {
                        if ((tmpAry[y] >= '0') && (tmpAry[y] <= '9'))
                            tmpQuad += tmpAry[y];
                        else
                            break; // startin from the end and working down, any non number stops the loop
                    }

                }
                else if (index == 4) // an entire IP address was found so just stop
                    break;
                else // this is a middle quad, there shoudln't be anythign to remove
                    tmpQuad = pieces[x];

                // try parsing
                try
                {
                    parts[index] = byte.Parse(tmpQuad);
                    index++;
                }
                catch
                {
                    // it didn't parse, so it is either too large of a number, or not really a number
                    index = 0;
                }

            }

            if (index == 4)
                return parts[0].ToString() + "." + parts[1].ToString() + "." + parts[2].ToString() + "." + parts[3].ToString();



            return "";
        }

    }
}
