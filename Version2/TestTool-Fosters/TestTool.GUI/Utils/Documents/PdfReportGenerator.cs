///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using System.IO;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.TestEngine;
using System.Reflection;
using TestTool.Tests.Definitions.Features;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Class with methods for creating PDF report.
    /// </summary>
    class PdfReportGenerator : PdfDocGenerator
    {

        public PdfReportGenerator()
        {
        }
        
        protected override void GenerateDocumentContent()
        {
            // Create Footer 
            Document.Footer = HeaderAndFooter();

            // Create cover page
            BuildCoverPage();

            // Create summary
            BuildSummary();

            // Create index
            BuildIndex();

            // Create test info
            AddTestGroupInfo();
        }

        /// <summary>
        /// Creates header and footer for report.
        /// </summary>
        /// <returns></returns>
        private HeaderFooter HeaderAndFooter()
        {
            HeaderFooter footer = new HeaderFooter(
                new Phrase(string.Format("Device - {0} {1} @ {2}  ONVIF Test Report Page: ",
                Log.DeviceInfo != null ? Log.DeviceInfo.Model : "<inknown>",
                Log.TestExecutionTime.ToShortDateString(),
                Log.TestExecutionTime.ToLongTimeString())), true);
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
                Document.Add(logo);
            }

            //
            Document.Add(new Paragraph(Environment.NewLine));
            Document.Add(new Paragraph(Environment.NewLine));
            Document.Add(new Paragraph(Environment.NewLine));

            //
            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            p0.Add(new Chunk("ONVIF Conformance Test", FontFactory.GetFont(FontFactory.TIMES_BOLD, 20)));
            p0.Add("\n\n");

            p0.Add(new Chunk("Performed by", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");

            p0.Add(String.Format("Operator - {0}\n", Log.TesterInfo.Operator));
            p0.Add(String.Format("Organization - {0}\n", Log.TesterInfo.Organization));
            p0.Add(String.Format("Address - {0}\n", Log.TesterInfo.Address));

            //add empty lines
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add(new Chunk("Device Under Test", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("\n\n");
            p0.Add(String.Format("Product Name - {0}\n", Log.DeviceInfo.ProductName));
            p0.Add(String.Format("Brand - {0}\n", Log.DeviceInformation.Manufacturer));
            p0.Add(String.Format("Model - {0}\n", Log.DeviceInformation.Model));
            p0.Add(String.Format("Serial Number - {0}\n", Log.DeviceInformation.SerialNumber));
            p0.Add(String.Format("Firmware Version - {0}\n", Log.DeviceInformation.FirmwareVersion));
            p0.Add(String.Format("Other - {0}\n", Log.OtherInformation));

            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            // add the ONVIF information

            p0.Add(String.Format("{0}", Log.Application.ToolVersionFull));
            p0.Add(Environment.NewLine);
            p0.Add(String.Format("{0}", Log.Application.TestSpecification));
            p0.Alignment = Element.ALIGN_CENTER;
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);
            p0.Add(Environment.NewLine);

            p0.Add(string.Format("Test Date and Time - {0} @ {1}", Log.TestExecutionTime.ToShortDateString(), Log.TestExecutionTime.ToLongTimeString()));

            Document.Add(p0);
        }

        /// <summary>
        /// Builds report summary.
        /// </summary>
        private void BuildSummary()
        {
            Document.NewPage();
            int testCount, testsPassed, testsFailed;

            testCount = Log.TestResults.Count;
            
            testsPassed =
                Log.TestResults.Where(KV => KV.Value.Log.TestStatus == TestStatus.Passed).Count();
            
            testsFailed = Log.TestResults.Where(KV => KV.Value.Log.TestStatus == TestStatus.Failed).Count();
            
            Paragraph p0 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p0.Add("ONVIF Test Summary\n");
            p0.Alignment = Element.ALIGN_CENTER;
            Document.Add(p0);

            
            Paragraph p2 = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));
            p2.Add(String.Format("Tests Executed: {0}\n", testCount));
            p2.Add(String.Format("Tests Passed:  {0}\n", testsPassed));
            p2.Add(String.Format("Tests Failed:  {0}\n", testsFailed));
            p2.Add("\n");
            Document.Add(p2);

           
            // Features
            Document.Add(new Chunk(Environment.NewLine));
            PrintFetures(FontFactory.GetFont(FontFactory.TIMES, 12)
                        , FontFactory.GetFont(FontFactory.TIMES, 10));

            //
            // Settings:
            //

            Paragraph pTimeouts = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));

            pTimeouts.Add("Timeouts: \n");
            pTimeouts.Add(string.Format("Message Timeout: {0}\n", Log.DeviceEnvironment.Timeouts.Message));
            pTimeouts.Add(string.Format("Reboot Timeout: {0}\n", Log.DeviceEnvironment.Timeouts.Reboot));
            pTimeouts.Add(string.Format("Time between Tests: {0}\n", Log.DeviceEnvironment.Timeouts.InterTests));
            pTimeouts.Add(string.Format("Time between Requests: {0}\n", Log.DeviceEnvironment.TestSettings.RecoveryDelay));
            pTimeouts.Add(string.Format("Operation Delay: {0}\n", Log.DeviceEnvironment.TestSettings.OperationDelay));
            Document.Add(pTimeouts);
            //Log.DeviceEnvironment.TestSettings.OperationDelay
            Paragraph pAccount = new Paragraph(new Chunk("\n", FontFactory.GetFont(FontFactory.TIMES, 12)));
            pAccount.Add(string.Format("Account: {0}", Log.DeviceEnvironment.Credentials.UserName));
            Document.Add(pAccount);
            
            bool passed = (testsFailed == 0);
 
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
            Document.Add(p6);

            AddFeaturesDefinitionLog();

            ListFailedTests(); 
        }

        private void PrintFetures(Font titleFont, Font itemsFont)
        {
            Document.Add(new Paragraph(new Paragraph("Features:", titleFont)));

            FeaturesSet featuresSet = FeaturesSet.CreateFeaturesSet();
            PrintFeaturesTree(itemsFont, featuresSet.Nodes, 10, 10);
        }
                
        private void PrintFeaturesTree(Font font, List<FeatureNode> data, float indentationLeft, float step)
        {
            if (data == null)
            {
                return;
            }
            foreach (var feature in data)
            {
                if (!Log.Features.Contains(feature.Feature))
                {
                    continue;
                }
                var featureName = new Paragraph(feature.Name, font);
                featureName.IndentationLeft = indentationLeft;
                Document.Add(featureName);
                if (feature.Status == FeatureStatus.Group)
                {
                    PrintFeaturesTree(font, feature.Nodes, indentationLeft + step, step);
                }
            }
        }

        private void ListFailedTests()
        {
            Document.NewPage();

            Paragraph p1 = new Paragraph(new Chunk("The following tests were FAILED:\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p1.Alignment = Element.ALIGN_CENTER;
            Document.Add(p1);

            foreach (KeyValuePair<TestInfo, Data.TestResult> testInfo in
                Log.TestResults.Where(tr => tr.Value.Log.TestStatus == TestStatus.Failed).OrderBy(T => T.Key.Category).ThenBy(T => T.Key.Order))
            {
                Paragraph p = new Paragraph(new Chunk(testInfo.Key.Name, 
                    FontFactory.GetFont(FontFactory.TIMES, 12)));
                Document.Add(p);
            }
        }

        /// <summary>
        /// Builds index
        /// </summary>
        private void BuildIndex()
        {
            //Add new page
            Document.NewPage();

            Paragraph p1 = new Paragraph(new Chunk("Tests\n", FontFactory.GetFont(FontFactory.TIMES, 16)));
            p1.Alignment = Element.ALIGN_CENTER;
            Document.Add(p1);

            foreach (TestInfo info in Log.TestResults.Keys.OrderBy(T => T.Category).ThenBy(T => T.Order))
            {
                string line = string.Format("{0}\n", info.Name);
                Anchor anchor = new Anchor(line, FontFactory.GetFont(FontFactory.TIMES, 12));
                anchor.Reference = string.Format("#{0}", info.Name);
                Document.Add(anchor);
            }
        }

        private void AddFeaturesDefinitionLog()
        {
            Document.NewPage();
            Paragraph title = new Paragraph("Features Definition Log", FontFactory.GetFont(FontFactory.TIMES, 16));
            title.Alignment = Element.ALIGN_CENTER;
            title.Add(Environment.NewLine);
            Document.Add(title);
            Paragraph log = new Paragraph(Log.FeaturesDefinitionLog.ShortTextLog, FontFactory.GetFont(FontFactory.TIMES, 12));
            Document.Add(log);
        }

        /// <summary>
        /// Adds test group info.
        /// </summary>
        private void AddTestGroupInfo()
        {
            Document.NewPage();

            Paragraph title = new Paragraph("ONVIF TEST", FontFactory.GetFont(FontFactory.TIMES, 18));
            Chapter chapter = new Chapter(title, 2);
            chapter.NumberDepth = 0;
            Paragraph empty = new Paragraph("\n\n");
            chapter.Add(empty);
            
            string lastGroup = string.Empty;
            Section section = null;
            foreach (TestInfo info in Log.TestResults.Keys.OrderBy(T => T.Category).ThenBy(TI => TI.Order))
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

                if (Log.TestResults.ContainsKey(info))
                {
                    testDescription = string.Format("\nTestResult\n{0}\n", Log.TestResults[info].ShortTextLog);
                    testDescription = testDescription.Replace(string.Format("{0}\r\n", info.Name), "");
                }
                else
                {
                    testDescription = "Test not run\n\n";
                }

                Paragraph sectionText = new Paragraph(testDescription, FontFactory.GetFont(FontFactory.TIMES, 11));
                testSection.Add(sectionText);
            }

            Document.Add(chapter);
            
        }
    }
}
