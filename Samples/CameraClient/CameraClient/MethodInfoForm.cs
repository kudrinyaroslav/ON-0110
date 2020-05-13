using System;
using System.Windows.Forms;
using System.Reflection;

namespace CameraClient
{
    public partial class MethodInfoForm : Form
    {
        private MethodInfo _methodInfo;

        public MethodInfoForm()
        {
            InitializeComponent();
        }

        public MethodInfoForm(MethodInfo methodInfo)
        {
            InitializeComponent();
            
            ShowMethodInfo(methodInfo);
            _methodInfo = methodInfo;
        }

        void ShowMethodInfo(MethodInfo methodInfo)
        {
            this.Text = methodInfo.Name;

            lblReturnType.Text = "Return type: " + methodInfo.ReturnType.Name;
            
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                ListViewItem lvi = new ListViewItem(parameterInfo.Name);
                
                ListViewItem.ListViewSubItem lvsi =  lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, parameterInfo.ParameterType.Name));
                lvsi.Tag = parameterInfo.ParameterType;
                listViewParameters.Items.Add(lvi);
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewParameters_DoubleClick(object sender, EventArgs e)
        {
            if (listViewParameters.SelectedItems.Count > 0)
            {
                ListViewItem lvi = listViewParameters.SelectedItems[0];
                Type type = (Type)lvi.SubItems[1].Tag;
                TypeInfoForm form = new TypeInfoForm(type);
                form.Show();

            }
        }

        private void btnViewType_Click(object sender, EventArgs e)
        {
            TypeInfoForm form = new TypeInfoForm(_methodInfo.ReturnType);
            form.Show();
        }

    }
}
