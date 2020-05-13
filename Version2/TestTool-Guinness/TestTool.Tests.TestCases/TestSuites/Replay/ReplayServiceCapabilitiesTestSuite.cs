using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class ReplayServiceCapabilitiesTestSuite : DeviceManagementTest
    {

        public ReplayServiceCapabilitiesTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Replay\\Capabilities";

        private string _replayServiceAddress;
        private ReplayPortClient _replayClient;

        protected ReplayPortClient ReplayClient
        {
            get
            {
                if (_replayClient == null)
                {
                    bool found = false;
                    if (string.IsNullOrEmpty(_replayServiceAddress))
                    {
                        RunStep(() =>
                        {
                            string address = Client.GetReplayServiceAddress(Features);
                            if (string.IsNullOrEmpty(address))
                            {
                                throw new AssertException("Replay service not found");
                            }
                            else
                            {
                                _replayServiceAddress = address;
                                found = true;
                                LogStepEvent(_replayServiceAddress);
                            }
                        }, "Get Replay service address", OnvifFaults.NoSuchService , true, true);
                        DoRequestDelay();
                    }

                    Assert(found, "Replay service address not found", "Check that the DUT returned Replay service address");

                    if (found)
                    {
                        EndpointController controller = new EndpointController(new EndpointAddress(_replayServiceAddress));

                        Binding binding = CreateBinding(
                            false,
                            new IChannelController[] { new SoapValidator(ReplaySchemasSet.GetInstance()), controller });

                        _replayClient = new ReplayPortClient(binding, new EndpointAddress(_replayServiceAddress));

                        AttachSecurity(_replayClient.Endpoint);
                        SetupChannel(_replayClient.InnerChannel);
                    }
                }
                return _replayClient;
            }
        }

        void CloseReplayClient()
        {
            if (_replayClient != null)
            {
                ReplayClient.Close();
            }
        }

        [Test(Name = "GET SERVICES - REPLAY SERVICE",
            Order = "01.01.13",
            Id = "1-1-13",
            Category = Category.DEVICE,
            Path = "Device Management\\Capabilities",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReplayService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesReplayServicesTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(false);

                Service service = services.FindService(Definitions.Onvif.OnvifService.REPLAY);
                Assert(service != null, "Replay service not found",
                       "Check that DUT returned Replay service address");
                // ToDo : version check?

                Assert(service.Capabilities == null, "Capabilities element not empty",
                       "Check that the DUT did not return service capabilities");
                
                services = GetServices(true);

                service = services.FindService(Definitions.Onvif.OnvifService.REPLAY );
                Assert(service != null, "Replay service not found",
                       "Check that DUT returned Replay service address");

                Assert(service.Capabilities != null, "Capabilities element is empty",
                       "Check that the DUT returned service capabilities");


            },
            () =>
            {
                CloseReplayClient();
            });
        }

        [Test(Name = "REPLAY SERVICE CAPABILITIES",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.REPLAY,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReplayService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReplayServiceCapabilities  })]
        public void ReplayServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                ReplayPortClient client = ReplayClient;

                Proxies.Onvif.ReplayServiceCapabilities capabilities = GetReplayCapabilities();
            }, 
            () =>
                {
                    CloseReplayClient();
                });
        }

        [Test(Name = "GET SERVICES AND GET REPLAY SERVICE CAPABILITIES CONSISTANCY",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.REPLAY,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReplayService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities, Functionality.GetServices })]
        public void CapabilitiesAndReplayServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service replayService = services.FindService(Definitions.Onvif.OnvifService.REPLAY);

                Assert(replayService != null, "No Replay service information returned", "Check that the DUT returned Replay service information");

                Assert((replayService.Capabilities != null), "No Capabilities information included",
                       "Check that Capabilities element is included in Services element");

                ReplayServiceCapabilities serviceCapabilities = ParseReplayCapabilities(replayService.Capabilities);

                ReplayServiceCapabilities capabilities = GetReplayCapabilities();
                
                CompareCapabilities(serviceCapabilities, capabilities);

            },
            () =>
            {
                CloseReplayClient();
            });
        }

        #region Validation

        // first - via GetServices;
        // second - via ServiceCapabilities
        void CompareCapabilities(ReplayServiceCapabilities serviceCapabilities, ReplayServiceCapabilities capabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;
            bool local;

            List<TestUtils.CheckSettings<ReplayServiceCapabilities>> batch = new List<TestUtils.CheckSettings<ReplayServiceCapabilities>>();

            batch.Add(new TestUtils.CheckSettings<ReplayServiceCapabilities>()
                          {
                              FieldName = "ReversePlayback",
                              SpecifiedSelector = (S) => true, 
                              ValueSelector = (S) => S.ReversePlayback
                          });

            local = TestUtils.BatchCheckBoolean(serviceCapabilities, capabilities, "GetServices", "GetServiceCapabilities", batch, "", dump);
            equal = local;
            
            // second field - range
            // must compare float[]
            
            bool scNotEmpty = serviceCapabilities.SessionTimeoutRange != null &&
                              serviceCapabilities.SessionTimeoutRange.Length > 0;
            bool capNotEmpty = capabilities.SessionTimeoutRange != null &&
                               capabilities.SessionTimeoutRange.Length > 0;

            if (scNotEmpty && capNotEmpty)
            {
                foreach (float value in serviceCapabilities.SessionTimeoutRange)
                {
                    if (!capabilities.SessionTimeoutRange.Contains(value))
                    {
                        equal = false;
                        // value not found
                        dump.AppendFormat("Value {0} not found in SessionTimeoutRange received via GetServiceCapabilities{0}", Environment.NewLine);
                    }
                }
                foreach (float value in capabilities.SessionTimeoutRange)
                {
                    if (!serviceCapabilities.SessionTimeoutRange.Contains(value))
                    {
                        equal = false;
                        // value not found
                        dump.AppendFormat("Value {0} not found in SessionTimeoutRange received via GetServices{0}", Environment.NewLine);
                    }
                }
            }
            else
            {
                if (scNotEmpty)
                {
                    equal = false;
                    dump.AppendFormat("SessionTimeoutRange is missing in structure received via GetServiceCapabilities{0}", Environment.NewLine);
                }

                if (capNotEmpty)
                {
                    equal = false;
                    dump.AppendFormat("SessionTimeoutRange is missing in structure received via GetServices{0}", Environment.NewLine);
                }
            }

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }
            
            StepPassed();
        }
        
        #endregion
        
        #region Steps

        protected ReplayServiceCapabilities ParseReplayCapabilities(XmlElement element)
        {
            return ExtractCapabilities<ReplayServiceCapabilities>(element,
                                                                   Definitions.Onvif.OnvifService.REPLAY);
        }

        protected ReplayServiceCapabilities GetReplayCapabilities()
        {
            ReplayPortClient client = ReplayClient;
            ReplayServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }

        #endregion


    }

}
