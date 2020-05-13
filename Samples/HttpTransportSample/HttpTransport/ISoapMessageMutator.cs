using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpTransport
{
    public interface ISoapMessageMutator : IChannelController
    {
        byte[] ProcessMessage(byte[] original);
    }
}
