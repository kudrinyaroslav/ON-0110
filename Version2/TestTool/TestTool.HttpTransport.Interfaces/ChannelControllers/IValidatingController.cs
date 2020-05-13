using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestTool.HttpTransport.Interfaces
{
    /// <summary>
    /// Performs validation
    /// </summary>
    public interface IValidatingController : IEncodingController
    {
        /// <summary>
        /// Validates stream
        /// </summary>
        /// <param name="stream"></param>
        void Validate(Stream stream);
    }
}
