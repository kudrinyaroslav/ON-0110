///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using ClientTestTool.Tests.SoapValidation.Extensions;
using ClientTestTool.Tests.SoapValidation.SchemaSets;

namespace ClientTestTool.Tests.SoapValidation
{
  public class SoapValidator
  {
    private readonly ISchemaSet mSchemas;

    public List<String> ValidationErrors
    {
      get;
      private set;
    }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="schemaSet"></param>
    public SoapValidator(ISchemaSet schemaSet)
    {
      mSchemas = schemaSet;
      ValidationErrors = new List<String>();
    }

    /// <summary>
    /// Validatates xmlContent using schemas 
    /// </summary>
    /// <param name="xmlContent"></param>
    /// <returns></returns>
    public bool Validate(TextReader xmlContent)
    {
      XmlReaderSettings xmlSettings = new XmlReaderSettings();
      xmlSettings.Schemas.XmlResolver = null;
      xmlSettings.Schemas.AddRange(mSchemas.Schemas);
      xmlSettings.ValidationType = ValidationType.Schema;

      //xmlSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
      xmlSettings.ValidationEventHandler += OnValidationError;

      XmlReader reader = XmlReader.Create(xmlContent, xmlSettings);

       
      while ( reader.Read() )
      {
      }

      return true;
    }

    private void OnValidationError(object sender, ValidationEventArgs e)
    {
      ValidationErrors.Add(e.Message);
    }
  }
}
