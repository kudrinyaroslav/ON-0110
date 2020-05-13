///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.GUI.Views;
using TestTool.GUI.Data;

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
        /// <param name="requestsController"></param>
        public void SetChildControllers(DeviceManagementController deviceManagementController, 
            MediaController mediaController,
            PtzController ptzController, 
            RequestsController requestsController)
        {
            _controllers.AddRange(new IController[]
                                      {
                                          deviceManagementController, 
                                          mediaController, 
                                          ptzController,
                                          requestsController
                                      });

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

            DebugInfo info = ContextController.GetDebugInfo();
            View.CapabilitiesExchange = info.CapabilitiesExchange;
            View.Security = info.Security;
        }

        public override void UpdateContext()
        {
            DoWithAll(controller => controller.UpdateContext());

            DebugInfo info = ContextController.GetDebugInfo();
            info.CapabilitiesExchange = View.CapabilitiesExchange;
            info.Security = View.Security;
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
        /// <param name="state">New state</param>
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

        /// <summary>
        /// Loads data saved in context.
        /// </summary>
        /// <param name="context"></param>
        public override void LoadSavedContext(SavedContext context)
        {
            base.LoadSavedContext(context);

            if (context.DebugInfo != null)
            {
                DebugInfo info = ContextController.GetDebugInfo();
                info.CapabilitiesExchange = context.DebugInfo.CapabilitiesExchange;
                info.Security = context.DebugInfo.Security;
            }

            foreach (IController contoller in _controllers)
            {
                contoller.LoadSavedContext(context);
            }
        }


        /// <summary>
        /// Starts background initialization.
        /// </summary>
        public override void Initialize()
        {
            foreach (IController controller in _controllers)
            {
                controller.Initialize();
            }
        }

    }
}
