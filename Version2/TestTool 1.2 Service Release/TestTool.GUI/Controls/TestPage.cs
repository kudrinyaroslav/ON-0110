///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Trace;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;

namespace TestTool.GUI.Controls
{
    partial class TestPage : UserControl, ITestView
    {
        /// <summary>
        /// Controller
        /// </summary>
        private TestController _controller;

        /// <summary>
        /// Dictionary which allows find TreeView node by TestInfo structure
        /// </summary>
        private Dictionary<TestInfo, TreeNode> _testNodes;
        
        /// <summary>
        /// Tests selected by the user (by clicking corresponding checkbox)
        /// </summary>
        private List<TestInfo> _selectedTests;
        /// <summary>
        /// Test groups selected by the user.
        /// </summary>
        private List<string> _selectedGroups;

        /// <summary>
        /// Nodes holding information about test status (failed/passed). Nodes are coloured after a test is 
        /// completed (for both "Run Selected" and "Run current" buttons), and cleared when "Clear" or "Run selected"
        /// button is clicked.
        /// </summary>
        private List<TreeNode> _colouredNodes;

        /// <summary>
        /// Font for nodes representing completed tests.
        /// </summary>
        private Font _completedTestFont;
        /// <summary>
        /// TreeView nodes representing groups of tests.
        /// </summary>
        private List<TreeNode> _groupNodes;

        /// <summary>
        /// Flag indicating that we are in certification mode.
        /// </summary>
        private bool _certificationMode = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestPage()
        {
            InitializeComponent();
            _controller = new TestController(this);
            _selectedTests = new List<TestInfo>();
            _selectedGroups = new List<string>();
            _testNodes = new Dictionary<TestInfo, TreeNode>();
            _colouredNodes = new List<TreeNode>();

            _groupNodes = new List<TreeNode>();

           _completedTestFont = new Font(tvTestCases.Font.FontFamily, tvTestCases.Font.Size, FontStyle.Bold);
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
        public void DisplayTests(List<TestInfo> tests)
        {
            //_groupNumbers = new Dictionary<string, string>();

            try
            {
                foreach (TestInfo testInfo in tests.OrderBy(ti => ti.Category).ThenBy(ti => ti.Order))
                {

                    // Find group node to add this test to.
                    TreeNode groupNode = FindGroupNode(testInfo.Group);

                    // Add node
                    testInfo.Name = string.Format("{0}-{1} {2}", testInfo.Category, testInfo.Id, testInfo.Name);
                    TreeNode node = groupNode.Nodes.Add(testInfo.Name);

                    System.Diagnostics.Debug.WriteLine(testInfo.Name);

                    // check if all features required are supported
                    bool supported =
                        testInfo.RequiredFeatures.Where(
                            f => FeaturesHelper.FeatureRealization(f) == FeaturesHelper.FeatureRealizationType.Supported)
                            .Count() > 0;

                    // check if all features required are implemented.
                    bool implemented =
                        testInfo.RequiredFeatures.Where(
                            f => FeaturesHelper.FeatureRealization(f) == FeaturesHelper.FeatureRealizationType.Implemented)
                            .Count() > 0;

                    // select image (depends on requirement level)
                    node.ImageKey = FindImageKey(testInfo.RequirementLevel, supported, implemented);
                    node.SelectedImageKey = node.ImageKey;

                    // create tooltip
                    node.ToolTipText = CreateTestTooltip(testInfo, null);
                    node.Tag = testInfo;
                    // add test node to dictionary
                    _testNodes.Add(testInfo, node);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }        
        }

        /// <summary>
        /// Method for finding image key for test node.
        /// </summary>
        /// <param name="level">Requirement level</param>
        /// <param name="supported">True if any of features required is "potentially supported"</param>
        /// <param name="implemented">True if any of features required is "potentially implemented"</param>
        /// <returns>Key of image in image list.</returns>
        string FindImageKey(Tests.Common.Enums.RequirementLevel level,
            bool supported,
            bool implemented)
        {
            string imageKey = "None.ico";
            switch (level)
            {
                case Tests.Common.Enums.RequirementLevel.Must:
                    imageKey = "MUST.ico";
                    break;
                case Tests.Common.Enums.RequirementLevel.ConditionalMust:
                    if (supported)
                    {
                        if (implemented)
                        {
                            imageKey = "MUSTIFSUPIMPL.ico";
                        }
                        else
                        {
                            imageKey = "MUSTIFSUP.ico";
                        }
                    }
                    else
                    {
                        imageKey = "MUSTIFIML.ico";
                    }
                    break;
                case Tests.Common.Enums.RequirementLevel.ConditionalShould:
                    {
                        if (supported)
                        {
                            if (implemented)
                            {
                                imageKey = "SHOULDIFSUPIMPL.ico";
                            }
                            else
                            {
                                imageKey = "SHOULDIFSUP.ico";
                            }
                        }
                        else
                        {
                            if (implemented)
                            {
                                imageKey = "SHOULDIFIML.ico";
                            }
                            else
                            {
                                imageKey = "None.ico";
                            }
                        }
                    }
                    break;
                case Tests.Common.Enums.RequirementLevel.Should:
                    imageKey = "SHOULD.ico";
                    break;
                case Tests.Common.Enums.RequirementLevel.Optional:
                    imageKey = "OPTIONAL.ico";
                    break;
            }
            return imageKey;
        }

        const int FIRSTGROUPNUMBER = 4;

        /// <summary>
        /// Finds node representing tests group with path specified. If a node does not exist, node 
        /// is created.
        /// </summary>
        /// <param name="path">Group path.</param>
        /// <returns>Old or newly created node.</returns>
        TreeNode FindGroupNode(string path)
        {
            // path separator is "\"
            string[] parts = path.Split('\\');
            // root group
            string rootName = parts[0];

            TreeNode rootNode = null;

            // find root node
            foreach (TreeNode root in tvTestCases.Nodes)
            {
                if (root.Name == rootName)
                {
                    rootNode = root;
                    break;
                }
            }

            // if root node not found - create root node.
            if (rootNode == null)
            {
                rootNode = tvTestCases.Nodes.Add(rootName);
                rootNode.Name = rootName;
                _groupNodes.Add(rootNode);
#if DEBUG
                rootNode.ToolTipText = string.Format("path: [{0}], number: [{1}]", rootName, (3 + tvTestCases.Nodes.Count).ToString());
#endif

            }

            if (parts.Length == 1)
            {
                // If test is under root group
                return rootNode;
            }
            else
            {
                // Find or create other nodes 
                TreeNode current = rootNode;

                string currentPath = rootName;
                
                // for each group name in the path
                for (int i = 1; i < parts.Length; i++)
                {
                    string group = parts[i];
                    
                    TreeNode next = null;
                    // enumerate child of node found or created for parent group
                    foreach (TreeNode node in current.Nodes)
                    {
                        if (node.Name == group)
                        {
                            next = node;
                            break;
                        }
                    }
                    
                    currentPath += "\\" + group;
                    
                   
                    // if child not found, create new node.
                    if (next == null)
                    {
                        next = current.Nodes.Add(group);
                        _groupNodes.Add(next);

                        next.Name = group;
                    }
                    current = next;
                }

                return current;
            }

        }

        /// <summary>
        /// Creates test tooltip.
        /// </summary>
        /// <param name="testInfo">Test information</param>
        /// <param name="testResult">Information from the last test run.</param>
        /// <returns>Tooltip for test node.</returns>
        string CreateTestTooltip(TestInfo testInfo, TestResult testResult)
        {
            string testName = testInfo.Name;
            string requirementLevel = testInfo.GetRequirementString();
            string state = "NOT PERFORMED";

            if (testResult != null)
            {
                if (testResult.Log.TestStatus == TestStatus.NotSupported)
                {
                    state = "NOT SUPPORTED";
                }
                else
                {
                    state = testResult.Log.TestStatus.ToString().ToUpper();
                }
            }

            string tooltip = string.Format("{0}\r\nRequirement Level: {1} \r\nState: {2}",
                testName, requirementLevel, state);

            return tooltip;
        }

        #endregion

        #region Toolbar buttons

        /// <summary>
        /// Run selected test or tests in selected group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRunSelected_Click(object sender, EventArgs e)
        {
            TreeNode node = tvTestCases.SelectedNode;
            if (node != null)
            {

                TestInfo testInfo = (TestInfo) node.Tag;
                TestSuiteParameters parameters = GetParameters();

                if (testInfo != null)
                {
                    parameters.TestCases.Add(testInfo);
                }
                else
                {
                    List<TestInfo> tests = new List<TestInfo>();
                    AddChildNodes(node, tests);

                    List<TestInfo> allowedTests = new List<TestInfo>();
                    if (_certificationMode)
                    {
                        allowedTests.AddRange(tests.Where(t => FeaturesHelper.AllFeaturesSelected(t)));
                    }
                    else
                    {
                        allowedTests.AddRange(tests);
                    }

                    if (parameters.InteractiveFirst)
                    {
                        parameters.TestCases.AddRange(allowedTests.OrderBy(t => !t.Interactive).ThenBy(t => t.Order));
                    }
                    else
                    {
                        parameters.TestCases.AddRange(allowedTests.OrderBy(t => t.Order));
                    }
                }
                ClearTestInfo();
                _controller.RunSingle(parameters);
            }
        }

        /// <summary>
        /// Run all selected tests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRunAll_Click(object sender, EventArgs e)
        {
            ClearTestResults(false);

            TestSuiteParameters parameters = GetParameters();
            if (parameters.InteractiveFirst)
            {
                parameters.TestCases.AddRange(_selectedTests.OrderBy(t => !t.Interactive).ThenBy(T => T.Category).ThenBy(t => t.Order));
            }
            else
            {
                parameters.TestCases.AddRange(_selectedTests.OrderBy(T => T.Category).ThenBy(t => t.Order));
            }

            _controller.Run(parameters);
        }
        
        /// <summary>
        /// Collects parameters for launching current test/current test group/all selected tests
        /// </summary>
        /// <returns></returns>
        TestSuiteParameters GetParameters()
        {
            TestSuiteParameters parameters = new TestSuiteParameters();
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            parameters.Address = devices.ServiceAddress;
            parameters.CameraIP = devices.DeviceAddress;
            parameters.NetworkInterfaceController = devices.NIC;
            if((devices.Current != null)&&(devices.Current.ByDiscovery != null))
            {
                parameters.CameraUUID = devices.Current.ByDiscovery.UUID;
            }

            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
            parameters.MessageTimeout = environment.Timeouts.Message;
            parameters.RebootTimeout = environment.Timeouts.Reboot;
            parameters.TimeBetweenTests = environment.Timeouts.InterTests;
            parameters.InteractiveFirst = tsbInteractiveFirst.Checked;
            parameters.PTZNodeToken = environment.TestSettings.PTZNodeToken;

            parameters.UseEmbeddedPassword = environment.TestSettings.UseEmbeddedPassword;
            parameters.Password1 = environment.TestSettings.Password1;
            parameters.Password2 = environment.TestSettings.Password2;
            parameters.OperationDelay = environment.TestSettings.OperationDelay;
            parameters.RecoveryDelay = environment.TestSettings.RecoveryDelay;
            
            parameters.Features.AddRange(environment.Features);

            return parameters;
        }

        /// <summary>
        /// Clears tests execution results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClear_Click(object sender, EventArgs e)
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
                tsbPause.Image = Properties.Resources.RunSelected;
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
            lvStepDetails.Items.Clear();
            tbResponse.Clear();
            tbRequest.Clear();
            tbTestResult.Clear();
           
        }

        void ClearTestResults(bool bAll)
        {
            ClearTestInfo();
            foreach (TreeNode node in _colouredNodes)
            {
                node.ForeColor = tvTestCases.ForeColor;
                node.NodeFont = tvTestCases.Font;
                node.ToolTipText = CreateTestTooltip((TestInfo) node.Tag, null);
            }

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
                    {
                        this.Enabled = false;
                    }
                    break;
            }
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

        /// <summary>
        /// Adds line to log.
        /// </summary>
        /// <param name="logEntry">Log entry</param>
        void InternalWriteLine(string logEntry)
        {
            tbTestResult.AppendText(string.Format("{0}{1}", logEntry, Environment.NewLine));
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
            get { return _selectedTests; }
        }

        /// <summary>
        /// Returns list of selected test groups.
        /// </summary>
        public List<string> SelectedGroups
        {
            get { return _selectedGroups; }
        }

        /// <summary>
        /// Returns "Interactive tests first" option selected by user.
        /// </summary>
        public bool InteractiveFirst
        {
            get { return tsbInteractiveFirst.Checked; }

        }

        private bool _bProfileBeingApplied;

        /// <summary>
        /// Applies profile options.
        /// </summary>
        /// <param name="profile"></param>
        public void ApplyProfileOptions(Profile profile)
        {
            _bProfileBeingApplied = true;

            tsbInteractiveFirst.Checked = profile.InteractiveFirst;
        
            foreach (TestInfo testInfo in _testNodes.Keys)
            {
                _testNodes[testInfo].Checked = profile.TestCases.Contains(testInfo.Order);
            }
            foreach (TreeNode groupNode in _groupNodes)
            {
                groupNode.Checked = profile.TestGroups.Contains(groupNode.Name);
            }

            _bProfileBeingApplied = false;

        }

        /// <summary>
        /// Switches between certification and diagnostics modes.
        /// </summary>
        /// <param name="bOn">True, if certification mode should be entered.</param>
        public void SetCertificationMode(bool bOn)
        {
            _bProfileBeingApplied = true;

            if (bOn)
            {
                ClearTestResults(true);
                SelectAllTests();
            }

            _certificationMode = bOn;
            EnableRunCurrent();

            _bProfileBeingApplied = false;
        }

        /// <summary>
        /// Selects default tests for certification mode.
        /// All "Must", "Should", "Optional" tests are selected.
        /// All "Must if", "Should if" tests are selected depending on features selected at the "Management" page.
        /// </summary>
        void SelectAllTests()
        {

            foreach (TestInfo testInfo in _testNodes.Keys)
            {
                switch (testInfo.RequirementLevel)
                {
                    case Tests.Common.Enums.RequirementLevel.Must:
                    case Tests.Common.Enums.RequirementLevel.Should:
                    case Tests.Common.Enums.RequirementLevel.Optional:
                        //System.Diagnostics.Debug.WriteLine(string.Format("Select {0}", testInfo.Name));
                        _testNodes[testInfo].Checked = true;
                        break;
                    case Tests.Common.Enums.RequirementLevel.ConditionalMust:
                    case Tests.Common.Enums.RequirementLevel.ConditionalShould:
                        {
                            bool bClear = !FeaturesHelper.AllFeaturesSelected(testInfo);
                            _testNodes[testInfo].Checked = !bClear;
                            if (!bClear)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("Select {0}", testInfo.Name));
                            }

                        }
                        break;
                }
            }

            SelectTestGroups();
        }

        /// <summary>
        /// Selects test groups depending on tests selected.
        /// </summary>
        void SelectTestGroups()
        {
            List<TreeNode> orderedGroups = new List<TreeNode>();
            // collect all root group nodes
            foreach (TreeNode node in tvTestCases.Nodes)
            {
                if (node.Tag == null)
                {
                    orderedGroups.Add(node);
                }
            }
            int i = 0;
            // add group nodes "downstream"
            while (i<orderedGroups.Count)
            {
                foreach (TreeNode node in orderedGroups[i].Nodes)
                {
                    if (node.Tag == null)
                    {
                        orderedGroups.Add(node);
                    }
                }
                i++;
            }

            // select group nodes starting from "leaf" groups.
            for (int j = orderedGroups.Count-1; j>=0; j--)
            {
                CheckIfAllChildrenChecked(orderedGroups[j]);
            }
        }

        /// <summary>
        /// Selects "must if" and "should if" tests depending on features selected.
        /// </summary>
        /// <param name="testInfos"></param>
        public void SelectFeatureDependentTests(IEnumerable<TestInfo> testInfos)
        {
            foreach (TestInfo testInfo in _testNodes.Keys)
            {
                switch (testInfo.RequirementLevel)
                {
                    case Tests.Common.Enums.RequirementLevel.ConditionalMust:
                    case Tests.Common.Enums.RequirementLevel.ConditionalShould:
                        {
                            _testNodes[testInfo].Checked = testInfos.Contains(testInfo);
                        }
                        break;
                }
            }
            SelectTestGroups();
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

        #endregion

        #region

        /// <summary>
        /// Adds list item representing step results.
        /// </summary>
        /// <param name="result">Step information.</param>
        void AddStepResult(StepResult result)
        {
            ListViewItem stepItem = new ListViewItem(result.Number.ToString());
            stepItem.Tag = result;
            stepItem.SubItems.Add(result.Status.ToString());
            stepItem.SubItems.Add(result.Message);

            lvStepDetails.Items.Add(stepItem);
        }

        /// <summary>
        /// Selects tests which currently is being executed.
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        void HighlightActiveTest(TestInfo testInfo)
        {
            TreeNode node = _testNodes[testInfo];
            tvTestCases.SelectedNode = node;
        }

        /// <summary>
        /// Highlightes completed test.
        /// </summary>
        /// <param name="testResult">Test results.</param>
        void HighlightCompletedTest(TestResult testResult)
        {
            TreeNode node = _testNodes[testResult.TestInfo];
            Color color = tvTestCases.ForeColor;
            switch (testResult.Log.TestStatus)
            {
                case TestStatus.Passed:
                    color = Color.Green;
                    break;
                case TestStatus.Failed:
                    color = Color.Red;
                    break;
                case TestStatus.NotSupported:
                    color = Color.Gray;
                    break;
            }
            if (testResult.Log.TestStatus == TestStatus.Failed || testResult.Log.TestStatus == TestStatus.Passed)
            {
                node.NodeFont = _completedTestFont;
                node.Text = node.Text;
            }
            node.ToolTipText = CreateTestTooltip(testResult.TestInfo, testResult);
            node.ForeColor = color;
            _colouredNodes.Add(node);
            
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
                tsbRunSelected.Enabled = false;
                tsbPause.Enabled = !_certificationMode;
                tsbStop.Enabled = true;
                tsbHalt.Enabled = true;
                tsbClear.Enabled = false;
                tsbInteractiveFirst.Enabled = false;
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
            tsbRunSelected.Enabled = false;
            tsbPause.Enabled = true;
            tsbStop.Enabled = false;
            tsbHalt.Enabled = true;
            tsbClear.Enabled = false;
            tsbInteractiveFirst.Enabled = false;
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

                tsbRunAll.Enabled = _selectedTests.Count > 0;

                EnableRunCurrent();
                
                tsbPause.Enabled = false;
                tsbStop.Enabled = false;
                tsbHalt.Enabled = false;
                tsbClear.Enabled = true;
                tsbInteractiveFirst.Enabled = true;
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
            if (!_certificationMode)
            {
                tsbRunSelected.Enabled = tvTestCases.SelectedNode != null;
            }
            else
            {
                /* RunSelected in CertificationMode depends on features*/
                TreeNode node = tvTestCases.SelectedNode;
                if (node != null)
                {
                    bool bCanRunTest = true;

                    if (node.Tag != null)
                    {
                        TestInfo testInfo = (TestInfo)node.Tag;
                        if (_certificationMode)
                        {
                            bCanRunTest = FeaturesHelper.AllFeaturesSelected(testInfo);
                        }
                    }
                    else
                    {
                        bCanRunTest = GroupNodeAllowed(node);
                    }
                    tsbRunSelected.Enabled = bCanRunTest;
                }
                else
                {
                    tsbRunSelected.Enabled = false;
                }                
            }
        }

        /// <summary>
        /// Checks if any of tests beneath node specified is allowed.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        bool GroupNodeAllowed(TreeNode node)
        {
            List<TestInfo> tests = new List<TestInfo>();
            AddChildNodes(node, tests);

            return tests.Where(t => FeaturesHelper.AllFeaturesSelected(t)).FirstOrDefault() != null;
                       
        }

        #endregion

        #region Form events

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
                StepResult result = (StepResult) item.Tag;
                tbRequest.Text = result.Request;
                tbResponse.Text = result.Response;
            }
            else
            {
                tbRequest.Text = string.Empty;
                tbResponse.Text = string.Empty;
            }
        }

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
            if (_certificationMode && !_bProfileBeingApplied && e.Node.Tag != null)
            {
                TestInfo testInfo = (TestInfo)e.Node.Tag;
                
                if (testInfo.RequirementLevel == Tests.Common.Enums.RequirementLevel.Must
                    || testInfo.RequirementLevel == Tests.Common.Enums.RequirementLevel.ConditionalMust)
                {
                    // cannot unselect "Must"/"Must if" tests.
                    // (such tests are selected by default)
                    e.Cancel = true;
                }
                else if (testInfo.RequirementLevel == Tests.Common.Enums.RequirementLevel.ConditionalShould)
                {
                    // cannot select "Should if" tests if not all features are implemented.
                    // (such tests are unselected by default)
                    bool bAllSelected = FeaturesHelper.AllFeaturesSelected(testInfo);
                    if (!bAllSelected)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles logic for nodes selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTestCases_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            
            // Add/ test to selected tests or remove or handle group selection

            if (node.Tag != null)
            {
                // node represents test
                TestInfo testInfo = (TestInfo)node.Tag;
                
                if (node.Checked)
                {
                    // add test
                    if (!_selectedTests.Contains(testInfo))
                    {
                        _selectedTests.Add(testInfo);
                    }
                }
                else
                {
                    // remove tests
                    _selectedTests.Remove(testInfo);
                }            
            }
            else
            {
                //
                //  If profile is being applied, don't check/uncheck child nodes. 
                //  If certification mode is being entered, don't check/uncheck child nodes - in this case we check 
                //  groups depending on tests selected.
                //  If a group is selected/unselected depending on child nodes state, don't propogate selection 
                //  in opposite direction.
                if (!_bProfileBeingApplied && !_bubbleCheck)
                {
                    // in certification mode, features selected should be considered.
                    if (_certificationMode)
                    {
                        SelectAvailableChildTests(node);
                        CheckIfAllChildrenChecked(node);
                    }
                    else
                    {
                        // else select all child nodes.
                        foreach (TreeNode child in node.Nodes)
                        {
                            child.Checked = node.Checked;
                        }                        
                    }

                }

                // track group selection (for saving profile)
                if (node.Checked)
                {
                    if (!_selectedGroups.Contains(node.Name))
                    {
                        _selectedGroups.Add(node.Name);
                    }
                }
                else
                {
                    _selectedGroups.Remove(node.Name);
                }
            }

            // if node is selected by a user, check if parent state should be updated.
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                if (node.Checked)
                {
                    TryCheckParent(node);
                }
                else
                {
                    UncheckParent(node);
                }
            }

            tsbRunAll.Enabled = !_controller.Running && _selectedTests.Count > 0;
        }

        /// <summary>
        /// Selects child tests available for current features selection in certification mode.
        /// </summary>
        /// <param name="node"></param>
        void SelectAvailableChildTests(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Tag != null)
                {
                    TestInfo testInfo = (TestInfo) child.Tag;
                    
                    switch (testInfo.RequirementLevel)
                    {
                        case Tests.Common.Enums.RequirementLevel.Should:
                        case Tests.Common.Enums.RequirementLevel.Optional:
                            {
                                child.Checked = node.Checked;
                            }
                            break;
                        case Tests.Common.Enums.RequirementLevel.ConditionalShould:
                            {
                                if (node.Checked)
                                {
                                    bool bClear = !FeaturesHelper.AllFeaturesSelected(testInfo);
                                    _testNodes[testInfo].Checked = !bClear;
                                }
                                else
                                {
                                    child.Checked = false;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    // group
                    child.Checked = node.Checked;
                    CheckIfAllChildrenChecked(child);
                }
            }

        }

        /// <summary>
        /// Unchecks parent nodes when any node is unchecked.
        /// </summary>
        /// <param name="node"></param>
        void UncheckParent(TreeNode node)
        {
            TreeNode currentNode = node;
            while (currentNode.Parent != null)
            {
                _bubbleCheck = true;
                currentNode.Parent.Checked = false;
                _bubbleCheck = false;
                currentNode = currentNode.Parent;
            }
        }

        /// <summary>
        /// Is used to cancel checking/unchecking child nodes in the following situations:
        /// 
        /// - Node "1" has child nodes "1.1." and "1.2". All three nodes are selected. 
        /// User clears selection from child node "1.1", "1" should be unselected automatically, BUT 
        /// this should not affect selection of node "1.2" (See UncheckParent)
        /// - Similar situation, but started e.g. by selecting tests accordingly to profile.
        /// 
        /// </summary>
        private bool _bubbleCheck = false;

        /// <summary>
        /// Check nodes "upstream" where all child nodes are selected.  
        /// </summary>
        /// <param name="node"></param>
        void TryCheckParent(TreeNode node)
        {
            TreeNode currentNode = node;

            while (currentNode.Parent != null)
            {
                CheckIfAllChildrenChecked(currentNode.Parent);
                currentNode = currentNode.Parent;
            }
        }

        /// <summary>
        /// Checks tree node if all child nodes are checked.
        /// </summary>
        /// <param name="node"></param>
        void CheckIfAllChildrenChecked(TreeNode node)
        {
            bool bAllSiblingsChecked = true;
            foreach (TreeNode child in node.Nodes)
            {
                if (!child.Checked)
                {
                    bAllSiblingsChecked = false;
                    break;
                }
            }
            _bubbleCheck = true;
            node.Checked = bAllSiblingsChecked;
            _bubbleCheck = false;
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
                    
                    if (_certificationMode)
                    {
                        bCanRunTest = FeaturesHelper.AllFeaturesSelected(testInfo);
                    }
                }
                else
                {
                    if (_certificationMode)
                    {
                        bCanRunTest = GroupNodeAllowed(node);
                    }
                }
                tsbRunSelected.Enabled = !_controller.Running && bCanRunTest;
                //tsbRunSelected.Enabled = !_controller.Running;
            }
        }

        /// <summary>
        /// Displays test execution result when test is selected in tests tree.
        /// </summary>
        /// <param name="log">Test execution information.</param>
        void DisplayTestResults(TestResult log)
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


    }
}
