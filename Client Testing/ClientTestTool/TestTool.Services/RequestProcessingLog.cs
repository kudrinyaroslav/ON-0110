using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Services
{
    public struct RequestProcessingLog
    {
        public DateTime Time { get; set; }
        public string Service { get; set; }
        public string Method { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
    }
}
