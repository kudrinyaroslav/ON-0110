///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
namespace TestTool.HttpTransport
{
    using System.ServiceModel.Channels;

    static class CustomHttpConstants
    {
        internal const string Scheme = "http";

        static MessageEncoderFactory _messageEncoderFactory;
        static CustomHttpConstants()
        {
            _messageEncoderFactory = new TextMessageEncodingBindingElement().CreateMessageEncoderFactory();
        }

        // ensure our advertised MessageVersion matches the version we're
        // using to serialize/deserialize data to/from the wire
        internal static MessageVersion MessageVersion
        {
            get
            {
                return _messageEncoderFactory.MessageVersion;
            }
        }

        // we can use the same encoder for all our Udp Channels as it's free-threaded
        internal static MessageEncoderFactory DefaultMessageEncoderFactory
       {
            get
            {
                return _messageEncoderFactory;
            }
        }
    }
}
