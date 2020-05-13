using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatedTesting.GUI.ExternalData
{
    public class TestCase
    {
        public string FileName { get; set; }
        [System.Xml.Serialization.XmlAttribute("ID")]
        public string TestCaseID { get; set; }

        [System.Xml.Serialization.XmlAttribute("Timeout")]
        public bool Timeout { get; set; }

    }
}
