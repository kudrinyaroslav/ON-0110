///
/// @Author Matthew Tuusberg
///

﻿using System.Drawing;
using System.Windows.Forms;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.Base;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView
{
  public sealed partial class FeaturesTree : BaseFeaturesTree
  {
    public FeaturesTree()
    {
      InitializeComponent();

      TreeView = tVFeatures;

      mCompletedTestFont = new Font(tVFeatures.Font, FontStyle.Bold);

      Build();
    }

    public override void ClearTestResults()
    {
      ClearTestResults(tVFeatures);
    }

    public override void HighlightCompletedTest(TestResult testResult)
    {
      HighlightCompletedTest(tVFeatures, testResult);
    }

    #region Tree Building

    protected override void Build()
    {
      mChildNodes.Clear();

      foreach (FeatureNode node in FeatureSet.Instance)
        AddFeatureNode(null, node);

      TreeNode tn = tVFeatures.Nodes[0];
      tn.EnsureVisible();
    }

    /// <summary>
    /// Adds feature node with subnodes.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="node"></param>
    private void AddFeatureNode(TreeNode parent, FeatureNode node)
    {
      AddFeatureNode(tVFeatures, parent, node);
    }

    #endregion
  }
}
