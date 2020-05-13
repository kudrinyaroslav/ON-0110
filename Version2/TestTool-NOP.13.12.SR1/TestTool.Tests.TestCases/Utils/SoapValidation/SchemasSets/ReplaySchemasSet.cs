using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    public class ReplaySchemasSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static ReplaySchemasSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>ReplaySchemasSet</returns>
        public static ReplaySchemasSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ReplaySchemasSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected ReplaySchemasSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.replay.xsd",
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

