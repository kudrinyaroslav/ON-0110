using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace TestTool.GUI.Data
{
    [Serializable]
    [XmlRoot("WarningsReport")]
    public class WarningsReport<T>
    {
        public DateTime ExecutionTime { get; set; }
        public T Results { get; set; }
    }

    [Serializable]
    public class TestResultWithWarnings
    {
        public string TestName { get; set; }
        public string TestStatus { get; set; }
        public List<string> TestWarnings { get; set; }
    }
}

