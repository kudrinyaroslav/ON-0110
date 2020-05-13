using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ONVIFTestTool
{
    /// <summary>
    /// Class for test result data
    /// </summary>
    class TestResult
    {
        
        #region Constructors

        /// <summary>
        /// TestResult default constructor
        /// </summary>
        public TestResult()
        {
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Add step result to test result
        /// </summary>
        /// <param name="testStepResult">Test result</param>
        public void AddStepResult(TestStepResult testStepResult)
        {
            m_testStepResultArray.Add(testStepResult);
        }

        #endregion //Methods

        #region Properties

        /// <summary>
        /// Message for log (tesdt id + result + message)
        /// </summary>
        public string MessageForShortLog
        {
            get 
            {
                string result = m_testId + " ";
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

        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        public bool Result
        {
            get { return m_result; }
            set { m_result = value; }
        }

        public string TestId
        {
            get { return m_testId; }
            set { m_testId = value; }
        }

        public string ResultString
        {
            get 
            {
                if (this.Result)
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
        /// Test group (usially commang name)
        /// </summary>
        public string Group
        {
            get { return m_group; }
            set { m_group = value; }
        }

        public string Service
        {
            get { return m_service; }
            set { m_service = value; }
        }

        public string TestName
        {
            get { return m_testName; }
            set { m_testName = value; }
        }

        public string TestDescription
        {
            get { return m_testDescription; }
            set { m_testDescription = value; }
        }

        #endregion //Properties

        #region Fields

        private bool m_result = false;
        private string m_message = "";
        private string m_testId = "";
        private string m_service = "";
        private string m_group = "";
        private string m_testName = "";
        private string m_testDescription = "";
        public List<TestStepResult> m_testStepResultArray = new List<TestStepResult>();

        #endregion //Fields
    }
}
