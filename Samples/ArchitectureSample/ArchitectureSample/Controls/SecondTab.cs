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
    public partial class SecondTab : UserControl, ISecondTab
    {
        public SecondTab()
        {
            InitializeComponent();
            _controller = new SecondTabController(this);
        }

        private SecondTabController _controller;

        public SecondTabController Controller
        {
            get { return _controller; }
        }

        public void BeginLongOperation()
        {
            label1.Visible = true;
            label2.Enabled = false;
            cmbOptions.Enabled = false;
        }

        public void EndLongOperation()
        {
            label1.Visible = false;
            label2.Enabled = true;
            cmbOptions.Enabled = true;
        }

        public void UpdateValues()
        {
            label2.Text = _controller.GetValues();
        }


        #region ISecondTab Members

        public string Option
        {
            get { return cmbOptions.SelectedItem != null ? cmbOptions.SelectedItem.ToString() : string.Empty; }
        }

        #endregion
    }
}
