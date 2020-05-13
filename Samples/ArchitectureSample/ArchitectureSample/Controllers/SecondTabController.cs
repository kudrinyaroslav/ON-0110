using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchitectureSample.Data;
using ArchitectureSample.Views;

namespace ArchitectureSample.Controllers
{
    public class SecondTabController
    {
        private ISecondTab _view;
        public SecondTabController(ISecondTab ctrl)
        {
            _view = ctrl;
        }

        public string GetValues()
        {
            return string.Format("Values at first tab: Some data - {0}, Something else - {1}",
                                 Data.GlobalOptions.FirstTabOptions.Data1, Data.GlobalOptions.FirstTabOptions.Data2);
        }

        public void UpdateOptions()
        {
            Data.SecondTabOptions options = new SecondTabOptions();
            options.Option = _view.Option;
            Data.GlobalOptions.UpdateSecondTabOptions(options);
        }

    }
}
