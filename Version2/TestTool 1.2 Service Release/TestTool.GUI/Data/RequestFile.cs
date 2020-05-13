///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using System.IO;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Request file.
    /// </summary>
    public class RequestFile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileInfo">System file nformation.</param>
        public RequestFile (FileInfo fileInfo)
        {
            _path = fileInfo.FullName;
            _fileName = fileInfo.Name;
        }

        private string _fileName;
        /// <summary>
        /// File name.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        private string _content;
        /// <summary>
        /// File XML content.
        /// </summary>
        public string Content
        {
            get 
            { 
                if (string.IsNullOrEmpty(_content))
                {
                    LoadContent();
                }
                return _content; 
            } 
        }

        /// <summary>
        /// Loads content
        /// </summary>
        void LoadContent()
        {
            FileStream f = File.OpenRead(_path);
            TextReader rdr = new StreamReader(f);
            _content = rdr.ReadToEnd();
            rdr.Close();
            f.Close();
            f.Dispose();
        }

        private string _path;

        /// <summary>
        /// File full name.
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

    }
}
