using System;
using System.Collections.Generic;
using TestTool.Device;

namespace TestTool.GUI.Views
{
    internal interface ITestView : IView
    {
        void Clear();
        void DisplayNetworkEvent(NetworkEventData data);
        void SwitchToWorkingMode();
        void SwitchToIdleMode();
    }
}
