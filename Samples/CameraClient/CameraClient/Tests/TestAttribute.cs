using System;

namespace CameraClient.Tests
{
    public class TestAttribute : System.Attribute
    {
        protected string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

    }
}
