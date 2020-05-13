///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Enums;

namespace TestTool.GUI.Views
{
    /// <summary>
    /// Setup page interface
    /// </summary>
    interface ISetupView : IView
    {
        CoreSpecification CoreSpecification { get; set; }

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
        void EnableSpecificationSelection(bool enable);
    }
}
