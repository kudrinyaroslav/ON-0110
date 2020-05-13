using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Data
{
    enum OperationUsage
    {
        NotCovered,
        Passed,
        Failed
    }

    class OperationInfo
    {
        public string OperationName { get; set; }
        public OperationUsage UsageInfo { get; set; }
    }

    class ServiceUsageInformation
    {
        public ServiceUsageInformation()
        {
            OperationsList = new List<OperationInfo>();
        }

        public string ServiceName { get; set; }
        public List<OperationInfo> OperationsList { get; private set; }

    }
}
