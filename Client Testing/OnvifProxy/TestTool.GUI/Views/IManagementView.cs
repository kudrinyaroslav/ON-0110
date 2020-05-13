using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Views
{
    internal interface IManagementView :IView
    {
        void SwitchToWorkingMode();
        void SwitchToIdleMode();

        string BaseAddress { get; set; }

        string DutAddress { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
