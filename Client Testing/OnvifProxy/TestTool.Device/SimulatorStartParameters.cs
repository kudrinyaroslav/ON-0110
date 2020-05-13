using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Device
{
    public class SimulatorStartParameters
    {
        public string IPAddress { get; set; }
        public string DeviceAddress {get;set;}

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
