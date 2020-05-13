///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TestTool.HttpTransport
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEndpointController : ITransportController
    {
        EndpointAddress Address { get; }
        bool WsaEnabled { get; }
    }
}
