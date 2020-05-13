///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Conformance.Extensions
{
  internal static class ConformanceInfoExtension
  {
    /// <summary>
    /// Gets the supported profiles string.
    /// </summary>
    /// <returns></returns>
    internal static String GetSupportedProfilesString(this ConformanceInfo info)
    {
      var supportedProfiles = Enum.GetValues(typeof(Profile)).Cast<Profile>().Where(profile => profile.IsSupported());
      //var profiles = ConformanceLog.Instance.Errors.Where(item => !item.Value.Any()).Select(item => item.Key).ToList(); //TODO
      //profiles.RemoveAll(item => ConformanceLog.Instance.Warnings[item].Any());
      return String.Join(", ", supportedProfiles);
    }

    /// <summary>
    /// Gets the supported features string.
    /// </summary>
    /// <returns></returns>
    internal static String GetSupportedFeaturesString(this ConformanceInfo info)
    {
      var features = FeatureSet.Instance.Where(item => FeatureStatus.Supported == item.Info.Status).Select(item => item.Feature);
      return String.Join(", ", features);
    }
  }
}
