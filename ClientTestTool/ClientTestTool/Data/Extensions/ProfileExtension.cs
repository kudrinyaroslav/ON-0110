///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.FeatureSet.Node;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Extensions
{
  public static class ProfileExtension
  {
    #region Logic

    public static bool IsSupported(this Profile profile)
    {
      var featureSet = profile.GetFeatureSet();

      featureSet.RemoveAll(item => item.Nodes.Cast<FeatureNode>().All(node => node.Info.Requirement[profile] == RequirementLevel.Conditional ||
                                                                              node.Info.Requirement[profile] == RequirementLevel.Optional));

      var parentFeatures = featureSet.Select(item => item.Feature).ToList();

      return parentFeatures.All(item => item.IsSupported());
    }

    #endregion

    #region Features

    /// <summary>
    /// Returns list of all child features 
    /// </summary>
    public static List<Feature> GetFeatures(this Profile profile)
    {
      return GetFeatures(profile, null);
    }

    public static List<Feature> GetMandatoryFeatures(this Profile profile)
    {
      return GetFeatures(profile, attribute => RequirementLevel.Mandatory == attribute.RequirementLevel);
    }

    public static List<Feature> GetConditionalFeatures(this Profile profile)
    {
      return GetFeatures(profile, attribute => RequirementLevel.Conditional == attribute.RequirementLevel);
    }

    public static List<Feature> GetOptionalFeatures(this Profile profile)
    {
      return GetFeatures(profile, attribute => RequirementLevel.Optional == attribute.RequirementLevel);
    }

    #endregion

    #region Tests

    public static List<TestInfo> GetTests(this Profile profile)
    {
      return profile.GetFeatures().Select(feature => feature.GetDependingTest()).ToList();
    }

    public static List<TestInfo> GetMandatoryTests(this Profile profile)
    {
      return profile.GetMandatoryFeatures().Select(feature => feature.GetDependingTest()).ToList();
    }

    public static List<TestInfo> GetConditionalTests(this Profile profile)
    {
      return profile.GetConditionalFeatures().Select(feature => feature.GetDependingTest()).ToList();
    }

    public static List<TestInfo> GetOptionalTests(this Profile profile)
    {
      return profile.GetConditionalFeatures().Select(feature => feature.GetDependingTest()).ToList();
    }

    #endregion

    #region Helpers

    //TODO
    public static List<FeatureNode> GetFeatureSet(this Profile profile)
    {
      return Data.Global.FeatureSet.Instance.GetFeatures(profile);
    }

    private static List<Feature> GetFeatures(Profile profile, Func<ProfileAttribute, bool> predicate)
    {
      var result = new List<Feature>();

      foreach (Feature feature in Enum.GetValues(typeof(Feature)))
      {
        String fieldName = feature.ToString();
        FieldInfo fieldInfo = feature.GetType().GetField(fieldName);

        ProfileAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(ProfileAttribute), false) as ProfileAttribute[];

        if (null == attributes)
          continue;

        var filteredAttributesList = attributes.Where(attribute => attribute.Profile == profile).ToList();
        if (null == predicate)
          result.AddRange(filteredAttributesList.Select(attribute => feature));
        else
          result.AddRange(filteredAttributesList.Where(predicate).Select(attribute => feature));
      }

      return result;
    }

    #endregion
  }
}
