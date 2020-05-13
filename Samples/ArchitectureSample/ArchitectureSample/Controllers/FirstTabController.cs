using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchitectureSample.Data;
using ArchitectureSample.Views;

namespace ArchitectureSample.Controllers
{
    public class FirstTabController
    {
        private IFirstTab _view;
        public FirstTabController(IFirstTab ctrl)
        {
            _view = ctrl;
        }
        

        public void BeginLongOperation()
        {
            if (LongOperationRequested != null)
            {
                LongOperationRequested();
            }
        }

        public event Action LongOperationRequested;
        
        public void UpdateOptions()
        {
            Data.FirstTabOptions options = new FirstTabOptions();
            options.Data1 = _view.Value1;
            options.Data2 = _view.Value2;
            Data.GlobalOptions.UpdateFirstTabOptions(options);
        }
        
        public string GetOption()
        {
            return Data.GlobalOptions.SecondTabOptions.Option;
        }
    }
}
