///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Xml.Schema;

namespace TestTool.Tests.TestCases.Utils.FaultValidation
{
    /// <summary>
    /// Schemas required or events services messages validation.
    /// </summary>
    class EventsSchemasSet : BaseSchemaSet, ISchemasSet
    {
        private static EventsSchemasSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>EventsSchemasSet</returns>
        public static EventsSchemasSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventsSchemasSet();
            }

            return _instance;
        }

        private List<XmlSchema> _soapSchemas;

        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected EventsSchemasSet()
        {
            _soapSchemas = new List<XmlSchema>();
            
            string[] schemas = new string[]
                                   {
                                       "TestTool.Tests.TestCases.Utils.FaultValidation.Schemas.xml.xsd",
                                       "TestTool.Tests.TestCases.Utils.FaultValidation.Schemas.b-2.xsd",
                                       "TestTool.Tests.TestCases.Utils.FaultValidation.Schemas.bf-2.xsd",
                                       "TestTool.Tests.TestCases.Utils.FaultValidation.Schemas.r-2.xsd",
                                       "TestTool.Tests.TestCases.Utils.FaultValidation.Schemas.t-1.xsd",
                                       "TestTool.Tests.TestCases.Utils.FaultValidation.Schemas.ws-addr.xsd"
                                    };

            foreach (string schemaLocation in schemas)
            {
                _soapSchemas.Add(GetXmlSchema(schemaLocation));
            }
            
        }

        #region ISchemasSet Members

        /// <summary>
        /// Returns set of schemas required.
        /// </summary>
        public IEnumerable<XmlSchema> Schemas
        {
            get { return _soapSchemas; }
        }

        #endregion
    }
}
