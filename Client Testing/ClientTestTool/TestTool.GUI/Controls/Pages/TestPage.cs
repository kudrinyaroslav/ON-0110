using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.Device.Data;
using TestTool.GUI.Controllers;

namespace TestTool.GUI.Controls.Pages
{
    internal partial class TestPage : BasePage, Views.ITestView
    {
        private Controllers.TestController _controller;

        public TestPage()
        {
            InitializeComponent();

            _controller = new TestController(this);
        }


        private void tsbStart_Click(object sender, EventArgs e)
        {
            TreeNode node = tvConfigurations.SelectedNode;
            Data.ConfigurationFactory factory = node.Tag as Data.ConfigurationFactory;

            _controller.Start(factory);

        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            _controller.Stop();
        }




        #region IView Members

        public IController GetController()
        {
            return _controller;
        }
        
        #endregion

        #region IDiagnosticView Members

        public void LogSimulatorEvent(string message)
        {
            Invoke(new Action(()=> { lvLog.LogEvent(message); }));
        }

        public void Clear()
        {
            Invoke(new Action(() =>
                                  {
                                      lvLog.Clear();
                                      foreach(TreeNode serviceNode in tvOperations.Nodes)
                                      {
                                          foreach (TreeNode operationNode in serviceNode.Nodes )
                                          {
                                              operationNode.ImageKey = "Undefined";
                                              operationNode.SelectedImageKey = "Undefined";
                                          }
                                      }
                                  }));
        }

        public void DisplayRequestProcessingLog(TestTool.Services.RequestProcessingLog log)
        {
            Invoke(new Action(() =>
                      {
                          lvLog.DisplayRequestProcessingLog(log);
                      }));
        }

        Dictionary<string, Dictionary<string, TreeNode>> _operationNodes = new Dictionary<string, Dictionary<string, TreeNode>>();

        public void ShowServiceInformation(List<ServiceContractInfo> infos)
        {
            foreach (ServiceContractInfo info in infos)
            {
                TreeNode node = new TreeNode(info.ServiceName);
                node.ImageKey = "Service";
                node.SelectedImageKey = "Service";
                tvOperations.Nodes.Add(node);

                _operationNodes.Add(info.ServiceName, new Dictionary<string, TreeNode>());

                foreach (string operation in info.OperationsList)
                {
                    TreeNode operationNode = new TreeNode(operation);
                    operationNode.ImageKey = "Undefined";
                    operationNode.SelectedImageKey = "Undefined";
                    node.Nodes.Add(operationNode);

                    _operationNodes[info.ServiceName].Add(operation, operationNode);
                }

                node.Expand();
            }
        }

        public void SwitchToWorkingState()
        {
            tsbStop.Enabled = true;
            tsbSave.Enabled = false;
            tsbClear.Enabled = false;
            tsbStart.Enabled = false;
        }

        public void SwitchToIdleState()
        {
            tsbStop.Enabled = false;
            tsbSave.Enabled = true;
            tsbClear.Enabled = true;
            tsbStart.Enabled = true;
        }

        public void DisplayConfigurations(IEnumerable<Data.ConfigurationFactory> configurations)
        {
            tvConfigurations.Nodes.Clear();
            
            foreach (Data.ConfigurationFactory factory in configurations)
            {
                // Find group node to add this test to.
                TreeNode groupNode = FindGroupNode(factory.Path);

                // Add node
                TreeNode node = groupNode.Nodes.Add(factory.Name);
                node.SelectedImageKey = node.ImageKey = "Configuration";
                node.Tag = factory;

            }

            tvConfigurations.ExpandAll();
        }

        public void UpdateOperationUsage(string serviceName, string operationName, Data.OperationUsage usage)
        {
            TreeNode node = _operationNodes[serviceName][operationName];
            string imageKey = string.Empty;
            switch (usage)
            {
                case Data.OperationUsage.NotCovered:
                    imageKey = "Undefined";
                    break;
                case Data.OperationUsage.Failed:
                    imageKey = "NotSupported";
                    break;
                case Data.OperationUsage.Passed:
                    imageKey = "Supported";
                    break;
            }

            node.ImageKey = imageKey;
            node.SelectedImageKey = imageKey;
        }

        #endregion


        internal TestController Controller
        {
            get { return _controller; }
        }

        private void tvOperations_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lvLog.ResetSelection();
        }

        //private void tvOperations_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    TreeNode node = e.Node;
        //    if (node != null)
        //    {
        //        if (node.Parent != null)
        //        {
        //            lvLog.SelectByMethod(node.Parent.Text, node.Text);
        //        }
        //        else
        //        {
        //            lvLog.SelectByService(node.Text);
        //        }           
        //    }
        //    else
        //    {
        //        lvLog.ResetSelection();
        //    }
        //}

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
            foreach (TreeNode root in tvConfigurations.Nodes)
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
                rootNode = tvConfigurations.Nodes.Add(rootName);
                rootNode.Name = rootName;
                rootNode.ImageKey = "Group";
                rootNode.SelectedImageKey = "Group";
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

                        next.Name = group;
                    }
                    current = next;
                }

                return current;
            }

        }

        private void tvConfigurations_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tsbStart.Enabled = tvConfigurations.SelectedNode != null && tvConfigurations.SelectedNode.Tag != null;
        }

        private void tvConfigurations_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_controller.Running)
            {
                e.Cancel = true;
            }
        }

        private void cmsConfigurationsTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool enabled = tvConfigurations.SelectedNode != null && tvConfigurations.SelectedNode.Tag != null;
            //if (_controller.Running && enabled)
            //{
            //    Data.ConfigurationFactory factory = tvConfigurations.SelectedNode.Tag as Data.ConfigurationFactory;
            //    enabled = _controller.CurrentConfig == factory.Id;
            //}

            showToolStripMenuItem.Enabled = enabled; 
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.ConfigurationFactory factory = tvConfigurations.SelectedNode.Tag as Data.ConfigurationFactory;
            Common.Configuration.SimulatorConfiguration configuration = _controller.GetConfiguration(factory.Id);

            ConfigurationView view = new ConfigurationView();
            view.Text = "Configuration " + factory.Name;
            view.Display(configuration);
            view.Show(this.FindForm());
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under Construction!");
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            lvLog.Clear();
        }

        private void tvOperations_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvOperations.SelectedNode = e.Node;

                if (e.Node != null)
                {
                    if (e.Node.Parent != null)
                    {
                        lvLog.SelectByMethod(e.Node.Parent.Text, e.Node.Text);
                    }
                    else
                    {
                        lvLog.SelectByService(e.Node.Text);
                    }
                }
                else
                {
                    lvLog.ResetSelection();
                }
            }
        }

    }
}
