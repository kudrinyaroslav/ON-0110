///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.Tests.SoapValidation.SchemaSets
{
  /// <summary>
  /// Schemas required for events services messages validation.
  /// </summary>
  public class OnvifSchemaSet : BaseSchemaSet
  {
    private static OnvifSchemaSet mInstance;

    public static OnvifSchemaSet Instance
    {
      get
      {
        return mInstance ?? (mInstance = new OnvifSchemaSet());
      }
    }

    /// <summary>
    /// ctor
    /// </summary>
    protected OnvifSchemaSet()
    {

      String[] schemas = {
                           Root + "xml.xsd",
                           Root + "onvif.xsd",
                           Root + "envelope.xsd",
                           Root + "xmlmime.xsd",
                           Root + "b-2.xsd",
                           Root + "include.xsd",
                           Root + "bf-2.xsd",
                           Root + "r-2.xsd",
                           Root + "t-1.xsd",
                           Root + "ws-addr.xsd"
                         };

      LoadSchemas(schemas);
    }

  }
}