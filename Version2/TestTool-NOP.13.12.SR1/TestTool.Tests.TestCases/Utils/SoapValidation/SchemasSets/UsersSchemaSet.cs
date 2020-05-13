///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
/// 

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Schemas for imaging service.
    /// </summary>
    public class UsersSchemaSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static UsersSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>UsersSchemaSet</returns>
        public static UsersSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UsersSchemaSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected UsersSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.Users.xsd",
                                       ROOT + "Extracted.AccessControl.xsd"
                                    };
            LoadSchemas(schemas);
            
        }


    }
}
