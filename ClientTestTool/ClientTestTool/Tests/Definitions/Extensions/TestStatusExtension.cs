///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.Tests.Definitions.Extensions
{
  public static class TestStatusExtension
  {
    public static FeatureStatus ToFeatureStatus(this TestStatus status)
    {
      switch (status)
      {
        case TestStatus.Passed:
          return FeatureStatus.Supported;
        case TestStatus.Failed:
          return FeatureStatus.NotSupported;
        case TestStatus.NotDetected:
          return FeatureStatus.Undefined;

        default:
          throw new ArgumentException("Unknown TestStatus", "status");
      }
    }
  }
}
