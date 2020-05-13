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
            scSettings.GetRecordings += new EventHandler(scSettings_GetRecordings);

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
        public override void SwitchToState(Enums.ApplicationState state)
        {
            _currentState = state;
            scSettings.SwitchToState(state);

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

        private Profile _currentProfile;

        public Profile CurrentProfile
        {
            get
            {
                return _currentProfile;
            }
        }
        
        #endregion

        #region IView Members


        public override IController GetController()
        {
            return _controller;
        }

        #endregion

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

            scSettings.RebootTimeout = profile.Reboot;
            scSettings.MessageTimeout = profile.Message;
            scSettings.TimeBetweenTests = profile.InterTests;
            scSettings.OperationDelay = profile.OperationDelay;
            scSettings.RecoveryDelay = profile.RecoveryDelay;

            scSettings.DnsIpv4 = profile.DnsIpv4;
            scSettings.NtpIpv4 = profile.NtpIpv4;
            scSettings.GatewayIpv4 = profile.GatewayIpv4;
            scSettings.DnsIpv6 = profile.DnsIpv6;
            scSettings.NtpIpv6 = profile.NtpIpv6;
            scSettings.GatewayIpv6 = profile.GatewayIpv6;

            scSettings.UseEmbeddedPassword = profile.UseEmbeddedPassword;
            scSettings.Password1 = profile.Password1;
            scSettings.Password2 = profile.Password2;
            scSettings.SecureMethod = profile.SecureMethod;

            scSettings.PTZNodeToken = profile.PTZNodeToken;
            scSettings.VideoSourceToken = profile.VideoSourceToken;

            scSettings.EventTopic = profile.EventTopic;
            scSettings.TopicNamespaces = profile.TopicNamespaces;
            scSettings.SubscriptionTimeout = profile.SubscriptionTimeout;

            scSettings.RelayOutputDelayTimeMonostable = profile.RelayOutputDelayTime; 

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
         
        void scSettings_GetPTZNodes(object sender, EventArgs e)
        {
            try
            {
                _controller.GetPTZNodes();
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
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
                ShowError(exc.Message);
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
                ShowError(ex.Message);
            }
        }


        public void SetEventsTopic(List<EventsTopicInfo> topics)
        {
            scSettings.SetEventsTopic(topics); 
        }


        void scSettings_GetRecordings(object sender, EventArgs e)
        {
            try
            {
                _controller.QueryRecordings();
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
            }
        }
        
        #region IManagementView Members
        
        public ITestSettingsView SettingsView
        {
            get { return scSettings; }
        }

        #endregion

        private void btnSaveAsXml_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.DefaultExt = ".xml";
            sfd.Filter = "XML file | *.xml | All Files | *.*";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _controller.UpdateContext();;
                _controller.SaveAsXML(sfd.FileName);
            }            
            
        }

        public bool OpenFileForEditing 
        {
            get { return chkOpenInNotepad.Checked; }
            set { chkOpenInNotepad.Checked = value; } 
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();

            sfd.DefaultExt = ".xml";
            sfd.Filter = "XML file | *.xml | All Files | *.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _controller.LoadSettingsFromXML(sfd.FileName);
            }
        }

    }
}
