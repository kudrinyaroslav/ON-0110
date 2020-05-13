using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ArchitectureSample.Views;

namespace ArchitectureSample.Controllers
{
    class MainController
    {
        private FirstTabController _firstTabController;
        private SecondTabController _secondTabController;

        private IMainView _view;
        public  MainController(IMainView view)
        {
            _view = view;
        }

        public void SetChildControllers(FirstTabController firstTabController, 
            SecondTabController secondTabController)
        {
            _firstTabController = firstTabController;
            _secondTabController = secondTabController;

            _firstTabController.LongOperationRequested += new Action(_firstTabController_LongOperationRequested);
            
        }

        void _firstTabController_LongOperationRequested()
        {
            System.Threading.Thread thread = new Thread(LongOperation);
            thread.Start();
        }

        void  LongOperation()
        {
            _view.BeginLongOperation();
            System.Threading.Thread.Sleep(3000);

            _view.EndLongOperation();
        }
    }
}
