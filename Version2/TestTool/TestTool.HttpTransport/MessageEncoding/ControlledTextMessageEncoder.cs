///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Linq;
using System.ServiceModel.Channels;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport.MessageEncoding
{
    /// <summary>
    /// Controlled message encoder.
    /// </summary>
    class ControlledTextMessageEncoder: MessageEncoder
    {
        /// <summary>
        /// Parent factory
        /// </summary>
        private ControlledTextMessageEncoderFactory _factory;
        /// <summary>
        /// Settings
        /// </summary>
        private XmlWriterSettings _writerSettings;
        /// <summary>
        /// Content-type
        /// </summary>
        private string _contentType;


        /// <summary>
        /// Channel Controllers
        /// </summary>
        private List<IChannelController> _controllers = new List<IChannelController>();
        /// <summary>
        /// Parent binding element
        /// </summary>
        private ControlledTextMessageBindingElement _bindingElement;

        private MessageEncoder _mtomEncoder;
        private MessageEncoder _textEncoder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factory"></param>
        public ControlledTextMessageEncoder(ControlledTextMessageEncoderFactory factory)
        {
            Initialize(factory);
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
        /// Saves reference to binding element.
        /// </summary>
        /// <param name="bindingElement"></param>
        public void SetBindingElement(ControlledTextMessageBindingElement bindingElement)
        {
            _bindingElement = bindingElement;
        }

        /// <summary>
        /// Content Type
        /// </summary>
        public override string ContentType
        {
            get
            {
                return this._contentType;
            }
        }

        /// <summary>
        /// Media type
        /// </summary>
        public override string MediaType
        {
            get
            {
                return _factory.MediaType;
            }
        }

        /// <summary>
        /// Message version
        /// </summary>
        public override MessageVersion MessageVersion
        {
            get
            {
                return this._factory.MessageVersion;
            }
        }

        /// <summary>
        /// Reads message from buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferManager"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);


            string content = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlDocument document = new XmlDocument();
                document.LoadXml(content);
                document.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                TextReader rdr = new StreamReader(memoryStream);
                content = rdr.ReadToEnd();
                rdr.Close();
            }
            catch (Exception )
            {
                // Log content as is
            }

            foreach (IChannelController controller in _controllers)
            {
                ITrafficListener listener = controller as ITrafficListener;
                if (listener != null)
                {
                    listener.LogResponse(content);
                }
            }

            if (contentType == Internals.Http.HttpHelper.APPLICATIONSOAPXML)
            {
                MemoryStream validateStream = new MemoryStream(msgContents);
                foreach (IChannelController controller in _controllers)
                {
                    IValidatingController validatingController = controller as IValidatingController;
                    if (validatingController != null)
                    {
                        validatingController.Validate(validateStream);
                    }
                }
                validateStream.Close();
            }

            MemoryStream stream = new MemoryStream(msgContents);
            Message message = ReadMessage(stream, int.MaxValue, contentType);
            
            return message;
        }

        /// <summary>
        /// Reads message from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="maxSizeOfHeaders"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            MessageEncoder encoder = GetEncoder(contentType);
            return encoder.ReadMessage(stream, maxSizeOfHeaders);
        }

        /// <summary>
        /// Writes message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="maxMessageSize"></param>
        /// <param name="bufferManager"></param>
        /// <param name="messageOffset"></param>
        /// <returns></returns>
        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            int idx = -1;
            for (int i = 0; i< message.Headers.Count; i++)
            {
                if (message.Headers[i].Name == "VsDebuggerCausalityData")
                {
                    idx = i;
                    break;
                }
            }
            if (idx >= 0)
            {
                message.Headers.RemoveAt(idx);
            }

            byte[] messageBytes = ProcessMessage(message);
            int messageLength = messageBytes.Length;

            int totalLength = messageLength + messageOffset;
            
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray;
            byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            
            string content;
            content = Encoding.GetEncoding(_factory.CharSet).GetString(messageBytes);
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlDocument document = new XmlDocument();
                document.LoadXml(content);
                document.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                TextReader rdr = new StreamReader(memoryStream);
                content = rdr.ReadToEnd();
                rdr.Close();
            }
            catch (Exception )
            {
                // Log content as is
            }

            foreach (IChannelController controller in _controllers)
            {
                ITrafficListener listener = controller as ITrafficListener;
                if (listener != null)
                {
                    listener.LogRequest(content);
                }
            }
            
            return byteArray;

        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this._writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }

        /// <summary>
        /// Processes message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private byte[] ProcessMessage(Message message)
        {
            MemoryStream originalStream = new MemoryStream();
            // todo: why text encoder?
            _textEncoder.WriteMessage(message, originalStream);

            byte[] messageBytes = new byte[originalStream.Length];
            Array.Copy(originalStream.GetBuffer(), messageBytes, originalStream.Length);

            ISoapMessageMutator messageController = null;
            foreach (IChannelController controller in _bindingElement.Controllers)
            {
                if (controller is ISoapMessageMutator)
                {
                    messageController = (ISoapMessageMutator)controller;
                    break;
                }
            }

            if (messageController != null)
            {
                messageBytes = messageController.ProcessMessage(messageBytes);
            }
            
            return messageBytes;
        }

        private void Initialize(ControlledTextMessageEncoderFactory factory)
        {
            this._factory = factory;
            //this.writerSettings = new XmlWriterSettings();
            //this.writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
            
            //this.contentType = string.Format("{0}; charset={1}", 
            //    this.factory.MediaType, this.writerSettings.Encoding.HeaderName);
            
            InitializeEncoders(factory.MessageVersion, Encoding.GetEncoding(factory.CharSet));
        }

        private void InitializeEncoders(MessageVersion messageVersion, Encoding encoding)
        {
            // MTOM
            var mtomBindingElement = new MtomMessageEncodingBindingElement(messageVersion, encoding);
            var mtomFactory = mtomBindingElement.CreateMessageEncoderFactory();
            _mtomEncoder = mtomFactory.Encoder;

            // Text
            var textBindingElement = new TextMessageEncodingBindingElement(messageVersion, encoding);
            //textBindingElement.ReaderQuotas.MaxStringContentLength = 65535;
            //textBindingElement.ReaderQuotas.MaxStringContentLength = textBindingElement.ReaderQuotas.MaxBytesPerRead = 65535;
            textBindingElement.ReaderQuotas.MaxStringContentLength = textBindingElement.ReaderQuotas.MaxBytesPerRead = 1024*1024*16;

            var textFactory = textBindingElement.CreateMessageEncoderFactory();
            _textEncoder = textFactory.Encoder;

            // Binary
            //var binaryBindingElement = new BinaryMessageEncodingBindingElement(messageVersion, encoding);
            //var binaryFactory = binaryBindingElement.CreateMessageEncoderFactory();
            //_binaryEncoder = binaryFactory.Encoder;
        }

        private MessageEncoder GetEncoder(string contentType)
        {
            //todo: this method is not completely implemented. Look at comments bellow.
            if (Regex.IsMatch(contentType, "^multipart.*"))
            {
                var mtomBindingElement = new MtomMessageEncodingBindingElement(_factory.MessageVersion, Encoding.GetEncoding(_factory.CharSet));
                var mtomFactory = mtomBindingElement.CreateMessageEncoderFactory();
                return mtomFactory.Encoder;
                //return _mtomEncoder;
            }
            else // if (...)
            {
                var textBindingElement = new TextMessageEncodingBindingElement(_factory.MessageVersion, Encoding.GetEncoding(_factory.CharSet));
                //textBindingElement.ReaderQuotas.MaxStringContentLength = 65535;
                //textBindingElement.ReaderQuotas.MaxStringContentLength = textBindingElement.ReaderQuotas.MaxBytesPerRead = 65535;
                textBindingElement.ReaderQuotas.MaxStringContentLength = textBindingElement.ReaderQuotas.MaxBytesPerRead = 1024 * 1024 * 16;

                var textFactory = textBindingElement.CreateMessageEncoderFactory();
                return textFactory.Encoder;
                //return _textEncoder;
            }
            //else if (...)
            //{
            //    return new BinaryMessageEncodingBindingElement();
            //}
            throw new NotSupportedException(string.Format("Unexpected content-type: \"{0}\".", contentType));
        }
    }
}
