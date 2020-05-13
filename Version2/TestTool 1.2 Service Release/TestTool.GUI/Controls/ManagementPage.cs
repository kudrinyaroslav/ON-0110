///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Data;
using TestTool.Tests.Common.Enums;

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
            scSettings.SelectTests += scSettings_SelectTests;

            EnableProfileControls();
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
                rbConformanceMode, 
                rbDiagnosticMode,
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
            EnableControls(new Control[] {
                rbConformanceMode,
                rbDiagnosticMode,
                lbProfiles});
            scSettings.EnableControl();
            scSettings.EnablePasswords(_currentState);

            EnableProfileControls();
        }

        /// <summary>
        /// Enables profile-dependent controls.
        /// </summary>
        void EnableProfileControls()
        {
            bool bCustom = !rbConformanceMode.Checked;
            lblProfilesList.Enabled = bCustom;
            lbProfiles.Enabled = bCustom;
            lblProfileName.Enabled = bCustom;
            tbProfileName.Enabled = bCustom;
            btnApplyProfile.Enabled = bCustom && lbProfiles.SelectedItem != null;
            btnSaveCurrentOptions.Enabled = bCustom;

            scSettings.EnableProfileControls(bCustom);
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
        
        public List<Feature> Features
        {
            get
            {
                return scSettings.Features;
            }
            set
            {
                SelectFeatures(value);
            }
        }

        public void SelectFeatures(List<Feature> features)
        {
            scSettings.SelectFeatures(features);
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
            get
            {
                return scSettings.RecoveryDelay;
            }
            set
            {
                scSettings.RecoveryDelay = value;
            }
        }
        
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


        public IController GetController()
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

            RebootTimeout = profile.Reboot;
            MessageTimeout = profile.Message;
            TimeBetweenTests = profile.InterTests;
            OperationDelay = profile.OperationDelay;

            DnsIpv4 = profile.DnsIpv4;
            NtpIpv4 = profile.NtpIpv4;
            GatewayIpv4 = profile.GatewayIpv4;

            DnsIpv6 = profile.DnsIpv6;
            NtpIpv6 = profile.NtpIpv6;
            GatewayIpv6 = profile.GatewayIpv6;

            UseEmbeddedPassword = profile.UseEmbeddedPassword;
            Password1 = profile.Password1;
            Password2 = profile.Password2;

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
                Profile profile = (Profile)lbProfiles.SelectedItem;
                ApplyProfile(profile);
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
        private void rbConformanceMode_CheckedChanged(object sender, EventArgs e)
        {
            rbConformanceMode.TabStop = true;
            rbDiagnosticMode.TabStop = true;

            EnableProfileControls();
            lbProfiles.SelectedItem = null;
            if (rbConformanceMode.Checked)
            {
                tbProfileName.Text = string.Empty;
            }
            _controller.SetCertificationMode(rbConformanceMode.Checked);

        }

        void scSettings_SelectTests(object sender, EventArgs e)
        {
            _controller.ApplyFeatures();
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
    }
}
