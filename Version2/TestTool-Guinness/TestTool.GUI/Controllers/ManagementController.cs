///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Definitions.UI;
using Onvif = TestTool.Proxies.Onvif;
using System.Threading;

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
            environment.TestSettings.RecordingToken = View.SettingsView.RecordingToken;

            environment.TestSettings.SecureMethod = View.SettingsView.SecureMethod;
            environment.TestSettings.SubscriptionTimeout = View.SettingsView.SubscriptionTimeout;
            environment.TestSettings.EventTopic = View.SettingsView.EventTopic;
            environment.TestSettings.TopicNamespaces = View.SettingsView.TopicNamespaces;

            environment.TestSettings.RelayOutputDelayTimeMonostable = View.SettingsView.RelayOutputDelayTimeMonostable;
            environment.TestSettings.SearchTimeout = View.SettingsView.SearchTimeout;

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
            View.SettingsView.DnsIpv4 = environment.EnvironmentSettings.DnsIpv4;
            View.SettingsView.DnsIpv6 = environment.EnvironmentSettings.DnsIpv6;
            View.SettingsView.NtpIpv4 = environment.EnvironmentSettings.NtpIpv4;
            View.SettingsView.NtpIpv6 = environment.EnvironmentSettings.NtpIpv6;
            View.SettingsView.GatewayIpv4 = environment.EnvironmentSettings.GatewayIpv4;
            View.SettingsView.GatewayIpv6 = environment.EnvironmentSettings.GatewayIpv6;
            View.SettingsView.PTZNodeToken = environment.TestSettings.PTZNodeToken;

            View.SettingsView.UseEmbeddedPassword = environment.TestSettings.UseEmbeddedPassword;
            View.SettingsView.Password1 = environment.TestSettings.Password1;
            View.SettingsView.Password2 = environment.TestSettings.Password2;

            View.SettingsView.OperationDelay = environment.TestSettings.OperationDelay;
            View.SettingsView.RecoveryDelay = environment.TestSettings.RecoveryDelay;

            View.SettingsView.SubscriptionTimeout = environment.TestSettings.SubscriptionTimeout;
            View.SettingsView.EventTopic = environment.TestSettings.EventTopic;
            View.SettingsView.TopicNamespaces = environment.TestSettings.TopicNamespaces;
            View.SettingsView.SecureMethod = environment.TestSettings.SecureMethod;

            View.SettingsView.RelayOutputDelayTimeMonostable = environment.TestSettings.RelayOutputDelayTimeMonostable;

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

        private Dictionary<string, Type> _advancedSettingsTypes;

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

            /*******************************/
            profile.AdvancedSettings = GetAdvancedSettings();

            /********************************/

            ProfilesManager.Save(_profiles);
            return profile;

        }

        List<XmlElement> GetAdvancedSettings()
        {
            MemoryStream memStream = new MemoryStream();
            XmlFragmentWriter xmlWriter = new XmlFragmentWriter(memStream, new UTF8Encoding(false));
            {
                List<object> settings = View.SettingsView.AdvancedSettings;
                int notNull = 0;
                foreach (object parameters in settings)
                {
                    if (parameters != null)
                    {
                        notNull++;
                        System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(parameters.GetType());
                        serializer.Serialize(xmlWriter, parameters);
                    }
                }
                if (notNull > 0)
                {
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.Close();

            string str = Encoding.UTF8.GetString(memStream.GetBuffer());

            XmlDocument doc = new XmlDocument();
            if (!string.IsNullOrEmpty(str))
            {
                doc.LoadXml(str);
            }

            /*********************************************/
            List<XmlElement> elements = new List<XmlElement>();
            if (doc.DocumentElement != null)
            {
                foreach (XmlElement element in doc.DocumentElement.ChildNodes)
                {
                    elements.Add(element);
                }
            }

            return elements;
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

        List<object> ParseAdvancedSettings(List<XmlElement> elements)
        {
            List<object> advancedSettings = new List<object>();
            foreach (XmlElement element in elements)
            {
                string name = element.LocalName;

                if (_advancedSettingsTypes.ContainsKey(name))
                {
                    Type t = _advancedSettingsTypes[name];

                    XmlNodeReader rdr = new XmlNodeReader(element);

                    System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(t);
                    object parameters = serializer.Deserialize(rdr);
                    advancedSettings.Add(parameters);
                }
            }
            return advancedSettings;
        }

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
            if (context.DeviceEnvironment != null)
            {
                if (context.DeviceEnvironment.Timeouts != null)
                {
                    View.SettingsView.MessageTimeout = context.DeviceEnvironment.Timeouts.Message;
                    View.SettingsView.RebootTimeout = context.DeviceEnvironment.Timeouts.Reboot;
                    View.SettingsView.TimeBetweenTests = context.DeviceEnvironment.Timeouts.InterTests;
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
                if (context.DeviceEnvironment.TestSettings != null)
                {
                    View.SettingsView.PTZNodeToken = context.DeviceEnvironment.TestSettings.PTZNodeToken;
                    View.SettingsView.VideoSourceToken = context.DeviceEnvironment.TestSettings.VideoSourceToken;

                    View.SettingsView.UseEmbeddedPassword = context.DeviceEnvironment.TestSettings.UseEmbeddedPassword;
                    View.SettingsView.Password1 = context.DeviceEnvironment.TestSettings.Password1;
                    View.SettingsView.Password2 = context.DeviceEnvironment.TestSettings.Password2;
                    View.SettingsView.SecureMethod = context.DeviceEnvironment.TestSettings.SecureMethod;

                    View.SettingsView.OperationDelay = context.DeviceEnvironment.TestSettings.OperationDelay;
                    View.SettingsView.RecoveryDelay = context.DeviceEnvironment.TestSettings.RecoveryDelay;

                    View.SettingsView.SubscriptionTimeout = context.DeviceEnvironment.TestSettings.SubscriptionTimeout;
                    View.SettingsView.EventTopic = context.DeviceEnvironment.TestSettings.EventTopic;

                    View.SettingsView.SearchTimeout = context.DeviceEnvironment.TestSettings.SearchTimeout;
                    View.SettingsView.RecordingToken = context.DeviceEnvironment.TestSettings.RecordingToken;

                    string loadedNs = context.DeviceEnvironment.TestSettings.TopicNamespaces.Replace("\n",
                                                                                                     Environment.NewLine);
                    context.DeviceEnvironment.TestSettings.TopicNamespaces = loadedNs;
                    View.SettingsView.TopicNamespaces = context.DeviceEnvironment.TestSettings.TopicNamespaces;

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
            Thread thread = new Thread(new ThreadStart(new Action(() =>
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
        public List<string> SecureOperations()
        {
            if (_operations == null)
            {
                _operations = new List<string>();

                //_operations.Add("CreateUsers");
                _operations.Add("GetDeviceInformation");
                //_operations.Add("GetSystemDateAndTime");
                _operations.Add("GetScopes");
                _operations.Add("GetDiscoveryMode");
                _operations.Add("GetUsers");
                //_operations.Add("GetWsdlUrl");
                //_operations.Add("GetCapabilities");
                //_operations.Add("GetHostname");
                _operations.Add("GetDNS");
                _operations.Add("GetNTP");
                _operations.Add("GetNetworkInterfaces");
                _operations.Add("GetNetworkProtocols");
                _operations.Add("GetNetworkDefaultGateway");
                _operations.Add("GetZeroConfiguration");

            }
            return _operations;
        }

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

    }
}
