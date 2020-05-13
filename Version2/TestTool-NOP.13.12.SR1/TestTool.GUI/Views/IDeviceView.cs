///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using TestTool.HttpTransport.Interfaces;

namespace TestTool.GUI.Views
{
    interface IDeviceView : IView
    {
        Security Security { get; set; }
        Data.CapabilitiesExchangeStyle CapabilitiesExchange { get; set; }
    }
}
