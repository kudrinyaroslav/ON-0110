///
/// @Author Matthew Tuusberg
///

﻿namespace ClientTestTool.Tests.SoapValidation.SchemaSets
{
  class XmlSchemaSet : BaseSchemaSet
  {
    private static XmlSchemaSet mInstance;

    public static XmlSchemaSet Instance
    {
      get
      {
        return mInstance ?? (mInstance = new XmlSchemaSet());
      }
    }
    /// <summary>
    /// ctor
    /// </summary>
    protected XmlSchemaSet()
    {
      string[] schemas = {
                           Root + "xml.xsd"
                         };

      LoadSchemas(schemas);
    }
  }
}
