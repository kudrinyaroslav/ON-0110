using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    public class DoorAlarmPropertyEventArgs : DoorPropertyEventArgs
    {
        #region members

        private DoorAlarmState m_doorAlarmState;

        #endregion

        #region properties

        public DoorAlarmState DoorAlarmState
        {
            get { return m_doorAlarmState; }
        }

        #endregion

        public DoorAlarmPropertyEventArgs(DoorControlService doorControlService, DateTime utcTime, string propertyOperation, string doorToken, DoorAlarmState doorAlarmState)
            : base(doorControlService, utcTime, propertyOperation, doorToken)
        {
            m_doorAlarmState = doorAlarmState;
        }

        public override string PropertyName
        {
            get { return "State"; }
        }

        public override string PropertyValue
        {
            get { return DoorAlarmState.ToString(); }
        }
    }
}
