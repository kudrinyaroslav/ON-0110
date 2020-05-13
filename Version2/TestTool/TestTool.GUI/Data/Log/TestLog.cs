///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine;

namespace TestTool.GUI.Data
{
    public class RealTimeouts
    {
        public int Maximum;
        public int Median;
        public int Average;
    };

    /// <summary>
    /// Tests execution information (created and filled at the "Tests" page)
    /// </summary>
    public class TestLog
    {
        /// <summary>
        /// Tests execution time
        /// </summary>
        public DateTime TestExecutionTime { get; set; }
        /// <summary>
        /// Test execution information (for tests launched)
        /// </summary>
        public Dictionary<TestInfo, TestResult> TestResults { get; set; }
        
        /// <summary>
        /// List of features defined
        /// </summary>
        public List<Feature> Features { get; set; }

        /// <summary>
        /// Initialization data
        /// </summary>
        public ConformanceInitializationData InitializationData { get; set; }

        /// <summary>
        /// Features definition log
        /// </summary>
        public TestResult FeaturesDefinitionLog { get; set; }

        public RealTimeouts Timeouts { get; set; }
    }

    /// <summary>
    /// Full test log to be passed to report generator.
    /// </summary>
    public class TestLogFull
    {
        /// <summary>
        /// Tests execution time.
        /// </summary>
        public DateTime TestExecutionTime { get; set; }
        /// <summary>
        /// Test results.
        /// </summary>
        public Dictionary<TestInfo, TestResult> TestResults { get; set; }
        /// <summary>
        /// List of features
        /// </summary>
        public List<Feature> Features { get; set; }
        /// <summary>
        /// Tester information
        /// </summary>
        public TesterInfo TesterInfo { get; set; }
        /// <summary>
        /// Responsible member info
        /// </summary>
        public MemberInfo MemberInfo { get; set; }

        public SupportInfo SupportInfo { get; set; }

        public string ProductName { get; set; }

        public string ProductTypes { get; set; }

        public string ProductTypesOther { get; set; }

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
        /// <summary>
        /// Initialization data
        /// </summary>
        public ConformanceInitializationData InitializationData { get; set; }
        /// <summary>
        /// Device information
        /// </summary>
        public Tests.Engine.DeviceInformation DeviceInformation { get; set; }
        /// <summary>
        /// Log of features definition process
        /// </summary>
        public TestResult FeaturesDefinitionLog { get; set; }

        public Data.Log.ManagementSettings ManagementSettings { get; set; }

        public RealTimeouts Timeouts { get; set; }
    }

}


