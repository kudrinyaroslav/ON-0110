///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using System.Collections.Generic;
using TestTool.HttpTransport.Interfaces;
using TestTool.HttpTransport.MessageEncoding;

namespace TestTool.HttpTransport
{
    /// <summary>
    /// Binding element (to inject custom encoder in WCF stack)
    /// </summary>
    public class ControlledTextMessageBindingElement: MessageEncodingBindingElement, IWsdlExportExtension
    {
        private MessageVersion msgVersion;
        private string mediaType;
        private string encoding;
        private XmlDictionaryReaderQuotas readerQuotas;

        private List<IChannelController> _controllers;

        /// <summary>
        /// Constructor (for cloning)
        /// </summary>
        /// <param name="binding"></param>
        ControlledTextMessageBindingElement(ControlledTextMessageBindingElement binding)
            : this(binding.Encoding, binding.MediaType, binding.MessageVersion)
        {
            this.readerQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.CopyTo(this.readerQuotas);

            _controllers = binding._controllers;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        /// <param name="msgVersion"></param>
        public ControlledTextMessageBindingElement(string encoding, string mediaType,
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
            
            _controllers = new List<IChannelController>();

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        public ControlledTextMessageBindingElement(string encoding, string mediaType)
            : this(encoding, mediaType, MessageVersion.Soap12WSAddressing10)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encoding"></param>
        public ControlledTextMessageBindingElement(string encoding)
            : this(encoding, "application/soap+xml")
            //: this(encoding, "text/xml")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ControlledTextMessageBindingElement()
            : this("utf-8")
        {
        }

        /// <summary>
        /// Adds controller
        /// </summary>
        /// <param name="controller"></param>
        public void AddController(IChannelController controller)
        {
            _controllers.Add(controller);
        }

        /// <summary>
        /// Adds controllers
        /// </summary>
        /// <param name="controllers"></param>
        public void AddControllers(IEnumerable<IChannelController> controllers)
        {
            _controllers.AddRange(controllers);
        }
        
        /// <summary>
        /// Message version
        /// </summary>
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

        /// <summary>
        /// Media type
        /// </summary>
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

        /// <summary>
        /// Encoding
        /// </summary>
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
        
        /// <summary>
        /// Creates encoder factory
        /// </summary>
        /// <returns></returns>
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            ControlledTextMessageEncoderFactory factory = new ControlledTextMessageEncoderFactory(this.MediaType,
                this.Encoding, this.MessageVersion);

            ControlledTextMessageEncoder encoder = (ControlledTextMessageEncoder)factory.Encoder;
            encoder.AddControllers(_controllers);
            encoder.SetBindingElement(this);
            return factory;
        }

        #endregion

        /// <summary>
        /// Clones binding element
        /// </summary>
        /// <returns></returns>
        public override BindingElement Clone()
        {
            ControlledTextMessageBindingElement clone = new ControlledTextMessageBindingElement(this);
            return clone;
        }

        /// <summary>
        /// Builds inner channel factory
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        /// <summary>
        /// Checks if binding element can build channel factory
        /// </summary>
        /// <typeparam name="TChannel">Type of channel</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        /// <summary>
        /// Builds channel listener
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        /// <summary>
        /// Checks if binding element can build channel factory
        /// </summary>
        /// <typeparam name="TChannel"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        /// <summary>
        /// Gets property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Controllers
        /// </summary>
        public List<IChannelController> Controllers
        {
            get { return _controllers; }
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
    

}
