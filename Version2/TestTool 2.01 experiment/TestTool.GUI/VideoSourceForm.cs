using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestTool.GUI
{
    /// <summary>
    /// Video source selection form.
    /// </summary>
    public partial class VideoSourceForm : Form
    {
        private List<string> _sources = new List<string>();
        /// <summary>
        /// Available sources.
        /// </summary>
        public List<string> Sources 
        {
            get { return _sources; }
        }
        /// <summary>
        ///Video source token selected.
        /// </summary>
        public string VideoSourceToken 
        {
            get { return cmbVideoSourceToken.Text; } 
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public VideoSourceForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void VideoSourceForm_Load(object sender, EventArgs e)
        {
            foreach (string token in _sources)
            {
                cmbVideoSourceToken.Items.Add(token);
            }
            if(cmbVideoSourceToken.Items.Count > 0)
            {
                cmbVideoSourceToken.SelectedIndex = 0;
            }
        }
    }
}
