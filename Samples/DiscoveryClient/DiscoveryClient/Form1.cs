using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using ONVIF_TestCases;

namespace DiscoveryClient
{
    public partial class Form1 : Form
    {
        private List<NIC> _listNIC;
        private DeviceDiscovered[] _devices;
        private delegate void AddDeviceToListDelegate(ListBox list, DeviceDiscovered device);
        private delegate void EnableControlsDelegate(bool enable);

        public Form1()
        {
            _listNIC = GetNICs();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FillNICs();
            comboNIC.SelectedIndex = 0;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int selected = comboNIC.SelectedIndex;
            comboNIC.Items.Clear();
            FillNICs();
            if(selected < comboNIC.Items.Count)
            {
                comboNIC.SelectedIndex = selected;
            }
        }
        private void FillNICs()
        {
            foreach (NIC nic in _listNIC)
            {
                NICListItem item = new NICListItem(nic, checkHWStyle.Checked);
                comboNIC.Items.Add(item);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboNIC.SelectedItem != null)
            {
                DiscoverDevices(((NICListItem)comboNIC.SelectedItem).NIC);
            }
            else
            {
                MessageBox.Show("Выбирете интерфейс из списка");
            }
        }
        private List<NIC> GetNICs()
        {
            List<NIC> listNic = new List<NIC>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            string selectedIP = string.Empty;
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation uinfo in adapter.GetIPProperties().UnicastAddresses)
                    {
                        listNic.Add(new NIC(adapter, uinfo.Address));
                    }
                }
            }
            return listNic;
        }
        private void OnDeviceDiscovered(object sender, DeviceDiscoveryEventArgs e)
        {
            AddDeviceToList(listBox1, e.Device);
        }
        private void OnDiscoveryFinished(object sender, EventArgs e)
        {
            EnableControlsDelegate del = new EnableControlsDelegate(EnableControls);
            button1.Invoke(del, new object[] { true });
        }
        private void EnableControls(bool enable)
        {
            button1.Text = enable ? "Discover" : "Please wait";
            button1.Enabled = enable;
            button2.Enabled = enable;
            button1.Refresh();
            button2.Refresh();
            Cursor.Current = enable ? Cursors.Default : Cursors.WaitCursor;
        }
        private void DiscoverDevices(NIC nic)
        {
            EnableControls(false);

            listBox1.Items.Clear();

            Discovery discovery = new Discovery(nic.IP);
            discovery.Discovered += OnDeviceDiscovered;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            discovery.Probe();
        }
        private void AddDeviceToList(ListBox list, DeviceDiscovered device)
        {
            if(list.InvokeRequired)
            {
                AddDeviceToListDelegate del = new AddDeviceToListDelegate(AddDeviceToList);
                list.Invoke(del, new object[] { list, device });
            }
            else
            {
                list.Items.Add(device);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceDiscovered tmpDev = ((DeviceDiscovered)listBox1.SelectedItem);
            textBox1.Text = tmpDev.UUID;
            textBox2.Text = tmpDev.Type;
            textIPAddess.Text = tmpDev.IP;
            textBox4.Text = tmpDev.ServiceAddress;
            textBox5.Text = tmpDev.Scopes;
            textBox6.Text = tmpDev.Metadata.ToString();
        }


        private void ProbeDevice(NIC nic, IPAddress address)
        {
            EnableControls(false);

            Cursor.Current = Cursors.WaitCursor;

            Discovery discovery = new Discovery(IPAddress.Any);
            //Discovery discovery = new Discovery(nic.IP);
            discovery.Discovered += OnDeviceDiscovered;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            discovery.Probe(address);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            IPAddress address = null;
            try
            {
                address = IPAddress.Parse(textIPAddess.Text);
            }
            catch
            {
            }
            if ((comboNIC.SelectedItem != null)&&(address != null))
            {
                ProbeDevice(((NICListItem)comboNIC.SelectedItem).NIC, address);
            }
            else
            {
                MessageBox.Show("Выбирете интерфейс из списка");
            }
        }
    }
    internal class NICListItem
    {
        public NIC NIC { get; protected set;}
        public bool HWStyle { get; protected set; }

        public NICListItem(NIC nic, bool hwStyle)
        {
            NIC = nic;
            HWStyle = hwStyle;
        }
        public override string ToString()
        {
            return NIC != null ? NIC.ToString(HWStyle) : string.Empty;
        }
    }
    internal class NIC
    {
        private const int _maxDesriptionLen = 30;
        public IPAddress IP { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public NIC(NetworkInterface adapter, IPAddress ip)
        {
            Name = adapter.Name;
            Description = adapter.Description;
            if (Description.Length > _maxDesriptionLen)
            {
                Description = Description.Substring(0, _maxDesriptionLen - 3) + "...";
            }
            IP = ip;
        }
        public string ToString(bool hwStyle)
        {
            return string.Format("{0} ({1})",
                hwStyle? Description : Name,
                IP.ToString());
        }
    }
    internal class DeviceDiscovered
    {
        public string UUID { get; set; }
        public string Type { get; set; }
        public string ServiceAddress { get; set; }
        public string IP { get; set; }
        public string Scopes { get; set; }
        public uint Metadata { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0} : {1}", IP, UUID);
        }
    }
    internal class DeviceData
    {
        public string UUID { get; set; }
        public string IPAddress { get; set; }

        public override string ToString() { return UUID; }
    };
}
