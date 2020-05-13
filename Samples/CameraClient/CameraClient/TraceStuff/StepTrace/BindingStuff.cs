using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Serialization.Formatters.Soap;

namespace CameraClient.TrafficTrace
{
    public class CustomTextMessageEncoder : MessageEncoder
    {

        private CustomTextMessageEncoderFactory factory;
        private XmlWriterSettings writerSettings;
        private string contentType;

        private MessageEncoder baseEncoder;

        private ITrafficListener _listener;
        public void SetListener(ITrafficListener listener)
        {
            _listener = listener;
        }

        public CustomTextMessageEncoder(CustomTextMessageEncoderFactory factory)
        {
            this.factory = factory;

            this.writerSettings = new XmlWriterSettings();
            this.writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
            this.contentType = string.Format("{0}; charset={1}",
                this.factory.MediaType, this.writerSettings.Encoding.HeaderName);

            MessageEncoderFactory baseFactory;
            TextMessageEncodingBindingElement element =
                new TextMessageEncodingBindingElement(factory.MessageVersion, Encoding.GetEncoding(factory.CharSet));
            baseFactory = element.CreateMessageEncoderFactory();

            baseEncoder = baseFactory.Encoder;

        }

        public override string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        public override string MediaType
        {
            get
            {
                return factory.MediaType;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.factory.MessageVersion;
            }
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);
            
            string content = Encoding.UTF8.GetString(buffer.Array);

            if (_listener != null)
            {
                _listener.LogResponse(content);
            }

            MemoryStream stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            return baseEncoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            ArraySegment<byte> byteArray = baseEncoder.WriteMessage(message, maxMessageSize, bufferManager);

            byte[] messageBytes;
            messageBytes = byteArray.Array;
            string content = Encoding.UTF8.GetString(messageBytes);

            if (_behaviour != null)
            {
                XmlDocument xmlDoc= new XmlDocument();
                xmlDoc.LoadXml(content);

                System.Diagnostics.Debug.WriteLine("Content before spoiling: ");
                System.Diagnostics.Debug.WriteLine(content);

                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                foreach (string key in _behaviour.Namespaces.Keys)
                {
                    manager.AddNamespace(key, _behaviour.Namespaces[key]);
                }

                XmlNode node = xmlDoc.SelectSingleNode(_behaviour.NodePath, manager);
                //node = xmlDoc.SelectSingleNode("/s:Envelope", manager);
                //node = xmlDoc.SelectSingleNode("/s:Envelope/s:Body", manager);
                //node = xmlDoc.SelectSingleNode("/s:Envelope/s:Body/onvif:SetDiscoveryMode", manager);
                if (node != null)
                {
                    node.InnerText = _behaviour.NodeValue;
                }

                MemoryStream stream = new MemoryStream();
                XmlWriter writer = new XmlTextWriter(stream, Encoding.GetEncoding(factory.CharSet));

                xmlDoc.WriteTo(writer);
                int messageLength = (int) stream.Length;
                writer.Close();

                messageBytes = stream.GetBuffer();

                content = Encoding.UTF8.GetString(messageBytes);
                System.Diagnostics.Debug.WriteLine("Content after spoiling: ");
                System.Diagnostics.Debug.WriteLine(content);

               
                int totalLength = messageLength + messageOffset;
                byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
                Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            }

            if (_listener != null)
            {
                _listener.LogRequest(content);
            }
            return byteArray;
            /*
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();
            
            string content = Encoding.UTF8.GetString(messageBytes);

            if (_listener != null)
            {
                _listener.LogRequest(content);
            }

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;*/
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }

        private CameraClient.TraceStuff.StepTrace.BreakingBehaviour _behaviour;
        public void AddBreakingBehaviour(CameraClient.TraceStuff.StepTrace.BreakingBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

    }

    public class CustomTextMessageEncoderFactory : MessageEncoderFactory
    {
        private MessageEncoder encoder;
        private MessageVersion version;
        private string mediaType;
        private string charSet;
        
        internal CustomTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.encoder = new CustomTextMessageEncoder(this);
            
        }

        public override MessageEncoder Encoder
        {
            get
            {
                return this.encoder;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.version;
            }
        }

        internal string MediaType
        {
            get
            {
                return this.mediaType;
            }
        }

        internal string CharSet
        {
            get
            {
                return this.charSet;
            }
        }
        
    }

    public class CustomTextMessageBindingElement : MessageEncodingBindingElement, IWsdlExportExtension
    {
        private MessageVersion msgVersion;
        private string mediaType;
        private string encoding;
        private XmlDictionaryReaderQuotas readerQuotas;

        CustomTextMessageBindingElement(CustomTextMessageBindingElement binding)
            : this(binding.Encoding, binding.MediaType, binding.MessageVersion)
        {
            this.readerQuotas = new XmlDictionaryReaderQuotas();
            this._listener = binding._listener;
            this._behaviour = binding._behaviour;
            binding.ReaderQuotas.CopyTo(this.readerQuotas);
        }

        public CustomTextMessageBindingElement(string encoding, string mediaType,
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
        }

        public CustomTextMessageBindingElement(string encoding, string mediaType)
            : this(encoding, mediaType, MessageVersion.Soap12)
        {
        }

        public CustomTextMessageBindingElement(string encoding)
            : this(encoding, "application/soap+xml")
        {

        }

        public CustomTextMessageBindingElement()
            : this("utf-8")
        {
        }

        private ITrafficListener _listener;
        public void SetListener(ITrafficListener listener)
        {
            _listener = listener;
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
            CustomTextMessageEncoderFactory factory = new CustomTextMessageEncoderFactory(this.MediaType,
                this.Encoding, this.MessageVersion);

            CustomTextMessageEncoder encoder = (CustomTextMessageEncoder)factory.Encoder;
            encoder.SetListener(_listener);
            encoder.AddBreakingBehaviour(_behaviour);
            return factory;

        }

        #endregion

        private CameraClient.TraceStuff.StepTrace.BreakingBehaviour _behaviour;
        public void AddBreakingBehaviour(CameraClient.TraceStuff.StepTrace.BreakingBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public override BindingElement Clone()
        {
            CustomTextMessageBindingElement clone = new CustomTextMessageBindingElement(this);
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

    public interface ITrafficListener
    {
        void LogRequest(string request);
        void LogResponse(string response);
    }

}
