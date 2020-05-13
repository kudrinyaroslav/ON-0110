///
/// @Author Matthew Tuusberg
///

﻿namespace ClientTestTool.Tests.SoapValidation.SchemaSets
{
  class SoapSchemaSet : BaseSchemaSet
  {
    private static SoapSchemaSet mInstance;

    public static SoapSchemaSet Instance
    {
      get
      {
        return mInstance ?? (mInstance = new SoapSchemaSet());
      }
    }
    /// <summary>
    /// ctor
    /// </summary>
    protected SoapSchemaSet()
    {
      string[] schemas = {
                           Root + "xml.xsd",
                           Root + "envelope.xsd"
                         };

      LoadSchemas(schemas);
    }
  }
}
