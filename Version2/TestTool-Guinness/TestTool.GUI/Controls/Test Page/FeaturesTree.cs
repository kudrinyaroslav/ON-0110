using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.GUI.Utils;
using TestTool.GUI.Views;
using TestTool.Tests.Definitions.Enums;
using System;
using TestTool.Tests.Definitions.Features;

namespace TestTool.GUI.Controls
{
    public partial class FeaturesTree : UserControl, IFeaturesView
    {
        public FeaturesTree()
        {
            InitializeComponent();
            BuildFeaturesTree();
        }

        #region Features

        private FeaturesSet _featuresSet;

        Dictionary<Feature, TreeNode> _featureNodes = new Dictionary<Feature, TreeNode>();

        private string SUPPORTEDFEATURE = "SUPPORTED";
        private string UNDEFINEDFEATURE = "UNDEFINED";
        private string NOTSUPPORTEDFEATURE = "NOT SUPPORTED";

        public event TreeViewCancelEventHandler BeforeSelect;

        #region Tree building

        /// <summary>
        /// Builds features tree.
        /// This operatio is performed when control is created.
        /// </summary>
        void BuildFeaturesTree()
        {
            _featureNodes.Clear();
            _featuresSet = FeaturesSet.CreateFeaturesSet();
            FeaturesHelper.Translate(_featuresSet);

            foreach (FeatureNode node in _featuresSet.Nodes)
            {
                AddFeatureNode(null, node);
            }

            tvFeatures.ExpandAll();

            TreeNode tn = tvFeatures.Nodes[0];
            tn.EnsureVisible();

        }

        /// <summary>
        /// Adds feature node with subnodes.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        void AddFeatureNode(TreeNode parent, FeatureNode node)
        {
            TreeNode treeNode = new TreeNode(node.DisplayName);
            treeNode.Tag = node;
            treeNode.Name = node.Name;

            if (node.Status != FeatureStatus.Group)
            {
                ClearNode(treeNode);
                _featureNodes.Add(node.Feature, treeNode);
            }
            else
            {
                treeNode.ImageKey = "Group";
                treeNode.SelectedImageKey = treeNode.ImageKey;
            }

            if (parent != null)
            {
                parent.Nodes.Add(treeNode);
            }
            else
            {
                tvFeatures.Nodes.Add(treeNode);
            }
            foreach (FeatureNode child in node.Nodes)
            {
                AddFeatureNode(treeNode, child);
            }
        }

        #endregion

        #endregion

        public void DisplayFeature(Feature feature, bool supported)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        if (_featureNodes.ContainsKey(feature))
                        {
                            TreeNode node = _featureNodes[feature];
                            SetupNode(node, supported);
                        }
                        else
                        {

                        }
                    }));
        }

        public void DisplayUndefinedFeature(Feature feature)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        if (_featureNodes.ContainsKey(feature))
                        {
                            TreeNode node = _featureNodes[feature];
                            node.ImageKey = UNDEFINEDFEATURE;
                            node.SelectedImageKey = UNDEFINEDFEATURE;
                        }
                        else
                        {

                        }
                    }));

        }

        void SetupNode(TreeNode node, bool supported)
        {
            node.ToolTipText = supported ? SUPPORTEDFEATURE : NOTSUPPORTEDFEATURE;

            node.ImageKey = supported ? "Supported" : "NotSupported";
            node.SelectedImageKey = node.ImageKey;
        }

        void ClearNode(TreeNode node)
        {
            node.ImageKey = "Clear";
            node.ToolTipText = "";
            node.SelectedImageKey = node.ImageKey;
        }

        public void Clear()
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        foreach (TreeNode node in _featureNodes.Values)
                        {
                            if (node.ImageKey != "Group")
                            {
                                ClearNode(node);
                            }
                        }
                    }));
        }

        public event EventHandler TreeActivated;

        private void tvFeatures_Click(object sender, EventArgs e)
        {
            if (TreeActivated != null)
            {
                TreeActivated(this, new EventArgs());
            }
        }

        private void tvFeatures_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (BeforeSelect != null)
            {
                BeforeSelect(this, e);
            }
        }

        void tvFeatures_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (_inactive)
            {
                if (TreeActivated != null)
                {
                    TreeActivated(this, new EventArgs());
                }
            }
        }

        private bool _inactive;
        public void SetInactive()
        {
            _inactive = true;
        }

        #region IFeaturesView Members


        #endregion

        #region IView Members

        public void SwitchToState(TestTool.GUI.Enums.ApplicationState state)
        {

        }

        public TestTool.GUI.Controllers.IController GetController()
        {
            return null;
        }

        #endregion
    }
}
