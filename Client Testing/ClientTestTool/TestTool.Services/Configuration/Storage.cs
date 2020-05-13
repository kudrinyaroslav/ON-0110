using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Common.Configuration;

namespace TestTool.Services.Configuration
{
    class Storage
    {
        private static SimulatorConfiguration _instance;

        public static SimulatorConfiguration Current
        {
            get
            {
                return _instance;
            }
        }

        public static void Load(SimulatorConfiguration configuration)
        {
            _instance = configuration;
        }
    }
}
