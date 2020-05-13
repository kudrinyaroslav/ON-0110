///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.FeatureSet.Interfaces;
using ClientTestTool.Tests.Definitions.FeatureSet.Node.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.FeatureSet.Node
{
  class TestNode : BaseNode
  {
    public TestNode(TestInfo info, NodeType type, INode parent) : base(type, parent)
    {
      Info = info;
    }

    public TestInfo Info
    {
      get;
      private set;
    }
  }
}
