using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMC.Controls;
using SMC.Proxies;
using SMC.Proxies.Configuration;

namespace SMC.Pages
{
    public partial class EventsManagement : BaseSmcControl
    {
        public EventsManagement()
        {
            InitializeComponent();

            CreateParameterControls();
        }


        Controls.ParameterInput<ComboBox> _accessPointTokenSelector;
        Controls.ParameterInput<ComboBox> _areaTokenSelector;
        Controls.ParameterInput<ComboBox> _doorTokenSelector;
        Controls.ParameterInput<ComboBox> _scheduleTokenSelector;
        Controls.ParameterInput<ComboBox> _specialDayGroupTokenSelector;
        Controls.ParameterInput<ComboBox> _credentialsTokenSelector;
        Controls.ParameterInput<ComboBox> _accessProfileTokenSelector;
        Controls.ParameterInput<TextBox> _holderNameInput;
        Controls.ParameterInput<TextBox> _reasonInput;
        Controls.ParameterInput<TextBox> _cardInput;
        Controls.ParameterInput<CheckBox> _activeStateInput;
        Controls.ParameterInput<CheckBox> _booleanInput;
        Controls.ParameterInput<CheckBox> _booleanExternal;
        Controls.ParameterInput<CheckBox> _booleanApbViolation;
        Controls.ParameterInput<CheckBox> _booleanClientUpdated;
        Controls.ParameterInput<CheckBox> _booleanState;
        

        Dictionary<string, ParameterInput> _inputs = new Dictionary<string, ParameterInput>();


        List<ParameterInput> _parameterControls = new List<ParameterInput>();

        void CreateParameterControls()
        {
            _accessPointTokenSelector = new ParameterInput<ComboBox>("AccessPointToken", "AccessPoint", new ComboBox() { Width = 200 });
            _accessProfileTokenSelector = new ParameterInput<ComboBox>("AccessProfileToken", "AccessProfile", new ComboBox() { Width = 200 });
            _areaTokenSelector = new ParameterInput<ComboBox>("AreaToken", "Area", new ComboBox() { Width = 200 });
            _credentialsTokenSelector = new ParameterInput<ComboBox>("CredentialToken", "Credentials", new ComboBox() { Width = 200 });
            _doorTokenSelector = new ParameterInput<ComboBox>("DoorToken", "Door", new ComboBox() { Width = 200 });
            _scheduleTokenSelector = new ParameterInput<ComboBox>("ScheduleToken", "Schedule", new ComboBox() { Width = 200 });
            _specialDayGroupTokenSelector = new ParameterInput<ComboBox>("SpecialDaysToken", "SpecialDays", new ComboBox() { Width = 200 });

            _credentialsTokenSelector.Input.SelectedValueChanged += new EventHandler(Input_SelectedValueChanged);

            _holderNameInput = new ParameterInput<TextBox>("CredentialHolderName", "Credential Holder Name", new TextBox() { Width = 200 });
            _reasonInput = new ParameterInput<TextBox>("Reason", "Reason", new TextBox() { Width = 200 });
            _cardInput = new ParameterInput<TextBox>("Card", "Card", new TextBox() { Width = 200 });
            _activeStateInput = new ParameterInput<CheckBox>("Active", "Active", new CheckBox() { Width = 200 });
            _booleanInput = new ParameterInput<CheckBox>("Enabled", "Enabled", new CheckBox() { Width = 200 });
            
            _booleanExternal = new ParameterInput<CheckBox>("External", "External", new CheckBox() { Width = 200 });
            _booleanApbViolation = new ParameterInput<CheckBox>("ApbViolation", "ApbViolation", new CheckBox() { Width = 200 });
            _booleanClientUpdated = new ParameterInput<CheckBox>("ClientUpdated", "ClientUpdated", new CheckBox() { Width = 200 });
            _booleanState = new ParameterInput<CheckBox>("State", "State", new CheckBox() { Width = 200 });

            _parameterControls.AddRange(new ParameterInput[]
                                                {
                                                    _accessPointTokenSelector,
                                                    _accessProfileTokenSelector,
                                                    _areaTokenSelector,
                                                    _doorTokenSelector,
                                                    _credentialsTokenSelector,
                                                    _scheduleTokenSelector,
                                                    _specialDayGroupTokenSelector,
                                                    //_accessControllerTokenSelector,
                                                    _holderNameInput,
                                                    _reasonInput,
                                                    _cardInput,
                                                    _activeStateInput,
                                                    _booleanInput,
                                                    _booleanExternal,
                                                    _booleanState,
                                                    _booleanApbViolation,
                                                    _booleanClientUpdated
                                                }

                );

            foreach (ParameterInput ctrl in _parameterControls)
            {
                ctrl.Width = 400;
                ctrl.Visible = false;
                _inputs.Add(ctrl.ParameterName, ctrl);
            }

            flpParameters.Controls.AddRange(_parameterControls.ToArray());
        }

        void Input_SelectedValueChanged(object sender, EventArgs e)
        {
            _holderNameInput.Input.Text = ((CredentialInfo)_credentialsTokenSelector.Input.SelectedItem).CredentialHolderReference;
        }
        
        private EventControlServiceSoapClient _eventControlClient;

        protected EventControlServiceSoapClient EventControlClient
        {
            get
            {
                if (_eventControlClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.EventControlServiceAddress);
                    _eventControlClient = new EventControlServiceSoapClient(binding, address);
                }
                return _eventControlClient;
            }

        }

        private ConfigurationServiceSoapClient _configurationServiceSoapClient;

        protected ConfigurationServiceSoapClient ConfigurationServiceClient
        {
            get
            {
                if (_configurationServiceSoapClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.ConfigurationServiceAddress);
                    _configurationServiceSoapClient = new ConfigurationServiceSoapClient(binding, address);
                }
                return _configurationServiceSoapClient;
            }

        }

        private PACSPortClient _pacsClient;

        protected PACSPortClient PACSClient
        {
            get
            {
                if (_pacsClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SmcContext.General.PACSServiceAddress);
                    _pacsClient = new PACSPortClient(binding, address);
                }
                return _pacsClient;
            }

        }

        private AccessRulesPortClient _accessRulesClient;

        protected AccessRulesPortClient AccessRulesClient
        {
            get
            {
                if (_accessRulesClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SmcContext.General.AccessRulesServiceAddress);
                    _accessRulesClient = new AccessRulesPortClient(binding, address);
                }
                return _accessRulesClient;
            }

        }

        private CredentialPortClient _credentialClient;

        protected CredentialPortClient CredentialClient
        {
            get
            {
                if (_credentialClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SmcContext.General.CredentialServiceAddress);
                    _credentialClient = new CredentialPortClient(binding, address);
                }
                return _credentialClient;
            }

        }

        private DoorControlPortClient _doorControlClient;

        protected DoorControlPortClient DoorControlClient
        {
            get
            {
                if (_doorControlClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.ManagementServiceAddress);
                    _doorControlClient = new DoorControlPortClient(binding, address);
                }
                return _doorControlClient;
            }

        }

        private SchedulePortClient _schedulePortControlClient;

        protected SchedulePortClient SchedulePortControlClient
        {
            get
            {
                if (_schedulePortControlClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.ScheduleServiceAddress);
                    _schedulePortControlClient = new SchedulePortClient(binding, address);
                }
                return _schedulePortControlClient;
            }

        }
        

        protected override void UpdateAddress()
        {
            if (_eventControlClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.EventControlServiceAddress;
                if (address != _eventControlClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _eventControlClient = null;
                }
            }
            if (_pacsClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.PACSServiceAddress;
                if (address != _pacsClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _pacsClient = null;
                }
            }
            if (_doorControlClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.ManagementServiceAddress;
                if (address != _doorControlClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _doorControlClient = null;
                }
            }
            if (_schedulePortControlClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.ScheduleServiceAddress;
                if (address != _schedulePortControlClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _schedulePortControlClient = null;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SafeInvoke( () =>
                            {
                                TopicInformation[] topicSet = EventControlClient.GetTopics();
                                DisplayTopicSet(topicSet);

                                //AreaInfo[] areas = PACSClient.GetAreaInfo(null);
                                List<AreaInfo> areas = GetList<AreaInfo>(PACSClient.GetAreaInfoList);
                                _areaTokenSelector.Input.DataSource = areas;
                                _areaTokenSelector.Input.DisplayMember = "token";

                                //AccessPointInfo[] points = PACSClient.GetAccessPointInfo(null);
                                List<AccessPointInfo> points = GetList<AccessPointInfo>(PACSClient.GetAccessPointInfoList);
                                _accessPointTokenSelector.Input.DataSource = points;
                                _accessPointTokenSelector.Input.DisplayMember = "token";

                                //AccessProfileInfo[] accessProfiles = ConfigurationServiceClient.GetAccessProfileInfo();
                                List<AccessProfileInfo> accessProfiles = GetList<AccessProfileInfo>(AccessRulesClient.GetAccessProfileInfoList);
                                _accessProfileTokenSelector.Input.DataSource = accessProfiles;
                                _accessProfileTokenSelector.Input.DisplayMember = "token";

                                //CredentialInfo[] credentials = ConfigurationServiceClient.GetCredentialInfo();
                                List<CredentialInfo> credentials = GetList<CredentialInfo>(CredentialClient.GetCredentialInfoList);
                                _credentialsTokenSelector.Input.DataSource = credentials;
                                _credentialsTokenSelector.Input.DisplayMember = "token";

                                //DoorInfo[] doors = DoorControlClient.GetDoorInfo(null);
                                List<DoorInfo> doors = GetList<DoorInfo>(DoorControlClient.GetDoorInfoList);
                                _doorTokenSelector.Input.DataSource = doors;
                                _doorTokenSelector.Input.DisplayMember = "token";

                                //ScheduleInfo[] schedules = SchedulePortControlClient.GetScheduleInfo(null);
                                List<ScheduleInfo> schedules = GetList<ScheduleInfo>(SchedulePortControlClient.GetScheduleInfoList);
                                _scheduleTokenSelector.Input.DataSource = schedules;
                                _scheduleTokenSelector.Input.DisplayMember = "token";

                                //SpecialDayGroupInfo[] specialDayGroups = SchedulePortControlClient.GetSpecialDayGroupInfo(null);
                                List<SpecialDayGroupInfo> specialDayGroups = GetList<SpecialDayGroupInfo>(SchedulePortControlClient.GetSpecialDayGroupInfoList);
                                _specialDayGroupTokenSelector.Input.DataSource = specialDayGroups;
                                _specialDayGroupTokenSelector.Input.DisplayMember = "token";
                            });
        }


        void DisplayTopicSet(TopicInformation[] topicSet)
        {
            tvTopics.Nodes.Clear();
            List<TopicInformation> ordered = topicSet.OrderBy(TI => TI.TopicString).ToList();

            Dictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();

            foreach (TopicInformation topic in ordered)
            {
                DisplayTopic(topic, nodes);
            }
        }

        void DisplayTopic(TopicInformation topic, Dictionary<string, TreeNode> nodes)
        {
            System.Diagnostics.Debug.WriteLine("Add " + topic.TopicString);

            TreeNode parentNode = null;
            if (!string.IsNullOrEmpty(topic.ParentTopicString) && nodes.ContainsKey(topic.ParentTopicString))
            {
                parentNode = nodes[topic.ParentTopicString];
            }

            string displayName = string.Empty;
            string[] topicSegments = topic.TopicString.Split('/');
            displayName = topicSegments[topicSegments.Length - 1];
            
            TreeNode node = new TreeNode(displayName);
            node.Tag = topic;

            node.ImageKey = topic.IsTopic ? ( topic.IsProperty ? "PropertyEvent" : "Topic" ): "TopicsNamespace";
            node.SelectedImageKey = node.ImageKey;

            if (parentNode == null)
            {
                tvTopics.Nodes.Add(node);
            }
            else
            {
                parentNode.Nodes.Add(node);
            }
            nodes.Add(topic.TopicString, node);
        }

        private bool _allParametersSupported = false;

        private void tvTopics_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool enableSend = false;
            tbMessage.Clear();
                
            List<string> usedParameters = new List<string>();

            if (tvTopics.SelectedNode != null)
            {
                TopicInformation topic = tvTopics.SelectedNode.Tag as TopicInformation;

                if (topic != null && topic.IsTopic)
                {
                    enableSend = !topic.IsProperty;

                    List<int> starts = new List<int>();
                    List<int> counts = new List<int>(); 

                    StringBuilder messageSample = new StringBuilder();
                    messageSample.AppendLine("<Message UtcTime=\"%NOW%\">");
                    starts.Add(18);
                    counts.Add(5);
                    messageSample.AppendLine("   <Source>");
                    foreach (SimpleItemDescription item in topic.SourceItems)
                    {
                        messageSample.AppendFormat("      <SimpleItem Name=\"{0}\" Value=\"", item.Name);
                        starts.Add(messageSample.Length - 2);
                        string line = string.Format("value of {0}:{1} data type", item.Type.Namespace, item.Type.Name);
                        counts.Add(line.Length);
                        messageSample.Append(line);
                        messageSample.AppendLine("\" />");

                        usedParameters.Add(item.Name);

                    }
                    messageSample.AppendLine("   </Source>");
                    messageSample.AppendLine("   <Data>");
                    int offset = 5;
                    foreach (SimpleItemDescription item in topic.DataItems)
                    {
                        messageSample.AppendFormat("      <SimpleItem Name=\"{0}\" Value=\"", item.Name);
                        starts.Add(messageSample.Length - offset);
                        offset++;
                        string line = string.Format("value of {0}:{1} data type", item.Type.Namespace, item.Type.Name);
                        counts.Add(line.Length);
                        messageSample.Append(line);
                        messageSample.AppendLine("\" />");

                        usedParameters.Add(item.Name);
                    }
                    messageSample.AppendLine("   </Data>");
                    messageSample.AppendLine("</Message>");
                    tbMessage.Text = messageSample.ToString();

                    for (int i = 0; i < starts.Count; i++)
                    {
                        int begin = starts[i];
                        int count = counts[i];

                        tbMessage.Select(begin, count);
                        tbMessage.SelectionColor = System.Drawing.Color.Red;
                    }
                }
            }

            _allParametersSupported = true;
            foreach (string parameter in usedParameters)
            {
                if (!_inputs.ContainsKey(parameter))
                {
                    _allParametersSupported = false;
                    break;
                }
            }

            if (_allParametersSupported)
            {
                foreach (ParameterInput ctrl in _parameterControls)
                {
                    ctrl.Visible = (usedParameters.Contains(ctrl.ParameterName));
                } 

            }
            else
            {
                flpParameters.Enabled = true;
                foreach (ParameterInput ctrl in _parameterControls)
                {
                    ctrl.Visible = false;
                }      
            }

            if (!_allParametersSupported)
            {
                enableSend = enableSend && (tcMessageConstricting.SelectedTab == tpRawMessage);
            }
            btnSend.Enabled = enableSend;  
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tvTopics.SelectedNode != null)
            {
                TopicInformation topic = tvTopics.SelectedNode.Tag as TopicInformation;
                if (topic != null)
                {
                    List<SimpleItem> sourceItems=new List<SimpleItem>();
                    List<SimpleItem> dataItems = new List<SimpleItem>();
                    DateTime messageTime = DateTime.MinValue;
                    string propertyOperation = null;

                    if (tcMessageConstricting.SelectedTab == tpRawMessage)
                    {

                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.LoadXml(tbMessage.Text);

                            XmlElement messageElement = doc.DocumentElement;
                            if (messageElement.HasAttribute("PropertyOperation"))
                            {
                                propertyOperation = messageElement.Attributes["PropertyOperation"].Value;
                            }
                            XmlNodeList sourceSimpleItems = messageElement.SelectNodes("Source/SimpleItem");
                            foreach (XmlNode node in sourceSimpleItems)
                            {
                                XmlElement element = node as XmlElement;
                                if (element != null)
                                {
                                    sourceItems.Add(new SimpleItem()
                                                        {
                                                            Name = element.Attributes["Name"].Value,
                                                            Value = element.Attributes["Value"].Value
                                                        });
                                }
                            }
                            XmlNodeList dataSimpleItems = messageElement.SelectNodes("Data/SimpleItem");
                            foreach (XmlNode node in dataSimpleItems)
                            {
                                XmlElement element = node as XmlElement;
                                if (element != null)
                                {
                                    dataItems.Add(new SimpleItem()
                                                      {
                                                          Name = element.Attributes["Name"].Value,
                                                          Value = element.Attributes["Value"].Value
                                                      });
                                }
                            }

                            string dateTimeValue = messageElement.Attributes["UtcTime"].Value;
                            if (dateTimeValue != "%NOW%")
                            {
                                messageTime = DateTime.Parse(dateTimeValue);
                            }

                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("Message format is not correct");
                            return;
                        }
                    }
                    else
                    {
                        foreach (SimpleItemDescription item in topic.SourceItems)
                        {
                            SimpleItem messageItem = new SimpleItem();
                            messageItem.Name = item.Name;
                            messageItem.Value = _inputs[item.Name].GetText();
                            sourceItems.Add(messageItem);
                        }
                        foreach (SimpleItemDescription item in topic.DataItems)
                        {
                            SimpleItem messageItem = new SimpleItem();
                            messageItem.Name = item.Name;
                            messageItem.Value = _inputs[item.Name].GetText();
                            dataItems.Add(messageItem);
                        }
                    }
                    try
                    {
                        EventControlClient.FireEvent(topic, messageTime, propertyOperation, sourceItems.ToArray() , dataItems.ToArray());
                    }
                    catch (Exception exc)
                    {
                        ShowError(exc);
                    }
                }
            }
        }

        private void copyTopicStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvTopics.SelectedNode != null)
            {
                TopicInformation topic = tvTopics.SelectedNode.Tag as TopicInformation;
                if (topic != null)
                {
                    string topicString = topic.TopicString;
                    Clipboard.SetDataObject(topicString);
                }
            }
        }

        private void copyNamespacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvTopics.SelectedNode != null)
            {
                TopicInformation topic = tvTopics.SelectedNode.Tag as TopicInformation;
                if (topic != null)
                {
                    string namespaces = string.Empty;
                    foreach (NamespaceDescription descr in topic.Namespaces)
                    {
                        namespaces += string.Format("{0}=\"{1}\"", descr.Prefix, descr.Namespace); 
                    }

                    Clipboard.SetDataObject(namespaces);
                }
            }
        }

        private void cmsTopics_Opening(object sender, CancelEventArgs e)
        {
            bool showMenu = false;
            if (tvTopics.SelectedNode != null)
            {
                TopicInformation topic = tvTopics.SelectedNode.Tag as TopicInformation;
                if (topic != null)
                {
                    showMenu = true;
                }
            }
            e.Cancel = !showMenu;
        }

        private void tcMessageConstricting_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enableSend = false;
            if (tvTopics.SelectedNode != null)
            {
                TopicInformation topic = tvTopics.SelectedNode.Tag as TopicInformation;

                if (topic != null && topic.IsTopic)
                {
                    if (tcMessageConstricting.SelectedTab == tpMessageBuilder)
                    {
                        enableSend = _allParametersSupported;
                    }
                    else
                    {
                        enableSend = true;
                    }
                }
            }
            btnSend.Enabled = enableSend;
        }
       
    }
}
