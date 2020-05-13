///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;

namespace ONVIFSampleApp
{
       /// <summary>
    /// Dialog window with single EditBox.
    /// </summary>
    public partial class TypeInput : Form
    {

        public TypeInput()
        {
            InitializeComponent();
        }


        public TypeInput(string caption, string label)
            :this()
        {
            lblPrompt.Text = label;
            this.Text = caption;
        }

        public string Type
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }

        public string Namespace
        {
            get { return txtNamespace.Text; }
            set { txtNamespace.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
