///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.FeatureSet.Interfaces;
using ClientTestTool.Tests.Definitions.FeatureSet.Node.Base;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.FeatureSet.Node
{
  public class FeatureNode : BaseNode
  {
    public FeatureNode(FeatureNode node) : base (node.Type, node.Parent)
    {
      if (null == node)
        throw new ArgumentNullException("node");

      Info = node.Info;

      if (FeatureType.Child == Info.Type)
        Info.PropertyChanged += (sender, args) => CheckParentStatus();
    }

    public FeatureNode(FeatureInfo info, INode parent = null) : base(NodeType.Feature, parent)
    {
      Info = info;

      if (FeatureType.Child == Info.Type)
        Info.PropertyChanged += (sender, args) => CheckParentStatus();
    }

    #region Properties

    public FeatureInfo Info
    {
      get;
      private set;
    }

    public Feature Feature
    {
      get
      {
        return Info.Feature;
      }
    }

    #endregion

    #region Logic

    private void CheckParentStatus()
    {
      var parentNode = Parent as FeatureNode;

      if (null != parentNode)
        parentNode.Info.Status = ExpectedScenarioHandler.GetFeatureStatus(parentNode.Feature);
    }

    #endregion
  }
}
