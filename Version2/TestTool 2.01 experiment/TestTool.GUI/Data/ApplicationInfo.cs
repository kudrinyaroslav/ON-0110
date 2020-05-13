///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using TestTool.GUI.Enums;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Information displayed / entered at the "Setup" tab.
    /// </summary>
    public class ApplicationInfo
    {
        const string SPEC102= "ONVIF Core Specification 1.02, June, 2010";
        private const string SPEC20 = "ONVIF Core Specification 2.0, November, 2010";
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationInfo()
        {
            ToolVersion = "ONVIF Test Tool version 2.0.1.3";
            TestSpecification = "ONVIF Test Specification 1.02.2 Dec, 2010";
            CoreSpecifications = new Dictionary<CoreSpecification, string>();
            CoreSpecifications.Add(CoreSpecification.V102, SPEC102);
            CoreSpecifications.Add(CoreSpecification.V20, SPEC20);
        }

        /// <summary>
        /// Core specifications supported.
        /// </summary>
        public Dictionary<CoreSpecification, string> CoreSpecifications { get; private set; }
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
