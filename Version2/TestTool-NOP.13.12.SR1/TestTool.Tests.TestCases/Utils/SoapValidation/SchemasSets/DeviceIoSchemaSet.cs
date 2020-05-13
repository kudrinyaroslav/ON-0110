///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Schems for IO service
    /// </summary>
    public class DeviceIoSchemaSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static DeviceIoSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>DeviceIoSchemaSet</returns>
        public static DeviceIoSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DeviceIoSchemaSet();
            }

            return _instance;
        }

        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected DeviceIoSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.deviceio.xsd",
                                       ROOT + "onvif.xsd",
                                       ROOT + "envelope.xsd",
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
