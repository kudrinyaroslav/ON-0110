///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Device;
using TestTool.Tests.Common.Discovery;
using IPAddress=System.Net.IPAddress;
using System.Threading;
using DateTime=System.DateTime;
using WSD = TestTool.Proxies.WSDiscovery;

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
            Order = "06.03.06",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void FactoryDefaultHardTest()
        {
            RunTest( () =>
             {
                SetSystemFactoryDefault(FactoryDefaultType.Hard);
            
                //BeginStep("Receive HELLO message");
                ReceiveHelloMessage(false, true, null);
                //StepPassed();
             },
            () =>
            {

                Assert(_operator.GetOkCancelAnswer("Please setup camera after hard reset and press OK"),
                    "Operator non managed to setup camera",
                    "Setting up camera after hard reset");
            }
            );

        }

        [Test(Name = "SYSTEM COMMAND FACTORY DEFAULT SOFT",
            Path = PATH,
            Order = "06.03.07",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
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

                         }, 
                         () =>
                             {
                                 Assert(_operator.GetOkCancelAnswer("Please setup camera after soft reset and press OK"),
                                     "Operator non managed to setup camera",
                                     "Setting up camera after soft reset");
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
            
            IAsyncResult result = del.BeginInvoke(ip, 50, 5000, null, null);
            
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
            Order = "06.03.08",
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
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
            
            public delegate String FindDeviceDelegate(System.Net.IPAddress address, int attempts, int interval);
            
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

            public string ProbeDevice(IPAddress address, int attempts, int interval)
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
                    discovery.Probe(true, address, null);
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}  Probe {1}",
                                                                     DateTime.Now.ToString("HH:mm:ss ffffff"), i));

                    DateTime startTime = DateTime.Now;
                    int handle = WaitHandle.WaitAny(new WaitHandle[] {_discoveredEvent, _timeoutEvent, _stopEvent}, interval);
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

                    int wait = interval - (int) (endTime - startTime).TotalMilliseconds;
                    if (wait > 0)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Sleep {0}", wait));
                        Thread.Sleep(wait);
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