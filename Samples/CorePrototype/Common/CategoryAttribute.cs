using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class CategoryAttribute : System.Attribute
    {
        protected string _category;

        public string Category
        {
            get { return _category; }
            set { _category = value;}
        }
        public CategoryAttribute(string category)
        {
            _category = category;
        }

    }
}
