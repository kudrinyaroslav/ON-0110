using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Device;

namespace TestTool.GUI.Views
{
    internal interface IManagementView :IView
    {
        string BaseAddress { get; set; }
        AuthenticationMode AuthenticationMode { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
