using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutomatedTesting.GUI.Data;
using AutomatedTesting.GUI.ExternalData;
using AutomatedTesting.GUI.Controllers;

namespace AutomatedTesting.GUI.Controls
{
    public partial class TestsTree : UserControl
    {
        public TestsTree()
        {
            InitializeComponent();

            GetNamedRoot(PARAMETERSROOT);
            GetNamedRoot(FEATUREDEFINITIONROOT);
            GetNamedRoot(TESTSROOT);

            _parametersNodes = new Dictionary<TreeNode, AutomatedTesting.GUI.ExternalData.TestParameters>();
            _testCaseNodes = new Dictionary<TreeNode, AutomatedTesting.GUI.ExternalData.TestCase>();
            _featureDefinitionNodes = new Dictionary<TreeNode, TestCase>();
            _dutTestCaseNodes = new Dictionary<TreeNode, DutTestCase>();

            _tcNodes = new Dictionary<string, TreeNode>();

            _fileNodes = new Dictionary<TreeNode, string>();
        }

        /// <summary>
        ///  Parameters nodes && corresponding Parameters sets (for creating Task)
        /// </summary>
        Dictionary<TreeNode, ExternalData.TestParameters> _parametersNodes;
        /// <summary>
        /// Nodes representing test cases (for creating Task)
        /// </summary>
        Dictionary<TreeNode, ExternalData.TestCase> _testCaseNodes;

        /// <summary>
        /// Nodes representing test cases (for creating Task)
        /// </summary>
        Dictionary<TreeNode, ExternalData.DutTestCase> _dutTestCaseNodes;

        /// <summary>
        /// Nodes representing feature definitions test cases (for creating Task)
        /// </summary>
        Dictionary<TreeNode, ExternalData.TestCase> _featureDefinitionNodes;

        /// <summary>
        /// Dictionary to provide access to test cases by IDs
        /// </summary>
        Dictionary<string, TreeNode> _tcNodes = new Dictionary<string, TreeNode>(); 

        /// <summary>
        /// Dictionary to store nodes representing files and file's full names
        /// </summary>
        Dictionary<TreeNode, string> _fileNodes;

        const string TESTSROOT = "Tests";
        const string PARAMETERSROOT = "Parameters";
        const string FEATUREDEFINITIONROOT = "FeatureDefinition";

        public void AddTestSuite(string fileName, ExternalData.TestSuite testSuite)
        {
            TreeNode root = GetTestSuiteRoot();
            
            TreeNode tsNode = root.Nodes.Add(System.IO.Path.GetFileName(fileName));
            tsNode.Tag = testSuite;
            _fileNodes.Add(tsNode, fileName);

            foreach (ExternalData.Test test in testSuite.Tests)
            {
                string testName = string.Empty;
                testName = string.Format("{0}-{1}", test.Category, test.TestID);

                TreeNode testNode = tsNode.Nodes.Add(testName, testName);
                testNode.Tag = test;

                foreach (ExternalData.TestCase tc in test.TestCases)
                {
                    string tcName = string.Format("{0}.{1}", testName, tc.TestCaseID);
                    string displayName = tc.TestCaseID;
                    TreeNode tcNode = testNode.Nodes.Add(tcName, displayName);
                    tcNode.Tag = tc; 

                    _testCaseNodes.Add(tcNode, tc);
                    _tcNodes.Add(tc.TestCaseID, tcNode);
                }
            }

            tsNode.ExpandAll();            
            
            root.Expand();
        }

        public void AddTestSuite(string fileName, ExternalData.DutTest testSuite)
        {
            TreeNode root = GetTestSuiteRoot();

            TreeNode tsNode = root.Nodes.Add(System.IO.Path.GetFileName(fileName));
            tsNode.Tag = testSuite;
            _fileNodes.Add(tsNode, fileName);

            foreach (ExternalData.DutTestCase test in testSuite.Tests)
            {
                string testName = string.Empty;
                testName = test.TestCaseID;

                TreeNode testNode = tsNode.Nodes.Add(testName, testName);
                testNode.Tag = test;
                testNode.ToolTipText = test.Name;

                _dutTestCaseNodes.Add(testNode, test);
                _tcNodes.Add(test.TestCaseID, testNode);

            }

            tsNode.ExpandAll();

            root.Expand();
        }

        public void AddParametersSet(string fileName, ExternalData.TestParameters parameters)
        {
            TreeNode root = GetParametersRoot();

            TreeNode parametersNode = root.Nodes.Add(fileName, System.IO.Path.GetFileName(fileName));
            parametersNode.Tag = parameters;

            _parametersNodes.Add(parametersNode, parameters);
            
            _fileNodes.Add(parametersNode, fileName);
            
            root.Expand();
        }

        public void AddFeatureDefinitionNode(string fileName, ExternalData.FeatureDefinition featureDefinition)
        {
            TreeNode node = GetFeatureDefinitionRoot();

            TreeNode tsNode = node.Nodes.Add(System.IO.Path.GetFileName(fileName));
            tsNode.Tag = featureDefinition;
            _fileNodes.Add(tsNode, fileName);

            foreach (ExternalData.TestCase tc in featureDefinition.TestCases)
            {
                string tcName = "Features." + tc.TestCaseID;
                TreeNode tcNode = tsNode.Nodes.Add(tcName, tc.TestCaseID);
                tcNode.Tag = tc;
                _featureDefinitionNodes.Add(tcNode, tc);

                _tcNodes.Add(tc.TestCaseID, tcNode);

            }

            tsNode.ExpandAll();            
            node.Expand();
        }

        internal Task GetTask()
        {
            Task task = new Task();

            foreach (TreeNode node in _testCaseNodes.Keys)
            {
                if (node.Checked)
                {
                    TreeNode testNode = node.Parent;
                    TestCase tc = _testCaseNodes[node];
                    Test test = (Test)testNode.Tag;
                                            
                    TestCaseSettings settings = new TestCaseSettings();

                    if (string.IsNullOrEmpty(tc.FileName))
                    {
                        settings.FileName = test.DefaultFileName;
                    }
                    else 
                    {
                        settings.FileName = tc.FileName;
                    }
                    settings.TestCaseID = tc.TestCaseID;

                    settings.Category = test.Category;
                    settings.TestID = test.TestID;
                                            
                    task.Tests.Add(settings);
                }
            }
            foreach (TreeNode node in _dutTestCaseNodes.Keys)
            {
                if (node.Checked)
                {
                    DutTestCase tc = _dutTestCaseNodes[node];
                    TreeNode testNode = node.Parent;
                    DutTest test = (DutTest)testNode.Tag;

                    TestCaseSettings settings = new TestCaseSettings();

                    settings.FileName = test.FileName;
                    settings.Category = test.Category;
                    settings.TestID = test.TestID;
                    settings.TestCaseID = tc.TestCaseID;

                    task.Tests.Add(settings);
                }
            }
            foreach (TreeNode node in _featureDefinitionNodes.Keys)
            {
                if (node.Checked)
                {
                    FeatureDefinition def = (FeatureDefinition)(node.Parent.Tag);
                    TestCase tc = _featureDefinitionNodes[node];
                    TestCaseSettings settings = new TestCaseSettings();
                    settings.TestCaseID = tc.TestCaseID;

                    if (string.IsNullOrEmpty(tc.FileName))
                    {
                        settings.FileName = def.DefaultFileName;
                    }
                    else
                    {
                        settings.FileName = tc.FileName;
                    }

                    if (task.FeatureDefnitionSettings != null)
                    {
                        throw new ApplicationException("More that one node of type 'FeatureDefinition' selected");
                    }
                    else
                    {
                        task.FeatureDefnitionSettings = settings;
                    }
                }
            }

            foreach (TreeNode node in _parametersNodes.Keys)
            {
                if (node.Checked)
                {
                    if (task.Parameters != null)
                    {
                        throw new ApplicationException("More that one node of type 'Parameters' selected");
                    }
                    else 
                    {
                        task.Parameters = _parametersNodes[node];
                    }
                }
            }

            if (task.Parameters == null)
            {
                throw new ApplicationException("No node of type 'Parameters' selected");
            }

            return task;
        }
                            
        void Remove(TreeNode node)
        {
            TreeNode current = node;
            TreeNode fileNode = null;
            while (current.Parent != null)
            {
                fileNode = current;
                current = current.Parent;
            }

            current.Nodes.Remove(fileNode);
            // remove from AppState

            string fileName = _fileNodes[fileNode];

            Context.TreeState state = Context.AppContext.Instance.TreeState;

            switch (current.Name)
            {
                case PARAMETERSROOT:
                    state.ParametersFiles.Remove(fileName);
                    _parametersNodes.Remove(fileNode);
                    break;
                case TESTSROOT:
                    state.TestFiles.Remove(fileName);
                    state.DutTestFiles.Remove(fileName);
                    TestSuite ts = fileNode.Tag as TestSuite;
                    if (ts != null)
                    {
                        if (TestSuiteRemoved != null)
                        {
                            TestSuiteRemoved(ts);
                        }
                        foreach (Test test in ts.Tests)
                        {
                            foreach (TestCase tc in test.TestCases)
                            {
                                TreeNode tcNode = _tcNodes[tc.TestCaseID];
                                _tcNodes.Remove(tc.TestCaseID);
                                _testCaseNodes.Remove(tcNode);
                            }
                        }
                    }
                    else
                    {
                        DutTest dt = fileNode.Tag as DutTest;
                        if (DutTestSuiteRemoved != null)
                        {
                            DutTestSuiteRemoved(dt);
                        }

                        foreach (DutTestCase tc in dt.Tests)
                        {
                            TreeNode tcNode = _tcNodes[tc.TestCaseID];
                            _tcNodes.Remove(tc.TestCaseID);
                            _dutTestCaseNodes.Remove(tcNode);
                        }

                    }
                    break;
                case FEATUREDEFINITIONROOT:
                    state.FeatureDefinitionFiles.Remove(fileName);
                    
                    FeatureDefinition def = (FeatureDefinition)fileNode.Tag;

                    foreach (TestCase tc in def.TestCases)
                    {
                        TreeNode tcNode = _tcNodes[tc.TestCaseID];
                        _tcNodes.Remove(tc.TestCaseID);
                        _featureDefinitionNodes.Remove(tcNode);
                    }
                    break;
            }
            _fileNodes.Remove(fileNode);  
        
        }

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
        /// Handles logic for nodes selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvTests_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;

            if (!_bubbleCheck)
            {
                // else select all child nodes.
                foreach (TreeNode child in node.Nodes)
                {
                    child.Checked = node.Checked;
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
        }

        public event Action<TestSuite> TestSuiteRemoved;
        public event Action<DutTest> DutTestSuiteRemoved;

        #region Root nodes

        TreeNode GetTestSuiteRoot()
        {
            return GetNamedRoot(TESTSROOT);
        }

        TreeNode GetParametersRoot()
        {
            return GetNamedRoot(PARAMETERSROOT);
        }

        TreeNode GetFeatureDefinitionRoot()
        {
            return GetNamedRoot(FEATUREDEFINITIONROOT);
        }

        TreeNode GetNamedRoot(string name)
        {
            if (tvTests.Nodes.ContainsKey(name))
            {
                return tvTests.Nodes[name];
            }
            else
            {
                TreeNode node = tvTests.Nodes.Add(name, name);
                return node;
            }
        }


        #endregion

        bool _readOnly;
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public void SelectTestCase(string tcId)
        {
            if (_tcNodes.ContainsKey(tcId))
            {
                tvTests.SelectedNode = _tcNodes[tcId];
            }        
        }

        public void DisplayTestResult(string caseId, TestCaseStatus status)
        {
            Color color = System.Drawing.Color.Black;
            switch (status)
            {
                case TestCaseStatus.Green:
                    color = Color.Green;
                    break;
                case TestCaseStatus.Red:
                    color = Color.Red;
                    break;
                case TestCaseStatus.Yellow:
                    color = Color.DarkOrchid;
                    break;            
            }

            if (_tcNodes.ContainsKey(caseId))
            {
                _tcNodes[caseId].ForeColor = color;
            }
        }

        #region Node Selection

        private void tvTests_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (NodeSelected != null)
            {
                NodeSelected(this, e);
            }
        }

        public event TreeViewEventHandler NodeSelected;

        #endregion

        private void cmsTreeMenu_Opening(object sender, CancelEventArgs e)
        {
            if (ReadOnly)
            {
                removeToolStripMenuItem.Enabled = false;
                selectAllToolStripMenuItem.Enabled = false;
                unselectTimeoutsToolStripMenuItem.Enabled = false;
                clearAllToolStripMenuItem.Enabled = false;
            }
            else
            {
                TreeNode node = tvTests.SelectedNode;
                if (node != null)
                {
                    removeToolStripMenuItem.Enabled = node.Parent != null;
                }
            }

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = tvTests.SelectedNode;
                if (node != null)
                {
                    Remove(node);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectSubTree(true);
        }
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectSubTree(false);
        }

        void SelectSubTree(bool select)
        { 
            TreeNode node = tvTests.SelectedNode;
            if (node != null)
            {
                SelectChildNodes(node, select);
            }        
        }

        void SelectChildNodes(TreeNode node, bool select)
        {
            node.Checked = select;
            foreach (TreeNode child in node.Nodes)
            {
                SelectChildNodes(child, select);
            }        
        }

        private void tvTests_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("tvTests_BeforeCheck, ReadOnly: {0}", _readOnly));
                e.Cancel = _readOnly;
            }
        }

        private void tvTests_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("tvTests_BeforeSelect, ReadOnly: {0}", _readOnly));
                e.Cancel = _readOnly;
            }
        }

        private void unselectTimeoutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectTimeoutsInSubstree(false);
        }

        void SelectTimeoutsInSubstree(bool select)
        {
            TreeNode node = tvTests.SelectedNode;
            if (node != null)
            {
                SelectTimeoutsInSubstree(node, select);
            }
        }

        void SelectTimeoutsInSubstree(TreeNode node, bool select)
        {
            TestCase tc = node.Tag as TestCase;
            if (tc != null && tc.Timeout)
            {
                node.Checked = select;
            }
            foreach (TreeNode child in node.Nodes)
            {
                SelectTimeoutsInSubstree(child, select);
            }
        }

  
    }
}
