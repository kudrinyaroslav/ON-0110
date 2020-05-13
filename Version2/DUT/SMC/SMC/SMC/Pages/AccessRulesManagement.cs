using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMC.Controls;
using SMC.Proxies;
using SMC.Proxies.Monitoring;
using SMC.StateMonitoring;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace SMC.Pages
{
    public partial class AccessRulesManagement : BaseSmcControl
    {

        CustomBinding _custombindingSoap12;

        public AccessRulesManagement()
        {
            InitializeComponent();

            tvAccessProfiles.AfterSelect += new TreeViewEventHandler(tvAccessProfiles_AfterSelect);
            DisplayAccessProfile(null);

            _custombindingSoap12 = new CustomBinding();
            _custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            _custombindingSoap12.Elements.Add(new HttpTransportBindingElement());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                tvAccessProfiles.Nodes.Clear();

                //AccessProfile[] accessProfiles = AccessRulesClient.GetAccessProfiles(null);

                List<AccessProfile> accessProfiles = GetList<AccessProfile>(AccessRulesClient.GetAccessProfileList);

                foreach (var accessProfile in accessProfiles)
                {
                    {
                        TreeNode accessProfileNode = new TreeNode(accessProfile.token);
                        accessProfileNode.Tag = accessProfile;

                        tvAccessProfiles.Nodes.Add(accessProfileNode);
                    }

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        #region Client

        private AccessRulesPortClient _accessRulesClient;

        protected AccessRulesPortClient AccessRulesClient
        {
            get
            {
                if (_accessRulesClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.AccessRulesServiceAddress);
                    _accessRulesClient = new AccessRulesPortClient(binding, address);
                }
                return _accessRulesClient;
            }

        }

        protected void CheckClientValid()
        {
            if (_accessRulesClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.AccessRulesServiceAddress;
                if (address != _accessRulesClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _accessRulesClient = null;
                }
            }
        }


        protected override void UpdateAddress()
        {
            CheckClientValid();

        }

        #endregion


        void tvAccessProfiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                AccessProfile info = node.Tag as AccessProfile;
                DisplayAccessProfile(info);
            }
            else
            {
                DisplayAccessProfile(null);

            }
        }



        #region Right pane

        private AccessProfile _accessProfile;

        public void DisplayAccessProfile(AccessProfile info)
        {
            _accessProfile = info;

            if (info != null)
            {
                tbToken.Text = info.token;
                tbName.Text = info.Name;
                tbDescription.Text = info.Description;

                //Access Policies

                lvAccessPolicies.Items.Clear();
                if (info.AccessPolicy != null)
                {
                    foreach (var accessPolisy in info.AccessPolicy)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Tag = accessPolisy;
                        item.Text = accessPolisy.ScheduleToken;
                        item.SubItems.Add(accessPolisy.Entity);
                        if (accessPolisy.EntityType != null)
                        {
                            item.SubItems.Add(accessPolisy.EntityType.Name);
                            item.SubItems.Add(accessPolisy.EntityType.Namespace);
                        }
                        else
                        {
                            item.SubItems.Add("(default)");
                        }
                        lvAccessPolicies.Items.Add(item);
                    }
                }
            }
            else
            {
                tbToken.Text = string.Empty;
                tbName.Text = string.Empty;
                tbDescription.Text = string.Empty;
            }

            foreach (Button btn in new Button[] { })
            {
                btn.Enabled = info != null;
            }
        }

        #endregion

    }
}
