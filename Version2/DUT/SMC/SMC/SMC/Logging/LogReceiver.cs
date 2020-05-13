using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Logging
{
    public delegate void MessageReceivedEvent(string message, MessageType type);

    /// <summary>
    /// Class for receiving logging messages.
    /// </summary>
    [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    class LogReceiver : LogReceiverSoap
    {
        public event MessageReceivedEvent MessageReceived;
        public event Action ConnectionClosed;

        #region LogReceiverSoap Members

        /// <summary>
        /// Logs message.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void LogMessage(LogMessageRequest request)
        {
            if (MessageReceived != null)
            {
                MessageReceived(request.Body.message, request.Body.type);
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
