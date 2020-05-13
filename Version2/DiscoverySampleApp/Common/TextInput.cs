///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;

namespace ONVIFSampleApp
{
    public delegate bool ValidateFunction(string input);

    /// <summary>
    /// Dialog window with single EditBox.
    /// </summary>
    public partial class TextInput : Form
    {

        protected TextInput()
        {
            InitializeComponent();
        }


        public TextInput(string caption, string label)
            :this()
        {
            lblPrompt.Text = label;
            this.Text = caption;
        }

        public string Input
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
