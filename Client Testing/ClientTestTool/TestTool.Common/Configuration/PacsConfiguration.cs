using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Onvif;

namespace TestTool.Common.Configuration
{
    public class PacsConfiguration
    {
        public PacsConfiguration()
        {
            _areaInfoList = new List<AreaInfo>();
            _accessPointInfoList = new List<AccessPointInfo>();
            _doorInfoList = new List<DoorInfo>();

            _doorCapabilitiesList = new Dictionary<string, DoorCapabilities>();
            _doorStateList = new Dictionary<string, DoorState>();
            _doorAccessList = new Dictionary<string, int>();
            _doorAccessPreviousStateList = new Dictionary<string, DoorModeType>();
        }

        #region Members

        private List<AreaInfo> _areaInfoList;
        private List<AccessPointInfo> _accessPointInfoList;
        private List<DoorInfo> _doorInfoList;
        
        private Dictionary<string, DoorCapabilities> _doorCapabilitiesList;
        private Dictionary<string, DoorState> _doorStateList;

        private Dictionary<string, int> _doorAccessList;
        private Dictionary<string, DoorModeType> _doorAccessPreviousStateList;

        #endregion //Members

        #region Properties

        public List<AreaInfo> AreaInfoList
        {
            get
            {
                return _areaInfoList;
            }
            set
            {
                _areaInfoList = value;
            }
        }

        public List<AccessPointInfo> AccessPointInfoList
        {
            get
            {
                return _accessPointInfoList;
            }
            set
            {
                _accessPointInfoList = value;
            }
        }

        public List<DoorInfo> DoorInfoList
        {
            get
            {
                return _doorInfoList;
            }
            set
            {
                _doorInfoList = value;
            }
        }

        public Dictionary<string, DoorCapabilities> DoorCapabilitiesList
        {
            get
            {
                return _doorCapabilitiesList;
            }
        }

        public Dictionary<string, DoorState> DoorStateList
        {
            get
            {
                return _doorStateList;
            }
        }

        public Dictionary<string, int> DoorAccessList
        {
            get
            {
                return _doorAccessList;
            }
        }

        public Dictionary<string, DoorModeType> DoorAccessPreviousStateList
        {
            get
            {
                return _doorAccessPreviousStateList;
            }
        }


        #endregion //Properties

    }
}
