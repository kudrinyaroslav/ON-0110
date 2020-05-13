using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PdfGenerator
{
  public static class XmlAttributeUtil
  {
    private const String ID_TAG = "id";

    public static IList<XElement> GetElementsWithTag(XElement docElement, String tag)
    {
      var elements = docElement.DescendantNodes().OfType<XElement>();//DescendantNodes().SelectMany(node => node.)
      return elements.Where(element => element.HasAttributes && null != element.Attribute(tag)).ToList();
    }

    /// <summary>
    /// returns XElement containg id, null otherwise
    /// </summary>
    /// <param name="docElement"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static XElement GetElementById(XElement docElement, String id)
    {
      var elements = GetElementsWithTag(docElement, ID_TAG);

      return elements.FirstOrDefault(item => item.Attribute(ID_TAG).Value == id);
    }
  }
}
