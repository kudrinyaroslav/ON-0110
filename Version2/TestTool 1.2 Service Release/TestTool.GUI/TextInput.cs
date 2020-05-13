///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;

namespace TestTool.GUI
{
    public delegate bool ValidateFunction(string input);

    /// <summary>
    /// Dialog window with single EditBox.
    /// </summary>
    public partial class TextInput : Form
    {
        private ValidateFunction _validateFunction;

        protected TextInput()
        {
            InitializeComponent();
        }

        public TextInput(string caption, string label)
            : this(caption,label, null)
        {

        }

        public TextInput(string caption, string label, ValidateFunction validateFunction)
            :this()
        {
            lblPrompt.Text = label;
            this.Text = caption;
            _validateFunction = validateFunction;
        }

        public string Input
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_validateFunction != null)
            {
                if (!_validateFunction(txtInput.Text))
                {
                    txtInput.Focus();
                    return;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
