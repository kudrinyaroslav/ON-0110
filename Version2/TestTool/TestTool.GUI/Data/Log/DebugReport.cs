using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TestTool.GUI.Data
{
    [Serializable]
    [XmlRoot("DebugReport")]
    public class DebugReport<T>
    {
        public DeviceInfo DeviceInfo { get; set; }

        public DateTime ExecutionTime { get; set; }

        public Log.ManagementSettings ManagementSettings { get; set; }

        public T Results { get; set; }
    }
}
