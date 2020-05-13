using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using AutomatedTesting.GUI.Controllers;

namespace AutomatedTesting.GUI.Data
{
    /// <summary>
    /// Test execution result.
    /// </summary>
    [Serializable]
    public class TestResult
    {
        /// <summary>
        /// Test information found in test assembly.
        /// </summary>
        public TestInfo TestInfo { get; set; }
        /// <summary>
        /// Test execution details.
        /// </summary>
        public TestTool.Tests.Definitions.Trace.TestLog Log { get; set; }
        /// <summary>
        /// Plain log as string.
        /// </summary>
        public string PlainTextLog { get; set; }
        /// <summary>
        /// Test Description
        /// </summary>
        public string TestDescription { get; set; }
        /// <summary>
        /// Test Result from DUT
        /// </summary>
        public bool TestResultDUT { get; set; }
        /// <summary>
        /// Expected Test Result underfined
        /// </summary>
        public bool TestResultONVIFTestUnderfined { get; set; }
        /// <summary>
        /// Final Result
        /// </summary>
        public string TestFinalResult { get; set; }
        /// <summary>
        /// Test Result the same with expected
        /// </summary>
        public bool TestResultONVIFTest { get; set; }
        /// <summary>
        /// Test Result Inner Exception
        /// </summary>
        public bool TestResultInnerException { get; set; }
        /// <summary>
        /// Test Name
        /// </summary>
        public string TestName { get; set; }
        /// <summary>
        /// Test Expected Result
        /// </summary>
        public string TestExpectedResult { get; set; }
        public string DutLog { get; set; }

        public TestCaseStatus Status { get; set; }
    }

}
