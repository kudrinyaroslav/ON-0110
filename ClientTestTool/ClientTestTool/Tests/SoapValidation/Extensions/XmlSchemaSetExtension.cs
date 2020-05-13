///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Xml.Schema;

namespace ClientTestTool.Tests.SoapValidation.Extensions
{
  public static class XmlSchemaSetExtension
  {
    /// <summary>
    /// Adds the lements of the specified collection to the end of the schemaSet
    /// </summary>
    /// <param name="schemaSet"></param>
    /// <param name="values"></param>
    public static void AddRange(this XmlSchemaSet schemaSet, IEnumerable<XmlSchema> values)
    {
      foreach (XmlSchema xmlSchema in values)
        schemaSet.Add(xmlSchema);
    }
  }
}
