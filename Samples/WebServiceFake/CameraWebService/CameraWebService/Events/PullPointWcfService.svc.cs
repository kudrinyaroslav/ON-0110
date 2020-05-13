using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CameraWebService
{
    // NOTE: If you change the class name "PullPointWcfService" here, you must also update the reference to "PullPointWcfService" in Web.config.
    public class PullPointWcfService : IPullPointWcfService
    {
        public void DoWork()
        {
        }

        [return: System.Xml.Serialization.XmlElementAttribute("CurrentTime")]
        public System.DateTime PullMessages([System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout, int MessageLimit, [System.Xml.Serialization.XmlAnyElementAttribute()] System.Xml.XmlElement[] Any, out System.DateTime TerminationTime, [System.Xml.Serialization.XmlElementAttribute("NotificationMessage", Namespace = "http://docs.oasis-open.org/wsn/b-2")] out NotificationMessageHolderType[] NotificationMessage)
        {
            throw new System.ServiceModel.FaultException<PullMessagesFaultResponseType>(new PullMessagesFaultResponseType());
        }

    }
}
