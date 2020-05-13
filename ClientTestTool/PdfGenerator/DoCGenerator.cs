using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PdfGenerator
{
  public class DoCGenerator : PdfGenerator
  {
    private const String ID_ONVIF_LOGO     = "onvifLogo";

    public  const String ID_MEMBER_NAME    = "memberName";
    public  const String ID_MEMBER_ADDRESS = "memberAddress";
    public  const String ID_PRODUCT_NAME   = "clientProductName";
    public  const String ID_BRAND_ROW      = "clientBrandRow";
    public  const String ID_BRAND          = "clientBrand";
    public  const String ID_MODEL_ROW      = "clientModelRow";
    public  const String ID_MODEL          = "clientModel";
    public  const String ID_VERSION        = "clientVersion";
    public  const String ID_PRODUCT_TYPE   = "clientProductType";
    public  const String ID_OTHER_INFO_ROW = "clientOtherInformationRow";
    public  const String ID_OTHER_INFO     = "clientOtherInformation";


    public DoCGenerator(String templatePath) : base (templatePath)
    {
      
    }

    public override void Generate(Dictionary<String, String> values, String pdfFilename)
    {
      XElement xmlDoc = XElement.Load(mTemplatePath);

      ReplaceTagValue(xmlDoc, ID_ONVIF_LOGO, "src", Path.Combine(Directory.GetCurrentDirectory(), "onvif_doc_template_logo.jpg" )); // TODO resource

      if (null != values)
        foreach (var key in values.Keys)
          ReplaceElementValue(xmlDoc, key, values[key]);

      PrintUniqueIds(xmlDoc);

      RenderPdf(xmlDoc, Path.Combine(Directory.GetCurrentDirectory(), pdfFilename));
    }

    private static void PrintUniqueIds(XElement xmlDoc) // TODO
    {
      var elements = XmlAttributeUtil.GetElementsWithTag(xmlDoc, "id");
      String[] ids = new string[elements.Count];

      int i = 0;
      elements.ToList().ForEach(item =>
                                {
                                  ids[i] = item.Attribute("id").Value;
                                  ++i;
                                });

      File.WriteAllLines(Path.Combine(Directory.GetCurrentDirectory(), "ids.txt"), ids); // TODO
    }

    private void ReplaceElementValue(XElement doc, String elementId, String replacement)
    {
      var element = XmlAttributeUtil.GetElementById(doc, elementId);

      if (null == element)
        return;

      element.Value = replacement;
    }

    private void ReplaceTagValue(XElement doc, String elementId, String tag, String replacement)
    {
      var element = XmlAttributeUtil.GetElementById(doc, elementId);

      if (null == element)
        return;

      var attribute = element.Attribute(tag);

      if (null == attribute)
        return;

      attribute.Value = replacement;
    }
  }
}
