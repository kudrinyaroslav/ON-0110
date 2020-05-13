///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Xml.Linq;

namespace ClientTestTool.Data.Extensions
{
  public static class XElementExtension
  {
    /// <summary>
    /// Returns element with matching name, null otherwise
    /// </summary>
    public static XElement GetElementWithName(this XElement doc, String name)
    {
      if (null == doc)
        throw new ArgumentNullException("doc");

      return doc.Descendants().FirstOrDefault(item => item.Name.LocalName == name);
    }
  }
}
