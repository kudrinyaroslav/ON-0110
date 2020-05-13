using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatedTesting.GUI.ExternalData;

namespace AutomatedTesting.GUI.Data
{
    class Task
    {
        public TestParameters Parameters { get; set; }
        public TestCaseSettings FeatureDefnitionSettings { get; set; }

        List<TestCaseSettings> _tests = new List<TestCaseSettings>();
        public List<TestCaseSettings> Tests
        {
            get { return _tests; }
        }

    }
}
