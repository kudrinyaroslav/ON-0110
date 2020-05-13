///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Xml.Schema;

namespace TestTool.Tests.TestCases.Utils.FaultValidation
{
    /// <summary>
    /// Represents a set of XmlSchemas.
    /// </summary>
    interface ISchemasSet
    {
        IEnumerable<XmlSchema> Schemas { get; }
    }
    
}
