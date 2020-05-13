///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.Interfaces
{
    /// <summary>
    /// Empty interface (for future use)
    /// </summary>
    public interface IChannelController
    {
    }

    /// <summary>
    /// Transport controller (acting at stream level)
    /// </summary>
    public interface ITransportController : IChannelController
    {
        
    }

    /// <summary>
    /// Encoding controller (acting at message level)
    /// </summary>
    public interface IEncodingController : IChannelController
    {

    }

}
