///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Common.Discovery
{
    public class DeviceDiscoveryData
    {
        public string EndPointAddress { get; set; }
        public string Type { get; set; }
        public List<string> ServiceAddresses { get; protected set; }
        public string Scopes { get; set; }
        public uint MetadataVersion { get; set; }
        public string UUID { get; set; }

        public DeviceDiscoveryData()
        {
            ServiceAddresses = new List<string>();
        } 
    }
}
