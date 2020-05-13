using System;
using System.Collections.Generic;
using TestTool.Tests.Common.Enums;

namespace TestTool.Tests.Common.TestEngine
{

    /// <summary>
    /// Feature with sub-features (if any)
    /// </summary>
    public class FeatureNode
    {
        public FeatureNode()
        {
            Nodes = new List<FeatureNode>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Feature Feature { get; set; }

        public FeatureState State { get; set; }
        public FeatureStatus Status { get; set; }

        public List<FeatureNode> Nodes { get; private set; }
    }
}
