///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Discovery;
using IPAddress=System.Net.IPAddress;
using System.Threading;
using DateTime=System.DateTime;
using WSD = TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class DeviceManagementSystemTestSuiteEx : Base.DeviceDiscoveryTest
    {
        public DeviceManagementSystemTestSuiteEx(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Device Management\\System";


        [Test(Name = "SYSTEM COMMAND FACTORY DEFAULT HARD",
            Path = PATH,
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.DEVICE,
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemFactoryDefault })]
        public void FactoryDefaultHardTest()
        {
            RunTest( () =>
             {
                SetSystemFactoryDefault(FactoryDefaultType.Hard);
                ReceiveHelloMessage(false, true, null);
             },
            () =>
            {

                Assert(Operator.GetOkCancelAnswer("Please setup device after hard reset and press OK"),
                    "Operator dit not manage to setup camera",
                    "Setting up device after hard reset");
            }
            );

        }

        [Test(Name = "SYSTEM COMMAND FACTORY DEFAULT SOFT",
            Path = PATH,
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.DEVICE,
            Version = 1.02,
            ExecutionOrder = TestExecutionOrder.Last,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemFactoryDefault })]
        public void FactoryDefaultSoftTest()
        {
            RunTest( () =>
                         {
                             SetSystemFactoryDefault(FactoryDefaultType.Soft);

                             double timeout = ((double)_rebootTimeout)/1000;

                             BeginStep(string.Format("Wait until Reboot Timeout expires ({0} sec)", timeout.ToString("0.000")));
                             Sleep(_rebootTimeout);
                             StepPassed();

                             FindDevice();

                         });
        }


        string FindDevice()
        {
            BeginStep("Transmit multicast PROBE message");
            LogStepEvent("Retransmit once per 5 seconds until a response is received or timeout (no more than 50 times)");

            DeviceFinder finder = new DeviceFinder(_nic.IP, _semaphore.StopEvent);
            finder.OnProgress += finder_OnProgress;

            Uri uri = new Uri(_cameraAddress);
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(uri.Host);
            
            DeviceFinder.FindDeviceDelegate del = new DeviceFinder.FindDeviceDelegate(finder.ProbeDevice);
            
            IAsyncResult result = del.BeginInvoke(ip, 50, null, null);
            
            WaitHandle[] handles = new WaitHandle[]{_semaphore.StopEvent, result.AsyncWaitHandle};

            int handle = WaitHandle.WaitAny(handles);

            if (handle == 0)
            {
                finder.Stop();
                throw new StopEventException();
            }

            string data = del.EndInvoke(result);
            
            if (!string.IsNullOrEmpty(data))
            {
                LogStepEvent(string.Format("PROBE match message: {0}", data));
            }

            StepPassed();

            Assert(data != null, "Device not found", "Check that answer has been received");
            return data;
        }

        void finder_OnProgress(string entry)
        {
            LogStepEvent(entry);   
        }

        [Test(Name = "SYSTEM COMMAND REBOOT",
            Path = PATH,
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.DEVICE,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Reboot})]
        public void SystemCommandRebootTest()
        {
            RunTest( () =>
                         {
                            string message = SystemReboot();

                            ReceiveHelloMessage(true, true, null);

                            SoapMessage<WSD.ProbeMatchesType> probeMatch = ProbeDeviceStep(false, null, null);
                            
                            string reason = null;
                            Assert(ValidateProbeMatchMessage(probeMatch, out reason), reason, "Validate probe match");
                             
                         });

        }
        
        class DeviceFinder
        {
            private WaitHandle _stopEvent;

            public DeviceFinder(IPAddress address, WaitHandle stopEvent)
            {
                _address = address;
                _discoveredEvent = new ManualResetEvent(false);
                _timeoutEvent = new AutoResetEvent(false);
                _stopEvent = stopEvent;
            }       
            
            public delegate String FindDeviceDelegate(System.Net.IPAddress address, int attempts);
            
            public event Action<string> OnProgress;

            private AutoResetEvent _timeoutEvent;
            
            private ManualResetEvent _discoveredEvent;

            private IPAddress _address;

            private string _probeMessage = null;

            private bool _stop;
            public void Stop()
            {
                lock (this)
                {
                    _stop = true;
                }
            }

            public string ProbeDevice(IPAddress address, int attempts)
            {
                Discovery discovery = new Discovery(_address);

                discovery.Discovered += discovery_Discovered;
                discovery.DiscoveryFinished += discovery_DiscoveryFinished;

                for (int i = 1; i <= attempts; i++ )
                {
                    bool stop;
                    lock (this)
                    {
                        stop = _stop;
                    }

                    if (stop)
                    {
                        return string.Empty;
                    }

                    NotifyProgress(string.Format("Sending Probe Request {0}", i));
                   
                    discovery.Probe(true, address, null, null);

                    System.Diagnostics.Debug.WriteLine(string.Format("{0}  Probe {1}",
                                                                     DateTime.Now.ToString("HH:mm:ss ffffff"), i));

                    DateTime startTime = DateTime.Now;
                    int handle = WaitHandle.WaitAny(new WaitHandle[] {_discoveredEvent, _timeoutEvent, _stopEvent}, Discovery.WS_DISCOVER_TIMEOUT + 3000);
                    DateTime endTime = DateTime.Now;
                    discovery.Close();
                    
                    if (handle >= 0)
                    {
                        if (handle == 0)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("{0} Discovered",
                                                                             DateTime.Now.ToString("HH:mm:ss ffffff")));
                            break;
                        }
                        else if (handle == 1)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("{0} Timeout",
                                                                             DateTime.Now.ToString("HH:mm:ss ffffff")));
                            NotifyProgress("No response");
                        }
                        else if (handle == 2)
                        {
                            System.Diagnostics.Debug.WriteLine("Device finder - stop");
                            throw new StopEventException();
                        }
                    }

                    System.Diagnostics.Debug.WriteLine(string.Format("{0}  continue...", DateTime.Now.ToString("HH:mm:ss ffffff")));
                }

                return _probeMessage;
            }

            void discovery_DiscoveryFinished(object sender, EventArgs e)
            {
                _timeoutEvent.Set();
            }

            void discovery_Discovered(object sender, DiscoveryMessageEventArgs e)
            {
                lock (this)
                {
                    if (_probeMessage != null)
                    {
                        return;
                    }
                    System.Diagnostics.Debug.WriteLine("Discovered");
                    _probeMessage = DiscoveryUtils.XmlToString(e.Message.Raw);
                    _discoveredEvent.Set();
                }
            }

            void NotifyProgress(string logEntry)
            {
                if (OnProgress != null)
                {
                    OnProgress(logEntry);
                }
            }
        }

    }



}
