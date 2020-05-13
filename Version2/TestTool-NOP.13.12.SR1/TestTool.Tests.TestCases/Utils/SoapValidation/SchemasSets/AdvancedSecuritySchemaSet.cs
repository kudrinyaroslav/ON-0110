﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Tests.CommonUtils.SoapValidation;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    public class AdvancedSecuritySchemaSet: BaseSchemaSet
    {
                /// <summary>
        /// Instance
        /// </summary>
        private static AdvancedSecuritySchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>RecordingSchemasSet</returns>
        public static AdvancedSecuritySchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AdvancedSecuritySchemaSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected AdvancedSecuritySchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.AdvancedSecurity.xsd",
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