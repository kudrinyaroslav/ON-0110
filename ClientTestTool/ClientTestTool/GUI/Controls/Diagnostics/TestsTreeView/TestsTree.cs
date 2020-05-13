///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.Base;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView
{
  public sealed partial class TestsTree : BaseTree
  {
    public TestsTree()
    {
      InitializeComponent();

      TreeView = tVTestCases;

      mGroupNodes        = new List<TreeNode>();
      mCompletedTestFont = new Font(tVTestCases.Font, FontStyle.Bold);
      Build();
    }

    #region Tree Building

    protected override void Build()
    {
      foreach (TestInfo testInfo in TestCaseSet.Instance.Tests.OrderedList())
      {
        TreeNode groupNode = FindGroupNode(testInfo.Path);

        AddTestNode(testInfo, groupNode);
      }
    }

    #endregion

    #region Logic

    public override void ClearTestResults()
    {
      ClearTestResults(tVTestCases);
    }

    public void ExpandAll()
    {
      tVTestCases.ExpandAll();
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Finds node representing tests group with path specified. If a node does not exist, node 
    /// is created.
    /// </summary>
    /// <param name="path">Group path.</param>
    /// <returns>Old or newly created node.</returns>
    private TreeNode FindGroupNode(String path)
    {
      String[] parts  = path.Split(PATH_DELIMETER);

      if (0 == parts.Length)
        throw new FormatException();

      String rootName = parts[0];

      TreeNode rootNode = tVTestCases.Nodes.Cast<TreeNode>().FirstOrDefault(root => root.Name == rootName);

      // if root node not found - create root node.
      if (null == rootNode)
      {
        rootNode                  = tVTestCases.Nodes.Add(rootName);
        rootNode.Name             = rootName;
        rootNode.ImageKey         = IMG_GROUP;
        rootNode.SelectedImageKey = rootNode.ImageKey;

        mGroupNodes.Add(rootNode);
      }

      if (1 == parts.Length) // If test is under root group
        return rootNode;
 
      TreeNode currentNode = rootNode;
      String   currentPath = rootName;

      for (int i = 1; i < parts.Length; ++i)
      {
        String groupName = parts[i];
        currentPath += PATH_DELIMETER + groupName;

        TreeNode nextNode = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(node => node.Name == groupName);

        // if child not found, create new node.
        if (null == nextNode)
        {
          nextNode                  = currentNode.Nodes.Add(groupName);
          nextNode.ImageKey         = IMG_GROUP;
          nextNode.SelectedImageKey = nextNode.ImageKey;
          mGroupNodes.Add(nextNode);

          nextNode.Name = groupName;
        }

        currentNode = nextNode;
      }

      return currentNode;
    }

    #endregion

    private const char PATH_DELIMETER = '\\';

  }
}

