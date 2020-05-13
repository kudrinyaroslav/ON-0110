using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Device.Data
{
    public class ServiceContractInfo
    {
        public ServiceContractInfo()
        {
            OperationsList = new List<string>();
        }

        public string ServiceName { get; set; }
        public List<string> OperationsList { get; private set; }
    }
}
