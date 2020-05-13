///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Definitions.Devices.Extensions
{
  public static class ClientExtension // TODO conformance logic
  {
    public static List<Feature> GetSupportedFeatures(this Client client)
    {
      return new List<Feature>();
    }

    public static List<Profile> GetSupportedProfiles(this Client client)
    {
      return Enum.GetValues(typeof(Profile)).Cast<Profile>().Where(profile => profile.IsSupported()).ToList();
    }
  }
}
