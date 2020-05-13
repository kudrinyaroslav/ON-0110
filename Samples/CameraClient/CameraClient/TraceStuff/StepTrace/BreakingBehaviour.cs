using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraClient.TraceStuff.StepTrace
{
    public class BreakingBehaviour
    {
        public BreakingBehaviour(string path, string value, Dictionary<string, string> namespaces)
        {
            NodePath = path;
            NodeValue = value;
            Namespaces = namespaces;
        }

        public string NodePath { get; set; }
        public string NodeValue { get; set; }
        public Dictionary<string,string> Namespaces { get; set; }
    }
}
