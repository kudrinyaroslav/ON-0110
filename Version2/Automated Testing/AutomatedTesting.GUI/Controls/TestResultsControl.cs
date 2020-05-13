using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutomatedTesting.GUI.Data;
using TestTool.Tests.Definitions.Trace;

namespace AutomatedTesting.GUI.Controls
{
    public partial class TestResultsControl : UserControl
    {
        public TestResultsControl()
        {
            InitializeComponent();
        }
        

        public void BeginTest()
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        ClearTestInfo();
                    }));
        }

        public void DisplayTestResults(TestResult log)
        {
            if (log.Log != null)
            {
                foreach (StepResult result in log.Log.Steps.OrderBy(s => s.Number))
                {
                    AddStepResult(result);
                }
            }
            tbTestToolOutput.Text = log.PlainTextLog;
            tbSimulatorOutput.Text = log.DutLog;
            tbTestDescription.Text = log.TestDescription;
            tbTestExpectedResult.Text = log.TestExpectedResult;
            tbTestName.Text = log.TestName;
            tbResults.Text = log.TestFinalResult;
        }

        public void ClearTestInfo()
        {
            lvStepDetails.Items.Clear();
            tbResponse.Clear();
            tbRequest.Clear();
            tbTestToolOutput.Clear();
            tbSimulatorOutput.Clear();
            tbTestDescription.Clear();
            tbTestExpectedResult.Clear();
            tbTestName.Clear();
            tbResults.Clear();
        }

        /// <summary>
        /// Adds line to test log.
        /// </summary>
        /// <param name="logEntry"></param>
        public void WriteLine(string logEntry)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        InternalWriteLine(logEntry);
                    }));


        }

        /// <summary>
        /// Adds line to log.
        /// </summary>
        /// <param name="logEntry">Log entry</param>
        void InternalWriteLine(string logEntry)
        {
            tbTestToolOutput.AppendText(string.Format("{0}{1}", logEntry, Environment.NewLine));
        }

        /// <summary>
        /// Displays step result.
        /// </summary>
        /// <param name="result"></param>
        public void DisplayStepResult(StepResult result)
        {
            BeginInvoke(new Action(() => AddStepResult(result)));
        }

        /// <summary>
        /// Adds list item representing step results.
        /// </summary>
        /// <param name="result">Step information.</param>
        public void AddStepResult(StepResult result)
        {
            ListViewItem stepItem = new ListViewItem(result.Number.ToString());
            stepItem.Tag = result;
            stepItem.SubItems.Add(result.Status.ToString());
            stepItem.SubItems.Add(result.Message);
            stepItem.ImageKey = string.IsNullOrEmpty(result.Request) && string.IsNullOrEmpty(result.Response) ?
                string.Empty : "NetworkOperation";
            lvStepDetails.Items.Add(stepItem);
        }

        /// <summary>
        /// Displays step details when a step is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvStepDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvStepDetails.SelectedItems.Count > 0)
            {
                ListViewItem item = lvStepDetails.SelectedItems[0];
                StepResult result = (StepResult)item.Tag;
                tbRequest.Text = result.Request;
                tbResponse.Text = result.Response;
            }
            else
            {
                tbRequest.Text = string.Empty;
                tbResponse.Text = string.Empty;
            }
        }
    }
}
