///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.GUI.Data;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Enums;
using TestTool.Tests.Definitions.Data;

namespace TestTool.GUI.Controls
{
    /// <summary>
    /// Setup tab control
    /// </summary>
    partial class ConformanceTestPage : Page, IConformanceTestView
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
        /// Gets or sets all available product types
        /// </summary>
        private string _productTypesAll = string.Empty;
        public string ProductTypesAll
        {
            get 
            {
                _productTypesAll = string.Empty;

                if (clbProductType.Items != null && clbProductType.Items.Count != 0)
                {
                    foreach (var item in clbProductType.Items)
                    {
                        _productTypesAll += item.ToString();

                        if (clbProductType.Items.IndexOf(item) != clbProductType.Items.Count - 1)
                            _productTypesAll += ", ";
                    }
                }

                return _productTypesAll;
            }

            set
            {
                _productTypesAll = value != null ? value : string.Empty;

                if (!string.IsNullOrEmpty(_productTypesAll))
                {
                    string[] delimiters = { ", " };
                    string[] types = _productTypesAll.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (types != null && types.Length != 0)
                    {
                        foreach (var type in types)
                        {
                            if (!clbProductType.Items.Contains(type))
                            {
                                clbProductType.Items.Add(type);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets product types
        /// </summary>
        private string _productTypes = string.Empty;
        public string ProductTypes
        {
            get
            {
                _productTypes = string.Empty;
                if (clbProductType.CheckedIndices != null || clbProductType.CheckedIndices.Count != 0)
                {

                    foreach (int index in clbProductType.CheckedIndices)
                    {
                        string type = clbProductType.Items[index].ToString();
                        _productTypes += type;

                        if (clbProductType.CheckedIndices.IndexOf(index) != clbProductType.CheckedIndices.Count - 1)
                            _productTypes += ", ";
                    }

                    return _productTypes;
                }
                else
                    return string.Empty;
            }
            set
            {
                _productTypes = value != null ? value : string.Empty;

                if (!string.IsNullOrEmpty(_productTypes))
                {
                    string[] delimiters = { ", " };
                    string[] types = _productTypes.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (types != null && types.Length != 0)
                    {
                        foreach (var type in types)
                        {
                            if (clbProductType.Items.Contains(type))
                            {
                                int index = clbProductType.Items.IndexOf(type);
                                clbProductType.SetItemChecked(index, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets product types which is absent in default ProductTypes list
        /// </summary>
        private string _productTypesOther = string.Empty;
        public string ProductTypesOther
        {
            get
            {
                _productTypesOther = string.Empty;
                if (!string.IsNullOrEmpty(tbProductTypeOther.Text))
                    _productTypesOther = tbProductTypeOther.Text;

                return _productTypesOther;
            }

            set
            {
                _productTypesOther = value != null ? value : string.Empty;
                tbProductTypeOther.Text = _productTypesOther;
            }
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

        /// <summary>
        /// Gets or sets General international support mailing address
        /// </summary>
        public string InternationalAddress
        {
            get { return tbInternationalAddress.Text; }
            set { tbInternationalAddress.Text = value; }
        }

        /// <summary>
        /// Gets or sets Regional support contact address
        /// </summary>
        public string RegionalAddress
        {
            get { return tbRegionalAddress.Text; }
            set { tbRegionalAddress.Text = value; }
        }

        /// <summary>
        /// Gets or sets Technical support website URL
        /// </summary>
        public string SupportUrl
        {
            get { return tbSupportWebsite.Text; }
            set { tbSupportWebsite.Text = value; }
        }

        /// <summary>
        /// Gets or sets Technical support email
        /// </summary>
        public string SupportEmail
        {
            get { return tbSupportEmail.Text; }
            set { tbSupportEmail.Text = value; }
        }

        /// <summary>
        /// Gets or sets Technical support phone
        /// </summary>
        public string SupportPhone
        {
            get { return tbSupportPhone.Text; }
            set { tbSupportPhone.Text = value; }
        }

        #endregion
        
        #region IView Members

        public override IController GetController()
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
                btnReport.Enabled = value;
                btnDatasheetReport.Enabled = value;
            }
        }

        /// <summary>
        /// Switches control to specified state
        /// </summary>
        /// <param name="state">New state</param>
        public override void SwitchToState(ApplicationState state)
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
            btnDoCReport.Enabled = _docEnabled && enable;
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

            foreach (int index in clbProductType.CheckedIndices)
            {
                clbProductType.SetItemCheckState(index, CheckState.Unchecked);
            }
            clbProductType.ClearSelected();
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
                if (!ValidateTestSettings())
                {
                    _controller.RaiseSettingsMissing(new List<string>(){ManagementController.EVENTSSETTINGS});
                    return;
                }
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

        bool ValidateTestSettings()
        {
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();

            bool topicEmpty = string.IsNullOrEmpty(environment.TestSettings.EventTopic) ||
                              string.IsNullOrEmpty(environment.TestSettings.TopicNamespaces); 

            if (topicEmpty)
            {
                ShowPrompt("Topic (with namespace) for Events tests not defined at the Management page", "Necessary settings missing");
            }
            
            return !topicEmpty;
        }

        /// <summary>
        /// Disables buttons
        /// </summary>
        public void BeginTests()
        {
            pbTestsExecution.Visible = true;
            IsReportGeneratorsEnabled = false;
            btnDoCReport.Enabled = false;
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
                       
                       //ContextController.

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
            btnDoCReport.Enabled = _docEnabled;
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

            Action<CheckedListBox, Label> validateAction2 = new Action<CheckedListBox, Label>(
                (clb, lbl1) =>
                    {
                        if (clbProductType.CheckedIndices == null || (clbProductType.CheckedIndices != null && clbProductType.CheckedIndices.Count == 0))
                        {
                            messages.Add(string.Format("Field \"{0}\" must be filled", lbl1.Text.TrimEnd(':')));
                        }
                    });

            validateAction(tbMemberName, lblMemberName);
            validateAction(tbMemberAddress, lblMemberAddress);
            validateAction(tbProductName, lblproductName);
            validateAction2(clbProductType, lblProductType);
            validateAction(tbInternationalAddress, lblInternationalAddress);
            validateAction(tbSupportWebsite, lblSupportWebsite);
            //validateAction(tbSupportPhone, lblSupportPhone);
            //validateAction(tbOrganizationName, lblOrganizationName);
            //validateAction(tbOrganizationAddress, lblOrganizationAddress);
            
            if (messages.Count > 0)
            {
                ShowPrompt(string.Join(Environment.NewLine, messages.ToArray()),
                                "Required information is missing");
                
                if (first is TextBox)
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

        private bool _docEnabled;
        public void EnableGenerateDoc(bool bEnable)
        {
            _docEnabled = bEnable;

            BeginInvoke(new Action(() =>
            {
                btnDoCReport.Enabled = bEnable;
            }));
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

        private void btnClearSupportInformation_Click(object sender, EventArgs e)
        {
            tbInternationalAddress.Clear();
            tbRegionalAddress.Clear();
            tbSupportWebsite.Clear();
            tbSupportEmail.Clear();
            tbSupportPhone.Clear();
        }

        private void btnAddProductType_Click(object sender, EventArgs e)
        {
            //string typeValue = tbProductTypeOther.Text.ToLower();
            //string firstLetter = typeValue[0].ToString().ToUpper();
            //string otherType = firstLetter + typeValue.Substring(1);

            string otherType = tbProductTypeOther.Text;

            // to prevent adding null string and string with spaces only
            if (!string.IsNullOrWhiteSpace(otherType) && 
                !string.IsNullOrEmpty(otherType))
            {
                // to prevent adding already existing product type
                if (!clbProductType.Items.Contains(otherType))
                {
                    clbProductType.Items.Add(otherType, true);
                    clbProductType.SelectedIndex = clbProductType.Items.Count - 1;
                }
            }

            // reset value in any case
            tbProductTypeOther.Text = string.Empty;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] defaultTypes = {"Access Controller", "Access Controller Gateway", "Access Control Management System",
                                                       "Decoder", "Encoder", "Fixed Camera", "PTZ Camera", "Recorder", "Video Management System"
                                                      };
            List<string> lstDefaultTypes = new List<string>(defaultTypes);

            CheckedListBox.CheckedItemCollection checkedItems = clbProductType.CheckedItems;
            List<string> checkedValues = new List<string>();
            for (int i = 0; i < checkedItems.Count; i++)
                checkedValues.Add(checkedItems[i].ToString());

            // remove all checked items besides default values
            foreach (var item in checkedValues)
            {
                if (!lstDefaultTypes.Contains(item))
                    clbProductType.Items.Remove(item);
            }
        }

    }
}
