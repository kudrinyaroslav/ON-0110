///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Windows.Forms;
using ClientTestTool.Data.Definitions.Worker;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Data.Global.SingletonInitializer;
using ClientTestTool.GUI.Logging;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Forms
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    protected bool mIsListViewResizing = false;

    #region Events

    #endregion

    #region EventHandlers

    private void MainForm_Load(object sender, EventArgs e)
    {
      Show();
#if(DEBUG)
      Properties.Settings.Default.DebugMode    = true;
      Properties.Settings.Default.DeleteOutput = false;
#else
      Properties.Settings.Default.DebugMode    = false;
      //Properties.Settings.Default.DeleteOutput = true;
#endif
      SingletonInitializer.Initialize();
      Logger.WriteLine(String.Format("Client Test Tool v.{0} has successfully loaded", Properties.Settings.Default.Version));
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = false; // to prevent validation 

      if (Properties.Settings.Default.DeleteOutput)
        DeleteOutputFolder();

      Logger.WriteLine("Application is closing");
    }

    private void tCMain_Selecting(object sender, TabControlCancelEventArgs e)
    {
      if (UnitSet.GetClients().Count <= 1)
        return;
      
      if (2 == e.TabPageIndex || 3 == e.TabPageIndex)
      {
        DialogHelper.ShowError("Please select only one client for Conformance testing.\r\n" +
                               "If you need to consider identified clients as a single system, please follow these steps:\r\n" +
                               "1. Select multiple clients in the list using either Shift or Ctrl\r\n" +
                               "2. Right click on selection and select Merge option");
        e.Cancel = true;
      }
    }

    private void tCMain_SelectedIndexChanged(object sender, EventArgs e)
    {
      ControlHelper.ResizeListViews(this);
    }

    private void MainForm_SizeChanged(object sender, EventArgs e)
    {
      ControlHelper.ResizeListViews(this);
      ControlHelper.ResizeGridViews(this);
    }

    #region Menu

    private void tSMenuItemExit_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogHelper.ShowNotImplementedMsg();
    }

    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogHelper.ShowNotImplementedMsg();
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new AboutForm().ShowDialog();
    }

    private void userManualStripMenuItem_Click(object sender, EventArgs e)
    {
      Help.ShowHelp(this, CTTSettings.GetUserManualFilename());
    }

    private void addNetworkTraceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (configurationPage.LoadNetworkTrace())
        tCMain.SelectTab(0);
    }

    private void showLogToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var logForm = new LogForm();
      logForm.Show();
    }

    #endregion

    #endregion

    #region Helpers

    private void DeleteOutputFolder()
    {
      try
      {
        String outputDir = CTTSettings.GetOutputDir();
        if (Directory.Exists(outputDir))
        {
          String[] directories = Directory.GetDirectories(outputDir);
          foreach (String dir in directories)
            Directory.Delete(dir, true);
        }
      }
      catch (IOException e)
      {
        Logger.WriteLine(e.ToString());
      }
    }

    #endregion

  }
}
