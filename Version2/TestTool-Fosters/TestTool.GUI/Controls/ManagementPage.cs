///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Data;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.UI;

namespace TestTool.GUI.Controls
{
    partial class ManagementPage : Page, IManagementView
    {
        private ManagementController _controller;

        public ManagementPage()
        {
            InitializeComponent();
            _controller = new ManagementController(this);

            scSettings.GetPTZNodes += scSettings_GetPTZNodes;
            scSettings.GetEventTopics += scSettings_GetEventTopics;
            scSettings.SetSecureMethods(Controller.SecureOperations());
            scSettings.GetVideoSources += scSettings_GetVideoSources;
        }
        
        internal ManagementController Controller
        {
            get { return _controller; }
        }

        private Enums.ApplicationState _currentState; 

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Management page has no effect on Discovery, so here use logic different from state.IsActive</remarks>
        /// <param name="state"></param>
        public void SwitchToState(Enums.ApplicationState state)
        {
            _currentState = state;
            scSettings.SetCurrentState(state);

            switch (state)
            {
                case Enums.ApplicationState.CommandRunning:
                case Enums.ApplicationState.TestRunning:
                case Enums.ApplicationState.TestPaused:
                case Enums.ApplicationState.DiscoveryRunning:
                case Enums.ApplicationState.ConformanceTestRunning:
                    BeginInvoke(new Action(() => DisablePage()));
                    break;
                case Enums.ApplicationState.Idle:
                    BeginInvoke(new Action(() => EnablePage()));
                    break;
            }
        }

        /// <summary>
        /// Disables child controls.
        /// </summary>
        void DisablePage()
        {
            DisableControls(new Control[] {
                lbProfiles, 
                tbProfileName, 
                btnApplyProfile, 
                btnSaveCurrentOptions});

            scSettings.DisableControl();
        }

        /// <summary>
        /// Enables child controls.
        /// </summary>
        void EnablePage()
        {
            EnableControls(new Control[]
                               {
                                   lbProfiles,tbProfileName,btnSaveCurrentOptions
                               });
            scSettings.EnableControl();
            scSettings.EnablePasswords(_currentState);
            btnApplyProfile.Enabled = lbProfiles.SelectedItem != null;
        }

        #region Properties

        public int MessageTimeout
        {
            get
            {
                return scSettings.MessageTimeout;
            }
            set
            {
                scSettings.MessageTimeout = value;
            }
        }

        public int RebootTimeout
        {
            get
            {
                return scSettings.RebootTimeout;
            }
            set
            {
                scSettings.RebootTimeout = value;
            }
        }

        public int TimeBetweenTests
        {
            get
            {
                return scSettings.TimeBetweenTests;
            }
            set
            {
                scSettings.TimeBetweenTests = value;
            }
        }
        
        public string NtpIpv4
        {
            get
            {
                return scSettings.NtpIpv4;
            }
            set
            {
                scSettings.NtpIpv4 = value;
            }
        }

        public string DnsIpv4
        {
            get
            {
                return scSettings.DnsIpv4;
            }
            set
            {
                scSettings.DnsIpv4 = value;
            }
        }

        public string GatewayIpv4
        {
            get
            {
                return scSettings.GatewayIpv4;
            }
            set
            {
                scSettings.GatewayIpv4 = value;
            }
        }

        public string GatewayIpv6
        {
            get
            {
                return scSettings.GatewayIpv6;
            }
            set
            {
                scSettings.GatewayIpv6 = value;
            }
        }

        public bool UseEmbeddedPassword
        {
            get
            {
                return scSettings.UseEmbeddedPassword;
            }
            set
            {
                scSettings.UseEmbeddedPassword = value;
            }
        }

        public string Password1
        {
            get
            {
                return scSettings.Password1;
            }
            set
            {
                scSettings.Password1 = value;
            }
        }

        public string Password2
        {
            get
            {
                return scSettings.Password2;
            }
            set
            {
                scSettings.Password2 = value;
            }
        }

        public string PTZNodeToken
        {
            get
            {
                return scSettings.PTZNodeToken;
            }
            set
            {
                scSettings.PTZNodeToken = value;
            }
        }

        /// <summary>
        /// Video source token selected.
        /// </summary>
        public string VideoSourceToken
        {
            get
            {
                return scSettings.VideoSourceToken;
            }
            set
            {
                scSettings.VideoSourceToken = value;
            }
        }

        public int SubscriptionTimeout
        {
            get
            {
                return scSettings.SubscriptionTimeout;
            }
            set 
            { 
                scSettings.SubscriptionTimeout = value;
            }
        }

        public string EventTopic 
        {
            get
            {
                return scSettings.EventTopic;
            }
            set
            {
                scSettings.EventTopic = value;
            }
        }

        public int RelayOutputDelayTimeMonostable
        {
            get { return scSettings.RelayOutputDelayTimeMonostable; }
            set { scSettings.RelayOutputDelayTimeMonostable = value;  }
        }

        public string TopicNamespaces
        {
            get
            {
                return scSettings.TopicNamespaces;
            }
            set
            {
                scSettings.TopicNamespaces = value;
            }
        }

        public string NtpIpv6
        {
            get
            {
                return scSettings.NtpIpv6;
            }
            set
            {
                scSettings.NtpIpv6 = value;
            }
        }

        public string DnsIpv6
        {
            get
            {
                return scSettings.DnsIpv6;
            }
            set
            {
                scSettings.DnsIpv6 = value;
            }
        }

        public int OperationDelay
        {
            get
            {
                return scSettings.OperationDelay;
            }
            set
            {
                scSettings.OperationDelay = value;
            }

        }

        public int RecoveryDelay 
        {
            get { return scSettings.RecoveryDelay; }
            set { scSettings.RecoveryDelay = value; }
        }

        public string SecureMethod
        {
            get { return scSettings.SecureMethod; }
            set { scSettings.SecureMethod = value; }
        }



        private Profile _currentProfile;

        public Profile CurrentProfile
        {
            get
            {
                return _currentProfile;
            }
        }

        public List<object> AdvancedSettings
        {
            get { return scSettings.AdvancedSettings; }
            set { scSettings.AdvancedSettings = value; }
        }


        #endregion

        #region IView Members


        public IController GetController()
        {
            return _controller;
        }

        #endregion


        public void AddSettingsPages(List<SettingsTabPage> pages)
        {
            scSettings.AddSettingsPages(pages);  
        }

        #region Profiles

        public void DisplayProfiles(List<Profile> profiles)
        {
            InternalDisplayProfiles(profiles);
        }

        private void btnSaveCurrentOptions_Click(object sender, EventArgs e)
        {
            // ToDo: check that name is OK
            string profileName = tbProfileName.Text;
            if (string.IsNullOrEmpty(profileName))
            {
                MessageBox.Show("Profile name should not be empty!", "Profile name is incorrect");
                return;
            }

            //bool bReplace = false;

            //if (lbProfiles.SelectedItem != null)
            //{
            //    Profile currentProfile = (Profile) lbProfiles.SelectedItem;
            //    bReplace = (currentProfile.Name != profileName &&
            //                _controller.Profiles.Where(p => p.Name == profileName).FirstOrDefault() != null);
            //}
            //else
            //{
            //    bReplace = _controller.Profiles.Where(p => p.Name == profileName).FirstOrDefault() != null;
            //}

            //if (bReplace)
            //{
            //    DialogResult result = MessageBox.Show(string.Format("Profile {0} will be replaced. Continue?", profileName), "Warning!",
            //                                          MessageBoxButtons.OKCancel);
            //    if (result == DialogResult.Cancel)
            //    {
            //        return;
            //    }
            //}

            Profile newProfile = _controller.SaveCurrentProfile(profileName);
            lbProfiles.DataSource = null;
            lbProfiles.DisplayMember = "Name";
            lbProfiles.DataSource = _controller.Profiles;
            lbProfiles.SelectedItem = newProfile;
        }

        public void InternalDisplayProfiles(List<Profile> profiles)
        {
            lbProfiles.DisplayMember = "Name";
            lbProfiles.DataSource = profiles;
            lbProfiles.SelectedItem = null;
            tbProfileName.Text = string.Empty;
        }

        void DisplayCurrentProfile()
        {
            if (lbProfiles.SelectedItem != null)
            {
                Profile profile = (Profile)lbProfiles.SelectedItem;
                DisplayProfile(profile);
            }
        }

        void DisplayProfile(Profile profile)
        {
            tbProfileName.Text = profile.Name;
        }

        void ApplyProfile(Profile profile)
        {
            _currentProfile = profile;

            RebootTimeout = profile.Reboot;
            MessageTimeout = profile.Message;
            TimeBetweenTests = profile.InterTests;
            OperationDelay = profile.OperationDelay;
            RecoveryDelay = profile.RecoveryDelay;

            DnsIpv4 = profile.DnsIpv4;
            NtpIpv4 = profile.NtpIpv4;
            GatewayIpv4 = profile.GatewayIpv4;
            DnsIpv6 = profile.DnsIpv6;
            NtpIpv6 = profile.NtpIpv6;
            GatewayIpv6 = profile.GatewayIpv6;

            UseEmbeddedPassword = profile.UseEmbeddedPassword;
            Password1 = profile.Password1;
            Password2 = profile.Password2;
            SecureMethod = profile.SecureMethod;

            PTZNodeToken = profile.PTZNodeToken;
            VideoSourceToken = profile.VideoSourceToken;

            EventTopic = profile.EventTopic;
            TopicNamespaces = profile.TopicNamespaces;
            SubscriptionTimeout = profile.SubscriptionTimeout;

            RelayOutputDelayTimeMonostable = profile.RelayOutputDelayTime; 

            _controller.ApplyProfile(profile);
        }

        private void lbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayCurrentProfile();
            btnApplyProfile.Enabled = lbProfiles.SelectedItem != null;
        }

        private void btnApplyProfile_Click(object sender, EventArgs e)
        {
            if (lbProfiles.SelectedItem != null)
            {
                try
                {
                    Profile profile = (Profile)lbProfiles.SelectedItem;
                    ApplyProfile(profile);
                }
                catch (Exception exc)
                {
                    //
                    // Yes, we really suppress any errors.
                    //
                }

            }
        }

        private void tbProfileName_TextChanged(object sender, EventArgs e)
        {
            toolTip.SetToolTip(tbProfileName, tbProfileName.Text);
        }

        #endregion
        
        public void SetPTZNodes(Proxies.Onvif.PTZNode[] nodes)
        {
            BeginInvoke(new Action(() =>
            {
                scSettings.SetPTZNodes(nodes);
            }));
        }

        /// <summary>
        /// Sets video sources
        /// </summary>
        /// <param name="sources">Video sources information</param>
        public void SetVideoSources(Proxies.Onvif.VideoSource[] sources)
        {
            BeginInvoke(new Action(() =>
            {
                scSettings.SetVideoSources(sources);
            }));
        }

        /// <summary>
        /// Displays exception message to user
        /// </summary>
        /// <param name="e">Exception to be displayed</param>
        public void ShowError(Exception e)
        {
            string message = e.Message.Length > 400 ? e.Message.Substring(0, 400) : e.Message;
            BeginInvoke(new Action(() =>
            {
                MessageBox.Show(this, "Unexpected error occurred: " + message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        void scSettings_GetPTZNodes(object sender, EventArgs e)
        {
            try
            {
                _controller.GetPTZNodes();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void scSettings_GetEventTopics(object sender, EventArgs e)
        {
            try
            {
                _controller.QueryEventTopics();
            }
            catch(Exception exc)
            {
                MessageBox.Show(this, exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void scSettings_GetVideoSources(object sender, EventArgs e)
        {
            try
            {
                _controller.QueryVideoSources();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void SetEventsTopic(List<EventsTopicInfo> topics)
        {
            scSettings.SetEventsTopic(topics); 
        }

    }
}
