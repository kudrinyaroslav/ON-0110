using TestTool.HttpTransport.Interfaces;

namespace TestTool.GUI.Data
{
    public enum CapabilitiesExchangeStyle
    {
        GetCapabilities,
        GetServices
    }

    public class DebugInfo
    {
        public CapabilitiesExchangeStyle CapabilitiesExchange { get; set; }
        public Security Security { get; set; }
    }
}
