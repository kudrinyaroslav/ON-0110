///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Windows.Forms;
using ClientTestTool.Properties;

namespace ClientTestTool.GUI.Utils
{
  public static class DialogHelper
  {
    public static void ShowMessage(String msg)
    {
      if (String.IsNullOrEmpty(msg))
        return;

      MessageBox.Show(msg);
    }

    public static DialogResult ShowWarning(String warning)
    {
      return ShowWarning(Resources.Message_Warning, warning);
    }

    public static DialogResult ShowWarning(String caption, String warning)
    {
      if (String.IsNullOrEmpty(warning))
        throw new ArgumentNullException("warning");

      return MessageBox.Show(warning, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
    }

    public static void ShowError(String error)
    {
      ShowError(Resources.Message_Error, error);
    }

    public static void ShowError(String caption, String error)
    {
      if (String.IsNullOrEmpty(error))
        return;

      MessageBox.Show(error, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void ShowNotImplementedMsg()
    {
      MessageBox.Show(Resources.Message_Not_implemented_yet); // TODO remove it 
    }
  }
}
