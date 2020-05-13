///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;

namespace ClientTestTool.Data.Global.Settings
{
  public static class CTTSettings
  {
    public const String FILTER_REQUEST  = @"request";
    public const String FILTER_RESPONSE = @"response";
    public const String PROTOCOL_HTTP   = @"http";
    public const String PROTOCOL_RTSP   = @"rtsp";
    public const String EXTENSION_XML   = @"xml";
    public const String EXTENSION_TXT   = @"txt";

    public static String GetUserManualFilename()
    {
      return Path.Combine(GetDocsDir(), "ONVIF Client Test Tool Help.chm");
    }

    public static String GetLicenseFilename()
    {
      return Path.Combine(GetDocsDir(), "License.rtf");
    }

    public static String GetDoCTemplateFilename()
    {
      return Path.Combine(GetStylesheetsDir(), "doc_template.fo");
    }

    public static String GetDoCErrataTemplateFilename()
    {
      return Path.Combine(GetStylesheetsDir(), "doc_errata_template.fo");
    }

    public static String GetOutputDir()
    {
      return Path.Combine(Directory.GetCurrentDirectory(), "output");
    }

    public static String GetStylesheetsDir()
    {
      return Path.Combine(Directory.GetCurrentDirectory(), "stylesheets");
    }

    public static String GetDocsDir()
    {
      return Path.Combine(Directory.GetCurrentDirectory(), "Docs");
    }

    public static String[] GetProtocols()
    {
      return new[] { PROTOCOL_HTTP, PROTOCOL_RTSP };
    }
  }
}
