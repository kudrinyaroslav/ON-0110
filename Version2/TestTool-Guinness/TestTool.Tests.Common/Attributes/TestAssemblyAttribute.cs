///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
