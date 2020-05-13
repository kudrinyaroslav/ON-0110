///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestTool.GUI.Enums;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using System.IO;
using TestTool.HttpTransport;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Common.TestEngine;
using TestTool.GUI.Utils;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Holds GUI logic for Requests tab
    /// </summary>
    class RequestsController : Controller<IRequestsView> 
    {
        /// <summary>
        /// Initialized object.
        /// </summary>
        /// <param name="view">View.</param>
        public RequestsController(IRequestsView view)
            :base(view)
        {
            _serviceAddresses = new Dictionary<DutService, string>();
        }

        /// <summary>
        /// Folders with other folders and request files.
        /// </summary>
        List<RequestFolder> _folders = new List<RequestFolder>();
        /// <summary>
        /// Request files in the root folder.
        /// </summary>
        List<RequestFile> _rootFiles = new List<RequestFile>();

        /// <summary>
        /// Provides ability to find RequestFolder by path;
        /// </summary>
        Dictionary<string, RequestFolder> _foldersList= new Dictionary<string, RequestFolder>();
        
        /// <summary>
        /// Used to stop requests execution.
        /// </summary>
        Tests.Common.TestEngine.TestSemaphore _semaphore = new TestSemaphore();

        /// <summary>
        /// Updates context when a user leaves corresponding view.
        /// </summary>
        public override void UpdateContext()
        {
            RequestsInfo info = new RequestsInfo();
            info.Service = View.Service;
            info.ServiceAddress = View.ServiceAddress;
            ContextController.UpdateRequestsInfo(info);
        }

        /// <summary>
        /// Loads saved context.
        /// </summary>
        /// <param name="context">Saved data.</param>
        public override void LoadSavedContext(SavedContext context)
        {
            if (context.RequestsInfo != null)
            {
                View.Service = context.RequestsInfo.Service;
                View.ServiceAddress = context.RequestsInfo.ServiceAddress;
                _serviceAddresses[context.RequestsInfo.Service] = context.RequestsInfo.ServiceAddress;
            }
        }

        /// <summary>
        /// Performs initialization.
        /// </summary>
        public override void Initialize()
        {
            string thisApplicationData = RequestsRootPath;

            if (Directory.Exists(thisApplicationData))
            {
                RequestFolder root = new RequestFolder(new DirectoryInfo(thisApplicationData));

                root.Load();
               
                _folders.AddRange(root.Folders);
                foreach (RequestFolder folder in root.Folders)
                {
                    AddFolderToDictionary(folder);
                }

                _rootFiles.AddRange(root.Requests);

                View.DisplayFolders(root);
            }
        }

        /// <summary>
        /// True if request is in progress.
        /// </summary>
        public override bool RequestPending
        {
            get
            {
                return _requestPending;
            }
        }

        /// <summary>
        /// True if shutdown is in progress.
        /// </summary>
        private bool _shutdown;

        /// <summary>
        /// Interrupts pending request.
        /// </summary>
        public override void Stop()
        {
            _shutdown = true;
            if (_requestPending)
            {
                _semaphore.Stop();
            }
        }

        /// <summary>
        /// Updates view when a user switches to this page.
        /// </summary>
        public override void UpdateView()
        {
            View.Request = ApplySecurity(View.Request);
        }

        /// <summary>
        /// Adds folder to folders dictionary.
        /// </summary>
        /// <param name="folder">Folder information.</param>
        void AddFolderToDictionary(RequestFolder folder)
        {
            _foldersList.Add(folder.Path, folder);
            foreach (RequestFolder child in folder.Folders)
            {
                AddFolderToDictionary(child);
            }
        }

        /// <summary>
        /// Folder with requests.
        /// </summary>
        private string _requestsRootPath = null;

        /// <summary>
        /// Folder with requests.
        /// </summary>
        public string RequestsRootPath
        {
            get
            {
                if (_requestsRootPath == null)
                {
                    string allUsersFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    _requestsRootPath = Path.Combine(allUsersFolder, @"ONVIF\ONVIF Device Test Tool\Requests");
                }
                return _requestsRootPath;
            } 

        }

        /// <summary>
        /// Service addresses
        /// </summary>
        private Dictionary<Enums.DutService, string> _serviceAddresses;

        /// <summary>
        /// Adds "wsse:security" element to a message, or replaces existing element.
        /// </summary>
        /// <param name="request">Request string.</param>
        /// <returns>Request string with added (updated) "wsse:security" element.</returns>
        public string ApplySecurity(string request)
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            DebugInfo info = ContextController.GetDebugInfo();

            string secureRequest = request;

            if (info.Security == HttpTransport.Interfaces.Security.WS)
            {
                if (!string.IsNullOrEmpty(env.Credentials.UserName))
                {
                    SoapHelper soapHelper = new SoapHelper();
                    secureRequest = 
                        soapHelper.ApplySecurity(
                        request, 
                        env.Credentials.UserName, 
                        env.Credentials.Password, 
                        env.Credentials.UseUTCTimeStamp, 
                        true);
                }
            }
            else
            {
                SoapHelper soapHelper = new SoapHelper();
                secureRequest = soapHelper.RemoveSecurity(request);
            }
            return secureRequest;
        }

        public void UpdateRequestSecurity()
        {
            string request = View.Request;
            View.Request = ApplySecurity(request);
        }

        /// <summary>
        /// Send request.
        /// </summary>
        public void SendRequest()
        {
            ReportOperationStarted();

            _serviceAddresses[View.Service] = View.ServiceAddress;
           
            _request = View.Request;
            _address = View.ServiceAddress;

            System.Threading.Thread thread = new Thread(SendRequestInternal);
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();
        }

        /// <summary>
        /// Request string.
        /// </summary>
        private string _request;

        /// <summary>
        /// Service address.
        /// </summary>
        private string _address;

        /// <summary>
        /// True if request is in progress.
        /// </summary>
        private bool _requestPending = false;

        /// <summary>
        /// Sends request.
        /// </summary>
        void SendRequestInternal()
        {
            try
            {
                DeviceEnvironment env = ContextController.GetDeviceEnvironment();
                CredentialsProvider cp = new CredentialsProvider();
                cp.Security = ContextController.GetDebugInfo().Security;
                cp.Username = env.Credentials.UserName;
                cp.Password = env.Credentials.Password;

                HttpClient client = new HttpClient(_address, env.Timeouts.Message, _semaphore, cp);
                _requestPending = true;
                string response = client.SendSoapMessage(_request);
                if (!_shutdown)
                {
                    View.DisplayResponse(response);
                }
            }
            catch (Exception exc)
            {
                if (!_shutdown)
                {
                    View.DisplayResponse(exc.Message);
                }
            }
            finally
            {
                _requestPending = false;
                if (!_shutdown)
                {
                    ReportOperationCompleted();
                }
            }
        }

        /// <summary>
        /// Selects service. Updates service address.
        /// </summary>
        /// <param name="service">Service</param>
        public void SelectService(DutService service)
        {
            if (_serviceAddresses.ContainsKey(service))
            {
                View.ServiceAddress = _serviceAddresses[service];
            }
            else
            {
                if (service == DutService.DeviceManagement)
                {
                    View.ServiceAddress = "/onvif/device_service";
                }
                else
                {
                    View.ServiceAddress = string.Empty;
                }
            }
        }

        /// <summary>
        /// Saves request.
        /// </summary>
        /// <param name="path">Path to desired file.</param>
        /// <param name="content">Request string.</param>
        /// <returns>Structure with request file data.</returns>
        public RequestFile SaveRequest(string path, string content)
        {
            if (File.Exists(path))
            {
                File.Delete(path);

                string dir = Path.GetDirectoryName(path);
                if (_foldersList.ContainsKey(dir))
                {
                    RequestFolder rf = _foldersList[dir];
                    RequestFile file = rf.Requests.Where(r => r.FileName == Path.GetFileName(path)).FirstOrDefault();
                    if (file != null)
                    {
                        rf.Requests.Remove(file);
                        View.DeleteFile(file, rf);
                    }
                }
                else
                {
                    RequestFile file = _rootFiles.Where(r => r.FileName == Path.GetFileName(path)).FirstOrDefault();
                    View.DeleteFile(file, null);
                }
            }
            FileStream fs = File.Create(path);
            TextWriter tw = new StreamWriter(fs);
            tw.Write(content);
            tw.Close();
            
            return AddFileToHierarchy(path);
        }
        
        /// <summary>
        /// Adds new request to hierarchy.
        /// </summary>
        /// <param name="path">Request path.</param>
        /// <returns>Structure with request data.</returns>
        RequestFile AddFileToHierarchy(string path)
        {
            if (!path.StartsWith(RequestsRootPath))
            {
                RequestFile requestFile = new RequestFile(new FileInfo(path));
                View.AddFile(requestFile, null);
                return requestFile;
            }

            // if no directory to add file exists in the hierarchy - go upstream until 
            // parent folder is found.

            // folders to create
            List<string> newFolders = new List<string>();
            // current folder
            string folderPath = Path.GetDirectoryName(path);
            // existing folder
            RequestFolder existing = null;

            // Go to the parent folder. Stop at the root or at some existing folder.
            while (!_foldersList.ContainsKey(folderPath) && (folderPath != _requestsRootPath))
            {
                int idx = folderPath.LastIndexOf(Path.DirectorySeparatorChar);

                System.Diagnostics.Debug.WriteLine(string.Format("Create directory: {0}", folderPath.Substring(idx + 1)));
                newFolders.Insert(0, folderPath.Substring(idx + 1));
                folderPath = folderPath.Substring(0, idx);

                System.Diagnostics.Debug.WriteLine(string.Format("Check if exists: {0}", folderPath));

                if (!folderPath.Contains(Path.DirectorySeparatorChar))
                {
                    // user wants to create file out of hierarchy - create in root folder!
                    folderPath = RequestsRootPath;
                    newFolders.Clear();
                    break;
                }
            }

            if (_foldersList.ContainsKey(folderPath))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Existing: {0}", folderPath));
                existing = _foldersList[folderPath];
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Add as root folder/file");
            }

            // create new folders

            // top new folder
            RequestFolder topFolder = null;

            // folder to add file in
            RequestFolder parentFolder = existing;

            // enumerate new folders.
            foreach (string folder in newFolders)
            {
                // next name
                string newDirectory = Path.Combine(folderPath, folder);

                System.Diagnostics.Debug.WriteLine(string.Format("Create {0} in {1}", folder, folderPath));

                // create directory
                DirectoryInfo directory = Directory.CreateDirectory(newDirectory);
                // add to the hierarchy
                RequestFolder requestFolder = new RequestFolder(directory);
                // save top folder
                if (topFolder == null)
                {
                    topFolder = requestFolder;
                }
                // add to parent folder or directly to the list
                if (_foldersList.ContainsKey(folderPath))
                {
                    _foldersList[folderPath].Folders.Add(requestFolder);
                }
                else
                {
                    _folders.Add(requestFolder);
                }
                // add to the dictionary
                _foldersList.Add(requestFolder.Path, requestFolder);

                // change current path
                folderPath = newDirectory;
                // folder where file should be added
                parentFolder = requestFolder;
            }

            // create file structure
            RequestFile newFile = new RequestFile(new FileInfo(path));
            // add to parent folder
            if (parentFolder != null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Add file to: {0}", parentFolder.Path));
                parentFolder.Requests.Add(newFile);
            }
            else
            {
                _rootFiles.Add(newFile);
            }
            // add file in the view
            if (topFolder == null)
            {
                // add file only
                View.AddFile(newFile, existing);
            }
            else
            {
                // add top folder. Other will be added automatically.
                System.Diagnostics.Debug.WriteLine(string.Format("Add folder {0} to {1}", topFolder.Name, existing == null ? "TOP" : existing.Path));

                View.AddFolder(topFolder, existing);
            }

            return newFile;
        }

        /// <summary>
        /// Deletes request file.
        /// </summary>
        /// <param name="file">Request file information.</param>
        public void DeleteRequest(RequestFile file)
        {
            if (File.Exists(file.Path))
            {
                File.Delete(file.Path);
            }
            else
            {
                throw new FileNotFoundException("File not found", file.Path);
            }
        }
    }
}
