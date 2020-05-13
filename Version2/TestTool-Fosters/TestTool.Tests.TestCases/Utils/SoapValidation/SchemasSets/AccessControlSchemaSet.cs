///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
/// 

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Schemas for imaging service.
    /// </summary>
    public class AccessControlSchemaSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static AccessControlSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>AccessControlSchemaSet</returns>
        public static AccessControlSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AccessControlSchemaSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected AccessControlSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.AccessControl.xsd",
                                       ROOT + "Extracted.pacstypes.xsd"
                                    };
            LoadSchemas(schemas);
            
        }


    }
}
