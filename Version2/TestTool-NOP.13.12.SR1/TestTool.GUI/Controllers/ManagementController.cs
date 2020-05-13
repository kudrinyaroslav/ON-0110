///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Definitions.UI;
using Onvif = TestTool.Proxies.Onvif;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// GUI logic for Management tab.
    /// </summary>
    class ManagementController : Controller<IManagementView>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view">View.</param>
        public ManagementController(IManagementView view) 
            : base(view)
        {
            _profiles = new ProfilesSet();
        }

        public event EventHandler SettingsLoaded;

        public const string EVENTSSETTINGS = "EventTopic";

        /// <summary>
        /// Updates context data when a user leaves management tab. 
        /// </summary>
        public override void UpdateContext()
        {
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment(); 

            environment.Timeouts = new Timeouts();
            environment.Timeouts.InterTests = View.SettingsView.TimeBetweenTests;
            environment.Timeouts.Message = View.SettingsView.MessageTimeout;
            environment.Timeouts.Reboot = View.SettingsView.RebootTimeout;

            environment.EnvironmentSettings = new EnvironmentSettings();
            environment.EnvironmentSettings.DnsIpv4 = View.SettingsView.DnsIpv4;
            environment.EnvironmentSettings.NtpIpv4 = View.SettingsView.NtpIpv4;
            environment.EnvironmentSettings.DnsIpv6 = View.SettingsView.DnsIpv6;
            environment.EnvironmentSettings.NtpIpv6 = View.SettingsView.NtpIpv6;
            environment.EnvironmentSettings.GatewayIpv4 = View.SettingsView.GatewayIpv4;
            environment.EnvironmentSettings.GatewayIpv6 = View.SettingsView.GatewayIpv6;

            environment.TestSettings = new TestSettings();
            environment.TestSettings.PTZNodeToken = View.SettingsView.PTZNodeToken;

            environment.TestSettings.UseEmbeddedPassword = View.SettingsView.UseEmbeddedPassword;
            environment.TestSettings.Password1 = View.SettingsView.Password1;
            environment.TestSettings.Password2 = View.SettingsView.Password2;
            environment.TestSettings.OperationDelay = View.SettingsView.OperationDelay;
            environment.TestSettings.RecoveryDelay = View.SettingsView.RecoveryDelay;
            environment.TestSettings.VideoSourceToken = View.SettingsView.VideoSourceToken;

            environment.TestSettings.SecureMethod = View.SettingsView.SecureMethod;
            environment.TestSettings.SubscriptionTimeout = View.SettingsView.SubscriptionTimeout;
            environment.TestSettings.EventTopic = View.SettingsView.EventTopic;
            environment.TestSettings.TopicNamespaces = View.SettingsView.TopicNamespaces;

            environment.TestSettings.RelayOutputDelayTimeMonostable = View.SettingsView.RelayOutputDelayTimeMonostable;

            environment.TestSettings.RecordingToken = View.SettingsView.RecordingToken;
            environment.TestSettings.SearchTimeout = View.SettingsView.SearchTimeout;
            environment.TestSettings.MetadataFilter = View.SettingsView.MetadataFilter;

            environment.TestSettings.RetentionTime = View.SettingsView.RetentionTime;

            environment.TestSettings.RawAdvancedSettings = GetAdvancedSettings();

            List<object> advanced = View.SettingsView.AdvancedSettings;
            environment.TestSettings.AdvancedSettings = advanced;

        }

        /// <summary>
        /// Updates view.
        /// </summary>
        public override void UpdateView()
        {
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
            View.SettingsView.TimeBetweenTests = environment.Timeouts.InterTests;
            View.SettingsView.MessageTimeout = environment.Timeouts.Message;
            View.SettingsView.RebootTimeout = environment.Timeouts.Reboot;

            View.SettingsView.OperationDelay = environment.TestSettings.OperationDelay;
           
            View.SettingsView.RecoveryDelay = environment.TestSettings.RecoveryDelay;
            View.SettingsView.DnsIpv4 = environment.EnvironmentSettings.DnsIpv4;
            View.SettingsView.DnsIpv6 = environment.EnvironmentSettings.DnsIpv6;
            View.SettingsView.NtpIpv4 = environment.EnvironmentSettings.NtpIpv4;
            View.SettingsView.NtpIpv6 = environment.EnvironmentSettings.NtpIpv6;
            View.SettingsView.GatewayIpv4 = environment.EnvironmentSettings.GatewayIpv4;
            View.SettingsView.GatewayIpv6 = environment.EnvironmentSettings.GatewayIpv6;

            View.SettingsView.UseEmbeddedPassword = environment.TestSettings.UseEmbeddedPassword;
            View.SettingsView.Password1 = environment.TestSettings.Password1;
            View.SettingsView.Password2 = environment.TestSettings.Password2;
            View.SettingsView.SecureMethod = environment.TestSettings.SecureMethod;

            View.SettingsView.PTZNodeToken = environment.TestSettings.PTZNodeToken;
            
            View.SettingsView.SubscriptionTimeout = environment.TestSettings.SubscriptionTimeout;
            View.SettingsView.EventTopic = environment.TestSettings.EventTopic;
            View.SettingsView.TopicNamespaces = environment.TestSettings.TopicNamespaces;

            View.SettingsView.RelayOutputDelayTimeMonostable = environment.TestSettings.RelayOutputDelayTimeMonostable;

            View.SettingsView.RecordingToken = environment.TestSettings.RecordingToken;
            View.SettingsView.SearchTimeout = environment.TestSettings.SearchTimeout;
            View.SettingsView.MetadataFilter = environment.TestSettings.MetadataFilter;
            View.SettingsView.RetentionTime = environment.TestSettings.RetentionTime;

            DisplayAdvancedSettings(environment.TestSettings.RawAdvancedSettings);
        }

        /// <summary>
        /// Performs initialization.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            
            View.DisplayProfiles(Load());
        }

        /// <summary>
        /// Shows missing settings. Currently includes only Events settings.
        /// </summary>
        /// <param name="settings"></param>
        public void ShowSettings(List<string> settings)
        {
            View.SettingsView.ShowMissingSettings(EVENTSSETTINGS);
        }

        /// <summary>
        /// Types of additional test settings
        /// </summary>
        private Dictionary<string, Type> _advancedSettingsTypes;

        /// <summary>
        /// Adds settings pages
        /// </summary>
        /// <param name="pages"></param>
        public void AddSettingsPages(List<SettingsTabPage> pages)
        {
            _advancedSettingsTypes = new Dictionary<string, Type>();
            foreach (SettingsTabPage page in pages)
            {
                Type t = page.ParametersType;
                _advancedSettingsTypes.Add(t.Name, t);
            }

            View.SettingsView.AddSettingsPages(pages);
        }
        
        #region PROFILES

        /// <summary>
        /// Profiles set.
        /// </summary>
        private ProfilesSet _profiles;

        /// <summary>
        /// List of profiles.
        /// </summary>
        public List<Profile> Profiles
        {
            get { return _profiles.Profiles; }
        }

        /// <summary>
        /// Loads profiles.
        /// </summary>
        /// <returns>list of profiles.</returns>
        public List<Profile> Load()
        {
            _profiles = ProfilesManager.Load();
            return _profiles.Profiles;
        }

        /// <summary>
        /// Saves current settings as a profile.
        /// </summary>
        /// <param name="profileName">Profile name.</param>
        /// <returns>New profile object.</returns>
        public Profile SaveCurrentProfile(string profileName)
        {
            Profile profile = _profiles.Profiles.Where(p => p.Name == profileName).FirstOrDefault();
            if (profile == null)
            {
                profile = new Profile(profileName);
                _profiles.Profiles.Add(profile);
            }

            profile.InterTests = View.SettingsView.TimeBetweenTests;
            profile.Reboot = View.SettingsView.RebootTimeout;
            profile.Message = View.SettingsView.MessageTimeout;

            profile.OperationDelay = View.SettingsView.OperationDelay;
            profile.RecoveryDelay = View.SettingsView.RecoveryDelay;

            profile.DnsIpv4 = View.SettingsView.DnsIpv4;
            profile.NtpIpv4 = View.SettingsView.NtpIpv4;
            profile.GatewayIpv4 = View.SettingsView.GatewayIpv4;

            profile.DnsIpv6 = View.SettingsView.DnsIpv6;
            profile.NtpIpv6 = View.SettingsView.NtpIpv6;
            profile.GatewayIpv6 = View.SettingsView.GatewayIpv6;

            profile.UseEmbeddedPassword = View.SettingsView.UseEmbeddedPassword;
            profile.Password1 = View.SettingsView.Password1;
            profile.Password2 = View.SettingsView.Password2;
            profile.SecureMethod = View.SettingsView.SecureMethod;

            profile.PTZNodeToken = View.SettingsView.PTZNodeToken;
            profile.VideoSourceToken = View.SettingsView.VideoSourceToken;

            profile.EventTopic = View.SettingsView.EventTopic;
            profile.TopicNamespaces = View.SettingsView.TopicNamespaces;
            profile.SubscriptionTimeout = View.SettingsView.SubscriptionTimeout;
            profile.RelayOutputDelayTime = View.SettingsView.RelayOutputDelayTimeMonostable;
            

            TestOptions options = ContextController.GetTestOptions();

            profile.TestCases = options.Tests;
            profile.TestGroups = options.Groups;
            
            // Save advanced settings - for future use
            profile.AdvancedSettings = GetAdvancedSettings();
            // End of "Advanced settings" part

            ProfilesManager.Save(_profiles);
            return profile;
            
        }

        /// <summary>
        /// Gets advanced settings
        /// </summary>
        /// <returns></returns>
        List<XmlElement> GetAdvancedSettings()
        {
            return AdvancedParametersUtils.Serialize(View.SettingsView.AdvancedSettings);
        }

        /// <summary>
        /// Is raised when a user clicks "Apply profile" button. 
        /// </summary>
        public event Action<Profile> ProfileApplied;
        
        /// <summary>
        /// Applies profile selected.
        /// </summary>
        /// <param name="profile">Profile data.</param>
        public void ApplyProfile(Profile profile)
        {
            UpdateContext();

            DisplayAdvancedSettings(profile.AdvancedSettings);
            
            if (ProfileApplied != null)
            {
                ProfileApplied(profile);
            }
        }

        /// <summary>
        /// Parses additional test settings, if they are present
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        List<object> ParseAdvancedSettings(IEnumerable<XmlElement> elements)
        {
            List<object> advancedSettings =
                Utils.AdvancedParametersUtils.Deserialize(elements, _advancedSettingsTypes.Values);

            return advancedSettings;
        }

        /// <summary>
        /// Displays additional test settings.
        /// </summary>
        /// <param name="elements"></param>
        void DisplayAdvancedSettings(List<XmlElement> elements)
        {
            if (elements == null)
            {
                View.SettingsView.AdvancedSettings = null;
                return;
            }


            List<object> advancedSettings = ParseAdvancedSettings(elements);
            View.SettingsView.AdvancedSettings = advancedSettings;
        }

        #endregion

        #region Save as XML

        public void SaveAsXML(string fileName)
        {
            // save;

            SerializableTestingParameters parameters = new SerializableTestingParameters();

            {
                parameters.Advanced = GetAdvancedSettings().ToArray();

                parameters.Device = new DeviceParameters();
                DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
                parameters.Device.DeviceIP = (null != devices.DeviceAddress) ? devices.DeviceAddress.ToString() : "";
                parameters.Device.DeviceServiceAddress = devices.ServiceAddress;

                DeviceInfo devInfo = ContextController.GetSetupInfo().DevInfo;
                parameters.Device.Model = devInfo != null ? devInfo.Model : string.Empty;

                {
                    parameters.Output = new Output();
                    parameters.Output.CreateNestedFolder = true;
                    parameters.Output.Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    parameters.Output.FeatureDefinitionLog = "FeatureDefinitionLog.xml";
                    parameters.Output.Report = "TestReport.pdf";
                    parameters.Output.TestLog = "TestLog.xml";

                }

                {
                    SetupInfo info = ContextController.GetSetupInfo();
                    if (info != null)
                    {
                        parameters.SessionInfo = new SessionInfo();
                        parameters.SessionInfo.MemberInfo = info.MemberInfo;
                        parameters.SessionInfo.OtherInformation = info.OtherInfo;
                        parameters.SessionInfo.TesterInfo = info.TesterInfo;
                    }
                }

                {
                    parameters.TestParameters = new TestParameters();
                    parameters.TestParameters.Address = devices.NIC.IP.ToString();
                    
                    DeviceEnvironment env = ContextController.GetDeviceEnvironment();

                    if (env.EnvironmentSettings != null)
                    {
                        parameters.TestParameters.DefaultGatewayIpv4 = env.EnvironmentSettings.GatewayIpv4;
                        parameters.TestParameters.DefaultGatewayIpv6 = env.EnvironmentSettings.GatewayIpv6;
                        parameters.TestParameters.DnsIpv4 = env.EnvironmentSettings.DnsIpv4;
                        parameters.TestParameters.DnsIpv6 = env.EnvironmentSettings.DnsIpv6;
                        parameters.TestParameters.NtpIpv4 = env.EnvironmentSettings.NtpIpv4;
                        parameters.TestParameters.NtpIpv6 = env.EnvironmentSettings.NtpIpv6;
                    }

                    if (env.Credentials != null)
                    {
                        parameters.TestParameters.UserName = env.Credentials.UserName;
                        parameters.TestParameters.Password = env.Credentials.Password;

                    }

                    if (env.Timeouts != null)
                    { 
                        parameters.TestParameters.MessageTimeout = env.Timeouts.Message;
                        parameters.TestParameters.RebootTimeout = env.Timeouts.Reboot;
                        parameters.TestParameters.TimeBetweenTests = env.Timeouts.InterTests;
                    }

                    if (env.TestSettings != null)
                    {
                        parameters.TestParameters.EventTopic = env.TestSettings.EventTopic;
                        parameters.TestParameters.TopicNamespaces = env.TestSettings.TopicNamespaces;
                        
                        parameters.TestParameters.OperationDelay = env.TestSettings.OperationDelay;
                        parameters.TestParameters.Password1 = env.TestSettings.Password1;
                        parameters.TestParameters.Password2 = env.TestSettings.Password2;
                        parameters.TestParameters.PTZNodeToken = env.TestSettings.PTZNodeToken;
                        parameters.TestParameters.RelayOutputDelayTime = env.TestSettings.RelayOutputDelayTimeMonostable;
                        parameters.TestParameters.SecureMethod = env.TestSettings.SecureMethod;
                        parameters.TestParameters.SubscriptionTimeout = env.TestSettings.SubscriptionTimeout;
                        parameters.TestParameters.TimeBetweenRequests = env.TestSettings.RecoveryDelay;
                        parameters.TestParameters.UseEmbeddedPassword = env.TestSettings.UseEmbeddedPassword;
                        parameters.TestParameters.VideoSourceToken = env.TestSettings.VideoSourceToken;

                        parameters.TestParameters.SearchTimeout = env.TestSettings.SearchTimeout;
                        parameters.TestParameters.RecordingToken = env.TestSettings.RecordingToken;
                        parameters.TestParameters.MetadataFilter = env.TestSettings.MetadataFilter;

                    }
                }
            }
            

            XmlSerializer serializer = new XmlSerializer(typeof(SerializableTestingParameters));
            FileStream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.OpenOrCreate);
                serializer.Serialize(stream, parameters);
            }
            catch (Exception exc)
            {
                View.ShowError(exc);
                return;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            // open in NotePad, if needed

            if (View.OpenFileForEditing)
            {
                ProcessStartInfo ProcessInfo;
                Process Process;

                try
                {
                    ProcessInfo = new ProcessStartInfo("Notepad.exe", fileName);
                    ProcessInfo.CreateNoWindow = false;
                    ProcessInfo.UseShellExecute = true;
                    Process = Process.Start(ProcessInfo);
                }
                catch (System.Exception ex)
                {
                    View.ShowError(ex);
                }

            }
        }

        public void LoadSettingsFromXML(string fileName)
        {
            try
            {
                SerializableTestingParameters userParameters = null;

                if (!File.Exists(fileName))
                {
                    View.ShowError(string.Format("File not found: {0}", fileName));
                    return;
                }

                XmlSerializer serializer = new XmlSerializer(typeof(SerializableTestingParameters));
                FileStream stream = null;
                try
                {
                    stream = new FileStream(fileName, FileMode.Open);
                    userParameters = (SerializableTestingParameters)serializer.Deserialize(stream);
                }
                catch (Exception exc)
                {
                    View.ShowError(exc);
                    View.ShowError(string.Format("Parameters loading failed: {0}", exc.Message));
                    return;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }

                // update context
                {                        
                    DeviceEnvironment devEnv = ContextController.GetDeviceEnvironment();
                    
                    if (userParameters.Advanced != null)
                    { 
                        devEnv.TestSettings.AdvancedSettings = ParseAdvancedSettings(userParameters.Advanced);
                        devEnv.TestSettings.RawAdvancedSettings = new List<XmlElement>(userParameters.Advanced);
                    }

                    if (userParameters.Device != null)
                    { 
                        DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
                        devices.ServiceAddress = userParameters.Device.DeviceServiceAddress;
                    }

                    if (userParameters.SessionInfo != null)
                    { 
                    
                    }

                    if (userParameters.TestParameters != null)
                    { 
                        TestParameters testParameters =userParameters.TestParameters;

                        devEnv.Credentials.UserName = testParameters.UserName;
                        devEnv.Credentials.Password = testParameters.Password;

                        devEnv.EnvironmentSettings.DnsIpv4 = testParameters.DnsIpv4;
                        devEnv.EnvironmentSettings.DnsIpv6 = testParameters.DnsIpv6;
                        devEnv.EnvironmentSettings.GatewayIpv4 = testParameters.DefaultGatewayIpv4;
                        devEnv.EnvironmentSettings.GatewayIpv6 = testParameters.DefaultGatewayIpv6;
                        devEnv.EnvironmentSettings.NtpIpv4 = testParameters.NtpIpv4;
                        devEnv.EnvironmentSettings.NtpIpv6 = testParameters.NtpIpv6;

                        devEnv.TestSettings.EventTopic = testParameters.EventTopic;
                        devEnv.TestSettings.MetadataFilter = testParameters.MetadataFilter;
                        devEnv.TestSettings.OperationDelay = testParameters.OperationDelay;
                        devEnv.TestSettings.Password1 = testParameters.Password1;
                        devEnv.TestSettings.Password2 = testParameters.Password2;
                        devEnv.TestSettings.PTZNodeToken = testParameters.PTZNodeToken;
                        devEnv.TestSettings.RecordingToken = testParameters.RecordingToken;
                        devEnv.TestSettings.RecoveryDelay = testParameters.TimeBetweenRequests;
                        devEnv.TestSettings.RelayOutputDelayTimeMonostable = testParameters.RelayOutputDelayTime;
                        devEnv.TestSettings.SearchTimeout = testParameters.SearchTimeout;
                        devEnv.TestSettings.SecureMethod = testParameters.SecureMethod;
                        devEnv.TestSettings.SubscriptionTimeout = testParameters.SubscriptionTimeout;
                        devEnv.TestSettings.TopicNamespaces = testParameters.TopicNamespaces;
                        devEnv.TestSettings.UseEmbeddedPassword = testParameters.UseEmbeddedPassword;
                        devEnv.TestSettings.VideoSourceToken = testParameters.VideoSourceToken;

                        devEnv.Timeouts.InterTests = testParameters.TimeBetweenTests;
                        devEnv.Timeouts.Message = testParameters.MessageTimeout;
                        devEnv.Timeouts.Reboot = testParameters.RebootTimeout;
                    }
                    
                    UpdateView();                    
                }
                

                if (SettingsLoaded != null)
                {
                    SettingsLoaded(this, new EventArgs());
                }

            }
            catch (Exception exc)
            {
                View.ShowError(exc);
            
           }
        
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////
        //!  @author        Ivan Vagunin
        ////
        public void OnPostLoadContextData()
        {
            Timeouts defaultTimeouts = new Timeouts();
            if (View.SettingsView.MessageTimeout == 0)
            {
                View.SettingsView.MessageTimeout = defaultTimeouts.Message;
            }
            if (View.SettingsView.RebootTimeout == 0)
            {
                View.SettingsView.RebootTimeout = defaultTimeouts.Reboot;
            }
            if (View.SettingsView.TimeBetweenTests == 0)
            {
                View.SettingsView.TimeBetweenTests = defaultTimeouts.InterTests;
            }
            if (string.IsNullOrEmpty(View.SettingsView.DnsIpv4))
            {
                View.SettingsView.DnsIpv4 = "10.1.1.1";
            }
            if (string.IsNullOrEmpty(View.SettingsView.NtpIpv4))
            {
                View.SettingsView.NtpIpv4 = "10.1.1.1";
            }
            if (string.IsNullOrEmpty(View.SettingsView.GatewayIpv4))
            {
                View.SettingsView.GatewayIpv4 = "10.1.1.1";
            }
            if (string.IsNullOrEmpty(View.SettingsView.DnsIpv6))
            {
                View.SettingsView.DnsIpv6 = "2001:1:1:1:1:1:1:1";
            }
            if (string.IsNullOrEmpty(View.SettingsView.NtpIpv6))
            {
                View.SettingsView.NtpIpv6 = "2001:1:1:1:1:1:1:1";
            }
            if (string.IsNullOrEmpty(View.SettingsView.GatewayIpv6))
            {
                View.SettingsView.GatewayIpv6 = "2001:1:1:1:1:1:1:1";
            }

            if (View.SettingsView.SubscriptionTimeout == 0)
            {
                View.SettingsView.SubscriptionTimeout = 60;
            }
            if (View.SettingsView.RelayOutputDelayTimeMonostable == 0)
            {
                View.SettingsView.RelayOutputDelayTimeMonostable = 20;
            }
            if (View.SettingsView.SearchTimeout == 0)
            {
                View.SettingsView.SearchTimeout = 10;
            }


            //call UpdateContext in this function, not in LoadSavedContext 
            UpdateContext();
        }

        ///////////////////////////////////////////////////////////////////////////
        //!  @author        Ivan Vagunin
        ////
        public override void LoadSavedContext(SavedContext context)
        {
            if(context.DeviceEnvironment != null)
            {
                if(context.DeviceEnvironment.Timeouts != null)
                {
                    View.SettingsView.MessageTimeout = context.DeviceEnvironment.Timeouts.Message;
                    View.SettingsView.RebootTimeout = context.DeviceEnvironment.Timeouts.Reboot;
                    //[24.04.2013] AKS: Don't recover value of TimeBetweenTests, ticket #88
                    //View.SettingsView.TimeBetweenTests = context.DeviceEnvironment.Timeouts.InterTests;
                    View.SettingsView.TimeBetweenTests = 0;
                }
                if (context.DeviceEnvironment.EnvironmentSettings != null)
                {
                    View.SettingsView.DnsIpv4 = context.DeviceEnvironment.EnvironmentSettings.DnsIpv4;
                    View.SettingsView.DnsIpv6 = context.DeviceEnvironment.EnvironmentSettings.DnsIpv6;
                    View.SettingsView.NtpIpv4 = context.DeviceEnvironment.EnvironmentSettings.NtpIpv4;
                    View.SettingsView.NtpIpv6 = context.DeviceEnvironment.EnvironmentSettings.NtpIpv6;
                    View.SettingsView.GatewayIpv4 = context.DeviceEnvironment.EnvironmentSettings.GatewayIpv4;
                    View.SettingsView.GatewayIpv6 = context.DeviceEnvironment.EnvironmentSettings.GatewayIpv6;
                }
                if(context.DeviceEnvironment.TestSettings != null)
                {
                    View.SettingsView.PTZNodeToken = context.DeviceEnvironment.TestSettings.PTZNodeToken;
                    View.SettingsView.VideoSourceToken = context.DeviceEnvironment.TestSettings.VideoSourceToken;

                    View.SettingsView.UseEmbeddedPassword = context.DeviceEnvironment.TestSettings.UseEmbeddedPassword;
                    View.SettingsView.Password1 = context.DeviceEnvironment.TestSettings.Password1;
                    View.SettingsView.Password2 = context.DeviceEnvironment.TestSettings.Password2;
                    View.SettingsView.SecureMethod = context.DeviceEnvironment.TestSettings.SecureMethod;

                    View.SettingsView.OperationDelay = context.DeviceEnvironment.TestSettings.OperationDelay;
                    //[24.04.2013] AKS: Don't recover value of RecoveryDelay, ticket #88
                    //View.SettingsView.RecoveryDelay = context.DeviceEnvironment.TestSettings.RecoveryDelay;
                    View.SettingsView.RecoveryDelay = 0;

                    View.SettingsView.SubscriptionTimeout = context.DeviceEnvironment.TestSettings.SubscriptionTimeout;
                    View.SettingsView.EventTopic = context.DeviceEnvironment.TestSettings.EventTopic;

                    string loadedNs = context.DeviceEnvironment.TestSettings.TopicNamespaces.Replace("\n",
                                                                                                     Environment.NewLine);
                    context.DeviceEnvironment.TestSettings.TopicNamespaces = loadedNs;
                    View.SettingsView.TopicNamespaces = context.DeviceEnvironment.TestSettings.TopicNamespaces;

                    View.SettingsView.RecordingToken = context.DeviceEnvironment.TestSettings.RecordingToken;
                    View.SettingsView.SearchTimeout = context.DeviceEnvironment.TestSettings.SearchTimeout;
                    View.SettingsView.MetadataFilter = context.DeviceEnvironment.TestSettings.MetadataFilter;
                    if (string.IsNullOrEmpty(context.DeviceEnvironment.TestSettings.RetentionTime))
                    {
                        View.SettingsView.RetentionTime = "P1D";
                    }
                    else
                    { 
                        View.SettingsView.RetentionTime = context.DeviceEnvironment.TestSettings.RetentionTime;
                    }


                    if (context.DeviceEnvironment.TestSettings.RelayOutputDelayTimeMonostable > 0)
                    {
                        View.SettingsView.RelayOutputDelayTimeMonostable =
                            context.DeviceEnvironment.TestSettings.RelayOutputDelayTimeMonostable;
                    }

                    DisplayAdvancedSettings(context.DeviceEnvironment.TestSettings.RawAdvancedSettings);

                    DeviceEnvironment env = ContextController.GetDeviceEnvironment();
                    env.TestSettings.AdvancedSettings =
                        ParseAdvancedSettings(context.DeviceEnvironment.TestSettings.RawAdvancedSettings);
                
                }
            }
        }

        #region Comboboxes for management

        /// <summary>
        /// Queries PTZ nodes
        /// </summary>
        public void GetPTZNodes()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            string address = devices != null ? devices.ServiceAddress : string.Empty;
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            ManagementServiceProvider deviceClient = new ManagementServiceProvider(address, env.Timeouts.Message);

            ReportOperationStarted();
            Thread thread = new Thread( new ThreadStart(new Action(() => 
            {
                try
                {
                    DeviceServicesInfo info = deviceClient.GetCapabilitiesDefineSecurity(new Onvif.CapabilityCategory[] { Onvif.CapabilityCategory.PTZ });
                    Onvif.Capabilities capabilities = info.Capabilities;

                    string ptzAddress = string.Empty;

                    if (capabilities != null)
                    {
                        if (capabilities.PTZ != null)
                        {
                            ptzAddress = capabilities.PTZ.XAddr;
                        }
                        else
                        {
                            throw new Exception("Device does not support PTZ service");
                        }
                    }
                    else
                    {
                        if (info.Services != null)
                        {
                            Onvif.Service ptzService = Tests.Common.CommonUtils.Extensions.FindService(
                                info.Services, OnvifService.PTZ);
                            if (ptzService != null)
                            {
                                ptzAddress = ptzService.XAddr;
                            }
                            else
                            {
                                throw new Exception("Device does not support PTZ service");
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to get service address");
                        }
                    }

                    PTZServiceProvider ptzClient = new PTZServiceProvider(ptzAddress, env.Timeouts.Message);

                    ptzClient.Security = deviceClient.Security;
                    Onvif.PTZNode[] nodes = ptzClient.GetNodes();
                    if ((nodes != null) && (nodes.Length > 0))
                    {
                        View.SettingsView.SetPTZNodes(nodes);
                    }
                    else
                    {
                        throw new Exception("No PTZ nodes returned by device");
                    }
                }
                catch (System.Exception ex)
                {
                    View.ShowError(ex);
                }
                finally
                {
                    ReportOperationCompleted();
                }
            })));
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();

        }
        
        private List<string> _operations;
        
        /// <summary>
        /// Operations which can be used for security tests.
        /// </summary>
        /// <returns></returns>
        public List<string> SecureOperations()
        {
            if (_operations == null)
            {
                _operations = new List<string>();

                _operations.Add("GetDeviceInformation");
                _operations.Add("GetScopes");
                _operations.Add("GetDiscoveryMode");
                _operations.Add("GetUsers");
                _operations.Add("GetDNS");
                _operations.Add("GetNTP");
                _operations.Add("GetNetworkInterfaces");
                _operations.Add("GetNetworkProtocols");
                _operations.Add("GetNetworkDefaultGateway");
                _operations.Add("GetZeroConfiguration");

            }
            return _operations;
        }

        /// <summary>
        /// Queries and displays event topics
        /// </summary>
        public void QueryEventTopics()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            string address = devices != null ? devices.ServiceAddress : string.Empty;
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            ManagementServiceProvider deviceClient = new ManagementServiceProvider(address, env.Timeouts.Message);

            ReportOperationStarted();
            Thread thread = new Thread(new ThreadStart(new Action(() =>
            {
                try
                {
                    DeviceServicesInfo info = deviceClient.GetCapabilitiesDefineSecurity(new Onvif.CapabilityCategory[] { Onvif.CapabilityCategory.Events });
                    Onvif.Capabilities capabilities = info.Capabilities;

                    string eventAddress = string.Empty;

                    if (capabilities != null)
                    {
                        if (capabilities.Events != null)
                        {
                            eventAddress = capabilities.Events.XAddr;
                        }
                        else
                        {
                            throw new Exception("Device does not support Events service");
                        }
                    }
                    else
                    {
                        if (info.Services != null)
                        {
                            Onvif.Service eventsService = Tests.Common.CommonUtils.Extensions.FindService(
                                info.Services, OnvifService.EVENTS);
                            if (eventsService != null)
                            {
                                eventAddress = eventsService.XAddr;
                            }
                            else
                            {
                                throw new Exception("Device does not support Events service");
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to get service address");
                        }
                    }

                    EventsServiceProvider client = new EventsServiceProvider(eventAddress, env.Timeouts.Message);
                    client.Security = deviceClient.Security;
                    List<EventsTopicInfo> infos = client.GetTopics();
                    if ((infos != null) && (infos.Count > 0))
                    {
                        View.SettingsView.SetEventsTopic(infos);
                    }
                    else
                    {
                        throw new Exception("No Event Topics returned by device");
                    }
                }
                catch (System.Exception ex)
                {
                    View.ShowError(ex);
                }
                finally
                {
                    ReportOperationCompleted();
                }
            })));
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();            
        }

        /// <summary>
        /// Queries and displays video sources
        /// </summary>
        public void QueryVideoSources()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            string address = devices != null ? devices.ServiceAddress : string.Empty;
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            ManagementServiceProvider deviceClient = new ManagementServiceProvider(address, env.Timeouts.Message);

            ReportOperationStarted();
            Thread thread = new Thread(new ThreadStart(new Action(() =>
            {
                try
                {
                    DeviceServicesInfo info = deviceClient.GetCapabilitiesDefineSecurity(new Onvif.CapabilityCategory[] { Onvif.CapabilityCategory.Media });
                    Onvif.Capabilities capabilities = info.Capabilities;

                    string mediaAddress = string.Empty;

                    if (capabilities != null)
                    {
                        if (capabilities.Media != null)
                        {
                            mediaAddress = capabilities.Media.XAddr;
                        }
                        else
                        {
                            throw new Exception("Device does not support Media service");
                        }
                    }
                    else
                    {
                        if (info.Services != null)
                        {
                            Onvif.Service mediaService = Tests.Common.CommonUtils.Extensions.FindService(
                                info.Services, OnvifService.MEDIA);
                            if (mediaService != null)
                            {
                                mediaAddress = mediaService.XAddr;
                            }
                            else
                            {
                                throw new Exception("Device does not support Media service");
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to get service address");
                        }
                    }
                    
                    MediaServiceProvider client = new MediaServiceProvider(mediaAddress, env.Timeouts.Message);
                    client.Security = deviceClient.Security;
                    Onvif.VideoSource[] sources = client.GetVideoSources();
                    if ((sources != null) && (sources.Length > 0))
                    {
                        View.SettingsView.SetVideoSources(sources);
                    }
                    else
                    {
                        throw new Exception("No Video Sources returned by device");
                    }
                }
                catch (System.Exception ex)
                {
                    View.ShowError(ex);
                }
                finally
                {
                    ReportOperationCompleted();
                }
            })));
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();
        }


        public void QueryRecordings()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            string address = devices != null ? devices.ServiceAddress : string.Empty;
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            ManagementServiceProvider deviceClient = new ManagementServiceProvider(address, env.Timeouts.Message);

            ReportOperationStarted();
            Thread thread = new Thread(new ThreadStart(new Action(() =>
            {
                try
                {
                    DeviceServicesInfo info = deviceClient.GetCapabilitiesDefineSecurity(new Onvif.CapabilityCategory[] { Onvif.CapabilityCategory.All });
                    Onvif.Capabilities capabilities = info.Capabilities;

                    string recordingControlAddress = string.Empty;

                    if (capabilities != null)
                    {
                        if (capabilities.Extension != null)
                        {
                            if (capabilities.Extension.Recording != null)
                            {
                                recordingControlAddress = capabilities.Extension.Recording.XAddr;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(recordingControlAddress))
                    {
                        if (info.Services != null)
                        {
                            Onvif.Service service = Tests.Common.CommonUtils.Extensions.FindService(
                                info.Services, OnvifService.RECORIDING);
                            if (service != null)
                            {
                                recordingControlAddress = service.XAddr;
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to get service address");
                        }
                    }
                    if (string.IsNullOrEmpty(recordingControlAddress))
                    {
                        throw new Exception("Device does not support Recording service");
                    }

                    RecordingServiceProvider client =
                        new RecordingServiceProvider(recordingControlAddress, env.Timeouts.Message);
                    client.Security = deviceClient.Security;
                    List<Onvif.GetRecordingsResponseItem> recordings = client.GetRecordings();
                    List<string> tokens = recordings.Select(R => R.RecordingToken).ToList();
                    if ((tokens != null) && (tokens.Count > 0))
                    {
                        View.SettingsView.SetRecordings(tokens);
                    }
                    else
                    {
                        throw new Exception("No Recordings returned by device");
                    }
                }
                catch (System.Exception ex)
                {
                    View.ShowError(ex);
                }
                finally
                {
                    ReportOperationCompleted();
                }
            })));
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();
        }

        #endregion
    }
}
