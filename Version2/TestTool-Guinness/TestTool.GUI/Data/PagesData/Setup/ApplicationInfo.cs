﻿///////////////////////////////////////////////////////////////////////////
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
            ToolVersion = "Guinness.120615";
            ToolVersionFull = string.Format("ONVIF Device Test Tool version {0}", ToolVersion);
            TestSpecification = "ONVIF Test Specification June, 2012";
        }

        /// <summary>
        /// Test specification.
        /// </summary>
        public string TestSpecification { get; private set; }
        /// <summary>
        /// Tool version.
        /// </summary>
        public string ToolVersion { get; private set; }

        public string ToolVersionFull { get; private set; }
    }
}