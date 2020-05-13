///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.GUI.Utils
{
  /// <summary>
  /// Contains methods for create tooltips etc. strings.
  /// </summary>
  public static class ConverterUtil
  {
    /// <summary>
    /// Creates list of features user-friendly names.
    /// </summary>
    /// <param name="features">list of features.</param>
    /// <returns></returns>
    public static String GetFeaturesString(this IList<Feature> features)
    {
      String feature = String.Empty;

      if (features.Count > 0)
        String.Join(",", features.SelectMany(item => item.GetDisplayName()));
      else
        feature = "UNDEFINED";

      return feature;
    }

    #region Colors

    public static Color GetStatusColor(FeatureStatus status)
    {
      return status.GetColor();
    }

    public static Color GetStatusColor(TestStatus status)
    {
      return status.GetColor();
    }

    #endregion
  }
}
