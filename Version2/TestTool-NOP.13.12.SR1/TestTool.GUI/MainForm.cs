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
    /// Main application window.
    /// </summary>
    partial class MainForm : Form, IMainView
    {
        private const string HELP_FILE_NAME = "ONVIF Device Test Tool Help.chm";

        private readonly MainController _controller;

        public MainForm()
        {
            InitializeComponent();

            _controller = new MainController(this);
            _controller.SetChildControllers(setupPage.Controller, 
                discoveryPage.Controller, 
                managementPage.Controller, 
                testPage.Controller, 
                devicePage.Controller);
            _controller.ActivateController(discoveryPage.Controller);
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

        public void ActivateManagementPage()
        {
            tcPages.SelectedTab = tpManagement;
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
            Invoke(new Action(() => OnPostLoad()));
            BeginInvoke(new Action(() => HideSplashScreen()));
        }

        void OnPostLoad()
        {
            _controller.LoadContext();
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

        #region IView Members


        public void ShowError(Exception e)
        {
            
        }

        public void ShowError(string message)
        {
            
        }

        #endregion

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+F combination causes setting focus on Find field 
            // it should work only on "Diagnostics" tab

            if (tcPages != null && tpTest != null && tcPages.SelectedTab == tpTest)
            {
                if (e.Control && e.KeyCode.ToString() == "F")
                {
                    Control.ControlCollection testPageControls = testPage.Controls;
                    Control[] foundControls1 = testPageControls.Find("scMain", true);

                    if (foundControls1.Length == 1 && 
                        foundControls1[0] is SplitContainer)
                    {
                        SplitContainer scMainCopy = (SplitContainer)foundControls1[0];
                        Control.ControlCollection scMainControlsPanel2 = scMainCopy.Panel2.Controls;

                        if (scMainControlsPanel2.Count == 1 && 
                            scMainControlsPanel2[0] is Controls.TestResultsControl)
                        {
                            Controls.TestResultsControl tcTestResultsCopy = (Controls.TestResultsControl)scMainControlsPanel2[0];
                            Control.ControlCollection tcTestResultControls = tcTestResultsCopy.Controls;

                            Control[] foundControls2 = tcTestResultControls.Find("tbFind", true);

                            if (foundControls2.Length == 1 && 
                                foundControls2[0] is TextBox)
                            {
                                foundControls2[0].Focus();
                            }
                        }
                    }
                }
            }
        }
    }
}
