///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.Tests.Common.TestEngine;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Tests execution information (created and filled at the "Tests" page)
    /// </summary>
    public class TestLog
    {
        /// <summary>
        /// All tests loaded
        /// </summary>
        public List<TestInfo> Tests { get; set; }
        /// <summary>
        /// Tests execution time
        /// </summary>
        public DateTime TestExecutionTime { get; set; }
        /// <summary>
        /// Test execution information (for tests launched)
        /// </summary>
        public Dictionary<TestInfo, TestResult> TestResults { get; set; }
    }

    /// <summary>
    /// Full test log to be passed to report generator.
    /// </summary>
    public class TestLogFull
    {
        /// <summary>
        /// All tests loaded.
        /// </summary>
        public List<TestInfo> Tests { get; set; }
        /// <summary>
        /// Tests execution time.
        /// </summary>
        public DateTime TestExecutionTime { get; set; }
        /// <summary>
        /// Test results.
        /// </summary>
        public Dictionary<TestInfo, TestResult> TestResults { get; set; }
        /// <summary>
        /// Device information.
        /// </summary>
        public DeviceInfo DeviceInfo { get; set; }
        /// <summary>
        /// Tester information
        /// </summary>
        public TesterInfo TesterInfo { get; set; }
        /// <summary>
        /// Additional information entered by test operator.
        /// </summary>
        public string OtherInformation { get; set; }
        /// <summary>
        /// Application information.
        /// </summary>
        public ApplicationInfo Application { get; set; }
        /// <summary>
        /// Device environment.
        /// </summary>
        public DeviceEnvironment DeviceEnvironment { get; set; }
    }

}


