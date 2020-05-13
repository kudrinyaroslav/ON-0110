///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.Base;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Definitions.Log.Test;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView
{
  public sealed partial class ProfilesTree : BaseFeaturesTree
  {
    public ProfilesTree()
    {
      InitializeComponent();

      TreeView = tVProfiles;

      mProfileNodes      = new Dictionary<Profile, TreeNode>();
      mCompletedTestFont = new Font(tVProfiles.Font, FontStyle.Bold);

      Build();
    }

    protected override void Build()
    {
      foreach (Profile profile in Enum.GetValues(typeof (Profile)))
      {
        TreeNode profileNode = new TreeNode(String.Format("Profile {0}", profile));
        ResetProfileNode(profile, profileNode);

        mProfileNodes.Add(profile, profileNode);

        tVProfiles.Nodes.Add(profileNode);

        var featuresSet = profile.GetFeatureSet();

        foreach (FeatureNode node in featuresSet)
          AddFeatureNode(profileNode, node, profile);
      }
    }

    /// <summary>
    /// Adds feature node with subnodes.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="node"></param>
    private void AddFeatureNode(TreeNode parent, FeatureNode node, Profile profile)
    {
      AddFeatureNode(tVProfiles, parent, node, profile);
    }

    public override void ClearTestResults()
    {
      ClearTestResults(tVProfiles);
    }

    protected override void ClearTestResults(TreeView treeView)
    {
      foreach (var pair in mProfileNodes)
          ResetProfileNode(pair.Key, pair.Value);

      base.ClearTestResults(treeView);
    }

    private void ResetProfileNode(Profile profile, TreeNode node)
    {
      SetupProfileNode(profile, node, TestStatus.NotDetected);
      node.ForeColor = DefaultForeColor;
    }

    public override void HighlightCompletedTest(TestResult testResult)
    {
      HighlightCompletedTest(tVProfiles, testResult);

      foreach (var profile in mProfileNodes.Keys)
        HighlightProfile(profile);
    }

    private void HighlightProfile(Profile profile) //TODO slowy
    {
      this.InvokeIfRequired((() =>
      {
        var status = profile.IsSupported() ? TestStatus.Passed : TestStatus.Failed;
        SetupProfileNode(profile, mProfileNodes[profile], status);
      }));
    }

    private void SetupProfileNode(Profile profile, TreeNode node, TestStatus status)
    {
      node.ToolTipText      = TooltipHelper.CreateProfileTooltip(profile);
      node.ForeColor        = ConverterUtil.GetStatusColor(status);
      node.ImageKey         = GetImageKey(status.ToFeatureStatus());
      node.SelectedImageKey = node.ImageKey;
    }

    protected void AddFeatureNode(TreeView treeView, TreeNode profileNode, FeatureNode featureNode, Profile profile)
    {
      var parentNode = CreateTreeNode(featureNode);

      AddParentNode(featureNode.Info, parentNode);

      if (null != profileNode)
        profileNode.Nodes.Add(parentNode);
      else
        treeView.Nodes.Add(parentNode);

      foreach (var childFeatureNode in featureNode.Nodes.Cast<FeatureNode>())
      {
        var treeNode = CreateTreeNode(childFeatureNode);
        AddChildNode(childFeatureNode.Info, treeNode);
        AddTestNode(profile, childFeatureNode.Feature.GetDependingTest(), treeNode);

        parentNode.Nodes.Add(treeNode);
      }
    }

    private void AddTestNode(Profile profile, TestInfo testInfo, TreeNode parentNode)
    {
      RequirementLevel requirementLevel = testInfo.FeatureUnderTest.GetInfo().Requirement[profile];

      TreeNode node = parentNode.Nodes.Add(testInfo.GetNameString());

      node.ToolTipText      = TooltipHelper.CreateTestTooltip(testInfo, null);
      node.ImageKey         = GetTestImage(requirementLevel);
      node.SelectedImageKey = node.ImageKey;
      node.Tag              = testInfo;

      if (!mTestNodes.ContainsKey(testInfo))
        mTestNodes.Add(testInfo, new List<TreeNode> { node });
      else
        mTestNodes[testInfo].Add(node);
    }

    private readonly Dictionary<Profile, TreeNode> mProfileNodes;
  }
}
