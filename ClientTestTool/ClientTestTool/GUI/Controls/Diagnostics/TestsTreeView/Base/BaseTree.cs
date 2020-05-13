///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Interfaces;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Log.Test;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.Base
{
  public abstract class BaseTree : UserControl, ITestsTree
  {
    protected BaseTree()
    {
      mTestNodes     = new Dictionary<TestInfo, List<TreeNode>>();
      mColouredNodes = new List<TreeNode>();
    }

    #region Properties

    public TreeView TreeView
    {
      get;
      protected set;
    }

    public TestInfo SelectedTest
    {
      get;
      private set;
    }

    #endregion

    #region Events

    public static event TreeViewEventHandler       AfterSelect;
    public static event TreeViewCancelEventHandler BeforeSelect;

    #endregion

    #region Event Handlers

    protected void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      if (null != BeforeSelect)
        BeforeSelect(this, e);
    }

    protected void treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      SelectedTest = mTestNodes.Where(pair => null != pair.Value.FirstOrDefault(node => e.Node == node)).Select(item => item.Key).FirstOrDefault();

      if (null != AfterSelect)
        AfterSelect(this, e);
    }

    #endregion

    #region Abstract

    public    abstract void ClearTestResults();
    protected abstract void Build();

    #endregion

    #region Highlighting

    /// <summary>
    /// Selects tests which currently is being executed.
    /// </summary>
    /// <param name="testInfo">Test information.</param>
    public virtual void HighlightActiveTest(TestInfo testInfo)
    {
      bool isScrollingEnabled = null == SelectedTest;

      BeginInvoke(new Action(() =>
      {
        TreeView.BeginUpdate();

        if (mTestNodes.ContainsKey(testInfo))
          foreach (var node in mTestNodes[testInfo])
          {
            node.BackColor = Color.LightGray;
            node.EnsureVisibleWithoutScrolling(isScrollingEnabled);
          }

        TreeView.EndUpdate();
      }));
    }

    /// <summary>
    /// Highlightes completed test.
    /// </summary>
    public virtual void HighlightCompletedTest(TestResult testResult)
    {
      BeginInvoke(new Action(() =>
      {
        if (!mTestNodes.ContainsKey(testResult.TestInfo))
          return;

        TreeView.BeginUpdate();

        foreach (var node in mTestNodes[testResult.TestInfo])
        {
          node.BackColor   = TreeView.BackColor;
          node.NodeFont    = mCompletedTestFont;
          node.Text        = node.Text;
          node.ToolTipText = TooltipHelper.CreateTestTooltip(testResult.TestInfo, testResult);
          node.ForeColor   = ConverterUtil.GetStatusColor(testResult.TestStatus);

          mColouredNodes.Add(node);
        }

        TreeView.EndUpdate();
      }));
    }

    protected void ClearTestResults(TreeView treeView)
    {
      foreach (TreeNode node in mColouredNodes)
      {
        node.ToolTipText = TooltipHelper.CreateTestTooltip((TestInfo)node.Tag, null);
        node.ForeColor   = treeView.ForeColor;
        node.NodeFont    = treeView.Font;
      }

      foreach (var node in mGroupNodes)
        node.Collapse();

      mColouredNodes.Clear();

      SelectedTest = null;
    }

    public void CollapseNotSupportedGroups()
    {
      var parentSet = new HashSet<TreeNode>();

      foreach (var groupNode in mGroupNodes)
      {
        var parent = groupNode.Parent;

        if (null != parent)
          parentSet.Add(groupNode.Parent);

        bool isGray = groupNode.Nodes.Cast<TreeNode>()
          .All(node => node.ForeColor == TestStatus.NotDetected.GetColor());

        if (isGray)
          groupNode.Collapse();
      }

      foreach (var parentNode in parentSet)
      {
        bool isAllNodesCollapsed = parentNode.Nodes.Cast<TreeNode>().All(node => !node.IsExpanded);

        if (isAllNodesCollapsed)
          parentNode.Collapse();
      }

      if (null != SelectedTest)
        TreeView.SelectedNode.EnsureVisible();
    }

    #endregion

    #region Tree Building

    protected void AddTestNode(TestInfo testInfo, TreeNode parentNode)
    {
      TreeNode node = parentNode.Nodes.Add(testInfo.GetNameString());

      node.ToolTipText      = TooltipHelper.CreateTestTooltip(testInfo, null);
      node.ImageKey         = GetImageKey(testInfo, false);
      node.SelectedImageKey = node.ImageKey;
      node.Tag              = testInfo;

      if (!mTestNodes.ContainsKey(testInfo))
        mTestNodes.Add(testInfo, new List<TreeNode> { node });
      else
        mTestNodes[testInfo].Add(node);
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Method for finding image key for test node.
    /// </summary>
    /// <returns>Key of image in image list.</returns>
    protected String GetImageKey(TestInfo info, bool isCategoryAllowed)
    {
      String imageKey = "MUSTIF.ico";

      if (Category.Core != info.Category && isCategoryAllowed)
      {
        Profile profile = info.Category.GetProfile().Value; //not null

        var requirementLevel = info.FeatureUnderTest.GetInfo().Requirement[profile];
        imageKey = GetTestImage(requirementLevel);
      }

      return imageKey;
    }

    protected String GetTestImage(RequirementLevel requirementLevel)
    {
      switch (requirementLevel)
      {
        case RequirementLevel.Mandatory:
          return "MUST.ico";

        case RequirementLevel.Optional:
          return "OPTIONALIF.ico";

        case RequirementLevel.Conditional:
          return "OPTIONAL.ico";

        default:
          throw new ArgumentOutOfRangeException("requirementLevel");
      }
    }

    #endregion

    /// <summary>
    /// TreeView nodes representing groups of tests.
    /// </summary>
    protected List<TreeNode> mGroupNodes;
    protected List<TreeNode> mColouredNodes;

    /// <summary>
    /// Dictionary which allows find TreeView node by TestInfo structure
    /// </summary>
    protected readonly Dictionary<TestInfo, List<TreeNode>> mTestNodes;

    protected Font mCompletedTestFont;

    protected const String IMG_GROUP        = "Group.ico";
    protected const String IMG_CLEAR        = "Clear.ico";
    protected const String IMG_SUPPORTED    = "Supported.ico";
    protected const String IMG_NOTSUPPORTED = "NotSupported.ico";
    protected const String IMG_UNDEFINED    = "Undefined.ico";
  }
}
