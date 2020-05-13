///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using TestTool.Tests.Definitions.Enums;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Request tab information.
    /// </summary>
    public class RequestsInfo
    {
        /// <summary>
        /// Service selected (Device management, media etc.)
        /// </summary>
        public Enums.DutService Service { get; set; }  
        /// <summary>
        /// Service address.
        /// </summary>
        public string ServiceAddress { get; set; }
    }
}
