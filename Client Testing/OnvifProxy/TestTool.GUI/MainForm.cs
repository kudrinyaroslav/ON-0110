using System;
using System.Windows.Forms;
using TestTool.GUI.Controllers;
using TestTool.GUI.Views;

namespace TestTool.GUI
{
    public partial class MainForm : Form, IMainView
    {
        private const string APPNAME = "ONVIF Client Test Tool";

        private MainController _controller;

        public MainForm()
        {
            InitializeComponent();

            InitializeControllers();

            tssLabelState.Image = Properties.Resources.NotRunning;
        }

        private TestController _testController;
        private ManagementController _managementController;
        void InitializeControllers()
        {
            _controller = new MainController(this);
            _controller.ActivateController(managementPage.GetController());
            _testController = testPage.Controller;
            _managementController = (ManagementController)managementPage.GetController();

            _testController.Started += _diagnosticController_Started;
            _testController.Stopped += _diagnosticController_Stopped;
        }


        void tcMain_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            IView view = tcMain.SelectedTab.Controls[0] as IView;
            _controller.ActivateController(view.GetController());
        }

        void _diagnosticController_Stopped()
        {
            Invoke(new Action(() =>
                                  {
                                      tssLabelState.Text = "Not Running";
                                      tssLabelState.Image = Properties.Resources.NotRunning;
                                      _managementController.SwitchToIdleMode();
                                      UpdateFormTitle(string.Empty);
                                  }));
        }

        void _diagnosticController_Started()
        {
            Invoke(new Action(() =>
                                  {
                                      tssLabelState.Text = "Running";
                                      tssLabelState.Image = Properties.Resources.Running;
                                      _managementController.SwitchToWorkingMode();
                                      UpdateFormTitle(Data.Environment.GetDeviceServiceAddress(Data.Context.Instance.Environment.BaseAddress));
                                  }));
        }


        #region IView Members

        public IController GetController()
        {
            return _controller;
        }

        public void ReportError(string message)
        {
            MessageBox.Show("Error!", message);
        }

        #endregion

        public void UpdateFormTitle(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                this.Text = APPNAME;
            }
            else
            {
                this.Text = string.Format("{0} - {1}", APPNAME, address);
            }
        }
    }
}
