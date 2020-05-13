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
        private MessageEncoder _encoder;
        private MessageVersion _version;
        private string _mediaType;
        private string _charSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="version"></param>
        internal ControlledTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version)
        {
            this._version = version;
            this._mediaType = mediaType;
            this._charSet = charSet;
            this._encoder = new ControlledTextMessageEncoder(this);
            
        }

        /// <summary>
        /// Message encoder
        /// </summary>
        public override MessageEncoder Encoder
        {
            get
            {
                return this._encoder;
            }
        }

        /// <summary>
        /// Message version
        /// </summary>
        public override MessageVersion MessageVersion
        {
            get
            {
                return this._version;
            }
        }

        /// <summary>
        /// Media type
        /// </summary>
        internal string MediaType
        {
            get
            {
                return this._mediaType;
            }
        }
        
        /// <summary>
        /// Cherset
        /// </summary>
        internal string CharSet
        {
            get
            {
                return this._charSet;
            }
        }
    }
}
