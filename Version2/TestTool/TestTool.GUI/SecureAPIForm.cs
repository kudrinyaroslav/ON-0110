using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTool.GUI
{
    public partial class SecureAPIForm : Form
    {
        public string Operation 
        {
            get { return cmbOperation.Text; }
        }
        public SecureAPIForm()
        {
            InitializeComponent();

            cmbOperation.Items.Add("CreateUsers");
            cmbOperation.Items.Add("GetDeviceInformation");
            cmbOperation.Items.Add("GetSystemDateAndTime");
            cmbOperation.Items.Add("GetScopes");
            cmbOperation.Items.Add("GetDiscoveryMode");
            cmbOperation.Items.Add("GetUsers");
            cmbOperation.Items.Add("GetWsdlUrl");
            cmbOperation.Items.Add("GetCapabilities");
            cmbOperation.Items.Add("GetHostname");
            cmbOperation.Items.Add("GetDNS");
            cmbOperation.Items.Add("GetNTP");
            cmbOperation.Items.Add("GetNetworkInterfaces");
            cmbOperation.Items.Add("GetNetworkProtocols");
            cmbOperation.Items.Add("GetNetworkDefaultGateway");
            cmbOperation.Items.Add("GetZeroConfiguration");

            cmbOperation.Text = "GetUsers";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
