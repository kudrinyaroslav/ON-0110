///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClientTestTool.GUI.Extensions
{
  internal static class TreeNodeExtension
  {
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetScrollPos(IntPtr hWnd, int nBar);

    [DllImport("user32.dll")]
    private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

    private const int SB_HORZ = 0x0;
    private const int SB_VERT = 0x1;

    /// <summary>
    /// Right scrolling fix, use it instead of node.EnsureVisible
    /// </summary>
    public static void EnsureVisibleWithoutScrolling(this TreeNode node, bool isVScrollingEnabled = true)
    {
      if (null == node)
        throw new ArgumentNullException("node");

      int lastPosition = GetScrollPos(node.TreeView.Handle, SB_VERT);

      node.TreeView.SuspendLayout();

      node.EnsureVisible();

      if (!isVScrollingEnabled)
        SetScrollPos(node.TreeView.Handle, SB_VERT, lastPosition, true);

      SetScrollPos(node.TreeView.Handle, SB_HORZ, 0, true);

      node.TreeView.ResumeLayout();
    }
  }
}
