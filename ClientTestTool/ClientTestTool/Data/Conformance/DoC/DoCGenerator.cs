///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ClientTestTool.Data.Conformance.DoC.Base;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Data.Utils;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Conformance.DoC
{
  public sealed class DoCGenerator : BaseDoCGenerator
  {

    public DoCGenerator(ConformanceInfo info, String templatePath) : base (info, templatePath)
    {
    }

    public override void Generate(String pdfFilename)
    {
      var doc = XElement.Load(mTemplatePath);

      ReplaceAttributeValue(doc, ID_ONVIF_LOGO, "src", Path.Combine(CTTSettings.GetStylesheetsDir(), "onvif_doc_template_logo.jpg" )); // TODO resource

      ReplaceIds(doc, Info);

      //PrintUniqueIds(xmlDoc);

      AddDevicesList(doc); // modify xml

      FillDevicesList(doc); // add device info

      RenderPdf(doc, Path.Combine(CTTSettings.GetOutputDir(), pdfFilename));
    }

  }
}
