///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Folder with request files and folders.
    /// </summary>
    public class RequestFolder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Directory system information</param>
        public RequestFolder(DirectoryInfo info)
        {
            _path = info.FullName;
            _name = info.Name;
        }

        private string _path;
        
        /// <summary>
        /// Full path
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        private string _name;

        /// <summary>
        /// Directory name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        List<RequestFolder> _folders = new List<RequestFolder>();
        
        /// <summary>
        /// Subfolders
        /// </summary>
        public List<RequestFolder> Folders
        {
            get { return _folders; }
        }

        List<RequestFile> _files = new List<RequestFile>();
        /// <summary>
        /// Files contained in this folder.
        /// </summary>
        public List<RequestFile> Requests
        {
            get { return _files; }
        }

        /// <summary>
        /// Loads content.
        /// </summary>
        public void Load()
        {
            _folders.Clear();
            _files.Clear();

            DirectoryInfo info = new DirectoryInfo(_path);
            foreach (DirectoryInfo child in info.GetDirectories().OrderBy(di => di.Name) )
            {
                RequestFolder folder = new RequestFolder(child);
                _folders.Add(folder);
                folder.Load();
            }

            foreach (FileInfo file in info.GetFiles("*.xml").OrderBy( fi => fi.Name) )
            {
                RequestFile requestFile = new RequestFile(file);
                _files.Add(requestFile);
            }

        }
    }
}
