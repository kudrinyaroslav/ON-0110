using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatedTesting.GUI.Data
{
    class TestCaseSettings
    {
        public string FileName { get; set; }
        public string TestCaseID { get; set; }

        public TestTool.Tests.Definitions.Enums.Category? Category { get; set; }
        public string TestID { get; set; }
    }
}
