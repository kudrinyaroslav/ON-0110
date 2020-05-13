using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace TestTool.HttpTransport.Interfaces
{
    /// <summary>
    /// Performs WSA control
    /// </summary>
    public interface IWsaController : ITransportController
    {
        /// <summary>
        /// Processes request before sending
        /// </summary>
        /// <param name="message"></param>
        void ProcessRequest(Message message);
        /// <summary>
        /// Validates message after receiving
        /// </summary>
        /// <param name="message"></param>
        void Validate(Message message);
    }
}
