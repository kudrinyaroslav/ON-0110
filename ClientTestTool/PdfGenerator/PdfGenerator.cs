using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Fonet;

namespace PdfGenerator
{
  public abstract class PdfGenerator
  {
    protected String      mTemplatePath;

    protected PdfGenerator(String templatePath)
    {
      mTemplatePath = templatePath;
    }

    protected void Prepare()
    {
      //TODO
    }

    public abstract void Generate(Dictionary<String, String> values, String pdfFile);

    protected void RenderPdf(XElement foDoc, String pdfFilename)
    {
      using (XmlReader xmlReader = foDoc.CreateReader())
      using (FileStream fileStream = File.Create(pdfFilename))
      {
        FonetDriver driver = FonetDriver.Make();
        driver.OnError += (o, args) =>
                          {};
        driver.Render(xmlReader, fileStream);
      }
    }

  }
}
