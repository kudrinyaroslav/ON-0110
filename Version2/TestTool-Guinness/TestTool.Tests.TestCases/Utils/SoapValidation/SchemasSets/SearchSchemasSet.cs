using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    public class SearchSchemasSet : BaseSchemaSet
    {
                /// <summary>
        /// Instance
        /// </summary>
        private static SearchSchemasSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>SearchSchemasSet</returns>
        public static SearchSchemasSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SearchSchemasSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected SearchSchemasSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.search.xsd",
                                       ROOT + "onvif.xsd",
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
