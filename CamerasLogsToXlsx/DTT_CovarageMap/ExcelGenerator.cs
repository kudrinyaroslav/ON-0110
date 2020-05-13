using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DTT_CovarageMap
{
    static class ExcelGenerator
    {
        public static void Convert(String coverageMapFolder, String testResultsPath, string profile)
        {
            const string XPath_SelectTestCases_ConformanceMode = "/DebugReport/Results/TestResult/TestInfo[FunctionalityUnderTest/Functionality/text()='{0}' and RequirementLevel/text()='Must']/Name";
            const string XPath_SelectTestCases_DiagnosticMode = "/DebugReport/Results/TestResult/TestInfo[FunctionalityUnderTest/Functionality/text()='{0}' and RequirementLevel/text()='Optional']/Name";
            const string XPath_SelectTestCases_NoFunctionality = "/DebugReport/Results/TestResult/TestInfo[not(FunctionalityUnderTest/node())]/Name";
            const string XPath_SelectTestCases_All = "/DebugReport/Results/TestResult/TestInfo[FunctionalityUnderTest/node()]/Name";

            ExcelFile myExcel = new ExcelFile(Path.Combine(coverageMapFolder, String.Format("Profile{0}_DeviceTestCoverageMap.xlsx", profile)));

            myExcel.AddTestCasesListWS();
            //myExcel.AddHeader_FeatureList();


            //Select from specs


            XmlDocument testResults = new XmlDocument();

            testResults.Load(testResultsPath);

            List<ProfileFeature> featureList = myExcel.GetProfileFeatureList();

            XmlNodeList testCases;
            String testCasesList;
            List<String> MappedToProfile = new List<string>();

            foreach (ProfileFeature profileFeature in featureList)
            {
                testCases = testResults.DocumentElement.SelectNodes(String.Format(XPath_SelectTestCases_ConformanceMode, profileFeature.FeatureID));

                testCasesList = "";

                foreach(XmlNode testCase in testCases)
                {
                    testCasesList = testCasesList + testCase.InnerText + "\n";
                    if (!MappedToProfile.Contains(testCase.InnerText))
                    {
                        MappedToProfile.Add(testCase.InnerText);
                    }
                }

                if (testCasesList != "")
                {
                    testCasesList = testCasesList.TrimEnd('\n');
                }

                myExcel.AddTestCases2Feature(profileFeature, testCasesList, true);

                testCases = testResults.DocumentElement.SelectNodes(String.Format(XPath_SelectTestCases_DiagnosticMode, profileFeature.FeatureID));
                
                testCasesList = "";

                foreach (XmlNode testCase in testCases)
                {
                    if (!MappedToProfile.Contains(testCase.InnerText))
                    {
                        MappedToProfile.Add(testCase.InnerText);
                    }
                    testCasesList = testCasesList + testCase.InnerText + "\n";
                }

                if (testCasesList != "")
                {
                    testCasesList = testCasesList.TrimEnd('\n');
                }

                myExcel.AddTestCases2Feature(profileFeature, testCasesList, false);
            }

            XmlNodeList testCasesWithoutFunctionality = testResults.DocumentElement.SelectNodes(XPath_SelectTestCases_NoFunctionality);

            if (testCasesWithoutFunctionality.Count > 0)
            {
                myExcel.AddTestCasesWithoutFunctionality();

                foreach (XmlNode testCase in testCasesWithoutFunctionality)
                {
                    myExcel.AddTestCaseWithoutFunctionality(testCase.InnerText);
                }
            }

            XmlNodeList testCases_All = testResults.DocumentElement.SelectNodes(XPath_SelectTestCases_All);

            myExcel.AddTestCasesOther();

            foreach (XmlNode testCase in testCases_All)
            {
                if (!MappedToProfile.Contains(testCase.InnerText))
                {
                    myExcel.AddTestCaseOther(testCase.InnerText);
                }
            }
            

            myExcel.Save();
        }
    }
}
