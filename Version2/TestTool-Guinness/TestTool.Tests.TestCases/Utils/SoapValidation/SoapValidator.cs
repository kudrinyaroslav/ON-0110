///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Xml;
using System.Xml.Schema;
using System.IO;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Validates SOAP packet against schemas passed in constructor.
    /// </summary>
    public class SoapValidator : IValidatingController 
    {
        private ISchemasSet _schemas;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SoapValidator():this(null)
        {

        }
        
        /// <summary>
        /// Constructor with schemas set initialization
        /// </summary>
        /// <param name="schemasSet">Schemas set</param>
        public SoapValidator(ISchemasSet schemasSet)
        {
            _schemas = schemasSet;
        }

        /// <summary>
        /// Validates SOAP message from the stream specified.
        /// </summary>
        /// <param name="stream">Stream containing SOAP packet</param>
        /// <returns>True, if stream contains valid messages.</returns>
        public void Validate(Stream stream)
        {
            XmlReader plainReader = XmlReader.Create(stream);
            while (plainReader.Read()) ;
            stream.Seek(0, SeekOrigin.Begin);
            
            if (_schemas != null)
            {

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.XmlResolver = null; //disable resolver - all schemas should be in place
                
                foreach (XmlSchema schema in _schemas.Schemas)
                {
                    settings.Schemas.Add(schema);
                }

                XmlReader reader = XmlReader.Create(stream, settings);

                while (reader.Read()) ; 
                
            }

        }

    }
}
