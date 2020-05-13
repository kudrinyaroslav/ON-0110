using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Engine.Base.Definitions
{
    public class BaseTestParam
    {
        /// <summary>
        /// "Advanced" parameters for tests which have non-empty "ParametersType" list.
        /// Dictionary key is GUID of type.
        /// Dictionary value is an object of this type.
        /// </summary>
        public Dictionary<string, object> AdvancedPrameters { get; set; }

    }
}
