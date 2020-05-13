///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.GUI.Views;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Contains GUI logic for Device tab (mostly - passing control to child tabs).
    /// </summary>
    class DeviceController : Controller<IDeviceView>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">View.</param>
        public DeviceController(IDeviceView view)
            :base(view)
        {
            _controllers = new List<IController>();
        }

        private MediaController _mediaController;
        private PtzController _ptzController;

        public MediaController MediaController
        {
            get { return _mediaController; }
        }
        public PtzController PTZController
        {
            get { return _ptzController; }
        }
        
        /// <summary>
        /// Controllers of child tabs.
        /// </summary>
        private List<IController> _controllers;

        /// <summary>
        /// Saves references to child tabs controllers.
        /// </summary>
        /// <param name="deviceManagementController">Controller for the Device -> Management tab.</param>
        /// <param name="mediaController">Controller for the Device -> Media tab.</param>
        /// <param name="ptzController">Controller for the Device -> PTZ tab.</param>
        public void SetChildControllers(DeviceManagementController deviceManagementController, 
            MediaController mediaController, PtzController ptzController)
        {
            _mediaController = mediaController;
            _ptzController = ptzController;
            
            _controllers.AddRange(new IController[] { deviceManagementController, mediaController, ptzController });

            foreach (IController controler in _controllers)
            {
                controler.OperationStarted += _childController_OperationStarted;
                controler.OperationCompleted += _childController_OperationCompleted;
            }
        }

        /// <summary>
        /// Raises OperationStarted event when a child controlled starts a time-consuming operation. 
        /// </summary>
        void _childController_OperationStarted()
        {
            ReportOperationStarted();
        }

        /// <summary>
        /// Raises OperationCompleted event when a time-consuming operation ends. 
        /// </summary>
        void _childController_OperationCompleted()
        {
            ReportOperationCompleted();
        }

        /// <summary>
        /// Perfirms action specified with all child controllers.
        /// </summary>
        /// <param name="action">Action to be performed.</param>
        void DoWithAll(Action<IController> action)
        {
            foreach (IController controler in _controllers)
            {
                action(controler);
            }
        }

        /// <summary>
        /// Updates all Views.
        /// </summary>
        public override void UpdateView()
        {
            DoWithAll( controller => controller.UpdateView());
        }

        /// <summary>
        /// Updates view functions.
        /// </summary>
        public override void UpdateViewFunctions()
        {
            DoWithAll( controller=> controller.UpdateViewFunctions());
        }

        /// <summary>
        /// Switches tab to the state specified.
        /// </summary>
        /// <param name="state"></param>
        public override void SwitchToState(Enums.ApplicationState state)
        {
            DoWithAll( controller => controller.SwitchToState(state));
        }

        /// <summary>
        /// Checks if any of child controllers performs time-consuming operation.
        /// </summary>
        /// <returns></returns>
        public bool InternalRequestPending()
        {
            foreach (IController controler in _controllers)
            {
                if (controler.RequestPending)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indicates that a time-consuming operation is being performed.
        /// </summary>
        public override bool RequestPending
        {
            get
            {
                return InternalRequestPending();
            }
        }

        /// <summary>
        /// Stops time-consuming operation, if any.
        /// </summary>
        public override void Stop()
        {
            DoWithAll( 
                controller => 
                { if (controller.RequestPending)
                    {
                    controller.Stop();
                    }
                }
            );
        }

        public override void LoadSavedContext(TestTool.GUI.Data.SavedContext context)
        {
            base.LoadSavedContext(context);
            foreach (IController contoller in _controllers)
            {
                contoller.LoadSavedContext(context);
            }
        }
    }
}
