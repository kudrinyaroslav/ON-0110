using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.UI;
using IPAddress = System.Net.IPAddress;

namespace TestTool.GUI.Controls
{
    /// <summary>
    /// Control with test settings at the "Management" tab.
    /// </summary>
    partial class SettingsControl : Page, ITestSettingsView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControl()
        {
            InitializeComponent();
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
                tbSafetyDelay, 
                cmbSecureMethod, 
                cmbEventTopic, 
                cmbVideoSource,
                tbRelayOutputsDelayMonostable,
                btnGetTopics,
                btnGetVideoSources,
                tbSubscriptionTimeout, 
                tbNamespaces, 
                tbTimeout, 
                cmbRecordingToken});

            foreach (SettingsTabPage page in _pages.Values)
            {
                page.Disable();
            }
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
                tbSafetyDelay, 
                cmbSecureMethod, 
                cmbEventTopic, 
                cmbVideoSource,
                tbRelayOutputsDelayMonostable,
                btnGetTopics,
                btnGetVideoSources,
                tbSubscriptionTimeout, 
                tbNamespaces, 
                tbTimeout, 
                cmbRecordingToken});

            foreach (SettingsTabPage page in _pages.Values)
            {
                page.Enable();
            }
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

        /// <summary>
        /// Time after query
        /// </summary>
        public int RecoveryDelay
        {
            get
            {
                int result;
                int.TryParse(tbSafetyDelay.Text, out result);
                return result;
            }
            set
            {
                _lastValidSafetyDelay = value;
                tbSafetyDelay.Text = value.ToString();
            }
        }
        
        public int SearchTimeout
        {
            get
            {
                int result;
                int.TryParse(tbTimeout.Text, out result);
                return result;
            }
            set
            {
                _lastValidSearchTimeout = value;
                tbTimeout.Text = value.ToString();
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
                if (!cmbPTZNodes.Items.Contains(value) && !string.IsNullOrEmpty(value))
                {
                    cmbPTZNodes.Items.Add(value);
                }
                cmbPTZNodes.Text = value;
            }
        }

        /// <summary>
        /// Video source token selected.
        /// </summary>
        public string VideoSourceToken
        {
            get
            {
                return cmbVideoSource.Text;
            }
            set
            {
                if (!cmbVideoSource.Items.Contains(value) && !string.IsNullOrEmpty(value))
                {
                    cmbVideoSource.Items.Add(value);
                }
                cmbVideoSource.Text = value;
            }
        }

        public string RecordingToken
        {
            get
            {
                return cmbRecordingToken.Text;
            }
            set
            {
                if (!cmbRecordingToken.Items.Contains(value) && !string.IsNullOrEmpty(value))
                {
                    cmbRecordingToken.Items.Add(value);
                }
                cmbRecordingToken.Text = value;
            }
        }

        /// <summary>
        /// Method for testing security operations
        /// </summary>
        public string SecureMethod
        {
            get
            {
                return cmbSecureMethod.Text;
            }
            set
            {
                cmbSecureMethod.Text = value;
            }
        }

        /// <summary>
        /// Subscription timeout
        /// </summary>
        public int SubscriptionTimeout
        {
            get
            {
                int result;
                int.TryParse(tbSubscriptionTimeout.Text, out result);
                return result;
            }
            set
            {
                _lastValidSubscriptionTimeout = value;
                tbSubscriptionTimeout.Text = value.ToString();
            }
        }
        public int RelayOutputDelayTimeMonostable
        {
            get
            {
                int result;
                int.TryParse(tbRelayOutputsDelayMonostable.Text, out result);
                return result;
            }
            set
            {
                _lastValidRelayOutputDelayMonostable = value;
                tbRelayOutputsDelayMonostable.Text = value.ToString();
            }
        }

        /// <summary>
        /// Event topic for subscription filter tests
        /// </summary>
        public string EventTopic
        {
            get
            {
                return cmbEventTopic.Text;
            }
            set
            {
                cmbEventTopic.Text = value;
            }
        }

        /// <summary>
        /// Topic namespace definitions
        /// </summary>
        public string TopicNamespaces
        {
            get
            {
                return tbNamespaces.Text.TrimEnd();
            }
            set
            {
                tbNamespaces.Text = value;
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

        /// <summary>
        /// Sets PTZ nodes
        /// </summary>
        /// <param name="nodes">PTZ nodes information</param>
        public void SetPTZNodes(Proxies.Onvif.PTZNode[] nodes)
        {
            Invoke(new Action(
                       () =>
                           {

                               string backup = cmbPTZNodes.Text;

                               cmbPTZNodes.Items.Clear();
                               foreach (Proxies.Onvif.PTZNode node in nodes)
                               {
                                   cmbPTZNodes.Items.Add(node.token);
                               }
                               if (cmbPTZNodes.Items.Count > 0)
                               {
                                   if (cmbPTZNodes.Items.Contains(backup))
                                   {
                                       cmbPTZNodes.SelectedItem = backup;
                                   }
                                   else
                                   {
                                       cmbPTZNodes.SelectedIndex = 0;
                                   }
                               }
                           }));
        }

        /// <summary>
        /// Sets video sources
        /// </summary>
        /// <param name="nodes">Video sources information</param>
        public void SetVideoSources(Proxies.Onvif.VideoSource[] sources)
        {
            Invoke(new Action(
                       () =>
                           {

                               string backup = cmbVideoSource.Text;

                               cmbVideoSource.Items.Clear();
                               foreach (Proxies.Onvif.VideoSource source in sources)
                               {
                                   cmbVideoSource.Items.Add(source.token);
                               }
                               if (cmbVideoSource.Items.Count > 0)
                               {
                                   if (cmbVideoSource.Items.Contains(backup))
                                   {
                                       cmbVideoSource.SelectedItem = backup;
                                   }
                                   else
                                   {
                                       cmbVideoSource.SelectedIndex = 0;
                                   }
                               }
                           }));
        }

        public void SetSecureMethods(IEnumerable<string> methods)
        {
            cmbSecureMethod.DataSource = methods;
        }

        public void SetEventsTopic(List<EventsTopicInfo> topics)
        {
            Invoke(new Action(() =>
            {
                cmbEventTopic.DisplayMember = "Topic";
                cmbEventTopic.DataSource = topics;
            }));
        }

        public List<object> AdvancedSettings
        {
            get
            {
                List<object> settings = new List<object>();
                if (_pages != null)
                {
                    foreach (SettingsTabPage page in _pages.Values)
                    {
                        settings.Add(page.Parameters);
                    }
                }
                return settings;
            }
            set
            {
                if (value != null)
                {
                    foreach (object settings in value)
                    {
                        string id = settings.GetType().GUID.ToString();
                        if (_pages.ContainsKey(id))
                        {
                            _pages[id].Parameters = settings;
                        }
                    }
                }
                else
                {
                    foreach (SettingsTabPage page in _pages.Values)
                    {
                        page.Clear();
                    }
                }
            }
        }

        #endregion

        private Dictionary<string, SettingsTabPage> _pages;
        public void AddSettingsPages(List<SettingsTabPage> pages)
        {
            _pages = new Dictionary<string, SettingsTabPage>();
            foreach (SettingsTabPage page in pages.OrderBy(P => P.Order))
            {
                TabPage tabPage = new TabPage(page.PageName);
                tabPage.Name = page.ParametersType.GUID.ToString();
                tabPage.Controls.Add(page);
                tcMiscSettings.TabPages.Add(tabPage);
                _pages.Add(page.ParametersType.GUID.ToString(), page);
            }
        }

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
            int timeout = -1;
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
            int time = -1;
            bool bOk = int.TryParse(tbTimeBetweenTests.Text, out time);
            if (bOk)
            {
                bOk = time > 0;
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

        private int _lastValidSafetyDelay;

        /// <summary>
        /// Validates operation delay. Forbids entering invalid value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSafetyDelay_Validating(object sender, CancelEventArgs e)
        {
            int time = 0;
            bool bOk = int.TryParse(tbSafetyDelay.Text, out time);
            if (bOk)
            {
                bOk = time >= 0;
            }
            if (!bOk)
            {
                tbSafetyDelay.Text = _lastValidSafetyDelay.ToString();
            }
            else
            {
                _lastValidSafetyDelay = time;
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

        private int _lastValidSubscriptionTimeout;

        private void tbSubscriptionTimeout_Validating(object sender, CancelEventArgs e)
        {
            int time = 0;
            bool bOk = int.TryParse(tbSubscriptionTimeout.Text, out time);
            if (bOk)
            {
                bOk = time > 0;
            }
            if (!bOk)
            {
                tbSubscriptionTimeout.Text = _lastValidSubscriptionTimeout.ToString();
            }
            else
            {
                _lastValidSubscriptionTimeout = time;
            }
        }
        
        private int _lastValidRelayOutputDelayMonostable;

        private void tbRelayOutputDelayTimeMonostable_Validating(object sender, CancelEventArgs e)
        {
            int time = 0;
            bool bOk = int.TryParse(tbRelayOutputsDelayMonostable.Text, out time);
            if (bOk)
            {
                bOk = time > 0;
            }
            if (!bOk)
            {
                tbRelayOutputsDelayMonostable.Text = _lastValidRelayOutputDelayMonostable.ToString();
            }
            else
            {
                _lastValidRelayOutputDelayMonostable = time;
            }
        }

        private void tbNamespaces_Leave(object sender, EventArgs e)
        {
            string[] namespaces = tbNamespaces.Text.Replace(Environment.NewLine, "").Split(' ');

            bool valid = true;
            foreach (string definition in namespaces)
            {
                if (!string.IsNullOrEmpty(definition) && !definition.Contains("="))
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                MessageBox.Show(
                    "Namespaces definition is incorrect. Should be in form \"prefix1=\"namespace1\"[ prefix2=\"namespace2\"]\" ");
            }
        }

        private int _lastValidSearchTimeout;

        private void tbTimeout_Validating(object sender, CancelEventArgs e)
        {
            int time = 0;
            bool bOk = int.TryParse(tbTimeout.Text, out time);
            if (bOk)
            {
                bOk = time > 0;
            }
            if (!bOk)
            {
                tbTimeout.Text = _lastValidSearchTimeout.ToString();
            }
            else
            {
                _lastValidSearchTimeout = time;
            }
        }

        #endregion

        /// <summary>
        /// Is raised when "Get PTZ nodes" button is clicked.
        /// </summary>
        public event EventHandler GetPTZNodes;

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

        private void btnGetTopics_Click(object sender, EventArgs e)
        {
            if (GetEventTopics != null)
            {
                GetEventTopics(this, e);
            }
        }

        public event EventHandler GetEventTopics;

        private void cmbEventTopic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEventTopic.SelectedIndex >= 0)
            {
                EventsTopicInfo topic = (EventsTopicInfo)cmbEventTopic.SelectedItem;
                tbNamespaces.Text = topic.NamespacesDefinition;
            }
        }

        public event EventHandler GetVideoSources;

        private void btnGetVideoSources_Click(object sender, EventArgs e)
        {
            if (GetVideoSources != null)
            {
                GetVideoSources(this, e);
            }
        }


        #region IView Members

        public void SwitchToState(TestTool.GUI.Enums.ApplicationState state)
        {
            SetCurrentState(state);
        }

        public Controllers.IController GetController()
        {
            return null;
        }

        #endregion

        public event EventHandler GetRecordings;

        private void btnGetRecordings_Click(object sender, EventArgs e)
        {
            if (GetRecordings != null)
            {
                GetRecordings(this, e);
            }
        }

        public void SetRecordings(IEnumerable<string> recordings)
        {
            Invoke(new Action( 
                () =>
                    {
                        string backup = cmbRecordingToken.Text;

                        cmbRecordingToken.Items.Clear();
                        foreach (string recording in recordings)
                        {
                            cmbRecordingToken.Items.Add(recording);
                        }
                        if (cmbRecordingToken.Items.Count > 0)
                        {
                            if (cmbRecordingToken.Items.Contains(backup))
                            {
                                cmbRecordingToken.SelectedItem = backup;
                            }
                            else
                            {
                                cmbRecordingToken.SelectedIndex = 0;
                            }
                        }                        

                    }));

        }

    }
}
