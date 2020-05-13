using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.TestCases.TestSuites
{
    class SimpleItemDescription
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Namespace { get; set; }
        public bool Mandatory { get; set; }

        public SimpleItemDescription()
            : this(null, null, null)
        { 
        
        }

        public SimpleItemDescription(string name, string type, string ns)
            : this(name, type, ns, true)
        {

        }

        public SimpleItemDescription(string name, string type, string ns, bool mandatory)
        {
            Name = name;
            Namespace = ns;
            Type = type;
            Mandatory = mandatory;
        }


    }

    class MessageDescription
    {
        public MessageDescription()
        {
            _dataSimpleItems = new Dictionary<string, SimpleItemDescription>();
        }

        Dictionary<string, SimpleItemDescription> _dataSimpleItems;

        public Dictionary<string, SimpleItemDescription> DataSimpleItems
        {
            get { return _dataSimpleItems; }
        }

        public bool IsProperty { get; set; }

        public void AddSimpleItem(string name, string type, string ns)
        {
            AddSimpleItem(name, type, ns, true);
        }

        public void AddSimpleItem(string name, string type, string ns, bool mandatory)
        {
            SimpleItemDescription item = new SimpleItemDescription(name, type, ns, mandatory);
            _dataSimpleItems.Add(name, item);
        }
    }
}
