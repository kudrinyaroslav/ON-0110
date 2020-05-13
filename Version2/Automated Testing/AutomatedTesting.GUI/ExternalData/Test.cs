using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatedTesting.GUI.ExternalData
{
    public class Test
    {
        [System.Xml.Serialization.XmlAttribute("Category")]
        public TestTool.Tests.Definitions.Enums.Category Category { get; set; }

        [System.Xml.Serialization.XmlAttribute("ID")]
        public string TestID { get; set; }

        public string DefaultFileName { get; set; }
        public List<TestCase> TestCases { get; set; }
    }
}
