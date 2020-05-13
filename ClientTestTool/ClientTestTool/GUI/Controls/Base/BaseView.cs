///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Windows.Forms;
using ClientTestTool.Data.Definitions.Worker;
using ClientTestTool.GUI.Interfaces;
using ClientTestTool.GUI.Logging;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Controls.Base
{
  public class BaseView : UserControl, IView //TODO add abstract when GUI is done
  {
    protected BaseView()
    {
      
    }

    protected virtual void HookUI() //TODO add abstract when GUI is done
    {
      
    }

    protected virtual void OnConfigurationChanged() //TODO add abstract when GUI is done
    {
      
    }

    public virtual void Clear() //TODO add abstract when GUI is done
    {
      
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
      if (!Visible)
        return;

      HookUI();
    }

    protected void OnProgressChanged(object sender, EventArgs e)
    {
      var workerSender = sender as Worker;

      if (null == workerSender)
        return;

      int progress = workerSender.Progress;
      ApplicationStatus.SetProgress(progress);
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
