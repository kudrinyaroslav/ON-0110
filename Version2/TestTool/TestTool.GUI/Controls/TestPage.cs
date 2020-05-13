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
using TestTool.GUI.Data;
using System.Drawing;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Engine;
using TestTool.Tests.Engine.Data;

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

        bool currentTestSelected = true;

        #region Tree initialization


        #endregion

        #region Toolbar buttons

        /// <summary>
        /// Run selected test or tests in selected group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRunCurrent_Click(object sender, EventArgs e)
        {
            currentTestSelected = true;
            RunCurrent(FeatureDefinitionMode.Default);
        }

        void RunCurrent(Tests.Engine.FeatureDefinitionMode mode)
        {
            TreeNode node = tvTestCases.SelectedNode;
            if (node != null)
            {
                TestInfo testInfo = (TestInfo)node.Tag;
                TestSuiteParameters parameters = _controller.GetParameters();
                parameters.FeatureDefinition = mode;

                if (testInfo != null)
                {
                    parameters.TestCases.Add(testInfo);
                }
                else
                {
                    var tests = new List<TestInfo>();
                    AddChildNodes(node, tests);

                    parameters.TestCases = new ExecutableTestList(tests.OrderBy(t => t.Order));
                }

                ClearTestInfo();
                _controller.RunSingle(parameters);
            }

        }

        private void tsbRunAll_Click(object sender, EventArgs e)
        {
            currentTestSelected = true;
            //ClearTestResults(true);
            _controller.RunAll();
        }

        private void tsbQueryFeatures_Click(object sender, EventArgs e)
        {
            currentTestSelected = true;

            DialogResult dr = DialogResult.Yes;
            if (_controller.HasTestResults())
            {
                dr = MessageBox.Show(this.FindForm(), "Test results will be cleared. Continue?", "Warning",
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
            currentTestSelected = true;
            RunSelected(FeatureDefinitionMode.Default);
        }

        void RunSelected(FeatureDefinitionMode mode)
        {
            ClearTestResults(false);
            TestSuiteParameters parameters = _controller.GetParameters();
            parameters.FeatureDefinition = mode;
            parameters.TestCases = tvTestCases.SelectedTests;
            tvTestCases.SelectedTests.Prepare();
            //parameters.TestCases.AddRange(tvTestCases.SelectedTests.OrderBy(TI => TI.ExecutionOrder).ThenBy(T => T.Category).ThenBy(t => t.Order));
            
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
            // to prevent more than one click
            tsbHalt.Enabled = false;

            tcTestResults.EnableSearch();
        }

        #endregion

        #region State

        void ClearTestInfo()
        {
            tcTestResults.ClearTestInfo();
        }

        public void ClearTestResults()
        {
            BeginInvoke(new Action(() =>
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
        public override void SwitchToState(Enums.ApplicationState state)
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
                        this.tsbRepeatTests,
                        this.tsbStop})
            {
                btn.Enabled = enable;
            }
            this.toolStripDropDownButtonClear.Enabled = enable;
            this.toolStripDropDownButtonSave.Enabled = enable;
            tsbRunCurrent.Enabled = enable;
            tsbRunSelected.Enabled = enable;
            tcTestResults.EnableControl(enable);
        }
        
        /// <summary>
        /// Begins test.
        /// </summary>
        /// <param name="testInfo"></param>
        public void BeginTest(TestInfo testInfo)
        {
            System.Diagnostics.Debug.WriteLine("BeginTest:::ClearTestInfo");

            BeginInvoke(new Action(() =>
                                       {
                                           //if (_controller.ScrollingEnabled)
                                           //{
                                           //    System.Diagnostics.Debug.WriteLine("INVOKED ClearTestInfo");
                                           //    ClearTestInfo();
                                           //}
                                           HighlightActiveTest(testInfo);

                                           // disable search functionality while tests execution started
                                           if (currentTestSelected)
                                            tcTestResults.DisableSearch();
                                       }));
        }

        /// <summary>
        /// Ends test.
        /// </summary>
        /// <param name="testResult"></param>
        public void EndTest(TestResult testResult)
        {
            BeginInvoke(new Action(() =>
                                        {
                                            HighlightCompletedTest(testResult);

                                            // enable search functionality while tests execution finished
                                            tcTestResults.EnableSearch();
                                        }
                                         ));
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

        public bool Repeat 
        { 
            get { return tsbRepeatTests.Checked;}
            set { tsbRepeatTests.Checked = value; }
        }


        #endregion

        #region



        /// <summary>
        /// Selects tests which currently is being executed.
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        void HighlightActiveTest(TestInfo testInfo)
        {
            tvTestCases.HighlightActiveTest(testInfo, _controller.ScrollingEnabled);
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
        /// <param name="node">Treeview node representing test or group.</param>
        /// <param name="testInfos"></param>
        void AddChildNodes(TreeNode node, List<TestInfo> testInfos)
        {
            if (node.Tag != null)
            {
                TestInfo testInfo = (TestInfo)node.Tag;
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
                tsbRepeatTests.Enabled = false;
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
                EnableControls(new Control[] { tvTestCases, featuresTree });

                tsbRunSelected.Enabled = tvTestCases.SelectedTests.Count > 0;
                tsbRunAll.Enabled = true;
                tsbQueryFeatures.Enabled = true;
                tsbRepeatTests.Enabled = true;
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
            //if (_controller.Running)
            if (_controller.Running && (_controller.Conformance || _controller.Single))
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
            tsbRunSelected.Enabled = !_controller.Running
                                     && 
                                     (
                                        (tvTestCases.SelectedTests.Contains(FeaturesDefinitionProcess.This) && tvTestCases.SelectedTests.Count > 1)
                                        || (!tvTestCases.SelectedTests.Contains(FeaturesDefinitionProcess.This) && tvTestCases.SelectedTests.Count > 0)
                                     );
        }

        /// <summary>
        /// Handles selection in tests tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTestCases_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // !!! Check that this not a group! Expanding group is not a test click!
            
            // Running :
            // if scrolling is disabled:
            //   - current test: __enable scrolling___
            //   - completed test : show results
            //   - not started test : ___enable scrolling___ and switch to current (?)
            // if scrolling is enabled:
            //   - completed test: stop scrolling and show results
            //   - current test : do nothing
            //   - not started test : stop scrolling, clear results

            // Idle :
            // - show results if exist;
            // - allow "Run Current" button
            
            TreeNode node = tvTestCases.SelectedNode;
            
            if (node != null)
            {
                if (node.Tag != null)
                {
                    TestInfo testInfo = (TestInfo)node.Tag;

                    bool showResults = true;
                    
                    if (_controller.Running)
                    {
                        System.Diagnostics.Debug.WriteLine("Controller running");
                        if (_controller.ScrollingEnabled)
                        {
                            // if not current, stop scrolling
                            // current result will be displayed when scrolling is started
                            if (_controller.CurrentTest != testInfo)
                            {
                                System.Diagnostics.Debug.WriteLine("Disable scrolling");
                                EnableScrolling(false);

                                tcTestResults.EnableSearch();
                                currentTestSelected = false;
                            }
                        }
                        else
                        {
                            // if current test - enable scrolling
                            if (_controller.CurrentTest == testInfo)
                            {
                                System.Diagnostics.Debug.WriteLine("Enable scrolling");
                                EnableScrolling(true);
                                showResults = false;

                                tcTestResults.DisableSearch();
                                currentTestSelected = true;
                            }
                        }

                        showResults = !_controller.ScrollingEnabled;
                    }
                    

                    if (showResults)
                    {
                        ClearTestInfo();
                        TestResult log = _controller.GetTestResult(testInfo);
                        if (log != null)
                        {
                            DisplayTestResults(log);
                        }
                    }

                }
                else
                {
                    // Group 
                    if (!(_controller.Running && _controller.ScrollingEnabled))
                    {
                        ClearTestInfo();
                    }
                }

                tvProfiles.SetInactive();
                featuresTree.SetInactive();

                tsbRunCurrent.Enabled = !_controller.Running ;
            }

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
        public override IController GetController()
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
            // will not forbid any selection. 
            //if (_controller.Running && _controller.ScrollingEnabled)
            //{
            //    if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }

        private void featuresTree_Click(object sender, EventArgs e)
        {
            TestResult log = null;
            if (Controller.Running)
            {
                bool featureDefinitionProcessRunning = (_controller.CurrentTest  != null &&
                                                        _controller.CurrentTest.ProcessType == ProcessType.FeatureDefinition);

                if (_controller.ScrollingEnabled)
                {
                    if (!featureDefinitionProcessRunning)
                    {
                        _controller.EnableScrolling(false);
                    }
                }
                else
                {
                    if (featureDefinitionProcessRunning)
                    {
                        EnableScrolling(true);
                        // log will be displayed in EnableScrolling.
                    }
                    else
                    {
                        log = _controller.GetFeaturesDefinitionLog();
                    }
                }
            }
            else
            {
                log = _controller.GetFeaturesDefinitionLog();
            }

            if (log != null)
            {
                ClearTestInfo();
                DisplayTestResults(log);
            }
            tvTestCases.SetInactive();
            tvProfiles.SetInactive();
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

            tabBounds.X = -tabBounds.X - tabBounds.Width;
            tabBounds.Y = -tabBounds.Y - tabBounds.Height;

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
            toolStripMenuItemSaveWarnings.Enabled =
                _controller.HasWarningsInTests() || _controller.HasWarningsInFeaturesLog();

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
                TestInfo testInfo = (TestInfo)node.Tag;

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
            tr.Log = _controller.GetFeaturesDefinitionLog().Log;
            tr.PlainTextLog = _controller.GetFeaturesDefinitionLog().PlainTextLog;
            SaveInternal((fileName) => { _controller.Save(fileName, tr); });
        }

        void Save(List<TestResult> results)
        {
            SaveInternal((fileName) => { _controller.Save(fileName, results); });
        }

        void Save(List<TestResultWithWarnings> results)
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
        
        #endregion


        #region ITestView Members

        public IProfilesView ProfilesView
        {
            get { return tvProfiles; }
        }

        public IFeaturesView FeaturesView
        {
            get { return featuresTree; }
        }

        public ITestResultView TestResultView
        {
            get { return tcTestResults; }
        }

        public ITestTreeView TestTreeView
        {
            get { return tvTestCases; }
        }

        #endregion

        void EnableScrolling(bool enable)
        {
            if (_controller.Running && enable)
            {
                TestResult log = _controller.GetCurrentLog();
                if (log != null)
                {
                    tcTestResults.ClearTestInfo();
                    DisplayTestResults(log);
                }
            }

            _controller.EnableScrolling(enable);
        }

        private void tsbRepeatTests_Click(object sender, EventArgs e)
        {
            bool repeat = tsbRepeatTests.Checked;
            tsbRepeatTests.Text = repeat ? "Repeat " : "No Repeat";
            tsbRepeatTests.Image = repeat ? Properties.Resources.RepeatTest : Properties.Resources.RunOnce;
        }

        #region Skip feature definition

        private void runSelectedAssumeAllFeaturesSupportedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunSelected(FeatureDefinitionMode.AssumeSupported);
        }

        private void runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunSelected(FeatureDefinitionMode.AssumeNotSupported);
        }

        private void runCurrentAssumeAllFeaturesSupportedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunCurrent(FeatureDefinitionMode.AssumeSupported);
        }

        private void runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunCurrent(FeatureDefinitionMode.AssumeNotSupported);
        }

        private void tsbRunCurrent_DropDownOpening(object sender, EventArgs e)
        {
            runCurrentAssumeAllFeaturesSupportedToolStripMenuItem.Enabled = !_controller.FeaturesDefined;
            runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem.Enabled = !_controller.FeaturesDefined;
        }

        private void tsbRunSelected_DropDownOpening(object sender, EventArgs e)
        {
            runSelectedAssumeAllFeaturesSupportedToolStripMenuItem.Enabled = !_controller.FeaturesDefined;
            runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem.Enabled = !_controller.FeaturesDefined;

        }

        #endregion

        private void toolStripMenuItemSaveWarnings_Click(object sender, EventArgs e)
        {
            List<TestResultWithWarnings> results = new List<TestResultWithWarnings>();

            // Warnings in Feature Log
            TestResult trFeatures = _controller.GetFeaturesDefinitionLog();
            if (trFeatures != null && trFeatures.Warnings != null && 
                trFeatures.Warnings.Count > 0)
            {
                TestResultWithWarnings result = new TestResultWithWarnings();
                result.TestName = trFeatures.TestInfo.Name;
                result.TestStatus = trFeatures.Log.TestStatus.ToString();
                result.TestWarnings = new List<string>(trFeatures.Warnings);

                results.Add(result);
            }

            // Warnings in Test Results Log
            foreach (TestInfo ti in _controller.TestInfos)
            {
                TestResult tr = _controller.GetTestResult(ti);
                if (tr != null && tr.Warnings != null)
                {
                    TestResultWithWarnings result = new TestResultWithWarnings();
                    
                    result.TestName = tr.TestInfo.Name;
                    result.TestStatus = tr.Log.TestStatus.ToString();
                    result.TestWarnings = new List<string>(tr.Warnings);

                    results.Add(result);
                }
            }

            Save(results);
        }


    }
}
