using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Controllers
{
    class ConfigurationController : Controller<Views.IConfigurationView>
    {
        public ConfigurationController (Views.IConfigurationView view)
            :base(view)
        {

        }
    }
}
