///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Drawing;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.GUI.Extensions
{
  internal static class FeatureStatusExtension
  {
    public static Color GetColor(this FeatureStatus status)
    {
      switch (status)
      {
        case FeatureStatus.Supported:
          return Color.Green;

        case FeatureStatus.NotSupported:
          return Color.Red;

        case FeatureStatus.Undefined:
          return Color.Gray;

        default:
          throw new ArgumentOutOfRangeException("status");
      }
    }
  }
}
