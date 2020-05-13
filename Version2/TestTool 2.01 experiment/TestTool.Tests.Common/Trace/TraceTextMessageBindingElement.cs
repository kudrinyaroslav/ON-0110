using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Serialization.Formatters.Soap;
using System.Collections.Generic;

namespace TestTool.Tests.Common.Trace
{
    public class TraceTextMessageBindingElement: MessageEncodingBindingElement, IWsdlExportExtension
    {
        private MessageVersion msgVersion;
        private string mediaType;
        private string encoding;
        private XmlDictionaryReaderQuotas readerQuotas;

        TraceTextMessageBindingElement(TraceTextMessageBindingElement binding)
            : this(binding.Encoding, binding.MediaType, binding.MessageVersion)
        {
            this.readerQuotas = new XmlDictionaryReaderQuotas();
            this._listeners = binding._listeners;
            this._behaviourContainer = binding._behaviourContainer;
            binding.ReaderQuotas.CopyTo(this.readerQuotas);
        }

        public TraceTextMessageBindingElement(string encoding, string mediaType,
            MessageVersion msgVersion)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (mediaType == null)
                throw new ArgumentNullException("mediaType");

            if (msgVersion == null)
                throw new ArgumentNullException("msgVersion");

            this.msgVersion = msgVersion;
            this.mediaType = mediaType;
            this.encoding = encoding;
            this.readerQuotas = new XmlDictionaryReaderQuotas();

            this._behaviourContainer = new BehaviourContainer();
        }

        public TraceTextMessageBindingElement(string encoding, string mediaType)
            : this(encoding, mediaType, MessageVersion.Soap12)
        {
        }

        public TraceTextMessageBindingElement(string encoding)
            : this(encoding, "application/soap+xml")
            //: this(encoding, "text/xml")
        {

        }

        public TraceTextMessageBindingElement()
            : this("utf-8")
        {
        }

        private List<ITrafficListener> _listeners = new List<ITrafficListener>();
        
        public void AddListener(ITrafficListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(ITrafficListener listener)
        {
            _listeners.Remove(listener);
        }

        private BehaviourContainer _behaviourContainer;
        public void AddBreakingBehaviour(TestEngine.MessageSpoiler spoiler)
        {
            _behaviourContainer.Spoiler = spoiler;
        }

        public TestEngine.MessageSpoiler Spoiler
        {
            get { return _behaviourContainer.Spoiler; }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.msgVersion;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.msgVersion = value;
            }
        }
        
        public string MediaType
        {
            get
            {
                return this.mediaType;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.mediaType = value;
            }
        }

        public string Encoding
        {
            get
            {
                return this.encoding;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.encoding = value;
            }
        }

        // This encoder does not enforces any quotas for the unsecure messages. The 
        // quotas are enforced for the secure portions of messages when this encoder
        // is used in a binding that is configured with security. 
        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get
            {
                return this.readerQuotas;
            }
        }

        #region IMessageEncodingBindingElement Members
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            TraceTextMessageEncoderFactory factory = new TraceTextMessageEncoderFactory(this.MediaType,
                this.Encoding, this.MessageVersion);

            TraceTextMessageEncoder encoder = (TraceTextMessageEncoder)factory.Encoder;
            encoder.AddListeners(_listeners);
            encoder.SetBindingElement(this);
            return factory;

        }

        #endregion


        public override BindingElement Clone()
        {
            TraceTextMessageBindingElement clone = new TraceTextMessageBindingElement(this);
            return clone;
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return (T)(object)this.readerQuotas;
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

        #region IWsdlExportExtension Members

        void IWsdlExportExtension.ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        void IWsdlExportExtension.ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            // The MessageEncodingBindingElement is responsible for ensuring that the WSDL has the correct
            // SOAP version. We can delegate to the WCF implementation of TextMessageEncodingBindingElement for this.
            TextMessageEncodingBindingElement mebe = new TextMessageEncodingBindingElement();
            mebe.MessageVersion = this.msgVersion;
            ((IWsdlExportExtension)mebe).ExportEndpoint(exporter, context);
        }

        #endregion    
    }

    public class BehaviourContainer
    {
        public TestEngine.MessageSpoiler Spoiler { get; set; }
    }


}
