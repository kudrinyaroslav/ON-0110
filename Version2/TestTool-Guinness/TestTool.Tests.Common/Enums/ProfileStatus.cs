using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Common.Enums
{
    /// <summary>
    /// Profile status
    /// </summary>
    public enum ProfileStatus
    {
        /// <summary>
        /// not claimed by the DUT
        /// </summary>
        NotSupported,
        /// <summary>
        /// Supported 
        /// </summary>
        Supported,
        /// <summary>
        /// Not confirmed (not enough features or some tests failed)
        /// </summary>
        Failed
    }
}
