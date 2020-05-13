///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Windows.Forms;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Controls.Base
{
  public class BasePage : UserControl //TODO add abstract when GUI is done
  {
    protected BasePage()
    {
      
    }

    protected virtual void HookUI() //TODO add abstract when GUI is done
    {
      
    }

    protected virtual void OnVisibleChanged(object sender, EventArgs e)
    {
      if (!Visible)
        return;

      HookUI();
    }

    protected void ResizeGridViews()
    {
      ControlHelper.ResizeGridViews(this);
    }

    protected void ResizeListViews()
    {
      ControlHelper.ResizeListViews(this);
    }
  }
}
