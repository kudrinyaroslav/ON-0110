///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Reflection;

namespace TestTool.Tests.Common.TestBase
{
    /// <summary>
    /// Test interface.
    /// </summary>
    public interface ITestSuite
    {
        void ExitTest(Exception ex);

        MethodInfo EntryPoint { get; set;}

        void Start();

        void Halt();

        void Pause();

        void Resume();
    }
}
