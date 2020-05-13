using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SMC.Proxies;
using SMC.Proxies.Monitoring;
using SMC.StateMonitoring;
using System.Collections.Generic;

namespace SMC.Pages
{
    public partial class AccessPointsManagement : BaseSmcControl
    {
        CustomBinding _custombindingSoap12;

        public AccessPointsManagement()
        {
            InitializeComponent();

            tvAccessPoints.AfterSelect += new TreeViewEventHandler(tvDoors_AfterSelect);
            DisplayAccessPointInfo(null);

            _custombindingSoap12 = new CustomBinding();
            _custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            _custombindingSoap12.Elements.Add(new HttpTransportBindingElement());

        }

        #region Client

        private PACSPortClient _pacsClient;

        protected PACSPortClient PACSClient
        {
            get
            {
                if (_pacsClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.PACSServiceAddress);
                    _pacsClient = new PACSPortClient(binding, address);
                }
                return _pacsClient;
            }

        }

        protected void CheckClientValid()
        {
            if (_pacsClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.PACSServiceAddress;
                if (address != _pacsClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _pacsClient = null;
                }
            }
        }


        protected override void UpdateAddress()
        {
            CheckClientValid();

        }

        #endregion

        void tvDoors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null )
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                AccessPointInfo info = node.Tag as AccessPointInfo;
                DisplayAccessPointInfo(info);
            }
            else
            {
                DisplayAccessPointInfo(null);

            }            
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                tvAccessPoints.Nodes.Clear();
                
                // old version
                //AccessPointInfo[] infos = PACSClient.GetAccessPointInfo(null);
               
                List<AccessPointInfo> infos = GetList<AccessPointInfo>(PACSClient.GetAccessPointInfoList);

                foreach (var info in infos)
                {
                    TreeNode doorNode = new TreeNode(info.token);
                    doorNode.Tag = info;
                    tvAccessPoints.Nodes.Add(doorNode); 
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }


        #region Right pane

        private AccessPointInfo _accessPoint;

        public void DisplayAccessPointInfo(AccessPointInfo info)
        {
            _accessPoint = info;

            if (info != null)
            {
                tbToken.Text = info.token;
                tbName.Text = info.Name;
                tbDescription.Text = info.Description;
                tbAreaFrom.Text = info.AreaFrom;
                tbAreaTo.Text = info.AreaTo;
                tbEntityType.Text = info.EntityType != null ? info.EntityType.Name : "tdc:Door";
                tbEntity.Text = info.Entity;

                AccessPointCapabilities capabilities = info.Capabilities;

                if (capabilities != null)
                {
                    chkAccessTaken.Checked = capabilities.AccessTakenSpecified && capabilities.AccessTaken;
                    chkDisable.Checked = capabilities.DisableAccessPoint;
                    chkTamper.Checked = capabilities.TamperSpecified && capabilities.Tamper;
                    chkExternal.Checked = capabilities.ExternalAuthorizationSpecified && capabilities.ExternalAuthorization;
                    chkAnonymousAccess.Checked = capabilities.AnonymousAccessSpecified && capabilities.AnonymousAccess;
                    chkDuress.Checked = capabilities.DuressSpecified && capabilities.Duress;
                }
            }
            else
            {
                tbToken.Text = string.Empty;
                tbName.Text = string.Empty;
                tbDescription.Text = string.Empty;
                tbAreaFrom.Text = string.Empty;
                tbAreaTo.Text = string.Empty;
                tbEntityType.Text = string.Empty;
                tbEntity.Text = string.Empty;

                chkAccessTaken.Checked = false;
                chkDisable.Checked = false;
                chkTamper.Checked = false;
                chkExternal.Checked = false;
                chkAnonymousAccess.Checked = false;
                chkDuress.Checked = false;
            }

            foreach (Button btn in new Button[] { })
            {
                btn.Enabled = info != null;
            }
        }

        #endregion

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (_accessPoint != null)
            {
                SafeInvoke(() => { PACSClient.DisableAccessPoint(_accessPoint.token); });
            }
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            if (_accessPoint != null)
            {
                SafeInvoke(() => { PACSClient.EnableAccessPoint(_accessPoint.token); });
            }
        }
        



    }
}
