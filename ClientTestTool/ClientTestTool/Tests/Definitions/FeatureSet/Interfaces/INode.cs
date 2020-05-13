///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;

namespace ClientTestTool.Tests.Definitions.FeatureSet.Interfaces
{
  public interface INode
  {
    INode Parent
    {
      get;
    }

    List<INode> Nodes
    {
      get;
    }
  }
}
