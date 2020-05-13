
using System;
using SMC.Proxies;

namespace SMC.StateMonitoring
{
    public delegate void UpdateReceivedEvent(string token, DoorState state);

    /// <summary>
    /// Class for receiving logging messages.
    /// </summary>
    [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    class StateMonitor : StateReportReceiverSoap
    {
        public event UpdateReceivedEvent StateUpdated;
        public event Action ConnectionClosed;

        
        #region StateReportReceiverSoap Members

        public void UpdateState(UpdateStateRequest request)
        {
            if (StateUpdated != null)
            {
                StateUpdated(request.Body.token, request.Body.state);
            }
        }

        /// <summary>
        /// Closes connection
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CloseConnectionResponse CloseConnection(CloseConnectionRequest request)
        {
            if (ConnectionClosed != null)
            {
                ConnectionClosed();
            }
            return new CloseConnectionResponse();
        }

        #endregion
    }

}
