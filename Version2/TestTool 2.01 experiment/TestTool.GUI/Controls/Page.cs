///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestTool.GUI.Controls
{
    class Page : UserControl
    {
        protected void EnableControls(IEnumerable<Control> controls)
        {
            foreach (Control control  in controls)
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
