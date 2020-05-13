using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatedTesting.GUI.Context
{
    public class TreeState
    {
        public TreeState()
        {
            TestFiles = new List<string>();
            FeatureDefinitionFiles = new List<string>();
            ParametersFiles = new List<string>();
            DutTestFiles = new List<string>();
        }

        public List<string> TestFiles { get; set; }
        public List<string> FeatureDefinitionFiles { get; set; }
        public List<string> ParametersFiles { get; set; }
        public List<string> DutTestFiles { get; set; }
    }
}
