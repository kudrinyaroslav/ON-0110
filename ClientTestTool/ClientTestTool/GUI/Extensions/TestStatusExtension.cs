///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Drawing;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.GUI.Extensions
{
  internal static class TestStatusExtension
  {
    public static Color GetColor(this TestStatus status)
    {
      switch (status)
      {
        case TestStatus.Passed:
          return Color.Green;

        case TestStatus.Failed:
          return Color.Red;

        case TestStatus.NotDetected:
          return Color.Gray;

        default:
          throw new ArgumentOutOfRangeException("status");
      }
    }
  }
}
