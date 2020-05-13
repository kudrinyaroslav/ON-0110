///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Engine
{
  public static class ProfilesSet
  {
    static ProfilesSet()
    {
      mProfiles = new Dictionary<Profile, FeaturesSet>();
    }

    public static FeaturesSet GetFeaturesForProfile(Profile profile)
    {
      List<Feature> featuresForProfile = GetFeatures(profile);

      FeaturesSet set = FeaturesSet.CreateFeaturesSet();

      foreach (var featureNode in set.Nodes)
        featureNode.Nodes.RemoveAll(item => !featuresForProfile.Contains(item.Feature));

      set.Nodes.RemoveAll(item => 0 == item.Nodes.Count);

      return set;
    }

    private static List<Feature> GetFeatures(Profile profile) // TODO move it
    {
      List<Feature> result = new List<Feature>();

      foreach (Feature feature in Enum.GetValues(typeof(Feature)))
      {
        String    fieldName = feature.ToString();
        FieldInfo fieldInfo = feature.GetType().GetField(fieldName);

        ProfileAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(ProfileAttribute), false) as ProfileAttribute[];

        if (null == attributes)
          continue;

        result.AddRange(attributes.Where(attribute => attribute.Profile == profile).Select(attribute => feature));
      }

      return result;
    }

    private static Dictionary<Profile, FeaturesSet> mProfiles;
  }
}
