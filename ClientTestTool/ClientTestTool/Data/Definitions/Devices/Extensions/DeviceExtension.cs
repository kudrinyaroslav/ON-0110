///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Devices.Definitions.FeatureList;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Definitions.Devices.Extensions
{
  public static class DeviceExtension
  {
    public static String GetConformanceName(this Device device)
    {
      if (!device.IsFeatureListAttached)
        return device.Name;

      return new DeviceFeatureListParser(device).GetTestProductName();
    }

    public static IEnumerable<Feature> GetSupportedFeatures(this Device device)
    {
      if (!device.IsFeatureListAttached)
        return Enumerable.Empty<Feature>();

      return new DeviceFeatureListParser(device).GetSupportedFeatures();
    }

    public static IEnumerable<Profile> GetSupportedProfiles(this Device device)
    {
      if (!device.IsFeatureListAttached)
        return Enumerable.Empty<Profile>();

      return new DeviceFeatureListParser(device).GetSupportedProfiles();
    }
  }
}
