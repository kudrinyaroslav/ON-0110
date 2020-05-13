﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Tests.CommonUtils.SoapValidation;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    public class AccessRulesSchemaSet: BaseSchemaSet
    {
                /// <summary>
        /// Instance
        /// </summary>
        private static AccessRulesSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>RecordingSchemasSet</returns>
        public static AccessRulesSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AccessRulesSchemaSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected AccessRulesSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.AccessRules.xsd",
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
