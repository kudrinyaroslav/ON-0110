using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    public class RecordingSchemasSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static RecordingSchemasSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>RecordingSchemasSet</returns>
        public static RecordingSchemasSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RecordingSchemasSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected RecordingSchemasSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.recording.xsd",
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
