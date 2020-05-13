///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Schema;
using System.IO;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Base schema set functionality.
    /// </summary>
    public class BaseSchemaSet : ISchemasSet
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseSchemaSet()
        {
            _soapSchemas = new List<XmlSchema>();
        }

        /// <summary>
        /// Schemas folder
        /// </summary>
        protected const string ROOT = "TestTool.Tests.TestCases.Utils.SoapValidation.Schemas.";

        /// <summary>
        /// Current assembly
        /// </summary>
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

        /// <summary>
        /// Schemas common cache
        /// </summary>
        private static Dictionary<string, XmlSchema> _schemas;

        /// <summary>
        /// Schemas loaded
        /// </summary>
        private List<XmlSchema> _soapSchemas;
        
        /// <summary>
        /// Schemas loaded
        /// </summary>
        protected List<XmlSchema> SoapSchemas
        {
            get { return _soapSchemas; }
        }

        /// <summary>
        /// Loads schemas
        /// </summary>
        /// <param name="schemas"></param>
        protected void LoadSchemas(IEnumerable<string> schemas)
        {
            foreach (string schemaLocation in schemas)
            {
                SoapSchemas.Add(GetXmlSchema(schemaLocation));
            }
        }

        #region ISchemasSet Members

        /// <summary>
        /// Returns set of schemas required.
        /// </summary>
        public IEnumerable<XmlSchema> Schemas
        {
            get { return _soapSchemas; }
        }

        #endregion

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