using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.Enums;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Feature state 
    /// </summary>
    enum FeatureState
    {
        /// <summary>
        /// Disabled for selection
        /// </summary>
        Undefined,
        /// <summary>
        /// Mandatory
        /// </summary>
        Mandatory,
        /// <summary>
        /// Optional
        /// </summary>
        Optional
    }

    /// <summary>
    /// Feature with sub-features (if any)
    /// </summary>
    class FeatureNode
    {
        public FeatureNode()
        {
            Nodes = new List<FeatureNode>();
            Mandatory = false;
            Enabled = false;
            State = FeatureState.Undefined;
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Feature Feature { get; set; }

        public bool Mandatory { get; set; }
        public bool Enabled { get; set; }
        /// <summary>
        /// Feature state (depends on device type and parent feature selection)
        /// </summary>
        public FeatureState State { get; set; }
        public bool Checked { get; set; }

        public List<FeatureNode> Nodes { get; private set; }
    }
}
