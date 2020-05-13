///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Application data.
    /// </summary>
    public class GeneralContext
    {
        /// <summary>
        /// Application information
        /// </summary>
        public ApplicationInfo AppInfo { get; set; }

        /// <summary>
        /// Devices found at the Discovery tab
        /// </summary>
        public DiscoveredDevices Devices { get; set; }

        /// <summary>
        /// Setup information from Setup tab
        /// </summary>
        public SetupInfo SetupInfo { get; set; }

        /// <summary>
        /// Data from Management tab
        /// </summary>
        public DeviceEnvironment Environment { get; set; }

        /// <summary>
        /// Selected tests and "Interactive first" - necessary for Profile
        /// </summary>
        public TestOptions TestOptions { get; set; }

        /// <summary>
        /// Test log to be passed from Test tab to Report tab
        /// </summary>
        public TestLog TestLog { get; set; }

        /// <summary>
        /// Debug settings
        /// </summary>
        public DebugInfo DebugInfo { get; set; }
        /// <summary>
        /// Requests page information
        /// </summary>
        public RequestsInfo RequestsInfo { get; set; }

        /// <summary>
        /// Media page information
        /// </summary>
        public MediaInfo MediaInfo { get; set; }

        /// <summary>
        /// PTZ page information.
        /// </summary>
        public PTZInfo PTZInfo { get; set; }
    }
}
