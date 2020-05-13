///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using TestTool.GUI.Enums;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Setups page information.
    /// </summary>
    public class SetupInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SetupInfo()
        {
            CoreSpecification = CoreSpecification.V20;   
        }

        /// <summary>
        /// Device information
        /// </summary>
        public DeviceInfo DevInfo { get; set; }
        /// <summary>
        /// Additional information entered by test operator.
        /// </summary>
        public string OtherInfo { get; set; }
        /// <summary>
        /// Tester information
        /// </summary>
        public TesterInfo TesterInfo { get; set; }
        /// <summary>
        /// Core specification selected.
        /// </summary>
        public CoreSpecification CoreSpecification { get; set; }
        
        /// <summary>
        /// True if Core specification 2.0 is selected
        /// </summary>
        /// <returns></returns>
        public bool Specification20Selected()
        {
            return CoreSpecification == CoreSpecification.V20;
        }
    }
}
