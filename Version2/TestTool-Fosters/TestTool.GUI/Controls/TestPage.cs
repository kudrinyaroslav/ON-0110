///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Trace;
using TestTool.GUI.Data;
using System.Drawing;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Engine;

namespace TestTool.GUI.Controls
{
    partial class TestPage : Page, ITestView
    {
        /// <summary>
        /// Controller
        /// </summary>
        private TestController _controller;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestPage()
        {
            InitializeComponent();
            _controller = new TestController(this);
        }
        
        /// <summary>
        /// Reference to controller.
        /// </summary>
        internal TestController Controller
        {
            get { return _controller; }
        }

        #region Tree initialization

        /// <summary>
        /// Builds tests tree from a list of tests.
        /// </summary>
        /// <param name="tests">List of tests (test information contains information about test group 
        /// also)</param>
        public void DisplayTests(IEnumerable<TestInfo> tests)
        {
            try
            {
                tvTestCases.DisplayTests(tests);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }        
        }

        public void DisplayProfiles(IEnumerable<IProfileDefinition> profiles)
        {
            tvProfiles.DisplayProfiles(profiles);
        }

        public void DisplayProfiles(IEnumerable<IProfileDefinition> supported,
            IEnumerable<IProfileDefinition> failed, IEnumerable<IProfileDefinition> notSupported)
        {
            BeginInvoke(
                new Action(
                    () =>
                        {
                            tvProfiles.DisplayProfiles(supported, failed, notSupported);
                        }));
        }

        public void DisplaySupportedFunctionality(Dictionary<Functionality, bool> functionality)
        {
            BeginInvoke(
                new Action(
                    () =>
                        {
                            tvProfiles.DisplaySupportedFunctionality(functionality);
                        }));
        }


        public void DisplayFunctionalityWithoutTestsInSuite(List<Functionality> functionality)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        tvProfiles.DisplayFunctionalityWithoutTestsInSuite(functionality);
                    }));
        }

        public void DisplaySkippedByFeaturesFunctionality(Dictionary<IProfileDefinition, List<Functionality>> functionality)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        tvProfiles.DisplaySkippedByFeaturesFunctionality(functionality);
                    }));
        }

        public void DisplayFunctionalityToBeTested(List<Functionality> functionality)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        tvProfiles.DisplayFunctionalityToBeTested(functionality);
                    }));
        }

        public void DisplayFailedByFeaturesFunctionality(Dictionary<IProfileDefinition, List<Functionality>> functionality)
        {

            BeginInvoke(
                new Action(
                    () =>
                        {
                            tvProfiles.DisplayFailedByFeaturesFunctionality(functionality);
                        }));
        }

        public void ClearProfiles()
        {
            tvProfiles.ClearProfiles();
        }

        #endregion

        #region Toolbar buttons

        /// <summary>
        /// Run selected test or tests in selected group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRunCurrent_Click(object sender, EventArgs e)
        {
            TreeNode node = tvTestCases.SelectedNode;
            if (node != null)
            {
                TestInfo testInfo = (TestInfo) node.Tag;
                TestSuiteParameters parameters = _controller.GetParameters();

                if (testInfo != null)
                {
                    parameters.TestCases.Add(testInfo);
                }
                else
                {
                    List<TestInfo> tests = new List<TestInfo>();
                    AddChildNodes(node, tests);

                    List<TestInfo> allowedTests = new List<TestInfo>();
                    allowedTests.AddRange(tests);

                    parameters.TestCases.AddRange(allowedTests.OrderBy(t => t.Order));
                }

                ClearTestInfo();
                _controller.RunSingle(parameters);
            }
        }

        private void tsbRunAll_Click(object sender, EventArgs e)
        {
            //ClearTestResults(true);
           _controller.RunAll();
        }

        private void tsbQueryFeatures_Click(object sender, EventArgs e)
        {
            DialogResult dr = DialogResult.Yes;
            if (_controller.HasTestResults())
            {
                dr = MessageBox.Show(this.FindForm(), "Test results wil be cleared. Continue?", "Warning",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (dr == DialogResult.Yes)
            {
                ClearTestResults(true);
                _controller.ClearFeatures();
                featuresTree.Clear();
                _controller.DefineFeatures(_controller.GetParameters());
            }
        }

        /// <summary>
        /// Run all selected tests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRunSelected_Click(object sender, EventArgs e)
        {
            ClearTestResults(false);

            TestSuiteParameters parameters = _controller.GetParameters();
            //parameters.InteractiveFirst = InteractiveFirst;
            //if (parameters.InteractiveFirst)
            //{
                parameters.TestCases.AddRange(tvTestCases.SelectedTests.OrderBy(TI => TI.ExecutionOrder).ThenBy(T => T.Category).ThenBy(t => t.Order));
            //}
            //else
            //{
            //    parameters.TestCases.AddRange(tvTestCases.SelectedTests.OrderBy(TI => TI.ExecutionOrder).ThenBy(T => T.Category).ThenBy(t => t.Order));
            //}

            _controller.Run(parameters);

        }


        /// <summary>
        /// Clears tests execution results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClear_Click(object sender, EventArgs e)
        {
            ClearTestResults(false);
        }

        private void tsbClearAll_Click(object sender, EventArgs e)
        {
            ClearTestResults(true);
        }
        
        /// <summary>
        /// Pauses tests execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPause_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} clicked", tsbPause.Text));
            
            _controller.Pause();
            tsbPause.Enabled = false; 
        }

        /// <summary>
        /// Updates pause button image and tooltip.
        /// </summary>
        void UpdatePauseButton()
        {
            if (_controller.State == TestState.Paused)
            {
                tsbPause.Text = "Continue";
                this.tsbPause.ToolTipText = "Continue tests execution";
                tsbPause.Image = Properties.Resources.RunCurrent;
            }
            else
            {
                tsbPause.Text = "Pause";
                this.tsbPause.ToolTipText = "Pause tests execution at IO operation";
                tsbPause.Image = Properties.Resources.Pause;
            }
        }

        /// <summary>
        /// Stops tests execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbStop_Click(object sender, EventArgs e)
        {
            tsbStop.Enabled = false;
            _controller.Stop();
        }

        /// <summary>
        /// Stops tests execution immediately.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbHalt_Click(object sender, EventArgs e)
        {
            _controller.Halt();
        }
        
        #endregion

        #region State

        void ClearTestInfo()
        {
            tcTestResults.ClearTestInfo();
        }

        public void ClearTestResults()
        {
            BeginInvoke(new Action( () =>
                            {
                                ClearTestInfo();
                                tvTestCases.ClearTestResults();
                                tvProfiles.ClearAll();
                            }));
        }

        void ClearTestResults(bool bAll)
        {
            _controller.Clear(bAll);
        }

        #endregion

        #region ITestView implementation

        /// <summary>
        /// Updates controls' availability.
        /// </summary>
        /// <param name="state"></param>
        public void SwitchToState(Enums.ApplicationState state)
        {
            switch (state)
            {
                case Enums.ApplicationState.Idle:
                    {
                        BeginInvoke(new Action(SwitchToIdleMode));
                    }
                    break;
                case Enums.ApplicationState.TestRunning:
                    {
                        BeginInvoke(new Action(SwitchToWorkingMode));
                    }
                    break;
                case Enums.ApplicationState.TestPaused:
                    {
                        BeginInvoke(new Action(SwitchToPausedMode));
                    }
                    break;
                case Enums.ApplicationState.CommandRunning:
                case Enums.ApplicationState.DiscoveryRunning:
                case Enums.ApplicationState.ConformanceTestRunning:
                    {
                        EnablePage(false);
                    }
                    break;
            }
        }

        void EnablePage(bool enable)
        {
            this.EnableControls(
                new Control[]
                    {
                        this.featuresTree,
                        this.tvTestCases
                    });
            foreach (ToolStripButton btn in new ToolStripButton[]{
                        this.tsbHalt,
                        this.tsbPause,
                        this.tsbQueryFeatures,
                        this.tsbRunAll,
                        this.tsbRunCurrent,
                        this.tsbRunSelected,
                        this.tsbStop})
            {
                btn.Enabled = enable;
            }
            this.toolStripDropDownButtonClear.Enabled = enable;
            this.toolStripDropDownButtonSave.Enabled = enable;
            tcTestResults.EnableControl(enable);
        }

        /// <summary>
        /// Begins test.
        /// </summary>
        /// <param name="testInfo"></param>
        public void BeginTest(TestInfo testInfo)
        {
            BeginInvoke(new Action(() => {
                                             ClearTestInfo();
                                             HighlightActiveTest(testInfo);
            }));
        }

        /// <summary>
        /// Ends test.
        /// </summary>
        /// <param name="testResult"></param>
        public void EndTest(TestResult testResult)
        {
            BeginInvoke(new Action(() => HighlightCompletedTest(testResult) ));
        }

        /// <summary>
        /// Displays step result.
        /// </summary>
        /// <param name="result"></param>
        public void DisplayStepResult(StepResult result)
        {
            BeginInvoke(new Action( () => AddStepResult(result)));
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

        public void ClearCurrentLog()
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        tcTestResults.ClearTestInfo();
                    }));

        }

        /// <summary>
        /// Adds line to log.
        /// </summary>
        /// <param name="logEntry">Log entry</param>
        void InternalWriteLine(string logEntry)
        {
            tcTestResults.WriteLine(logEntry);
        }

        /// <summary>
        /// Enables/disables test running.
        /// </summary>
        /// <param name="enable">True if buttons should be enabled; false otherwise.</param>
        public void EnableTestRun(bool enable)
        {
            toolStripTestManagement.Enabled = enable;
        }

        /// <summary>
        /// Returns list of tests selected by test operator.
        /// </summary>
        public List<TestInfo> SelectedTests
        {
            get { return tvTestCases.SelectedTests; }
        }

        /// <summary>
        /// Returns list of selected test groups.
        /// </summary>
        public List<string> SelectedGroups
        {
            get { return tvTestCases.SelectedGroups; }
        }
        
        /// <summary>
        /// Applies profile options.
        /// </summary>
        /// <param name="profile"></param>
        public void ApplyProfileOptions(Profile profile)
        {
            tvTestCases.ApplyProfileOptions(profile);
        }
        
        public void SelectTests(IEnumerable<TestInfo> testInfos)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        tvTestCases.SelectTests(testInfos);
                    }));
        }

        /// <summary>
        /// Reports tests execution completion.
        /// </summary>
        public void ReportTestSuiteCompleted()
        {
            BeginInvoke(
                new Action(
                    () =>
                        {
                            MessageBox.Show(this.FindForm(), "Tests completed", "Done", MessageBoxButtons.OK);
                        })); 
        }

        /// <summary>
        /// Returns application main window.
        /// </summary>
        public Form Window 
        { 
            get
            {
                return this.FindForm();   
            }
        }
        
        private VideoContainer _videoWindow;


        /// <summary>
        /// Gets video form.
        /// </summary>
        /// <returns></returns>
        public IVideoForm GetVideoForm()
        {
            if (_videoWindow == null)
            {
                VideoContainer wnd = new VideoContainer();
                _videoWindow = wnd;
            }
            return _videoWindow;
        }

        public void DisplayFeature(Feature feature, bool supported)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        featuresTree.DisplayFeature(feature, supported);
                    }));
        }

        public void DisplayUndefinedFeature(Feature feature)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        featuresTree.DisplayUndefinedFeature(feature);
                    }));
        }

        public void DisplayScope(string scope, bool supported)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        tvProfiles.DisplayScope(scope, supported);
                    }));
        }

        public void ClearFeatures()
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        featuresTree.Clear();
                    }));
        }

        #endregion

        #region

        /// <summary>
        /// Adds list item representing step results.
        /// </summary>
        /// <param name="result">Step information.</param>
        void AddStepResult(StepResult result)
        {
            tcTestResults.AddStepResult(result);
        }

        /// <summary>
        /// Selects tests which currently is being executed.
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        void HighlightActiveTest(TestInfo testInfo)
        {
            tvTestCases.HighlightActiveTest(testInfo);
        }

        /// <summary>
        /// Highlightes completed test.
        /// </summary>
        /// <param name="testResult">Test results.</param>
        void HighlightCompletedTest(TestResult testResult)
        {
            tvTestCases.HighlightCompletedTest(testResult);
        }

        /// <summary>
        /// Adds test to list. 
        /// </summary>
        /// <param name="node">Treevice node representing test or group.</param>
        /// <param name="testInfos"></param>
        void AddChildNodes(TreeNode node, List<TestInfo> testInfos)
        {
            if (node.Tag != null)
            {
                TestInfo testInfo = (TestInfo) node.Tag;
                testInfos.Add(testInfo);
            }

            foreach (TreeNode child in node.Nodes)
            {
                AddChildNodes(child, testInfos);
            }
        }

        /// <summary>
        /// Changes controls availability when tests are being executed.
        /// </summary>
        void SwitchToWorkingMode()
        {
            Invoke(new Action(() =>
            {
                tsbRunAll.Enabled = false;
                tsbRunCurrent.Enabled = false;
                tsbRunSelected.Enabled = false;
                tsbQueryFeatures.Enabled = false;
                tsbPause.Enabled = true;
                tsbStop.Enabled = true;
                tsbHalt.Enabled = true;
                toolStripDropDownButtonClear.Enabled = false;
                toolStripDropDownButtonSave.Enabled = false;
                this.Cursor = Cursors.AppStarting;
                toolStripTestManagement.Cursor = Cursors.Default;

                UpdatePauseButton();
            }));
        }

        /// <summary>
        /// Changes controls availability when tests execution is paused.
        /// </summary>
        void SwitchToPausedMode()
        {
            tsbRunAll.Enabled = false;
            tsbRunCurrent.Enabled = false;
            tsbRunSelected.Enabled = false;
            tsbQueryFeatures.Enabled = false;
            tsbPause.Enabled = true;
            tsbStop.Enabled = false;
            tsbHalt.Enabled = true;
            toolStripDropDownButtonClear.Enabled = false;
            toolStripDropDownButtonSave.Enabled = false;
            this.Cursor = Cursors.Arrow;
            toolStripTestManagement.Cursor = Cursors.Default;

            UpdatePauseButton();
        }

        /// <summary>
        /// Changes controls availability when no time-consuming operations are being performed.
        /// </summary>
        void SwitchToIdleMode()
        {
            Invoke(new Action(() =>
            {
                this.Enabled = true;
                tcTestResults.EnableControl(true);
                EnableControls(new Control[]{tvTestCases, featuresTree});

                tsbRunSelected.Enabled = SelectedTests.Count > 0;
                tsbRunAll.Enabled = true;
                tsbQueryFeatures.Enabled = true;
                EnableRunCurrent();
                
                tsbPause.Enabled = false;
                tsbStop.Enabled = false;
                tsbHalt.Enabled = false;
                toolStripDropDownButtonClear.Enabled = true;
                toolStripDropDownButtonSave.Enabled = true;
                this.Cursor = Cursors.Arrow;
                toolStripTestManagement.Cursor = Cursors.Default;

                UpdatePauseButton();
            }));
        }

        /// <summary>
        /// Enables/disables running selected test(s).
        /// </summary>
        void EnableRunCurrent()
        {
            tsbRunCurrent.Enabled = tvTestCases.SelectedNode != null;
        }

        #endregion

        #region Form events

        /// <summary>
        /// Cancels test selection if forbidden (test is running, certification mode)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTestCases_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (_controller.Running)
            {
                // no selection when test is running
                e.Cancel = true;
            }

        }

        /// <summary>
        /// Handles logic for nodes selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTestCases_AfterCheck(object sender, TreeViewEventArgs e)
        {
            tsbRunSelected.Enabled = !_controller.Running && SelectedTests.Count > 0;
        }

        /// <summary>
        /// Handles selection in tests tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTestCases_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_controller.Running)
            {
                return;
            }

            TreeNode node = tvTestCases.SelectedNode;
            ClearTestInfo();            
            if (node != null)
            {
                bool bCanRunTest = true;

                if (node.Tag != null)
                {
                    TestInfo testInfo = (TestInfo) node.Tag;
                    TestResult log = _controller.GetTestResult(testInfo);
                    
                    if (log != null)
                    {
                        DisplayTestResults(log);
                    }
                }
                tsbRunCurrent.Enabled = !_controller.Running && bCanRunTest;
            }
            tvProfiles.SetInactive();
            featuresTree.SetInactive();
        }

        /// <summary>
        /// Displays test execution result when test is selected in tests tree.
        /// </summary>
        /// <param name="log">Test execution information.</param>
        void DisplayTestResults(TestResult log)
        {
            tcTestResults.DisplayTestResults(log);
        }

        #endregion
        
        #region IView Members

        /// <summary>
        /// Returns controller.
        /// </summary>
        /// <returns></returns>
        public IController GetController()
        {
            return _controller;
        }

        #endregion


        /// <summary>
        /// Forbids selecting test cases during execution. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTestCases_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_controller.Running)
            {
                if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
                {
                    e.Cancel = true;
                }
            }
        }


        private void featuresTree_Click(object sender, EventArgs e)
        {
            if (!Controller.Running)
            {
                TestResult log = _controller.GetFeaturesDefinitionLog();
                if (log != null)
                {
                    ClearTestInfo();
                    DisplayTestResults(log);
                }
                tvTestCases.SetInactive();
                tvProfiles.SetInactive();
            }
        }

        void tvProfiles_ProfileSelected(IProfileDefinition profile)
        {
            if (!Controller.Running)
            {
                if (profile != null)
                {
                    ProfileTestInfo info = _controller.GetProfileInformation(profile);
                    if (info != null)
                    {
                        tcTestResults.DisplayProfileTestLog(info.Log);
                    }
                }   
                tvTestCases.SetInactive();
                featuresTree.SetInactive();
            }

        }


        void tcTestsAndFeatures_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush textBrush;

            // Get the item from the collection.
            TabPage tabPage = tcTestsAndFeatures.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle tabBounds = tcTestsAndFeatures.GetTabRect(e.Index);
            
            if (e.State == DrawItemState.Selected)
            {
                textBrush = new System.Drawing.SolidBrush(tcTestsAndFeatures.ForeColor);
                // Draw a different background color, and don't paint a focus rectangle.
                g.FillRectangle(new SolidBrush(tcTestsAndFeatures.BackColor), e.Bounds);
            }
            else
            {
                textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                g.FillRectangle(new SolidBrush(tcTestsAndFeatures.BackColor), e.Bounds);
            }

            Font tabFont = tcTestsAndFeatures.Font;  
            
            //e.Graphics.TranslateTransform(0, tabBounds.Height);
            e.Graphics.RotateTransform(180);

            tabBounds.X = - tabBounds.X - tabBounds.Width;
            tabBounds.Y = - tabBounds.Y - tabBounds.Height;
            
            // Draw string. Center the text.
            StringFormat stringFlags = new StringFormat();
            stringFlags.Alignment = StringAlignment.Center;
            stringFlags.LineAlignment = StringAlignment.Center;
            stringFlags.FormatFlags = StringFormatFlags.DirectionVertical;
            g.DrawString(tabPage.Text, tabFont, textBrush, tabBounds, new StringFormat(stringFlags));

            e.Graphics.ResetTransform();

        }

        #region Save

        void toolStripSplitButtonSave_DropDownOpening(object sender, System.EventArgs e)
        {
            toolStripMenuItemSaveAll.Enabled = _controller.HasTestResults();
            saveFeatureDefinitionLogToolStripMenuItem.Enabled = _controller.GetFeaturesDefinitionLog() != null;

            TreeNode node = tvTestCases.SelectedNode;
            if (node != null)
            {
                bool canSaveResult = false;

                if (node.Tag != null)
                {
                    TestInfo testInfo = (TestInfo)node.Tag;
                    canSaveResult = _controller.GetTestResult(testInfo) != null;
                }
                else
                {
                    // group
                    List<TestInfo> tests = new List<TestInfo>();
                    AddChildNodes(node, tests);
                    foreach (TestInfo ti in tests)
                    {
                        if (_controller.GetTestResult(ti) != null)
                        {
                            canSaveResult = true;
                            break;
                        }
                    }
                }
                toolStripMenuItemSaveCurrent.Enabled = canSaveResult;
            }
            else
            {
                toolStripMenuItemSaveCurrent.Enabled = false;
            }
        }

        void toolStripMenuItemSave_Click(object sender, System.EventArgs e)
        {
            List<TestResult> results = new List<TestResult>();
            
            TreeNode node = tvTestCases.SelectedNode;
            if (node != null)
            {
                TestInfo testInfo = (TestInfo) node.Tag;

                if (testInfo != null)
                {
                    results.Add(_controller.GetTestResult(testInfo));
                }
                else
                {
                    List<TestInfo> tests = new List<TestInfo>();
                    AddChildNodes(node, tests);
                    foreach (TestInfo ti in tests)
                    {
                        TestResult tr = _controller.GetTestResult(ti);
                        if (tr != null)
                        {
                            results.Add(tr);
                        }
                    }
                }
            }
            Save(results);
        }

        void toolStripMenuItemSaveAll_Click(object sender, System.EventArgs e)
        {
            List<TestResult> results = new List<TestResult>();
            foreach (TestInfo ti in _controller.TestInfos)
            {
                TestResult tr = _controller.GetTestResult(ti);
                if (tr != null)
                {
                    results.Add(tr);
                }
            }
            Save(results);
        }

        void toolStripMenuItemSaveFeatureDefinitionLog_Click(object sender, System.EventArgs e)
        {
            TestResult tr = new TestResult();
            tr.Log =  _controller.GetFeaturesDefinitionLog().Log;
            tr.PlainTextLog = _controller.GetFeaturesDefinitionLog().PlainTextLog;
            SaveInternal((fileName) => { _controller.Save(fileName, tr); });
        }

        void Save(List<TestResult> results)
        {
            SaveInternal((fileName) => { _controller.Save(fileName, results); });
        }

        void SaveInternal(Action<string> saveAction)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".xml";
            sfd.Filter = "XML file | *.xml | All Files | *.*";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                saveAction(sfd.FileName);
            }
        }

        public void ReportError(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

    }
}
