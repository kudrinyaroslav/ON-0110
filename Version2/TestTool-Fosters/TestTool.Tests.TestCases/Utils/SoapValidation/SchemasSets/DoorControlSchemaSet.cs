///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
/// 

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Schemas for imaging service.
    /// </summary>
    public class DoorControlSchemaSet : BaseSchemaSet
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static DoorControlSchemaSet _instance;

        /// <summary>
        /// Gets instance (if instance has not been created - creates it).
        /// </summary>
        /// <returns>DoorControlSchemaSet</returns>
        public static DoorControlSchemaSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DoorControlSchemaSet();
            }

            return _instance;
        }


        /// <summary>
        /// Initializes schema set.
        /// </summary>
        protected DoorControlSchemaSet()
        {
            string[] schemas = new string[]
                                   {
                                       ROOT + "Extracted.DoorControl.xsd",
                                       ROOT + "Extracted.pacstypes.xsd"
                                    };
            LoadSchemas(schemas);
            
        }


    }
}
