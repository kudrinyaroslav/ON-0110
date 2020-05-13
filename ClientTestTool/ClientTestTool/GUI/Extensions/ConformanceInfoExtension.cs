///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Conformance;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Extensions
{
  internal static class ConformanceInfoExtension
  {
    public static String GetDoCFilename(this ConformanceInfo info, bool errata = false)
    {
      if (null == info)
        throw new ArgumentNullException("info");

      String docName = errata ? "DoC_Errata" : "DoC";

      String productName = info.ProductName;
      String version     = info.Version;

      return FileHelper.TrimFilename(String.Format("ONVIF_{0}_{1}_{2}.pdf", docName, productName, version));
    }

    public static String GetFeatureListFilename(this ConformanceInfo info)
    {
      if (null == info)
        throw new ArgumentNullException("info");

      String productName = info.ProductName;
      String version     = info.Version;

      return FileHelper.TrimFilename(String.Format("ONVIF_FeatureList_{0}_{1}.xml", productName, version));
    }
  }
}
