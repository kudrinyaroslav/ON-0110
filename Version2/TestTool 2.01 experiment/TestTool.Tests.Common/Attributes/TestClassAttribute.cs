///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Tests.Common.Attributes
{
    /// <summary>
    /// Attribute to mark the class containing tests (to reduce the number of classes checked for test methods)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute
    {
    }
}
