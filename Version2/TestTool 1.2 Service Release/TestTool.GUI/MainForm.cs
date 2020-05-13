///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using System.Diagnostics;

namespace TestTool.GUI
{
    /// <summary>
    /// Main aplplication window.
    /// </summary>
    partial class MainForm : Form, IMainView
    {
        private const string HELP_FILE_NAME = "ONVIF Conformance Test Tool Help.chm";

        private readonly MainController _controller;

        public MainForm()
        {
            InitializeComponent();

//#if !FULL
//            this.tcPages.TabPages.Remove(tpRequests);
//#endif

            _controller = new MainController(this);
            _controller.SetChildControllers(setupPage.Controller, 
                discoveryPage.Controller, 
                managementPage.Controller, 
                testPage.Controller, 
                reportPage.Controller, 
                devicePage.Controller, 
                requestsPage.Controller);
            _controller.ActivateController(setupPage.Controller);
        }

        /// <summary>
        /// Sets cursor
        /// </summary>
        /// <param name="state">Application state</param>
        public void SwitchToState(Enums.ApplicationState state)
        {
            BeginInvoke(new Action( () =>
            {
                if (state == Enums.ApplicationState.Idle || state == Enums.ApplicationState.TestPaused)
                {
                    this.Cursor = Cursors.Arrow;
                }
                else
                {
                    this.Cursor = Cursors.AppStarting;
                }
            }
            ));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_controller.TestIsRunning())
            {
                DialogResult result = MessageBox.Show("Test is running. Stop the test and exit?", "Warning!", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    _controller.StopTest();
                }
            }

            if (_controller.RequestInProgress())
            {
                _controller.StopRequest();
            }

            try
            {
                _controller.SaveContextData();
            }
            catch (System.Exception)
            {
            }
        }

        /// <summary>
        /// Returns controller.
        /// </summary>
        /// <returns></returns>
        public IController GetController()
        {
            return _controller;
        }
        private void tcPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            IView view = tcPages.SelectedTab.Controls[0] as IView;
            _controller.ActivateController(view.GetController());
        }

        private void howDoIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo ProcessInfo;
            Process Process;

            try
            {
                ProcessInfo = new ProcessStartInfo(HELP_FILE_NAME);
                ProcessInfo.CreateNoWindow = false;
                ProcessInfo.UseShellExecute = true;
                Process = Process.Start(ProcessInfo);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _controller.LoadContextData();
            }
            catch (System.Exception)
            {
            }
            _controller.OnPostLoadContextData();
        }

        #region SplashScreen

        /// <summary>
        /// Displays start image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            ShowSplashScreen();

            this.Cursor = Cursors.AppStarting;

            Action initAction = new Action(InitializeControllers);
            IAsyncResult asyncResult = initAction.BeginInvoke(null, null);
    
            Action<IAsyncResult> waitAction = new Action<IAsyncResult>(WaitMethod);
            waitAction.BeginInvoke(asyncResult, null, null);
        }

        /// <summary>
        /// Initializes controllers.
        /// </summary>
        void InitializeControllers()
        {
            _controller.Initialize();
        }

        void WaitMethod(IAsyncResult mainAction)
        {
#if DEBUG
            System.Threading.Thread.Sleep(300);
#else
            System.Threading.Thread.Sleep(3000);
#endif
            WaitHandle.WaitAll(new WaitHandle[] { mainAction.AsyncWaitHandle });
            BeginInvoke(new Action(() => HideSplashScreen()));
        }

        void ShowSplashScreen()
        {
            this.mainMenuStrip.Visible = false;
            splashImage.Location = new Point(0,0);
            splashImage.Size = new Size(this.Width, this.Height);
            splashImage.Update();
        }

        void HideSplashScreen()
        {
            splashImage.Visible = false;
            this.MainMenuStrip.Visible = true;
            this.Cursor = Cursors.Default;
            tcPages.BringToFront();
        }

        #endregion
    }
}
