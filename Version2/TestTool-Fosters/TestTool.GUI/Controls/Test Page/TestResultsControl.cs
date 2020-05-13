using System;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Data;
using TestTool.Tests.Definitions.Trace;

namespace TestTool.GUI.Controls
{
    public partial class TestResultsControl : UserControl
    {
        public TestResultsControl()
        {
            InitializeComponent();
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
            tbTestResult.Text = log.PlainTextLog;
        }

        public void DisplayProfileTestLog(string dump)
        {
            ClearTestInfo();
            tbTestResult.Text = dump;
        }

        public void ClearTestInfo()
        {
            lvStepDetails.Items.Clear();
            tbResponse.Clear();
            tbRequest.Clear();
            tbTestResult.Clear();

        }

        public void WriteLine(string logEntry)
        {
            tbTestResult.AppendText(string.Format("{0}{1}", logEntry, Environment.NewLine));
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

        public void EnableControl(bool bEnable)
        {
        }


    }
}
