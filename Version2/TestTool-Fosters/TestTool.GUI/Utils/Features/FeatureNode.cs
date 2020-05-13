using System;
using System.Collections.Generic;
using TestTool.Tests.Common.Enums;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Feature state 
    /// </summary>
    enum FeatureState
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,
        /// <summary>
        /// Supported
        /// </summary>
        Supported,
        /// <summary>
        /// Not supported
        /// </summary>
        NotSupported,
        /// <summary>
        /// Not a feature
        /// </summary>
        Group
    }

    /// <summary>
    /// Feature with sub-features (if any)
    /// </summary>
    class FeatureNode
    {
        public FeatureNode()
        {
            Nodes = new List<FeatureNode>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Feature Feature { get; set; }

        public FeatureState Status { get; set; }

        public List<FeatureNode> Nodes { get; private set; }
    }
}
