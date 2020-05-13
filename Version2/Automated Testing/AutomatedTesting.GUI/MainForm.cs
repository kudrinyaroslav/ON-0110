using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutomatedTesting.GUI.ExternalData;
using AutomatedTesting.GUI.Data;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Trace;
using System.Xml;
using AutomatedTesting.GUI.Controllers;

namespace AutomatedTesting.GUI
{
    public partial class MainForm : Form, Controllers.ITestView
    {
        public MainForm()
        {
            InitializeComponent();

            testsTree.NodeSelected += new TreeViewEventHandler(testsTree_NodeSelected);
            testsTree.TestSuiteRemoved += new Action<TestSuite>(testsTree_TestSuiteRemoved);
            testsTree.DutTestSuiteRemoved += new Action<DutTest>(testsTree_DutTestSuiteRemoved);
        }

        void testsTree_DutTestSuiteRemoved(DutTest dt)
        {
            _testController.DutTests.Remove(dt);
        }

        void testsTree_TestSuiteRemoved(TestSuite ts)
        {
            _testController.TestSuites.Remove(ts);
        }

        Controllers.TestController _testController;

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _testController = new AutomatedTesting.GUI.Controllers.TestController(this);
                _testController.Initialize();
                
                Controllers.StateController ctrl = new AutomatedTesting.GUI.Controllers.StateController();
                ctrl.LoadState();

                foreach (string fileName in Context.AppContext.Instance.TreeState.ParametersFiles)
                {
                    TestParameters tp = TestParameters.Load(fileName);
                    if (tp != null)
                    {
                        testsTree.AddParametersSet(fileName, tp);
                    }
                }                
                foreach (string fileName in Context.AppContext.Instance.TreeState.FeatureDefinitionFiles)
                {
                    FeatureDefinition def = FeatureDefinition.Load(fileName);
                    if (def != null)
                    {
                        testsTree.AddFeatureDefinitionNode(fileName, def);
                    }
                }                
                foreach (string fileName in Context.AppContext.Instance.TreeState.TestFiles)
                {
                    TestSuite ts = TestSuite.Load(fileName);
                    if (ts != null)
                    {
                        testsTree.AddTestSuite(fileName, ts);
                        _testController.TestSuites.Add(ts);
                    }
                }
                foreach (string fileName in Context.AppContext.Instance.TreeState.DutTestFiles)
                {
                    DutTest ts = DutTest.Load(fileName);
                    if (ts != null)
                    {
                        testsTree.AddTestSuite(fileName, ts);
                        _testController.DutTests.Add(ts);
                    }
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {                        
            try
            {
                Controllers.StateController ctrl = new AutomatedTesting.GUI.Controllers.StateController();
                ctrl.SaveState();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void tsbLoadTests_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = ofd.FileName;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);

                    switch (doc.DocumentElement.LocalName)
                    { 
                        case FeatureDefinition.ROOT:
                            FeatureDefinition def = FeatureDefinition.Load(fileName);
                            testsTree.AddFeatureDefinitionNode(fileName, def);
                            Context.AppContext.Instance.TreeState.FeatureDefinitionFiles.Add(fileName);                            
                            break;
                        case TestSuite.ROOT:
                            TestSuite ts = TestSuite.Load(fileName);
                            testsTree.AddTestSuite(fileName, ts);
                            Context.AppContext.Instance.TreeState.TestFiles.Add(fileName);
                            break;
                        case TestParameters.ROOT:
                            TestParameters parameters = TestParameters.Load(fileName);
                            testsTree.AddParametersSet(fileName, parameters);
                            Context.AppContext.Instance.TreeState.ParametersFiles.Add(fileName);
                            break;
                        case DutTest.ROOT:
                            DutTest test = DutTest.Load(fileName);
                            testsTree.AddTestSuite(fileName, test);
                            Context.AppContext.Instance.TreeState.DutTestFiles.Add(fileName);
                            break;

                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }
        
        private void tsbRun_Click(object sender, EventArgs e)
        {
            Task task = null;
            try
            {
                task = testsTree.GetTask();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            if (task.FeatureDefnitionSettings == null && !_testController.FeaturesDefined)
            { 
                // if dispatcher has no features...
                MessageBox.Show("No feature definition added to task, and no features are defined yet");
                return;
            }

            try
            {
                _testController.RunTests(task);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        #region ITestView

        public void BeginTesting()
        {
            EnableControls(false);
        }

        public void EndTesting()
        {
            EnableControls(true);        
        }

        public void WriteLine(string message)
        {
            testResultsView.WriteLine(message);
        }

        public void BeginTest(TestInfo testInfo, string testCaseId)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    testResultsView.ClearTestInfo();
                    testsTree.SelectTestCase(testCaseId); 
                }));
            }
            else
            {
                testResultsView.ClearTestInfo();
                testsTree.SelectTestCase(testCaseId);
            }
        }

        public void EndTest(TestResult testResult)
        { 
        
        }

        public void DisplayStepResult(StepResult result)
        {
            testResultsView.DisplayStepResult(result);
        }

        public void DisplayTestResult(string caseId, TestCaseStatus status)
        {
            BeginInvoke(new Action(() => { testsTree.DisplayTestResult(caseId, status); }));
        }


        #endregion
                
        void EnableControls(bool enable)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Enable form: {0}", enable));
            Invoke(new Action(
                () => {
                    tsbLoad.Enabled = enable;
                    tsbRun.Enabled = enable;
                    tsbStop.Enabled = !enable;
                    testsTree.ReadOnly = !enable;
                }));            
        }


        void testsTree_NodeSelected(object sender, TreeViewEventArgs e)
        {
            TestCase tc = e.Node.Tag as TestCase;
            DutTestCase dt = e.Node.Tag as DutTestCase;
            testResultsView.ClearTestInfo();

            if (tc != null)
            {
                TestResult result = _testController.GetTestResult(tc.TestCaseID);
                if (result != null)
                {
                    testResultsView.DisplayTestResults(result);
                }
            }
            else if (dt != null)
            {
                TestResult result = _testController.GetTestResult(dt.TestCaseID);
                if (result != null)
                {
                    testResultsView.DisplayTestResults(result);
                }
            }
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            _testController.Stop();
        }

        private void tsbExportFailed_Click(object sender, EventArgs e)
        {
            try
            {
                string defaultName = string.Format("Failed {0}.xml", System.DateTime.Now.ToString("yyyy.MM.dd HH-mm"));

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = defaultName;
                
                sfd.AddExtension = true;
                sfd.DefaultExt = ".xml";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _testController.ExportFailedTests(sfd.FileName);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }


    }
}
