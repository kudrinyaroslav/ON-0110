///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Utils;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Engine
{
  internal static class FeatureHelper
  {
    public static IEnumerable<Feature> GetFeaturesWithType(FeatureType type)
    {
      return Enum.GetValues(typeof(Feature)).Cast<Feature>()
                                             .Where(feature => feature != Feature.Unknown &&
                                                    feature.GetAttribute<FeatureAttribute>().Type == type)
                                             .OrderBy(item => (int)item).ToList();
    }

    public static IEnumerable<Feature> GetChildFeatures(Feature parentFeature)
    {
      return GetFeaturesWithType(FeatureType.Child).Where(feature => GetParentFeature(feature) == parentFeature).ToList(); // TODO
    }

    public static Feature GetParentFeature(Feature feature)
    {
      var attribute = feature.GetAttribute<FeatureAttribute>();

      if (null == attribute)
        return Feature.Unknown;

      return attribute.ParentFeature;
    }
  }
}
