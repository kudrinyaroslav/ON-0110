///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Enums;
using TestTool.GUI.Data;

namespace TestTool.GUI.Controls
{
    /// <summary>
    /// Setup tab control
    /// </summary>
    partial class SetupPage : UserControl, ISetupView
    {
        private SetupController _controller;

        #region ISetupView Members
        /// <summary>
        /// Gets or sets firmware version
        /// </summary>
        public string FirmwareVersion 
        { 
            get { return tbFirmwareVersion.Text; } 
            set { tbFirmwareVersion.Text = value; } 
        }
        /// <summary>
        /// Gets or sets brand
        /// </summary>
        public string Brand 
        { 
            get { return tbBrand.Text; }
            set { tbBrand.Text = value; } 
        }
        /// <summary>
        /// Gets or sets model
        /// </summary>
        public string Model 
        { 
            get { return tbModel.Text; }
            set { tbModel.Text = value; } 
        }
        /// <summary>
        /// Gets or sets serial number
        /// </summary>
        public string Serial 
        { 
            get { return tbSerial.Text; }
            set { tbSerial.Text = value; } 
        }
        /// <summary>
        /// Gets or sets other information text
        /// </summary>
        public string OtherInformation 
        { 
            get { return tbOtherInformation.Text; }
            set { tbOtherInformation.Text = value; } 
        }

        /// <summary>
        /// Gets or sets operator name
        /// </summary>
        public string OperatorName 
        { 
            get { return tbOperatorName.Text; }
            set { tbOperatorName.Text = value; } 
        }

        /// <summary>
        /// Gets or sets organization name
        /// </summary>
        public string OrganizationName 
        { 
            get { return tbOrganizationName.Text; }
            set { tbOrganizationName.Text = value; } 
        }

        /// <summary>
        /// Gets or sets organization address
        /// </summary>
        public string OrganizationAddress 
        { 
            get { return tbOrganizationAddress.Text; }
            set { tbOrganizationAddress.Text = value; } 
        }

        /// <summary>
        /// Shows error message to user
        /// </summary>
        /// <param name="e">Exception to display</param>
        public void ShowError(Exception e)
        {
            string message = e.Message.Length > 400 ? e.Message.Substring(0, 400) : e.Message;
            BeginInvoke(new Action(() =>
            {
                MessageBox.Show(this, "Unexpected error occurred: " + message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }
        #endregion

        /// <summary>
        /// C-tor
        /// </summary>
        public SetupPage()
        {
            InitializeComponent();
            _controller = new SetupController(this);
            _controller.DeviceInformationReceived += OnDeviceInformationReceived;
            _controller.OperationStarted += OnGetDeviceInformationBegin;
            _controller.OperationCompleted += OnGetDeviceInformationEnd;
        }

        /// <summary>
        /// Returns controller
        /// </summary>
        internal SetupController Controller
        {
            get { return _controller; }
        }
        
        /// <summary>
        /// Switches control to specified state
        /// </summary>
        /// <param name="state">New state</param>
        public void SwitchToState(ApplicationState state)
        {
            switch (state)
            {
                case ApplicationState.Idle:
                    Invoke(new Action<bool>(EnableGetFromDevice), new object[] { true });
                    break;
                case ApplicationState.CommandRunning:
                case ApplicationState.DiscoveryRunning:
                case ApplicationState.TestPaused:
                case ApplicationState.TestRunning:
                    Invoke(new Action<bool>(EnableGetFromDevice), new object[] { false });
                    break;
            }
        }

        /// <summary>
        /// Handles button clear device information click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnClearDeviceInformation_Click(object sender, EventArgs e)
        {
            tbBrand.Clear();
            tbModel.Clear();
            tbSerial.Clear();
            tbFirmwareVersion.Clear();
            tbOtherInformation.Clear();
        }

        /// <summary>
        /// Handles button clear click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbOperatorName.Clear();
            tbOrganizationName.Clear();
            tbOrganizationAddress.Clear();
        }

        /// <summary>
        /// Handles button get device information click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnGetDeviceInformation_Click(object sender, EventArgs e)
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            string url = devices.ServiceAddress;
            if(!string.IsNullOrEmpty(url))
            {
                try
                {
                    Controller.GetDeviceInformation();
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    MessageBox.Show(this, "Could not connect to " + url, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch(Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        /// <summary>
        /// Handles device information received event
        /// </summary>
        /// <param name="manufacturer">Device manufacturer</param>
        /// <param name="model">Device model</param>
        /// <param name="firmwareVersion">Device firmware version</param>
        /// <param name="serial">Device serial number</param>
        /// <param name="hardwareId">Device hardware id</param>
        private void OnDeviceInformationReceived(string manufacturer, string model, string firmwareVersion, string serial, string hardwareId)
        {
            BeginInvoke(new Action(() => {tbModel.Text = model;}));
            BeginInvoke(new Action(() => {tbFirmwareVersion.Text = firmwareVersion;}));
            BeginInvoke(new Action(() => {tbSerial.Text = serial;}));
            BeginInvoke(new Action(() => {tbBrand.Text = manufacturer;}));
        }

        /// <summary>
        /// Handles GetDeviceInformationBegin event
        /// </summary>
        protected void OnGetDeviceInformationBegin()
        {
            Controller.SwitchToState(ApplicationState.CommandRunning);
        }

        /// <summary>
        /// Handles GetDeviceInformationEnd event
        /// </summary>
        protected void OnGetDeviceInformationEnd()
        {
            Controller.SwitchToState(ApplicationState.Idle);
        }

        #region IView Members
        public IController GetController()
        {
            return _controller;
        }
        #endregion

        /// <summary>
        /// Enable or disables "Get from device" button
        /// </summary>
        /// <param name="enable"></param>
        public void EnableGetFromDevice(bool enable)
        {
            btnGetDeviceInformation.Enabled = enable;
            btnGetDeviceInformation.Refresh();
        }

        /// <summary>
        /// Show information about Test Tool
        /// </summary>
        public void ShowToolInfo()
        {
            BeginInvoke(new Action(() =>
            {
                ApplicationInfo info = _controller.GetApplicationInfo();
                tbToolVersion.Text = info.ToolVersion;
                tbTestSpec.Text = info.TestSpecification;
                tbCoreSpec.Text = info.CoreSpecification;
            }));
        }
    }
}
