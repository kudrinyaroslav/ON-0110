using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace DUT.PACS.Simulator.ExternalLogging
{
    /// <summary>
    /// Logging service
    /// </summary>
    public class LoggingService : BaseLoggingService<LogSubscriptionHolder, LogMessageRequestBody>
    {
        public LoggingService()
        {

        }

        /// <summary>
        /// Adds log subscription
        /// </summary>
        /// <param name="uri">Receiver address</param>
        /// <param name="messageType">Message types of interest</param>
        /// <returns>Subscription ID for firther operations</returns>
        public Guid Subscribe(string uri, MessageType messageType)
        {
            CustomBinding custombindingSoap12 = new CustomBinding();
            custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            custombindingSoap12.Elements.Add(new HttpTransportBindingElement());
            
            EndpointAddress address = new EndpointAddress(uri);
            
            LogReceiverSoapClient client = new LogReceiverSoapClient(custombindingSoap12, address);

            LogSubscriptionHolder holder = new LogSubscriptionHolder();
            holder.Client = client;
            holder.MessageType = messageType;

            Guid guid = Subscribe(holder);
            
            return guid;
        }
        
        /// <summary>
        /// Sends logs messages.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void LogMessage(string message, MessageType type)
        {
            LogMessageRequestBody request = new LogMessageRequestBody(message, type);

            SendMessage(request);
        }
     
        /// <summary>
        /// Sends welcome message - logging service supports it
        /// </summary>
        /// <param name="logSubscriptionHolder"></param>
        protected override void SendWelcomeMessage(LogSubscriptionHolder logSubscriptionHolder)
        {
            SendWelcomeMessage(logSubscriptionHolder.Client);

        }

        /// <summary>
        /// Notifies client that a connection is to be closed.
        /// </summary>
        /// <param name="holder"></param>
        protected override void CloseConnection(LogSubscriptionHolder holder)
        {
            holder.Client.CloseConnection();
        }

        /// <summary>
        /// Sends notification to all clients
        /// </summary>
        /// <param name="request">Message to send</param>
        protected override void SendNotifications(LogMessageRequestBody request)
        {
            foreach (LogSubscriptionHolder listener in Subscribers.Values)
            {
                if ((int)listener.MessageType >= (int)request.type)
                {
                    try
                    {
                        listener.Client.LogMessage(request.message, request.type);
                    }
                    catch (Exception exc)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception in LoggingService.SendNotification: " +
                                                           exc.Message);
                        // Nothing to do.
                        // Event no possiblity to log...
                    }
                }
            }
        }

        /// <summary>
        /// Sends welcome message to new client
        /// </summary>
        /// <param name="client"></param>
        void SendWelcomeMessage(LogReceiverSoapClient client)
        {
            client.LogMessage("Your subscription (to logging) has been created successfully", MessageType.Message);
        }
 
    }
}
