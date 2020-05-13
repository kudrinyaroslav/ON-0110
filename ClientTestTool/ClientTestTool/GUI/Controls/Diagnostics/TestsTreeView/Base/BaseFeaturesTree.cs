///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Definitions.Log.Test;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.Base
{
  public abstract class BaseFeaturesTree : BaseTree
  {
    protected BaseFeaturesTree()
    {
      mGroupNodes  = new List<TreeNode>();
      mChildNodes  = new Dictionary<FeatureInfo, List<TreeNode>>();
      mParentNodes = new Dictionary<FeatureInfo, List<TreeNode>>();
    }

    protected virtual void ClearTestResults(TreeView treeView)
    {
      BeginInvoke(new Action(() =>
      {
        foreach (var pair in mChildNodes.Union(mParentNodes))
          foreach (TreeNode node in pair.Value)
            ResetFeatureNode(pair.Key, node);
      }));

      base.ClearTestResults(treeView);
    }

    #region Tree Building

    protected void AddFeatureNode(TreeView treeView, TreeNode node, FeatureNode featureNode)
    {
      var parentNode = CreateTreeNode(featureNode);

      AddParentNode(featureNode.Info, parentNode);

      if (null != node)
        node.Nodes.Add(parentNode);
      else
        treeView.Nodes.Add(parentNode);

      foreach (var childFeatureNode in featureNode.Nodes.Cast<FeatureNode>())
      {
        var treeNode = CreateTreeNode(childFeatureNode);
        AddChildNode(childFeatureNode.Info, treeNode);
        AddTestNode(childFeatureNode.Feature.GetDependingTest(), treeNode);
        parentNode.Nodes.Add(treeNode);
      }
    }

    protected TreeNode CreateTreeNode(FeatureNode node)
    {
      var treeNode = new TreeNode(node.Info.Name)
      {
        Name = node.Info.Name
      };

      ResetFeatureNode(node.Info, treeNode);

      return treeNode;
    }

    protected void AddParentNode(FeatureInfo parentFeature, TreeNode treeNode)
    {
      if (FeatureType.Parent != parentFeature.Type)
        throw new ArgumentException();

      if (!mParentNodes.ContainsKey(parentFeature))
        mParentNodes.Add(parentFeature, new List<TreeNode> { treeNode });
      else
        mParentNodes[parentFeature].Add(treeNode);

      mGroupNodes.Add(treeNode);
    }

    protected void AddChildNode(FeatureInfo childFeature, TreeNode treeNode)
    {
      if (!mChildNodes.ContainsKey(childFeature))
        mChildNodes.Add(childFeature, new List<TreeNode> { treeNode });
      else
        mChildNodes[childFeature].Add(treeNode);
    }

    #endregion

    #region Highlighting

    /// <summary>
    /// Selects tests which currently is being executed.
    /// </summary>
    /// <param name="testInfo">Test information.</param>
    public override void HighlightActiveTest(TestInfo testInfo)
    {
    }

    public virtual void HighlightCompletedTest(TreeView treeView, TestResult testResult)
    {
      base.HighlightCompletedTest(testResult);

      var featureInfo = testResult.TestInfo.FeatureUnderTest.GetInfo();
      HighlightFeature(featureInfo, featureInfo.Status);

      var parentFeatureInfo = mParentNodes.Keys.First(item => item.Feature == featureInfo.Feature.GetParentFeature());
      HighlightParentFeature(parentFeatureInfo, parentFeatureInfo.Status);
    }

    private void HighlightParentFeature(FeatureInfo featureInfo, FeatureStatus status)
    {
      this.InvokeIfRequired((() =>
      {
        if (!mParentNodes.ContainsKey(featureInfo))
          return;

        foreach (var node in mParentNodes[featureInfo])
          RefreshFeatureNode(featureInfo, node, status);
      }));
    }

    private void HighlightFeature(FeatureInfo featureInfo, FeatureStatus status)
    {
      this.InvokeIfRequired((() =>
      {
        if (!mChildNodes.ContainsKey(featureInfo))
          return;

        foreach (var node in mChildNodes[featureInfo])
          RefreshFeatureNode(featureInfo, node, status);
      }));
    }

    #endregion

    #region Helpers

    protected void ResetFeatureNode(FeatureInfo featureInfo, TreeNode node)
    {
      RefreshFeatureNode(featureInfo, node, FeatureStatus.Undefined);
      node.ForeColor = DefaultForeColor;
    }

    protected void RefreshFeatureNode(FeatureInfo feature, TreeNode node, FeatureStatus status)
    {
      node.ToolTipText      = TooltipHelper.CreateFeatureTooltip(feature);
      node.ForeColor        = ConverterUtil.GetStatusColor(status);
      node.ImageKey         = GetImageKey(status);
      node.SelectedImageKey = node.ImageKey;
    }

    protected String GetImageKey(FeatureStatus status)
    {
      switch (status)
      {
        case FeatureStatus.Supported:
          return IMG_SUPPORTED;

        case FeatureStatus.NotSupported:
          return IMG_NOTSUPPORTED;

        case FeatureStatus.Undefined:
          return IMG_UNDEFINED;

        default:
          throw new ArgumentOutOfRangeException("status");
      }
    }

    #endregion

    protected readonly Dictionary<FeatureInfo, List<TreeNode>> mParentNodes;
    protected readonly Dictionary<FeatureInfo, List<TreeNode>> mChildNodes;
  }
}
