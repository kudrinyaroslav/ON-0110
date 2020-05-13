///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;

namespace TestTool.GUI
{
    /// <summary>
    /// Dialog window for entering IP information.
    /// </summary>
    public partial class IPInput : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public IPInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Network interface token entered by test operator.
        /// </summary>
        public string Token
        {
            get { return tbToken.Text; }
        }

        /// <summary>
        /// IP address entered by test operator
        /// </summary>
        public string IP
        {
            get { return tbIpAddress.Text; }
        }

        /// <summary>
        /// Prefix length
        /// </summary>
        public string Prefix
        {
            get { return tbPrefix.Text; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbToken.Text))
            {
                MessageBox.Show("Please enter network interface token", "Token is empty", MessageBoxButtons.OK);
                tbToken.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbIpAddress.Text))
            {
                MessageBox.Show("Please enter device IP address", "IP Address is empty", MessageBoxButtons.OK);
                tbIpAddress.Focus();
                return;
            }

            int prefixLength;
            if (!int.TryParse(tbPrefix.Text, out prefixLength))
            {
                MessageBox.Show("Prefix should be integer", "Prefix is incorrect", MessageBoxButtons.OK);
                tbPrefix.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

   
    }
}
