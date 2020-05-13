using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Validates XMl-element against schema
    /// </summary>
    /// <remarks>Used in GetServices with includeCapabilities = true to validate Capabilities element</remarks>
    public class XmlElementValidator
    {
        private BaseSchemaSet _schemas;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlElementValidator():this(null)
        {

        }
        
        /// <summary>
        /// Constructor with schemas set initialization
        /// </summary>
        /// <param name="schemasSet">Schemas set</param>
        public XmlElementValidator(BaseSchemaSet schemasSet)
        {
            _schemas = schemasSet;
        }


        /// <summary>
        /// Validates SOAP message from the stream specified.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>True, if stream contains valid messages.</returns>
        public void Validate(XmlElement element )
        {
            XmlReader plainReader = new XmlNodeReader(element);
            while (plainReader.Read()) ;
            plainReader.Close();
            if (_schemas != null)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.XmlResolver = null; //disable resolver - all schemas should be in place

                foreach (XmlSchema schema in _schemas.Schemas)
                {
                    settings.Schemas.Add(schema);
                }
                plainReader = new XmlNodeReader(element);
                XmlReader reader = XmlNodeReader.Create(plainReader, settings);

                while (reader.Read())
                {

                }


            }
        }

    }
}
