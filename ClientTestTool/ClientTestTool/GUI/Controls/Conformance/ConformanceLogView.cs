///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Conformance;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Interfaces;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.GUI.Controls.Conformance
{
  public partial class ConformanceLogView : UserControl, IView
  {
    public ConformanceLogView()
    {
      InitializeComponent();
    }

    public new void Refresh()
    {
      if (NetworkTraceSet.IsEmpty)
        return;

      if (!TestCaseSet.Instance.IsTestingDone)
        return;

      this.InvokeIfRequired(() =>
      {
        tBLog.Clear();

        foreach (Profile profile in Enum.GetValues(typeof(Profile)))
        {
          tBLog.AppendLine("Profile {0}:", profile);

          if (!ConformanceLog.Instance.Errors[profile].Any() && !ConformanceLog.Instance.Warnings[profile].Any())
          {
            tBLog.AppendLine("    No issues found");
            tBLog.AppendLine();
            continue;
          }

          foreach (var error in ConformanceLog.Instance.Errors[profile])
          {
            tBLog.AppendLine("    Error:            {0}", error);
          }

          foreach (var warning in ConformanceLog.Instance.Warnings[profile])
          {
            tBLog.AppendLine("    Warning:      {0}", warning);
          }

          tBLog.AppendLine();
        }
      });
    }

    public void Clear()
    {
      tBLog.Clear();
    }
  }
}
