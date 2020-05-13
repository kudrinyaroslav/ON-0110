///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;

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
            RequirementLevel = RequirementLevel.Should)]
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
