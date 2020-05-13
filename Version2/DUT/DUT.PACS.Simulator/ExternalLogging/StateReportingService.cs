using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.ExternalLogging
{
    /// <summary>
    /// State monitor subscription holder.
    /// </summary>
    public class StateSubscriptionHolder : BaseSubscriptionHolder
    {
        /// <summary>
        /// Event receiver.
        /// </summary>
        public StateReportReceiverSoapClient Client { get; set; }

        public override System.ServiceModel.ICommunicationObject Channel
        {
            get { return Client.InnerChannel; }
        }
    }

    /// <summary>
    /// Notifies subscribers about door states changes.
    /// </summary>
    public class StateReportingService  : BaseLoggingService<StateSubscriptionHolder, UpdateStateRequestBody>
    {
        public StateReportingService()
        {

        }

        /// <summary>
        /// Adds log subscription
        /// </summary>
        /// <param name="uri">Receiver address</param>
        /// <returns>Subscription ID for further operations</returns>
        public Guid Subscribe(string uri)
        {
            CustomBinding custombindingSoap12 = new CustomBinding();
            custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            custombindingSoap12.Elements.Add(new HttpTransportBindingElement());

            EndpointAddress address = new EndpointAddress(uri);

            StateReportReceiverSoapClient client = new StateReportReceiverSoapClient(custombindingSoap12, address);

            StateSubscriptionHolder holder = new StateSubscriptionHolder();
            holder.Client = client;

            Guid guid = Subscribe(holder);

            return guid;
        }

        /// <summary>
        /// Notifies client that a connection is to be closed.
        /// </summary>
        /// <param name="holder"></param>
        protected override void CloseConnection(StateSubscriptionHolder holder)
        {
            holder.Client.CloseConnection();
        }

        /// <summary>
        /// Notifies client that door state has been changed.
        /// </summary>
        /// <param name="doorToken">Door token</param>
        /// <param name="state">New door state</param>
        public void ReportStateUpdate(string doorToken, DoorState state)
        {
            UpdateStateRequestBody request = new UpdateStateRequestBody(doorToken, state);
            SendMessage(request);
        }

        /// <summary>
        /// Sends notification to all clients
        /// </summary>
        /// <param name="request">Message to send</param>
        protected override void SendNotifications(UpdateStateRequestBody request)
        {
            foreach (StateSubscriptionHolder listener in Subscribers.Values)
            {
                try
                {
                    listener.Client.UpdateState(request.token, request.state);
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine("Exception in StateReportingService.SendNotification: " +
                                                       exc.Message);
                }
            }
        }


    }
}
