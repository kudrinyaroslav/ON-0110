using System;
using System.Windows.Forms;
using System.Reflection;

namespace CameraClient
{
    public partial class QuickWatchForm : Form
    {
        public QuickWatchForm()
        {
            InitializeComponent();
        }

        public QuickWatchForm(string methodInfo, object obj)
        {
            InitializeComponent();

            lblInfo.Text = methodInfo;
            if (obj != null)
            {
                CreateTree(obj);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void CreateTree(object obj)
        {
            TreeNode rootNode = tvObject.Nodes.Add("Result");
            ConstructNode(rootNode, obj);
        }

        void ConstructNode(TreeNode root, object obj)
        {
            if (obj == null)
            {
                return;
            }

            try
            {
                Type type = obj.GetType();
                PropertyInfo[] properties = type.GetProperties();
                if ((properties.Length == 0) || (obj.GetType() == typeof(string)))
                {
                    TreeNode typeNode = root.Nodes.Add(obj.GetType().Name, obj.GetType().Name);
                    typeNode.ForeColor = System.Drawing.Color.Gray;
                    TreeNode valueNode = root.Nodes.Add(Convert.ToString(obj), Convert.ToString(obj));
                    valueNode.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    TreeNode tNode = root.Nodes.Add(obj.GetType().Name, obj.GetType().Name);
                    tNode.ForeColor = System.Drawing.Color.Gray;

                    foreach (PropertyInfo pi in properties)
                    {
                        object value = pi.GetValue(obj, null);
                        TreeNode propertyNode = root.Nodes.Add(pi.Name, string.Format("{0}", pi.Name));
                        if (value== null)
                        {
                            TreeNode typeNode = propertyNode.Nodes.Add(pi.PropertyType.Name, pi.PropertyType.Name );
                            typeNode.ForeColor = System.Drawing.Color.Gray;

                            TreeNode valueNode = propertyNode.Nodes.Add("null", "NULL");
                            valueNode.ForeColor = System.Drawing.Color.Blue;

                        }
                        else
                        {
                            ConstructNode(propertyNode, value);
                            
                        }
                    }                    
                }

            }
            catch (Exception ex)
            {
                root.ForeColor = System.Drawing.Color.Red;
            }

        }

    }
}
