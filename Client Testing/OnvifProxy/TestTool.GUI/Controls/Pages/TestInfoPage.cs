using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestTool.GUI.Controllers;

namespace TestTool.GUI.Controls
{
    internal partial class TestInfoPage : BasePage, Views.ITestInfoView
    {
        private Controllers.TestInfoController _controller;

        public TestInfoPage()
        {
            InitializeComponent();

            _controller = new TestInfoController(this);
        }

        private void btnClearMemberInfo_Click(object sender, EventArgs e)
        {
            tbMemberName.Clear();
            tbMemberAddress.Clear();
        }

        private void btnClearDeviceInformation_Click(object sender, EventArgs e)
        {
            tbProductName.Clear();
            tbBrand.Clear();
            tbModel.Clear();
            tbOtherInformation.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbOperatorName.Clear();
            tbOrganizationName.Clear();
            tbOrganizationAddress.Clear();
        }

        #region IView Members

        public IController GetController()
        {
            return _controller;
        }
        
        #endregion
    }
}
