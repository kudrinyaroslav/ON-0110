using System;
using System.Windows.Forms;

namespace TestTool.GUI
{
    /// <summary>
    /// Form for defining subscription timeout.
    /// </summary>
    public partial class SubscriptionTimeoutForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SubscriptionTimeoutForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Prompt for entering timeout.
        /// </summary>
        public string Prompt
        {
            get { return lblPrompt.Text; }
            set { lblPrompt.Text = value;}
        }

        /// <summary>
        /// Timeout.
        /// </summary>
        public string Timeout
        {
            get { return tbInput.Text; }
            set { tbInput.Text = value;}
        }

        /// <summary>
        /// Prompt for future actions.
        /// </summary>
        public string EventAction
        {
            get { return lblEventAction.Text; }
            set { lblEventAction.Text = value; }
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
                MessageBox.Show("Timeout should be non-negative integer");
            }
        }
        
    }
}
