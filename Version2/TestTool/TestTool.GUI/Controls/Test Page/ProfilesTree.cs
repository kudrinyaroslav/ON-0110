using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.Tests.Definitions;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.GUI.Controls
{
    public partial class ProfilesTree : UserControl, IProfilesView
    {
        public ProfilesTree()
        {
            InitializeComponent();
        }

        private const string GROUP = "Group";

        #region Profiles

        private const string PROFILEUNDEFINED = "ProfileUndefined";
        private const string PROFILESUPPORTED = "ProfileSupported";
        private const string PROFILENOTSUPPORTED = "ProfileNotSupported";
        private const string PROFILEFAILED = "ProfileFailed";
        private const string PROFILERCUNDEFINED = "ProfileRcUndefined";
        private const string PROFILERCSUPPORTED = "ProfileRcSupported";
        private const string PROFILERCNOTSUPPORTED = "ProfileRcNotSupported";
        private const string PROFILERCFAILED = "ProfileRcFailed";
        
        #endregion

        #region scopes

        private const string SCOPEUNDEFINED = "Undefined";
        private const string SCOPESUPPORTED = "Supported";
        private const string SCOPENOTSUPPORTED = "NotSupported";
        private const string OPTIONALSCOPENOTSUPPORTED = "Skipped";
        
        #endregion

        #region Functionality

        private const string UNDEFINED = "Undefined";
        private const string ASSUMESUPPORTED = "NotTested";
        private const string SKIPPED = "Skipped";
        private const string EXPECTED = "Expected";
        private const string SUPPORTED = "Supported";
        private const string FAILED = "NotSupported";
        
        #endregion


        private const string NOTESTS = "No tests were found for this functionality";
        private const string NOFEATURE = "Optional feature not supported";
        private const string CONFIRMED = "Confirmed";
        private const string NOTCONFIRMED = "Not confirmed";
        private const string FEATURENOTSUPPORTED = "Feature not supported";

        private const string SELECTEDFORTEST = "Will be defined after selected tests are executed"; 

        private const string PROFILE_SUPPORTED = "Profile supported";
        private const string PROFILE_NOT_SUPPORTED = "Profile not supported";
        private const string PROFILE_FAILED = "Profile not confirmed";


        private Dictionary<Functionality, List<TreeNode>> _functionalityNodes;
        private Dictionary<String, List<TreeNode>> _scopeNodes;
        private Dictionary<Feature, List<TreeNode>> _discoveryTypesNodes;
        private Dictionary<IProfileDefinition, TreeNode> _profileNodes;
        private Dictionary<IProfileDefinition, Dictionary<Functionality, TreeNode>> _profileFunctionalityNodes;

        public event Action<IProfileDefinition> ProfileSelected;
        
        public event TreeViewCancelEventHandler BeforeSelect;

        /// <summary>
        /// TreeView nodes representing groups of tests.
        /// </summary>
        private List<TreeNode> _groupNodes;

        public void DisplayProfiles(IEnumerable<IProfileDefinition> profiles)
        {
            tvProfiles.Nodes.Clear();
            _scopeNodes = new Dictionary<String, List<TreeNode>>();
            _discoveryTypesNodes = new Dictionary<Feature, List<TreeNode>>();
            _functionalityNodes = new Dictionary<Functionality, List<TreeNode>>();
            _profileNodes = new Dictionary<IProfileDefinition, TreeNode>();
            _groupNodes = new List<TreeNode>();
            _profileFunctionalityNodes = new Dictionary<IProfileDefinition, Dictionary<Functionality, TreeNode>>();

            foreach (IProfileDefinition profile in profiles)
            {
                TreeNode profileNode = tvProfiles.Nodes.Add(profile.GetProfileName());
                //if (profile.Status == ProfileVersionStatus.Release)
                //{
                //    SetImageKey(profileNode, PROFILEUNDEFINED);
                //}
                //else 
                //{
                //    SetImageKey(profileNode, PROFILERCUNDEFINED);
                //}
                SetImageKey(profileNode, PROFILEUNDEFINED);

                profileNode.Tag = profile;
                _profileNodes.Add(profile, profileNode);

                Dictionary<Functionality, TreeNode> funcNodes = new Dictionary<Functionality, TreeNode>();
                _profileFunctionalityNodes.Add(profile, funcNodes);

                TreeNode featureNode = profileNode.Nodes.Add(profile.GetProfileName() + "Scopes", "Scopes");

                foreach (String item in profile.MandatoryScopes)
                {
                    string scopeName = item;
                    TreeNode node = new TreeNode(scopeName);
                    SetImageKey(node, UNDEFINED);
                    node.Tag = profile.MandatoryScopes.Contains(item);

                    featureNode.Nodes.Add(node);
                    if (!_scopeNodes.ContainsKey(item))
                    {
                        _scopeNodes.Add(item, new List<TreeNode>());
                    }

                    _scopeNodes[item].Add(node);
                }

                var typesNode = profileNode.Nodes.Add(profile.GetProfileName() + "Discovery Types", "Discovery Types");
                SetImageKey(typesNode, GROUP);
                if (null != profile.MandatoryDiscoveryTypes)
                {
                    foreach (var dt in profile.MandatoryDiscoveryTypes)
                    {
                        var node = new TreeNode(FeaturesHelper.GetDisplayName(dt));
                        SetImageKey(node, UNDEFINED);
                        node.Tag = true;
                        typesNode.Nodes.Add(node);

                        if (!_discoveryTypesNodes.ContainsKey(dt))
                            _discoveryTypesNodes.Add(dt, new List<TreeNode>());

                        _discoveryTypesNodes[dt].Add(node);
                    }
                }

                TreeNode funcNode = profileNode.Nodes.Add(profile.GetProfileName() + "Functionality", "Functionality");
                SetImageKey(funcNode, GROUP);

                foreach (FunctionalityItem item in profile.Functionalities)
                {

                    // Find group node to add this test to.
                    TreeNode groupNode = FindGroupNode(funcNode, item.Path);
                    
                    TreeNode node = new TreeNode(Utils.FunctionalityHelper.GetDisplayName(item.Functionality));
                    SetImageKey(node, UNDEFINED);
                    groupNode.Nodes.Add(node);
                    if (!_functionalityNodes.ContainsKey(item.Functionality))
                    {
                        _functionalityNodes.Add(item.Functionality, new List<TreeNode>());
                    }
                    _functionalityNodes[item.Functionality].Add(node);
                    funcNodes.Add(item.Functionality, node); 
                }
            }
        }

        /// <summary>
        /// Finds node representing tests group with path specified. If a node does not exist, node 
        /// is created.
        /// </summary>
        /// <param name="path">Group path.</param>
        /// <returns>Old or newly created node.</returns>
        TreeNode FindGroupNode(TreeNode funcNode, string path)
        {
            // path separator is "\"
            string[] parts = path.Split('\\');
            // root group
            string rootName = parts[0];

            TreeNode rootNode = null;

            // find root node
            foreach (TreeNode root in funcNode.Nodes)
            {
                if (root.Name == rootName)
                {
                    rootNode = root;
                    break;
                }
            }

            // if root node not found - create root node.
            if (rootNode == null)
            {
                rootNode = funcNode.Nodes.Add(rootName);
                rootNode.Name = rootName;
                SetImageKey(rootNode, GROUP);
                _groupNodes.Add(rootNode);
            }

            if (parts.Length == 1)
            {
                // If test is under root group
                return rootNode;
            }
            else
            {
                // Find or create other nodes 
                TreeNode current = rootNode;

                string currentPath = rootName;

                // for each group name in the path
                for (int i = 1; i < parts.Length; i++)
                {
                    string group = parts[i];

                    TreeNode next = null;
                    // enumerate child of node found or created for parent group
                    foreach (TreeNode node in current.Nodes)
                    {
                        if (node.Name == group)
                        {
                            next = node;
                            break;
                        }
                    }

                    currentPath += "\\" + group;
                    
                    // if child not found, create new node.
                    if (next == null)
                    {
                        next = current.Nodes.Add(group);
                        SetImageKey(next, GROUP);
                        _groupNodes.Add(next);

                        next.Name = group;
                    }
                    current = next;
                }
                return current;
            }
        }

        void SetImageKey(TreeNode node, string imageKey)
        {
            node.ImageKey = imageKey;
            node.SelectedImageKey = imageKey;
        }

        public void ClearAll()
        {
            ClearProfiles();
            foreach (List<TreeNode> list in _scopeNodes.Values)
            {
                foreach (TreeNode featureNode in list)
                {
                    SetImageKey(featureNode, UNDEFINED);
                    featureNode.ToolTipText = string.Empty;
                }
            }

            foreach (List<TreeNode> list in _discoveryTypesNodes.Values)
            {
                foreach (TreeNode node in list)
                {
                    SetImageKey(node, UNDEFINED);
                    node.ToolTipText = string.Empty;
                }
            }
        }

        public void ClearProfiles()
        {
            foreach (TreeNode profileNode in _profileNodes.Values)
            {
                IProfileDefinition profile = profileNode.Tag as IProfileDefinition;
                //if (profile.Status == ProfileVersionStatus.Release)
                //{
                //    SetImageKey(profileNode, PROFILEUNDEFINED);
                //}
                //else 
                //{
                //    SetImageKey(profileNode, PROFILERCUNDEFINED);
                //}
                SetImageKey(profileNode, PROFILEUNDEFINED);
                profileNode.ToolTipText = string.Empty;
            }
            foreach (List<TreeNode> list in _functionalityNodes.Values)
            {
                foreach (TreeNode funcNode in list)
                {
                    SetImageKey(funcNode, UNDEFINED);
                    funcNode.ToolTipText = string.Empty;
                }
            }
        }

        public void DisplayProfiles(IEnumerable<IProfileDefinition> supported,
            IEnumerable<IProfileDefinition> failed, IEnumerable<IProfileDefinition> notSupported)
        {
            Action action  = new Action(
                () =>
                    {
                        foreach (IProfileDefinition profile in _profileNodes.Keys)
                        {
                            string imageKey = UNDEFINED;
                            string toolTip = string.Empty;

                            //bool rc = profile.Status == ProfileVersionStatus.ReleaseCandidate;
                            bool rc = false;

                            if (supported.Contains(profile))
                            {
                                imageKey = rc ? PROFILERCSUPPORTED : PROFILESUPPORTED;
                                toolTip = PROFILE_SUPPORTED;
                            }
                            else
                            {
                                if (failed.Contains(profile))
                                {
                                    imageKey = rc ? PROFILERCFAILED : PROFILEFAILED;
                                    toolTip = PROFILE_FAILED;
                                }
                                else
                                {                                    
                                    imageKey = rc ? PROFILERCNOTSUPPORTED : PROFILENOTSUPPORTED;
                                    toolTip = PROFILE_NOT_SUPPORTED;
                                }
                            }
                            SetImageKey(_profileNodes[profile], imageKey);
                            _profileNodes[profile].ToolTipText = toolTip;

                        }
                    });

            BeginInvoke(action);
        }

        public void DisplaySupportedFunctionality(Dictionary<Functionality, bool> functionality)
        {
            Action action = new Action(
                () =>
                    {
                        foreach (Functionality f in _functionalityNodes.Keys)
                        {
                            foreach (TreeNode node in _functionalityNodes[f])
                            {
                                if (functionality.ContainsKey(f))
                                {
                                    SetImageKey(node, functionality[f] ? SUPPORTED : FAILED);
                                    node.ToolTipText = functionality[f] ? CONFIRMED : NOTCONFIRMED;
                                }
                            }
                        }

                    });

            BeginInvoke(action);
        }

        //public void DisplayDiscoveryTypes(IEnumerable<Functionality> supported)
        //{
        //    Action action = new Action(
        //        () =>
        //        {
        //            foreach (Functionality f in _functionalityNodes.Keys)
        //            {
        //                foreach (TreeNode node in _functionalityNodes[f])
        //                {
        //                    if (functionality.ContainsKey(f))
        //                    {
        //                        SetImageKey(node, functionality[f] ? SUPPORTED : FAILED);
        //                        node.ToolTipText = functionality[f] ? CONFIRMED : NOTCONFIRMED;
        //                    }
        //                }
        //            }

        //        });

        //    BeginInvoke(action);
        //}

        public void DisplayFunctionalityWithoutTestsInSuite(List<Functionality> functionality)
        {
            Action action = new Action(() =>
                                           {
                                               RunForFunctionalityNodes(functionality,
                                                                        node =>
                                                                            {
                                                                                SetImageKey(node, ASSUMESUPPORTED);
                                                                                node.ToolTipText = NOTESTS;
                                                                            });

                                           });

            BeginInvoke(action);
        }

        public void DisplayOptionalFunctionalityWithoutFeatures(Dictionary<IProfileDefinition, List<Functionality>> functionality)
        {
            Action action = new Action(
                () =>
                    {
                        foreach (IProfileDefinition p in functionality.Keys)
                        {
                            if (_profileFunctionalityNodes.ContainsKey(p))
                            {
                                foreach (Functionality f in functionality[p])
                                {
                                    if (_profileFunctionalityNodes[p].ContainsKey(f))
                                    {
                                        TreeNode node = _profileFunctionalityNodes[p][f];
                                        SetImageKey(node, SKIPPED);
                                        node.ToolTipText = NOFEATURE;
                                    }
                                }
                            }
                        }                        
                    });

            BeginInvoke(action);
        }

        public void DisplayMandatoryFunctionalityWithoutFeatures(Dictionary<IProfileDefinition, List<Functionality>> functionality)
        {
            Action action = new Action(
                () =>
                    {
                        foreach (IProfileDefinition p in functionality.Keys)
                        {
                            if (_profileFunctionalityNodes.ContainsKey(p))
                            {
                                foreach (Functionality f in functionality[p])
                                {
                                    if (_profileFunctionalityNodes[p].ContainsKey(f))
                                    {
                                        TreeNode node = _profileFunctionalityNodes[p][f];
                                        SetImageKey(node, FAILED);
                                        node.ToolTipText = FEATURENOTSUPPORTED;
                                    }
                                }
                            }
                        }

                    });

            BeginInvoke(action);

        }


        public void DisplayFunctionalityToBeTested(List<Functionality> functionality)
        {
            Action action = new Action(
                ()=>
                    {
                        RunForFunctionalityNodes(functionality,
                                                 node =>
                                                     {
                                                         SetImageKey(node, EXPECTED);
                                                         node.ToolTipText = SELECTEDFORTEST;
                                                     });
                        

                    });
            BeginInvoke(action);
        }

        void RunForFunctionalityNodes(IEnumerable<Functionality> functionality, Action<TreeNode> action)
        {
            foreach (Functionality f in functionality)
            {
                if (_functionalityNodes.ContainsKey(f))
                {
                    foreach (TreeNode node in _functionalityNodes[f])
                    {
                        action(node);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No node for: " + f.ToString());
                }
            }
        }

        public void DisplayScope(string scope, bool supported)
        {
            BeginInvoke(
                new Action(
                    () =>
                        {
                            if (_scopeNodes.ContainsKey(scope))
                            {
                                foreach (TreeNode node in _scopeNodes[scope])
                                {
                                    bool mandatory = (bool) node.Tag;
                                    SetImageKey(node, supported ? SUPPORTED : (mandatory ? FAILED : SKIPPED));
                                }
                            }
                        }));
        }

        public void DisplayDiscoveryType(Feature dt, bool supported)
        {
            BeginInvoke(
                new Action(
                    () =>
                    {
                        if (_discoveryTypesNodes.ContainsKey(dt))
                        {
                            foreach (TreeNode node in _discoveryTypesNodes[dt])
                            {
                                //bool mandatory = (bool)node.Tag;
                                SetImageKey(node, supported ? SUPPORTED : FAILED);
                            }
                        }
                    }));
        }

        void tvProfiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                IProfileDefinition profile = (IProfileDefinition) node.Tag;

                if (ProfileSelected != null)
                {
                    ProfileSelected(profile);
                }
            }
        }

        private void tvProfiles_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (BeforeSelect != null)
            {
                BeforeSelect(this, e);
            }
        }

        void tvProfiles_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (_inactive)
            {
                if (e.Node == tvProfiles.SelectedNode && _inactive)
                {
                    _inactive = false;

                    TreeNode node = e.Node;
                    if (node != null)
                    {
                        while (node.Parent != null)
                        {
                            node = node.Parent;
                        }

                        IProfileDefinition profile = (IProfileDefinition)node.Tag;

                        if (ProfileSelected != null)
                        {
                            ProfileSelected(profile);
                        }
                    }
                }
            }
        }

        private bool _inactive;
        public void SetInactive()
        {
            _inactive = true;
        }


    }
}
