///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Xml.Schema;

namespace TestTool.Tests.CommonUtils.SoapValidation
{
    /// <summary>
    /// Represents a set of XmlSchemas.
    /// </summary>
    public interface ISchemasSet
    {
        IEnumerable<XmlSchema> Schemas { get; }
    }
    
}
