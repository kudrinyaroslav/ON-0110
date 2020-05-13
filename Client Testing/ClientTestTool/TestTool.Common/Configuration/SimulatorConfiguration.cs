using System;
using System.Collections.Generic;
using TestTool.Onvif;

namespace TestTool.Common.Configuration
{


    public class SimulatorConfiguration
    {
        // Services
        public ServicesConfiguration ServicesConfiguration { get; set; }
        
        // DeviceInformation
        public DeviceInformation DeviceInformation { get; set; }
        
        // Scopes
        public List<Scope> Scopes { get; set; }

        // Users
        

        // Doors list
        // AccessPoints list
        // Areas List
        public PacsConfiguration PacsConfiguration { get; set; }


    }
}
