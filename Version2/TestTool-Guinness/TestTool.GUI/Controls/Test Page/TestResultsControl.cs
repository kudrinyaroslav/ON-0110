using System;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Data;
using TestTool.GUI.Views;
using TestTool.Tests.Definitions.Trace;

namespace TestTool.GUI.Controls
{
    public partial class TestResultsControl : UserControl, ITestResultView
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
            tbTestResult.AppendText(string.Format("{0}{1}", logEntry, Environment.NewLine));
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

        public void EnableControl(bool bEnable)
        {
        }



        #region IView Members

        public void SwitchToState(TestTool.GUI.Enums.ApplicationState state)
        {

        }

        public TestTool.GUI.Controllers.IController GetController()
        {
            return null;
        }

        #endregion
    }
}
