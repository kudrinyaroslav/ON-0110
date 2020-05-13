using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ONVIFTestTool
{
    /// <summary>
    /// Class for Test Suit operations
    /// </summary>
    class TestSuit:IEnumerable<Test>
    {

        #region Constructors

        /// <summary>
        /// TestSuit default constructor
        /// </summary>
        /// <param name="pathToTestResult">Folder for test results (also used for requests and response file preparation)</param>
        public TestSuit(string pathToTestResult)
        {
            m_testSuitXML = null;
            m_pathToTestResult = pathToTestResult;
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Open test suit from file
        /// </summary>
        /// <param name="pathToTestSuit"> Path to XML file with test suit description</param>
        /// <returns>Null if all OK, error message in other case</returns>
        public string Open (string pathToTestSuit)
        {
            //TODO: verification

            XmlReader reader = null;
            string result = null;

            try
            {

                reader = XmlReader.Create(pathToTestSuit);
                m_testSuitXML = new XmlDocument();
                m_testSuitXML.Load(reader);

                //Select enabled tests
                m_testList = m_testSuitXML.SelectNodes("/TestSuit/TestList/Service[@enabled = \"true\"]/TestGroup[@enabled = \"true\"]/Test[@enabled = \"true\"]");

                if (m_pathToTestResult != null)
                {
                    m_pathToTestSuit = pathToTestSuit;
                    ApplyParameters();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = ex.Message;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return result;

        }

        /// <summary>
        /// Prepare request files and response files for using in the test:
        /// 1) Replace parameters
        /// 2) Copy to test result folder
        /// </summary>
        private void ApplyParameters()
        { 
            foreach (XmlNode test in m_testList)
            {
                foreach (XmlNode step in test.ChildNodes)
                {
                    if (string.Compare(step.Name, "Step") == 0)
                    {
                        string pathToRequestFile = PathToTests + step.Attributes.GetNamedItem("fileRequest").InnerText;
                        string pathToRequestFileNew = m_pathToTestResult + "TestFiles\\" + step.Attributes.GetNamedItem("fileRequest").InnerText;
                        string pathToResponseFile = PathToTests + step.Attributes.GetNamedItem("fileAnswer").InnerText;
                        string pathToResponseFileNew = m_pathToTestResult + "TestFiles\\" + step.Attributes.GetNamedItem("fileAnswer").InnerText;

                        Directory.CreateDirectory(Path.GetDirectoryName(pathToRequestFileNew));

                        Directory.CreateDirectory(Path.GetDirectoryName(pathToResponseFileNew));

                        if (step.Attributes.GetNamedItem("parameter") != null)
                        {
                            string parametersString = step.Attributes.GetNamedItem("parameter").InnerText;
                            string[] parametersList = parametersString.Split(new char[] { ',' });

                            ReplaceParameter(pathToRequestFile, pathToRequestFileNew, parametersList);
                            ReplaceParameter(pathToResponseFile, pathToResponseFileNew, parametersList);

                        }
                        else
                        {
                            File.Copy(pathToRequestFile, pathToRequestFileNew, true);
                            File.SetAttributes(pathToRequestFileNew, FileAttributes.Normal);
                            File.Copy(pathToResponseFile, pathToResponseFileNew, true);
                            File.SetAttributes(pathToResponseFileNew, FileAttributes.Normal);
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Replace parameters in requests and response files
        /// </summary>
        /// <param name="pathToFile">Path to initial file</param>
        /// <param name="pathToFileNew">Path to file with applyed parameters</param>
        /// <param name="parametersList">Parameters list for apply</param>
        private void ReplaceParameter(string pathToFile, string pathToFileNew, string[] parametersList)
        {
            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                content = reader.ReadToEnd();
            }

            foreach (string parameter in parametersList)
            {
                content = content.Replace(parameter, m_testSuitXML.SelectSingleNode("/TestSuit/Parameters/" + parameter).InnerText);
            }
            using (StreamWriter writer = new StreamWriter(pathToFileNew))
            {
                writer.Write(content);
                writer.Flush();
                writer.Close();
            }
        }

        #endregion //Methods
        
        #region Properties

        /// <summary>
        /// Return information about test suit (name, version, date, camera info)
        /// </summary>
        public string TestSuitInfo
        {
            get 
            {
                string result = "";

                if (m_testSuitXML != null)
                {
                    result = "Name: ";
                    result = result + m_testSuitXML.SelectSingleNode("/TestSuit/Info/Name").InnerText;
                    result = result + "\r\n";

                    result = result + "Version: ";
                    result = result + m_testSuitXML.SelectSingleNode("/TestSuit/Info/Version").InnerText;
                    result = result + "\r\n";

                    result = result + "Date: ";
                    result = result + m_testSuitXML.SelectSingleNode("/TestSuit/Info/Date").InnerText;
                    result = result + "\r\n";

                    result = result + "Camera Model: ";
                    result = result + m_testSuitXML.SelectSingleNode("/TestSuit/Info/Camera/Model").InnerText;
                    result = result + "\r\n";

                    result = result + "Path to Tests: ";
                    result = result + m_testSuitXML.SelectSingleNode("/TestSuit/Info/PathToTests").InnerText;
                    result = result + "\r\n";
                }

                return result;
            }
        }

        /// <summary>
        /// Test name
        /// </summary>
        public string Name
        {
            get
            {
                string result = "";

                if (m_testSuitXML != null)
                {
                    result = m_testSuitXML.SelectSingleNode("/TestSuit/Info/Name").InnerText;
                }

                return result;
            }
        }

        /// <summary>
        /// Return XMLDocument with test suit
        /// </summary>
        public XmlDocument XMLDocument
        {
            get
            {
                return m_testSuitXML;
            }
        }

        /// <summary>
        /// Camra model
        /// </summary>
        public string CamraModel
        {
            get
            {
                string result = "";

                if (m_testSuitXML != null)
                {
                    result = m_testSuitXML.SelectSingleNode("/TestSuit/Info/Camera/Model").InnerText;
                }

                return result;
            }
        }

        /// <summary>
        /// Test suit date
        /// </summary>
        public string Date
        {
            get
            {
                string result = "";

                if (m_testSuitXML != null)
                {
                    result = m_testSuitXML.SelectSingleNode("/TestSuit/Info/Date").InnerText;
                }

                return result;
            }
        }

        /// <summary>
        /// Test suit version
        /// </summary>
        public string Version
        {
            get
            {
                string result = "";

                if (m_testSuitXML != null)
                {
                    result = m_testSuitXML.SelectSingleNode("/TestSuit/Info/Version").InnerText;
                }

                return result;
            }
        }

        /// <summary>
        /// Return path to initial test files
        /// </summary>
        public string PathToTests
        {
            get
            {
                string result = "";

                if (m_testSuitXML != null)
                {
                    result = Path.Combine(Path.GetDirectoryName(m_pathToTestSuit),m_testSuitXML.SelectSingleNode("/TestSuit/Info/PathToTests").InnerText);
                }

                return result;
            }
        }

        #endregion //Properties

        #region Fields

        private XmlDocument m_testSuitXML;
        private XmlNodeList m_testList;
        private String m_pathToTestResult;
        private String m_pathToTestSuit;

        #endregion //Fields

        #region IEnumerable<Test> Members

        /// <summary>
        /// IEnumerator interface implementation for Test enumeration
        /// </summary>
        /// <returns>Return Test</returns>
        public IEnumerator<Test> GetEnumerator()
        {
            foreach (XmlNode test in m_testList)
            {
                yield return new Test(test, m_pathToTestResult + "TestFiles\\", m_pathToTestResult);
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// IEnumerator interface implementation for Test enumeration
        /// </summary>
        /// <returns>Return Test</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (XmlNode test in m_testList)
            {
                yield return new Test(test, m_pathToTestResult + "TestFiles\\", m_pathToTestResult);
            }
        }

        #endregion
    }
}
