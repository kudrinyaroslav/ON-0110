using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SMC.Controls;
using SMC.Proxies;
using SMC.Proxies.Monitoring;
using SMC.StateMonitoring;
using System.Collections.Generic;

namespace SMC.Pages
{
    public partial class DoorsManagement : BaseDoorClientControl
    {
        public DoorsManagement()
        {
            InitializeComponent();

            tbListenerUri.Text = System.Configuration.ConfigurationManager.AppSettings["StateReceiver"];

            tvDoors.AfterSelect += new TreeViewEventHandler(tvDoors_AfterSelect);
            DisplayDoorInfo(null);

            _custombindingSoap12 = new CustomBinding();
            _custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            _custombindingSoap12.Elements.Add(new HttpTransportBindingElement());

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }


        protected override void UpdateAddress()
        {
            CheckClientValid();

        }

        void tvDoors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null )
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                DoorInfo info = node.Tag as DoorInfo;
                DisplayDoorInfo(info);
            }
            else
            {
                DisplayDoorInfo(null);

            }            
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                tvDoors.Nodes.Clear();
                
                //old version:
                //DoorInfo[] doors = DoorControlClient.GetDoorInfo(null);

                List<DoorInfo> doors = GetList<DoorInfo>(DoorControlClient.GetDoorInfoList);

                foreach (var door in doors)
                {
                    {
                        TreeNode doorNode = new TreeNode(door.token);
                        doorNode.Tag = door;
                        
                        DoorState state = DoorControlClient.GetDoorState(door.token);

                        doorNode.Nodes.Add("Alarm", "Alarm: " + (state.AlarmSpecified ? state.Alarm.ToString() : "Not specified"));
                        doorNode.Nodes.Add("DoorPhysicalState", "DoorPhysicalState: " + (state.DoorPhysicalStateSpecified ? state.DoorPhysicalState.ToString() : "Not specified"));
                        doorNode.Nodes.Add("LockPhysicalState", "LockPhysicalState: " + (state.LockPhysicalStateSpecified ? state.LockPhysicalState.ToString() : "Not specified"));
                        doorNode.Nodes.Add("DoubleLockPhysicalState", "DoubleLockPhysicalState: " + (state.DoubleLockPhysicalStateSpecified ? state.DoubleLockPhysicalState.ToString() : "Not specified"));
                        doorNode.Nodes.Add("DoorMode", "DoorMode: " + state.DoorMode.ToString());
                        
                        string tamper = state.Tamper != null ? state.Tamper.State.ToString() : "Undefined";
                        doorNode.Nodes.Add("Tamper", "Tamper: " + tamper);

                        string fault = state.Fault != null ? state.Fault.State.ToString() : "Undefined";
                        doorNode.Nodes.Add("Fault", "Fault: " + fault);

                        tvDoors.Nodes.Add(doorNode);
                    }

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }


        #region Right pane

        private DoorInfo _door;

        public void DisplayDoorInfo(DoorInfo info)
        {
            _door = info;

            if (info != null)
            {
                tbToken.Text = info.token;
                tbName.Text = info.Name;
                tbDescription.Text = info.Description;

                DoorCapabilities capabilities = info.Capabilities;
                if (capabilities == null)
                {
                    capabilities = SmcData.Context.Instance.DoorsControl.GetDoorCapabilities(info.token);
                }

                if (capabilities != null)
                {
                    chkBlock.Checked = capabilities.Block && capabilities.BlockSpecified;
                    chkDoubleLock.Checked = capabilities.DoubleLock && capabilities.DoubleLockSpecified;
                    chkLock.Checked = capabilities.Lock && capabilities.LockSpecified;
                    chkLockDown.Checked = capabilities.LockDown && capabilities.LockDownSpecified;
                    chkLockOpen.Checked = capabilities.LockOpen && capabilities.LockOpenSpecified;
                    chkMomentaryAccess.Checked = capabilities.AccessSpecified && capabilities.Access;
                    chkUnlock.Checked = capabilities.Unlock && capabilities.UnlockSpecified;
                    chkAccessTimingOverride.Checked = capabilities.AccessTimingOverrideSpecified && capabilities.AccessTimingOverride;
                    chkAlarm.Checked = capabilities.AlarmSpecified && capabilities.Alarm;
                    chkDoorMonitor.Checked = capabilities.DoorMonitorSpecified && capabilities.DoorMonitor;
                    chkDoubleLockMonitor.Checked = capabilities.DoubleLockMonitorSpecified && capabilities.DoubleLockMonitor;
                    chkLockMonitor.Checked = capabilities.LockMonitorSpecified && capabilities.LockMonitor;
                    chkTamper.Checked = capabilities.TamperSpecified && capabilities.Tamper;
                    chkFault.Checked = capabilities.FaultSpecified && capabilities.Fault;
                }
            }
            else
            {
                tbToken.Text = string.Empty;
                tbName.Text = string.Empty;
                tbDescription.Text = string.Empty;

                foreach (CheckBox checkBox in new CheckBox[] { 
                    chkBlock, 
                    chkDoubleLock,
                    chkLock,
                    chkLockDown,
                    chkLockOpen,
                    chkMomentaryAccess,
                    chkUnlock,
                    chkAccessTimingOverride,
                    chkAlarm,
                    chkDoorMonitor,
                    chkDoubleLockMonitor,
                    chkLockMonitor,
                    chkTamper, 
                    chkFault})
                {
                    checkBox.Checked = false;
                }
            }

            foreach (Button btn in new Button[] { btnLock, btnDoubleLock, btnAccess, btnBlock, btnLockDown, btnLockOpen, btnUnlock, btnLockDownRelease, btnLockOpenRelease })
            {
                btn.Enabled = info != null;
            }
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.LockDoor(_door.token));
        }

        private void btnDoubleLock_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.DoubleLockDoor(_door.token));
        }

        private void btnLockDown_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.LockDownDoor(_door.token));
        }

        private void btnAccess_Click(object sender, EventArgs e)
        {
            bool? useExtended = null;
            if (chkUseExtendedTime.CheckState == CheckState.Checked)
            {
                useExtended = true;
            }
            else if (chkUseExtendedTime.CheckState == CheckState.Unchecked)
            {
                useExtended = false;
            }

            SafeInvoke(
                () => DoorControlClient.AccessDoor(_door.token,
                    useExtended,
                    tbAccessTime.Text,
                    tbOpenTooLong.Text,
                    tbPreAlarm.Text, new AccessDoorExtension()));
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.UnlockDoor(_door.token));
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.BlockDoor(_door.token));
        }

        private void btnLockOpen_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.LockOpenDoor(_door.token));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.LockDownReleaseDoor(_door.token));
        }

        private void chkDoubleLock_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnLockOpenRelease_Click(object sender, EventArgs e)
        {
            SafeInvoke(() => DoorControlClient.LockOpenReleaseDoor(_door.token));
        }

        #endregion

        #region State monitoring

        CustomBinding _custombindingSoap12;
        private ServiceHost _listenerHost;
        private Guid _subscriptionId;
        private MonitorServiceSoapClient _client;

        private SensorServiceSoapClient _sensorServiceClient;

        protected SensorServiceSoapClient SensorServiceClient
        {
            get
            {
                if (_sensorServiceClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.SensorServiceAddress);
                    _sensorServiceClient = new SensorServiceSoapClient(binding, address);
                }
                return _sensorServiceClient;
            }

        }
        
        private void btnMonitor_Click(object sender, EventArgs e)
        {
            if (_listenerHost == null)
            {
                System.Threading.Thread thread = new Thread(new ThreadStart(OpenHost));
                thread.Start();
            }
            else
            {
                System.Threading.Thread thread = new Thread(new ThreadStart(Unsubscribe));
                thread.Start();
            }
        }

        protected MonitorServiceSoapClient Client
        {
            get
            {
                if (_client == null)
                {
                    string loggerAddress = SmcContext.General.MonitorServiceAddress;
                    _client = new MonitorServiceSoapClient(GetBinding(), new EndpointAddress(loggerAddress));
                }
                return _client;
            }
        }

        void OpenHost()
        {
            try
            {
                _subscriptionId = Client.Subscribe(tbListenerUri.Text);

                StateMonitor stateMonitor = new StateMonitor();
                stateMonitor.StateUpdated += stateMonitor_StateUpdated;
                stateMonitor.ConnectionClosed += logReceiver_ConnectionClosed;

                _listenerHost = new ServiceHost(stateMonitor, new Uri(tbListenerUri.Text));
                ServiceEndpoint endpoint =
                    _listenerHost.AddServiceEndpoint(typeof(StateReportReceiverSoap), _custombindingSoap12, string.Empty);
                
                _listenerHost.Open();

                Invoke(new Action(() => { btnMonitor.Text = "Stop"; }));

            }
            catch (Exception exc)
            {
                ShowError(exc);
            }

        }
        
        void Unsubscribe()
        {
            try
            {
                Client.Unsubscribe(_subscriptionId);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
            finally
            {
                _listenerHost.Close();
                _listenerHost = null;
                Invoke(new Action(() => { btnMonitor.Text = "Monitor"; }));
            }
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (_listenerHost != null)
                {
                    Unsubscribe();
                    _listenerHost.Close();
                }
                if (_client != null)
                {
                    _client.Close();
                }
            }
            catch (Exception)
            {

            }
        }


        void stateMonitor_StateUpdated(string token, DoorState state)
        {
            Invoke(new Action(
                       () =>
                           {
                               foreach (TreeNode item in tvDoors.Nodes)
                               {
                                   DoorInfo info = (DoorInfo) item.Tag;
                                   if (info.token == token)
                                   {
                                       UpdateDoorState(item, state);
                                       break;
                                   }
                               }
                           }
                       ));
        }
        
        void UpdateDoorState(TreeNode doorNode, DoorState state)
        {
            TreeNode node = doorNode.Nodes["Alarm"];
            node.Text = "Alarm: " + (state.AlarmSpecified ? state.Alarm.ToString() : "Not specified");

            node = doorNode.Nodes["DoubleLockPhysicalState"];
            node.Text = "DoubleLockPhysicalState: " + (state.DoubleLockPhysicalStateSpecified ? state.DoubleLockPhysicalState.ToString() : "Not specified");

            node = doorNode.Nodes["DoorPhysicalState"];
            node.Text = "DoorPhysicalState: " + (state.DoorPhysicalStateSpecified ? state.DoorPhysicalState.ToString() : "Not specified");

            node = doorNode.Nodes["DoorMode"];
            node.Text = "DoorMode: " + (state.DoorMode.ToString());

            node = doorNode.Nodes["LockPhysicalState"];
            node.Text = "LockPhysicalState: " + (state.LockPhysicalStateSpecified ? state.LockPhysicalState.ToString() : "Not specified");

            node = doorNode.Nodes["Tamper"];
            node.Text = "Tamper: " + (state.Tamper != null ? state.Tamper.State.ToString() : "Undefined");

            node = doorNode.Nodes["Fault"];
            node.Text = "Fault: " + (state.Fault != null ? state.Fault.State.ToString() : "Undefined");

        }

        void logReceiver_ConnectionClosed()
        {
            Invoke(new Action(
                       () =>
                           {
                               _listenerHost = null;
                               btnMonitor.Text = "Monitor";
                           }
                       ));
        }

        #endregion

        private void tvDoors_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.TreeView.SelectedNode = e.Node;
            if (MouseButtons.Right == e.Button && null != e.Node.Parent)
            {
                string enumName = treeNodeNameToEnumTypeName(e.Node.Name);

                    if (!string.IsNullOrEmpty(enumName))
                    {
                        var enumType = Type.GetType("SMC.Proxies." + enumName);
                        var dialog = new DoorStateFlagChangeComboBox(e.Node, Enum.GetNames(enumType));

                        var deviceToken = e.Node.Parent.Name;
                        var sensorName = e.Node.Name;

                        dialog.SelectedValueChanged += (o, args) =>
                            {
                                if (-1 != dialog.SelectedIndex)
                                {
                                    var newValue = (string)dialog.Items[dialog.SelectedIndex];
                                    e.Node.Text = e.Node.Name + ": " + newValue;
                                    SafeInvoke(() =>
                                    {
                                        SensorServiceClient.SignalReceived(deviceToken, "Door", sensorName, newValue);
                                    });
                                }
                            };
                }
            }
        }

        private string treeNodeNameToEnumTypeName(string nodeName)
        {
            switch (nodeName)
            {
                case "Alarm":
                    return "DoorAlarmState";
                case "DoubleLockPhysicalState":
                    return "LockPhysicalState";
                case "DoorPhysicalState":
                    return "DoorPhysicalState";
                case "DoorMode":
                    return "DoorAlarmState";
                case "LockPhysicalState":
                    return "LockPhysicalState";
                case "Tamper":
                    return "DoorTamperState";
                case "Fault":
                    return "DoorFaultState";
            }

            return "";
        }





    }
}
