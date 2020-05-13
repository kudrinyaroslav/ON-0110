///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Views
{
    interface IDeviceManagementView : IView
    {
        string DeviceManagementUrl { get; set; }
        string IP { get; set; }

        void EnableFunctions(bool bEnable);

        void DisplayLog(string logEntry);

        void DisplayDeviceInformation(string manufacturer, string model, string firmwareVersion, string serial,
                                      string hardwareId);

        void Clear();

        void ReportError(string error);
    }
}
