///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Setups page information.
    /// </summary>
    public class SetupInfo
    {
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
    }
}
