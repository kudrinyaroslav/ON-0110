using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// Event arguments for Door Mode Property Evens
    /// </summary>
    public class DoorModePropertyEventArgs : DoorPropertyEventArgs
    {
        #region Memebers

        private DoorMode m_currentState;

        #endregion //Memebers

        #region Constructor

        /// <summary>
        /// Constructor for DoorModePropertyEventArgs
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="utcTime">UTC time of event</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Mode state</param>
        public DoorModePropertyEventArgs(DoorControlService doorControlService, DateTime utcTime, string propertyOperation, string doorToken, DoorMode currentState)
            :base(doorControlService, utcTime, propertyOperation, doorToken)
        {
            m_currentState = currentState;
        }

        #endregion //Constructor

        #region Properties
        
        public DoorMode CurrentState
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
