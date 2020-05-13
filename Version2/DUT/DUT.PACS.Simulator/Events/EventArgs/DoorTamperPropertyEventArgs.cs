using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    public class DoorTamperPropertyEventArgs : DoorPropertyEventArgs
    {
        #region members

        private DoorTamperState m_doorTamperState;
        #endregion

        #region properties

        public DoorTamperState DoorTamperState
        {
            get { return m_doorTamperState; }
        }
        
        #endregion

        public DoorTamperPropertyEventArgs(DoorControlService doorControlService, DateTime utcTime, string propertyOperation, string doorToken, DoorTamperState doorTamperState)
            : base(doorControlService, utcTime, propertyOperation, doorToken)
        {
            m_doorTamperState = doorTamperState;
        }


        public override string PropertyName
        {
            get { return "State"; }
        }

        public override string PropertyValue
        {
            get { return DoorTamperState.ToString(); }
        }
    }
}
