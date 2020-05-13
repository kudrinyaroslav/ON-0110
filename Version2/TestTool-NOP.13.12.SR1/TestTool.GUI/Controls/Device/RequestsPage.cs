///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using System.IO;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using System.ComponentModel;

namespace TestTool.GUI.Controls.Device
{
    partial class RequestsPage : Page, IRequestsView
    {
        private RequestsController _controller;

        public RequestsPage()
        {
            InitializeComponent();
            _controller = new RequestsController(this);
            InitPage();
        }

        internal RequestsController Controller
        {
            get { return _controller; }
        }

        public override void SwitchToState(Enums.ApplicationState state)
        {
            Control[] controls = new Control[]
                                     {
                                         cmbService, 
                                         tbServiceAddress, 
                                         tvTemplates,
                                         btnAddRequestToTemplates,
                                         btnSendRequest,
                                         btnRequestFile,
                                         tbRequestFile,
                                         tbRequest,
                                         tbResponse
                                     };

            if (state.IsActive())
            {
                BeginInvoke( new Action ( () =>
                                              {
                                                  DisableControls(controls);
                                                  btnDelete.Enabled = false;
                                              } ) );
            }
            else
            {
                BeginInvoke(new Action(() =>
                                           {
                                               EnableControls(controls);
                                                TreeNode node = tvTemplates.SelectedNode;
                                                if (node != null)
                                                {
                                                    RequestFile file = node.Tag as RequestFile;
                                                    btnDelete.Enabled = (file != null);
                                                }
                                                else
                                                {
                                                    btnDelete.Enabled = false;
                                                }
                                           }));

            }
        }

        #region IView Members

        public override IController GetController()
        {
            return _controller;
        }

        #endregion

        #region IRequestsView Members

        public Enums.DutService Service
        {
            get
            {
                ServiceListItem item = (ServiceListItem) cmbService.SelectedItem;
                return item.Service;
            }
            set
            {
                List<ServiceListItem> items = (List<ServiceListItem>) cmbService.DataSource;
                cmbService.SelectedItem = items.Where(i => i.Service == value).FirstOrDefault();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ServiceAddress
        {
            get
            {
                return tbServiceAddress.Text;
            }
            set
            {
                tbServiceAddress.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Request
        {
            get
            {
                return tbRequest.Text;
            }
            set
            {
                tbRequest.Text = value;
            }
        }

        public void DisplayResponse(string response)
        {
            BeginInvoke(new Action(() => { tbResponse.Text = response; }));
        }

        Dictionary<RequestFolder, TreeNode> _folderNodes = new Dictionary<RequestFolder, TreeNode>();

        public void DisplayFolders(RequestFolder rootFolder)
        {
            tvTemplates.Nodes.Clear();
            _folderNodes.Clear();
            _requestNodes.Clear();
            
            foreach (RequestFolder folder in rootFolder.Folders)
            {
                AddFolderNode(null, folder);
            }
            foreach (RequestFile file in rootFolder.Requests)
            {
                AddRequestNode(null, file);
            }
        }

        void AddFolderNode(TreeNode parent, RequestFolder folder)
        {
            TreeNode node = new TreeNode(folder.Name);
            node.Tag = folder;
            _folderNodes.Add(folder, node);

            if (parent == null)
            {
                tvTemplates.Nodes.Add(node);
            }
            else
            {
                parent.Nodes.Add(node);
            }

            foreach (RequestFolder child in folder.Folders)
            {
                AddFolderNode(node, child);
            }
            foreach (RequestFile file in folder.Requests)
            {
                AddRequestNode(node, file);
            }
        }

        Dictionary<RequestFile, TreeNode> _requestNodes = new Dictionary<RequestFile, TreeNode>();
        
        void AddRequestNode(TreeNode parent, RequestFile file)
        {
            TreeNode requestNode = new TreeNode(file.FileName);
            requestNode.Tag = file;
            if (parent != null)
            {
                parent.Nodes.Add(requestNode);
            }
            else
            {
                tvTemplates.Nodes.Add(requestNode);
            }
            _requestNodes.Add(file, requestNode);
        }

        public void AddFolder(RequestFolder folder, 
            RequestFolder parentFolder)
        {
            if (parentFolder == null)
            {
                AddFolderNode(null, folder);
            }
            else
            {
                TreeNode parent = _folderNodes[parentFolder];
                AddFolderNode(parent, folder);
            }
        }

        public void AddFile(RequestFile file, 
            RequestFolder parentFolder)
        {
            if (parentFolder == null)
            {
                AddRequestNode(null, file);
            }
            else
            {
                TreeNode parent = _folderNodes[parentFolder];
                AddRequestNode(parent, file);
            }
        }

        public void DeleteFile(RequestFile file, RequestFolder parentFolder)
        {
            TreeNode node = _requestNodes[file];
            if (parentFolder == null)
            {
                    tvTemplates.Nodes.Remove(node);
            }
            else
            {
                TreeNode parent = _folderNodes[parentFolder];
                parent.Nodes.Remove(node);
            }
        }


        #endregion

        void InitPage()
        {
            List<ServiceListItem> items = new List<ServiceListItem>();
            items.Add(new ServiceListItem(Enums.DutService.DeviceManagement));
            items.Add(new ServiceListItem(Enums.DutService.Media));
            items.Add(new ServiceListItem(Enums.DutService.Event));
            items.Add(new ServiceListItem(Enums.DutService.PTZ));
            items.Add(new ServiceListItem(Enums.DutService.Imaging));
            items.Add(new ServiceListItem(Enums.DutService.AccessControl));
            items.Add(new ServiceListItem(Enums.DutService.DoorControl));
            items.Add(new ServiceListItem(Enums.DutService.User));
            cmbService.DataSource = items;
            cmbService.DisplayMember = "DisplayName";
        }

        class ServiceListItem
        {
            public Enums.DutService Service { get; private set; }
            public String DisplayName { get; private set; }

            public ServiceListItem(Enums.DutService service)
            {
                Service = service;
                switch (service)
                {
                    case Enums.DutService.DeviceManagement:
                        DisplayName = "Device Management";
                        break;
                    case Enums.DutService.Event:
                        DisplayName = "Event";
                        break;
                    case Enums.DutService.Media:
                        DisplayName = "Media";
                        break;
                    case Enums.DutService.PTZ:
                        DisplayName = "PTZ";
                        break;
                    case Enums.DutService.AccessControl:
                        DisplayName = "Access Control";
                        break;
                    case Enums.DutService.DoorControl:
                        DisplayName = "Door Control";
                        break;
                    default:
                        DisplayName = service.ToString();
                        break;
                }
            }
        }

        private void btnRequestFile_Click(object sender, EventArgs e)
        { 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Multiselect = false;
            dlg.DefaultExt = "xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = dlg.FileName;
                tbRequestFile.Text = path;

                FileStream f = File.OpenRead(path);
                TextReader rdr = new StreamReader(f);
                string request = rdr.ReadToEnd();
                tbRequest.Text = _controller.ApplySecurity(request);
                rdr.Close();
                f.Close();
                f.Dispose();
            }

        }

        private void cmbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceListItem item = (ServiceListItem)cmbService.SelectedItem;
            _controller.SelectService(item.Service);
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            _controller.SendRequest();
        }

        private void tvTemplates_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = tvTemplates.SelectedNode;
            if (node != null)
            {
                RequestFile file = node.Tag as RequestFile;
                btnDelete.Enabled = (file != null);
                if (file != null)
                {
                    try
                    {
                        tbRequest.Text = _controller.ApplySecurity(file.Content);
                    }
                    catch (Exception)
                    {
                        DialogResult dr = MessageBox.Show(
                                "An error occurred during loading the file. Remove this request from the hierarchy?",
                                "Unable to load request", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {
                            node.Remove();
                        }

                    }

                    return;
                }
            }

            btnDelete.Enabled = false;
            tbRequest.Text = string.Empty;
        }

        private void btnAddRequestToTemplates_Click(object sender, EventArgs e)
        {
            SaveRequest();
        }

        RequestFile SaveRequest()
        {
            TreeNode node = tvTemplates.SelectedNode;

            string initialPath = null;

            if (node != null)
            {
                RequestFolder f = node.Tag as RequestFolder;
                if (f != null)
                {
                    // folder is selected - save in this folder
                    initialPath = f.Path;
                }
                else
                {
                    // file is selected
                    TreeNode parent = node.Parent;
                    if (parent != null)
                    {
                        f = parent.Tag as RequestFolder;
                        if (f != null)
                        {
                            // folder is selected - save in this folder
                            initialPath = f.Path;
                        }
                    }
                }
            }
            if (initialPath == null)
            {
                initialPath = _controller.RequestsRootPath;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = initialPath;
            dlg.AddExtension = true;
            dlg.Filter = "XML files | *.xml";
            dlg.DefaultExt = "xml";
            dlg.OverwritePrompt = true;

            RequestFile newFile = null;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = dlg.FileName;

                try
                {
                    newFile = _controller.SaveRequest(fileName, tbRequest.Text);
                }
                catch (Exception)
                {
                    ShowError("An error occured during saving the request. File was not saved.");
                }
            }

            return newFile;
        }
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            TreeNode node = tvTemplates.SelectedNode;
            if (node != null)
            {
                RequestFile file = node.Tag as RequestFile;
                if (file != null)
                {
                    DialogResult dr = MessageBox.Show("Request will be deleted. Continue?", "Question",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            _controller.DeleteRequest(file);
                            tvTemplates.Nodes.Remove(node);
                        }
                        catch(Exception exc)
                        {
                            ShowError(string.Format("Request could not be deleted: {0}", exc.Message));
                        }

                    }
                }

                }
            }
    
    }
}
