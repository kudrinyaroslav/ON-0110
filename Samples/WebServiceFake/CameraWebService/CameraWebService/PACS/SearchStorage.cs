using System.Collections.Generic;
using System.Xml;
using PACS.DoorControl;
using System.Linq;

namespace CameraWebService.PACS
{
    
    public class Storage
    {
        private static Storage _instance;
        public static Storage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Storage();
                }
                return _instance;
            }
        }

        List<DoorInfo> _doorInfos;

        public List<DoorInfo> DoorInfos
        {
            get
            {
                if (_doorInfos == null)
                {
                    _doorInfos = new List<DoorInfo>();

                    _doorInfos.Add(new DoorInfo() { Description = "Reception", Name = "Reception", token = "door001" });
                    _doorInfos.Add(new DoorInfo() { Description = "WC", Name = "WC", token = "door003" });
                    _doorInfos.Add(new DoorInfo() { Description = "Chef's office", Name = "Chef", token = "door025" });

                }

                return _doorInfos;

            }
        }


        public DoorInfo GetDoor(string token)
        {
            DoorInfo info = _doorInfos.FirstOrDefault(D => D.token == token);

            if (info == null)
            {
                CommonUtils.ReturnFault(new string[]{"Sender", "InvalidArgVal", "InvalidToken"});
            }
            return info;
        }

        private Dictionary<string, DoorCapabilities> _capabilities;

        public Dictionary<string, DoorCapabilities> Capabilities
        {
            get
            {
                if (_capabilities == null)
                {
                    _capabilities = new Dictionary<string, DoorCapabilities>();
                    foreach (DoorInfo info in DoorInfos)
                    {
                        _capabilities.Add(info.token, new DoorCapabilities());
                    }
                }
                return _capabilities;
            }
        }

    }
}
