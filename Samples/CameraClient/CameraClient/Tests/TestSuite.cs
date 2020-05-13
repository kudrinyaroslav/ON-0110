using System;
using System.Threading;

namespace CameraClient.Tests
{
    // possibly marked with special attribute
    class TestSuite : DeviceManagementTest 
    {
        public TestSuite(string address)
            : base(address)
        {

        }

        // Test attributes are added here
        
        [Test(Name="Discovery test", Path= "01\\01")]
        public void RunDiscoveryTest()
        {
            DiscoveryTest();
        }


        [Test(Name="Discovery test 1", Path= "01\\03")]
        public void RunDiscoveryTest2()
        {
            DiscoveryTestAsync2();
        }

        [Test(Name = "Discovery test - stepped version 1", Path = "01\\02")]
        public void RunDiscoveryTest1()
        {
            try
            {
                DiscoveryTestAsync1();
            }
            catch (StopEventException)
            {
            }
        }
        
        // Test attributes are added here
        public void RunDiscoveryTestAsync()
        {
            Thread thread = new Thread(DiscoveryTest);
            thread.Start();
        }

        void DiscoveryTest()
        {
            try
            {
                BeginTest();

                System.Diagnostics.Debug.WriteLine(string.Format("{0}, Before GetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                DiscoveryMode backup = GetDiscoveryMode();
                System.Diagnostics.Debug.WriteLine(string.Format("{0}, After GetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));

                // Set Discoverable

                System.Diagnostics.Debug.WriteLine(string.Format("{0}, Before SetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                SetDiscoveryMode(DiscoveryMode.Discoverable);
                System.Diagnostics.Debug.WriteLine(string.Format("{0}, After SetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));

                System.Diagnostics.Debug.WriteLine(string.Format("{0}, Before GetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                DiscoveryMode afterSet = GetDiscoveryMode();
                System.Diagnostics.Debug.WriteLine(string.Format("{0}, After GetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));

                Assert(afterSet == DiscoveryMode.Discoverable);

                System.Diagnostics.Debug.WriteLine(string.Format("{0}, Before SetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                SetDiscoveryMode(DiscoveryMode.NonDiscoverable);
                System.Diagnostics.Debug.WriteLine(string.Format("{0}, After SetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));

                System.Diagnostics.Debug.WriteLine(string.Format("{0}, Before GetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                afterSet = GetDiscoveryMode();
                System.Diagnostics.Debug.WriteLine(string.Format("{0}, After GetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));

                Assert(afterSet == DiscoveryMode.NonDiscoverable);

                System.Diagnostics.Debug.WriteLine(string.Format("{0}, Before SetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                SetDiscoveryMode(backup);
                System.Diagnostics.Debug.WriteLine(string.Format("{0}, After SetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));

                EndTest();
            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }
        }
        
        void DiscoveryTestAsync1()
        {
            DiscoveryMode backup;

            BeginTest();

            System.Diagnostics.Debug.WriteLine(string.Format("{0}, 1 - BeginGetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
            IAsyncResult result = BeginGetDiscoveryMode();

            WaitForSomething(result);
            backup = EndGetDiscoveryMode(result);
            System.Diagnostics.Debug.WriteLine("Asynchronous method completed");
            
            System.Diagnostics.Debug.WriteLine(string.Format("{0}, 2 - BeginSetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
            result = BeginSetDiscoveryMode(DiscoveryMode.Discoverable);
           
            WaitForSomething(result);
            EndSetDiscoveryMode(result);
            System.Diagnostics.Debug.WriteLine(string.Format("{0}, 3 - BeginGetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));

            result = BeginGetDiscoveryMode();

            DiscoveryMode afterSet;
            WaitForSomething(result);
            afterSet = EndGetDiscoveryMode(result);

            Assert(afterSet == DiscoveryMode.Discoverable);

            System.Diagnostics.Debug.WriteLine(string.Format("{0}, 4 - BeginSetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
            result = BeginSetDiscoveryMode(DiscoveryMode.NonDiscoverable);

            WaitForSomething(result);
            EndSetDiscoveryMode(result);
            System.Diagnostics.Debug.WriteLine(string.Format("{0}, 5 - BeginGetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
            
            result = BeginGetDiscoveryMode();

            WaitForSomething(result);
            afterSet = EndGetDiscoveryMode(result);

            Assert(afterSet == DiscoveryMode.NonDiscoverable);

            System.Diagnostics.Debug.WriteLine(string.Format("{0}, 6 - BeginSetDiscoveryMode", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
            result = BeginSetDiscoveryMode(backup);

            WaitForSomething(result);
            EndSetDiscoveryMode(result);
            System.Diagnostics.Debug.WriteLine("Asynchronous method completed");

            EndTest();
            System.Diagnostics.Debug.WriteLine("Exit from DiscoveryTestStepped");
        }

        void DiscoveryTestAsync2()
        {
            try
            {
                BeginTest();

                DiscoveryMode backup = BeginEndGetDiscoveryMode();

                // Set Discoverable

                BeginEndSetDiscoveryMode(DiscoveryMode.Discoverable);

                DiscoveryMode afterSet = BeginEndGetDiscoveryMode();

                Assert(afterSet == DiscoveryMode.Discoverable);

                BeginEndSetDiscoveryMode(DiscoveryMode.NonDiscoverable);

                afterSet = BeginEndGetDiscoveryMode();

                Assert(afterSet == DiscoveryMode.NonDiscoverable);

                BeginEndSetDiscoveryMode(backup);

                EndTest();
            }
            catch (StopEventException)
            {

            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }
        }
    }
}
