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
using System.Xml;
using System.Collections.Generic;

namespace SMC.Pages
{
    public partial class CredentialManagement : BaseSmcControl
    {
        CustomBinding _custombindingSoap12;

        public CredentialManagement()
        {
            InitializeComponent();

            tvCredentials.AfterSelect += new TreeViewEventHandler(tvCredentials_AfterSelect);
            DisplayCredential(null);

            _custombindingSoap12 = new CustomBinding();
            _custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            _custombindingSoap12.Elements.Add(new HttpTransportBindingElement());
        }

        #region Client

        private CredentialPortClient _credentialClient;

        protected CredentialPortClient CredentialClient
        {
            get
            {
                if (_credentialClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.CredentialServiceAddress);
                    _credentialClient = new CredentialPortClient(binding, address);
                }
                return _credentialClient;
            }

        }

        protected void CheckClientValid()
        {
            if (_credentialClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.CredentialServiceAddress;
                if (address != _credentialClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _credentialClient = null;
                }
            }
        }


        protected override void UpdateAddress()
        {
            CheckClientValid();

        }

        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                tvCredentials.Nodes.Clear();

                //Credential[] credentials = CredentialClient.GetCredentials(null);

                List<Credential> credentials = GetList<Credential>(CredentialClient.GetCredentialList);

                foreach (var credential in credentials)
                {
                    {
                        TreeNode credentialNode = new TreeNode(credential.token);
                        credentialNode.Tag = credential;

                        CredentialState state = CredentialClient.GetCredentialState(credential.token);

                        credentialNode.Nodes.Add("Enabled", "Enabled: " + (state.Enabled.ToString()));
                        credentialNode.Nodes.Add("Reason", "Reason: " + (state.Reason));
                        credentialNode.Nodes.Add("AntipassbackState", "AntipassbackState: " + (state.AntipassbackState.AntipassbackViolated.ToString()));

                        tvCredentials.Nodes.Add(credentialNode);
                    }

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        void tvCredentials_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                Credential info = node.Tag as Credential;
                DisplayCredential(info);
            }
            else
            {
                DisplayCredential(null);

            }
        }



        #region Right pane

        private Credential _credential;

        public void DisplayCredential(Credential info)
        {
            _credential = info;

            if (info != null)
            {
                tbToken.Text = info.token;
                tbDescription.Text = info.Description;
                tbCredentialHolderReference.Text = info.CredentialHolderReference;

                if (info.ValidFromSpecified)
                {
                    tbValidFrom.Text = XmlConvert.ToString(info.ValidFrom, XmlDateTimeSerializationMode.Utc);
                }
                else
                {
                    tbValidFrom.Text = "(none)";
                }

                if (info.ValidToSpecified)
                {
                    tbValidTo.Text = XmlConvert.ToString(info.ValidTo, XmlDateTimeSerializationMode.Utc);
                }
                else
                {
                    tbValidTo.Text = "(none)";
                }

                //Credential Identifiers
                lvCredentialIdentifiers.Items.Clear();
                if (info.CredentialIdentifier != null)
                {
                    foreach (var cedentialIdentifier in info.CredentialIdentifier)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Tag = cedentialIdentifier;
                        item.Text = cedentialIdentifier.Type.Name;
                        item.SubItems.Add(cedentialIdentifier.ExemptedFromAuthentication.ToString());

                        if (cedentialIdentifier.Value != null)
                        {
                            item.SubItems.Add("{" + BitConverter.ToString(cedentialIdentifier.Value) + "}");
                        }
                        else
                        {
                            item.SubItems.Add("(none)");
                        }
                        
                        lvCredentialIdentifiers.Items.Add(item);
                    }
                }

                //Credential Access Profiles
                lvCredentialAccessProfiles.Items.Clear();
                if (info.CredentialAccessProfile != null)
                {
                    foreach (var credentialAccessProfile in info.CredentialAccessProfile)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Tag = credentialAccessProfile;
                        item.Text = credentialAccessProfile.AccessProfileToken;

                        if (credentialAccessProfile.ValidFromSpecified)
                        {
                            item.SubItems.Add(XmlConvert.ToString(credentialAccessProfile.ValidFrom, XmlDateTimeSerializationMode.Utc));
                        }
                        else
                        {
                            item.SubItems.Add("(none)");
                        }

                        if (credentialAccessProfile.ValidToSpecified)
                        {
                            item.SubItems.Add(XmlConvert.ToString(credentialAccessProfile.ValidTo, XmlDateTimeSerializationMode.Utc));
                        }
                        else
                        {
                            item.SubItems.Add("(none)");
                        }

                        lvCredentialAccessProfiles.Items.Add(item);
                    }
                }
            }
            else
            {
                tbToken.Text = string.Empty;
                tbCredentialHolderReference.Text = string.Empty;
                tbDescription.Text = string.Empty;
                tbValidFrom.Text = string.Empty;
                tbValidTo.Text = string.Empty;
            }

            foreach (Button btn in new Button[] { })
            {
                btn.Enabled = info != null;
            }
        }

        #endregion
    }
}
