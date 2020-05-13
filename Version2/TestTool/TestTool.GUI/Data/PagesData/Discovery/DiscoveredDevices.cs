///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System.Collections.Generic;
using System.Net;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Definitions.Data;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Data from "Discovery" tab.
    /// </summary>
    public  class DiscoveredDevices
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public DiscoveredDevices()
        {
            Discovered = new List<DeviceInfoFull>();
        }

        /// <summary>
        /// Currently selected network interface.
        /// </summary>
        public NetworkInterfaceDescription NIC { get; set; }
        /// <summary>
        /// Discovered devices.
        /// </summary>
        public List<DeviceInfoFull> Discovered { get; private set; }
        /// <summary>
        /// Currently selected device.
        /// </summary>
        public DeviceInfoFull Current { get; set; }
        /// <summary>
        /// Service address.
        /// </summary>
        public string ServiceAddress { get; set; }
        /// <summary>
        /// Device IP address.
        /// </summary>
        public IPAddress DeviceAddress { get; set; }

        /// <summary>
        ///  Search options checkbox state
        /// </summary>
        public bool ShowSearchOptions { get; set; }
        /// <summary>
        /// Scopes for search
        /// </summary>
        public string SearchScopes { get; set; }
    }
}
