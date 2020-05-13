using System;
using System.Windows.Forms;

namespace TestTool.GUI
{
    /// <summary>
    /// Dialog for selecting secury API
    /// </summary>
    public partial class SecureAPIForm : Form
    {
        /// <summary>
        /// Currently selected operation.
        /// </summary>
        public string Operation 
        {
            get { return cmbOperation.Text; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
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
