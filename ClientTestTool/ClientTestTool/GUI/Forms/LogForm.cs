///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Windows.Forms;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Logging;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Properties;

namespace ClientTestTool.GUI.Forms
{
  public partial class LogForm : Form
  {
    public LogForm()
    {
      InitializeComponent();
    }

    #region EventHandlers

    private void LogForm_Load(object sender, EventArgs e)
    {
      LoadLog();
      
      mWatcher = new FileSystemWatcher(CTTSettings.GetOutputDir())
      {
        NotifyFilter          = NotifyFilters.LastWrite,
        Filter                = Path.GetFileName(Settings.Default.LogFilename),
        IncludeSubdirectories = false,
        EnableRaisingEvents   = true

      };

      mWatcher.Changed += (o, args) => Invoke(new Action(LoadLog));
    }

    private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      mWatcher.Dispose();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      Close();
    }

    #endregion

    #region Helpers

    private void LoadLog()
    {
      try
      {
        using (var stream = new FileStream(
            Settings.Default.LogFilename,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite))
        {
          using (var reader = new StreamReader(stream))
          {
            tBLog.Text = reader.ReadToEnd();
            tBLog.ScrollToEnd();
          }
        }
      }
      catch (Exception e)
      {
        Logger.LogException("Error loading log file:", e);
      }
    }

    #endregion

    private FileSystemWatcher mWatcher;
  }
}
