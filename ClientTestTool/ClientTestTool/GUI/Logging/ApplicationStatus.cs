///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Windows.Forms;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Logging
{
  public static class ApplicationStatus
  {
    public static void SetStatus(String status)
    {
      Form mainForm = Application.OpenForms["MainForm"];

      if (null == status)
        throw new ArgumentNullException("status");

      if (null == mainForm)
        return;

      var statusStrip = mainForm.Controls["sSMain"] as StatusStrip;

      if (null == statusStrip)
        return;

      var statusLabel = statusStrip.Items["tSSLStatus"];

      statusStrip.InvokeIfRequired(() => statusLabel.Text = status);
    }

    public static void SetProgress(int value)
    {
      Form mainForm = Application.OpenForms["MainForm"];

      if (null == mainForm)
        return;

      var statusStrip = mainForm.Controls["sSMain"] as StatusStrip;

      if (null == statusStrip)
        return;

      var progressBar = statusStrip.Items["tSPBProgress"] as ToolStripProgressBar;

      if (null == progressBar)
        return;

      var statusLabel = statusStrip.Items["tSSLPercentage"];

      String text = String.Format("{0}%", value);

      statusStrip.InvokeIfRequired(() =>
      {
        progressBar.Value = value;
        statusLabel.Text = text;
      });
    }
  }
}
