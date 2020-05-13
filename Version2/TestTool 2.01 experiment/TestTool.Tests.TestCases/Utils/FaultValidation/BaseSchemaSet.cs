///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Schema;
using System.IO;

namespace TestTool.Tests.TestCases.Utils.FaultValidation
{
    /// <summary>
    /// Base schema set functionality.
    /// </summary>
    class BaseSchemaSet
    {
        private static Assembly _currentAssembly;

        /// <summary>
        /// Current assembly.
        /// </summary>
        protected Assembly CurrentAssembly
        {
            get
            {
                if (_currentAssembly == null)
                {
                    _currentAssembly = Assembly.GetExecutingAssembly();
                }
                return _currentAssembly;
            }
        }

        private static Dictionary<string, XmlSchema> _schemas;

        /// <summary>
        /// Gets XML schema by path. Loads, if the schema not found.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected XmlSchema GetXmlSchema(string path)
        {
            if (_schemas == null)
            {
                _schemas = new Dictionary<string, XmlSchema>(StringComparer.CurrentCultureIgnoreCase);
            }

            if (!_schemas.ContainsKey(path))
            {
                Stream schemaStream = CurrentAssembly.GetManifestResourceStream(path);
                XmlSchema schema = XmlSchema.Read(schemaStream, null);
                schemaStream.Close();
                _schemas.Add(path, schema);
            }

            return _schemas[path];
        }

    }
}
