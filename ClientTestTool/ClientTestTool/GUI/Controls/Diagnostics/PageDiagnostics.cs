///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Configuration;
using ClientTestTool.Data.Global.Conformance;
using ClientTestTool.Data.Utils;
using ClientTestTool.GUI.Controls.Base;
using ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.Base;
using ClientTestTool.GUI.Interfaces;
using ClientTestTool.GUI.Logging;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Properties;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Events;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.GUI.Controls.Diagnostics
{
  /// <summary>
  /// Diagnostics page
  /// </summary>
  public partial class PageDiagnostics : BaseView
  {
    #region Initialization

    /// <summary>
    /// ctor
    /// </summary>
    public PageDiagnostics()
    {
      InitializeComponent();

      LoadTests();

      BaseTree.AfterSelect += treeView_AfterSelect;

      TestCaseSet.Instance.TestExecutor.OnProgressChanged += OnProgressChanged;
      TestCaseSet.Instance.TestExecutor.OnWorkStarted     += OnTestingStarted;
      TestCaseSet.Instance.TestExecutor.OnWorkCompleted   += OnTestingCompleted;

      mSelectedTest = null;
    }

    private void OnTestingStarted(object sender, EventArgs eventArgs)
    {
      BeginInvoke(new Action(() => btnClear.Enabled = false));
      ApplicationStatus.SetProgress(0);
    }

    private void OnTestingCompleted(object sender, EventArgs eventArgs)
    {
      ApplicationStatus.SetProgress(100);
      ApplicationStatus.SetStatus("Done");

      BeginInvoke(new Action(() =>
      {
        mTestTrees.ToList().ForEach(item => item.CollapseNotSupportedGroups());
        btnClear.Enabled = true;
      }));
    }

    #endregion

    #region Test Preparation

    private void LoadTests()
    {
      foreach (var testInfo in TestCaseSet.Instance.Tests)
        PrepareTest(testInfo);
    }

    private void PrepareTest(TestInfo testInfo)
    {
      var testInstance = (BaseTest) testInfo.Test;

      testInstance.OnTestStarted        += (sender, args) => OnTestStarted(testInfo);
      testInstance.OnConversationTested += (sender, args) => OnConversationTested(args);
      testInstance.OnTestCompleted      += (sender, args) => OnTestCompleted(testInfo, args);
    }

    private void OnTestStarted(TestInfo testInfo)
    {
      this.InvokeIfRequired((() =>
      {
        var sb = new StringBuilder();

        String msg = String.Format("{0} has started", testInfo.GetNameString());

        ApplicationStatus.SetStatus(msg);

        sb.AppendLine(msg.ToUpper());
        sb.AppendLine();

        vTestOutput.AppendText(sb.ToString());

      }));

      HighlightActiveTest(testInfo);
    }

    private void OnConversationTested(ConversationLogEventArgs args)
    {
      this.InvokeIfRequired((() =>
      {
        var sb = new StringBuilder();

        sb.AppendLine(args.Log.Conversation.Name + ":");
        args.Log.Steps.ForEach(step => sb.AppendLine(String.Format("{0} {1}:{2}", step.Name, step.Message, step.Status.GetDescription().ToUpper())));

        vTestOutput.AppendText(sb.ToString());
        vTestOutput.AppendLine();
      }));
    }

    private void OnTestCompleted(TestInfo testInfo, TestCompletedEventArgs args)
    {
      this.InvokeIfRequired((() =>
      {
        var sb = new StringBuilder();

        String msg = String.Format("{0} {1}", testInfo.GetNameString(),
          args.Result.TestStatus.GetDescription());

        sb.AppendLine(msg.ToUpper());
        sb.AppendLine();

        vTestOutput.AppendText(sb.ToString());
      }));

      HighlightCompletedTest(args.Result);
    }

    private void HighlightActiveTest(TestInfo testInfo)
    {
      if (null == testInfo)
        throw new ArgumentNullException("testInfo");

      mTestTrees.ToList().ForEach(item => item.HighlightActiveTest(testInfo));
    }

    private void HighlightCompletedTest(TestResult testResult)
    {
      if (null == testResult)
        throw new ArgumentNullException("testResult");

      Logger.WriteLine(String.Format("{0}:{1}", testResult.TestInfo.GetNameString(), testResult.TestStatus));
      mTestTrees.ToList().ForEach(item => item.HighlightCompletedTest(testResult));
    }

    #endregion

    #region EventHandlers

    private void PageDiagnostics_Load(object sender, EventArgs e)
    {
      mTestTrees = new[]
      {
        (ITestsTree)testsTree, featuresTree, profilesTree
      };

      CTTConfiguration.OnConfigurationChanged += (o, args) => BeginInvoke(new Action(OnConfigurationChanged));

      btnRun.Enabled = false;

      ConversationList.Instance.OnConversationListValidated += (o, args) => BeginInvoke(new Action(() =>
      {
        btnRun.Enabled = true;
      }));
    }

    private void tCModes_SelectedIndexChanged(object sender, EventArgs e)
    {
      //btnRun.Enabled = btnRunSelected.Enabled = (0 == tCModes.SelectedIndex);
    }

    private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      vTestDetails.Clear();

      var activeTree = sender as ITestsTree;

      if (null == activeTree)
        throw new ArgumentException("", "sender");

      if (null == activeTree.SelectedTest)
        return;

      mSelectedTest = (BaseTest) activeTree.SelectedTest.Test;

      vTestDetails.SelectedTest = mSelectedTest;

      if (mSelectedTest.IsCompleted)
        vTestOutput.SelectTestOutput(mSelectedTest.TestInfo);
    }

    private void btnRunConformance_Click(object sender, EventArgs e)
    {
      if (ApplicationState.Idle != StateManager.GetState())
        return;

      if (ConversationList.IsEmpty)
      {
        DialogHelper.ShowError(Resources.Message_Diagnostics_ConversationList_Empty);
        return;
      }

      var devices = UnitSet.GetDevices();

      if (devices.Any(item => !item.IsFeatureListAttached))
      {
        var result = DialogHelper.ShowWarning(Resources.Message_Diagnostics_FeatureList_NotAttached);

        if (DialogResult.Cancel == result)
          return;
      }

      Clear();
      RunTests();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      if (ApplicationState.Idle == StateManager.GetState())
        Clear();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (ConversationList.IsEmpty)
        return;

      sFDDiagnosticsReport.FileName = String.Format("DiagnosticsReport_{0}.xml", DateTime.Now.ToShortTimeString().Replace(':', '_'));
      var result = sFDDiagnosticsReport.ShowDialog();

      if (DialogResult.OK != result)
        return;

      String filename = sFDDiagnosticsReport.FileName;

      TestCaseSet.SaveTestsResult(filename);
    }

    private void sCMainContainer_SplitterMoved(object sender, SplitterEventArgs e)
    {
      HookUI();
    }

    #endregion

    #region Helpers

    protected override void HookUI()
    {
      ResizeListViews();

      btnRun.Enabled = !ConversationList.IsEmpty &&ConversationList.Instance.Validated && null != UnitSet.GetClient();
    }

    public override void Clear()
    {
      vTestOutput.Clear();
      vTestDetails.Clear();

      mTestTrees.ToList().ForEach(item => item.ClearTestResults());

      TestCaseSet.Instance.ClearTestResults();
      FeatureSet.Instance.ClearTestResults();
    }

    protected override void OnConfigurationChanged()
    {
      Clear();
    }

    private async void RunTests()
    {
      vTestOutput.Clear();
      vTestDetails.Clear();

      await Task.Run(() => TestCaseSet.Instance.TestExecutor.Run()).ContinueWith(t => ConformanceRuleChecker.Instance.Run());
      
    }

    #endregion

    private ITestsTree[]    mTestTrees;
    private BaseTest        mSelectedTest;
  }
}
