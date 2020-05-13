using System;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.Engine
{
    /// <summary>
    /// Holds information got during features definition process.
    /// </summary>
    public class ConformanceInitializationData
    {
        public ConformanceInitializationData()
        {
            TestsSelected = new List<TestInfo>();
            SupportedProfiles = new List<IProfileDefinition>();
            UnsupportedProfiles = new List<IProfileDefinition>();
            FailedProfiles = new List<IProfileDefinition>();

            SupportedFeatures = new List<Feature>();
            UnsupportedFeatures = new List<Feature>();
            UndefinedFeatures = new List<Feature>();
            Scopes = new List<string>();
        }
        
        /// <summary>
        /// Tests selected for conformance
        /// </summary>
        public List<TestInfo> TestsSelected { get; private set; }
        
        /// <summary>
        /// Device information
        /// </summary>
        public DeviceInformation DeviceInformation { get; set; }

        /// <summary>
        /// Supported profiles (more correctly, "claimed")
        /// </summary>
        public List<IProfileDefinition> SupportedProfiles { get; private set; }

        /// <summary>
        /// Not supported profiles
        /// </summary>
        public List<IProfileDefinition> UnsupportedProfiles { get; private set; }
        
        /// <summary>
        /// Profiles aleady failed
        /// </summary>
        public List<IProfileDefinition> FailedProfiles { get; private set; }

        /// <summary>
        /// Supported features
        /// </summary>
        public List<Feature> SupportedFeatures { get; private set; }
        /// <summary>
        /// Not supported features
        /// </summary>
        public List<Feature> UnsupportedFeatures { get; private set; }
        /// <summary>
        /// Undefined features
        /// </summary>
        public List<Feature> UndefinedFeatures { get; private set; }
        /// <summary>
        /// Scopes
        /// </summary>
        public List<string> Scopes { get; private set; }

        /// <summary>
        /// True if process will be continued.
        /// </summary>
        public bool Continue { get; set; }
    }
}
