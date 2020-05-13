/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;

namespace ONVIF_TestSummary
{
    public class TestSummary
    {
        // public structure used to send all the test information when creating the test report
        public struct TestSummary_TestInfo_Type
        {
            public string ToolVersion;
            public string TestVersion;
            public string CoreVersion;

            public string Device_Brand;
            public string Device_Model;
            public string Device_SerialNumber;
            public string Device_FWversion;
            public string Device_Other;

            public string Operator;
            public string OrganizationName;
            public string OrganizationAddress;

            public string TestDateAndTime;
        }

        private bool CreateIndex = false;

        private int SuiteCounter = 1;
        private bool verboseOutput = false;

        /// <summary>
        /// Retrieve the ONVIF logo from the encoded assembly
        /// </summary>
        /// <returns>Stream</returns>
        private Stream GetLogo()
        {
            Stream imgStream = null;
            
            // get a reference to the current assembly
            Assembly a = Assembly.GetExecutingAssembly();

            // get a list of resource names from the manifest
            string[] resNames = a.GetManifestResourceNames();

            // look for the logo
            foreach (string s in resNames)
            {
                // locate the logo.gif in the assembly
                if (s.EndsWith("logo.gif"))
                {
                    // attach to stream to the resource in the manifest
                    imgStream = a.GetManifestResourceStream(s);
                    break;
                }
            }

            return imgStream;
        }


        /// <summary>
        /// Build a test summary and save it to the specified URL
        /// </summary>
        /// <param name="fileName">Save location for test summary</param>
        /// <param name="testHeader">Test summary header information</param>
        /// <param name="testData">Test summary data information</param>
        /// <returns>passed indicator</returns>
        public bool WriteTestSummary(string fileName, TestSummary_TestInfo_Type TestInfo, ONVIF_TestCases.TestCases_Class.TestGroup_Type Tests)
        {
            bool passed = false;
            Document myDocument = new Document(PageSize.A4);

            //ChapterCount = 1;

            // make sure the date has been set
            if(TestInfo.TestDateAndTime == "")
                TestInfo.TestDateAndTime = String.Format("{0} @ {1}\n", System.DateTime.Now.ToShortDateString(), System.DateTime.Now.ToLongTimeString());
            

            if(fileName.Contains("verbose"))
                verboseOutput = true;
            else
                verboseOutput = false;


            try
            {
                // create a new instance of the results file
                PdfWriter.GetInstance(myDocument, new FileStream(fileName, FileMode.Create));

                myDocument.Open();
                
                // build the footer with the ONVIF required information
                HeaderFooter footer = new HeaderFooter(new Phrase("Device - " + TestInfo.Device_Model + " " + TestInfo.TestDateAndTime + " ONVIF Test Report Page: "), true);
                footer.Border = Rectangle.NO_BORDER;
                footer.Alignment = Element.ALIGN_CENTER;
                myDocument.Footer = footer;

                BuildCoverPage(myDocument, TestInfo);

                // add summary
                BuildSummary(myDocument, Tests);

                // add index
                if (CreateIndex)
                    BuildIndex(myDocument, Tests);
                
                // add test data
                AddTestGroupInfo(myDocument, Tests);

               
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
                passed = false;
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
                passed = false;
            }

            // don't forget to close the file
            myDocument.Close();

            return passed;
        }

        /// <summary>
        /// Build index page
        /// </summary>
        /// <param name="myDocument">Document</param>
        /// <param name="Tests">Tests</param>
        private void BuildIndex(Document myDocument, ONVIF_TestCases.TestCases_Class.TestGroup_Type Tests)
        {
            myDocument.NewPage();

            Paragraph p1 = new Paragraph(new Chunk("Tests\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p1.Alignment = Element.ALIGN_CENTER;
            myDocument.Add(p1);

            foreach (ONVIF_TestCases.TestCases_Class.TestSuite_Type Suite in Tests.Group)
            {
                foreach (ONVIF_TestCases.TestCases_Class.Test_Type Test in Suite.Tests)
                {

                    string line = String.Format("{0, -13} {1}\n", Test.Number, Test.Name);
                    Anchor anchor = new Anchor(line, FontFactory.GetFont(FontFactory.TIMES, 12));
                    anchor.Reference = "#" + Test.Name;
                    myDocument.Add(anchor);
                }
            }


        }

        /// <summary>
        /// Build Cover page
        /// </summary>
        /// <param name="myDocument">Document</param>
        /// <param name="TestInfo">Tests</param>
        private void BuildCoverPage(Document myDocument, TestSummary_TestInfo_Type TestInfo)
        {
            Stream logoStream = GetLogo();


            // if the logo was found add it to the file
            if (logoStream != null)
            {
                Image logo = Image.GetInstance(logoStream);
                logo.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(logo);
            }
            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));

            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            p0.Add(new Chunk("ONVIF Conformance Test", FontFactory.GetFont(FontFactory.TIMES_BOLD, 20)));
            p0.Add("\n\n");

            p0.Add(new Chunk("Performed by", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");
            
            p0.Add(String.Format("Operator - {0}\n", TestInfo.Operator));
            p0.Add(String.Format("Organization - {0}\n", TestInfo.OrganizationName));
            p0.Add(String.Format("Address - {0}\n", TestInfo.OrganizationAddress));
            
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add(new Chunk("Device Under Test", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");
            p0.Add(String.Format("Brand - {0}\n", TestInfo.Device_Brand));
            p0.Add(String.Format("Model - {0}\n", TestInfo.Device_Model));
            p0.Add(String.Format("Serial Number - {0}\n", TestInfo.Device_SerialNumber));
            p0.Add(String.Format("Firmware Version - {0}\n", TestInfo.Device_FWversion));
            p0.Add(String.Format("Other - {0}\n", TestInfo.Device_Other));

            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            // add the ONVIF information

            //p0.Add("ONVIF Test Tool Version\n");
            p0.Add(String.Format("{0}", TestInfo.ToolVersion));
            p0.Add(Environment.NewLine);
            //p0.Add("ONVIF Test Specification Version\n");
            p0.Add(String.Format("{0}", TestInfo.TestVersion));
            p0.Add(Environment.NewLine);
            //p0.Add("ONVIF Core Specification Version\n");
            p0.Add(String.Format("{0}", TestInfo.CoreVersion));
            p0.Alignment = Element.ALIGN_CENTER;


            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add("Test Date and Time - " + TestInfo.TestDateAndTime);

            myDocument.Add(p0);
          
        }

        /// <summary>
        /// Build Test summary
        /// </summary>
        /// <param name="myDocument">Document</param>
        /// <param name="Tests">Tests</param>
        private void BuildSummary(Document myDocument, ONVIF_TestCases.TestCases_Class.TestGroup_Type Tests)
        {
            myDocument.NewPage();
            int testCount, optional_Skipped, manditory_Skipped;
            int testsRan, testsPassed, testsFailed;

            int optionalCount, optionalFailed, optionalPassed;

            testCount = 0;
            optional_Skipped = 0;
            manditory_Skipped = 0;
            //marked_Failed = 0;
            //marked_Passed = 0;

            testsRan = 0;
            testsPassed = 0;
            testsFailed = 0;

            optionalCount = 0;
            optionalFailed = 0;
            optionalPassed = 0;

            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("ONVIF Test Summary\n");
            p0.Alignment = Element.ALIGN_CENTER;
            myDocument.Add(p0);
            
            if (!Tests.ONVIF_Conformace_Test)
            {
                Paragraph p1 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                p1.Add("THIS IS NOT A VALID ONVIF CONFORMANCE TEST\n\n");
                p1.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(p1);
            }

            Paragraph p_tests;

            foreach (ONVIF_TestCases.TestCases_Class.TestSuite_Type suite in Tests.Group)
            {
                foreach (ONVIF_TestCases.TestCases_Class.Test_Type test in suite.Tests)
                {
                    testCount++;                    

                    p_tests = new Paragraph(new Chunk(test.Name, FontFactory.GetFont(FontFactory.TIMES, 12)));
                    
                    // if complete look at the details
                    if (test.TestComplete)
                    {

                        // if it was a "skipped" test it isn't valid
                        if (test.TestSkipped)
                        {
                            // if this test wasn't required don't count it against the user

                            if ((test.Compliance == ONVIF_TestCases.TestCases_Class.TestCompliance.Must) ||
                                (test.Compliance == ONVIF_TestCases.TestCases_Class.TestCompliance.Must_if_Supported))
                                manditory_Skipped++;
                            else
                                optional_Skipped++;
                        }
                        else
                        {
                            testsRan++;

                            if (test.Compliance == ONVIF_TestCases.TestCases_Class.TestCompliance.Optional)
                            {
                                optionalCount++;
                                if (test.TestPassed)
                                    optionalPassed++;
                                else
                                    optionalFailed++;
                            }
                            else
                            {

                                if (test.TestPassed)
                                    testsPassed++;
                                else
                                    testsFailed++;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }


            Paragraph p2 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            p2.Add(String.Format("Test Count: {0}\n", testCount));
            p2.Add(String.Format("Manditory Tests Skipped: {0}\n", manditory_Skipped));
            p2.Add(String.Format("Optional Tests Skipped: {0}\n", optional_Skipped));
            p2.Add(String.Format("Tests Executed: {0}\n", testsRan));
            p2.Add(String.Format("Tests Passed:  {0}\n", testsPassed + optionalPassed));
            p2.Add(String.Format("Tests Failed:  {0}\n", testsFailed));


            if (optionalFailed > 0)
            {
                p2.Add(String.Format("Optional Tests Failed: {1}\n", "", optionalFailed));
            }

            p2.Add("\n");

            //p2.Add(String.Format("Test executed but marked \"FAILED\": {1}\n", "", marked_Failed));
            //p2.Add(String.Format("Test executed but marked \"PASSED\": {1}\n", "", marked_Passed));
            //p2.Add(String.Format("Tests executed but marked \"SKIPPED\": {1}\n", "", marked_Skipped));

            myDocument.Add(p2);

            // if not all tests were run this is not a valid conformance test
            if(testCount != (testsRan + optional_Skipped))
            {
                Paragraph p3 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                p3.Add("NOT ALL TESTS RUN, NOT A VALID ONVIF CONFORMANCE TEST\n\n");
                p3.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(p3);
            }
            else if (!Tests.ONVIF_Conformace_Test)
            {
                Paragraph p4 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                p4.Add("THIS IS NOT A VALID ONVIF CONFORMANCE TEST\n\n");
                p4.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(p4);
            }

            

        }

        /// <summary>
        /// Add Test Group information to document
        /// </summary>
        /// <param name="myDocument">Document</param>
        /// <param name="Tests">Tests</param>
        private void AddTestGroupInfo(Document myDocument, ONVIF_TestCases.TestCases_Class.TestGroup_Type Tests)
        {
            myDocument.NewPage();
            Chapter aChapter;

            // go through each test suite and record all the info
            if (Tests.Name != null)
            {
                Paragraph p0 = new Paragraph(new Chunk(Tests.Name + "\n\n", FontFactory.GetFont(FontFactory.TIMES, 18)));
                //p0.Alignment = Element.ALIGN_CENTER;
                //myDocument.Add(p0);


                aChapter = new Chapter(p0, 8);
                aChapter.BookmarkTitle = Tests.Name;
                
            }
            else
            {
                Paragraph p0 = new Paragraph(new Chunk("ONVIF TEST" + "\n\n", FontFactory.GetFont(FontFactory.TIMES, 18)));

                aChapter = new Chapter(p0, 1);
                aChapter.BookmarkTitle = Tests.Name;
            }
            aChapter.NumberDepth = 0;
            myDocument.Add(aChapter);
            if (Tests.Group != null)
            {
                foreach (ONVIF_TestCases.TestCases_Class.TestSuite_Type Suite in Tests.Group)
                {
                    AddTestSuiteInfo(myDocument, Suite, ref aChapter);
                }
            }
        }
        
        /// <summary>
        /// Add Test Suite information to document
        /// </summary>
        /// <param name="myDocument">Document</param>
        /// <param name="Suite">Test Suite</param>
        /// <param name="aChapter">Current chapter</param>
        private void AddTestSuiteInfo(Document myDocument, ONVIF_TestCases.TestCases_Class.TestSuite_Type Suite, ref Chapter aChapter)
        {
            
            Section section;
            int sectionNumber = 1;
            //myDocument.NewPage();
            // go throug the suite and display all the test info
            if (Suite.Description != null)
            {
                Paragraph p0 = new Paragraph(new Chunk(Suite.Description + "\n\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                section = aChapter.AddSection(p0);
            }
            else
            {
                Paragraph p0 = new Paragraph(new Chunk("Test Suite - " + SuiteCounter.ToString() + "\n\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                section = aChapter.AddSection(p0);
            }

            SuiteCounter++;
            section.NumberDepth = 0;
            myDocument.Add(section);
                //myDocument.Add(new iTextSharp.text.Paragraph(Suite.Description + Environment.NewLine));

            if (Suite.Tests != null)
            {
                foreach (ONVIF_TestCases.TestCases_Class.Test_Type Test in Suite.Tests)
                {
                    AddTestTestInfo(myDocument, Test, ref section, ++sectionNumber);
                }

            }

            
        }

        /// <summary>
        /// Add Test, Test Case information to document
        /// </summary>
        /// <param name="myDocument">Document</param>
        /// <param name="Test">Test</param>
        /// <param name="section">Current Section</param>
        /// <param name="sectionNumber">Section Number</param>
        private void AddTestTestInfo(Document myDocument, ONVIF_TestCases.TestCases_Class.Test_Type Test, ref Section section, int sectionNumber)
        {
            int x;
            Anchor anchor1;

            

            Paragraph subTitle = new Paragraph(Test.Number + " - " + Test.Name);

            // if building an index page add the anchor
            if (CreateIndex)
            {
                anchor1 = new Anchor(".");
                anchor1.Name = Test.Name;
                subTitle.Add(anchor1);
            }

            if (Test.Compliance == ONVIF_TestCases.TestCases_Class.TestCompliance.Optional)
                subTitle.Add("*\n   *Optional Test");

            Section subSection = section.AddSection(50f, subTitle, sectionNumber);

            subSection.BookmarkTitle = Test.Name;
            subSection.NumberDepth = 0;
            

            //section1.Title = new Paragraph(Test.Name);

            myDocument.Add(subSection);

            //if (Test.Compliance == ONVIF_TestCases.TestCases_Class.TestCompliance.Optional)
            //{
            //    Paragraph p3 = new Paragraph(new Chunk("", FontFactory.GetFont(FontFactory.TIMES, 14)));
            //    p3.Add("OPTIONAL TEST\n");
            //    myDocument.Add(p3);
            //}

            // if this test wasn't run just indicate that to reduce confusion
            if (!Test.TestComplete)
            {
                myDocument.Add(new iTextSharp.text.Paragraph("Test not run"));
            }
            else
            {



                myDocument.Add(new iTextSharp.text.Paragraph("Test Results"));

                myDocument.Add(new iTextSharp.text.Paragraph(Test.Results));
                myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                if (verboseOutput)
                {
                    for (x = 0; x < Test.CurrentStep; x++)
                    {
                        //Paragraph subsubTitle = new Paragraph("Step " + (x + 1).ToString() + " information");
                        //Section subSection2 = subSection.AddSection(50f, subsubTitle, sectionNumber);
                        //myDocument.Add(subSection2);
                        Paragraph p0 = new Paragraph(new Chunk("Step " + (x + 1).ToString() + " information", FontFactory.GetFont(FontFactory.TIMES_BOLD, 14)));
                        myDocument.Add(p0);

                        //myDocument.Add(new iTextSharp.text.Paragraph("Step " + (x + 1).ToString() + " information"));
                        //myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));

                        if (Test.SoapError(x) != "")
                        {
                            myDocument.Add(new iTextSharp.text.Paragraph("Soap Errors"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Test.SoapError(x)));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }
                        else
                        {
                            myDocument.Add(new iTextSharp.text.Paragraph("No Soap Error received"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }

                        if (Test.SoapError(x) != "")
                        {

                            myDocument.Add(new iTextSharp.text.Paragraph("XML Errors"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Test.XML_Error(x)));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }
                        else
                        {
                            myDocument.Add(new iTextSharp.text.Paragraph("No XML Error received"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }

                        if (Test.XML_MessageSent(x) != "")
                        {
                            myDocument.Add(new iTextSharp.text.Paragraph("Message sent"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Test.XML_MessageSent(x)));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }
                        else
                        {
                            myDocument.Add(new iTextSharp.text.Paragraph("No Message sent"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }

                        if (Test.XML_MessageReceived(x) != "")
                        {
                            myDocument.Add(new iTextSharp.text.Paragraph("Message received"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Test.XML_MessageReceived(x)));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }
                        else
                        {
                            myDocument.Add(new iTextSharp.text.Paragraph("No Message received"));
                            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
                        }
                    }

                }
            }

            //myDocument.NewPage();


            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));
            myDocument.Add(new iTextSharp.text.Paragraph(Environment.NewLine));


        }

    }
}
