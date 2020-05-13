using System;

///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Device information.
    /// </summary>
    [Serializable]
    public class DeviceInfo
    {

        public string ProductName { get; set; }

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
        /// All available product types via comma
        /// </summary>
        public string ProductTypesAll { get; set; }
        /// <summary>
        /// Product types via comma
        /// </summary>
        public string ProductTypes { get; set; }
        /// <summary>
        /// Product types (other) via comma
        /// </summary>
        public string ProductTypesOther { get; set; }
    }
}
