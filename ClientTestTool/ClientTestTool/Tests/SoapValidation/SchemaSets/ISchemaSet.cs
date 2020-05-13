///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Xml.Schema;

namespace ClientTestTool.Tests.SoapValidation.SchemaSets
{
  /// <summary>
  /// Represents a set of XmlSchemas.
  /// </summary>
  public interface ISchemaSet
  {
    IEnumerable<XmlSchema> Schemas
    {
      get;
    }
  }

}
