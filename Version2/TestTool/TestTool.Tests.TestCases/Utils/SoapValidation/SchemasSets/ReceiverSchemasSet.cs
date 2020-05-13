using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.CommonUtils.SoapValidation;

namespace TestTool.Tests.TestCases.Utils.SoapValidation
{
    public class ReceiverSchemasSet : BaseSchemaSet
    {
                        
        /// <summary>
        /// Instance
        /// </summary>
        private static ReceiverSchemasSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>ReceiverSchemasSet</returns>
        public static ReceiverSchemasSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ReceiverSchemasSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected ReceiverSchemasSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.receiver.xsd",
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
