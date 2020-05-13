///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Utils;

namespace TestTool.GUI.Controls.Device
{
    partial class DeviceManagementPage : Page, IDeviceManagementView
    {
        private DeviceManagementController _controller;

        public DeviceManagementPage()
        {
            InitializeComponent();
            _controller = new DeviceManagementController(this);
        }

        internal DeviceManagementController Controller
        {
            get { return _controller; }
        }

        public void SwitchToState(Enums.ApplicationState state)
        {
            if (state.IsActive())
            {
                BeginInvoke(new Action(() => EnableFunctions(false)));
            }
            else
            {
                BeginInvoke(new Action(() => EnableFunctions(_controller.DeviceNotEmpty() && !_controller.RequestPending)));
            }
        }
        
        public void EnableFunctions(bool bEnable)
        {
            Control[] controls = new Control[]{btnDeviceInfo, btnGetHostname, btnGetInterfaces, btnHardReset, btnProbe, btnReboot, btnSetIPAddress, btnSetGateway, btnSyncTime};
            if (bEnable)
            {
                EnableControls(controls);
            }
            else
            {
                DisableControls(controls);
            }
        }

        public void DisplayLog(string logEntry)
        {
            BeginInvoke(new Action( () => tbReport.Text = logEntry));
        }

        #region IView Members


        public IController GetController()
        {
            return _controller;
        }

        #endregion

        #region IDeviceManagementView Members

        public string DeviceManagementUrl
        {
            get
            {
                return tbUrl.Text;
            }
            set
            {
                tbUrl.Text = value;
            }
        }

        public string IP
        {
            get
            {
                return tbIP.Text;
            }
            set
            {
                tbIP.Text = value;
            }
        }

        public void DisplayDeviceInformation(string manufacturer, string model, string firmwareVersion, string serial,
                              string hardwareId)
        {
            BeginInvoke(new Action(() => { 
                tbManufacturer.Text = manufacturer;
                tbModel.Text = model;
                tbFirmware.Text = firmwareVersion;
                tbSerial.Text = serial;
                tbHardware.Text = hardwareId;
            }));
        }
        
        public void Clear()
        {
            tbReport.Clear();
            tbFirmware.Clear();
            tbHardware.Clear();
            tbManufacturer.Clear();
            tbModel.Clear();
            tbSerial.Clear();
        }
        
        public void ReportError(string error)
        {
            MessageBox.Show(error, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void btnDeviceInfo_Click(object sender, EventArgs e)
        {
            tbReport.Clear();
            _controller.GetDeviceInformation();
        }

        private void btnGetHostname_Click(object sender, EventArgs e)
        {
            tbReport.Clear();
            _controller.GetHostname();
        }

        private void btnGetInterfaces_Click(object sender, EventArgs e)
        {
            tbReport.Clear();
            _controller.GetNetworkInterfaces();
        }

        private void btnProbe_Click(object sender, EventArgs e)
        {
            tbReport.Clear();
            _controller.GetScopes(); 
        }

        private void btnHardReset_Click(object sender, EventArgs e)
        {
            tbReport.Clear();
            _controller.HardReset();
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            tbReport.Clear();
            _controller.Reboot();
        }

        private void btnSetIPAddress_Click(object sender, EventArgs e)
        {
            IPInput dialog = new IPInput();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string token = dialog.Token;
                string ip = dialog.IP;
                string prefix = dialog.Prefix;
                int prefixLength = int.Parse(prefix);

                tbReport.Clear();
                _controller.SetIPAddress(token, ip, prefixLength);
            }
        }

        private void btnSetGateway_Click(object sender, EventArgs e)
        {
            TextInput input = new TextInput("Enter Network Gateway", "Gateway: ", ValidateIPAddress);
            if (input.ShowDialog() == DialogResult.OK)
            {
                string gateway = input.Input;
                tbReport.Clear();
                _controller.SetGateway(gateway);
            }
        }

        bool ValidateIPAddress(string address)
        {
            System.Net.IPAddress buffer;
            if (!System.Net.IPAddress.TryParse(address, out buffer))
            {
                MessageBox.Show("IP address is not correct");
                return false;
            }
            return true;
        }

        private void btnSyncTime_Click(object sender, EventArgs e)
        {
            tbReport.Clear();
            _controller.SyncTime();
        }


    }
}
