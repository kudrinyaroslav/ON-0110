using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Controllers
{
    class TestInfoController : Controllers.Controller<Views.ITestInfoView>
    {

        public TestInfoController(Views.ITestInfoView view)
            :base(view)
        {

        }
    }
}
