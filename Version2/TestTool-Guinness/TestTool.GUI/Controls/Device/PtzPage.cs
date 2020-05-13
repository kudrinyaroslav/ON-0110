///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Utils;
using Onvif = TestTool.Proxies.Onvif;
using TestTool.GUI.Data;
using TestTool.Tests.Common.Media;
using TestTool.Tests.Common.TestEngine;
using System.ComponentModel;

namespace TestTool.GUI.Controls.Device
{
    partial class PtzPage : UserControl, IPtzView
    {
        private class ProfileWrapper
        {
            public Onvif.Profile Profile { get; set; }
            public override string ToString()
            {
                return Profile != null ? !string.IsNullOrEmpty(Profile.Name) ? string.Format("{0} ({1})", Profile.Name, Profile.token) : Profile.token : string.Empty;
            }
        }
        private PtzController _controller;
        private VideoContainer _videoWindow;

        #region IPtzView
        public void DisplayLog(string logEntry)
        {
            BeginInvoke(new Action(() => tbReport.Text = logEntry));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PTZAddress
        {
            get { return tbPtzUrl.Text; }
            set
            {
                BeginInvoke(new Action(() =>
                {
                    tbPtzUrl.Text = value;
                }));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MediaAddress
        {
            get { return tbMediaUrl.Text; }
            set
            {
                BeginInvoke(new Action(() =>
                {
                    tbMediaUrl.Text = value;
                    cmbPTZProfiles.Items.Clear();
                }));
            }
        }
        public void ShowVideo(Onvif.MediaUri uri, Onvif.VideoEncoderConfiguration encoder)
        {
            try
            {
                _videoWindow = new VideoContainer();
                DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
                int messageTimeout = environment.Timeouts.Message;
                VideoUtils.AdjustVideo(
                    _videoWindow,
                    environment.Credentials.UserName,
                    environment.Credentials.Password,
                    messageTimeout,
                    Onvif.TransportProtocol.UDP,
                    Onvif.StreamType.RTPUnicast,
                    uri,
                    encoder);
                _videoWindow.KEEPALIVE = true;
                _videoWindow.OpenWindow(false);
                Invoke(new Action(() => { btnVideo.Text = "Stop Video"; }));
            }
            catch 
            {
                _videoWindow = null;
                throw;            	
            }
        }
        public void SetProfiles(Onvif.Profile[] profiles)
        {
            Invoke(new Action(() =>
            {
                cmbPTZProfiles.Items.Clear();
                foreach (Onvif.Profile profile in profiles)
                {
                    cmbPTZProfiles.Items.Add(new ProfileWrapper() { Profile = profile });
                }
                if (cmbPTZProfiles.Items.Count > 0)
                {
                    cmbPTZProfiles.SelectedIndex = 0;
                }
            }));
        }
        public void OnPTZConfigurationAdded(string profile, string config)
        {
            Invoke(new Action(() =>
            {
                foreach (object item in cmbPTZProfiles.Items)
                {
                    Onvif.Profile mediaProfile = (item as ProfileWrapper).Profile;
                    if (mediaProfile.token == profile)
                    {
                        //this is for disabling Add config button
                        mediaProfile.PTZConfiguration = new Onvif.PTZConfiguration();
                        break;
                    }
                }
            }));
        }
        #endregion

        public PtzPage()
        {
            InitializeComponent();
            _controller = new PtzController(this);
        }

        internal PtzController Controller
        {
            get { return _controller; }
        }
        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            panelAbsoluteMove.Enabled = rbAbsoluteMove.Checked || rbRelativeMove.Checked;
            panelContiniusMove.Enabled = rbContuniousMove.Checked;
        }

        public void SwitchToState(Enums.ApplicationState state)
        {
            if (state.IsActive())
            {
                EnableControls(false);
            }
            else
            {
                EnableControls(true);
            }
        }

        public void EnableControls(bool enable)
        {
            Invoke(new Action(() =>
            {
                DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
                string address = devices != null ? devices.ServiceAddress : string.Empty;

                Onvif.Profile profile = (cmbPTZProfiles.SelectedItem != null) ? (cmbPTZProfiles.SelectedItem as ProfileWrapper).Profile : null;
                Onvif.PTZConfiguration config = profile != null ? profile.PTZConfiguration : null;

                btnGetPtzUrl.Enabled = enable && !string.IsNullOrEmpty(address);
                btnGetProfiles.Enabled = enable && !string.IsNullOrEmpty(MediaAddress);
                btnVideo.Enabled = (enable && !string.IsNullOrEmpty(MediaAddress))||(_videoWindow != null);
                rbAbsoluteMove.Enabled = enable && !string.IsNullOrEmpty(PTZAddress) && (config != null);
                rbRelativeMove.Enabled = enable && !string.IsNullOrEmpty(PTZAddress) && (config != null);
                rbContuniousMove.Enabled = enable && !string.IsNullOrEmpty(PTZAddress) && (config != null);
                panelAbsoluteMove.Enabled = enable && (rbAbsoluteMove.Checked || rbRelativeMove.Checked) && !string.IsNullOrEmpty(PTZAddress) && (config != null);
                panelContiniusMove.Enabled = enable && rbContuniousMove.Checked && !string.IsNullOrEmpty(PTZAddress) && (config != null);
                btnAddPTZConfig.Enabled = enable && (profile != null) && (config == null);

                cmbPTZProfiles.Enabled = enable;
            }));
        }
        #region IView Members


        public IController GetController()
        {
            return _controller;
        }

        #endregion
        public void ShowError(Exception e)
        {
            BeginInvoke(new Action(() =>
            {
                MessageBox.Show(this, "Unexpected error occurred: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private void btnGetPtzUrl_Click(object sender, EventArgs e)
        {
            try
            {
                Controller.GetAddress(new Onvif.CapabilityCategory[] { Onvif.CapabilityCategory.All });
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_videoWindow == null)
                {
                    Controller.GetStreamUri();
                }
                else
                {
                    _videoWindow.CloseWindow();
                    _videoWindow = null;
                    btnVideo.Text = "Play Video";
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void IncrementalAbsoluteRelativeMove(bool x, bool y, bool z)
        {
            Onvif.Profile profile = (cmbPTZProfiles.SelectedItem != null) ? (cmbPTZProfiles.SelectedItem as ProfileWrapper).Profile : null;
            if (profile == null)
            {
                MessageBox.Show(this, "Select profile with PTZ configuration", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Controller.AbosuteRelativeIncrementalMove(
                        rbAbsoluteMove.Checked,
                        profile.token, 
                        x ? -1 : nudX.Value,
                        x ? 1 : nudX.Value, 
                        y ? -1 : nudY.Value,
                        y ? 1 : nudY.Value,
                        z ? 0 : nudZoom.Value,
                        z ? 1 : nudZoom.Value);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
            //rbAbsoluteMove.Checked = true; //why it loses selection?
        }
        private void AbsoluteRelativeMove()
        {
            Onvif.Profile profile = (cmbPTZProfiles.SelectedItem != null) ? (cmbPTZProfiles.SelectedItem as ProfileWrapper).Profile : null;
            if (profile == null)
            {
                MessageBox.Show(this, "Select profile with PTZ configuration", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Controller.AbosuteRelativeMove(rbAbsoluteMove.Checked, profile.token, nudX.Value, nudY.Value, nudZoom.Value);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
            //rbAbsoluteMove.Checked = true; //why it loses selection?
        }
        private void btnXMin_Click(object sender, EventArgs e)
        {
            if (nudX.Value == -1)
            {
                AbsoluteRelativeMove();
            }
            else
            {
                nudX.Value = -1;
            }
        }

        private void btnXMax_Click(object sender, EventArgs e)
        {
            if (nudX.Value == 1)
            {
                AbsoluteRelativeMove();
            }
            else
            {
                nudX.Value = 1;
            }
        }

        private void btnXFromMinToMax_Click(object sender, EventArgs e)
        {
            IncrementalAbsoluteRelativeMove(true, false, false);
            nudX.Value = 1;
        }

        private void btnYMin_Click(object sender, EventArgs e)
        {
            if (nudY.Value == -1)
            {
                AbsoluteRelativeMove();
            }
            else
            {
                nudY.Value = -1;
            }
        }

        private void btnYMax_Click(object sender, EventArgs e)
        {
            if (nudY.Value == 1)
            {
                AbsoluteRelativeMove();
            }
            else
            {
                nudY.Value = 1;
            }
        }

        private void btnYFromMinToMax_Click(object sender, EventArgs e)
        {
            IncrementalAbsoluteRelativeMove(false, true, false);
            nudY.Value = 1;
        }

        private void btnZoomMin_Click(object sender, EventArgs e)
        {
            if (nudZoom.Value == 0)
            {
                AbsoluteRelativeMove();
            }
            else
            {
                nudZoom.Value = 0;
            }
        }

        private void btnZoomMax_Click(object sender, EventArgs e)
        {
            if (nudZoom.Value == 1)
            {
                AbsoluteRelativeMove();
            }
            else
            {
                nudZoom.Value = 1;
            }
        }

        private void btnZoomFromMinToMax_Click(object sender, EventArgs e)
        {
            IncrementalAbsoluteRelativeMove(false, false, true);
            nudZoom.Value = 1;
        }

        private void nudX_ValueChanged(object sender, EventArgs e)
        {
            AbsoluteRelativeMove();
        }

        private void nudY_ValueChanged(object sender, EventArgs e)
        {
            AbsoluteRelativeMove();
        }

        private void nudZoom_ValueChanged(object sender, EventArgs e)
        {
            AbsoluteRelativeMove();
        }
        private void btnGetProfiles_Click(object sender, EventArgs e)
        {
            try
            {
                Controller.GetProfiles();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnAddPTZConfig_Click(object sender, EventArgs e)
        {
            try
            {
                Onvif.Profile profile = (cmbPTZProfiles.SelectedItem != null) ? (cmbPTZProfiles.SelectedItem as ProfileWrapper).Profile : null;
                Controller.AddPTZConfiguration(profile);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void cmbPTZProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Onvif.Profile profile = (cmbPTZProfiles.SelectedItem != null) ? (cmbPTZProfiles.SelectedItem as ProfileWrapper).Profile : null;
            Onvif.PTZConfiguration config = profile != null ? profile.PTZConfiguration : null;
            btnAddPTZConfig.Enabled = (config == null);

            panelAbsoluteMove.Enabled = (rbAbsoluteMove.Checked || rbRelativeMove.Checked) && !string.IsNullOrEmpty(PTZAddress) && (config != null);
            panelContiniusMove.Enabled = rbContuniousMove.Checked && !string.IsNullOrEmpty(PTZAddress) && (config != null);
            rbAbsoluteMove.Enabled = !string.IsNullOrEmpty(PTZAddress) && (config != null);
            rbRelativeMove.Enabled = !string.IsNullOrEmpty(PTZAddress) && (config != null);
            rbContuniousMove.Enabled = !string.IsNullOrEmpty(PTZAddress) && (config != null);
        }

        private void ContinuousMove(bool panTilt, bool zoom)
        {
            Onvif.Profile profile = (cmbPTZProfiles.SelectedItem != null) ? (cmbPTZProfiles.SelectedItem as ProfileWrapper).Profile : null;
            if (profile == null)
            {
                MessageBox.Show(this, "Select profile with PTZ configuration", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int timeout = -1;
                if (chkUseTimeout.Checked && (!int.TryParse(tbTimeout.Text, out timeout) || (timeout < 0)))
                {
                    MessageBox.Show(this, "Incorrect timeout", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        Controller.ContinuousMove(profile.token, panTilt, zoom, nudVx.Value, nudVy.Value, nudVzoom.Value, timeout);
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                }
            }
        }
        private void Stop(bool panTilt, bool zoom)
        {
            Onvif.Profile profile = (cmbPTZProfiles.SelectedItem != null) ? (cmbPTZProfiles.SelectedItem as ProfileWrapper).Profile : null;
            if (profile == null)
            {
                MessageBox.Show(this, "Select profile with PTZ configuration", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Controller.Stop(profile.token, panTilt, zoom);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }
        private void btnVxStartMove_Click(object sender, EventArgs e)
        {
            ContinuousMove(true, false);
        }

        private void btnVxStopMove_Click(object sender, EventArgs e)
        {
            Stop(true, false);
        }

        private void btnStartZoom_Click(object sender, EventArgs e)
        {
            ContinuousMove(false, true);
        }

        private void btnStopZoom_Click(object sender, EventArgs e)
        {
            Stop(false, true);
        }

        private void btnStartAll_Click(object sender, EventArgs e)
        {
            ContinuousMove(true, true);
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            Stop(true, true);
        }



    }
}
