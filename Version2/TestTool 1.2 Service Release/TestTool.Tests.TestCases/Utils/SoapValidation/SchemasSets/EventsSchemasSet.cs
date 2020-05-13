///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.Tests.TestCases.Utils.SoapValidation
{
    /// <summary>
    /// Schemas required or events services messages validation.
    /// </summary>
    class EventsSchemasSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
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

        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected EventsSchemasSet()
        {
           
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.events.xsd",
                                       ROOT + "onvif.xsd",
                                       ROOT + "xmlmime.xsd",
                                       ROOT + "b-2.xsd",
                                       ROOT + "include.xsd",
                                       ROOT + "xml.xsd",
                                       ROOT + "bf-2.xsd",
                                       ROOT + "r-2.xsd",
                                       ROOT + "t-1.xsd",
                                       ROOT + "ws-addr.xsd"
                                    };

            LoadSchemas(schemas);
            
        }

    }
}
