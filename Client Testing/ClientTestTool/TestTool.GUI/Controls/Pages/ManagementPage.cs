using System;
using System.Collections.Generic;
using TestTool.GUI.Controllers;
using TestTool.GUI.Data;
using TestTool.Device;

namespace TestTool.GUI.Controls.Pages
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

            tbUsername.Text = "admin";
            tbPassword.Text = "12345";
        }

        #region IView Members

        public IController GetController()
        {
            return _controller;
        }

        public AuthenticationMode AuthenticationMode
        {
            get
            {
                if (rbNone.Checked)
                {
                    return AuthenticationMode.None;
                }
                if (rbDigest.Checked)
                {
                    return AuthenticationMode.Digest;
                }
                return AuthenticationMode.WS;
            }
            set
            {
                switch (value)
                {
                    case AuthenticationMode.None:
                        rbNone.Checked = true;
                        break;
                    case AuthenticationMode.Digest:
                        rbDigest.Checked = true;
                        break;
                    case AuthenticationMode.WS:
                        rbWsUsername.Checked = true;
                        break;
                }
            }
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
        
        #region IManagementView Members

        public string BaseAddress
        {
            get
            {
                NICListItem item = (NICListItem) cmbNICs.SelectedItem;
                string port = tbServicePort.Text;
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

        public string Username
        {
            get { return tbUsername.Text; }
            set { tbUsername.Text = value; }
        }
        public string Password
        {
            get { return tbPassword.Text; }
            set { tbPassword.Text = value;}
        }

        #endregion

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
            tbDeviceServiceAddress.Text = ServicesEnvironment.GetDeviceServiceAddress(BaseAddress);
        }

        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            tbUsername.Enabled = !rbNone.Checked;
            tbPassword.Enabled = !rbNone.Checked;
        }

        private void ManagementPage_SizeChanged(object sender, EventArgs e)
        {
            lblDescription.Left = (this.Width - lblDescription.Width)/2;
        }

    }
}
