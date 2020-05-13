///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using ClientTestTool.Tests.Definitions.FeatureSet.Interfaces;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.FeatureSet.Node.Base
{
  public abstract class BaseNode : INode
  {
    protected BaseNode(NodeType type, INode parent = null)
    {
      Type   = type;
      Parent = parent;
      Nodes  = new List<INode>();
    }

    public NodeType Type
    {
      get;
      private set;
    }

    #region INode Implementation

    public INode Parent
    {
      get;
      protected set;
    }

    public List<INode> Nodes
    {
      get;
      private set;
    }

    #endregion

  }
}
