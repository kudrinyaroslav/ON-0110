using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class NameAttribute : System.Attribute
    {
        protected string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value;}
        }

        public NameAttribute()
        {

        }

        public NameAttribute(string name)
        {
            _name = name;
        }
    }
}
