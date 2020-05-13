///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.Tests.Common.Enums;
using TestTool.GUI.Utils;
using Device = TestTool.Proxies.Device;
using PTZ = TestTool.Proxies.PTZ;
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

            environment.Features = new List<Feature>(View.Features);

            environment.DeviceTypes = View.DeviceTypes;

            environment.Services.Clear();
            environment.Services.AddRange(View.Services);

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

            ContextController.UpdateDeviceEnvironment(environment);
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
            
            SetupInfo info = ContextController.GetSetupInfo();
            if (!info.Specification20Selected())
            {
                View.DeviceTypes = DeviceType.NVT; 
            }
            View.EnableDeviceTypes(info.Specification20Selected());
        }

        /// <summary>
        /// Performs initialization.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            
            View.DisplayProfiles(Load());
        }

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
        /// <returns></returns>
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

            profile.DnsIpv4 = View.DnsIpv4;
            profile.NtpIpv4 = View.NtpIpv4;
            profile.GatewayIpv4 = View.GatewayIpv4;

            profile.DnsIpv6 = View.DnsIpv6;
            profile.NtpIpv6 = View.NtpIpv6;
            profile.GatewayIpv6 = View.GatewayIpv6;

            profile.UseEmbeddedPassword = View.UseEmbeddedPassword;
            profile.Password1 = View.Password1;
            profile.Password2 = View.Password2;

            profile.OperationDelay = View.OperationDelay;

            TestOptions options = ContextController.GetTestOptions();

            profile.TestCases = options.Tests;
            profile.InteractiveFirst = options.InteractiveFirst;
            profile.TestGroups = options.Groups;

            profile.Features = new List<Feature>(View.Features);
            profile.Services = new List<Service>(View.Services);
            profile.DeviceType = View.DeviceTypes;

            ProfilesManager.Save(_profiles);
            return profile;
            
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
            if (ProfileApplied != null)
            {
                ProfileApplied(profile);
            }
        }

        /// <summary>
        /// Is raised when a user switches between embedded and custom settings.
        /// </summary>
        public event Action<bool> OnCertificationMode;

        /// <summary>
        /// Switch application to certification/diagnostics mode.
        /// </summary>
        /// <param name="bOn">True, if certification mode is selected.</param>
        /// <remarks>The following page changes their behaviour in Certification mode:
        /// - Tests page (in certification mode mandatory tests cannot be unselected, 
        ///   some buttons are disabled)
        /// - Report page (report generation is available only in certification mode)
        /// - Setup page (Core specification cannot be changed during certification)
        /// </remarks>
        public void SetCertificationMode(bool bOn)
        {
            UpdateContext();
            if (OnCertificationMode != null)
            {
                OnCertificationMode(bOn);
            }
        }

        public event Action<List<Feature>> FeaturesApplied;

        /// <summary>
        /// Selects feature-dependent tests. 
        /// </summary>
        public void ApplyFeatures()
        {
            List<Feature> features = View.Features;
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
            environment.Features = new List<Feature>(features);
            
            environment.Services.Clear();
            environment.Services.AddRange(View.Services);

            if (FeaturesApplied!= null)
            {
                FeaturesApplied(features);
            }

        }

        ///////////////////////////////////////////////////////////////////////////
        //!  @author        Ivan Vagunin
        ////
        public void OnPostLoadContextData()
        {
            if (View.MessageTimeout == 0)
            {
                View.MessageTimeout = ProfilesSet.Default.Message;
            }
            if (View.RebootTimeout == 0)
            {
                View.RebootTimeout = ProfilesSet.Default.Reboot;
            }
            if (View.TimeBetweenTests == 0)
            {
                View.TimeBetweenTests = ProfilesSet.Default.InterTests;
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
            if (View.DeviceTypes == DeviceType.None)
            {
                // force TestTool to disable Conformance Mode if no settings were loaded.
                View.DeviceTypes = DeviceType.None;
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

                View.DeviceTypes = context.DeviceEnvironment.DeviceTypes;

                View.SelectFeatures(context.DeviceEnvironment.Services, context.DeviceEnvironment.Features);

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

                    View.UseEmbeddedPassword = context.DeviceEnvironment.TestSettings.UseEmbeddedPassword;
                    View.Password1 = context.DeviceEnvironment.TestSettings.Password1;
                    View.Password2 = context.DeviceEnvironment.TestSettings.Password2;
                    View.OperationDelay = context.DeviceEnvironment.TestSettings.OperationDelay;
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
                    Device.Capabilities capabilities = deviceClient.GetCapabilitiesSync(new Device.CapabilityCategory[] { Device.CapabilityCategory.PTZ });
                    if (capabilities.PTZ != null)
                    {
                        string ptzAddress = capabilities.PTZ.XAddr;
                        PTZServiceProvider ptzClient = new PTZServiceProvider(ptzAddress, env.Timeouts.Message);
                        PTZ.PTZNode[] nodes = ptzClient.GetNodes();
                        if ((nodes != null) && (nodes.Length > 0))
                        {
                            View.SetPTZNodes(nodes);
                        }
                        else
                        {
                            throw new Exception("No PTZ nodes returned by device");
                        }
                    }
                    else
                    {
                        throw new Exception("Device does not support PTZ service");
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
