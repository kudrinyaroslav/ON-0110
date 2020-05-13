///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Discovery page data to be saved.
    /// </summary>
    public class SavedDiscoveryContext
    {
        /// <summary>
        /// Servie address.
        /// </summary>
        public string ServiceAddress { get; set; }
        /// <summary>
        /// Device IP address.
        /// </summary>
        public string DeviceAddress { get; set; }
        /// <summary>
        /// Interface address.
        /// </summary>
        public string InterfaceAddress { get; set; }
    }
    /// <summary>
    /// Information to be saved before application closing.
    /// </summary>
    public class SavedContext
    {
        /// <summary>
        /// Setup page information
        /// </summary>
        public SetupInfo SetupInfo { get; set; }
        /// <summary>
        /// Device environment settings.
        /// </summary>
        public DeviceEnvironment DeviceEnvironment { get; set; }
        /// <summary>
        /// Requests page information
        /// </summary>
        public RequestsInfo RequestsInfo { get; set; }
        /// <summary>
        /// Discovery page information.
        /// </summary>
        public SavedDiscoveryContext DiscoveryContext { get; set; }//discovery from general context cannot be serialized
        /// <summary>
        /// Media page information
        /// </summary>
        public MediaInfo MediaInfo { get; set; }
        /// <summary>
        /// PTZ page information
        /// </summary>
        public PTZInfo PTZInfo { get; set; }

        public DebugInfo DebugInfo { get; set; }

    }
}
