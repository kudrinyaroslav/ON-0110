using System;
using System.Reflection;
using System.Windows.Forms;

namespace CameraClient
{
    public partial class TypeInfoForm : Form
    {
        public TypeInfoForm()
        {
            InitializeComponent();
        }

        public TypeInfoForm(Type type)
        {
            InitializeComponent();
            CreateTree(type);
        }

        void CreateTree(Type type)
        {
            TreeNode rootNode = treeViewType.Nodes.Add(type.Name);
            ConstructNode(rootNode, type);
        }

        bool SkipType(Type type)
        {
            return (type == typeof (System.Xml.XmlElement))
                   || (type == typeof (System.Xml.XmlNode))
                   || (type == typeof (System.Xml.XmlAttribute));
        }

        void ConstructNode(TreeNode root, Type type)
        {
            if (SkipType(type)  )
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine(type.Name);

            try
            {
                if (type.IsArray)
                {
                    Type memberType = type.GetElementType();

                    TreeNode itemNode = root.Nodes.Add("Element");
                    itemNode.ForeColor = System.Drawing.Color.Green;

                    TreeNode typeNode = itemNode.Nodes.Add(memberType.Name, memberType.Name);
                    typeNode.ForeColor = SkipType(memberType) ? System.Drawing.Color.Gray : System.Drawing.Color.Blue;

                    ConstructNode(itemNode, memberType);

                }
                else
                {
                    PropertyInfo[] properties = type.GetProperties();
                    
                    foreach (PropertyInfo pi in properties)
                    {
                        TreeNode propertyNode = root.Nodes.Add(pi.Name, string.Format("{0}", pi.Name));
                        TreeNode typeNode = propertyNode.Nodes.Add(pi.PropertyType.Name, pi.PropertyType.Name);

                        TreeNode node = propertyNode; 

                        if (pi.PropertyType.IsArray)
                        {
                            typeNode.ForeColor = System.Drawing.Color.DarkGreen;
                            node = typeNode;
                        }
                        else
                        {
                            typeNode.ForeColor = SkipType(pi.PropertyType) ? System.Drawing.Color.Gray : System.Drawing.Color.Blue;
                        }

                        if (pi.PropertyType != type)
                        {
                            ConstructNode(node, pi.PropertyType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                root.ForeColor = System.Drawing.Color.Red;
            }

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
