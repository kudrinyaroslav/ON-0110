using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMC.Controls
{
    class ReadOnlyCheckbox : CheckBox
    {
        int WM_LBUTTONDOWN = 0x0201; //513
        int WM_KEYDOWN = 0x100; 
        int WM_LBUTTONDBLCLK = 0x0203; //515

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK || m.Msg == WM_KEYDOWN)
            {
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
