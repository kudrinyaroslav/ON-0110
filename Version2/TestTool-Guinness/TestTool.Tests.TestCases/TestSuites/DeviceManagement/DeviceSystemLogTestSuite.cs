///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class DeviceSystemLogTestSuite : Base.DeviceManagementTest
    {
        public DeviceSystemLogTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private const string PATH = "Device Management\\System";

        [Test(Name = "SYSTEM COMMAND GETSYSTEMLOG",
            Order = "03.01.10",
            Id = "3-1-10",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequiredFeatures =  new Feature[]{ Feature.SystemLogging },
            FunctionalityUnderTest = new Functionality[]{ Functionality.GetSystemLog },
            RequirementLevel = RequirementLevel.Must)]
        public void GetSystemLogTest()
        {
            RunTest(
                () =>
                {
                    SystemLog systemLog = GetSystemLog(SystemLogType.System, "Get system log (system)", "Sender/InvalidArgs/SystemlogUnavailable");

                    SystemLog accessLog = GetSystemLog(SystemLogType.Access, "Get system log (access)", "Sender/InvalidArgs/AccesslogUnavailable");

                });
        }

    }
}
