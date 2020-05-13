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
            environment.Timeouts.InterTests = View.TimeBetweenTests;
            environment.Timeouts.Message = View.MessageTimeout;
            environment.Timeouts.Reboot = View.RebootTimeout;

            environment.EnvironmentSettings = new EnvironmentSettings();
            environment.EnvironmentSettings.DnsIpv4 = View.DnsIpv4;
            environment.EnvironmentSettings.NtpIpv4 = View.NtpIpv4;
            environment.EnvironmentSettings.DnsIpv6 = View.DnsIpv6;
            environment.EnvironmentSettings.NtpIpv6 = View.NtpIpv6;
            environment.EnvironmentSettings.GatewayIpv4 = View.GatewayIpv4;
            environment.EnvironmentSettings.GatewayIpv6 = View.GatewayIpv6;

            environment.TestSettings = new TestSettings();
            environment.TestSettings.PTZNodeToken = View.PTZNodeToken;
            
            environment.TestSettings.UseEmbeddedPassword = View.UseEmbeddedPassword;
            environment.TestSettings.Password1 = View.Password1;
            environment.TestSettings.Password2 = View.Password2;
            environment.TestSettings.OperationDelay = View.OperationDelay;
            environment.TestSettings.RecoveryDelay = View.RecoveryDelay;
            environment.TestSettings.VideoSourceToken = View.VideoSourceToken;

            environment.TestSettings.SecureMethod = View.SecureMethod;
            environment.TestSettings.SubscriptionTimeout = View.SubscriptionTimeout;
            environment.TestSettings.EventTopic = View.EventTopic;
            environment.TestSettings.TopicNamespaces = View.TopicNamespaces;

            environment.TestSettings.RelayOutputDelayTimeMonostable = View.RelayOutputDelayTimeMonostable;

            environment.TestSettings.RawAdvancedSettings = GetAdvancedSettings();

            List<object> advanced = View.AdvancedSettings;
            environment.TestSettings.AdvancedSettings = advanced;

        }

        /// <summary>
        /// Updates view.
        /// </summary>
        public override void UpdateView()
        {
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
            View.TimeBetweenTests = environment.Timeouts.InterTests;
            View.MessageTimeout = environment.Timeouts.Message;
            View.RebootTimeout = environment.Timeouts.Reboot;
            View.DnsIpv4 = environment.EnvironmentSettings.DnsIpv4;
            View.DnsIpv6 = environment.EnvironmentSettings.DnsIpv6;
            View.NtpIpv4 = environment.EnvironmentSettings.NtpIpv4;
            View.NtpIpv6 = environment.EnvironmentSettings.NtpIpv6;
            View.GatewayIpv4 = environment.EnvironmentSettings.GatewayIpv4;
            View.GatewayIpv6 = environment.EnvironmentSettings.GatewayIpv6;
            View.PTZNodeToken = environment.TestSettings.PTZNodeToken;

            View.UseEmbeddedPassword = environment.TestSettings.UseEmbeddedPassword;
            View.Password1 = environment.TestSettings.Password1;
            View.Password2 = environment.TestSettings.Password2;
            
            View.OperationDelay = environment.TestSettings.OperationDelay;
            View.RecoveryDelay = environment.TestSettings.RecoveryDelay;
            
            View.SubscriptionTimeout = environment.TestSettings.SubscriptionTimeout;
            View.EventTopic = environment.TestSettings.EventTopic;
            View.TopicNamespaces = environment.TestSettings.TopicNamespaces;
            View.SecureMethod = environment.TestSettings.SecureMethod;

            View.RelayOutputDelayTimeMonostable = environment.TestSettings.RelayOutputDelayTimeMonostable;

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

            View.AddSettingsPages(pages);
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

            profile.InterTests = View.TimeBetweenTests;
            profile.Reboot = View.RebootTimeout;
            profile.Message = View.MessageTimeout;

            profile.OperationDelay = View.OperationDelay;
            profile.RecoveryDelay = View.RecoveryDelay;

            profile.DnsIpv4 = View.DnsIpv4;
            profile.NtpIpv4 = View.NtpIpv4;
            profile.GatewayIpv4 = View.GatewayIpv4;

            profile.DnsIpv6 = View.DnsIpv6;
            profile.NtpIpv6 = View.NtpIpv6;
            profile.GatewayIpv6 = View.GatewayIpv6;

            profile.UseEmbeddedPassword = View.UseEmbeddedPassword;
            profile.Password1 = View.Password1;
            profile.Password2 = View.Password2;
            profile.SecureMethod = View.SecureMethod;

            profile.PTZNodeToken = View.PTZNodeToken;
            profile.VideoSourceToken = View.VideoSourceToken;

            profile.EventTopic = View.EventTopic;
            profile.TopicNamespaces = View.TopicNamespaces;
            profile.SubscriptionTimeout = View.SubscriptionTimeout;
            profile.RelayOutputDelayTime = View.RelayOutputDelayTimeMonostable;

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
                List<object> settings = View.AdvancedSettings;
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
                View.AdvancedSettings = null;
                return;
            }


            List<object> advancedSettings = ParseAdvancedSettings(elements);
            View.AdvancedSettings = advancedSettings;
        }

        #endregion
        
        ///////////////////////////////////////////////////////////////////////////
        //!  @author        Ivan Vagunin
        ////
        public void OnPostLoadContextData()
        {
            Timeouts defaultTimeouts = new Timeouts();
            if (View.MessageTimeout == 0)
            {
                View.MessageTimeout = defaultTimeouts.Message;
            }
            if (View.RebootTimeout == 0)
            {
                View.RebootTimeout = defaultTimeouts.Reboot;
            }
            if (View.TimeBetweenTests == 0)
            {
                View.TimeBetweenTests = defaultTimeouts.InterTests;
            }
            if (string.IsNullOrEmpty(View.DnsIpv4))
            {
                View.DnsIpv4 = "10.1.1.1";
            }
            if (string.IsNullOrEmpty(View.NtpIpv4))
            {
                View.NtpIpv4 = "10.1.1.1";
            }
            if(string.IsNullOrEmpty(View.GatewayIpv4))
            {
                View.GatewayIpv4 = "10.1.1.1";
            }
            if(string.IsNullOrEmpty(View.DnsIpv6))
            {
                View.DnsIpv6 = "2001:1:1:1:1:1:1:1";
            }
            if (string.IsNullOrEmpty(View.NtpIpv6))
            {
                View.NtpIpv6 = "2001:1:1:1:1:1:1:1";
            }
            if (string.IsNullOrEmpty(View.GatewayIpv6))
            {
                View.GatewayIpv6 = "2001:1:1:1:1:1:1:1";
            }

            if (View.SubscriptionTimeout == 0)
            {
                View.SubscriptionTimeout = 60;
            }
            if (View.RelayOutputDelayTimeMonostable == 0)
            {
                View.RelayOutputDelayTimeMonostable = 20;
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
                    View.MessageTimeout = context.DeviceEnvironment.Timeouts.Message;
                    View.RebootTimeout = context.DeviceEnvironment.Timeouts.Reboot;
                    View.TimeBetweenTests = context.DeviceEnvironment.Timeouts.InterTests;
                }
                if (context.DeviceEnvironment.EnvironmentSettings != null)
                {
                    View.DnsIpv4 = context.DeviceEnvironment.EnvironmentSettings.DnsIpv4;
                    View.DnsIpv6 = context.DeviceEnvironment.EnvironmentSettings.DnsIpv6;
                    View.NtpIpv4 = context.DeviceEnvironment.EnvironmentSettings.NtpIpv4;
                    View.NtpIpv6 = context.DeviceEnvironment.EnvironmentSettings.NtpIpv6;
                    View.GatewayIpv4 = context.DeviceEnvironment.EnvironmentSettings.GatewayIpv4;
                    View.GatewayIpv6 = context.DeviceEnvironment.EnvironmentSettings.GatewayIpv6;
                }
                if(context.DeviceEnvironment.TestSettings != null)
                {
                    View.PTZNodeToken = context.DeviceEnvironment.TestSettings.PTZNodeToken;
                    View.VideoSourceToken = context.DeviceEnvironment.TestSettings.VideoSourceToken;

                    View.UseEmbeddedPassword = context.DeviceEnvironment.TestSettings.UseEmbeddedPassword;
                    View.Password1 = context.DeviceEnvironment.TestSettings.Password1;
                    View.Password2 = context.DeviceEnvironment.TestSettings.Password2;
                    View.SecureMethod = context.DeviceEnvironment.TestSettings.SecureMethod;

                    View.OperationDelay = context.DeviceEnvironment.TestSettings.OperationDelay;
                    View.RecoveryDelay = context.DeviceEnvironment.TestSettings.RecoveryDelay;

                    View.SubscriptionTimeout = context.DeviceEnvironment.TestSettings.SubscriptionTimeout;
                    View.EventTopic = context.DeviceEnvironment.TestSettings.EventTopic;

                    string loadedNs = context.DeviceEnvironment.TestSettings.TopicNamespaces.Replace("\n",
                                                                                                     Environment.NewLine);
                    context.DeviceEnvironment.TestSettings.TopicNamespaces = loadedNs;
                    View.TopicNamespaces = context.DeviceEnvironment.TestSettings.TopicNamespaces;

                    if (context.DeviceEnvironment.TestSettings.RelayOutputDelayTimeMonostable > 0)
                    {
                        View.RelayOutputDelayTimeMonostable =
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
                        View.SetPTZNodes(nodes);
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
                        View.SetEventsTopic(infos);
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
                        View.SetVideoSources(sources);
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


    }
}
