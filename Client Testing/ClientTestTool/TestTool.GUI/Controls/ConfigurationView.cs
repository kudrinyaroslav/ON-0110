using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestTool.Onvif;

namespace TestTool.GUI.Controls
{
    public partial class ConfigurationView : Form
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }

        public void Display(Common.Configuration.SimulatorConfiguration config)
        {
            if (config == null)
            {
                return;
            }

            {
                TreeNode deviceInfoNode = new TreeNode("Device Information");
                tvConfiguration.Nodes.Add(deviceInfoNode);

                if (config.DeviceInformation != null)
                {
                    deviceInfoNode.Nodes.Add(string.Format("Brand: {0}", config.DeviceInformation.Brand));
                    deviceInfoNode.Nodes.Add(string.Format("Model: {0}", config.DeviceInformation.Model));
                    deviceInfoNode.Nodes.Add(string.Format("FirmwareVersion: {0}", config.DeviceInformation.FirmwareVersion));
                    deviceInfoNode.Nodes.Add(string.Format("HardwareId: {0}", config.DeviceInformation.HardwareId));
                    deviceInfoNode.Nodes.Add(string.Format("SerialNumber: {0}", config.DeviceInformation.SerialNumber));
                }

                if (config.Scopes != null)
                {
                    TreeNode scopesNode = tvConfiguration.Nodes.Add("Scopes");

                    foreach (TestTool.Onvif.Scope scope in config.Scopes)
                    {
                        scopesNode.Nodes.Add(scope.ScopeItem);
                    }
                }

                if (config.PacsConfiguration != null)
                {
                    TreeNode pacs = tvConfiguration.Nodes.Add("PACS");

                    TreeNode apl = pacs.Nodes.Add("Access Points");
                    if (config.PacsConfiguration.AccessPointInfoList != null)
                    {
                        foreach (AccessPointInfo info in config.PacsConfiguration.AccessPointInfoList)
                        {
                            TreeNode ap = apl.Nodes.Add(info.token);
                            ap.Nodes.Add(string.Format("Token: {0}", info.token));

                            ap.Nodes.Add(string.Format("Name: {0}", info.Name));
                            ap.Nodes.Add(string.Format("Description: {0}", info.Description));
                            ap.Nodes.Add(string.Format("Enabled: {0}", info.Enabled));
                            ap.Nodes.Add(string.Format("Type: {0}", info.Type));
                            ap.Nodes.Add(string.Format("Entity: {0}", info.Entity));
                            ap.Nodes.Add(string.Format("AreaFrom: {0}", info.AreaFrom));
                            ap.Nodes.Add(string.Format("AreaTo: {0}", info.AreaTo));

                            TreeNode capabilities =  ap.Nodes.Add("Capabilities");
                            if (info.Capabilities != null)
                            {
                                capabilities.Nodes.Add(string.Format("DisableAccessPoint: {0}", info.Capabilities.DisableAccessPoint));
                            }
                        }
                    }
                    
                    TreeNode al = pacs.Nodes.Add("Areas");
                    if (config.PacsConfiguration.AreaInfoList != null)
                    {
                        foreach (AreaInfo info in config.PacsConfiguration.AreaInfoList)
                        {
                            TreeNode ai = al.Nodes.Add(info.token);
                            ai.Nodes.Add(string.Format("Token: {0}", info.token));
                            ai.Nodes.Add(string.Format("Name: {0}", info.Name));
                            ai.Nodes.Add(string.Format("Description: {0}", info.Description));
                        }
                    }

                    TreeNode dl = pacs.Nodes.Add("Doors");
                    if (config.PacsConfiguration.DoorInfoList != null)
                    {
                        foreach (DoorInfo info in config.PacsConfiguration.DoorInfoList)
                        {
                            TreeNode door = dl.Nodes.Add(info.token);
                            door.Nodes.Add(string.Format("Token: {0}", info.token));

                            door.Nodes.Add(string.Format("Name: {0}", info.Name));
                            door.Nodes.Add(string.Format("Description: {0}", info.Description));

                            TreeNode capabilities = door.Nodes.Add("Capabilities");
                            if (info.Capabilities != null)
                            {
                                capabilities.Nodes.Add(string.Format("MomentaryAccess: {0}", info.Capabilities.MomentaryAccess));
                                capabilities.Nodes.Add(string.Format("Lock: {0}", info.Capabilities.Lock));
                                capabilities.Nodes.Add(string.Format("Unlock: {0}", info.Capabilities.Unlock));
                                capabilities.Nodes.Add(string.Format("Block: {0}", info.Capabilities.Block));
                                capabilities.Nodes.Add(string.Format("DoubleLock: {0}", info.Capabilities.DoubleLock));
                                capabilities.Nodes.Add(string.Format("LockDown: {0}", info.Capabilities.LockDown));
                                capabilities.Nodes.Add(string.Format("LockOpen: {0}", info.Capabilities.LockOpen));
                            }
                        }
                    }
                }

            }

        }

        private void ConfigurationView_Resize(object sender, EventArgs e)
        {
            btnOK.Left = (this.Width - btnOK.Width)/2;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
