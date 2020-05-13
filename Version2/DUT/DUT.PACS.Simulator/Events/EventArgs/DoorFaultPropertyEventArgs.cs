using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    public class DoorFaultPropertyEventArgs : DoorPropertyEventArgs
    {
        #region members

        private DoorFaultState m_doorFaultState;

        #endregion

        #region properties

        public DoorFaultState DoorFaultState
        {
            get { return m_doorFaultState; }
        }

        #endregion

        public DoorFaultPropertyEventArgs(DoorControlService doorControlService, DateTime utcTime, string propertyOperation, string doorToken, DoorFaultState doorFaultState)
            : base(doorControlService, utcTime, propertyOperation, doorToken)
        {
            m_doorFaultState = doorFaultState;
        }

        public override string PropertyName
        {
            get { return "State"; }
        }

        public override string PropertyValue
        {
            get { return DoorFaultState.ToString(); }
        }
    }
}
