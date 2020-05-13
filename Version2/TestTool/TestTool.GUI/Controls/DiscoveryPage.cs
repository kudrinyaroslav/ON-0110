﻿///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.Tests.Common.Discovery;
using TestTool.GUI.Utils;
using TestTool.Tests.Definitions.Data;
using System.Linq;
using System.Linq.Expressions;

namespace TestTool.GUI.Controls
{
    /// <summary>
    /// Device discovery tab control
    /// </summary>
    partial class DiscoveryPage : Page, IDiscoveryView
    {
        private const string _serviceDataEmpty = "<Press Check to get information>";
        private DiscoveryController _controller;
        private string _prevDeviceIp;
        private string _prevAddress;
        private bool _autoSelectDevice = false;
        private bool _probeAnswered = false;
        private bool _unicastProbing = false;

        #region IDiscoveryView Members
        /// <summary>
        /// Returns current network interfaces controller
        /// </summary>
        public NetworkInterfaceDescription NIC
        {
            get 
            {
                NetworkInterfaceDescription res = null;

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        if (cmbNICs.SelectedItem != null)
                        {
                            res = ((NICListItem)cmbNICs.SelectedItem).NIC;
                        }
                        else if (cmbNICs.Items.Count > 0)
                        {
                            res = ((NICListItem)cmbNICs.Items[0]).NIC;
                        }
                    }));
                }
                else
                {
                    if (cmbNICs.SelectedItem != null)
                    {
                        res = ((NICListItem)cmbNICs.SelectedItem).NIC;
                    }
                    else if (cmbNICs.Items.Count > 0)
                    {
                        res = ((NICListItem)cmbNICs.Items[0]).NIC;
                    }
                }
                return res;
            }
        }

        /// <summary>
        /// Selects network interface controller by IP address
        /// </summary>
        public string NICAddress 
        { 
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //select saved IP address
                    foreach (object item in cmbNICs.Items)
                    {
                        NICListItem nicItem = item as NICListItem;
                        if ((nicItem.NIC.IP != null) && (nicItem.NIC.IP.ToString() == value))
                        {
                            cmbNICs.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns list of discovered devices
        /// </summary>
        public List<DeviceDiscoveryData> Devices
        {
            get 
            {
                List<DeviceDiscoveryData> devices = new List<DeviceDiscoveryData>();
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        foreach (ListViewItem item in lvDevices.SelectedItems)
                        {
                            devices.Add(item.Tag as DeviceDiscoveryData);
                        }
                    }));
                }
                else
                { 
                    foreach (ListViewItem item in lvDevices.SelectedItems)
                    {
                        devices.Add(item.Tag as DeviceDiscoveryData);
                    }
                }

                return devices;
            }
        }

        /// <summary>
        /// Returns current device (selected in device list)
        /// </summary>
        public DeviceDiscoveryData Current 
        { 
            get
            {
                DeviceDiscoveryData res = null;

                Func<DeviceDiscoveryData> action = () =>
                                                   {
                                                       var device = Devices.FirstOrDefault(d => d.UUID == tbEpAddress.Text);
                                                       if (null != device)
                                                           return device;

                                                       return Devices.FirstOrDefault(d => d.EndPointAddress == tbDeviceIP.Text);
                                                   };

                if (InvokeRequired)
                    Invoke(new Action(() => res = action()));
                else
                {
                    res = action();
                    //foreach (ListViewItem item in lvDevices.Items)
                    //{
                    //    DeviceDiscoveryData device = item.Tag as DeviceDiscoveryData;
                    //    if (string.Compare(device.EndPointAddress, tbDeviceIP.Text, true) == 0)
                    //    {
                    //        res = device;
                    //        break;
                    //    }
                    //}
                }
                return res;
            }
        }

        /// <summary>
        /// Returns or sets current service address
        /// </summary>
        public string ServiceAddress
        {
            get
            {
                if (InvokeRequired)
                {
                    string ip = null;
                    Invoke(new Action(() =>
                    {
                        ip = cmbServiceAddress.Text;
                     }));
                    return ip;
                }
                else 
                    return cmbServiceAddress.Text; 
            }
            set 
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        cmbServiceAddress.Text = value; 
                    }));
                }
                else 
                    cmbServiceAddress.Text = value; 
            }
        }

        public string UserName
        {
            get
            {
                string sValue="";

                if (tbUsername.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => sValue = tbUsername.Text));
                }
                else
                {
                    sValue = tbUsername.Text;
                }

                return sValue;
            }
            set
            {
                if (tbUsername.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => tbUsername.Text = value));
                }
                else
                {
                    tbUsername.Text = value;
                }
            }
        }

        public string Password
        {
            get
            {
                string sValue = "";

                if (tbPassword.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => sValue = tbPassword.Text));
                }
                else
                {
                    sValue = tbPassword.Text;
                }

                return sValue;
            }
            set
            {
                if (tbPassword.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => tbPassword.Text = value));
                }
                else
                {
                    tbPassword.Text = value;
                }
            }
        }

        public bool ShowOptions 
        {
            get 
            {
                if (InvokeRequired)
                {
                    bool showOptions = false;
                    Invoke(new Action(() =>
                    {
                        showOptions = chkEnableSearchOptions.Checked;
                    }));

                    return showOptions;
                }
                else
                {
                    return chkEnableSearchOptions.Checked;
                }
            }
            set { chkEnableSearchOptions.Checked = value;  }
        }
        
        public string SearchScopes 
        {
            get 
            {
                if (InvokeRequired)
                {
                    string searchScopes = "";

                    Invoke(new Action(() =>
                    {
                        searchScopes = chkEnableSearchOptions.Checked ? tbSearchScopes.Text : "";
                    }));

                    return searchScopes;
                }
                else
                {
                    return chkEnableSearchOptions.Checked ? tbSearchScopes.Text : "";  
                }

            }
            set { tbSearchScopes.Text = value; } 
        }
        
        /// <summary>
        /// Returns or sets current device IP address
        /// </summary>
        public IPAddress DeviceAddress
        {
            get 
            {
                if (InvokeRequired)
                {
                    IPAddress ipAddr = null;
                    Invoke(new Action(() =>
                    {
                        bool ipv6 = (NIC != null) && (NIC.IP != null) && (NIC.IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
                        ipAddr = DiscoveryUtils.GetIP(tbDeviceIP.Text, ipv6);
                    }));

                    return ipAddr;
                }
                else
                {
                    bool ipv6 = (NIC != null) && (NIC.IP != null) && (NIC.IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
                    return DiscoveryUtils.GetIP(tbDeviceIP.Text, ipv6);
                }
            }
            set 
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        tbDeviceIP.Text = value != null ? value.ToString() : string.Empty;
                    }));
                }
                else
                    tbDeviceIP.Text = value != null ? value.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Handles discovery error event
        /// </summary>
        /// <param name="e">Thrown exception</param>
        public void OnDiscoveryError(Exception e)
        {
            if(_unicastProbing)
            {
                _probeAnswered = true;
                ShowError(e);
            }
        }

        /// <summary>
        /// Updates main form title. Appends device IP address
        /// </summary>
        public void UpdateFormTitle()
        {

            Form main = FindForm();
            if (main != null)
            {
                string[] origin = main.Text.Split(new string[] { " -" }, StringSplitOptions.RemoveEmptyEntries);
                IPAddress address = DeviceAddress;

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        main.Text = origin.Length > 0 ? origin[0] : string.Empty;
                        if (address != null)
                        {
                            main.Text += " - " + address.ToString();
                        }
                    }));
                }
                else
                {
                    main.Text = origin.Length > 0 ? origin[0] : string.Empty;
                    if (address != null)
                    {
                        main.Text += " - " + address.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Enables or disables buttons, depending on some conditions
        /// </summary>
        public void UpdateButtons()
        {
            btnCheckIP.Enabled = !string.IsNullOrEmpty(tbDeviceIP.Text) && btnCheckIP.Enabled;
            btnCheckIP.Refresh();
            btnCheckService.Enabled = !string.IsNullOrEmpty(cmbServiceAddress.Text) && btnCheckService.Enabled;
            btnCheckService.Refresh();
        }
        #endregion

        #region IComparer<ListViewItem> Members

        #endregion
        /// <summary>
        /// C-tor
        /// </summary>
        public DiscoveryPage()
        {
            InitializeComponent();
            _controller = new DiscoveryController(this);
            _controller.DeviceDiscovered += OnDeviceDiscovered;
            _controller.DiscoveryCompleted +=OnDiscoveryCompleted;
            _controller.DeviceInformationReceived += OnDeviceInformationReceived;
            _controller.DiscoveryError += OnDiscoveryError;
            ClearServiceInfo();
            FillNetworkInterfaces();
            if (cmbNICs.Items.Count > 0)
            {
                cmbNICs.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Return controller
        /// </summary>
        internal DiscoveryController Controller
        {
            get { return _controller; }
        }

        /// <summary>
        /// Handles losing focus event of service address box
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void cmbServiceAddress_TextChanged(object sender, EventArgs e)
        {
            //if ((cmbServiceAddress.Enabled) && (string.Compare(_prevAddress, cmbServiceAddress.Text, true) != 0))
            if (string.Compare(_prevAddress, cmbServiceAddress.Text, true) != 0)
            {
                _prevAddress = cmbServiceAddress.Text;

                //address changed
                string url = cmbServiceAddress.Text;
                Uri uri = null;
                if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    //http:// protocol can be omitted
                    Uri.TryCreate("http://" + url, UriKind.Absolute, out uri);
                }
                if (uri != null)
                {
                    IPAddress address = null;
                    tbDeviceIP.Text = IPAddress.TryParse(uri.Host, out address) ? address.ToString() : uri.Host;
                }
                //lvDevices.SelectedItems.Clear();
            }
        }

        /// <summary>
        /// Handles changing of IP address
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void tbDeviceIP_Enter(object sender, EventArgs e)
        {
            _prevDeviceIp = tbDeviceIP.Text;
        }

        /// <summary>
        /// Handles losing focus event of IP address box
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void tbDeviceIP_Leave(object sender, EventArgs e)
        {
            if ((tbDeviceIP.Enabled) && (string.Compare(_prevDeviceIp, tbDeviceIP.Text, true) != 0))
            {
                //ip address changed
                lvDevices.SelectedItems.Clear();
            }
        }

        /// <summary>
        /// Switches control to specified state
        /// </summary>
        /// <param name="state">New state</param>
        public override void SwitchToState(Enums.ApplicationState state)
        {
            if (state.IsActive())
            {
                Invoke(new Action(() => EnableControls(false)));
            }
            else
            {
                Invoke(new Action(() => EnableControls(true)));
            }
        }

        /// <summary>
        /// Populates list of network interface controllers
        /// </summary>
        private void FillNetworkInterfaces()
        {
            List<NetworkInterfaceDescription> interfaces = _controller.GetNetworkInterfaces();
            foreach (NetworkInterfaceDescription nic in interfaces)
            {
                NICListItem item = new NICListItem(nic, checkHWStyle.Checked);
                cmbNICs.Items.Add(item);
            }
        }

        /// <summary>
        /// Handles HW Style chekbox checked event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void checkHWStyle_CheckedChanged(object sender, EventArgs e)
        {
            int selected = cmbNICs.SelectedIndex;
            cmbNICs.Items.Clear();
            FillNetworkInterfaces();
            if (selected < cmbNICs.Items.Count)
            {
                cmbNICs.SelectedIndex = selected;
            }
        }

        /// <summary>
        /// Handles discover button click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnDiscover_Click(object sender, EventArgs e)
        {
            if(cmbNICs.SelectedItem != null)
            {
                try
                {
                    lvDevices.Items.Clear();
                    NICListItem item = (NICListItem)cmbNICs.SelectedItem;
                    Controller.RunDiscovery(item.NIC.IP);
                }
                catch(Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        /// <summary>
        /// Handles check device by IP button click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnCheckIP_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbDeviceIP.Text))
            {
                try
                {
                    IPAddress deviceIP = DiscoveryUtils.GetIP(tbDeviceIP.Text, false);
                    if (deviceIP != null)
                    {
                        ClearServiceInfo();
                        _autoSelectDevice = true; //set flag for auto select device if found
                        _probeAnswered = false;
                        _unicastProbing = true;
#if true
                        NICListItem item = (NICListItem)cmbNICs.SelectedItem;
                        Controller.ProbeDevice(item.NIC.IP, deviceIP);
#else
Controller.ProbeDevice(deviceIP);
#endif
                    }
                    else
                    {
                        ShowError("Invalid IP address or hostname");
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        /// <summary>
        /// Compares two byte arrays by elements
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(byte[] x, byte[] y)
        {
            int res = 0;
            if (x.Length != y.Length)
            {
                return x.Length > y.Length ? 1 : -1;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return x[i].CompareTo(y[i]);
                }
            }
            return res;
        }
        /// <summary>
        /// Adds device to device list
        /// </summary>
        /// <param name="device">Device to be added</param>
        private void AddDeviceToList(DeviceDiscoveryData device)
        {
            IPAddress devAddress = null;
            IPAddress.TryParse(device.EndPointAddress, out devAddress);
            if(devAddress != null)
            {
                ListViewItem deviceItem = null;
                for (int i = 0; i < lvDevices.Items.Count; i++)
                {
                    DeviceDiscoveryData listDev = (DeviceDiscoveryData)lvDevices.Items[i].Tag;
                    IPAddress address = IPAddress.Parse(listDev.EndPointAddress);
                    int res = Compare(devAddress.GetAddressBytes(), address.GetAddressBytes());
                    if (res == 0 && listDev.UUID == device.UUID)
                    {
                        deviceItem = lvDevices.Items[i];
                        break;
                    }
                }
                if (deviceItem == null)
                {
                    var scopes = device.Scopes.Split(' ');

                    var hardwarePrefix = "onvif://www.onvif.org/hardware/";
                    var hardware = "";
                    if (scopes.Where(e => e.StartsWith(hardwarePrefix)).Any()) 
                        hardware = scopes.Where(e => e.StartsWith(hardwarePrefix)).First().Substring(hardwarePrefix.Count());

                    var manufacturerPrefix = "onvif://www.onvif.org/manufacturer/";
                    var manufacturer = "";
                    if (scopes.Where(e => e.StartsWith(manufacturerPrefix)).Any()) 
                        manufacturer = scopes.Where(e => e.StartsWith(manufacturerPrefix)).First().Substring(manufacturerPrefix.Count());

                    deviceItem = new ListViewItem(new string[] { device.EndPointAddress, device.UUID, hardware, manufacturer });
                    lvDevices.Items.Add(deviceItem);

                    lvDevices.Sort();
                }
                deviceItem.Tag = device; //renew properties
                if (_unicastProbing)
                {
                    ShowDeviceDiscoveryData(device);
                }
                if (!deviceItem.Selected && _autoSelectDevice)
                {
                    lvDevices.SelectedItems.Clear();
                    deviceItem.Selected = true;
                    lvDevices.Refresh();
                }
            }
        }

        /// <summary>
        /// Handles discovery completed event
        /// </summary>
        private void OnDiscoveryCompleted()
        {
            _autoSelectDevice = false;//reset flag
            if (!_probeAnswered && _unicastProbing)
            {
                Invoke(new Action(() => { MessageBox.Show(this, "Device did not respond", "No response", MessageBoxButtons.OK, MessageBoxIcon.Information); }));
            }
            _probeAnswered = false;
            _unicastProbing = false;
        }

        /// <summary>
        /// Handles device discovered event
        /// </summary>
        /// <param name="device">Discovered device</param>
        private void OnDeviceDiscovered(DeviceDiscoveryData device)
        {
            _probeAnswered = true;
            Invoke(new Action<DeviceDiscoveryData>(AddDeviceToList), new object[] { device });
        }

        /// <summary>
        /// Handles selection changed event of device list
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void lvDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvDevices.SelectedItems.Count > 0)
            {
                ClearServiceInfo();
                ListViewItem item = lvDevices.SelectedItems[0];
                DeviceDiscoveryData device = (DeviceDiscoveryData)item.Tag;
                ShowDeviceDiscoveryData(device);
            }
        }


        void ShowDeviceDiscoveryData(DeviceDiscoveryData device)
        { 
            tbEpAddress.Text = device.UUID;
            tbDeviceIP.Text = device.EndPointAddress;
            cmbServiceAddress.Items.Clear();
            foreach (string s in device.ServiceAddresses)
            {
                cmbServiceAddress.Items.Add(s);
            }
            cmbServiceAddress.Text = device.ServiceAddresses.Count > 0 ? device.ServiceAddresses[0] : string.Empty;
            tbType.Text = device.Type;
            string[] scopes = device.Scopes.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            tbScopes.Text = string.Empty;
            foreach (string scope in scopes)
            {
                tbScopes.Text += scope + "\r\n";
            }
            tbMetadataVersion.Text = device.MetadataVersion.ToString();
        
        }

        /// <summary>
        /// Enables of disables controls on the page
        /// </summary>
        /// <param name="enable">If true, enable controls</param>
        private void EnableControls(bool enable)
        {
            btnDiscover.Enabled = enable;
            btnDiscover.Refresh();
            btnCheckIP.Enabled = !string.IsNullOrEmpty(tbDeviceIP.Text) && enable;
            btnCheckIP.Refresh();
            btnCheckService.Enabled = !string.IsNullOrEmpty(cmbServiceAddress.Text) && enable;
            btnCheckService.Refresh();
            cmbNICs.Enabled = enable;
            cmbServiceAddress.Enabled = enable;
            tbUsername.Enabled = enable;
            tbPassword.Enabled = enable;
            tbDeviceIP.Enabled = enable;
            checkHWStyle.Enabled = enable;
            lvDevices.Enabled = enable;
            chkEnableSearchOptions.Enabled = enable;
            tbSearchScopes.Enabled = enable;
            Cursor.Current = enable ? Cursors.Default : Cursors.WaitCursor;
        }

        /// <summary>
        /// Clear textboxes with device info 
        /// </summary>
        private void ClearServiceInfo()
        {
            tbModel.Text = _serviceDataEmpty;
            tbFirmwareVersion.Text = _serviceDataEmpty;
            tbSerial.Text = _serviceDataEmpty;
            tbBrand.Text = _serviceDataEmpty;
        }

        /// <summary>
        /// Handles check service address button click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnCheckService_Click(object sender, EventArgs e)
        {
            string url = cmbServiceAddress.Text;
            if(!string.IsNullOrEmpty(url))
            {
                try
                {
                    Controller.GetDeviceInformation(url);
                }
                catch(System.ServiceModel.EndpointNotFoundException)
                {
                    ShowError("Could not connect to " + url);
                }
                catch(Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        /// <summary>
        /// Handles device information received event
        /// </summary>
        /// <param name="manufacturer">Device manufacturer</param>
        /// <param name="model">Device model</param>
        /// <param name="firmwareVersion">Device firmware version</param>
        /// <param name="serial">Device serial number</param>
        /// <param name="hardwareId">Device hardware id</param>
        private void OnDeviceInformationReceived(string manufacturer, string model, string firmwareVersion, string serial, string hardwareId)
        {
            BeginInvoke(new Action(() => { tbModel.Text = model; }));
            BeginInvoke(new Action(() => { tbFirmwareVersion.Text = firmwareVersion; }));
            BeginInvoke(new Action(() => { tbSerial.Text = serial; }));
            BeginInvoke(new Action(() => { tbBrand.Text = manufacturer; }));
        }

        /// <summary>
        /// Clear info about current device
        /// </summary>
        private void ClearDUTInfo()
        {
            cmbServiceAddress.Text = string.Empty;
            cmbServiceAddress.Items.Clear();
            btnCheckService.Enabled = false;
            tbDeviceIP.Text = string.Empty;
            tbScopes.Text = string.Empty;
            tbEpAddress.Text = string.Empty;
            tbMetadataVersion.Text = string.Empty;
            tbType.Text = string.Empty;
            lvDevices.SelectedItems.Clear();
        }

        /// <summary>
        /// Handle clear button click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearDUTInfo();
            ClearServiceInfo();
        }

        #region IView Members
        public override IController GetController()
        {
            return _controller;
        }
        #endregion


        /// <summary>
        /// Handles service address text update event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void cmbServiceAddress_TextUpdate(object sender, EventArgs e)
        {
            btnCheckService.Enabled = !string.IsNullOrEmpty(cmbServiceAddress.Text);
            btnCheckService.Refresh();
        }

        /// <summary>
        /// Handles device address text update event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void tbDeviceIP_TextChanged(object sender, EventArgs e)
        {
            btnCheckIP.Enabled = !string.IsNullOrEmpty(tbDeviceIP.Text);
            btnCheckIP.Refresh();
        }

        /// <summary>
        /// Handles service address selection changed event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void cmbServiceAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCheckService.Enabled = !string.IsNullOrEmpty(cmbServiceAddress.Text);
            btnCheckService.Refresh();
        }

        private void chkShowSearchOptions_CheckedChanged(object sender, EventArgs e)
        {
            panelScopes.Visible = chkEnableSearchOptions.Checked;
        }


    }
}