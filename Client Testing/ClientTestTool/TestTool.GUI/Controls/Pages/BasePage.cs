using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTool.GUI.Controls.Pages
{
    internal class BasePage : UserControl
    {
        public void ReportError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void EnableControls(IEnumerable<Control> controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = true;
            }
        }

        protected void DisableControls(IEnumerable<Control> controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = false;
            }
        }
    }
}
