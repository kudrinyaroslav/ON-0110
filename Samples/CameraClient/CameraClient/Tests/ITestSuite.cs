using System;
using System.Reflection;

namespace CameraClient.Tests
{
    public interface ITestSuite
    {
        void ReportMethodInvocationException(MethodInfo methodInfo, Exception exception);
        
        void Halt();

        void Pause();

        void Resume();
    }
}
