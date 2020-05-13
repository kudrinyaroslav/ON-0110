///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Conformance.DoC;
using ClientTestTool.Data.Conformance.FeatureList;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Configuration;
using ClientTestTool.Data.Global.Conformance;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.GUI.Controls.Base;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Forms;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Properties;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.GUI.Controls.Conformance
{
  public partial class PageConformance : BaseView // TODO cleanup
  {
    public PageConformance()
    {
      InitializeComponent();
    }

    #region EventHandlers

    private void ConformancePage_Load(object sender, EventArgs e)
    {
      conformanceLogView.Refresh();
      
      ConformanceRuleChecker.Instance.OnWorkCompleted += (o, args) => this.InvokeIfRequired(() =>
      {
        btnGenerateFeatureList.Enabled = CheckIfFeatureListAvailable();
        btnGenetateDoC.Enabled         = CheckIfDoCAvailable();
      });

      ConformanceLog.Instance.OnMessageAdded += (o, args) => this.InvokeIfRequired(() => conformanceLogView.Refresh());

      CTTConfiguration.OnConfigurationChanged += (o, args) => this.InvokeIfRequired(() =>
      {
        conformanceInfoView.ClientUnderTest = null;
        conformanceInfoView.Clear();
        conformanceLogView.Clear();
      });
    }

    /// <summary>
    /// Handles the Click event of the btnGenerateDoC control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <exception cref="System.ApplicationException">This button must be disabled</exception>
    private void btnGenerateDoC_Click(object sender, EventArgs e)
    {
      if (ConversationList.IsEmpty)
        throw new ApplicationException("This button must be disabled");

      if (!conformanceInfoView.IsInputValid)
      {
        DialogHelper.ShowError(Resources.Message_Some_mandatory_fields_are_invalid);
        return;
      }

      if (mIsErrataModeEnabled)
        ShowErrataDialog();
      else
        ShowDoCDialog();

      ShowFeatureListDialog();
    }

    private void btnGenerateFeatureList_Click(object sender, EventArgs e)
    {
      if (null == UnitSet.GetClient())
      {
        DialogHelper.ShowMessage("You must have client to generate Feature List xml");
        return;
      }

      if (!conformanceInfoView.IsInputValid)
      {
        DialogHelper.ShowError(Resources.Message_Some_mandatory_fields_are_invalid);
        return;
      }

      ShowFeatureListDialog();
    }

    #endregion

    #region Helpers

    protected override void HookUI()
    {
      conformanceInfoView.ClientUnderTest = UnitSet.GetClient();

      conformanceInfoView.ResetBindings();

      btnGenetateDoC.Enabled         = CheckIfDoCAvailable();
      btnGenerateFeatureList.Enabled = CheckIfFeatureListAvailable();

      conformanceLogView.Clear();
      conformanceLogView.Refresh();
    }

    private void ShowFeatureListDialog()
    {
      sFDConformance.FileName = conformanceInfoView.Info.GetFeatureListFilename();
      sFDConformance.DefaultExt = Path.GetExtension(sFDConformance.FileName);
      DialogResult dResult = sFDConformance.ShowDialog();

      if (DialogResult.OK != dResult)
        return;

      String filename = sFDConformance.FileName;

      new FeatureListGenerator(conformanceInfoView.Info).Generate(filename);
    }

    private void ShowErrataDialog()
    {
      Form errataForm = new ErrataForm(conformanceInfoView.Info)
      {
        ShowInTaskbar = false
      };

      errataForm.ShowDialog();
    }

    private void ShowDoCDialog()
    {
      sFDConformance.FileName = conformanceInfoView.Info.GetDoCFilename();
      sFDConformance.DefaultExt = Path.GetExtension(sFDConformance.FileName);
      DialogResult dResult = sFDConformance.ShowDialog();

      if (DialogResult.OK != dResult)
        return;

      String filename = sFDConformance.FileName;

      var generator = new DoCGenerator(conformanceInfoView.Info, CTTSettings.GetDoCTemplateFilename());
      generator.Generate(filename);
    }

    private bool CheckIfDoCAvailable()
    {
      mIsErrataModeEnabled = ConformanceLog.Instance.Errors.Values.Any(item => item.Any());
      btnGenetateDoC.Text = mIsErrataModeEnabled ? DOC_ERRATA_BTN_CAPTION : DOC_BTN_CAPTION;

      bool isTestingDone = TestCaseSet.Instance.IsTestingDone;

      var deviceSupportedProfiles = UnitSet.GetSupportedProfiles();
      var clientSupportedProfiles = Enum.GetValues(typeof (Profile)).Cast<Profile>().Where(profile => profile.IsSupported());

      return isTestingDone && (clientSupportedProfiles.Any() || deviceSupportedProfiles.Any());
    }

    private bool CheckIfFeatureListAvailable()
    {
      bool isTestingDone = TestCaseSet.Instance.IsTestingDone;
      return isTestingDone;
    }

    #endregion

    private bool mIsErrataModeEnabled = true;

    private const String DOC_BTN_CAPTION        = "Generate DoC";
    private const String DOC_ERRATA_BTN_CAPTION = "Generate DoC With Errata";
  }
}

