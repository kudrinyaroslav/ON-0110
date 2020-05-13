using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel.Discovery;

namespace ONVIFSampleApp
{
    public partial class ProbeDialog : Form
    {
        public ProbeDialog()
        {

            InitializeComponent();

        }

        public ListView TypesList
        {
            get { return lvTypes; }
        }

        public ListView ScopesList
        {
            get { return lvScopes; }
        }

        public DiscoveryVersion DiscoveryVersion
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
                
        
        internal void InitializeTypes(IEnumerable<DiscoveryType> types)
        {
            foreach (DiscoveryType type in types)
            { 
                ListViewItem item = new ListViewItem(type.Type);
                item.SubItems.Add(type.Namespace);
                lvTypes.Items.Add(item);          
            }

        }

        public void InitializeScopes(IEnumerable<string> scopes)
        {
            foreach (string scope in scopes)
            { 
                ListViewItem scope1 = new ListViewItem(scope);
                lvScopes.Items.Add(scope1);             
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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvScopes.SelectedItems)
            {
                lvScopes.Items.Remove(item);
            }
        }

        private void lvTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveType.Enabled = lvTypes.SelectedItems.Count > 0;
        }

        private void lvScopes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = lvScopes.SelectedItems.Count > 0;
        }

    }
}
