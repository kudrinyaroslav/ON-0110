using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CameraWebService.Events.NotificationConsumerProxy
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://docs.oasis-open.org/wsn/bw-2", ConfigurationName = "NotificationConsumer")]
    public interface NotificationConsumer
    {

        // CODEGEN: Generating message contract since the operation Notify is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "urn:#Notify")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void Notify(Notify1 request);
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
    public partial class Notify
    {

        private NotificationMessageHolderType[] notificationMessageField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NotificationMessage", Order = 0)]
        public NotificationMessageHolderType[] NotificationMessage
        {
            get
            {
                return this.notificationMessageField;
            }
            set
            {
                this.notificationMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
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

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class Notify1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2", Order = 0)]
        public Notify Notify;

        public Notify1()
        {
        }

        public Notify1(Notify Notify)
        {
            this.Notify = Notify;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface NotificationConsumerChannel : NotificationConsumer, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class NotificationConsumerClient : System.ServiceModel.ClientBase<NotificationConsumer>, NotificationConsumer
    {

        public NotificationConsumerClient()
        {
        }

        public NotificationConsumerClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public NotificationConsumerClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public NotificationConsumerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public NotificationConsumerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void NotificationConsumer.Notify(Notify1 request)
        {
            base.Channel.Notify(request);
        }

        public void Notify(Notify Notify1)
        {
            Notify1 inValue = new Notify1();
            inValue.Notify = Notify1;
            ((NotificationConsumer)(this)).Notify(inValue);
        }
    }


}
