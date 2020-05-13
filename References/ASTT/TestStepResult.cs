using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ONVIFTestTool
{
    /// <summary>
    /// Class for test step result data
    /// </summary>
    class TestStepResult
    {
        #region Constructors

        /// <summary>
        /// TestResult default constructor
        /// </summary>
        public TestStepResult()
        {
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Message for log (step ID + Result string + message)
        /// </summary>
        public string MessageForLog
        {
            get
            {
                string result = "Step " + m_stepId + " ";
                if (m_result)
                {
                    result = result + "PASSED";
                }
                else
                {
                    result = result + "FAILED";
                }
                result = result + " " + m_message;
                return result;
            }
        }

        /// <summary>
        /// Result string
        /// </summary>
        public string ResultString
        {
            get
            {
                if (m_result)
                {
                    return "PASSED";
                }
                else
                {
                    return "FAILED";
                }
            }
        }

        /// <summary>
        /// Message for step result
        /// </summary>
        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        /// <summary>
        /// Step result
        /// </summary>
        public bool Result
        {
            get { return m_result; }
            set { m_result = value; }
        }

        /// <summary>
        /// Step ID
        /// </summary>
        public string StepId
        {
            get { return m_stepId; }
            set { m_stepId = value; }
        }

        /// <summary>
        /// Path to response file in result folder
        /// </summary>
        public string PathToRealResponseFile
        {
            get { return m_pathToRealResponseFile; }
            set { m_pathToRealResponseFile = value; }
        }

        /// <summary>
        /// Path to file with comparation on expected and real response
        /// </summary>
        public string PathToCompareFile
        {
            get { return m_pathToCompareFile; }
            set { m_pathToCompareFile = value; }
        }

        /// <summary>
        /// Path to response file with HTTP header
        /// </summary>
        public string PathToRealResponseFileFull
        {
            get { return m_pathToRealResponseFileFull; }
            set { m_pathToRealResponseFileFull = value; }
        }

        /// <summary>
        /// Path to request file in result folder 
        /// </summary>
        public string PathToRequestFile
        {
            get { return m_pathToRequestFile; }
            set { m_pathToRequestFile = value; }
        }

        /// <summary>
        /// Deskription of the step (what we do there)
        /// </summary>
        public string StepDescription
        {
            get { return m_stepDescription; }
            set { m_stepDescription = value; }
        }

        /// <summary>
        /// Time between request and first response packet
        /// </summary>
        public double Time
        {
            get { return m_time; }
            set { m_time = value; }
        }

        #endregion //Properties

        #region Fields

        private bool m_result = false;
        private string m_message = "";
        private string m_stepId = "";
        private string m_pathToRealResponseFile = "";
        private string m_pathToRealResponseFileFull = "";
        private string m_pathToCompareFile = "";
        private string m_pathToRequestFile = "";
        private string m_stepDescription = "";
        private double m_time = 0;
        
        #endregion //Fields
    }
}
