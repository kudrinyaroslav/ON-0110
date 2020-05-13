
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.GUI.Views;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Trace;

namespace TestTool.GUI.Controls
{
    public partial class TestsTree : UserControl, ITestTreeView
    {
        public TestsTree()
        {
            InitializeComponent();

            _selectedTests = new List<TestInfo>();
            _selectedGroups = new List<string>();
            _testNodes = new Dictionary<TestInfo, TreeNode>();
            _colouredNodes = new List<TreeNode>();

            _groupNodes = new List<TreeNode>();

            _completedTestFont = new Font(tvTestCases.Font.FontFamily, tvTestCases.Font.Size, FontStyle.Bold);

        }

        public event TreeViewEventHandler AfterSelect;

        public event TreeViewCancelEventHandler BeforeSelect;

        public event TreeViewEventHandler AfterCheck;

        public event TreeViewCancelEventHandler BeforeCheck;

        /// <summary>
        /// Dictionary which allows find TreeView node by TestInfo structure
        /// </summary>
        private Dictionary<TestInfo, TreeNode> _testNodes;

        /// <summary>
        /// Tests selected by the user (by clicking corresponding checkbox)
        /// </summary>
        private List<TestInfo> _selectedTests;

        public List<TestInfo> SelectedTests
        {
            get { return _selectedTests; }
        }
        /// <summary>
        /// Test groups selected by the user.
        /// </summary>
        private List<string> _selectedGroups;

        /// <summary>
        /// Returns list of selected test groups.
        /// </summary>
        public List<string> SelectedGroups
        {
            get { return _selectedGroups; }
        }

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

        private bool _certificationMode;
        public bool CertificationMode
        {
            get { return _certificationMode; }
            set { _certificationMode = value; }
        }

        public TreeNode SelectedNode
        {
            get
            {
                return tvTestCases.SelectedNode;
            }
            set
            {
                tvTestCases.SelectedNode = value;
            }
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
                foreach (TestInfo testInfo in tests.OrderBy(ti => ti.Category).ThenBy(ti => ti.Order))
                {

                    // Find group node to add this test to.
                    TreeNode groupNode = FindGroupNode(testInfo.Group);

                    // Add node
                    testInfo.Name = string.Format("{0}-{1} {2}", testInfo.Category, testInfo.Id, testInfo.Name);
                    TreeNode node = groupNode.Nodes.Add(testInfo.Name);


#if DEBUG
                    if (testInfo.FunctionalityUnderTest != null && testInfo.FunctionalityUnderTest.Count > 0)
                    {
                        string functionalities = ";";
                        foreach (TestTool.Tests.Definitions.Enums.Functionality f in testInfo.FunctionalityUnderTest)
                        {
                            functionalities += f.ToString() + " ";
                        }
                        System.Diagnostics.Debug.WriteLine(testInfo.Name + functionalities);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(testInfo.Name);
                    }
#else
                System.Diagnostics.Debug.WriteLine(testInfo.Name);
#endif

                    // select image (depends on requirement level)
                    node.ImageKey = FindImageKey(testInfo);
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
        /// <param name="info">Test information</param>
        /// <returns>Key of image in image list.</returns>
        string FindImageKey(TestInfo info)
        {
            string imageKey = "None.ico";
            switch (info.RequirementLevel)
            {
                case Tests.Definitions.Enums.RequirementLevel.Must:
                    if (info.RequiredFeatures.Count == 0)
                    {
                        imageKey = "MUST.ico";
                    }
                    else
                    {
                        imageKey = "MUSTIF.ico";
                    }
                    break;
                case Tests.Definitions.Enums.RequirementLevel.Optional:
                    if (info.RequiredFeatures.Count == 0)
                    {
                        imageKey = "OPTIONAL.ico";
                    }
                    else
                    {
                        imageKey = "OPTIONALIF.ico";
                    }
                    break;
            }
            return imageKey;
        }

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
                rootNode.ImageKey = "Group";
                rootNode.SelectedImageKey = "Group";
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
                        next.ImageKey = "Group";
                        next.SelectedImageKey = "Group";
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

            //string tooltip = string.Format("{0}\r\nRequirement Level: {1} \r\nState: {2}",
            //    testName, requirementLevel, state);

            string tooltip = string.Format("{0}\r\n{1} \r\nState: {2}",
                testName, requirementLevel, state);


            return tooltip;
        }

        #endregion

        private bool _certificationTestsSelected = false;
        public void SelectTests(IEnumerable<TestInfo> testInfos)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {

                        _certificationTestsSelected = true;
                        foreach (TestInfo info in _testNodes.Keys)
                        {
                            _testNodes[info].Checked = testInfos.Contains(info);
                        }

                        SelectTestGroups();
                        _certificationTestsSelected = false;
                    }));
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
            while (i < orderedGroups.Count)
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
            for (int j = orderedGroups.Count - 1; j >= 0; j--)
            {
                CheckIfAllChildrenChecked(orderedGroups[j]);
            }
        }

        public void ClearTestResults()
        {
            foreach (TreeNode node in _colouredNodes)
            {
                node.ForeColor = tvTestCases.ForeColor;
                node.NodeFont = tvTestCases.Font;
                node.ToolTipText = CreateTestTooltip((TestInfo)node.Tag, null);
            }
        }

        private bool _bProfileBeingApplied;

        public void ApplyProfileOptions(Profile profile)
        {
            _bProfileBeingApplied = true;

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
        /// Selects tests which currently is being executed.
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        public void HighlightActiveTest(TestInfo testInfo)
        {
            if (_testNodes.ContainsKey(testInfo))
            {
                TreeNode node = _testNodes[testInfo];
                tvTestCases.SelectedNode = node;
            }
        }

        /// <summary>
        /// Highlightes completed test.
        /// </summary>
        /// <param name="testResult">Test results.</param>
        public void HighlightCompletedTest(TestResult testResult)
        {
            if (_testNodes.ContainsKey(testResult.TestInfo))
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
        }

        /// <summary>
        /// Cancels test selection if forbidden (test is running, certification mode)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTestCases_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // tests are selected programmatically
            if (_certificationTestsSelected)
            {
                return;
            }

            // parent control checks if tests are running
            if (BeforeCheck != null)
            {
                BeforeCheck(this, e);
            }
            if (e.Cancel)
            {
                return;
            }

            // check if test can be selected/unselected
            if (_certificationMode)
            {
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

            if (AfterCheck != null)
            {
                AfterCheck(this, e);
            }
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
                    TestInfo testInfo = (TestInfo)child.Tag;
                    child.Checked = node.Checked;

                    /*
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
                    }*/
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


        private void tvTestCases_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (AfterSelect != null)
            {
                AfterSelect(this, e);
            }
        }

        private void tvTestCases_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (BeforeSelect != null)
            {
                BeforeSelect(this, e);
            }
        }

        void tvTestCases_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (_inactive)
            {
                if (e.Node == tvTestCases.SelectedNode && _inactive)
                {
                    if (AfterSelect != null)
                    {
                        _inactive = false;
                        AfterSelect(this, new TreeViewEventArgs(e.Node));
                    }
                }
            }
        }

        private bool _inactive;
        public void SetInactive()
        {
            _inactive = true;
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
