///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace TestTool.Tests.Definitions.Trace
{
    /// <summary>
    /// Step results.
    /// </summary>
    [Serializable]
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

        [XmlIgnore]
        public bool ProcessingTimesSpecified = false;

        private List<int> m_ProcessingTimes;
        public List<int> ProcessingTimes
        {
            get
            {
                ProcessingTimesSpecified = true;
                return m_ProcessingTimes ?? (m_ProcessingTimes = new List<int>());
            }
            set { m_ProcessingTimes = value; }
        }

        /// <summary>
        /// Exception, if occurred.
        /// </summary>
        [XmlIgnore]
        public Exception Exception { get; set; }
    }

    public delegate void StepCompleted(StepResult result);

}
