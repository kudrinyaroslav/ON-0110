using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestTool.HttpTransport
{
    public interface IValidatingController : IChannelController
    {
        void Validate(Stream stream);
    }
}
