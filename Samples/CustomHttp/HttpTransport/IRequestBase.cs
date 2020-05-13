using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

namespace CustomHttpTransport
{
    internal interface IRequestBase
    {
        // Methods
        void Abort(IRequestChannel requestChannel);
        void Fault(IRequestChannel requestChannel);
    }

 

}
