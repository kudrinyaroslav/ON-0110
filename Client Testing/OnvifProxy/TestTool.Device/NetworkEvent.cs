using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Device
{
    public enum NetworkEventType
    {
        ClientRequest,
        DutRequest,
        DutResponse,
        ClientResponse
    }

    public class NetworkEventData
    {
        public NetworkEventType Type { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        
        public NetworkEventData()
        {
            Timestamp = DateTime.Now;
        }

        public NetworkEventData(NetworkEventType type, byte[] message)
            : this()
        {
            Type = type;
            Message = Encoding.UTF8.GetString(message);
        }
    }
}
