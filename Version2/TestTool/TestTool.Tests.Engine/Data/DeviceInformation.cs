namespace TestTool.Tests.Engine
{
    public class DeviceInformation
    {
        /// <summary>
        /// Manufacturer
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// Device model
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Device serial number
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Firmware version
        /// </summary>
        public string FirmwareVersion { get; set; }
        /// <summary>
        /// Hardware identifier
        /// </summary>
        public string HardwareID { get; set; }

        /// <summary>
        /// Device capabilities
        /// </summary>
        public Proxies.Onvif.Capabilities Capabilities { get; set; }

        /// <summary>
        /// Services
        /// </summary>
        public Proxies.Onvif.Service[] Services { get; set; }
    }
}
