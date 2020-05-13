///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Schema;

namespace ClientTestTool.Tests.SoapValidation.SchemaSets
{
  /// <summary>
  /// Base schema set functionality.
  /// </summary>
  public class BaseSchemaSet : ISchemaSet
  {
    /// <summary>
    /// Constructor
    /// </summary>
    public BaseSchemaSet()
    {
      mSoapSchemas = new List<XmlSchema>();
    }

    /// <summary>
    /// Schemas folder
    /// </summary>
    protected const string Root = "ClientTestTool.Tests.SoapValidation.Schemas.";

    /// <summary>
    /// Current assembly
    /// </summary>
    private static Assembly sCurrentAssembly;

    /// <summary>
    /// Current assembly.
    /// </summary>
    protected Assembly CurrentAssembly
    {
      get
      {
        return sCurrentAssembly ?? (sCurrentAssembly = Assembly.GetExecutingAssembly());
      }
    }

    /// <summary>
    /// Schemas common cache
    /// </summary>
    private static Dictionary<String, XmlSchema> sSchemas;

    /// <summary>
    /// Schemas loaded
    /// </summary>
    private readonly List<XmlSchema> mSoapSchemas;

    /// <summary>
    /// Schemas loaded
    /// </summary>
    protected List<XmlSchema> SoapSchemas
    {
      get
      {
        return mSoapSchemas;
      }
    }

    /// <summary>
    /// Loads schemas
    /// </summary>
    /// <param name="schemas"></param>
    protected void LoadSchemas(IEnumerable<String> schemas)
    {
      foreach (string schemaLocation in schemas)
        SoapSchemas.Add(GetXmlSchema(schemaLocation));
      
    }

    #region ISchemasSet Members

    /// <summary>
    /// Returns set of schemas required.
    /// </summary>
    public IEnumerable<XmlSchema> Schemas
    {
      get
      {
        return mSoapSchemas;
      }
    }

    #endregion

    /// <summary>
    /// Gets XML schema by path. Loads, if the schema not found.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected XmlSchema GetXmlSchema(string path) 
    {
      if (null == sSchemas)
        sSchemas = new Dictionary<String, XmlSchema>(StringComparer.CurrentCultureIgnoreCase);
 
      if (!sSchemas.ContainsKey(path))
      {
        Stream schemaStream = CurrentAssembly.GetManifestResourceStream(path);
        XmlSchema schema = XmlSchema.Read(schemaStream, null);
        schemaStream.Close();
        sSchemas.Add(path, schema);
      }

      return sSchemas[path];
    }

  }
}