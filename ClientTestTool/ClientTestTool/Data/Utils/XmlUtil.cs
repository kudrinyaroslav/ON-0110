///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ClientTestTool.Data.Utils
{
  public static class XmlUtil
  {
    public static XElement RemoveAllNamespaces(XElement doc)
    {
      return new XElement(doc.Name.LocalName,
        (from n in doc.Nodes()
         select ((n is XElement) ? RemoveAllNamespaces(n as XElement) : n)),
            (doc.HasAttributes) ?
              (from a in doc.Attributes()
               where (!a.IsNamespaceDeclaration)
               select new XAttribute(a.Name.LocalName, a.Value)) : null);
    }

    public static IList<XElement> GetElementsWithTag(XElement doc, String tag)
    {
      var elements = doc.DescendantNodes().OfType<XElement>();
      return elements.Where(element => element.HasAttributes && null != element.Attribute(tag)).ToList();
    }

    /// <summary>
    /// returns XElement containg id, null otherwise
    /// </summary>
    public static XElement GetElementById(XElement doc, String id)
    {
      var elements = GetElementsWithTag(doc, ID_TAG);

      return elements.FirstOrDefault(item => item.Attribute(ID_TAG).Value == id);
    }

    public static void ReplaceId(XElement doc, String oldId, String newId)
    {
      if (doc.HasAttributes)
      {
        var idAttribute = doc.Attribute(ID_TAG);
        if (null != idAttribute && idAttribute.Value == oldId)
        {
          idAttribute.Value = newId;
          return;
        }
      }

      var element = GetElementById(doc, oldId);

      if (null == element)
        return;

      element.Attribute(ID_TAG).Value = newId;
    }

    private const String ID_TAG = "id";
    
  }
}
