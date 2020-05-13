///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.GUI.Views
{
    interface ISetupView : IView
    {
        void EnableGetFromDevice(bool enable);
        string FirmwareVersion { get; set; }
        string Brand { get; set; }
        string Model { get; set; }
        string Serial { get; set; }
        string OtherInformation { get; set; }
        string OperatorName { get; set; }
        string OrganizationName { get; set; }
        string OrganizationAddress { get; set; }
        void ShowToolInfo();
        void ShowError(Exception e);
    }
}
