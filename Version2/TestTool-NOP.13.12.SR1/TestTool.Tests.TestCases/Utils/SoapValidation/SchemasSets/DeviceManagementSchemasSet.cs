///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
/// 

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Device management service schemas.
    /// </summary>
    public class DeviceManagementSchemasSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static DeviceManagementSchemasSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>EventsSchemasSet</returns>
        public static DeviceManagementSchemasSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DeviceManagementSchemasSet();
            }

            return _instance;
        }

        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected DeviceManagementSchemasSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.devicemgmt.xsd",
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
