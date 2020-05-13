using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Serialization.Formatters.Soap;

namespace TestTool.Tests.Common.Trace
{
    class TraceTextMessageEncoderFactory: MessageEncoderFactory
    {
        private MessageEncoder encoder;
        private MessageVersion version;
        private string mediaType;
        private string charSet;

        internal TraceTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.encoder = new TraceTextMessageEncoder(this);
            
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
}
