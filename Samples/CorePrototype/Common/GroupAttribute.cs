using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class GroupAttribute : System.Attribute
    {
        protected string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value;}
        }
        public GroupAttribute(string path)
        {
            _path = path;
        }
    }
}
