///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Interfaces;

namespace ClientTestTool.Tests.Engine.Builder
{
  internal class FeatureNodeListBuilder : IBuilder<List<FeatureNode>>
  {
    public List<FeatureNode> Build()
    {
      var result = new List<FeatureNode>();

      var parentFeatures = FeatureHelper.GetFeaturesWithType(FeatureType.Parent);

      foreach (var parentFeature in parentFeatures)
      {
        var parentInfo = FeatureInfo.Builder.Build(parentFeature, FeatureType.Parent);
        var parentNode = new FeatureNode(parentInfo);

        foreach (var childFeature in FeatureHelper.GetChildFeatures(parentFeature))
        {
          var childInfo = FeatureInfo.Builder.Build(childFeature, FeatureType.Child);
          var childNode = new FeatureNode(childInfo, parentNode);

          parentNode.Nodes.Add(childNode);
        }

        result.Add(parentNode);
      }

      return result;
    }

  }
}
