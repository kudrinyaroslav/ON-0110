using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CameraClient.Tests;
using CameraClient.Log;
using System.Reflection;

namespace CameraClient
{
    partial class MainForm
    {
        List<TestInfo> _testInfos = new List<TestInfo>();
        List<TestInfo> _selectedTests = new List<TestInfo>();

        void InitTestsTree()
        {

            Type t = typeof(Tests.TestSuite);

            foreach (MethodInfo mi in t.GetMethods())
            {
                object[] attributes = mi.GetCustomAttributes(typeof(Tests.TestAttribute), true);

                if (attributes.Length > 0)
                {
                    TestAttribute attribute = (TestAttribute)attributes[0];

                    TestInfo testInfo = new TestInfo();
                    testInfo.Method = mi;
                    testInfo.Name = attribute.Name;
                    testInfo.Group = attribute.Path;

                    _testInfos.Add(testInfo);

                }
            }

            foreach (TestInfo testInfo in _testInfos.OrderBy(ti => ti.Group).ThenBy(ti => ti.Name))
            {
                TreeNode groupNode = FindGroupNode(testInfo.Group);
                TreeNode node = groupNode.Nodes.Add(testInfo.Name);
                node.Tag = testInfo;
            }

        }

        TreeNode FindGroupNode(string path)
        {
            string[] parts = path.Split('\\');
            string rootName = parts[0];

            TreeNode rootNode = null;

            foreach (TreeNode root in treeViewTests.Nodes)
            {
                if (root.Name == rootName)
                {
                    rootNode = root;
                    break;
                }
            }

            if (rootNode == null)
            {
                rootNode = treeViewTests.Nodes.Add(rootName);
                rootNode.Name = rootName;
            }

            if (parts.Length == 1)
            {
                return rootNode;
            }
            else
            {
                TreeNode current = rootNode;

                for (int i = 1; i < parts.Length; i++)
                {
                    string group = parts[i];
                    TreeNode next = null;
                    foreach (TreeNode node in current.Nodes)
                    {
                        if (node.Name == group)
                        {
                            next = node;
                            break;
                        }
                    }
                    if (next == null)
                    {
                        next = current.Nodes.Add(group);
                        next.Name = group;
                    }

                    current = next;
                }

                return current;
            }

        }

        private TestDispatcher _td;
        private void btnDiscoveryTest_Click(object sender, EventArgs e)
        {
            listViewTrace.Items.Clear();
            richTextBoxStepRequest.Text = string.Empty;
            richTextBoxStepAnswer.Text = string.Empty;
            richTextBoxException.Text = string.Empty;
            
            btnDiscoveryTest.Enabled = false;
            btnStop.Enabled = true;
            btnPause.Enabled = true;
            btnHalt.Enabled = true;
            _testIsRunning = true;

            TestSuiteParameters parameters = new TestSuiteParameters();
            foreach (TestInfo ti in _selectedTests)
            {
                parameters.TestCases.Add(ti.Method);
            }
            parameters.Address = tbAddress.Text;


            try
            {
                _td = new TestDispatcher();
                _td.OnException += new Action<Exception>(_tr_OnException);
                _td.OnStepCompleted += ts_OnStepCompleted;
                _td.OnTestSuiteCompleted += new Action(_tr_OnTestSuiteCompleted);
                _td.Run(parameters);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        
        void ts_OnStepCompleted(StepResult result)
        {
            try
            {
                BeginInvoke(new DisplayStepDelegate(DisplayStep), new object[] {result});
            }
            catch (Exception )
            {
            }
        }

        void _tr_OnException(Exception obj)
        {
            try
            {
                BeginInvoke(new Action( () => ShowStatusMessage(obj.Message)));
            }
            catch (Exception)
            {
                
            }
        }

        void _tr_OnTestSuiteCompleted()
        {
            try
            {
            BeginInvoke(new Action(() => 
            { 
                btnDiscoveryTest.Enabled = true;
                btnStop.Enabled = false;
                btnPause.Enabled = false;
                btnHalt.Enabled = false;
                _testIsRunning = false;
                ShowStatusMessage("Done"); 
            }));
            }
            catch (Exception)
            {

            }

        }

        public delegate void DisplayStepDelegate(StepResult result);

        void DisplayStep(StepResult result)
        {
            ListViewItem stepItem = new ListViewItem(result.Number.ToString());
            stepItem.Tag = result;
            stepItem.SubItems.Add(result.StepName);
            stepItem.SubItems.Add(result.Status.ToString());
            stepItem.SubItems.Add(result.Message);

            listViewTrace.Items.Add(stepItem);
        }

        private void listViewTrace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewTrace.SelectedItems.Count > 0)
            {
                ListViewItem item = listViewTrace.SelectedItems[0];
                StepResult step = (StepResult)item.Tag;
                richTextBoxStepRequest.Text = step.Request;
                richTextBoxStepAnswer.Text = step.Response;
                if (step.Exception != null)
                {
                    richTextBoxException.Text = step.Exception.ToString();
                }
                else
                {
                    richTextBoxException.Text = string.Empty;
                }
            }
        }

        private void treeViewTests_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node.Tag == null)
            {
                return;
            }
            TestInfo testInfo = (TestInfo)node.Tag;
            if (node.Checked)
            {
                _selectedTests.Add(testInfo);
            }
            else
            {
                _selectedTests.Remove(testInfo);
            }

            btnDiscoveryTest.Enabled = _selectedTests.Count > 0;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_td != null)
            {
                _td.Stop();

            }
        }
        
        private void btnHalt_Click(object sender, EventArgs e)
        {
            _td.Halt();
        }

        private bool _testIsRunning;
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (_testIsRunning)
            {
                _testIsRunning = false;
                _td.Pause();
                btnPause.Text = "Continue";
            }
            else
            {
                _testIsRunning = true;
                _td.Resume();
                btnPause.Text = "Pause";
            }
        }

    }
}
