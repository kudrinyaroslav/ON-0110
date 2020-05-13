///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Windows.Forms;
using ClientTestTool.Data.Utils;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.Tests.Definitions.Data;

namespace ClientTestTool.GUI.Controls.Diagnostics
{
  public partial class TestOutputView : UserControl
  {
    public TestOutputView()
    {
      InitializeComponent();
    }

    #region Event Handlers

    private void TestOutputView_Load(object sender, EventArgs e)
    {
      mOutputHighlighter = new TextHighlighter(tBOutput);
    }

    private void btnSearchNext_Click(object sender, EventArgs e)
    {
      mOutputHighlighter.SearchPattern = tBFind.Text;
      mIsScrollingEnabled = false;
      mOutputHighlighter.HighlightNext();
    }

    private void btnSearchPrev_Click(object sender, EventArgs e)
    {
      mOutputHighlighter.SearchPattern = tBFind.Text;
      mIsScrollingEnabled = false;
      mOutputHighlighter.HighlightPrev();
    }

    #endregion

    #region Properties

    

    #endregion

    #region Helpers

    public void Clear()
    {
      tBOutput.Clear();
      mIsScrollingEnabled = true;
    }

    public void AppendLine()
    {
      AppendLine(String.Empty);
    }

    public void AppendLine(String text)
    {
      tBOutput.AppendLine(text);

      if (mIsScrollingEnabled)
        tBOutput.ScrollToEnd();
    }

    public void AppendText(String text)
    {
      tBOutput.AppendText(text);

      if (mIsScrollingEnabled)
        tBOutput.ScrollToEnd();
    }

    public void SelectTestOutput(TestInfo info)
    {
      if (null == info)
        throw new ArgumentNullException("info");

      String testName   = info.GetNameString();
      String testStatus = info.Status.GetDescription();

      int firstIndex = tBOutput.Text.IndexOf    (testName, StringComparison.Ordinal);
      int lastIndex  = tBOutput.Text.LastIndexOf(testName, StringComparison.Ordinal);

      if (-1 != lastIndex)
        lastIndex += testName.Length + testStatus.Length + 1;

      tBOutput.ClearSelection();
      tBOutput.ScrollToChar(firstIndex);
      tBOutput.HighlightText(firstIndex, lastIndex - firstIndex);

      mIsScrollingEnabled = false;
    }

    #endregion

    private TextHighlighter mOutputHighlighter;
    private bool            mIsScrollingEnabled = true;

    private void tBOutput_Leave(object sender, EventArgs e)
    {
      if (!mIsScrollingEnabled)
        mIsScrollingEnabled = true;
    }
  }
}
