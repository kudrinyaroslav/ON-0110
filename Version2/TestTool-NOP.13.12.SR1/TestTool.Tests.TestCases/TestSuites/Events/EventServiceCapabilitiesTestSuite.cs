using System;
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
using TestTool.Proxies.Event;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
#if FULL
    [TestClass]
#endif
    class EventServiceCapabilitiesTestSuite : DeviceManagementTest
    {
        public EventServiceCapabilitiesTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Event Handling\\Capabilities";

        private string _eventServiceAddress;
        private EventPortTypeClient _eventClient;

        protected EventPortTypeClient EventClient
        {
            get
            {
                if (_eventClient == null )
                {
                    bool found = false;
                    if (string.IsNullOrEmpty(_eventServiceAddress))
                    {
                        RunStep(() =>
                        {
                            string address = Client.GetEventServiceAddress(Features);
                            if (string.IsNullOrEmpty(address))
                            {
                                throw new AssertException("Events service not found");
                            }
                            else
                            {
                                _eventServiceAddress = address;
                                found = true;
                                LogStepEvent(_eventServiceAddress);
                            }
                        }, "Get event service address", OnvifFaults.NoSuchService , true, true);
                        DoRequestDelay();
                    }

                    Assert(found, "Event service address not found", "Check that the DUT returned Events service address");

                    if (found)
                    {
                        EndpointController controller = new EndpointController(new EndpointAddress(_eventServiceAddress));
                        controller.WsaEnabled = true;

                        Binding binding = CreateBinding(
                            false,
                            new IChannelController[] { new SoapValidator(EventsSchemasSet.GetInstance()), controller });

                        _eventClient = new EventPortTypeClient(binding, new EndpointAddress(_eventServiceAddress));

                        AttachSecurity(_eventClient.Endpoint);
                        SetupChannel(_eventClient.InnerChannel);
                    }
                }
                return _eventClient;
            }
        }

        void CloseEventClient()
        {
            if (_eventClient != null)
            {
                EventClient.Close();
            }
        }
        
        /*[Test(Name = "EVENT SERVICE CAPABILITIES",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.EVENT,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventsServiceCapabilities })]
        */public void DeviceServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                EventPortTypeClient client = EventClient;

                Proxies.Event.Capabilities capabilities = GetEventCapabilities();
            }, 
            () =>
                {
                    CloseEventClient();
                });
        }

        /*[Test(Name = "GET SERVICES AND EVENT SERVICE CAPABILITIES CONSISTENCY",
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.EVENT,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities, Functionality.GetServices })]
       */ public void CapabilitiesAndEventsServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service eventsService = services.FindService(Definitions.Onvif.OnvifService.EVENTS);

                Assert(eventsService != null, "No events service information returned", "Check that the DUT returned events service information");

                if (eventsService.Capabilities == null)
                {
                    LogTestEvent("No Capabilities information included, skip the test" + Environment.NewLine);
                }
                else
                {
                    Proxies.Event.Capabilities capabilities = GetEventCapabilities();

                    Proxies.Event.Capabilities serviceCapabilities = ParseEventCapabilities(eventsService.Capabilities);

                    CompareCapabilities(serviceCapabilities, capabilities);

                }
            },
            () =>
            {
                CloseEventClient();
            });
        }
        
        /*[Test(Name = "CAPABILITIES AND EVENT SERVICE CAPABILITIES CONSISTENCY",
            Order = "05.01.03",
            Id = "5-1-3",
            Category = Category.EVENT,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventsServiceCapabilities, Functionality.GetServices })]
        */public void EventsServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                Proxies.Onvif.Capabilities capabilities = GetCapabilities(new CapabilityCategory[] { CapabilityCategory.Events  });

                Assert(capabilities.Events != null, "Events field is empty", "Check that the DUT returned Events capabilities");

                Proxies.Event.Capabilities serviceCapabilities = GetEventCapabilities();

                // compare...
                CompareCapabilities(serviceCapabilities, capabilities);
            },
            () =>
            {
                CloseEventClient();
            });
        }


        #region Validation

        void CompareCapabilities(Proxies.Event.Capabilities serviceSapabilities, Proxies.Event.Capabilities capabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            List<TestUtils.CheckSettings<Proxies.Event.Capabilities>> batch = new List<TestUtils.CheckSettings<Proxies.Event.Capabilities>>();

            batch.Add(new TestUtils.CheckSettings<Proxies.Event.Capabilities>() { FieldName = "WSPausableSubscriptionManagerInterfaceSupport", SpecifiedSelector = (S) => S.WSPausableSubscriptionManagerInterfaceSupportSpecified, ValueSelector = (S) => S.WSPausableSubscriptionManagerInterfaceSupport });
            batch.Add(new TestUtils.CheckSettings<Proxies.Event.Capabilities>() { FieldName = "WSPullPointSupport", SpecifiedSelector = (S) => S.WSPullPointSupportSpecified, ValueSelector = (S) => S.WSPullPointSupport });
            batch.Add(new TestUtils.CheckSettings<Proxies.Event.Capabilities>() { FieldName = "WSSubscriptionPolicySupport", SpecifiedSelector = (S) => S.WSSubscriptionPolicySupportSpecified, ValueSelector = (S) => S.WSSubscriptionPolicySupport });

            local = TestUtils.BatchCheckBoolean(serviceSapabilities, capabilities, "GetServices", "GetServiceCapabilities", batch, "", dump);
            equal = equal && local;

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }

            StepPassed();
        }

        void CompareCapabilities(Proxies.Event.Capabilities serviceSapabilities, Proxies.Onvif.Capabilities capabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            Proxies.Onvif.EventCapabilities events = capabilities.Events;


            local = CheckCapabilitiesField(serviceSapabilities.WSPausableSubscriptionManagerInterfaceSupportSpecified,
                             serviceSapabilities.WSPausableSubscriptionManagerInterfaceSupport,
                             events !=null,
                             events != null ? events.WSPausableSubscriptionManagerInterfaceSupport : false,
                             "WSPausableSubscriptionManagerInterfaceSupport", dump);
            equal = equal && local;

            local = CheckCapabilitiesField(serviceSapabilities.WSPullPointSupportSpecified,
                                         serviceSapabilities.WSPullPointSupport,
                                         events != null,
                                         events != null ? events.WSPullPointSupport : false,
                                         "WSPullPointSupport", dump);
            equal = equal && local;
            local = CheckCapabilitiesField(serviceSapabilities.WSSubscriptionPolicySupportSpecified,
                                         serviceSapabilities.WSSubscriptionPolicySupport,
                                         events != null,
                                         events != null ? events.WSSubscriptionPolicySupport : false,
                                         "WSSubscriptionPolicySupport", dump);
            equal = equal && local;

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }

            StepPassed();
        }

        protected bool CheckCapabilitiesField(bool specified1, bool field1,
            bool specified2, bool field2,
            string fieldName, StringBuilder dump)
        {
            return TestUtils.CheckField(specified1,
                             field1,
                             specified2,
                             field2,
                             fieldName, "GetServiceCapabilities", "GetCapabilities", dump);
        }

        #endregion



        #region Steps

        protected Proxies.Event.Capabilities ParseEventCapabilities(XmlElement element)
        {
            return ExtractCapabilities<Proxies.Event.Capabilities>(element,
                                                                   Definitions.Onvif.OnvifService.EVENTS);
        }

        protected Proxies.Event.Capabilities GetEventCapabilities()
        {
            EventPortTypeClient client = EventClient;
            Proxies.Event.Capabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }

        #endregion

    }
}
