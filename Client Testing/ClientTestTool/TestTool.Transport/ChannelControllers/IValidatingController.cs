using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestTool.Transport.Interfaces
{
    /// <summary>
    /// Performs validation
    /// </summary>
    public interface IValidatingController : IChannelController
    {
        /// <summary>
        /// Validates stream
        /// </summary>
        /// <param name="stream"></param>
        void Validate(Stream stream);
    }
}
