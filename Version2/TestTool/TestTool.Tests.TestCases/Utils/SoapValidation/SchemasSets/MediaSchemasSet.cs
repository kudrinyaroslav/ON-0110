///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Schemas for media service. 
    /// </summary>
    public class MediaSchemasSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static MediaSchemasSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>MediaSchemasSet</returns>
        public static MediaSchemasSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MediaSchemasSet();
            }

            return _instance;
        }

        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected MediaSchemasSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.media.xsd",
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
