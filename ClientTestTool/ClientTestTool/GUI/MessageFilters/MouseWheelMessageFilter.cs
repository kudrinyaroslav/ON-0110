using System;
using System.Drawing;
using System.Windows.Forms;
using ClientTestTool.GUI.Native;

namespace ClientTestTool.GUI.MessageFilters
{  
  /// <summary>
  /// Filter used to scroll non-focused controls
  /// </summary>
  class MouseWheelMessageFilter : IMessageFilter
  {
    public bool PreFilterMessage(ref Message m)
    {
      if (m.Msg == (int)WinApi.Messages.WM_MOUSEWHEEL)
      {
        Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
        IntPtr hWnd = WinApi.WindowFromPoint(pos);

        if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
        {
          WinApi.SendMessage(hWnd, m.Msg, (int)m.WParam, (int)m.LParam);
          return true;
        }
      }
      return false;
    }
  }
}
