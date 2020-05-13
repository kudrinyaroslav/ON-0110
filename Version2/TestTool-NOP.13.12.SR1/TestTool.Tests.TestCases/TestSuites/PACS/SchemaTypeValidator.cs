using System;
using System.Linq;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using TestTool.Tests.CommonUtils.SoapValidation;

namespace TestTool.Tests.TestCases
{
    /// <summary>
    /// Validates attribute value against schema
    /// </summary>
    /// <remarks>The task to solve is the following: check if some string is a correct value of type 
    /// known by its Q-Name. To check this, we create an XML document which will be validated against 
    /// dynamically created schema definition.
    /// </remarks>
    class SchemaTypeValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="schemasSet"></param>
        public SchemaTypeValidator(BaseSchemaSet schemasSet)
        {
            _schemasSet = schemasSet;
        }

        /// <summary>
        /// Schemas list
        /// </summary>
        BaseSchemaSet _schemasSet;

        /// <summary>
        /// Namespace for creating test XML document
        /// </summary>
        const string TESTNAMESPACE = "http://onvif.org/TestTool/Development";

        /// <summary>
        /// Schema pattern
        /// </summary>
        string SCHEMAPATTERN = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<xs:schema attributeFormDefault=\"unqualified\" elementFormDefault=\"qualified\" targetNamespace=\"{0}\" " +
                "xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tns=\"{1}\"> " +
                "<xs:import namespace=\"{1}\" schemaLocation=\"{1}\"/>" +
                "<xs:element name=\"attrHolder\"> " +
                "<xs:complexType><xs:attribute name=\"attr\" type=\"tns:{2}\"/></xs:complexType> " +
                "</xs:element></xs:schema>";

        /// <summary>
        /// Test XML document pattern
        /// </summary>
        string XMLPATTERN = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
            "<ns:attrHolder xmlns:ns=\"{0}\" attr=\"{1}\" ></ns:attrHolder>";

        /// <summary>
        /// Check if value is a correct value of type defined by (ns, type) pair
        /// </summary>
        /// <param name="value">Value string</param>
        /// <param name="type">Type local name</param>
        /// <param name="ns">XSD schema where type is defined</param>
        /// <returns></returns>
        public bool IsValidTypeValue(string value, string type, string ns)
        {
            bool valid = true;
            XmlReader xmlValidatingReader = null;
            try
            {
                string schemaStr = string.Format(SCHEMAPATTERN, TESTNAMESPACE, ns, type);
                TextReader rdr = new StringReader(schemaStr);

                XmlSchema schema = XmlSchema.Read(rdr, null);

                string xmlStr = string.Format(XMLPATTERN, TESTNAMESPACE, value);
                XmlReader xmlReader = new XmlTextReader(new StringReader(xmlStr));
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.XmlResolver = null; //disable resolver - all schemas should be in place

                foreach (XmlSchema sch in _schemasSet.Schemas)
                {
                    settings.Schemas.Add(sch);
                }
                settings.Schemas.Add(schema);

                xmlValidatingReader = XmlReader.Create(xmlReader, settings);

                while (xmlValidatingReader.Read()) ; 
            }
            catch (Exception exc)
            {
                valid = false;
            }
            finally 
            {
                if (xmlValidatingReader != null)
                {
                    xmlValidatingReader.Close();
                }
            
            }
            return valid;
        }


    }
}
