using System;
using System.Collections.Generic;
using System.Net.Sockets;
using TestTool.GUI.Controllers;
using TestTool.GUI.Data;

namespace TestTool.GUI.Controls
{
    internal partial class ManagementPage : BasePage, Views.IManagementView
    {
        private ManagementController _controller;

        public ManagementPage()
        {
            InitializeComponent();

            _controller = new ManagementController(this);
            FillNetworkInterfaces();
            cmbNICs.SelectedIndex = 0;

            tbPort.Text = "8080";
        }

        #region IView Members

        public IController GetController()
        {
            return _controller;
        }

        #endregion

        /// <summary>
        /// Populates list of network interface controllers
        /// </summary>
        private void FillNetworkInterfaces()
        {
            List<NetworkInterfaceDescription> interfaces = _controller.GetNetworkInterfaces();
            foreach (NetworkInterfaceDescription nic in interfaces)
            {
                NICListItem item = new NICListItem(nic, checkHWStyle.Checked);
                cmbNICs.Items.Add(item);
            }
        }

        /// <summary>
        /// Handles HW Style chekbox checked event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void checkHWStyle_CheckedChanged(object sender, EventArgs e)
        {
            int selected = cmbNICs.SelectedIndex;
            cmbNICs.Items.Clear();
            FillNetworkInterfaces();
            if (selected < cmbNICs.Items.Count)
            {
                cmbNICs.SelectedIndex = selected;
            }
        }

        private void cmbNICs_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateServiceAddres();
        }

        private void tbServicePort_TextChanged(object sender, EventArgs e)
        {
            UpdateServiceAddres();
        }

        void UpdateServiceAddres()
        {
            NICListItem item = (NICListItem)cmbNICs.SelectedItem;
            string port = tbPort.Text;
            string ip;
            if (item.NIC.IP.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ip = string.Format("[{0}]", item.NIC.IP);
            }
            else
            {
                ip = item.NIC.IP.ToString();
            }
            string addr = string.Format("http://{0}:{1}/onvif/device_service", ip, port);
            tbDeviceServiceAddress.Text = addr;
        }

        #region IManagementView Members


        public string BaseAddress
        {
            get
            {
                NICListItem item = (NICListItem) cmbNICs.SelectedItem;
                string port = tbPort.Text;
                string addr = string.Format("http://{0}:{1}/", item.NIC.IP, port);
                return addr;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //select saved IP address
                    foreach (object item in cmbNICs.Items)
                    {
                        NICListItem nicItem = item as NICListItem;
                        if ((nicItem.NIC.IP != null) && (nicItem.NIC.IP.ToString() == value))
                        {
                            cmbNICs.SelectedItem = item;
                            break;
                        }
                    }
                }

            }
        }

        public string DutAddress
        {
            get
            {
                return tbDeviceAddress.Text;
            }
            set
            {
                tbDeviceAddress.Text = value;
            }
        }

        public string Username
        {
            get
            {
                return tbUsername.Text; 
            }
            set
            {
                tbUsername.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return tbPassword.Text;
            }
            set
            {
                tbPassword.Text = value;
            }
        }

        public void SwitchToWorkingMode()
        {
            EnableControls(false);
        }
        
        public void SwitchToIdleMode()
        {
            EnableControls(true);
        }

        #endregion

        void EnableControls(bool bEnable)
        {
            cmbNICs.Enabled = bEnable;
            tbPort.Enabled = bEnable;
            tbDeviceAddress.Enabled = bEnable;
            tbUsername.Enabled = bEnable;
            tbPassword.Enabled = bEnable;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            DiscoveryForm discoveryForm = new DiscoveryForm(_controller);

            if (discoveryForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
        
    }
}
