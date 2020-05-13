using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    public class TypesSchemaSet : BaseSchemaSet
    {
                 /// <summary>
         /// Instance
         /// </summary>
        private static TypesSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>EventsSchemasSet</returns>
        public static TypesSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TypesSchemaSet();
            }

            return _instance;
        }

       /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected TypesSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "onvif.xsd",
                                       ROOT + "Extracted.devicemgmt.xsd",
                                       ROOT + "Extracted.deviceio.xsd",
                                       ROOT + "Extracted.media.xsd",
                                       ROOT + "Extracted.ptz.xsd",
                                       ROOT + "Extracted.imaging.xsd",
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
