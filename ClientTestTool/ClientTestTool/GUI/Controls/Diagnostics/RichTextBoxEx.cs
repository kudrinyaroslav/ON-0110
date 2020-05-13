///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
﻿using ClientTestTool.GUI.Native;

namespace ClientTestTool.GUI.Controls.Diagnostics
{
  public class RichTextBoxEx : RichTextBox
  {
    public event EventHandler ScrolledToBottom;

    [DllImport("user32.dll")]
    private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, ref Point lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetScrollPos(IntPtr hWnd, int nBar);

    [DllImport("user32.dll")]
    private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

    private bool IsAtMaxScroll()
    {
      int minScroll;
      int maxScroll;
      GetScrollRange(Handle, SB_VERT, out minScroll, out maxScroll);
      Point scrollPosition = Point.Empty;
      SendMessage(Handle, EM_GETSCROLLPOS, 0, ref scrollPosition);

      int bottomLine = (int) (maxScroll * (1 - (double)SCROLL_OFFSET_PERCENTAGE / 100));

      return (scrollPosition.Y + ClientSize.Height >= bottomLine);
    }

    protected virtual void OnScrolledToBottom(EventArgs e)
    {
      if (ScrolledToBottom != null)
        ScrolledToBottom(this, e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (IsAtMaxScroll())
        OnScrolledToBottom(EventArgs.Empty);

      
      base.OnKeyDown(e);
    }

    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case WM_VSCROLL: //scrolling messages
          HandleScroll(ref m);
          break;

        case WM_MOUSEWHEEL: //mousewheel messages
          var delta = (short)WinApi.HiWord((int)m.WParam);

          if (delta < 0) // scrolling down
            HandleScroll(ref m);
          else
            base.WndProc(ref m);
          break;

        case WM_SETFOCUS:
          break; // ignore focus messages

        default:
          base.WndProc(ref m);
          break;
      }
    }

    private void HandleScroll(ref Message m)
    {
      base.WndProc(ref m);

      if (IsAtMaxScroll())
        OnScrolledToBottom(EventArgs.Empty);
    }

    private const int WM_SETFOCUS      = 0x7;
    private const int WM_VSCROLL       = 0x115;
    private const int WM_MOUSEWHEEL    = 0x20A;
    private const int WM_USER          = 0x400;
    private const int SB_VERT          = 0x1;
    private const int EM_SETSCROLLPOS  = WM_USER + 222;
    private const int EM_GETSCROLLPOS  = WM_USER + 221;

    private const int SCROLL_OFFSET_PERCENTAGE = 10;
  }
}