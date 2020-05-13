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
                                       ROOT + "Extracted.deviceio.xsd",
                                       ROOT + "Extracted.AccessControl.xsd",
                                       ROOT + "Extracted.AccessRules.xsd",
                                       ROOT + "Extracted.AdvancedSecurity.xsd",
                                       ROOT + "Extracted.Credential.xsd",
                                       ROOT + "Extracted.Schedule.xsd",
                                       ROOT + "Extracted.DoorControl.xsd",
                                       ROOT + "Extracted.events.xsd",
                                       ROOT + "Extracted.imaging.xsd",
                                       ROOT + "Extracted.media.xsd",
                                       ROOT + "Extracted.ptz.xsd",
                                       ROOT + "Extracted.receiver.xsd",
                                       ROOT + "Extracted.recording.xsd",
                                       ROOT + "Extracted.replay.xsd",
                                       ROOT + "Extracted.search.xsd",
                                       //ROOT + "Extracted.Users.xsd",
                                       ROOT + "Extracted.pacstypes.xsd",
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
