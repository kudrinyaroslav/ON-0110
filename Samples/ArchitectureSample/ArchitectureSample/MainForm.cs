using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArchitectureSample.Controllers;
using ArchitectureSample.Views;

namespace ArchitectureSample
{
    public partial class MainForm : Form, IMainView
    {
        public MainForm()
        {
            InitializeComponent();

            _controller = new MainController(this);
            _controller.SetChildControllers(firstTab1.Controller, secondTab1.Controller);
        }

        private MainController _controller;


        public void BeginLongOperation()
        {
            Invoke(new Action(InternalBeginLongOperation));
        }

        void InternalBeginLongOperation()
        {
            tabControl.TabPages[2].Enabled = false;
            firstTab1.BeginLongOperation();
            secondTab1.BeginLongOperation();
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
        }

        public void EndLongOperation()
        {
            Invoke(new Action(InternalEndLongOperation));
        }
        
        void InternalEndLongOperation()
        {
            thirdTabPage.Enabled = true;
            firstTab1.EndLongOperation();
            secondTab1.EndLongOperation();
           
            toolStripProgressBar.Style = ProgressBarStyle.Blocks;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == secondTabPage)
            {
                // may be call some method of firstTab1 ?
                firstTab1.Controller.UpdateOptions();
                secondTab1.UpdateValues();
            }
            else if (tabControl.SelectedTab == firstTabPage)
            {
                secondTab1.Controller.UpdateOptions();
                firstTab1.UpdateValues();
            }
        }
    }
}
