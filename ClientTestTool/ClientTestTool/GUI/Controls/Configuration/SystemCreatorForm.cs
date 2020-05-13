///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Global;

namespace ClientTestTool.GUI.Controls.Configuration
{
  public partial class SystemCreatorForm : Form
  {
    public SystemCreatorForm(IEnumerable<Client> clients)
    {
      InitializeComponent();
      mClients = clients.ToList();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      OnEnter();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void tBSystemName_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData == Keys.Return)
        OnEnter();
    }

    private void OnEnter()
    {
      String systemName = tBSystemName.Text.Trim();
      UnitSet.Merge(systemName, mClients);
      Close();
    }

    private readonly List<Client> mClients;
  }
}
