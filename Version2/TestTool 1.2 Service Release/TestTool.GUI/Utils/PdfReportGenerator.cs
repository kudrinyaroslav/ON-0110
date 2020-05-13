///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using TestTool.Tests.Common.Trace;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestEngine;
using System.Reflection;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Class with methods for creating PDF report.
    /// </summary>
    class PdfReportGenerator
    {
        private Document _document;
        private Data.TestLogFull _log;
        
        public PdfReportGenerator()
        {
        }

        /// <summary>
        /// Is raised when an exception occurs.
        /// </summary>
        public event Action<Exception> OnException;

        /// <summary>
        /// Is raised when report is saved.
        /// </summary>
        public event Action OnReportSaved;

        /// <summary>
        /// Creates report.
        /// </summary>
        /// <param name="fileName">Path to save report.</param>
        /// <param name="log">Test execution information.</param>
        public void CreateReport(string fileName, Data.TestLogFull log)
        {
            _log = log;
            bool ok = false;
            try
            {
                _document = new Document(PageSize.A4);

                PdfWriter.GetInstance(_document, new FileStream(fileName, FileMode.Create));

                _document.Open();

                // Create Footer 
                _document.Footer = HeaderAndFooter();

                // Create cover page
                BuildCoverPage();

                // Create summary
                BuildSummary();

                // Create index
                BuildIndex();

                // Create test info
                AddTestGroupInfo();

                ok = true;
            }
            catch (Exception ex)
            {
                if (OnException != null)
                {
                    OnException(ex);
                }
            }
            finally
            {
                if (_document.IsOpen())
                {
                    _document.Close();
                }
            }
            if (ok && OnReportSaved != null)
            {
                OnReportSaved();
            }
        }
        
        /// <summary>
        /// Creates header and footer for report.
        /// </summary>
        /// <returns></returns>
        private HeaderFooter HeaderAndFooter()
        {
            HeaderFooter footer = new HeaderFooter(
                new Phrase(string.Format("Device - {0} {1} @ {2}  ONVIF Test Report Page: ",
                _log.DeviceInfo.Model, 
                _log.TestExecutionTime.ToShortDateString(), 
                _log.TestExecutionTime.ToLongTimeString())), true);
            footer.Border = Rectangle.NO_BORDER;
            footer.Alignment = Element.ALIGN_CENTER;

            return footer;
        }

        /// <summary>
        /// Builds cover page with logo and general information.
        /// </summary>
        private void BuildCoverPage()
        {

            //Add logo
            Stream imgStream = null;

            // get a reference to the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // get a list of resource names from the manifest
            string[] resNames = assembly.GetManifestResourceNames();

            // look for the logo
            foreach (string s in resNames)
            {
                // locate the logo.gif in the assembly
                if (s.EndsWith("logo.gif"))
                {
                    // attach to stream to the resource in the manifest
                    imgStream = assembly.GetManifestResourceStream(s);
                    break;
                }
            }
            if (imgStream != null)
            {
                Image logo = Image.GetInstance(imgStream);
                logo.Alignment = Element.ALIGN_CENTER;
                _document.Add(logo);
            }

            //
            _document.Add(new Paragraph(Environment.NewLine));
            _document.Add(new Paragraph(Environment.NewLine));
            _document.Add(new Paragraph(Environment.NewLine));

            //
            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            p0.Add(new Chunk("ONVIF Conformance Test", FontFactory.GetFont(FontFactory.TIMES_BOLD, 20)));
            p0.Add("\n\n");

            p0.Add(new Chunk("Performed by", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");

            p0.Add(String.Format("Operator - {0}\n", _log.TesterInfo.Operator));
            p0.Add(String.Format("Organization - {0}\n", _log.TesterInfo.Organization));
            p0.Add(String.Format("Address - {0}\n", _log.TesterInfo.Address));

            //add empty lines
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add(new Chunk("Device Under Test", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");
            p0.Add(String.Format("Brand - {0}\n", _log.DeviceInfo.Manufacturer));
            p0.Add(String.Format("Model - {0}\n", _log.DeviceInfo.Model));
            p0.Add(String.Format("Serial Number - {0}\n", _log.DeviceInfo.SerialNumber));
            p0.Add(String.Format("Firmware Version - {0}\n", _log.DeviceInfo.FirmwareVersion));
            p0.Add(String.Format("Other - {0}\n", _log.OtherInformation));

            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            // add the ONVIF information

            p0.Add(String.Format("{0}", _log.Application.ToolVersion));
            p0.Add(Environment.NewLine);
            p0.Add(String.Format("{0}", _log.Application.TestSpecification));
            p0.Alignment = Element.ALIGN_CENTER;
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add(string.Format("Test Date and Time - {0} @ {1}", _log.TestExecutionTime.ToShortDateString(), _log.TestExecutionTime.ToLongTimeString() ));
            
            _document.Add(p0);

        }

        /// <summary>
        /// Builds report summary.
        /// </summary>
        private void BuildSummary()
        {
            List<TestInfo> requiredTests = new List<TestInfo>();
            List<TestInfo> optionalTests = new List<TestInfo>();
            List<TestInfo> shouldIfTests = new List<TestInfo>();

            // sort tests
            foreach (TestInfo info in _log.Tests)
            {
                bool bAdd = true;
                
                switch (info.RequirementLevel)
                {
                    case RequirementLevel.Must:
                        {
                            requiredTests.Add(info);
                        }
                        break;
                    case RequirementLevel.ConditionalMust:
                    case RequirementLevel.ConditionalShould:
                        {
                            foreach (Feature feature in info.RequiredFeatures)
                            {
                                if (feature == Feature.PTZAbsoluteOrRelative)
                                {
                                    if (!_log.DeviceEnvironment.Features.Contains(Feature.PTZAbsolute)
                                        && !_log.DeviceEnvironment.Features.Contains(Feature.PTZRelative))
                                    {
                                        bAdd = false;
                                        break;

                                    }
                                }
                                else
                                {
                                    if (!_log.DeviceEnvironment.Features.Contains(feature))
                                    {
                                        bAdd = false;
                                        break;
                                    }
                                }

                            }
                            if (bAdd)
                            {
                                if (info.RequirementLevel == RequirementLevel.ConditionalMust)
                                {
                                    requiredTests.Add(info);
                                }
                                else
                                {
                                    shouldIfTests.Add(info);
                                }
                            }
                        }
                        break;
                    case RequirementLevel.Optional:
                    case RequirementLevel.Should:
                        {
                            optionalTests.Add(info);
                        }
                        break;
                } // switch
            }

            _document.NewPage();
            int testCount, optional_Skipped, mandatory_Skipped;
            int testsRan, testsPassed, testsFailed;

            int optional_Failed;

            testCount = requiredTests.Count + shouldIfTests.Count + optionalTests.Count;

            testsRan = _log.TestResults.Count;
            testsPassed =
                _log.TestResults.Where(KV => KV.Value.Log.TestStatus == TestStatus.Passed || KV.Value.Log.TestStatus == TestStatus.NotSupported).
                    Count();
            testsFailed = _log.TestResults.Where(KV => KV.Value.Log.TestStatus == TestStatus.Failed).
                Count();

            optional_Skipped = 0;
            foreach (TestInfo info in optionalTests)
            {
                if (!_log.TestResults.ContainsKey(info))
                {
                    optional_Skipped++;
                }
            }
            foreach (TestInfo info in shouldIfTests)
            {
                if (!_log.TestResults.ContainsKey(info))
                {
                    optional_Skipped++;
                }
            }

            optional_Failed = _log.TestResults.Where(KV =>
                                                    KV.Value.Log.TestStatus == TestStatus.Failed
                                                    && KV.Key.RequirementLevel == RequirementLevel.Optional).
                Count();
            
            mandatory_Skipped = 0;
            foreach (TestInfo info in requiredTests)
            {
                if (!_log.TestResults.ContainsKey(info))
                {
                    mandatory_Skipped++;
                }
            }
            
            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("ONVIF Test Summary\n");
            p0.Alignment = Element.ALIGN_CENTER;
            _document.Add(p0);

            if (mandatory_Skipped > 0)
            {
                Paragraph p1 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                p1.Add("THIS IS NOT A VALID ONVIF CONFORMANCE TEST\n\n");
                p1.Alignment = Element.ALIGN_CENTER;
                _document.Add(p1);
            }
            
            Paragraph p2 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            p2.Add(String.Format("Test Count: {0}\n", testCount));
            p2.Add(String.Format("Mandatory Tests Skipped: {0}\n", mandatory_Skipped));
            p2.Add(String.Format("Optional Tests Skipped: {0}\n", optional_Skipped));
            p2.Add(String.Format("Tests Executed: {0}\n", testsRan));
            p2.Add(String.Format("Tests Passed:  {0}\n", testsPassed));
            p2.Add(String.Format("Tests Failed:  {0}\n", testsFailed));
            
            if (optional_Failed > 0)
            {
                p2.Add(String.Format("Optional Tests Failed: {0}\n", optional_Failed));
            }

            p2.Add("\n");
            _document.Add(p2);

           
            // Features
            Paragraph p35 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));
            p35.Add("Features selected: ");

            List<string> features = FeaturesHelper.SelectedFeatures(_log);
            foreach (string featureString in features)
            {
                p35.Add(string.Format("{0}\n", featureString));
            }
            _document.Add(p35);

            //
            // Settings:
            //

            Paragraph pTimeouts = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            pTimeouts.Add("Timeouts: \n");
            pTimeouts.Add(string.Format("Message Timeout: {0}\n", _log.DeviceEnvironment.Timeouts.Message));
            pTimeouts.Add(string.Format("Reboot Timeout: {0}\n", _log.DeviceEnvironment.Timeouts.Reboot));
            pTimeouts.Add(string.Format("Time between tests: {0}\n", _log.DeviceEnvironment.Timeouts.InterTests));
            _document.Add(pTimeouts);

            Paragraph pAccount = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));
            pAccount.Add(string.Format("Account: {0}", _log.DeviceEnvironment.Credentials.UserName));
            _document.Add(pAccount);
            
            // if not all mandatory tests were run this is not a valid conformance test
            if (mandatory_Skipped > 0)
            {
                Paragraph p5 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
                p5.Add("NOT ALL TESTS RUN, NOT A VALID ONVIF CONFORMANCE TEST\n");
                p5.Alignment = Element.ALIGN_CENTER;
                _document.Add(p5);
            }

            bool passed = true;
            if (_log.TestResults.Where(KV => KV.Value.Log.TestStatus == TestStatus.Failed ).
                Count() > 0)
            {
                // any test FAILED
                passed = false;
            }
            else
            {
                // only optional, should, shouldIf can be skipped
                if (mandatory_Skipped > 0)
                {
                    passed = false;
                }
            }

            // Summary
            Paragraph p6 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            if (passed)
            {
                p6.Add("TEST PASSED\n");
            }
            else
            {
                p6.Add("TEST FAILED\n");
            }
            p6.Alignment = Element.ALIGN_CENTER;
            _document.Add(p6);

            ListFailedTests(); 
        }


        private void ListFailedTests()
        {
            _document.NewPage();

            Paragraph p1 = new Paragraph(new Chunk("The following tests were FAILED:\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p1.Alignment = Element.ALIGN_CENTER;
            _document.Add(p1);

            foreach (KeyValuePair<TestInfo, Data.TestResult> testInfo in
                _log.TestResults.Where(tr => tr.Value.Log.TestStatus == TestStatus.Failed).OrderBy(T => T.Key.Category).ThenBy(T => T.Key.Order))
            {
                Paragraph p = new Paragraph(new Chunk(testInfo.Key.Name, 
                    FontFactory.GetFont(FontFactory.TIMES, 12)));
                _document.Add(p);
            }


        }

        /// <summary>
        /// Builds index
        /// </summary>
        private void BuildIndex()
        {
            //Add new page
            _document.NewPage();

            Paragraph p1 = new Paragraph(new Chunk("Tests\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p1.Alignment = Element.ALIGN_CENTER;
            _document.Add(p1);

            foreach (TestInfo info in _log.Tests.OrderBy(T => T.Category).ThenBy(T => T.Order))
            {
                string line = string.Format("{0}\n", info.Name);
                Anchor anchor = new Anchor(line, FontFactory.GetFont(FontFactory.TIMES, 12));
                anchor.Reference = string.Format("#{0}", info.Name);
                _document.Add(anchor);
            }
        }

        /// <summary>
        /// Adds test group info.
        /// </summary>
        private void AddTestGroupInfo()
        {
            _document.NewPage();

            Paragraph title = new Paragraph("ONVIF TEST", FontFactory.GetFont(FontFactory.TIMES, 18));
            Chapter chapter = new Chapter(title, 2);
            chapter.NumberDepth = 0;
            Paragraph empty = new Paragraph("\n\n");
            chapter.Add(empty);
            
            string lastGroup = string.Empty;
            Section section = null;
            foreach (TestInfo info in _log.Tests.OrderBy(T => T.Category).ThenBy(TI => TI.Order))
            {
                string group = info.Group.Split('\\')[0];
                if (lastGroup != group)
                {
                    Paragraph groupTitle = new Paragraph(string.Format("\n\n{0}", group), FontFactory.GetFont(FontFactory.TIMES, 18));
                    section = chapter.AddSection(groupTitle);
                    section.NumberDepth = 0;
                    Paragraph someSectionText = new Paragraph("\n");
                    section.Add(someSectionText);
                    lastGroup = group;
                }

                Paragraph testTitle = new Paragraph(info.Name);
                Anchor ancor = new Anchor(".");
                ancor.Name = info.Name;
                testTitle.Add(ancor);
                Section testSection = section.AddSection(testTitle);
                testSection.NumberDepth = 0;

                if (info.RequirementLevel == RequirementLevel.Optional)
                {
                    Paragraph optional = new Paragraph("* Optional Test");
                    testSection.Add(optional);
                }

                string testDescription = string.Empty;
                
                if (_log.TestResults.ContainsKey(info))
                {
                    testDescription = string.Format("\nTestResult\n{0}\n", _log.TestResults[info].ShortTextLog);
                    testDescription = testDescription.Replace(string.Format("{0}\r\n", info.Name), "");
                }
                else
                {
                    testDescription = "Test not run\n\n";
                }

                Paragraph sectionText = new Paragraph(testDescription, FontFactory.GetFont(FontFactory.TIMES, 11));
                testSection.Add(sectionText);

            }

            _document.Add(chapter);

        }

    }
}
