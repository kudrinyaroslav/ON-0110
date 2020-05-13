///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Enums;
using TestTool.Tests.Definitions.Data;

namespace TestTool.GUI.Controls
{
    /// <summary>
    /// Setup tab control
    /// </summary>
    partial class ConformanceTestPage : UserControl, IConformanceTestView
    {
        private ConformanceTestController _controller;
        
        /// <summary>
        /// C-tor
        /// </summary>
        public ConformanceTestPage()
        {
            InitializeComponent();

            pbTestsExecution.Visible = false;

            _controller = new ConformanceTestController(this);
            _controller.DeviceInformationReceived += OnDeviceInformationReceived;
        }

        #region ISetupView Members

        /// <summary>
        /// Gets or sets brand
        /// </summary>
        public string Brand 
        { 
            get { return tbBrand.Text; }
            set { tbBrand.Text = value; } 
        }
        /// <summary>
        /// Gets or sets model
        /// </summary>
        public string Model 
        { 
            get { return tbModel.Text; }
            set { tbModel.Text = value; } 
        }

        public string OnvifProductName
        {
            get { return tbProductName.Text; }
            set { tbProductName.Text = value;}
        }

        /// <summary>
        /// Gets or sets other information text
        /// </summary>
        public string OtherInformation 
        { 
            get { return tbOtherInformation.Text; }
            set { tbOtherInformation.Text = value; } 
        }

        /// <summary>
        /// Gets or sets operator name
        /// </summary>
        public string OperatorName 
        { 
            get { return tbOperatorName.Text; }
            set { tbOperatorName.Text = value; } 
        }

        /// <summary>
        /// Gets or sets organization name
        /// </summary>
        public string OrganizationName 
        { 
            get { return tbOrganizationName.Text; }
            set { tbOrganizationName.Text = value; } 
        }

        /// <summary>
        /// Gets or sets organization address
        /// </summary>
        public string OrganizationAddress 
        { 
            get { return tbOrganizationAddress.Text; }
            set { tbOrganizationAddress.Text = value; } 
        }

        /// <summary>
        /// Gets or sets responsible member name
        /// </summary>
        public string MemberName
        {
            get { return tbMemberName.Text; }
            set { tbMemberName.Text = value; }
        }

        /// <summary>
        /// Gets or sets responsible member address
        /// </summary>
        public string MemberAddress
        {
            get { return tbMemberAddress.Text; }
            set { tbMemberAddress.Text = value; }
        }
        
        #endregion
        
        #region IView Members

        public IController GetController()
        {
            return _controller;
        }

        #endregion
        
        /// <summary>
        /// Returns controller
        /// </summary>
        internal ConformanceTestController Controller
        {
            get { return _controller; }
        }

        private bool _isReportGeneratorsEnabled = false;
        private bool IsReportGeneratorsEnabled
        {
            get { return _isReportGeneratorsEnabled; }
            set 
            { 
                _isReportGeneratorsEnabled = value;
                btnDoCReport.Enabled = value;
                btnReport.Enabled = value;
                btnDatasheetReport.Enabled = value;
            }
        }

        /// <summary>
        /// Switches control to specified state
        /// </summary>
        /// <param name="state">New state</param>
        public void SwitchToState(ApplicationState state)
        {
            switch (state)
            {
                case ApplicationState.Idle:
                    Invoke(new Action(() => { EnableControls(true); }));
                    break;
                case ApplicationState.CommandRunning:
                case ApplicationState.DiscoveryRunning:
                case ApplicationState.TestPaused:
                case ApplicationState.TestRunning:
                    Invoke(new Action(() => { EnableControls(false); }));
                    break;
                case ApplicationState.ConformanceTestRunning:
                    Invoke(new Action(() => { BeginTests(); }));
                    break;
            }
        }
        
        /// <summary>
        /// Enable or disables "Get from device" button
        /// </summary>
        /// <param name="enable"></param>
        public void EnableControls(bool enable)
        {
            IsReportGeneratorsEnabled = _reportEnabled && enable;
            btnRun.Enabled = enable;
        }

        #region DeviceInfo
        
        /// <summary>
        /// Handles device information received event
        /// </summary>
        /// <param name="manufacturer">Device manufacturer</param>
        /// <param name="model">Device model</param>
        /// <param name="firmwareVersion">Device firmware version</param>
        /// <param name="serial">Device serial number</param>
        /// <param name="hardwareId">Device hardware id</param>
        private void OnDeviceInformationReceived(string manufacturer, string model, string firmwareVersion, string serial, string hardwareId)
        {
            BeginInvoke(new Action(() =>
                                       {
                                           tbModel.Text = model;
                                           tbBrand.Text = manufacturer;
                                       }));

        }
        
        /// <summary>
        /// Handles button clear device information click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnClearDeviceInformation_Click(object sender, EventArgs e)
        {
            tbProductName.Clear();
            tbBrand.Clear();
            tbModel.Clear();
            tbOtherInformation.Clear();
        }

        /// <summary>
        /// Handles button clear click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbOperatorName.Clear();
            tbOrganizationName.Clear();
            tbOrganizationAddress.Clear();
        }

        private void btnClearMemberInfo_Click(object sender, EventArgs e)
        {
            tbMemberName.Clear();
            tbMemberAddress.Clear();
        }
        
        #endregion
        
        #region TEST
        
        private bool _testRunning;
        
        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!_testRunning)
            {
                btnRun.Text = "Stop";
                btnRun.Image = Properties.Resources.Stop;
                pbTestsExecution.Value = 0;
                lblProgress.Text = string.Empty;
                pbTestsExecution.Style = ProgressBarStyle.Marquee;
                _controller.RunAll();
            }
            else
            {
                lblTestExecutionMessage.Text = "Stop tests execution...";
                btnRun.Enabled = false;
                _controller.Halt();
            }
            _testRunning = !_testRunning;
        }

        /// <summary>
        /// Disables buttons
        /// </summary>
        public void BeginTests()
        {
            pbTestsExecution.Visible = true;
            IsReportGeneratorsEnabled = false;
        }
 
        public void BeginTest(TestInfo testInfo)
        {
            if (string.IsNullOrEmpty(testInfo.Id))
            {
                BeginInvoke(new Action(() =>
                                           {
                                               lblTestExecutionMessage.Text = testInfo.Name + " - process started";
                                           }));
            }
            else
            {
                BeginInvoke(new Action(() =>
                                           {
                                               lblTestExecutionMessage.Text = testInfo.Name + " - test started";
                                           }));
            }
        }

        public void EndTest(string testId, string testName, TestTool.Tests.Definitions.Trace.TestStatus status)
        {
            BeginInvoke(new Action(() => { lblTestExecutionMessage.Text = testName + " - test completed"; }));
        }

        public void ReportProgress(int done, int total, int failed)
        {
            BeginInvoke(new Action(() =>
                                       {
                                           lblProgress.Text = string.Format("{0} of {1} done, {2} failed", done, total, failed);
                                           pbTestsExecution.Value = done;
                                       }));
        }


        // is called the same way as "EndTest"
        public void EndFeatureDefinition(TestTool.Tests.Definitions.Trace.TestStatus status)
        {
            BeginInvoke(new Action(() =>
                                       {
                                           pbTestsExecution.Style = ProgressBarStyle.Blocks;
                                           lblTestExecutionMessage.Text = "FEATURE DEFINITION - process completed";
                                       }));
        }

        // Test suite completed before tests were defined
        public void ReportFeatureDefinitionCompleted(bool finished)
        {
            BeginInvoke(new Action(() =>
            {
                pbTestsExecution.Visible = false;
                pbTestsExecution.Style = ProgressBarStyle.Blocks;
                if (finished)
                {
                    lblTestExecutionMessage.Text = "Feature definition process FAILED. No tests to run were defined";
                }
                else
                {
                    lblTestExecutionMessage.Text = "Feature definition process stopped.";
                }
                SwitchToIdle();
            }));
        }

        public void ReportTestSuiteCompleted(bool preliminaryPassed, int passed, int failed, bool completedNormally)
        {
            BeginInvoke(new Action(() =>
               {
                   pbTestsExecution.Visible = false;
                   if (completedNormally)
                   {
                       string status = "Conformance Testing " + (failed == 0 && preliminaryPassed ? "PASSED" : "FAILED");
                       lblTestExecutionMessage.Text = status +
                                                      ". Details are available at the Diagnostic tab.";
                       
                       Log(string.Format("{0} ({1} passed, {2} failed)", status, passed, failed));
                   }
                   else
                   {
                       lblTestExecutionMessage.Text = "Test execution stopped";
                   }
                   SwitchToIdle();
                   if (completedNormally)
                   {
                       MessageBox.Show(this.FindForm(), "Tests completed", "Done", MessageBoxButtons.OK);
                   }
               }));
        }

        public void ClearInfo()
        {
            BeginInvoke(new Action(() =>
                                       {
                                           pbTestsExecution.Value = 0;
                                           lblProgress.Text = string.Empty;
                                           lblTestExecutionMessage.Text = string.Empty;
                                       }));
        }

        void SwitchToIdle()
        {
            IsReportGeneratorsEnabled = _reportEnabled;
            btnRun.Enabled = true;
            _testRunning = false;
            btnRun.Text = "Start Conformance Test";
            btnRun.Image = TestTool.GUI.Properties.Resources.RunAll;
        }

        public void DefineTestsCount(int total)
        {
            BeginInvoke(new Action(() =>
                                       {
                                           pbTestsExecution.Style = ProgressBarStyle.Blocks;
                                           pbTestsExecution.Minimum = 0;
                                           pbTestsExecution.Maximum = total;
                                       }));
        }

        #endregion

        #region REPORT

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                string fileName;
                if (TryToGetPdfFileName("pdf", "PDF Document |*.pdf", out fileName))
                {
                    Controller.CreateReport(fileName);
                }
            }
        }

        private void btnDoCReport_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                string fileName;
                if (TryToGetPdfFileName("pdf", "PDF Document |*.pdf", out fileName))
                {
                    Controller.CreateDoCReport(fileName);
                }
            }
        }

        private void btnDatasheetReport_Click(object sender, EventArgs e)
        {
            string fileName;
            if (TryToGetPdfFileName("xml", "Xml document |*.xml", out fileName))
            {
                Controller.CreateDatasheetReport(fileName);
            }
        }

        private bool ValidateFields()
        { 
            var messages = new List<string>();
            TextBox first = null;
            Action<TextBox, Label> validateAction = new Action<TextBox, Label>(
                (tb, lbl) =>
                    {
                        if (string.IsNullOrEmpty(tb.Text))
                        {
                            messages.Add(string.Format("Field \"{0}\" must be filled"
                                                      , lbl.Text.TrimEnd(':')));
                            if (first == null)
                            {
                                first = tb;
                            }
                        }
                    });

            validateAction(tbMemberName, lblMemberName);
            validateAction(tbMemberAddress, lblMemberAddress);
            validateAction(tbProductName, lblproductName);
            //validateAction(tbOrganizationName, lblOrganizationName);
            //validateAction(tbOrganizationAddress, lblOrganizationAddress);
            
            if (messages.Count > 0)
            {
                MessageBox.Show(this, string.Join(Environment.NewLine, messages.ToArray()),
                                "Required information is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                first.Focus();
            }

            return messages.Count == 0;
        }

        private bool TryToGetPdfFileName(string ext, string filter, out string fileName)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ext;
            dlg.CheckPathExists = true;
            dlg.OverwritePrompt = false;
            dlg.Filter = filter;

            DialogResult dr = dlg.ShowDialog();
            fileName = dlg.FileName;
            return dr == DialogResult.OK;
        }

        private bool _reportEnabled;

        public void EnableSaveReport(bool bEnable)
        {
            _reportEnabled = bEnable;
            BeginInvoke(new Action(() =>
            {
                IsReportGeneratorsEnabled = bEnable;
            }));
        }

        public void ReportException(Exception exception)
        {
            MessageBox.Show(exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ReportDocumentCreationCompleted()
        {
            MessageBox.Show("Report saved", "Operation completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        public void Log(string entry)
        {
            BeginInvoke(
                new Action( 
                    () =>
                        {
                            tbLog.AppendText(entry);
                        }));
        }

        public void ClearLog()
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        tbLog.Clear();
                    }));

        }

    }
}
