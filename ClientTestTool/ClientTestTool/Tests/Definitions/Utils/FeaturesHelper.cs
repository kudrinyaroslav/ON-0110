///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Engine;
using ClientTestTool.Tests.Enums;

namespace ClientTestTool.Tests.Definitions.Utils
{
  /// <summary>
  /// Contains methods for handling Features logic.
  /// </summary>
  public static class FeaturesHelper
  {
    /// <summary>
    /// Display names (for features where display name differs from Feature.ToString() )
    /// </summary>
    private static Dictionary<Feature, String> mDisplayNames;

    /// <summary>
    /// Accessor to display names.
    /// </summary>
   private static Dictionary<Feature, String> DisplayNames
    {
      get
      {
        if (null == mDisplayNames)
          InitDisplayNames();

        return mDisplayNames;
      }
    }

    /// <summary>
    /// Initializes display names (for test tooltips)
    /// </summary>
    private static void InitDisplayNames()
    {
      mDisplayNames = new Dictionary<Feature, String> {
                                                        { Feature.WSU, "WS-UsernameToken"   },
                                                        { Feature.HTTPDigest, "HTTP Digest" },
                                                        { Feature.EventHandling, "Event Handling"}
                                                      };
    }

    /// <summary>
    /// Feature display name. For most features it's just feature.ToString()
    /// </summary>
    /// <param name="feature"></param>
    /// <returns></returns>
    public static string GetDisplayName(Feature feature)
    {
      if (DisplayNames.ContainsKey(feature))
        return DisplayNames[feature];

      return feature.ToString();
    }

    /// <summary>
    /// Initializes display names for features tree.
    /// </summary>
    /// <param name="featuresSet"></param>
    public static void Translate(FeaturesSet featuresSet)
    {
      foreach (FeatureNode node in featuresSet.Nodes)
        Translate(node);
    }

    /// <summary>
    /// Initializes display name.
    /// </summary>
    /// <param name="node"></param>
    private static void Translate(FeatureNode node)
    {
      node.DisplayName = GetTreeDisplayName(node.Feature);
      foreach (FeatureNode child in node.Nodes)
        Translate(child);
    }

    /// <summary>
    /// Gets display name for Tree. If not overridden, "common" display name is used.
    /// </summary>
    /// <param name="feature"></param>
    /// <returns></returns>
    private static String GetTreeDisplayName(Feature feature)
    {
      return GetDisplayName(feature);
    }

  }

}
