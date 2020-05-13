using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    public class LockPhysicalStatePropertyEventArgs : DoorPropertyEventArgs
    {
        #region members

        private LockPhysicalState _mLockPhysicalState;

        #endregion

        #region properties

        public LockPhysicalState LockPhysicalState
        {
            get { return _mLockPhysicalState; }
        }

        #endregion

        public LockPhysicalStatePropertyEventArgs(DoorControlService doorControlService, DateTime utcTime, string propertyOperation, string doorToken, LockPhysicalState lockPhysicalState)
            : base(doorControlService, utcTime, propertyOperation, doorToken)
        {
            _mLockPhysicalState = lockPhysicalState;
        }


        public override string PropertyName
        {
            get { return "State"; }
        }

        public override string PropertyValue
        {
            get { return LockPhysicalState.ToString(); }
        }
    }
}
