using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// Event arguments for Door Monitor Propoerty Evens
    /// </summary>
    public class DoorPhysicalStatePropertyEventArgs : DoorPropertyEventArgs
    {
        #region Members


        private DoorPhysicalState m_currentState;

        #endregion //Members

        #region Constructor

        /// <summary>
        /// Constructor for DoorPhysicalStatePropertyEventArgs
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="utcTime">UTC time of event</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor state</param>
        public DoorPhysicalStatePropertyEventArgs(DoorControlService doorControlService, DateTime utcTime, string propertyOperation, string doorToken, DoorPhysicalState currentState)
            : base(doorControlService, utcTime, propertyOperation, doorToken)
        {
            m_currentState = currentState;
        }

        #endregion //Constructor

        #region Properties

        public DoorPhysicalState CurrentState
        {
            get { return m_currentState; }
        }

        #endregion //Properties

        public override string PropertyName
        {
            get { return "State"; }
        }

        public override string PropertyValue
        {
            get { return CurrentState.ToString(); }
        }
    }
}
