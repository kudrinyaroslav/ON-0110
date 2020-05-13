///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using ClientTestTool.GUI.Logging;
using Fonet;

namespace ClientTestTool.Data.Conformance.DoC.Base.Pdf
{
  public abstract class PdfRenderer
  {
    protected PdfRenderer(String templatePath)
    {
      mTemplatePath = templatePath;
    }

    protected void RenderPdf(XElement foDoc, String pdfFilename)
    {
      try
      {
        using (XmlReader xmlReader = foDoc.CreateReader())
        using (FileStream fileStream = File.Create(pdfFilename))
        {
          FonetDriver driver = FonetDriver.Make();
          //driver.OnError += (o, args) =>
          //                {};

          driver.Render(xmlReader, fileStream);
        }
      }
      catch (Exception e)
      {
        Logger.WriteLine(e.ToString());
      }
    }

    protected readonly String mTemplatePath;

  }
}
