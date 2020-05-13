///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;

namespace TestTool.GUI
{
    /// <summary>
    /// Function for validating string input.
    /// </summary>
    /// <param name="input">Value entered by a user.</param>
    /// <returns>True, if valus can be accepted.</returns>
    public delegate bool ValidateFunction(string input);

    /// <summary>
    /// Dialog window with single EditBox.
    /// </summary>
    public partial class TextInput : Form
    {
        private ValidateFunction _validateFunction;

        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected TextInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="caption">Window caption.</param>
        /// <param name="label">Prompt.</param>
        public TextInput(string caption, string label)
            : this(caption,label, null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="caption">Window caption.</param>
        /// <param name="label">Prompt.</param>
        /// <param name="validateFunction">Function for validating input.</param>
        public TextInput(string caption, string label, ValidateFunction validateFunction)
            :this()
        {
            lblPrompt.Text = label;
            this.Text = caption;
            _validateFunction = validateFunction;
        }

        /// <summary>
        /// Sets initial value or gets user input.
        /// </summary>
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

            DialogResult = DialogResult.OK;
        }
    }
}
