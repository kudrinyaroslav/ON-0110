///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.HttpTransport.Interfaces;

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
                ptzPage.Controller, 
                requestsPage.Controller);
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

        private bool _notUserAction = false;

        public Security Security
        {
            get
            {
                if (rbNone.Checked)
                {
                    return Security.None;
                }
                if (rbDigest.Checked)
                {
                    return Security.Digest;
                }
                return Security.WS;
            }
            set
            {
                _notUserAction = true;
                switch (value)
                {
                    case Security.None:
                        rbNone.Checked = true;
                        break;
                    case Security.Digest:
                        rbDigest.Checked = true;
                        break;
                    case Security.WS:
                        rbWsUsername.Checked = true;
                        break;
                }
                _notUserAction = false;
            }
        }

        public Data.CapabilitiesExchangeStyle CapabilitiesExchange
        {
            get
            {
                return rbGetCapabilities.Checked
                           ? Data.CapabilitiesExchangeStyle.GetCapabilities
                           : Data.CapabilitiesExchangeStyle.GetServices;
            }
            set
            {
                _notUserAction = true;
                rbGetCapabilities.Checked = (value == Data.CapabilitiesExchangeStyle.GetCapabilities);
                _notUserAction = false;
            }
        }


        private void rb_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_notUserAction)
            {
                return;
            }
            _controller.UpdateContext();
            requestsPage.Controller.UpdateRequestSecurity();
        }

    }
}
