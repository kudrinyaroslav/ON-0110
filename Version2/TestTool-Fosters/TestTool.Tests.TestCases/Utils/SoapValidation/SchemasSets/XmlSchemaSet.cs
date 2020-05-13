using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    class XmlSchemaSet : BaseSchemaSet
    {                         
        /// <summary>
         /// Instance
         /// </summary>
        private static XmlSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>XmlSchemaSet</returns>
        public static XmlSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new XmlSchemaSet();
            }

            return _instance;
        }

       /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected XmlSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "xml.xsd"
                                   };

            LoadSchemas(schemas);            
        }
    }
}
