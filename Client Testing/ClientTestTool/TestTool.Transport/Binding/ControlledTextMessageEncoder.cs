///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.ServiceModel.Channels;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using TestTool.Transport.Interfaces;

namespace TestTool.Transport.MessageEncoding
{
    /// <summary>
    /// Controlled message encoder.
    /// </summary>
    public class CustomTextMessageEncoder : MessageEncoder
    {
        private CustomTextMessageEncoderFactory factory;
        private XmlWriterSettings writerSettings;
        private string contentType;

        public CustomTextMessageEncoder(CustomTextMessageEncoderFactory factory)
        {
            this.factory = factory;

            this.writerSettings = new XmlWriterSettings();
            this.writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);

            this.contentType = string.Format("{0}; charset={1}",
                this.factory.MediaType, this.writerSettings.Encoding.HeaderName);


            _controllers = new List<IChannelController>();
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

            MemoryStream stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }
        
        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            LogRequest(stream);

            XmlReader reader = XmlReader.Create(stream);
            return Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
        }

        void LogRequest(Stream requestStream)
        {
            TextReader rdr = new StreamReader(requestStream);
            string content = rdr.ReadToEnd();
            requestStream.Seek(0, SeekOrigin.Begin);

            content = FormatContent(content);


            foreach (IChannelController controller in _controllers)
            {
                ITrafficListener listener = controller as ITrafficListener;
                if (listener != null)
                {
                    listener.LogRequest(content);
                }
            }

        }

        string FormatContent(string content)
        {
            string formatted = content;

            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlDocument document = new XmlDocument();
                document.LoadXml(content);
                document.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                TextReader xmlReader = new StreamReader(memoryStream);
                formatted = xmlReader.ReadToEnd();
                xmlReader.Close();
            }
            catch (Exception exc)
            {
                // Log content as is
            }
            
            return formatted;
        }


        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            LogResponse(totalBytes, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }

        void LogResponse(byte[] messageBytes, int count)
        {
            int offset = 0;
            while(messageBytes[offset] != '<' && offset < count)
            {
                offset++;
            }
            string content = FormatContent(Encoding.UTF8.GetString(messageBytes, offset, count-offset));
            content = FormatContent(content);

            foreach (IChannelController controller in _controllers)
            {
                ITrafficListener listener = controller as ITrafficListener;
                if (listener != null)
                {
                    listener.LogResponse(content);
                }
            }
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }


        #region Logging

        private List<IChannelController> _controllers;

        /// <summary>
        /// Adds controllers
        /// </summary>
        /// <param name="controllers"></param>
        public void AddControllers(IEnumerable<IChannelController> controllers)
        {
            _controllers.AddRange(controllers);
        }

        #endregion
    }
}
