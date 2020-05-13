using System;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.Tests.Definitions.Trace
{
    /// <summary>
    /// Single test log.
    /// </summary>
    [Serializable]
    public class TestLog
    {
        private List<StepResult> _steps = new List<StepResult>();
        /// <summary>
        /// Test steps.
        /// </summary>
        public List<StepResult> Steps
        {
            get { return _steps; }
        }

        private string _testErrorMessage;
        /// <summary>
        /// Test error message (if an error occurred outside a step)
        /// </summary>
        public string ErrorMessage
        {
            get { return _testErrorMessage; }
            set { _testErrorMessage = value;}
        }

        /// <summary>
        /// Adds step result to the log.
        /// </summary>
        /// <param name="result">Step result.</param>
        void AddStepResult(StepResult result)
        {
            _steps.Add(result);
        }

        private TestStatus _testStatus;
        /// <summary>
        /// Test status.
        /// </summary>
        public TestStatus TestStatus
        {
            get { return _testStatus; }
            set { _testStatus = value;}
        }

    }
        
    public delegate void TestCompleted(TestLog log);
}
