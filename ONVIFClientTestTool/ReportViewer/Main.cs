using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ReportViewer
{
    public partial class fMain : Form
    {
        ClientTestTool.LogFile logFile = new ClientTestTool.LogFile();
        Dictionary<string, ClientTestTool.LogTest> logFile_OldResults = new Dictionary<string, ClientTestTool.LogTest>();
        ClientTestTool.LogFile logFile_Results = new ClientTestTool.LogFile();
        DataGridViewCellStyle styleFailed = new DataGridViewCellStyle();
        DataGridViewCellStyle stylePassed = new DataGridViewCellStyle();


        public fMain()
        {
            InitializeComponent();
            styleFailed.Font = new Font(dgvReport.Font.FontFamily, dgvReport.Font.Size, FontStyle.Bold);
            styleFailed.ForeColor = Color.Red;
            stylePassed.Font = new Font(dgvReport.Font.FontFamily, dgvReport.Font.Size, FontStyle.Bold);
            stylePassed.ForeColor = Color.Green;
        }

        private void bOpenReport_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ofdReport.InitialDirectory))
            { 
                ofdReport.InitialDirectory = @"\\nov-fs01\ONVIF\AutoTesting\TestReports";
            }
            if (ofdReport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ofdReport.InitialDirectory = Path.GetDirectoryName(ofdReport.FileName);
                tbReport.Text = ofdReport.FileName;
                logFile = logFile.DeSerializeData(tbReport.Text);
                tbResults.Text = "";
                logFile_Results = new ClientTestTool.LogFile();
                UpdateLog();
            }
        }

        private void UpdateLog()
        {
            string relatedItems;
            string relatedTraces;
            string relatedFeatures;
            bool testResult;

            dgvReport.Rows.Clear();
            foreach (var test in logFile.test)
            {
                relatedItems = "";
                foreach (var relatedItem in test.relatedItems)
                {
                    relatedItems = relatedItems + relatedItem + "\n";
                }
                relatedItems = relatedItems.Remove(relatedItems.Count() - 1);

                relatedFeatures = "";
                foreach (var featureDeviceFile in test.featureDevice.Values)
                {
                    relatedFeatures = relatedFeatures + featureDeviceFile + "\n";
                }
                if (relatedFeatures.Count() > 0)
                {
                    relatedFeatures = relatedFeatures.Remove(relatedFeatures.Count() - 1);
                }

                relatedTraces = "";
                foreach (var pcapngFile in test.pcapng)
                {
                    relatedTraces = relatedTraces + pcapngFile + "\n";
                }
                if (relatedTraces.Count() > 0)
                {
                    relatedTraces = relatedTraces.Remove(relatedTraces.Count() - 1);
                }


                testResult = true;
                foreach (var check in test.check)
                {
                    if (check.result == "FAILED")
                    {
                        testResult = false;
                        break;
                    }
                }

                foreach (var checkProfile in test.checkProfiles)
                {
                    if (checkProfile.result == "FAILED")
                    {
                        testResult = false;
                        break;
                    }
                }

                foreach (var checkFeature in test.checkFeatures)
                {
                    if (checkFeature.result == "FAILED")
                    {
                        testResult = false;
                        break;
                    }
                }


                string testresultOldStr = "";
                bool testresultOld = true;
                if (logFile_OldResults.ContainsKey(test.id))
                {
                    foreach (var check in logFile_OldResults[test.id].check)
                    {
                        if (check.result == "FAILED")
                        {
                            testresultOld = false;
                            break;
                        }
                    }
                    testresultOldStr = TestResultToString(testresultOld);
                }


                dgvReport.Rows.Add(new string[] { test.id, test.name, test.description.Trim(), relatedItems, testresultOldStr, TestResultToString(testResult), test.relatedBugs, relatedTraces, relatedFeatures });
                if (testResult)
                { dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[5].Style.ApplyStyle(stylePassed); }
                else
                { dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[5].Style.ApplyStyle(styleFailed); }
                if (testresultOldStr == "PASSED")
                { dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[4].Style.ApplyStyle(stylePassed); }
                else
                { dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[4].Style.ApplyStyle(styleFailed); }


            }
        }

        private string TestResultToString(bool result)
        {
            return result ? "PASSED" : "FAILED";
        }

        private void bSaveReport_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.DataGridViewRow raw in dgvReport.Rows)
            {
                if (raw.Cells[6].Value != null)
                {
                    logFile.test.Find(C => C.id == raw.Cells[0].Value.ToString()).relatedBugs = raw.Cells[6].Value.ToString();
                }
            }
            logFile.SerializeData(logFile, tbReport.Text);
        }

        private void dgvReport_SelectionChanged(object sender, EventArgs e)
        {
            dgvChecks.Rows.Clear();
            ClientTestTool.LogTest selectedTest;
            string expectedResult = "";
            string currentResult = "";
            string testName = "";
            string profileName = "";
            string featureName = "";
            if (dgvReport.SelectedCells.Count != 0)
            {
                selectedTest = logFile.test.Find(C => C.id == dgvReport.Rows[dgvReport.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                foreach (var check in selectedTest.check)
                {
                    testName = "";
                    string[] splittedexpecktedResult = check.expectedResult.Split(' ');

                    switch (splittedexpecktedResult[splittedexpecktedResult.Count() - 1])
                    {
                        case "PASSED":
                        case "FAILED":
                            expectedResult = splittedexpecktedResult[splittedexpecktedResult.Count() - 1];
                            for (int i = 1; i < splittedexpecktedResult.Count() - 1; i++)
                            {
                                testName = testName + " " + splittedexpecktedResult[i];
                            }
                            break;
                        case "DETECTED":
                        case "STARTED":
                            expectedResult = splittedexpecktedResult[splittedexpecktedResult.Count() - 2] + " " + splittedexpecktedResult[splittedexpecktedResult.Count() - 1];
                            for (int i = 1; i < splittedexpecktedResult.Count() - 2; i++)
                            {
                                testName = testName + " " + splittedexpecktedResult[i];
                            }
                            break;
                    }

                    string[] splittedcurrentResult = check.currentResult.Split(' ');

                    switch (splittedcurrentResult[splittedcurrentResult.Count() - 1])
                    {
                        case "PASSED":
                        case "FAILED":
                            currentResult = splittedcurrentResult[splittedcurrentResult.Count() - 1];
                            break;
                        case "DETECTED":
                        case "STARTED":
                            currentResult = splittedcurrentResult[splittedcurrentResult.Count() - 2] + " " + splittedcurrentResult[splittedcurrentResult.Count() - 1];
                            break;
                    }



                    dgvChecks.Rows.Add(new string[] { check.deviceMAC, splittedexpecktedResult[0], testName, expectedResult, currentResult, check.result });

                    if (check.result == "PASSED")
                    { dgvChecks.Rows[dgvChecks.Rows.Count - 1].Cells[5].Style.ApplyStyle(stylePassed); }
                    else
                    { dgvChecks.Rows[dgvChecks.Rows.Count - 1].Cells[5].Style.ApplyStyle(styleFailed); }
                }

                foreach (var checkProfile in selectedTest.checkProfiles)
                {
                    profileName = "";
                    string[] splittedexpecktedResult = checkProfile.expectedResult.Split(' ');

                    if (splittedexpecktedResult[splittedexpecktedResult.Count() - 2] == "NOT")
                    {
                        expectedResult = splittedexpecktedResult[splittedexpecktedResult.Count() - 2] + " " + splittedexpecktedResult[splittedexpecktedResult.Count() - 1];
                        for (int i = 1; i < splittedexpecktedResult.Count() - 2; i++)
                        {
                            profileName = profileName + " " + splittedexpecktedResult[i];
                        }
                    }
                    else
                    {
                        expectedResult = splittedexpecktedResult[splittedexpecktedResult.Count() - 1];
                        for (int i = 1; i < splittedexpecktedResult.Count() - 1; i++)
                        {
                            profileName = profileName + " " + splittedexpecktedResult[i];
                        }
                    }


                    string[] splittedcurrentResult = checkProfile.currentResult.Split(' ');

                    if (splittedcurrentResult[splittedcurrentResult.Count() - 2] == "NOT")
                    {
                        currentResult = splittedcurrentResult[splittedcurrentResult.Count() - 2] + " " + splittedcurrentResult[splittedcurrentResult.Count() - 1];
                    }
                    else
                    {
                        currentResult = splittedcurrentResult[splittedcurrentResult.Count() - 1];
                    }

                    dgvChecks.Rows.Add(new string[] { "", splittedexpecktedResult[0], profileName, expectedResult, currentResult, checkProfile.result });

                    if (checkProfile.result == "PASSED")
                    { dgvChecks.Rows[dgvChecks.Rows.Count - 1].Cells[5].Style.ApplyStyle(stylePassed); }
                    else
                    { dgvChecks.Rows[dgvChecks.Rows.Count - 1].Cells[5].Style.ApplyStyle(styleFailed); }
                }

                foreach (var checkFeature in selectedTest.checkFeatures)
                {
                    featureName = "";
                    string[] splittedexpecktedResult = checkFeature.expectedResult.Split(' ');

                    if (splittedexpecktedResult[splittedexpecktedResult.Count() - 2] == "NOT")
                    {
                        expectedResult = splittedexpecktedResult[splittedexpecktedResult.Count() - 2] + " " + splittedexpecktedResult[splittedexpecktedResult.Count() - 1];
                        for (int i = 0; i < splittedexpecktedResult.Count() - 2; i++)
                        {
                            featureName = featureName + " " + splittedexpecktedResult[i];
                        }
                    }
                    else
                    {
                        expectedResult = splittedexpecktedResult[splittedexpecktedResult.Count() - 1];
                        for (int i = 0; i < splittedexpecktedResult.Count() - 1; i++)
                        {
                            featureName = featureName + " " + splittedexpecktedResult[i];
                        }
                    }


                    string[] splittedcurrentResult = checkFeature.currentResult.Split(' ');

                    if (splittedcurrentResult[splittedcurrentResult.Count() - 2] == "NOT")
                    {
                        currentResult = splittedcurrentResult[splittedcurrentResult.Count() - 2] + " " + splittedcurrentResult[splittedcurrentResult.Count() - 1];
                    }
                    else
                    {
                        currentResult = splittedcurrentResult[splittedcurrentResult.Count() - 1];
                    }

                    dgvChecks.Rows.Add(new string[] { "", "Feature", featureName, expectedResult, currentResult, checkFeature.result });

                    if (checkFeature.result == "PASSED")
                    { dgvChecks.Rows[dgvChecks.Rows.Count - 1].Cells[5].Style.ApplyStyle(stylePassed); }
                    else
                    { dgvChecks.Rows[dgvChecks.Rows.Count - 1].Cells[5].Style.ApplyStyle(styleFailed); }
                }
            }
        }

        private void dgvChecks_SelectionChanged(object sender, EventArgs e)
        {
            dgvSteps.Rows.Clear();
            ClientTestTool.LogTest selectedTest;
            ClientTestTool.LogData selectedCheck;
            string stepDetails = "";
            string stepResult = "";
            string stepDescription = "";
            if (dgvReport.SelectedCells.Count != 0)
            {
                selectedTest = logFile.test.Find(C => C.id == dgvReport.Rows[dgvReport.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                if (dgvChecks.SelectedRows.Count != 0)
                {

                    selectedCheck = selectedTest.check.Find(C => (C.deviceMAC == dgvChecks.SelectedRows[0].Cells[0].Value.ToString()) &&
                                                                    (C.expectedResult == dgvChecks.SelectedRows[0].Cells[1].Value.ToString() + dgvChecks.SelectedRows[0].Cells[2].Value.ToString() + " " + dgvChecks.SelectedRows[0].Cells[3].Value.ToString()));
                    if (selectedCheck != null)
                    {
                        foreach (var testStep in selectedCheck.testSteps)
                        {
                            stepDetails = "";
                            stepResult = "";
                            stepDescription = "";
                            string[] splittedTestStep = testStep.results.Split(';');

                            int currentPart = 0;

                            for (int i = splittedTestStep.Count() - 1; i >= 0; i--)
                            {
                                if (splittedTestStep[i].StartsWith("Step Details:"))
                                {
                                    stepDetails = splittedTestStep[i].Substring("Step Details:".Count());
                                    currentPart = 3;
                                }
                                else
                                {
                                    if (splittedTestStep[i].StartsWith(" Result:"))
                                    {
                                        stepResult = splittedTestStep[i].Substring(" Result:".Count());
                                        if ((currentPart == 0) && (stepDescription != ""))
                                        {
                                            stepResult = stepResult + ";" + stepDescription;
                                        }
                                        currentPart = 2;
                                    }
                                    else
                                    {
                                        if (splittedTestStep[i].StartsWith(" Description:"))
                                        {
                                            stepDescription = splittedTestStep[i].Substring(" Description:".Count());
                                            currentPart = 1;
                                        }
                                        else
                                        {
                                            switch (currentPart)
                                            {
                                                case 0:
                                                    stepDescription = splittedTestStep[i] + ";" + stepDescription;
                                                    break;
                                                case 1:
                                                    stepResult = splittedTestStep[i] + ";" + stepResult;
                                                    break;
                                                case 2:
                                                case 3:
                                                    stepDetails = splittedTestStep[i] + ";" + stepDetails;
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }

                            dgvSteps.Rows.Add(new string[] { stepDetails, stepResult, stepDescription });
                        }
                    }
                }
            }
        }

        private void fMain_ResizeEnd(object sender, EventArgs e)
        {
            dgvSteps.AutoResizeColumns();
            dgvSteps.AutoResizeRows();

            dgvChecks.AutoResizeColumns();
            dgvChecks.AutoResizeRows();

            dgvReport.AutoResizeColumns();
            dgvReport.AutoResizeRows();
        }

        private void bLoadResults_Click(object sender, EventArgs e)
        {
            if (ofdReport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbResults.Text = ofdReport.FileName;
                logFile_Results = logFile_Results.DeSerializeData(tbResults.Text);
                LoadResults();
            }
        }

        private void LoadResults()
        {
            logFile_OldResults = new Dictionary<string, ClientTestTool.LogTest>();
            foreach (var test in logFile.test)
            {
                if (logFile_Results.test.Any(C => C.id == test.id))
                {
                    var testWithResult = logFile_Results.test.Find(C => C.id == test.id);
                    test.relatedBugs = testWithResult.relatedBugs;
                }
            }

            foreach (var test in logFile_Results.test)
            {
                logFile_OldResults.Add(test.id, test);
            }

            UpdateLog();
        }

        private void tbReport_TextChanged(object sender, EventArgs e)
        {

        }

        private void lReport_Click(object sender, EventArgs e)
        {

        }
    }
}
