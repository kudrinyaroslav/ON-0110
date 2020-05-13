///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Tests.Definitions.FeatureSet.Node.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.FeatureSet.Node
{
  class ProfileNode : BaseNode
  {
    public ProfileNode(Profile profile, NodeType type) : base(type, null)
    {
      Profile = profile;
    }

    public Profile Profile
    {
      get;
      private set;
    }
  }
}
