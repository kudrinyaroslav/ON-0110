using System;

namespace CameraClient.Tests
{
    class DeviceManagementTest : BaseServiceTest<Device, DeviceClient>
    {
        public DeviceManagementTest(string address)
            : base(address)
        {

        }

        #region Synchronous version

        protected DiscoveryMode GetDiscoveryMode()
        {
            BeginStep("GetDiscoveryMode");
            DiscoveryMode mode = Client.GetDiscoveryMode();
            StepPassed();
            return mode;
        }

        protected void SetDiscoveryMode(DiscoveryMode mode)
        {
            BeginStep(string.Format("SetDiscoveryMode({0})", mode));
            Client.SetDiscoveryMode(mode);
            StepPassed();
        }
        
        #endregion

        #region Async version 1

        protected IAsyncResult BeginGetDiscoveryMode()
        {
            BeginStep("GetDiscoveryMode");
            return Client.BeginGetDiscoveryMode(null, null);
        }

        protected DiscoveryMode EndGetDiscoveryMode(IAsyncResult result)
        {
            DiscoveryMode mode = Client.EndGetDiscoveryMode(result);
            StepPassed();
            return mode;
        }
        
        protected IAsyncResult BeginSetDiscoveryMode(DiscoveryMode mode)
        {
            BeginStep("SetDiscoveryMode");
            return Client.BeginSetDiscoveryMode(mode, null, null);
        }

        protected void EndSetDiscoveryMode(IAsyncResult result)
        {
            Client.EndSetDiscoveryMode(result);
            StepPassed();
        }

        #endregion

        #region Async version 2

        protected DiscoveryMode BeginEndGetDiscoveryMode()
        {
            BeginStep("GetDiscoveryMode");
            IAsyncResult result = Client.BeginGetDiscoveryMode(null, null);
            WaitForSomething(result);
            DiscoveryMode mode = Client.EndGetDiscoveryMode(result);
            StepPassed();
            return mode;
        }
        
        protected void BeginEndSetDiscoveryMode(DiscoveryMode mode)
        {
            BeginStep("SetDiscoveryMode");
            IAsyncResult result = Client.BeginSetDiscoveryMode(mode, null, null);
            WaitForSomething(result);
            Client.EndSetDiscoveryMode(result);
            StepPassed();
        }

        #endregion
    }
}
