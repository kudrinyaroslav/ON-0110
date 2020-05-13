///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.Tests.TestCases.Utils.SoapValidation
{
    /// <summary>
    /// Base set.
    /// </summary>
    /// <remarks>Not used</remarks>
    class OnvifSchemaSet : BaseSchemaSet
    {
         /// <summary>
         /// Instance
         /// </summary>
        private static OnvifSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>EventsSchemasSet</returns>
        public static OnvifSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OnvifSchemaSet();
            }

            return _instance;
        }

       /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected OnvifSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "onvif.xsd",
                                       ROOT + "xmlmime.xsd",
                                       ROOT + "b-2.xsd",
                                       ROOT + "include.xsd",
                                       ROOT + "xml.xsd",
                                       ROOT + "bf-2.xsd",
                                       ROOT + "t-1.xsd",
                                       ROOT + "ws-addr.xsd"
                                    };

            LoadSchemas(schemas);
            
        }
    }
}
