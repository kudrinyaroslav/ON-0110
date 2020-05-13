using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel.Discovery;
using System.ServiceModel;
using System.Xml;

namespace ONVIFSampleApp
{
    public partial class DevicesList : UserControl
    {
        public DevicesList()
        {
            InitializeComponent();

            InitializeListeninig();

            InitializeTypes();
            InitializeScopes();

        }

        #region Start/Stop listen
        
        AnnouncementService _announcementService;
        ServiceHost _announcementServiceHost;

        void InitializeListeninig()
        { 
             _announcementService = new AnnouncementService();
            _announcementService.OnlineAnnouncementReceived += OnOnlineEvent;
            _announcementService.OfflineAnnouncementReceived += OnOfflineEvent;
       
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

        private void btnDiscover_Click(object sender, EventArgs e)
        {
            if (_announcementServiceHost == null)
            {
                _announcementServiceHost = new ServiceHost(_announcementService);
                _announcementServiceHost.AddServiceEndpoint(new UdpAnnouncementEndpoint(SelectedVersion));
                _announcementServiceHost.Open();
                btnDiscover.Text = "Stop listening";
            }
            else
            {
                _announcementServiceHost.Close();
                _announcementServiceHost = null;
                btnDiscover.Text = "Listen";
            }
        }
        
        void OnOnlineEvent(object sender, AnnouncementEventArgs e)
        {
            ListViewItem item = new ListViewItem(e.EndpointDiscoveryMetadata.Address.ToString());
            lvDevices.Items.Add(item);

            Console.WriteLine("Received an online announcement from {0}", e.EndpointDiscoveryMetadata.Address);
        }

        void OnOfflineEvent(object sender, AnnouncementEventArgs e)
        {
            string addr = e.EndpointDiscoveryMetadata.Address.ToString();
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (ListViewItem item in lvDevices.Items)
            {
                if (item.Text == addr)
                {
                    items.Add(item);
                }
            }

            foreach (ListViewItem item in items)
            {
                lvDevices.Items.Remove(item);
            }

            Console.WriteLine("Received an offline announcement from {0}", e.EndpointDiscoveryMetadata.Address);
        }

        #endregion

        #region Probe
        
        DiscoveryClient _discoveryClient;

        void InitializeDiscoveryClient(DiscoveryVersion version)
        {
            UdpDiscoveryEndpoint endpoint = new UdpDiscoveryEndpoint(version);
            _discoveryClient = new DiscoveryClient(endpoint);

            _discoveryClient.FindProgressChanged += DiscoveryClient_FindProgressChanged;
            _discoveryClient.FindCompleted += DiscoveryClient_FindCompleted;
        }

        void SynchronizeParameters(ProbeDialog dlg)
        {
            List<DiscoveryType> oldTypes = new List<DiscoveryType>(_types);
            List<string> oldScopes = new List<string>(_scopes);

            foreach (ListViewItem item in dlg.TypesList.Items)
            {
                string type = item.SubItems[0].Text;
                string ns =  item.SubItems[1].Text;

                DiscoveryType t = oldTypes.Where(T => T.Type == type && T.Namespace == ns).FirstOrDefault();
                if (t == null)
                {
                    DiscoveryType newType = new DiscoveryType() { Type = type, Namespace = ns};
                    _types.Add(newType);
                }
                else
                {
                    oldTypes.Remove(t);
                }

            }

            foreach (DiscoveryType t in oldTypes)
            {
                _types.Remove(t);
            }
            foreach (ListViewItem item in dlg.ScopesList.Items)
            {
                string scope = item.SubItems[0].Text;

                if (_scopes.Contains(scope))
                {
                    oldScopes.Remove(scope);
                }
                else 
                {
                    _scopes.Add(scope);
                }
            }

            foreach (string scope in oldScopes)
            {
                _scopes.Remove(scope);
            }

        }


        private void btnProbe_Click(object sender, EventArgs e)
        {
            ProbeDialog dlg = new ProbeDialog();

            dlg.InitializeTypes(_types);
            dlg.InitializeScopes(_scopes);

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            SynchronizeParameters(dlg);
            
            InitializeDiscoveryClient(dlg.DiscoveryVersion);
            lvDevices.Items.Clear();

            FindCriteria request = new FindCriteria();

            request.ScopeMatchBy = new Uri("http://schemas.xmlsoap.org/ws/2005/04/discovery/rfc3986");

            foreach (ListViewItem item in dlg.TypesList.CheckedItems)
            { 
                XmlQualifiedName type = new XmlQualifiedName(item.SubItems[0].Text, item.SubItems[1].Text);
                request.ContractTypeNames.Add(type);            
            }

            foreach (ListViewItem item in dlg.ScopesList.CheckedItems)
            {
                request.Scopes.Add(new Uri((item.SubItems[0].Text)));             
            }


            request.Duration = new TimeSpan(0, 0, 0, 10, 0);
            btnProbe.Enabled = false;
            _discoveryClient.FindAsync(request);
        }

        void DiscoveryClient_FindCompleted(object sender, FindCompletedEventArgs e)
        {
            Console.WriteLine("Search completed ({0} results)", e.Result.Endpoints.Count);

            _discoveryClient.FindProgressChanged -= DiscoveryClient_FindProgressChanged;
            _discoveryClient.FindCompleted -= DiscoveryClient_FindCompleted;

            btnProbe.Enabled = true;
        }
                
        void DiscoveryClient_FindProgressChanged(object sender, FindProgressChangedEventArgs e)
        {
            Console.WriteLine("Found service at: " + e.EndpointDiscoveryMetadata.Address);

            ListViewItem item = new ListViewItem(e.EndpointDiscoveryMetadata.Address.ToString());
            lvDevices.Items.Add(item);

        }

        List<DiscoveryType> _types = new List<DiscoveryType>();

        void InitializeTypes()
        {
            DiscoveryType type12 = new DiscoveryType(){Type = ONVIF_DISCOVER_TYPES, Namespace = ONVIF_NETWORK_WSDL_URL};
            _types.Add(type12);
                        
            DiscoveryType type20 = new DiscoveryType(){Type = ONVIF_20_DEVICE_TYPE, Namespace = ONVIF_20_DEVICE_NS};
            _types.Add(type20);
   
        }

        List<string> _scopes = new List<string>();

        void InitializeScopes()
        {
            _scopes.Add("onvif://www.onvif.org/type/video_encoder");
        }
              
        public const string ONVIF_DISCOVER_TYPES = "NetworkVideoTransmitter";
        public const string ONVIF_20_DEVICE_TYPE = "Device";
        public const string ONVIF_NETWORK_WSDL_URL = "http://www.onvif.org/ver10/network/wsdl";
        public const string ONVIF_20_DEVICE_NS = "http://www.onvif.org/ver10/device/wsdl";


        #endregion

    }
}

