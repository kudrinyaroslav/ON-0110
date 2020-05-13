
namespace DUT.PACS.Simulator.ExternalLogging
{
    /// <summary>
    /// Logging subscription holder
    /// </summary>
    public class LogSubscriptionHolder : BaseSubscriptionHolder
    {
        public MessageType MessageType { get; set; }
        public LogReceiverSoapClient Client { get; set; }
        
        public override System.ServiceModel.ICommunicationObject Channel
        {
            get { return Client.InnerChannel; }
        }
    }
}
