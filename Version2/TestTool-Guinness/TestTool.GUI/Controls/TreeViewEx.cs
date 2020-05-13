using System;
using System.Windows.Forms;

namespace TestTool.GUI.Controls
{
    class TreeViewEx : TreeView 
    {
        protected override void WndProc(ref Message m) 
        {
            if (m.Msg == 0x203)
            {
                m.Result = IntPtr.Zero;
            }
            else
            {
                base.WndProc(ref m);
            }
        }

    }

}
