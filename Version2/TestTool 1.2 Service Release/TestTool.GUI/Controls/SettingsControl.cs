using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TestTool.GUI.Utils;
using TestTool.Tests.Common.Enums;
using IPAddress=System.Net.IPAddress;

namespace TestTool.GUI.Controls
{
    /// <summary>
    /// Control with test settings at the "Management" tab.
    /// </summary>
    partial class SettingsControl : Page
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControl()
        {
            InitializeComponent();
            BuildFeaturesTree();
        }

        /// <summary>
        /// Current application state
        /// </summary>
        private Enums.ApplicationState _currentState;

        /// <summary>
        /// Saves current state for future use
        /// </summary>
        /// <param name="state">New application state.</param>
        public void SetCurrentState(Enums.ApplicationState state)
        {
            _currentState = state;
        }

        /// <summary>
        /// Disables child controls.
        /// </summary>
        public void DisableControl()
        {
            DisableControls(new Control[] {
                tbMessageTimeout,
                tbRebootTimeout,
                tbTimeBetweenTests,
                tvFeatures, 
                tbDnsIp4,
                tbNtpIp4,
                tbGatewayIpv4,
                tbDnsIp6,
                tbNtpIp6,
                tbGatewayIpv6,
                cmbPTZNodes,
                btnGetPTZNodes, 
                rbEmbeddedPasswords, 
                rbOwnPasswords, 
                tbPassword1, 
                tbPassword2,
                tbOperationDelay, 
                tbRecoveryDelay});
        }

        /// <summary>
        /// Enables child controls.
        /// </summary>
        public void EnableControl()
        {
            EnableControls(new Control[] {
                tbMessageTimeout,
                tbRebootTimeout,
                tbTimeBetweenTests,
                tvFeatures, 
                tbDnsIp4,
                tbNtpIp4,
                tbGatewayIpv4,
                tbDnsIp6,
                tbNtpIp6,
                tbGatewayIpv6,
                cmbPTZNodes,
                btnGetPTZNodes, 
                rbEmbeddedPasswords, 
                rbOwnPasswords,                 
                tbPassword1, 
                tbPassword2,
                tbOperationDelay, 
                tbRecoveryDelay});
        }
        
        /// <summary>
        /// Changes state of profile-dependent controls.
        /// </summary>
        /// <param name="bCustom">True, if custom profile is in use.</param>
        public void EnableProfileControls(bool bCustom)
        {
            tbMessageTimeout.ReadOnly = !bCustom;
            tbRebootTimeout.ReadOnly = !bCustom;
            tbTimeBetweenTests.ReadOnly = !bCustom;
            tbDnsIp4.ReadOnly = !bCustom;
            tbNtpIp4.ReadOnly = !bCustom;
            tbGatewayIpv4.ReadOnly = !bCustom;
            tbDnsIp6.ReadOnly = !bCustom;
            tbNtpIp6.ReadOnly = !bCustom;
            tbGatewayIpv6.ReadOnly = !bCustom;
            cmbPTZNodes.Enabled = bCustom;
            btnGetPTZNodes.Enabled = bCustom;

            rbEmbeddedPasswords.Enabled = bCustom;
            rbOwnPasswords.Enabled = bCustom;
            tbPassword1.ReadOnly = !bCustom;
            tbPassword2.ReadOnly = !bCustom;

            tbOperationDelay.ReadOnly = !bCustom;
            tbRecoveryDelay.ReadOnly = !bCustom;

            tvFeatures.BackColor = bCustom ? SystemColors.Window : SystemColors.ButtonFace;
            btnSelectTests.Enabled = bCustom;

            _featuresReadOnly = !bCustom;

        }
        
        /// <summary>
        /// Enables passwords controls.
        /// </summary>
        /// <param name="currentState">Application state.</param>
        public void EnablePasswords(Enums.ApplicationState currentState)
        {
            tbPassword1.Enabled = !rbEmbeddedPasswords.Checked && (currentState == Enums.ApplicationState.Idle);
            tbPassword2.Enabled = !rbEmbeddedPasswords.Checked && (currentState == Enums.ApplicationState.Idle);
        }

        #region Properties

        /// <summary>
        /// Message timeout entered by the operator
        /// </summary>
        public int MessageTimeout
        {
            get
            {
                int result = 0;
                int.TryParse(tbMessageTimeout.Text, out result);
                return result;
            }
            set
            {
                _lastValidMessageTimeout = value;
                tbMessageTimeout.Text = value.ToString();
            }
        }

        /// <summary>
        /// Reboot timeout entered by the operator
        /// </summary>
        public int RebootTimeout
        {
            get
            {
                int result = 0;
                int.TryParse(tbRebootTimeout.Text, out result);
                return result;
            }
            set
            {
                _lastValidRebootTimeout = value;
                tbRebootTimeout.Text = value.ToString();
            }
        }

        /// <summary>
        /// Interval between tests entered by the operator
        /// </summary>
        public int TimeBetweenTests
        {
            get
            {
                int result;
                int.TryParse(tbTimeBetweenTests.Text, out result);
                return result;
            }
            set
            {
                _lastValidTimeBetweenTests = value;
                tbTimeBetweenTests.Text = value.ToString();
            }
        }
        
        List<Feature> _features = new List<Feature>();
        /// <summary>
        /// Features selected
        /// </summary>
        public List<Feature> Features
        {
            get
            {
                return _features;
            }
        }

        
        /// <summary>
        /// Selects features and services.
        /// </summary>
        /// <param name="features">Features selected.</param>
        public void SelectFeatures(List<Feature> features)
        {
            _features.Clear();
            _features.AddRange(features);

            // update internal data structure.
            _featuresSet.SelectNodes(features);

            // update control
            foreach (FeatureNode node in _featuresSet.Nodes)
            {
                TransmitProperties(node, _featureNodes[node.Feature]);
                SetupChildFeatures(node);
            }
        }
        

        /// <summary>
        /// NTP address entered by operator (IP4)
        /// </summary>
        public string NtpIpv4
        {
            get
            {
                return tbNtpIp4.Text;
            }
            set
            {
                tbNtpIp4.Text = value;
                _lastValidIpv4Ntp = value;
            }
        }

        /// <summary>
        /// DNS address entered by operator (IP4)
        /// </summary>
        public string DnsIpv4
        {
            get
            {
                return tbDnsIp4.Text;
            }
            set
            {
                tbDnsIp4.Text = value;
                _lastValidIpv4Dns = value;
            }
        }

        /// <summary>
        /// Gateway entered by operator (IP4)
        /// </summary>
        public string GatewayIpv4
        {
            get
            {
                return tbGatewayIpv4.Text;
            }
            set
            {
                tbGatewayIpv4.Text = value;
                _lastValidIpv4Gateway = value;
            }
        }

        /// <summary>
        /// Gateway entered by operator (IP6)
        /// </summary>
        public string GatewayIpv6
        {
            get
            {
                return tbGatewayIpv6.Text;
            }
            set
            {
                tbGatewayIpv6.Text = value;
                _lastValidIpv6Gateway = value;
            }
        }

        /// <summary>
        /// NTP address entered by operator (IP6)
        /// </summary>
        public string NtpIpv6
        {
            get
            {
                return tbNtpIp6.Text;
            }
            set
            {
                tbNtpIp6.Text = value;
                _lastValidIpv6Ntp = value;
            }
        }

        /// <summary>
        /// DNS address entered by operator (IP6)
        /// </summary>
        public string DnsIpv6
        {
            get
            {
                return tbDnsIp6.Text;
            }
            set
            {
                tbDnsIp6.Text = value;
                _lastValidIpv6Dns = value;
            }
        }
        
        /// <summary>
        /// "Use embedde passwords" selection
        /// </summary>
        public bool UseEmbeddedPassword
        {
            get
            {
                return rbEmbeddedPasswords.Checked;
            }
            set
            {
                rbEmbeddedPasswords.Checked = value;
                rbOwnPasswords.Checked = !value;
                if (value)
                {
                    tbPassword1.Text = "OnvifTest123";
                    tbPassword2.Text = "OnvifTest321";
                }

                rbEmbeddedPasswords.TabStop = true;
                rbOwnPasswords.TabStop = true;
                EnablePasswords(_currentState);
            }
        }

        /// <summary>
        /// First user-defined password for security tests.
        /// </summary>
        public string Password1
        {
            get
            {
                return tbPassword1.Text;
            }
            set
            {
                tbPassword1.Text = value;
            }
        }

        /// <summary>
        /// Second user-defined password for security tests.
        /// </summary>
        public string Password2
        {
            get
            {
                return tbPassword2.Text;
            }
            set
            {
                tbPassword2.Text = value;
            }
        }

        /// <summary>
        /// PTZ node token selected.
        /// </summary>
        public string PTZNodeToken
        {
            get
            {
                return cmbPTZNodes.Text;
            }
            set
            {
                cmbPTZNodes.Text = value;
            }
        }

        /// <summary>
        /// Operation delay (for operation like getting address from DHCP)
        /// </summary>
        public int OperationDelay
        {
            get
            {
                int result;
                int.TryParse(tbOperationDelay.Text, out result);
                return result;
            }
            set
            {
                _lastValidOperationDelay = value;
                tbOperationDelay.Text = value.ToString();
            }
        }

        public int RecoveryDelay
        {
            get
            {
                int result;
                int.TryParse(tbRecoveryDelay.Text, out result);
                return result;
            }
            set
            {
                _lastValidRecoveryDelay = value;
                tbRecoveryDelay.Text = value.ToString();
            }
        }


        /// <summary>
        /// Sets PTZ nodes
        /// </summary>
        /// <param name="nodes">PTZ nodes information</param>
        public void SetPTZNodes(Proxies.Onvif.PTZNode[] nodes)
        {
            cmbPTZNodes.Items.Clear();
            foreach (Proxies.Onvif.PTZNode node in nodes)
            {
                cmbPTZNodes.Items.Add(node.token);
            }
            if (cmbPTZNodes.Items.Count > 0)
            {
                cmbPTZNodes.SelectedIndex = 0;
            }
        }
        
        #endregion

        #region Validation

        private int _lastValidMessageTimeout;

        /// <summary>
        /// Validates message timeout. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMessageTimeout_Validating(object sender, CancelEventArgs e)
        {
            int timeout = -1;
            bool bOk = int.TryParse(tbMessageTimeout.Text, out timeout);
            if (bOk)
            {
                bOk = timeout > 0;
            }

            if (!bOk)
            {
                tbMessageTimeout.Text = _lastValidMessageTimeout.ToString();
            }
            else
            {
                _lastValidMessageTimeout = timeout;
            }
        }

        private int _lastValidRebootTimeout;

        /// <summary>
        /// Validates reboot timeout. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRebootTimeout_Validating(object sender, CancelEventArgs e)
        {
            int timeout = 0;
            bool bOk = int.TryParse(tbRebootTimeout.Text, out timeout);
            if (bOk)
            {
                bOk = timeout > 0;
            }
            if (!bOk)
            {
                tbRebootTimeout.Text = _lastValidRebootTimeout.ToString();
            }
            else
            {
                _lastValidRebootTimeout = timeout;
            }
        }

        private int _lastValidTimeBetweenTests;

        /// <summary>
        /// Validates time between tests. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTimeBetweenTests_Validating(object sender, CancelEventArgs e)
        {
            int time = 0;
            bool bOk = int.TryParse(tbTimeBetweenTests.Text, out time);
            if (bOk)
            {
                bOk = time >= 0;
            }
            if (!bOk)
            {
                tbTimeBetweenTests.Text = _lastValidTimeBetweenTests.ToString();
            }
            else
            {
                _lastValidTimeBetweenTests = time;
            }
        }

        private int _lastValidOperationDelay;

        /// <summary>
        /// Validates operation delay. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbOperationDelay_Validating(object sender, CancelEventArgs e)
        {
            int time = 0;
            bool bOk = int.TryParse(tbOperationDelay.Text, out time);
            if (bOk)
            {
                bOk = time >= 0;
            }
            if (!bOk)
            {
                tbOperationDelay.Text = _lastValidOperationDelay.ToString();
            }
            else
            {
                _lastValidOperationDelay = time;
            }
        }


        private int _lastValidRecoveryDelay;

        /// <summary>
        /// Validates operation delay. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRecoveryDelay_Validating(object sender, CancelEventArgs e)
        {
            int time = 0;
            bool bOk = int.TryParse(tbRecoveryDelay.Text, out time);
            if (bOk)
            {
                bOk = time >= 0;
            }
            if (!bOk)
            {
                tbRecoveryDelay.Text = _lastValidRecoveryDelay.ToString();
            }
            else
            {
                _lastValidRecoveryDelay = time;
            }
        }

        private string _lastValidIpv4Dns;
        private string _lastValidIpv6Dns;
        private string _lastValidIpv4Ntp;
        private string _lastValidIpv6Ntp;
        private string _lastValidIpv4Gateway;
        private string _lastValidIpv6Gateway;

        /// <summary>
        /// Validates IP address. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDnsIp4_Validating(object sender, CancelEventArgs e)
        {
            IPAddress address;
            bool bOk = IPAddress.TryParse(tbDnsIp4.Text, out address);
            if (bOk)
            {
                bOk = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }

            if (!bOk)
            {
                tbDnsIp4.Text = _lastValidIpv4Dns;
            }
            else
            {
                _lastValidIpv4Dns = tbDnsIp4.Text;
            }
        }

        /// <summary>
        /// Validates IP address. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbNtpIp4_Validating(object sender, CancelEventArgs e)
        {
            IPAddress address;
            bool bOk = IPAddress.TryParse(tbNtpIp4.Text, out address);
            if (bOk)
            {
                bOk = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }

            if (!bOk)
            {
                tbNtpIp4.Text = _lastValidIpv4Ntp;
            }
            else
            {
                _lastValidIpv4Ntp = tbDnsIp4.Text;
            }
        }

        /// <summary>
        /// Validates IP address. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDnsIp6_Validating(object sender, CancelEventArgs e)
        {
            IPAddress address;
            bool bOk = IPAddress.TryParse(tbDnsIp6.Text, out address);
            if (bOk)
            {
                bOk = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
            }

            if (!bOk)
            {
                tbDnsIp6.Text = _lastValidIpv6Dns;
            }
            else
            {
                _lastValidIpv6Dns = tbDnsIp6.Text;
            }
        }

        /// <summary>
        /// Validates IP address. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbNtpIp6_Validating(object sender, CancelEventArgs e)
        {
            IPAddress address;
            bool bOk = IPAddress.TryParse(tbNtpIp6.Text, out address);
            if (bOk)
            {
                bOk = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
            }

            if (!bOk)
            {
                tbNtpIp6.Text = _lastValidIpv6Ntp;
            }
            else
            {
                _lastValidIpv6Ntp = tbDnsIp6.Text;
            }
        }

        /// <summary>
        /// Validates IP address. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbGatewayIpv4_Validating(object sender, CancelEventArgs e)
        {
            IPAddress address;
            bool bOk = IPAddress.TryParse(tbGatewayIpv4.Text, out address);
            if (bOk)
            {
                bOk = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }

            if (!bOk)
            {
                tbGatewayIpv4.Text = _lastValidIpv4Gateway;
            }
            else
            {
                _lastValidIpv4Gateway = tbGatewayIpv4.Text;
            }
        }

        /// <summary>
        /// Validates IP address. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbGatewayIpv6_Validating(object sender, CancelEventArgs e)
        {
            IPAddress address;
            bool bOk = IPAddress.TryParse(tbGatewayIpv6.Text, out address);
            if (bOk)
            {
                bOk = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
            }

            if (!bOk)
            {
                tbGatewayIpv6.Text = _lastValidIpv6Gateway;
            }
            else
            {
                _lastValidIpv6Gateway = tbGatewayIpv6.Text;
            }
        }

        #endregion

        #region Features
        
        private FeaturesSet _featuresSet;

        private bool _featuresReadOnly;

        Dictionary<Feature, TreeNode> _featureNodes = new Dictionary<Feature, TreeNode>();

        private string MANDATORYFEATURE = "MANDATORY for current selection";
        private string OPTIONALFEATURE = "OPTIONAL for current selection";
        private string DISABLEDFEATURE = "DISABLED for current selection";

        #region Tree building

        /// <summary>
        /// Builds features tree.
        /// This operatio is performed when control is created.
        /// </summary>
        void BuildFeaturesTree()
        {
            _featureNodes.Clear();
            _featuresSet = FeaturesHelper.CreateFeaturesSet();

            foreach (FeatureNode node in _featuresSet.Nodes)
            {
                AddFeatureNode(null, node);
            }

            tvFeatures.ExpandAll();
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

            TransmitProperties(node, treeNode);

            if (parent != null)
            {
                parent.Nodes.Add(treeNode);
            }
            else
            {
                tvFeatures.Nodes.Add(treeNode);
            }

            if (node.Feature != Feature.None)
            {
                _featureNodes.Add(node.Feature, treeNode);
            }

            foreach (FeatureNode child in node.Nodes)
            {
                AddFeatureNode(treeNode, child);
            }
        }
        
        /// <summary>
        /// Sets up tree node properties accordingly to feature node properties.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="treeNode"></param>
        void TransmitProperties(FeatureNode node, TreeNode treeNode)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Set properties for {0} node. Selected: {1}", node.DisplayName, node.Checked));

            treeNode.Checked = node.Checked;
            HighlightNode(treeNode, node.Enabled);

            treeNode.ImageKey = node.State.ToString();
            treeNode.SelectedImageKey = treeNode.ImageKey;

            switch (node.State)
            {
                case FeatureState.Mandatory:
                    treeNode.ToolTipText = MANDATORYFEATURE;
                    break;
                case FeatureState.Optional:
                    treeNode.ToolTipText = OPTIONALFEATURE;
                    break;
                case FeatureState.Undefined:
                    treeNode.ToolTipText = DISABLEDFEATURE;
                    break;
            }

        }

        #endregion

        /// <summary>
        /// Changes "disabled" node color to gray.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="bEnabled"></param>
        void HighlightNode(TreeNode node, bool bEnabled)
        {
            if (bEnabled)
            {
                node.ForeColor = tvFeatures.ForeColor;
            }
            else
            {
                node.ForeColor = Color.DarkGray;
            }
        }

        /// <summary>
        /// Forbids features selection/deselection when necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvFeatures_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (_featuresReadOnly)
            {
                e.Cancel = true;
                return;
            }

            if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
            {
                TreeNode node = e.Node;
                FeatureNode featureNode = (FeatureNode)node.Tag;
                if (featureNode.State != FeatureState.Optional || featureNode.Enabled == false)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Handles service/feature selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvFeatures_AfterCheck(object sender, TreeViewEventArgs e)
        {

            TreeNode node = e.Node;
            FeatureNode featureNode = (FeatureNode)node.Tag;

            System.Diagnostics.Debug.WriteLine(
                string.Format("tvFeatures_AfterCheck [processing] {0}", node.Text));

            // update list of selected features
            if (node.Checked)
            {
               if (!_features.Contains(featureNode.Feature))
                {
                    _features.Add(featureNode.Feature);
                }            
            }
            else
            {
                _features.RemoveAll( v => (v == featureNode.Feature));
            }
            
            if (e.Action != TreeViewAction.ByMouse && e.Action != TreeViewAction.ByKeyboard)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} checked automatically - skip part of processing", featureNode.DisplayName));
                return;
            }

            System.Diagnostics.Debug.WriteLine(string.Format(" --- check feature {0}", featureNode.DisplayName));
            featureNode.Checked = node.Checked; 
            
            FeaturesSet.UpdateChildFeatures(featureNode); 
            SetupChildFeatures(featureNode);
            
            // for PTZ Configurable Home / Fixed Home:
            // only one feature can be implemented
            if (featureNode.Feature == Feature.PTZConfigurableHome && node.Checked)
            {
                _featureNodes[Feature.PTZFixedHome].Checked = false;
            }
            else if (featureNode.Feature == Feature.PTZFixedHome && node.Checked)
            {
                _featureNodes[Feature.PTZConfigurableHome].Checked = false;
            }
            else if (featureNode.Feature == Feature.PTZHome && featureNode.Checked)
            {
                if (! (_featureNodes[Feature.PTZConfigurableHome].Checked ||  _featureNodes[Feature.PTZFixedHome].Checked) )
                {
                    _featureNodes[Feature.PTZConfigurableHome].Checked = true;
                }
            }
        }
       
        /// <summary>
        /// Sets up child features
        /// </summary>
        /// <param name="node"></param>
        void SetupChildFeatures(FeatureNode node)
        {
            foreach (FeatureNode child in node.Nodes)
            {
                TreeNode treeNode = _featureNodes[child.Feature];
                TransmitProperties(child, treeNode);
                SetupChildFeatures(child);
            }
        }

        #endregion

        /// <summary>
        /// Is raised when "Select Tests" button is clicked.
        /// </summary>
        public event EventHandler SelectTests;

        private void btnSelectTests_Click(object sender, EventArgs e)
        {
            if (SelectTests != null)
            {
                SelectTests(this, e);
            }
        }

        /// <summary>
        /// Is raised when "Get PTZ nodes" button is clicked.
        /// </summary>
        public event EventHandler  GetPTZNodes;

        private void btnGetPTZNodes_Click(object sender, EventArgs e)
        {
            if (GetPTZNodes != null)
            {
                GetPTZNodes(this, e);
            }

        }
        
        private string _password1;
        
        private string _password2;

        private void rbEmbeddedPasswords_CheckedChanged(object sender, EventArgs e)
        {
            rbEmbeddedPasswords.TabStop = true;
            rbOwnPasswords.TabStop = true;
            EnablePasswords(_currentState);

            if (rbEmbeddedPasswords.Checked)
            {
                _password1 = tbPassword1.Text;
                _password2 = tbPassword2.Text;

                tbPassword1.Text = "OnvifTest123";
                tbPassword2.Text = "OnvifTest321";
            }
            else
            {
                tbPassword1.Text = _password1;
                tbPassword2.Text = _password2;
            }

        }




    }
}
