using System.Windows.Forms;
using TestTool.GUI.Controllers;

namespace TestTool.GUI.Controls.Pages
{
    partial class CustomConfigurationPage : BasePage, Views.IConfigurationView
    {
        private Controllers.ConfigurationController _controller;
        public CustomConfigurationPage()
        {
            InitializeComponent();

            _controller = new ConfigurationController(this);
        }

        #region IView Members

        public TestTool.GUI.Controllers.IController GetController()
        {
            return _controller;
        }

        #endregion

        private void btnLoad_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Under Construction!");
        }
    }
}
