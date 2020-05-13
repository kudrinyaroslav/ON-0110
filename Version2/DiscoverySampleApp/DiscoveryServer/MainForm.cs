using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ONVIFSampleApp;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Xml;

namespace DiscoveryServer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            InitializeScopes();
            InitializeTypes();

            _guid = Guid.NewGuid();
            //_baseAddress = new Uri("http://localhost:8000/" + _guid.ToString());
            _baseAddress = new Uri("http://localhost:17934/ServiceDevice10/DeviceServiceFake.asmx");
        }

        Guid _guid;
        Uri _baseAddress;

        public const string ONVIF_DISCOVER_TYPES = "NetworkVideoTransmitter";
        public const string ONVIF_20_DEVICE_TYPE = "Device";
        public const string ONVIF_NETWORK_WSDL_URL = "http://www.onvif.org/ver10/network/wsdl";
        public const string ONVIF_20_DEVICE_NS = "http://www.onvif.org/ver10/device/wsdl";

        internal void InitializeTypes()
        {
            {
                ListViewItem item = new ListViewItem(ONVIF_DISCOVER_TYPES);
                item.SubItems.Add(ONVIF_NETWORK_WSDL_URL);
                lvTypes.Items.Add(item);
            }
            {
                ListViewItem item = new ListViewItem(ONVIF_20_DEVICE_TYPE);
                item.SubItems.Add(ONVIF_20_DEVICE_NS);
                lvTypes.Items.Add(item);
            }
        }

        public void InitializeScopes()
        {
            ListViewItem scope1 = new ListViewItem("onvif://www.onvif.org/type/video_encoder");
            lvScopes.Items.Add(scope1);
        }

        private void btnAddType_Click(object sender, EventArgs e)
        {
            TypeInput input = new TypeInput();

            if (input.ShowDialog() == DialogResult.OK)
            {
                ListViewItem item = new ListViewItem(input.Type);
                item.SubItems.Add(input.Namespace);
                lvTypes.Items.Add(item);
            }
        }

        private void btnRemoveType_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTypes.SelectedItems)
            {
                lvTypes.Items.Remove(item);
            }
        }

         private void btnAddScope_Click(object sender, EventArgs e)
        {
            TextInput input = new TextInput("Enter scope", "Scope:");
            if (input.ShowDialog() == DialogResult.OK)
            {
                ListViewItem scope = new ListViewItem(input.Input);
                lvScopes.Items.Add(scope);
            }
        }       
        
        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvScopes.SelectedItems)
            {
                lvScopes.Items.Remove(item);
            }
        }

        public DiscoveryVersion SelectedVersion
        {
            get
            {
                if (rbVersion11.Checked)
                {
                    return System.ServiceModel.Discovery.DiscoveryVersion.WSDiscovery11;
                }
                if (rbVersionApril2005.Checked)
                {
                    return System.ServiceModel.Discovery.DiscoveryVersion.WSDiscoveryApril2005;
                }
                return System.ServiceModel.Discovery.DiscoveryVersion.WSDiscoveryCD1;
            }
        }

        ServiceHost _serviceHost;

        private void btnListen_Click(object sender, EventArgs e)
        {

            if (_serviceHost == null)
            {
                _serviceHost = new ServiceHost(typeof(SampleService), _baseAddress);

                {
                    // Add an endpoint to the service
                    ServiceEndpoint discoverableEndpoint = _serviceHost.AddServiceEndpoint(
                        typeof(ISampleService),
                        new BasicHttpBinding(),
                        "/DiscoverableEndpoint");

                    // Add Scopes to the endpoint
                    EndpointDiscoveryBehavior discoverableEndpointBehavior = new EndpointDiscoveryBehavior();

                    foreach (ListViewItem item in lvTypes.CheckedItems)
                    {
                        XmlQualifiedName type = new XmlQualifiedName(item.SubItems[0].Text, item.SubItems[1].Text);
                        discoverableEndpointBehavior.ContractTypeNames.Add(type);
                    }

                    foreach (ListViewItem item in lvScopes.CheckedItems)
                    {
                        discoverableEndpointBehavior.Scopes.Add(new Uri(item.SubItems[0].Text));
                    }

                    discoverableEndpointBehavior.Enabled = true;
                    discoverableEndpoint.Behaviors.Add(discoverableEndpointBehavior);
                }

                // without this fragment, Probe does not work
                {
                    ServiceDiscoveryBehavior sdb = new ServiceDiscoveryBehavior();
                    _serviceHost.Description.Behaviors.Add(sdb);
                    UdpDiscoveryEndpoint discoveryEndpoint = new UdpDiscoveryEndpoint(SelectedVersion);
                    discoveryEndpoint.TransportSettings.TimeToLive = 5;                    
                    _serviceHost.AddServiceEndpoint(discoveryEndpoint);                   
                    //discoveryEndpoint.Address = new EndpointAddress(new Uri("urn:uuid" + _guid.ToString()));
                }

                _serviceHost.Open();

                btnListen.Text = "Stop";
            }
            else 
            {
                _serviceHost.Close();
                _serviceHost = null;
                btnListen.Text = "Listen";
            }
        }


        EndpointDiscoveryMetadata GetAnnouncementSettings()
        { 
            EndpointDiscoveryMetadata metadata = new EndpointDiscoveryMetadata();
            metadata.Address = new EndpointAddress(new Uri("urn:uuid:" + tbGUID.Text));

            string[] addressList = tbAdresses.Text.Split(' ');
            foreach (string address in addressList)
            {
                if (address.Trim().Length >= 0)
                {
                    metadata.ListenUris.Add(new Uri(address.Trim()));
                }
            }

            foreach (ListViewItem item in lvTypes.CheckedItems)
            {
                XmlQualifiedName type = new XmlQualifiedName(item.SubItems[0].Text, item.SubItems[1].Text);
                metadata.ContractTypeNames.Add(type); 
            }

            foreach (ListViewItem item in lvScopes.CheckedItems)
            {
                metadata.Scopes.Add(new Uri(item.SubItems[0].Text));
            }

            return metadata;
        }

        AnnouncementClient AnnouncementClient()
        {
            AnnouncementClient anouncementClient;
            UdpAnnouncementEndpoint endpoint = new UdpAnnouncementEndpoint(SelectedVersion);

            anouncementClient = new AnnouncementClient(endpoint);

            return anouncementClient;
        }



        private void btnHello_Click(object sender, EventArgs e)
        {
            AnnouncementClient client = AnnouncementClient();
            client.AnnounceOnline(GetAnnouncementSettings());
            client.Close();
        }

        private void btnBye_Click(object sender, EventArgs e)
        {
            AnnouncementClient client = AnnouncementClient();
            client.AnnounceOffline(GetAnnouncementSettings());
            client.Close();
        }


    }
}
