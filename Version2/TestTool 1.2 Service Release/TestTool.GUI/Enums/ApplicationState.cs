///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Enums
{
    /// <summary>
    /// Application states
    /// </summary>
    public enum ApplicationState
    {
        /// <summary>
        /// No time-comsuming operations
        /// </summary>
        Idle,
        /// <summary>
        /// Tests are being executed
        /// </summary>
        TestRunning,
        /// <summary>
        /// Test is paused
        /// </summary>
        TestPaused,
        /// <summary>
        /// Discovery running
        /// </summary>
        DiscoveryRunning,
        /// <summary>
        /// Command executed (Device or Requests tab)
        /// </summary>
        CommandRunning
    }
}
