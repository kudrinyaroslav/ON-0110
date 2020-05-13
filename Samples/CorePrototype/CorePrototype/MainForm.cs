using System;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace TestEngineProto
{
    public partial class MainForm : Form
    {
        private Assembly _asm;

        private IListener _listener;

        private Common.IPrimitivesCollection _primitivesCollection ;

        private List<TestInfo> _tests;

        public MainForm()
        {
            InitializeComponent();
            _listener = new TestBoxListener(txtConsole);
            _primitivesCollection = new PrimitivesCollection(_listener);

            _tests = new List<TestInfo>();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "DLL | *.dll";
            
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string assemblyName = ofd.FileName;
                CreateTree(assemblyName);
            }
            
        }

        private  void CreateTree(string fileName)
        {
            try
            {
                _asm = Assembly.LoadFile(fileName);
                listBoxAssemblies.Items.Add(fileName);

                List<TestInfo> testInfos = new List<TestInfo>();

                foreach (Type t in _asm.GetTypes())
                {
                    object[] attrs = t.GetCustomAttributes(typeof (Common.TestSuiteAttribute), true);
                    if (attrs.Length > 0)
                    {
                        if (t.IsSubclassOf(typeof (Common.TestsCollectionBase)))
                        {
                            foreach (MethodInfo mi in t.GetMethods())
                            {
                                object[] categories = mi.GetCustomAttributes(typeof (Common.CategoryAttribute), true);

                                if (categories.Length > 0)
                                {
                                    TestInfo testInfo = new TestInfo();
                                    testInfo.Method = mi;

                                    object[] path = mi.GetCustomAttributes(typeof (Common.GroupAttribute), true);
                                    if (path.Length > 0)
                                    {
                                        Common.GroupAttribute groupAttribute = (Common.GroupAttribute) path[0];
                                        testInfo.Group = groupAttribute.Path;
                                    }
                                    else
                                    {
                                        testInfo.Group = "Default Group";
                                    }

                                    object[] name = mi.GetCustomAttributes(typeof (Common.NameAttribute), true);
                                    if (name.Length > 0)
                                    {
                                        Common.NameAttribute nameAttribute = (Common.NameAttribute) name[0];
                                        testInfo.Name = nameAttribute.Name;
                                    }
                                    else
                                    {
                                        testInfo.Name = mi.Name;
                                    }

                                    
                                    _tests.Add(testInfo);

                                    testInfos.Add(testInfo);

                                }

                            }
                        }
                    }
                }

                foreach (TestInfo testInfo in testInfos.OrderBy( ti => ti.Group).ThenBy(ti=>ti.Name))
                {
                    TreeNode groupNode = FindGroupNode(testInfo.Group);
                    TreeNode node = groupNode.Nodes.Add(testInfo.Name);
                    node.Tag = testInfo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Unable to load assembly: {0}", ex.Message));
            }
        }

        TreeNode FindGroupNode(string path)
        {
            string[] parts = path.Split('\\');
            string rootName = parts[0];

            TreeNode rootNode = null;
            
            foreach (TreeNode root in treeTests.Nodes)
            {
                if (root.Name == rootName)
                {
                    rootNode = root;
                    break;
                }
            }

            if (rootNode == null)
            {
                rootNode = treeTests.Nodes.Add(rootName);
                rootNode.Name = rootName;
            }

            if (parts.Length == 1)
            {
                return rootNode;
            }
            else
            {
                TreeNode current = rootNode;

                for (int i =1 ; i< parts.Length; i++)
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

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode selectedTest = treeTests.SelectedNode;
                if (selectedTest != null)
                {
                    TestInfo testInfo = (TestInfo) selectedTest.Tag;
                    MethodInfo mi = testInfo.Method ;

                    if (mi != null)
                    {

                        object[] Args = {_primitivesCollection};

                        Type[] types = new Type[1];
                        types[0] = typeof (Common.IPrimitivesCollection);

                        Type t = mi.DeclaringType;

                        System.Reflection.ConstructorInfo ci
                            =
                            t.GetConstructor(
                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                                null, System.Reflection.CallingConventions.HasThis, types, null);
                        object itObject = ci.Invoke(Args);

                        mi.Invoke(itObject, new object[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                Exception exc = ex.InnerException ?? ex;
                _listener.Write(string.Format("Exception: {0}", exc.Message));
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtConsole.Text = string.Empty;
        }
        
        private void btnClearTree_Click(object sender, EventArgs e)
        {
            treeTests.Nodes.Clear();
            listBoxAssemblies.Items.Clear();
            
            _tests.Clear();
            
            btnRun.Enabled = false;
        }

        private void treeTests_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnRun.Enabled = (treeTests.SelectedNode.Tag != null);
        }

    }
}
