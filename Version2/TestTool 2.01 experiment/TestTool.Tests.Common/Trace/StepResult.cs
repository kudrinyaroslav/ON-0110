///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Tests.Common.Trace
{
    /// <summary>
    /// Step results.
    /// </summary>
    public class StepResult
    {
        /// <summary>
        /// Step number
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Step name
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Request sent (if step includes sending requests)
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// Response received (if step includes sending requests)
        /// </summary>
        public string Response { get; set; }
        /// <summary>
        /// Step status
        /// </summary>
        public StepStatus Status { get; set; }
        /// <summary>
        /// Exception, if occurred.
        /// </summary>
        public Exception Exception { get; set; }
    }

    public delegate void StepCompleted(StepResult result);

}
