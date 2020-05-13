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
        public IPInput()
        {
            InitializeComponent();
        }

        public string Token
        {
            get { return tbToken.Text; }
        }

        public string IP
        {
            get { return tbIpAddress.Text; }
        }

        public string Prefix
        {
            get { return tbPrefix.Text; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbToken.Text))
            {
                ShowPrompt("Please enter network interface token", "Token is empty");
                tbToken.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbIpAddress.Text))
            {
                ShowPrompt("Please enter device IP address", "IP Address is empty");
                tbIpAddress.Focus();
                return;
            }

            int prefixLength;
            if (!int.TryParse(tbPrefix.Text, out prefixLength))
            {
                ShowPrompt("Prefix should be integer", "Prefix is incorrect");
                tbPrefix.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        void ShowPrompt(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK);
        }
   
    }
}
