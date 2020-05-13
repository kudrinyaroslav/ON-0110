///
/// @Author Matthew Tuusberg
///

﻿using System.Windows.Forms;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.GUI.Interfaces
{
  interface ITestsTree
  {
    void ClearTestResults();
    void HighlightActiveTest(TestInfo testInfo);
    void HighlightCompletedTest(TestResult testResult);
    void CollapseNotSupportedGroups();

    TreeView TreeView
    {
      get;
    }

    TestInfo SelectedTest
    {
      get;
    }
  }
}
