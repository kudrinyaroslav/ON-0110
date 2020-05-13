using System;
using System.Xml.Serialization;
using TestTool.Tests.Definitions.Data;
using System.Text;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
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
        public Tests.Definitions.Trace.TestLog Log { get; set; }
        /// <summary>
        /// Plain log as string.
        /// </summary>
        public string PlainTextLog { get; set; }
        /// <summary>
        /// Shortened log (without step details).
        /// </summary>
        [XmlIgnore]
        public string ShortTextLog { get; set; }
        /// <summary>
        /// Warnings log.
        /// </summary>
        [XmlIgnore]
        public List<string> Warnings { get; set; } 

    }
}
