///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.ServiceModel.Channels;

namespace TestTool.HttpTransport.MessageEncoding
{
    /// <summary>
    /// Encoder factory
    /// </summary>
    class ControlledTextMessageEncoderFactory: MessageEncoderFactory
    {
        private MessageEncoder encoder;
        private MessageVersion version;
        private string mediaType;
        private string charSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="version"></param>
        internal ControlledTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.encoder = new ControlledTextMessageEncoder(this);
            
        }

        /// <summary>
        /// Message encoder
        /// </summary>
        public override MessageEncoder Encoder
        {
            get
            {
                return this.encoder;
            }
        }

        /// <summary>
        /// Message version
        /// </summary>
        public override MessageVersion MessageVersion
        {
            get
            {
                return this.version;
            }
        }

        /// <summary>
        /// Media type
        /// </summary>
        internal string MediaType
        {
            get
            {
                return this.mediaType;
            }
        }
        
        /// <summary>
        /// Cherset
        /// </summary>
        internal string CharSet
        {
            get
            {
                return this.charSet;
            }
        }
    }
}
