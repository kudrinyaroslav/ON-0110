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
    class TraceTextMessageEncoder: MessageEncoder
    {
        private TraceTextMessageEncoderFactory factory;
        private XmlWriterSettings writerSettings;
        private string contentType;

        private MessageEncoder baseEncoder;

        private List<ITrafficListener> _listeners = new List<ITrafficListener>();
        public void AddListener(ITrafficListener listener)
        {
            _listeners.Add(listener);
        }

        public void AddListeners(IEnumerable<ITrafficListener> listeners)
        {
            _listeners.AddRange(listeners);
        }

        public void RemoveListener(ITrafficListener listener)
        {
            _listeners.Remove(listener);
        }

        private TraceTextMessageBindingElement _bindingElement;
        public void SetBindingElement(TraceTextMessageBindingElement bindingElement)
        {
            _bindingElement = bindingElement;
        }

        public TraceTextMessageEncoder(TraceTextMessageEncoderFactory factory)
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

            string content = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);

            foreach (ITrafficListener listener in _listeners)
            {
                listener.LogResponse(content);
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
            ArraySegment<byte> byteArray;
            string content;

            TestEngine.MessageSpoiler spoiler = null;
            if (_bindingElement != null)
            {
                spoiler = _bindingElement.Spoiler; 
            }

            if (spoiler == null)
            {
                byteArray = baseEncoder.WriteMessage(message, maxMessageSize, bufferManager);

                byte[] messageBytes = byteArray.Array;
                content = Encoding.GetEncoding(factory.CharSet).GetString(messageBytes, byteArray.Offset, byteArray.Count);
            }
            else
            {
                byte[] messageBytes = ProcessMessage(message, spoiler );
                int messageLength = messageBytes.Length;

                int totalLength = messageLength + messageOffset;
                
                byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
                Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);

                content = Encoding.GetEncoding(factory.CharSet).GetString(messageBytes);
                System.Diagnostics.Debug.WriteLine("Content after spoiling: ");
                System.Diagnostics.Debug.WriteLine(content);
            }

            foreach (ITrafficListener listener in _listeners)
            {
                listener.LogRequest(content);
            }

            return byteArray;

        }

        byte[] ProcessMessage(Message message, TestEngine.MessageSpoiler spoiler)
        {
            MemoryStream originalStream = new MemoryStream();
            baseEncoder.WriteMessage(message, originalStream);

            byte[] originalMessage = originalStream.GetBuffer();
            string content = Encoding.GetEncoding(factory.CharSet).GetString(originalMessage);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            System.Diagnostics.Debug.WriteLine("Content before spoiling: ");
            System.Diagnostics.Debug.WriteLine(content);

            if (spoiler.NodesToReplace != null)
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                foreach (string key in spoiler.Namespaces.Keys)
                {
                    manager.AddNamespace(key, spoiler.Namespaces[key]);
                }

                foreach (string nodePath in spoiler.NodesToReplace.Keys)
                {
                    XmlNode node = xmlDoc.SelectSingleNode(nodePath, manager);
                    if (node != null)
                    {
                        node.InnerText = spoiler.NodesToReplace[nodePath];
                    }
                }
            }

            MemoryStream stream = new MemoryStream();

            Encoding encoding;
            if (factory.CharSet.ToUpper() == "UTF-8")
            {
                encoding = new UTF8Encoding(false);
            }
            else
            {
                encoding = Encoding.GetEncoding(factory.CharSet);
            }
            XmlWriter writer = new XmlTextWriter(stream, encoding);
            xmlDoc.WriteTo(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            return messageBytes;
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }
    }
}
