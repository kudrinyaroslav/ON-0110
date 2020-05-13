using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMC.Proxies;

namespace SMC.SmcData
{
    class DoorsControl
    {
        public DoorsControl()
        {
            _doorInfos = new List<DoorInfo>();
            _doorCapabilities = new Dictionary<string, DoorCapabilities>();
        }

        private List<DoorInfo> _doorInfos;
        
        private Dictionary<string, DoorCapabilities> _doorCapabilities;

        public DoorCapabilities GetDoorCapabilities(string token)
        {
            if (_doorCapabilities.ContainsKey(token))
            {
                return _doorCapabilities[token];
            }
            else
            {
                return null;
            }
        }

        public void SaveDoorCapabilities(string token, DoorCapabilities capabilities)
        {
            _doorCapabilities[token] = capabilities;
        }

        public void SaveDoorInfo(IEnumerable<DoorInfo> infos)
        {
            _doorInfos.AddRange(infos);
            foreach (DoorInfo info  in infos)
            {
                if (info.Capabilities != null)
                {
                    SaveDoorCapabilities(info.token, info.Capabilities);
                }
            }
        }

        public void Clear()
        {
            _doorInfos.Clear();
            _doorCapabilities.Clear();
        }

    }
}
