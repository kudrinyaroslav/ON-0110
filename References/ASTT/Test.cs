using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
//Takes from http://www.microsoft.com/downloads/details.aspx?FamilyID=3471df57-0c08-46b4-894d-f569aa7f7892&DisplayLang=en
using Microsoft.XmlDiffPatch;

namespace ONVIFTestTool
{
    /// <summary>
    /// Class for Test
    /// </summary>
    class Test
    {

        #region Constructors

        /// <summary>
        /// Test default constructor
        /// </summary>
        public Test()
        {
            m_test = null;
            m_pathToTests = null;
            m_pathToTestsResult = null;
        }

        /// <summary>
        /// Test constructor
        /// </summary>
        /// <param name="test">Test description in XML node</param>
        /// <param name="pathToTests">Path to test files with applied parameters</param>
        /// <param name="pathToTestsResult">Path to test result files</param>
        public Test(XmlNode test, string pathToTests, string pathToTestsResult)
        {
            m_test = test;
            m_pathToTests = pathToTests;
            m_pathToTestsResult = pathToTestsResult;
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Run test
        /// </summary>
        /// <param name="serviceInfo">Host information</param>
        /// <param name="CloseConnection">Do we need close connection after each request (true recomended)</param>
        /// <returns>Return test result</returns>
        public TestResult Run(ServiceInfo serviceInfo, bool CloseConnection)
        {
            TestResult testResult = new TestResult();
            testResult.Result = true;
            testResult.TestId = m_test.Attributes.GetNamedItem("id").InnerText;
            testResult.Service = m_test.ParentNode.ParentNode.Attributes.GetNamedItem("name").InnerText;
            testResult.Group = m_test.ParentNode.Attributes.GetNamedItem("command").InnerText;
            testResult.TestName = m_test.SelectSingleNode("Name").InnerText;
            testResult.TestDescription = m_test.SelectSingleNode("Description").InnerText;

            TestStepResult stepResult;

            //Run steps of the test
            foreach (XmlNode step in m_test.ChildNodes)
            {
                if (string.Compare(step.Name, "Step")==0)
                {
                    stepResult = RunStep(step, serviceInfo, CloseConnection);

                    testResult.AddStepResult(stepResult);

                    if (!stepResult.Result)
                    {
                        testResult.Message = stepResult.MessageForLog;
                        testResult.Result = false;
                        return testResult; //stop if step fail
                    }
                }
            }

            return testResult;
        }

        #endregion //Methods

        #region Other

        /// <summary>
        /// Run test step
        /// </summary>
        /// <param name="step">Step information</param>
        /// <param name="serviceInfo">Host information</param>
        /// <param name="CloseConnection">Do we need close connection after each request (true recomended)</param>
        /// <returns>Test step result</returns>
        private TestStepResult RunStep(XmlNode step, ServiceInfo serviceInfo, bool CloseConnection)
        {
            TestStepResult testStepResult = new TestStepResult();
            testStepResult.Result = false;
            testStepResult.StepId = step.Attributes.GetNamedItem("id").InnerText;
            testStepResult.StepDescription = step.PreviousSibling.InnerText;
            
            string resultSoapMessage = "";
            string resultCompareMessage;
            
            string pathToRequestFile = m_pathToTests + step.Attributes.GetNamedItem("fileRequest").InnerText;
            string pathToResponseFile = m_pathToTests + step.Attributes.GetNamedItem("fileAnswer").InnerText;
            string pathToResponseFileReal = m_pathToTestsResult + this.TestId + "_" + testStepResult.StepId + "_response.xml";
            string pathToResponseFileRealFull = m_pathToTestsResult + this.TestId + "_" + testStepResult.StepId + "_response_full.txt";
            string pathToCompareFile = m_pathToTestsResult + this.TestId + "_" + testStepResult.StepId + "_compare.htm";
            testStepResult.PathToRequestFile = pathToRequestFile;

            //Send request and get answer
            double time = 0;
            if (CloseConnection)
            {
                try
                {
                    SoapClient.OpenConnection(serviceInfo);
                    testStepResult.Result = SoapClient.SendSoapRequest(pathToRequestFile, pathToResponseFileReal, pathToResponseFileRealFull, serviceInfo, out resultSoapMessage, out time, CloseConnection);
                    
                }
                catch (Exception e)
                {
                    resultSoapMessage = e.Message;
                    testStepResult.Result = false;
                }
                finally
                {
                    SoapClient.CloseConnection();
                }
            }
            else
            {
                testStepResult.Result = SoapClient.SendSoapRequest(pathToRequestFile, pathToResponseFileReal, pathToResponseFileRealFull, serviceInfo, out resultSoapMessage, out time, CloseConnection);
            }
            testStepResult.Time = time;

            if (testStepResult.Result)
            {
                testStepResult.PathToRealResponseFile = pathToResponseFileReal;
                testStepResult.PathToRealResponseFileFull = pathToResponseFileRealFull;

                //Compare result to ethalon
                testStepResult.Result = CompareResponse(pathToResponseFile, pathToResponseFileReal, out resultCompareMessage, ref pathToCompareFile);
                testStepResult.Message = resultCompareMessage;
                if (!testStepResult.Result)
                {
                    if (System.IO.File.Exists(pathToCompareFile))
                    {
                        testStepResult.PathToCompareFile = pathToCompareFile;
                    }
                }
            }
            else
            {
                    testStepResult.Message = resultSoapMessage;
            }

            return testStepResult;
        }

        /// <summary>
        /// Compare resopnse from server with ethalon
        /// </summary>
        /// <param name="responseFile">Ethalon response file</param>
        /// <param name="responseFileReal">Real response file</param>
        /// <param name="resultMessage">Result mesage</param>
        /// <param name="pathToCompareFile">Path to file with comparation</param>
        /// <returns>Return true if file identical and false in other case</returns>
        private bool CompareResponse(string responseFile, string responseFileReal, out string resultMessage, ref string  pathToCompareFile)
        {

            bool result = false;
            string diffFile = null;
            XmlDiff diff = new XmlDiff();
            XmlTextReader orig = null;
            XmlDiffOptions diffOptions = new XmlDiffOptions();
            StreamWriter sw1 = null;
            XmlTextReader diffGram = null;

            //output diff file.
            diffFile = Path.GetDirectoryName(responseFileReal) + "\\vxd.out";
            XmlTextWriter tw = new XmlTextWriter(new StreamWriter(diffFile));
            tw.Formatting = Formatting.Indented;

            //This method sets the diff.Options property.
            diffOptions = XmlDiffOptions.None;
            diff.Options = diffOptions;

            bool isEqual = false;

            //Now compare the two files.
            try
            {
                isEqual = diff.Compare(responseFile, responseFileReal, false, tw);
            }
            catch (XmlException ex)
            {
                pathToCompareFile = "";
                resultMessage = ex.Message;
                result = false;
                return result;
            }
            finally
            {
                tw.Close();
            }

            if (isEqual)
            {
                //This means the files were identical for given options.
                result = true;
                pathToCompareFile = "";
                resultMessage = "Files are equal.";
                return result; //dont need to show the differences.
            }

            //Files were not equal, so construct XmlDiffView.
            XmlDiffView dv = new XmlDiffView();

            result = false;
            resultMessage = "Files are not equal.";

            try
            {
                //Load the original file again and the diff file.
                orig = new XmlTextReader(responseFile);
                diffGram = new XmlTextReader(diffFile);
                dv.Load(orig,
                    diffGram);


                //Wrap the HTML file with necessary html and 
                //body tags and prepare it before passing it to the GetHtml method.

                sw1 = new StreamWriter(pathToCompareFile);


                sw1.Write("<html><body><table width='100%'>");
                //Write Legend.
                sw1.Write("<tr><td colspan='2' align='center'><b>Legend:</b> <font style='background-color: yellow'" +
                    " color='black'>added</font>&nbsp;&nbsp;<font style='background-color: red'" +
                    " color='black'>removed</font>&nbsp;&nbsp;<font style='background-color: " +
                    "lightgreen' color='black'>changed</font>&nbsp;&nbsp;" +
                    "<font style='background-color: red' color='blue'>moved from</font>" +
                    "&nbsp;&nbsp;<font style='background-color: yellow' color='blue'>moved to" +
                    "</font>&nbsp;&nbsp;<font style='background-color: white' color='#AAAAAA'>" +
                    "ignored</font></td></tr>");


                sw1.Write("<tr><td><b> File Name : ");
                sw1.Write(responseFile);
                sw1.Write("</b></td><td><b> File Name : ");
                sw1.Write(responseFileReal);
                sw1.Write("</b></td></tr>");

                //This gets the differences but just has the 
                //rows and columns of an HTML table
                dv.GetHtml(sw1);

                //Finish wrapping up the generated HTML and complete the file.
                sw1.Write("</table></body></html>");


            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
                result = false;
                return result; //dont need to show the differences.
            }
            finally
            {
                //HouseKeeping...close everything we dont want to lock.
                if (sw1 != null) { sw1.Close(); };
                dv = null;
                if (orig != null) { orig.Close(); };
                if (diffGram != null) { diffGram.Close(); };
            }

            resultMessage = "File are not equal.";
            return result;
        }


        #endregion //Other

        #region Properties

        public string TestId
        {
            get 
            {
                return m_test.Attributes.GetNamedItem("id").InnerText; 
            }
        }

        public string Command
        {
            get
            {
                return m_test.ParentNode.Attributes.GetNamedItem("command").InnerText;
            }
        }

        public string Name
        {
            get
            {
                return m_test.SelectSingleNode("Name").InnerText;
            }
        }

        #endregion //Properties


        #region Fields

        private XmlNode m_test;
        private string m_pathToTests;
        private string m_pathToTestsResult;

        #endregion //Fields
    }
}
