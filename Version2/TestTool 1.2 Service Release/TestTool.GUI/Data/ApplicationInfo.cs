///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Information displayed / entered at the "Setup" tab.
    /// </summary>
    public class ApplicationInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationInfo()
        {
            ToolVersion = "ONVIF Test Tool version 1.02.4.4";
            TestSpecification = "ONVIF Test Specification 1.02.4 June, 2011";
            CoreSpecification = "ONVIF Core Specification 1.02, June, 2010";
        }

        /// <summary>
        /// Core specification.
        /// </summary>
        public string CoreSpecification { get; private set; }
        /// <summary>
        /// Test specification.
        /// </summary>
        public string TestSpecification { get; private set; }
        /// <summary>
        /// Tool version.
        /// </summary>
        public string ToolVersion { get; private set; }

    }
}
