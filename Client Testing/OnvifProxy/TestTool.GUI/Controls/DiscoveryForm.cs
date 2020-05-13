using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.GUI.Data;

namespace TestTool.GUI.Controls
{
    internal partial class DiscoveryForm : Form
    {
        public DiscoveryForm()
        {
            InitializeComponent();
        }

        private Controllers.ManagementController _controller;
                                                 
        public DiscoveryForm(Controllers.ManagementController controller) : this()
        {
            _controller = controller; 
            FillNetworkInterfaces();
            if (cmbNICs.Items.Count > 0)
            {
                cmbNICs.SelectedIndex = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

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

        private void btnDiscover_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented in demo-version", "Under construction!");
        }

        public string ServiceAddress
        {
            get { return cmbServiceAddress.Text; }
        }
    }
}
