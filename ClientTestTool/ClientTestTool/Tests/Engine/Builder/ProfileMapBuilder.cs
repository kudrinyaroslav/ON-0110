///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Interfaces;

namespace ClientTestTool.Tests.Engine.Builder
{
  internal class ProfileMapBuilder : IBuilder<Dictionary<Profile, List<FeatureNode>>>
  {
    internal ProfileMapBuilder(List<FeatureNode> plainFeatures)
    {
      mPlainFeatures = plainFeatures;
    }

    private readonly List<FeatureNode> mPlainFeatures;

    public Dictionary<Profile, List<FeatureNode>> Build()
    {
      return Enum.GetValues(typeof (Profile)).Cast<Profile>().ToDictionary(profile => profile, GetProfileNodes);
    }

    private List<FeatureNode> GetProfileNodes(Profile profile)
    {
      var result = new List<FeatureNode>();

      foreach (var parentNode in mPlainFeatures)
      {
        var node = new FeatureNode(parentNode);
        node.Nodes.AddRange(parentNode.Nodes.Where(item => ((FeatureNode)item).Info.Requirement[profile] != RequirementLevel.None)
                                            .Select(item => new FeatureNode(item as FeatureNode)));

        if (node.Nodes.Count > 0)
          result.Add(node);
      }

      return result;
    }
  }
}
