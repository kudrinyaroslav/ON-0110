///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;

namespace TestTool.GUI.Controls
{
    partial class DevicePage : UserControl, IDeviceView
    {
        private DeviceController _controller;

        public DevicePage()
        {
            InitializeComponent();

            _controller = new DeviceController(this);
            _controller.SetChildControllers(deviceManagementPage.Controller, 
                mediaPage.Controller, 
                ptzPage.Controller);
        }
        
        internal DeviceController Controller
        {
            get { return _controller; }
        }

        public void SwitchToState(Enums.ApplicationState state)
        {
            deviceManagementPage.SwitchToState(state);
            mediaPage.SwitchToState(state);
            ptzPage.SwitchToState(state);
        }

        #region IView Members


        public IController GetController()
        {
            return _controller;
        }

        #endregion
    }
}
