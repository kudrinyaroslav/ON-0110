using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.Engine.Base.BaseOnvifService
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class OnvifServiceAttribute: System.Attribute
    {
        public OnvifServiceInitializationPriority InitializationPriority { get; set; }
    }
}
