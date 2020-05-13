﻿///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
/// 

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Schemas for imaging service.
    /// </summary>
    public class ImagingSchemaSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static ImagingSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>ImagingSchemaSet</returns>
        public static ImagingSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ImagingSchemaSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected ImagingSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.imaging.xsd",
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