///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using ClientTestTool.Data.Global.Settings;

namespace ClientTestTool.GUI.Forms
{
  public partial class AboutForm : Form
  {
    public AboutForm()
    {
      InitializeComponent();
    }

    #region Event Handlers

    private void AboutForm_Load(object sender, EventArgs e)
    {
      lblVersion.Text = String.Format("Version: {0}", Properties.Settings.Default.Version);
    }

    private void lblONVIFLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://www.onvif.org/");
    }

    private void lblEULA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(CTTSettings.GetLicenseFilename());
    }

    #endregion
  }
}
