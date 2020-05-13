using System;
using System.Collections.Generic;

namespace HttpTransport
{
    /// <summary>
    /// HTTP packet description
    /// </summary>
    class HttpPacket
    {
        public string HttpVersion { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string Encoding { get; set; }
        public int BodyOffset { get; set; }
        public Dictionary<string, string> Headers { get; private set; }

        public HttpPacket()
        {
            Headers = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }


    }
}
