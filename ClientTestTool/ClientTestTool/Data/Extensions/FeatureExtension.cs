///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Utils;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Extensions
{
  public static class FeatureExtension
  {
    public static bool IsSupported(this Feature feature)
    {
      return FeatureSet.Instance.GetInfo(feature).Status == FeatureStatus.Supported;
    }

    public static FeatureInfo GetInfo(this Feature feature)
    {
      return FeatureSet.Instance.GetInfo(feature);
    }

    public static String GetDisplayName(this Feature feature)
    {
      return feature.GetDescription();
    }

    public static FeatureType GetFeatureType(this Feature feature)
    {
      return feature.GetInfo().Type;
    }

    public static IList<Feature> GetChildFeatures(this Feature feature)
    {
      return FeatureSet.Instance.First(item => item.Feature == feature)
                                .Nodes.Select(item => ((FeatureNode) item).Feature)
                                .ToList();
    }

    public static RequirementLevel GetRequirementLevel(this Feature feature, Profile profile)
    {
      var attributes = feature.GetAttributes<ProfileAttribute>();

      if (0 == attributes.Count)
        return RequirementLevel.Optional;

      var profileAttribute = attributes.FirstOrDefault(item => profile == item.Profile);

      if (null == profileAttribute)
        return RequirementLevel.None;

      return profileAttribute.RequirementLevel;
    }
    
    public static Feature GetParentFeature(this Feature feature)
    {
      var attribute = feature.GetAttribute<FeatureAttribute>();

      if (null == attribute)
        return Feature.Unknown;

      return attribute.ParentFeature;
    }

    public static String GetFullName(this Feature feature)
    {
      return String.Format("{0}_{1}", feature.GetParentFeature(), feature);
    }

    /// <summary>
    /// Can be used only with a Child Feature
    /// </summary>
    /// <param name="feature"></param>
    /// <returns></returns>
    public static TestInfo GetDependingTest(this Feature feature)
    {
      if (FeatureType.Parent == feature.GetFeatureType())
        throw new ArgumentException("feature");

      return TestCaseSet.Instance.Tests.First(item => item.FeatureUnderTest == feature);
    }
  }
}
