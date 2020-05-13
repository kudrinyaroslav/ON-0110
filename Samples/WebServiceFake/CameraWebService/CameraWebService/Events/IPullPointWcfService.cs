using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CameraWebService
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    public partial class PullMessagesFaultResponseType
    {

        private string maxTimeoutField;

        private int maxMessageLimitField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration", Order = 0)]
        public string MaxTimeout
        {
            get
            {
                return this.maxTimeoutField;
            }
            set
            {
                this.maxTimeoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int MaxMessageLimit
        {
            get
            {
                return this.maxMessageLimitField;
            }
            set
            {
                this.maxMessageLimitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
    }


    // NOTE: If you change the interface name "IPullPointWcfService" here, you must also update the reference to "IPullPointWcfService" in Web.config.
    [ServiceContract]
    public interface IPullPointWcfService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        System.DateTime PullMessages(
            string Timeout,
            int MessageLimit,
            System.Xml.XmlElement[] Any,
            out System.DateTime TerminationTime,
            out NotificationMessageHolderType[]
                NotificationMessage);
    }
}
