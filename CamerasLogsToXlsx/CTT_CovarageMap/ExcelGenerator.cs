using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CTT_CovarageMap
{
    static class ExcelGenerator
    {
        public static void Convert(String pathFolder, string profile)
        {
            const string XPath_SelectFeatures = "/db:book/db:chapter/db:section[starts-with(@xml:id,'tc.')]";
            const string XPath_SelectSupplimentaryTestCases = "/db:book/db:chapter[@xml:id='tc.Profile.ProfileSupplementaryFeatures']/db:section[starts-with(@xml:id,'tc.')]";
            const string XPath_GetFeatureRequerementLevel = "db:section/db:para[db:emphasis/text()='Profile {0} Requirement:' and contains(text(),'{1}')]";
            const string XPath_GetFeatureLinkToProfile = "db:section/db:para/db:emphasis[text()='Profile {0} Requirement:']/@annotations";
            const string XPath_GetTestCaseRequerementLevel = "db:para[db:emphasis/text()='Profile {0} Normative Reference:' and contains(text(),'{1}')]";
            const string XPath_GetTestCaseLinkToProfile = "db:para/db:emphasis[text()='Profile {0} Normative Reference:']/@annotations";

            List<Feature> featuresList = new List<Feature>();

            ExcelFile myExcel = new ExcelFile(Path.Combine(pathFolder, String.Format("Profile{0}_ClientTestCoverageMap.xlsx", profile)));

            myExcel.AddFeatureListWS();
            myExcel.AddHeader_FeatureList();


            //Select from specs

            foreach (String path in Directory.GetFiles(pathFolder, String.Format("Done_ONVIF_Profile_{0}_Client_Test_Specification.xml", profile)))
            {

                XmlDocument testSpec = new XmlDocument();

                testSpec.Load(path);

                XmlNamespaceManager xmlNM = new XmlNamespaceManager(testSpec.NameTable);
                xmlNM.AddNamespace("db", "http://docbook.org/ns/docbook");
                xmlNM.AddNamespace("xml", "http://www.w3.org/XML/1998/namespace");

                XmlNodeList featuresSpec = testSpec.DocumentElement.SelectNodes(XPath_SelectFeatures, xmlNM);

                foreach (XmlNode featireSpec in featuresSpec)
                {
                    RequirementLevel featireReqL = RequirementLevel.None;

                    if (featireSpec.SelectSingleNode(String.Format(XPath_GetFeatureRequerementLevel, profile, RequirementLevel.Conditional.ToString()), xmlNM) != null)
                    {
                        featireReqL = RequirementLevel.Conditional;
                    }

                    if (featireSpec.SelectSingleNode(String.Format(XPath_GetFeatureRequerementLevel, profile, RequirementLevel.Mandatory.ToString()), xmlNM) != null)
                    {
                        featireReqL = RequirementLevel.Mandatory;
                    }

                    if (featireSpec.SelectSingleNode(String.Format(XPath_GetFeatureRequerementLevel, profile, RequirementLevel.Optional.ToString()), xmlNM) != null)
                    {
                        featireReqL = RequirementLevel.Optional;
                    }

                    if (featireReqL != RequirementLevel.None)
                    {
                        string link = null;

                        if (featireSpec.SelectSingleNode(String.Format(XPath_GetFeatureLinkToProfile, profile), xmlNM) != null)
                        {
                            link = featireSpec.SelectSingleNode(String.Format(XPath_GetFeatureLinkToProfile, profile), xmlNM).Value;
                        }


                        Feature feature = new Feature(featireSpec.SelectSingleNode("db:title", xmlNM).InnerText, featireSpec.SelectSingleNode("@xml:id", xmlNM).Value.Substring(3), featireReqL, link);

                        featuresList.Add(feature);
                        


                        foreach (XmlNode testCase in featireSpec.SelectNodes("db:section[position()>2]", xmlNM))
                        {
                            RequirementLevel testCaseReqL = RequirementLevel.None;

                            if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseRequerementLevel, profile, RequirementLevel.Conditional.ToString()), xmlNM) != null)
                            {

                                testCaseReqL = RequirementLevel.Conditional;
                            }

                            if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseRequerementLevel, profile, RequirementLevel.Mandatory.ToString()), xmlNM) != null)
                            {
                                testCaseReqL = RequirementLevel.Mandatory;
                            }

                            if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseRequerementLevel, profile, RequirementLevel.Optional.ToString()), xmlNM) != null)
                            {
                                testCaseReqL = RequirementLevel.Optional;
                            }

                            string FeatureUnderTest = "None";
                            if (testCase.SelectSingleNode("db:para[contains(db:emphasis/text(), 'Feature Under Test:')]", xmlNM) != null)
                            {
                                FeatureUnderTest = testCase.SelectSingleNode("db:para[contains(db:emphasis/text(),'Feature Under Test:')]", xmlNM).LastChild.InnerText.Trim(' ', '\r', '\n');
                            }

                            string ValidatedFeatureList = "None";
                            if (testCase.SelectSingleNode("db:para[contains(db:emphasis/text(),'Validated Feature List:')]", xmlNM) != null)
                            {
                                ValidatedFeatureList = testCase.SelectSingleNode("db:para[contains(db:emphasis/text(),'Validated Feature List:')]", xmlNM).LastChild.InnerText.Trim(' ', '\r', '\n');
                            }

                            string linkTC = null;

                            if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseLinkToProfile, profile), xmlNM) != null)
                            {
                                linkTC = testCase.SelectSingleNode(String.Format(XPath_GetTestCaseLinkToProfile, profile), xmlNM).Value;
                            }

                            TestCase test = new TestCase(testCase.SelectSingleNode("db:title", xmlNM).InnerText, testCase.SelectSingleNode("@xml:id", xmlNM).Value.Substring(3), testCaseReqL, FeatureUnderTest, linkTC);

                            feature.TestCases.Add(test);
                        }
                    }
                }

                Feature featureSup = new Feature("Supplimentary Test Cases", "Supplimentary", RequirementLevel.Supplimentary, "");

                featuresList.Add(featureSup);

                foreach (XmlNode testCase in testSpec.DocumentElement.SelectNodes(XPath_SelectSupplimentaryTestCases, xmlNM))
                {
                    RequirementLevel testCaseReqL = RequirementLevel.None;

                    if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseRequerementLevel, profile, RequirementLevel.Conditional.ToString()), xmlNM) != null)
                    {

                        testCaseReqL = RequirementLevel.Conditional;
                    }

                    if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseRequerementLevel, profile, RequirementLevel.Mandatory.ToString()), xmlNM) != null)
                    {
                        testCaseReqL = RequirementLevel.Mandatory;
                    }

                    if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseRequerementLevel, profile, RequirementLevel.Optional.ToString()), xmlNM) != null)
                    {
                        testCaseReqL = RequirementLevel.Optional;
                    }

                    string FeatureUnderTest = "None";
                    if (testCase.SelectSingleNode("db:para[contains(db:emphasis/text(), 'Feature Under Test:')]", xmlNM) != null)
                    {
                        FeatureUnderTest = testCase.SelectSingleNode("db:para[contains(db:emphasis/text(),'Feature Under Test:')]", xmlNM).LastChild.InnerText.Trim(' ', '\r', '\n');
                    }

                    //string ValidatedFeatureList = "None";
                    //if (testCase.SelectSingleNode("db:para[contains(db:emphasis/text(),'Validated Feature List:')]", xmlNM) != null)
                    //{
                    //    ValidatedFeatureList = testCase.SelectSingleNode("db:para[contains(db:emphasis/text(),'Validated Feature List:')]", xmlNM).LastChild.InnerText.Trim(' ', '\r', '\n');
                    //}

                    string linkTC = null;

                    if (testCase.SelectSingleNode(String.Format(XPath_GetTestCaseLinkToProfile, profile), xmlNM) != null)
                    {
                        linkTC = testCase.SelectSingleNode(String.Format(XPath_GetTestCaseLinkToProfile, profile), xmlNM).Value;
                    }

                    TestCase test = new TestCase(testCase.SelectSingleNode("db:title", xmlNM).InnerText, testCase.SelectSingleNode("@xml:id", xmlNM).Value.Substring(3), testCaseReqL, FeatureUnderTest, linkTC);

                    featureSup.TestCases.Add(test);
                }
            }

            //Adding to Excel

            myExcel.AddSection(RequirementLevel.Mandatory.ToString());

            foreach (Feature feature in featuresList.Where(C => C.RequirementLevel == RequirementLevel.Mandatory))
            {

                myExcel.AddFeature(feature);

                foreach (var testCase in feature.TestCases)
                {
                    myExcel.AddTestCase(testCase);
                }
            }

            myExcel.AddSection(RequirementLevel.Conditional.ToString());

            foreach (Feature feature in featuresList.Where(C => C.RequirementLevel == RequirementLevel.Conditional))
            {
                myExcel.AddFeature(feature);
                foreach (var testCase in feature.TestCases)
                {
                    myExcel.AddTestCase(testCase);
                }
            }

            myExcel.AddSection(RequirementLevel.Optional.ToString());

            foreach (Feature feature in featuresList.Where(C => C.RequirementLevel == RequirementLevel.Optional))
            {
                myExcel.AddFeature(feature);
                foreach (var testCase in feature.TestCases)
                {
                    myExcel.AddTestCase(testCase);
                }
            }

            myExcel.AddSection(RequirementLevel.Supplimentary.ToString());

            foreach (Feature feature in featuresList.Where(C => C.RequirementLevel == RequirementLevel.Supplimentary))
            {
                myExcel.AddFeature(feature);
                foreach (var testCase in feature.TestCases)
                {
                    myExcel.AddTestCase(testCase);
                }
            }

            myExcel.AddEventsNotes();


            myExcel.Save();
        }
    }
}
