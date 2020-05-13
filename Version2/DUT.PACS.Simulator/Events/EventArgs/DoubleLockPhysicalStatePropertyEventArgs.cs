using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    public class DoubleLockPhysicalStatePropertyEventArgs : DoorPropertyEventArgs
    {
        #region members

        private LockPhysicalState _mDoubleLockPhysicalState;

        #endregion

        #region Properties

        public LockPhysicalState DoubleLockPhysicalState
        {
            get { return _mDoubleLockPhysicalState; }
        }

        #endregion

        public DoubleLockPhysicalStatePropertyEventArgs(DoorControlService doorControlService, DateTime utcTime, string propertyOperation, string doorToken, LockPhysicalState doubleLockPhysicalStateType)
            : base(doorControlService, utcTime, propertyOperation, doorToken)
        {
            _mDoubleLockPhysicalState = doubleLockPhysicalStateType;
        }

        public override string PropertyName
        {
            get { return "State"; }
        }

        public override string PropertyValue
        {
            get { return DoubleLockPhysicalState.ToString(); }
        }
    }
}
