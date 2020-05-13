///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TestTool.HttpTransport.Interfaces
{
    /// <summary>
    /// Provides address information
    /// </summary>
    public interface IEndpointController : ITransportController
    {
        EndpointAddress Address { get; }
    }
}
