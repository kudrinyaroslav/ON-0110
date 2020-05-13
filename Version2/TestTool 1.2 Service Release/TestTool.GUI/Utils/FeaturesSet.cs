using System.Collections.Generic;
using System.Linq;
using TestTool.Tests.Common.Enums;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Set of all features
    /// </summary>
    class FeaturesSet
    {
        public FeaturesSet()
        {
            Nodes = new List<FeatureNode>();
        }

        public List<FeatureNode> Nodes { get; private set; }

        /// <summary>
        /// Propogate changes to child features.
        /// </summary>
        /// <param name="node"></param>
        public static void UpdateChildFeatures(FeatureNode node)
        {
            foreach (FeatureNode child in node.Nodes)
            {
                DefineState(node, child);
                UpdateChildFeatures(child);
            }
        }

        /// <summary>
        /// Selects nodes
        /// </summary>
        /// <param name="features"></param>
        public void SelectNodes(IEnumerable<Feature> features)
        {
            Dictionary<Feature, FeatureNode> nodes = new Dictionary<Feature, FeatureNode>();
            foreach (FeatureNode n in Nodes)
            {
                nodes.Add(n.Feature, n);
            }

            foreach (Feature feature in nodes.Keys)
            {
                if (nodes[feature].Mandatory)
                {
                    nodes[feature].Checked = true;
                }
                else
                {
                    nodes[feature].Checked = features.Contains(feature);
                }

                UpdateChildFeatures(nodes[feature]);

                foreach (FeatureNode child in nodes[feature].Nodes)
                {
                    SelectNode(nodes[feature], child, features.Contains(child.Feature));
                    SelectFeatures(child, features);
                }
            }
        }

        /// <summary>
        /// Selects child features.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="features"></param>
        static void SelectFeatures(FeatureNode node, IEnumerable<Feature> features)
        {
            foreach (FeatureNode child in node.Nodes)
            {
                SelectNode(node, child, features.Contains(child.Feature));
                SelectFeatures(child, features);
            }
        }

        static void SelectNode(FeatureNode node, FeatureNode child, bool selected)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("SelectNode - Feature: {0}, Mandatory: {1}, Enabled: {2}, Selected: {3}",
                child.Feature, child.Mandatory, node.Checked, selected));

            child.Enabled = node.Checked;
            if (node.Checked)
            {
                child.State = child.Mandatory ? FeatureState.Mandatory : FeatureState.Optional;
                if (child.Mandatory)
                {
                    child.Checked = true;
                }
                else
                {
                    child.Checked = selected;
                }
            }
            else
            {
                child.Checked = false;
                child.State = FeatureState.Undefined;
            }

        }

        /// <summary>
        /// Defines node's state.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="child"></param>
        static void DefineState(FeatureNode node, FeatureNode child)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Feature: {0}, Mandatory: {1}, Enabled: {2}",
                child.Feature, child.Mandatory, node.Checked));
            
            child.Enabled = node.Checked;
            if (node.Checked)
            {
                child.State = child.Mandatory ? FeatureState.Mandatory : FeatureState.Optional;
                if (child.Mandatory)
                {
                    child.Checked = true;
                }
            }
            else
            {
                child.Checked = false;
                child.State = FeatureState.Undefined;
            }

        }
    }
}
