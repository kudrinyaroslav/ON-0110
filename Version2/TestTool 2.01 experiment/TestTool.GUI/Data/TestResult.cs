///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Test execution result.
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Test execution details.
        /// </summary>
        public Tests.Common.Trace.TestLog Log { get; set; }
        /// <summary>
        /// Test information found in test assembly.
        /// </summary>
        public Tests.Common.TestEngine.TestInfo TestInfo { get; set; }
        /// <summary>
        /// Plain log as string.
        /// </summary>
        public string PlainTextLog { get; set; }
        /// <summary>
        /// Shortened log (without step details).
        /// </summary>
        public string ShortTextLog { get; set; }

    }
}
