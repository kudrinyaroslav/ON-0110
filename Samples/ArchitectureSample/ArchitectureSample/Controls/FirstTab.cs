using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArchitectureSample.Controllers;
using ArchitectureSample.Views;

namespace ArchitectureSample.Controls
{
    public partial class FirstTab : UserControl, IFirstTab
    {
        public FirstTab()
        {
            InitializeComponent();
            _controller = new FirstTabController(this);
        }

        private FirstTabController _controller;

        public FirstTabController Controller
        {
            get { return _controller; }
        }

        public void BeginLongOperation()
        {
            EnableControls(false);
        }

        public void EndLongOperation()
        {
            EnableControls(true);
        }

        void EnableControls(bool enable)
        {
            EnableControl(textBox1, enable);
            EnableControl(textBox2, enable);
            EnableControl(label1, enable);
            EnableControl(label2, enable);
            EnableControl(btnShortOperation, enable);
            EnableControl(btnLongOperation, enable);
            EnableControl(label3, enable);
        }

        void EnableControl(Control ctrl, bool enable)
        {
            ctrl.Enabled = enable;
        }

        private void btnLongOperation_Click(object sender, EventArgs e)
        {
            _controller.BeginLongOperation();
        }

        public void UpdateValues()
        {
            label3.Text = string.Format("Option selected at second tab: {0}", _controller.GetOption());
        }

        #region IFirstTab Members


        public string Value1
        {
            get { return textBox1.Text; }
        }

        public string Value2
        {
            get { return textBox2.Text; }
        }

        #endregion
    }
}
