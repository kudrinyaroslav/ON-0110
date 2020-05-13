///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Information entered at "Management" tab (with credentials added) 
    /// </summary>
    public class DeviceEnvironment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public  DeviceEnvironment()
        {
            Credentials = new Credentials();

            Timeouts = new Timeouts();
            Timeouts.InterTests = 0;
            Timeouts.Message = 30000;
            Timeouts.Reboot = 30000;

            EnvironmentSettings = new EnvironmentSettings();

            TestSettings = new TestSettings();
        }

        /// <summary>
        /// Admin account
        /// </summary>
        public Credentials Credentials { get; set; }
        /// <summary>
        /// Timeouts (message timeout,  reboot timeout etc.)
        /// </summary>
        public Timeouts Timeouts { get; set; }
        /// <summary>
        /// Environment-dependent settings (DNS, NTP etc addresses)
        /// </summary>
        public EnvironmentSettings EnvironmentSettings { get; set; }
        /// <summary>
        /// Miscellaneous test settings (mostly defined by implementation details which can differ for different devices)
        /// </summary>
        public TestSettings TestSettings { get; set; }
    }
}
