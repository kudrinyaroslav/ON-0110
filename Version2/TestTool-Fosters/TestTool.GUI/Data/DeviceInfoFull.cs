///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.Tests.Common.Discovery;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Full device information
    /// </summary>
    public class DeviceInfoFull
    {
        /// <summary>
        /// Obtained during discovery.
        /// </summary>
        public DeviceDiscoveryData ByDiscovery { get; set;}
        /// <summary>
        /// Obtained via "GetDeviceInformation" method call.
        /// </summary>
        public DeviceInfo ByDeviceInfo { get; set; }

    }
}
