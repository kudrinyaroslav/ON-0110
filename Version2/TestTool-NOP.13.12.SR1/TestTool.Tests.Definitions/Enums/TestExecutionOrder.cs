using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Definitions.Enums
{
    /// <summary>
    /// Order of tests in tests sequence
    /// </summary>
    public enum TestExecutionOrder
    {
        FeatureDefinition = 0,
        First = 1,
        Normal = 2,
        Last = 3
    }
}
