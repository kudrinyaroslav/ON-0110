///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
namespace TestTool.GUI.Data
{
    /// <summary>
    /// Timeouts for testing.
    /// </summary>
    public class Timeouts
    {
        /// <summary>
        /// Timeout for reboot operation.
        /// </summary>
        public int Reboot { get; set; }
        /// <summary>
        /// Timeout for receive device answer.
        /// </summary>
        public int Message { get; set; }
        /// <summary>
        /// Pause between tests.
        /// </summary>
        public int InterTests { get; set; }

    }
}
