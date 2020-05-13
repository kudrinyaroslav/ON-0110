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
    public partial class SubscriptionTimeoutForm : Form
    {
        public SubscriptionTimeoutForm()
        {
            InitializeComponent();
        }

        public string Prompt
        {
            get { return lblPrompt.Text; }
            set { lblPrompt.Text = value;}
        }

        public string Timeout
        {
            get { return tbInput.Text; }
            set { tbInput.Text = value;}
        }

        public string EventAction
        {
            get { return lblEventAction.Text; }
            set { lblEventAction.Text = value; }
        }

        public void HideEventAction()
        {
            lblEventAction.Height = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int timeout;
            bool bOk = int.TryParse(tbInput.Text, out timeout);
            if (bOk)
            {
                bOk = timeout > 0;
            }
            if (bOk)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Timeout should be non-negative integer", "Value is incorrect");
            }
        }
        
    }
}
