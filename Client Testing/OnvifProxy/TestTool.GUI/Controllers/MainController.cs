using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Controllers
{
    class MainController : Controller<Views.IMainView>
    {
        public MainController(Views.IMainView view)
            : base(view)
        {

        }


        private IController _activeController;

        /// <summary>
        /// Activates controller when a view is activated.
        /// </summary>
        /// <param name="controller">New active controller.</param>
        public void ActivateController(IController controller)
        {
            if (_activeController != null)
            {
                _activeController.UpdateContext();
            }
            controller.UpdateView();
            controller.UpdateViewFunctions();
            _activeController = controller;
        }

    }
}
