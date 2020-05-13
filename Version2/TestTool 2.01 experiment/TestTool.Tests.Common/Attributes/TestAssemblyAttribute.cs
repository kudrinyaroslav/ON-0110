///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Tests.Common.Attributes
{
    /// <summary>
    /// Attribute to mark the assembly containing tests
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class TestAssemblyAttribute : Attribute
    {
    }
}
