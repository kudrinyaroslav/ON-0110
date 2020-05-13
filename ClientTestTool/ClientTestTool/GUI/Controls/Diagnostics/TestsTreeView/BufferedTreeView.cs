///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView
{
  public partial class BufferedTreeView : TreeView
  {
    public BufferedTreeView()
    {
      InitializeComponent();
      SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
    }

    protected override void OnHandleCreated(EventArgs e)
    {
      SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
      base.OnHandleCreated(e);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

    private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
    private const int TVS_EX_DOUBLEBUFFER  = 0x0004;
  }
}
