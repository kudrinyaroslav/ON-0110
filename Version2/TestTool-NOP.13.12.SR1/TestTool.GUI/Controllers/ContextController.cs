///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.GUI.Data;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Holds application data (values entered/selected by the user etc)
    /// </summary>
    static class ContextController
    {
        private static readonly GeneralContext _context = new GeneralContext();

        /// <summary>
        /// General context
        /// </summary>
        public static GeneralContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Initializes context data
        /// </summary>
        public static void InitGeneralContext()
        {
            _context.AppInfo = new ApplicationInfo();
            _context.Devices = new DiscoveredDevices();
            _context.Environment = new DeviceEnvironment();
            _context.SetupInfo = new SetupInfo();
            _context.TestOptions = new TestOptions();
            _context.TestLog = new TestLog();
            _context.MediaInfo = new MediaInfo();
            _context.PTZInfo = new PTZInfo();
            _context.DebugInfo = new DebugInfo();
        }

        /// <summary>
        /// Updates device enviroment data
        /// </summary>
        /// <param name="environment">Structure with new values</param>
        public static void UpdateDeviceEnvironment(DeviceEnvironment environment)
        {
            _context.Environment = environment;
        }

        /// <summary>
        /// Returns device environment data (credentials, timeouts, features selected by the user).
        /// </summary>
        /// <returns>Device environment data </returns>
        public static DeviceEnvironment GetDeviceEnvironment()
        {
            return _context.Environment;
        }

        public static Data.Log.ManagementSettings GetManagementSettings()
        {
            Data.Log.ManagementSettings settings = new TestTool.GUI.Data.Log.ManagementSettings();
            
            DeviceEnvironment environment = GetDeviceEnvironment();
            TestSettings testSettings = environment.TestSettings;

            TestTool.GUI.Data.Log.Timeouts timeouts = new TestTool.GUI.Data.Log.Timeouts();
            timeouts.MessageTimeout = environment.Timeouts.Message;
            timeouts.OperationDelay = testSettings.OperationDelay;
            timeouts.RebootTimeout = environment.Timeouts.Reboot;
            timeouts.TimeBetweenRequests = testSettings.RecoveryDelay;
            timeouts.TimeBetweenTests = environment.Timeouts.InterTests;

            settings.Timeouts = timeouts;

            Data.Log.Miscellaneous misc = new TestTool.GUI.Data.Log.Miscellaneous();
            misc.DefaultGatewayIpv4 = environment.EnvironmentSettings.GatewayIpv4;
            misc.DefaultGatewayIpv6 = environment.EnvironmentSettings.GatewayIpv6;
            misc.DnsIpv4 = environment.EnvironmentSettings.DnsIpv4;
            misc.DnsIpv6 = environment.EnvironmentSettings.DnsIpv6;
            misc.NtpIpv4 = environment.EnvironmentSettings.NtpIpv4;
            misc.NtpIpv6 = environment.EnvironmentSettings.NtpIpv6;

            misc.EventTopic = testSettings.EventTopic;
            misc.MetadataFilter = testSettings.MetadataFilter;
            misc.Password1 = testSettings.Password1;
            misc.Password2 = testSettings.Password2;
            misc.PTZNodeToken = testSettings.PTZNodeToken;
            misc.RecordingToken = testSettings.RecordingToken;
            misc.RelayOutputDelayTime = testSettings.RelayOutputDelayTimeMonostable;
            misc.SearchTimeout = testSettings.SearchTimeout;
            misc.SecureMethod = testSettings.SecureMethod;
            misc.SubscriptionTimeout = testSettings.SubscriptionTimeout;
            misc.TopicNamespaces = testSettings.TopicNamespaces;
            misc.UseEmbeddedPassword = testSettings.UseEmbeddedPassword;
            misc.RetentionTime = testSettings.RetentionTime;

            settings.Miscellaneous = misc;

            return settings;
        }

        /// <summary>
        /// Updates list of devices discovered, currently selected device, selected NIC, service address etc.
        /// </summary>
        /// <param name="devices">"Discovery" page data.</param>
        public static void UpdateDiscoveredDevices(DiscoveredDevices devices)
        {
            _context.Devices = devices;
        }

        /// <summary>
        /// Gets data entered/obtained at the "Discovery" page.
        /// </summary>
        /// <returns>Data entered/obtained at the "Discovery" page.</returns>
        public static DiscoveredDevices GetDiscoveredDevices()
        {
            return _context.Devices;
        }

        /// <summary>
        /// Gets data entered by the user at the "Test" tab (selected tests and groups, "Interactive first" 
        /// button state).
        /// </summary>
        /// <returns>Data entered at the "Test" tab.</returns>
        public static TestOptions GetTestOptions()
        {
            return _context.TestOptions;
        }

        /// <summary>
        /// Updates data entered at the "Test" tab.
        /// </summary>
        /// <param name="options">Data entered at the "Test" tab.</param>
        public static void UpdateTestOptions(TestOptions options)
        {
            _context.TestOptions = options;
        }

        /// <summary>
        /// Updates test execution information.
        /// </summary>
        /// <param name="log">Test execution information.</param>
        public static void UpdateTestLog(TestLog log)
        {
            _context.TestLog = log;
        }

        /// <summary>
        /// Gets test execution information.
        /// </summary>
        /// <returns>Test execution information.</returns>
        public static TestLog GetTestLog()
        {
            return _context.TestLog;
        }
    
        /// <summary>
        /// Gets data from the "Setup" tab.
        /// </summary>
        /// <returns></returns>
        public static SetupInfo GetSetupInfo()
        {
            return _context.SetupInfo;
        }

        /// <summary>
        /// Updates data from the "Setup" tab.
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateSetupInfo(SetupInfo info)
        {
            _context.SetupInfo = info;
        }

        /// <summary>
        /// Gets application information.
        /// </summary>
        /// <returns></returns>
        public static ApplicationInfo GetApplicationInfo()
        {
            return _context.AppInfo;
        }

        /// <summary>
        /// Gets data from the "Requests" tab.
        /// </summary>
        /// <returns></returns>
        public static RequestsInfo GetRequestsInfo()
        {
            return _context.RequestsInfo;
        }

        /// <summary>
        /// Gets data from the "Media" tab.
        /// </summary>
        /// <returns></returns>
        public static MediaInfo GetMediaInfo()
        {
            return _context.MediaInfo;
        }

        public static  DebugInfo GetDebugInfo()
        {
            return _context.DebugInfo;
        }

        /// <summary>
        /// Updates report information.
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateRequestsInfo(RequestsInfo info)
        {
            _context.RequestsInfo = info;
        }

        /// <summary>
        /// Updates media information.
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateMediaInfo(MediaInfo info)
        {
            _context.MediaInfo = info;
        }
        
        /// <summary>
        /// Updates ptz information.
        /// </summary>
        /// <param name="info"></param>
        public static void UpdatePTZInfo(PTZInfo info)
        {
            _context.PTZInfo = info;
        }

        public static void UpdateDebugInfo(DebugInfo info)
        {
            _context.DebugInfo = info;
        }

        /// <summary>
        /// Gets data from the "PTZ" tab.
        /// </summary>
        /// <returns></returns>
        public static PTZInfo GetPTZInfo()
        {
            return _context.PTZInfo;
        }
    }
}
