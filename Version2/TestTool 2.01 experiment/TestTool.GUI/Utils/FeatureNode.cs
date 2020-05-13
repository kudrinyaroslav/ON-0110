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
            Enabled = false;
            Visible = true;
            State = FeatureState.Undefined;
        }

        /// <summary>
        /// Feature name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Feature (or service)
        /// </summary>
        public Feature Feature { get; set; }
        /// <summary>
        /// Feature state (depends on device type and parent feature selection)
        /// </summary>
        public FeatureState State { get; set; }

        /// <summary>
        /// True if feature is mandatory (for parent service, like "JPEG" in "Media")
        /// </summary>
        public bool Mandatory { get; set; }
        /// <summary>
        /// True if feature is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// True if feature has been checked manually
        /// </summary>
        public bool CheckedManually { get; private set; }
        /// <summary>
        /// True if feature is selected
        /// </summary>
        public bool Checked { get; private set; }

        /// <summary>
        /// Selects feature.
        /// </summary>
        /// <param name="check"></param>
        /// <param name="manually"></param>
        public void Check(bool check, bool manually)
        {
            Checked = check;
            CheckedManually = manually;
        }

        /// <summary>
        /// Visible (not used now)
        /// </summary>
        public bool Visible { get; set; }
        
        /// <summary>
        /// Sub-features
        /// </summary>
        public List<FeatureNode> Nodes { get; private set; }

    }
}
