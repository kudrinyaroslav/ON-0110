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
    [TestClass]
   partial class ServiceCapabilitiesTestSuite : DeviceManagementTest
    {
        public ServiceCapabilitiesTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        protected override void Release()
        {
            base.Release();
            CloseClients(); 
        }

        private const string PATH_DEVICE = "Device Management\\Capabilities";
        private const string PATH_EVENTS = "Event Handling\\Capabilities";
        private const string PATH_MEDIA = "Media Configuration\\Capabilities";
        private const string PATH_PTZ = "PTZ\\Capabilities";
        private const string PATH_IMAGING = "Imaging\\Capabilities";

        private const string GETSERVICES = "GetServices";
        private const string GETSERVICECAPABILITIES = "GetServiceCapabilities";
        private const string COMPARECAPABILITIESSTEP = "Compare Capabilities";
        private const string SETTINGSDONTMATCH = "Settings don't match";

        #region Clients
        
        private ServiceHolder<EventPortTypeClient, EventPortType> _eventServiceHolder;
        private ServiceHolder<MediaClient, Media> _mediaServiceHolder;
        private ServiceHolder<PTZClient, PTZ> _ptzServiceHolder;
        private ServiceHolder<ImagingPortClient, ImagingPort> _imagingServiceHolder;
        
        protected EventPortTypeClient EventClient
        {
            get
            {
                if (_eventServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_eventServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(EventsSchemasSet.GetInstance()),
                                                           };
                   
                    InitServiceClient(_eventServiceHolder, true, controllers);

                }
                return _eventServiceHolder.Client;
            }
        }

        protected MediaClient MediaClient
        {
            get
            {
                if (_mediaServiceHolder == null)
                {
                    InitServiceHolders();
                }
                
                if (_mediaServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(MediaSchemasSet.GetInstance()),
                                                           };

                    InitServiceClient(_mediaServiceHolder, false, controllers);

                }
                return _mediaServiceHolder.Client;
            }
        }

        protected PTZClient PTZClient
        {
            get
            {
                if (_ptzServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_ptzServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(PtzSchemasSet.GetInstance()),
                                                           };

                    InitServiceClient(_ptzServiceHolder, false, controllers);

                }
                return _ptzServiceHolder.Client;
            }
        }

        protected ImagingPortClient ImagingClient
        {
            get
            {
                if (_imagingServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_imagingServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(PtzSchemasSet.GetInstance()),
                                                           };

                    InitServiceClient(_imagingServiceHolder, false, controllers);

                }
                return _imagingServiceHolder.Client;
            }
        }

        void InitServiceHolders()
        {
            // event
            _eventServiceHolder = new ServiceHolder<EventPortTypeClient, EventPortType>(
                Client.GetEventServiceAddress, 
                (binding, address) => { return new EventPortTypeClient(binding, address); },
                "Event");
            
            // media 
            _mediaServiceHolder = new ServiceHolder<MediaClient, Media>(
                Client.GetMediaServiceAddress,
                (binding, address) => { return new MediaClient(binding, address); },
                "Media");


            // PTZ
            _ptzServiceHolder = new ServiceHolder<PTZClient, PTZ>(
                Client.GetPtzServiceAddress,
                (binding, address) => { return new PTZClient(binding, address); },
                "PTZ");
            
            // Imaging
            _imagingServiceHolder = new ServiceHolder<ImagingPortClient, ImagingPort>(
                Client.GetImagingServiceAddress,
                (binding, address) => { return new ImagingPortClient(binding, address); },
                "Imaging");

        }

        void InitServiceClient(ServiceHolder serviceHolder, bool wsa, IEnumerable<IChannelController> controllers)
        {
            bool found = false;
            if (!serviceHolder.HasAddress)
            {
                RunStep(() =>
                {
                    serviceHolder.Retrieve(Features);
                    if (!serviceHolder.HasAddress)
                    {
                        throw new AssertException(string.Format("{0} service not found", serviceHolder.ServiceName));
                    }
                    else
                    {
                        found = true;
                        LogStepEvent(serviceHolder.Address);
                    }
                }, string.Format("Get {0} service address", serviceHolder.ServiceName), 
                OnvifFaults.NoSuchService , true, true);
                DoRequestDelay();
            }

            Assert(found, 
                string.Format("{0} service address not found", serviceHolder.ServiceName), 
                string.Format("Check that the DUT returned {0} service address", serviceHolder.ServiceName));

            if (found)
            {
                EndpointController controller = new EndpointController(new EndpointAddress(serviceHolder.Address));

                WsaController wsaController = new WsaController(); 

                List<IChannelController> ctrls= new List<IChannelController>();
                ctrls.Add(controller);
                ctrls.AddRange(controllers);

                if (wsa)
                {
                    ctrls.Add(wsaController);
                }

                Binding binding = CreateBinding(
                    false,
                    ctrls);

                serviceHolder.CreateClient(binding, AttachSecurity, SetupChannel);
            }
        }

        void CloseClients()
        {
            foreach (ServiceHolder sh in new ServiceHolder[]{_eventServiceHolder, _mediaServiceHolder, _ptzServiceHolder, _imagingServiceHolder} )
            {
                if (sh != null)
                {
                    sh.Close();
                }
            }
        }
        
        #endregion
        
        /* Erdinger Tests */
        [Test(Name = "GET SERVICES – DEVICE SERVICE",
            Order = "01.01.13",
            Id = "1-1-13",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] {  Functionality.GetServices })]
        public void GetServicesDeviceServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.DEVICE, "Device");
            }); 
        }

        [Test(Name = "GET SERVICES – MEDIA SERVICE",
             Order = "01.01.14",
             Id = "1-1-14",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.MediaService },
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesMediaServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.MEDIA, "Media");
            });
        }

        [Test(Name = "GET SERVICES – PTZ SERVICE",
             Order = "01.01.15",
             Id = "1-1-15",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.PTZService },
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesPtzServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.PTZ, "PTZ");
            });
        }
        
        [Test(Name = "GET SERVICES – EVENT SERVICE",
             Order = "01.01.16",
             Id = "1-1-16",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetServices },
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesEventServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.EVENTS, "Event");
            });
        }

        [Test(Name = "GET SERVICES – IMAGING SERVICE",
             Order = "01.01.17",
             Id = "1-1-17",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ImagingService },
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesImagingServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.IMAGING, "Imaging");
            });
        }

        [Test(Name = "GET SERVICES - REPLAY SERVICE",
            Order = "01.01.20",
            Id = "1-1-20",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReplayService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void ReplayServicesTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.REPLAY, "Replay");
            });
        }

        [Test(Name = "GET SERVICES – RECORDING SEARCH SERVICE",
            Order = "01.01.21",
            Id = "1-1-21",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingSearchService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void SearchServicesTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.SEARCH, "Search");
            });
        }

        [Test(Name = "GET SERVICES – RECORDING CONTROL SERVICE",
            Order = "01.01.22",
            Id = "1-1-22",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void RecordingServicesTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.RECORIDING, "Recording");
            });
        }

        [Test(Name = "GET SERVICES – RECEIVER SERVICE",
            Order = "01.01.23",
            Id = "1-1-23",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void ReceiverServicesTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.RECEIVER, "Receiver");
            });
        }

        [Test(Name = "GET SERVICES – ADVANCED SECURITY SERVICE",
             Order = "01.01.26",
             Id = "1-1-26",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Optional,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.AdvancedSecurity },
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesAdvancedSecurityServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.ADVANCED_SECURITY, "Advanced Security");
            });
        }

#if __PROFILE_A__
        [Test(Name = "GET SERVICES – ACCESS RULES SERVICE",
             Order = "01.01.27",
             Id = "1-1-27",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Optional,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.AccessRulesService },
             LastChangedIn = "v15.06",
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesAccessRulesServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.ACCESS_RULES_SERVICE, "Access Rules");
            });
        }

        [Test(Name = "GET SERVICES – CREDENTIALS SERVICE",
             Order = "01.01.28",
             Id = "1-1-28",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Optional,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.Credential },
             LastChangedIn = "v15.06",
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesCredentialsServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.CREDENTIAL_SERVICE, "Credentials");
            });
        }

        [Test(Name = "GET SERVICES – SCHEDULE SERVICE",
             Order = "01.01.29",
             Id = "1-1-29",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Optional,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.Schedule },
             LastChangedIn = "v15.06",
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesScheduleServiceTest()
        {
            RunTest(() =>
            {
                CommonCapabilitiesTest(OnvifService.SCHEDULE_SERVICE, "Schedule");
            });
        }
#endif



        #region Service capabilities

        //moved from 1.1.13
        [Test(Name = "DEVICE SERVICE CAPABILITIES",
            Order = "01.01.18",
            Id = "1-1-18",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDeviceServiceCapabilities })]
        public void DeviceServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                DeviceServiceCapabilities capabilities = GetServiceCapabilities();

                // validate ?..

            });
        }
        
        // POSTPONED 
        [Test(Name = "EVENT SERVICE CAPABILITIES",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.EVENT,
            Path = PATH_EVENTS,
            Version = 2.1,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventsServiceCapabilities })]
        public void EventServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                EventPortTypeClient client = EventClient;

                Proxies.Event.EventServiceCapabilities eventServiceCapabilities = GetEventCapabilities();
            });
        }

        [Test(Name = "MEDIA SERVICE CAPABILITIES",
            Order = "08.01.01",
            Id = "8-1-1",
            Category = Category.MEDIA,
            Path = PATH_MEDIA,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetMediaServiceCapabilities })]
        public void MediaServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                MediaClient client = MediaClient;

                Proxies.Onvif.MediaServiceCapabilities capabilities = GetMediaCapabilities();
            });
        }

        [Test(Name = "PTZ SERVICE CAPABILITIES",
            Order = "08.01.01",
            Id = "8-1-1",
            Category = Category.PTZ,
            Path = PATH_PTZ,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetPTZServiceCapabilities })]
        public void PTZServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                PTZClient client = PTZClient;

                Proxies.Onvif.PtzServiceCapabilities capabilities = GetPtzCapabilities();
            });
        }


        [Test(Name = "IMAGING SERVICE CAPABILITIES",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.IMAGING,
            Path = PATH_IMAGING,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ImagingService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetImagingServiceCapabilities })]
        public void ImagingServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                ImagingPortClient client = ImagingClient;
                Proxies.Onvif.ImagingServiceCapabilities capabilities = GetImagingCapabilities();
            });
        }

        #endregion

        #region Services and service capabilities consistency

        [Test(Name = "GET SERVICES AND GET DEVICE SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.19",
            Id = "1-1-19",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDeviceServiceCapabilities, Functionality.GetServices })]
        public void DeviceServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service deviceService = services.FindService(OnvifService.DEVICE);

                Assert(deviceService != null, "No device service information returned", "Check that the DUT returned Device service information");

                string version = deviceService.Version == null
                     ? "missing"
                     : string.Format("{0}.{1}", deviceService.Version.Major, deviceService.Version.Minor);

                Assert(deviceService.Capabilities != null,
                    string.Format("Capabilities are not included in entry for Device service version {0}", version),
                    "Check that the DUT returned Capabilities element");
                
                DeviceServiceCapabilities capabilities = GetServiceCapabilities();

                DeviceServiceCapabilities serviceCapabilities = ExtractDeviceCapabilities(deviceService.Capabilities);

                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }
        
        // POSTPONED 
        [Test(Name = "GET SERVICES AND EVENT SERVICE CAPABILITIES CONSISTENCY",
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.EVENT,
            Path = PATH_EVENTS,
            Version = 2.1,
            LastChangedIn = "v15.06",
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventsServiceCapabilities })]
        public void CapabilitiesAndEventsServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service eventsService = services.FindService(OnvifService.EVENTS);

                Assert(eventsService != null, "No Events service information returned", "Check that the DUT returned events service information");

                string version = eventsService.Version == null
                 ? "missing"
                 : string.Format("{0}.{1}", eventsService.Version.Major, eventsService.Version.Minor);

                Assert(eventsService.Capabilities != null,
                    string.Format("Capabilities are not included in entry for Events service version {0}", version),
                    "Check that the DUT returned Capabilities element");
                
                Proxies.Event.EventServiceCapabilities eventServiceCapabilities = GetEventCapabilities();

                Proxies.Event.EventServiceCapabilities serviceEventServiceCapabilities = ParseEventCapabilities(eventsService.Capabilities);

                CompareCapabilities(serviceEventServiceCapabilities, eventServiceCapabilities);

            });
        }

        //[Test(Name = "CAPABILITIES AND EVENT SERVICE CAPABILITIES CONSISTENCY",
        //    Order = "05.01.03",
        //    Id = "5-1-3",
        //    Category = Category.EVENT,
        //    Path = PATH_EVENTS,
        //    Version = 2.1,
        //    RequirementLevel = RequirementLevel.Must,
        //    RequiredFeatures = new Feature[] { Feature.GetServices },
        //    FunctionalityUnderTest = new Functionality[] { Functionality.GetEventsServiceCapabilities, Functionality.GetServices })]
        //public void EventsServiceCapabilitiesConsistencyTest()
        //{
        //    RunTest(() =>
        //    {
        //        Proxies.Onvif.Capabilities capabilities = GetCapabilities(new CapabilityCategory[] { CapabilityCategory.Events });

        //        Assert(capabilities.Events != null, "Events field is empty", "Check that the DUT returned Events capabilities");

        //        Proxies.Event.Capabilities serviceCapabilities = GetEventCapabilities();

        //        // compare...
        //        CompareCapabilities(serviceCapabilities, capabilities);
        //    });
        //}


        // Media 
        [Test(Name = "GET SERVICES AND GET MEDIA SERVICE CAPABILITIES CONSISTENCY",
            Order = "08.01.02",
            Id = "8-1-2",
            Category = Category.MEDIA,
            Path = PATH_MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetMediaServiceCapabilities })]
        public void MediaServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service mediaService = services.FindService(OnvifService.MEDIA);

                Assert(mediaService != null, "No media service information returned", "Check that the DUT returned Media service information");

                string version = mediaService.Version == null
                 ? "missing"
                 : string.Format("{0}.{1}", mediaService.Version.Major, mediaService.Version.Minor);

                Assert(mediaService.Capabilities != null,
                    string.Format("Capabilities are not included in entry for Media serice version {0}", version),
                    "Check that the DUT returned Capabilities element");
                
                MediaServiceCapabilities capabilities = GetMediaCapabilities();

                MediaServiceCapabilities serviceCapabilities = ExtractMediaCapabilities(mediaService.Capabilities);

                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }
        
        // PTZ 
        [Test(Name = "GET SERVICES AND GET PTZ SERVICE CAPABILITIES CONSISTENCY",
            Order = "08.01.02",
            Id = "8-1-2",
            Category = Category.PTZ,
            Path = PATH_PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetPTZServiceCapabilities })]
        public void PtzServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service ptzService = services.FindService(OnvifService.PTZ);

                Assert(ptzService != null, "No PTZ service information returned", "Check that the DUT returned PTZ service information");

                string version = ptzService.Version == null
                 ? "missing"
                 : string.Format("{0}.{1}", ptzService.Version.Major, ptzService.Version.Minor);

                Assert(ptzService.Capabilities != null,
                    string.Format("Capabilities are not included in entry for PTZ sevice version {0}", version),
                    "Check that the DUT returned Capabilities element");
                
                PtzServiceCapabilities capabilities = GetPtzCapabilities();

                PtzServiceCapabilities serviceCapabilities = ExtractPtzCapabilities(ptzService.Capabilities);

                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }
        
        // Imaging 
        [Test(Name = "GET SERVICES AND GET IMAGING SERVICE CAPABILITIES CONSISTENCY",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.IMAGING,
            Path = PATH_IMAGING,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ImagingService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetImagingServiceCapabilities })]
        public void ImagingServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service imagingService = services.FindService(OnvifService.IMAGING);

                Assert(imagingService != null, "No Imaging service information returned", "Check that the DUT returned Imaging service information");

                string version = imagingService.Version == null
                     ? "missing"
                     : string.Format("{0}.{1}", imagingService.Version.Major, imagingService.Version.Minor);

                Assert(imagingService.Capabilities != null,
                    string.Format("Capabilities are not included in entry for Imaging service version {0}", version),
                    "Check that the DUT returned Capabilities element");
                
                ImagingServiceCapabilities capabilities = GetImagingCapabilities();

                ImagingServiceCapabilities serviceCapabilities = ExtractImagingCapabilities(imagingService.Capabilities);

                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }

        #endregion

        #region capabilities and service capabilities
        
        // Moved from DeviceManagementCapabilitiesTest.cs, just to have common functions in one place 
        /*[Test(Name = "CAPABILITIES AND DEVICE SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.12",
            Id = "1-1-12",
            Category = Category.DEVICE,
            Path = "Device Management\\Capabilities",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities, Functionality.GetServices })]*/
        public void CapabilitiesAndDeviceServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Proxies.Onvif.Capabilities capabilities = GetCapabilities(new CapabilityCategory[] { CapabilityCategory.Device });

                Assert(capabilities.Device != null, "Device field is empty", "Check that the DUT returned Device capabilities");

                DeviceServiceCapabilities serviceCapabilities = GetServiceCapabilities();

                // compare...
                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }
        
        #endregion

        #region Utils

        void CommonCapabilitiesTest(string ns, string name)
        {
            Service[] servicesList = GetServices(false);

            Service service = null;
            Action<Service[]> check = new Action<Service[]>(
                (services) =>
                {
                    service = services.FindService(ns);

                    Assert(service != null,
                        string.Format("{0} service not found", name),
                        string.Format("Check that DUT returned {0} service address", name));
                });

            check(servicesList);

            string version = service.Version == null
                     ? "missing"
                     : string.Format("'{0}.{1}'", service.Version.Major, service.Version.Minor);


            // Check that no capabilities returned
            Assert(service.Capabilities == null,
                string.Format("Capabilities are included in entry for {0} service version {1}", name, version),
                "Check that no Capabilities returned");

            servicesList = GetServices(true);
            check(servicesList);

            // check Capabilities
            version = service.Version == null
                                 ? "missing"
                                 : string.Format("'{0}.{1}'", service.Version.Major, service.Version.Minor);

            Assert(service.Capabilities != null,
                string.Format("Capabilities are not included in entry for {0} service version {1}", name, version),
                "Check that the DUT returned Capabilities element");

            // validator 
            XmlElementValidator validator = null;

            StringBuilder sb = new StringBuilder();
            bool hasErrors = false;

            if (service.Capabilities.LocalName != "Capabilities" ||
                service.Capabilities.NamespaceURI.ToLower() != ns.ToLower())
            {
                hasErrors = true;
                
                sb.AppendFormat("Capabilities element included in entry with version {0} is incorrect: child element must be 'Capabilities' from namespace {1} {2}",
                                version, ns, Environment.NewLine);
            }

            // schema validation will be performed automatically only for Device service
            BaseSchemaSet schemaSet = TypesSchemaSet.GetInstance();
            validator = new XmlElementValidator(schemaSet);

            //validate
            XmlElement capabilities = service.Capabilities;
            string error = string.Empty;
            try
            {
                validator.Validate(capabilities);
            }
            catch (Exception exc)
            {
                hasErrors = true;
                error = exc.Message;

                sb.AppendFormat("Capabilities element included in entry for {0} service  with version {1} is incorrect: {2} {3}",
                                name, version, error, Environment.NewLine);
            }

            string errDump = sb.ToStringTrimNewLine();
            Assert(!hasErrors, errDump, "Check that Capabilities element is correct");
        }
        
        #endregion

        #region Validation

        void CompareCapabilities(DeviceServiceCapabilities fromGetServices, 
            DeviceServiceCapabilities fromGetCapabilities)
        {
            string FROMGETSERVICES = string.Format("received via {0}", GETSERVICES);
            string FROMGETCAPABILITIES = string.Format("received via {0}", GETSERVICECAPABILITIES);

            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            if (fromGetServices.Network != null || fromGetCapabilities.Network != null)
            {
                NetworkCapabilities network1 = fromGetServices.Network;
                NetworkCapabilities network2 = fromGetCapabilities.Network;

                List<TestUtils.CheckSettings<NetworkCapabilities>> batch = new List<TestUtils.CheckSettings<NetworkCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "IPFilter", SpecifiedSelector = (S) => S.IPFilterSpecified, ValueSelector = (S) => S.IPFilter });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "ZeroConfiguration", SpecifiedSelector = (S) => S.ZeroConfigurationSpecified, ValueSelector = (S) => S.ZeroConfiguration });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "IPVersion6", SpecifiedSelector = (S) => S.IPVersion6Specified, ValueSelector = (S) => S.IPVersion6 });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "DynDNS", SpecifiedSelector = (S) => S.DynDNSSpecified, ValueSelector = (S) => S.DynDNS });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "Dot11Configuration", SpecifiedSelector = (S) => S.Dot11ConfigurationSpecified, ValueSelector = (S) => S.Dot11Configuration });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "HostnameFromDHCP", SpecifiedSelector = (S) => S.HostnameFromDHCPSpecified, ValueSelector = (S) => S.HostnameFromDHCP });

                local = TestUtils.BatchCheckBooleanAllowMissing(network1, network2, GETSERVICES, GETSERVICECAPABILITIES, batch, "Network", dump);
                equal = equal && local;

                local = TestUtils.CheckIntField<NetworkCapabilities>(network1, network2, N => N != null ? N.NTP : 0, N => N != null ? N.NTPSpecified : false,
                                                           "Network.NTP", GETSERVICES, GETSERVICECAPABILITIES, dump);
                equal = equal && local;

            }

            if (fromGetCapabilities.System != null || fromGetServices.System != null)
            {
                SystemCapabilities system1 = fromGetServices.System;
                SystemCapabilities system2 = fromGetCapabilities.System;

                List<TestUtils.CheckSettings<SystemCapabilities>> batch = new List<TestUtils.CheckSettings<SystemCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "DiscoveryBye", SpecifiedSelector = (S) => S.DiscoveryByeSpecified, ValueSelector = (S) => S.DiscoveryBye });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "DiscoveryResolve", SpecifiedSelector = (S) => S.DiscoveryResolveSpecified, ValueSelector = (S) => S.DiscoveryResolve });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "FirmwareUpgrade", SpecifiedSelector = (S) => S.FirmwareUpgradeSpecified, ValueSelector = (S) => S.FirmwareUpgrade });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "HttpFirmwareUpgrade", SpecifiedSelector = (S) => S.HttpFirmwareUpgradeSpecified, ValueSelector = (S) => S.HttpFirmwareUpgrade });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "HttpSupportInformation", SpecifiedSelector = (S) => S.HttpSupportInformationSpecified, ValueSelector = (S) => S.HttpSupportInformation });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "HttpSystemBackup", SpecifiedSelector = (S) => S.HttpSystemBackupSpecified, ValueSelector = (S) => S.HttpSystemBackup });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "HttpSystemLogging", SpecifiedSelector = (S) => S.HttpSystemLoggingSpecified, ValueSelector = (S) => S.HttpSystemLogging });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "RemoteDiscovery", SpecifiedSelector = (S) => S.RemoteDiscoverySpecified, ValueSelector = (S) => S.RemoteDiscovery });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "SystemBackup", SpecifiedSelector = (S) => S.SystemBackupSpecified, ValueSelector = (S) => S.SystemBackup });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() { FieldName = "SystemLogging", SpecifiedSelector = (S) => S.SystemLoggingSpecified, ValueSelector = (S) => S.SystemLogging });

                local = TestUtils.BatchCheckBooleanAllowMissing(system1, system2, GETSERVICES, GETSERVICECAPABILITIES, batch, "System", dump);
                equal = equal && local;
            }

            if (fromGetServices.Security != null || fromGetCapabilities.Security != null)
            {
                SecurityCapabilities s1 = fromGetServices.Security;
                SecurityCapabilities s2 = fromGetCapabilities.Security;

                List<TestUtils.CheckSettings<SecurityCapabilities>> batch = new List<TestUtils.CheckSettings<SecurityCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "TLS10", SpecifiedSelector = (S) => S.TLS10Specified, ValueSelector = (S) => S.TLS10 });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "TLS11", SpecifiedSelector = (S) => S.TLS11Specified, ValueSelector = (S) => S.TLS11 });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "TLS12", SpecifiedSelector = (S) => S.TLS12Specified, ValueSelector = (S) => S.TLS12 });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "OnboardKeyGeneration", SpecifiedSelector = (S) => S.OnboardKeyGenerationSpecified, ValueSelector = (S) => S.OnboardKeyGeneration });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "AccessPolicyConfig", SpecifiedSelector = (S) => S.AccessPolicyConfigSpecified, ValueSelector = (S) => S.AccessPolicyConfig });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "Dot1X", SpecifiedSelector = (S) => S.Dot1XSpecified, ValueSelector = (S) => S.Dot1X });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "RemoteUserHandling", SpecifiedSelector = (S) => S.RemoteUserHandlingSpecified, ValueSelector = (S) => S.RemoteUserHandling });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "X509Token", SpecifiedSelector = (S) => S.X509TokenSpecified, ValueSelector = (S) => S.X509Token });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "SAMLToken", SpecifiedSelector = (S) => S.SAMLTokenSpecified, ValueSelector = (S) => S.SAMLToken });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "KerberosToken", SpecifiedSelector = (S) => S.KerberosTokenSpecified, ValueSelector = (S) => S.KerberosToken });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "UsernameToken", SpecifiedSelector = (S) => S.UsernameTokenSpecified, ValueSelector = (S) => S.UsernameToken });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "HttpDigest", SpecifiedSelector = (S) => S.HttpDigestSpecified, ValueSelector = (S) => S.HttpDigest });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "RELToken", SpecifiedSelector = (S) => S.RELTokenSpecified, ValueSelector = (S) => S.RELToken });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "DefaultAccessPolicy", SpecifiedSelector = (S) => S.DefaultAccessPolicySpecified, ValueSelector = (S) => S.DefaultAccessPolicy });

                local = TestUtils.BatchCheckBooleanAllowMissing(s1, s2, GETSERVICES, GETSERVICECAPABILITIES, batch, "Security", dump);
                equal = equal && local;


                bool check = TestUtils.BothNotNull(out local, "SupportedEAPMethod",
                                    FROMGETSERVICES, FROMGETCAPABILITIES, s1.SupportedEAPMethods,
                                    s2.SupportedEAPMethods, dump);

                if (check)
                {
                    local = CheckSupportedEAPMethods(s1.SupportedEAPMethods, s2.SupportedEAPMethods,
                        GETSERVICES, GETSERVICECAPABILITIES, dump);
                }
                equal = equal && local;

            }

            {
                List<string> commandsGetServices = null;
                List<string> commandsGetCapabilities = null;
                if (fromGetCapabilities.Misc != null)
                {
                    if (fromGetCapabilities.Misc.AuxiliaryCommands != null &&
                        fromGetCapabilities.Misc.AuxiliaryCommands.Length > 0)
                    {
                        commandsGetCapabilities = new List<string>(fromGetCapabilities.Misc.AuxiliaryCommands);
                    }
                }
                if (fromGetServices.Misc != null)
                {
                    if (fromGetServices.Misc.AuxiliaryCommands != null &&
                        fromGetServices.Misc.AuxiliaryCommands.Length > 0)
                    {
                        commandsGetServices = new List<string>(fromGetServices.Misc.AuxiliaryCommands);
                    }
                }

                bool check = TestUtils.BothNotNull(out local, "AuxiliaryCommands",
                                                   FROMGETSERVICES, FROMGETCAPABILITIES,
                                                   commandsGetServices,
                                                   commandsGetCapabilities, dump);

                if (check)
                {
                    foreach (string cmd in commandsGetCapabilities)
                    {
                        if (!commandsGetServices.Contains(cmd))
                        {
                            local = false;
                            dump.AppendFormat(
                                string.Format("Command '{0}' not found in the list {1}{2}", cmd, FROMGETSERVICES,
                                              Environment.NewLine));
                        }
                    }
                    foreach (string cmd in commandsGetServices)
                    {
                        if (!commandsGetCapabilities.Contains(cmd))
                        {
                            local = false;
                            dump.AppendFormat(
                                string.Format(
                                    "Command '{0}' not found in the list {1}{2}", cmd, FROMGETCAPABILITIES,
                                    Environment.NewLine));
                        }
                    }
                }

                equal = equal && local;
            }

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();
        }

        void CompareCapabilities(DeviceServiceCapabilities serviceCapabilities, 
            Proxies.Onvif.Capabilities capabilities)
        {
            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            if (serviceCapabilities.Network != null || capabilities.Device.Network != null)
            {
                NetworkCapabilities scNetwork = serviceCapabilities.Network;
                NetworkCapabilities1 cNetwork = capabilities.Device.Network;

                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.IPFilterSpecified : false,
                                             scNetwork != null ? scNetwork.IPFilter : false,
                                             cNetwork != null ? cNetwork.IPFilterSpecified : false,
                                             cNetwork != null ? cNetwork.IPFilter : false,
                                             "IPFilter", dump);
                equal = equal && local;
                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.ZeroConfigurationSpecified : false,
                                             scNetwork != null ? scNetwork.ZeroConfiguration : false,
                                             cNetwork != null ? cNetwork.ZeroConfigurationSpecified : false,
                                             cNetwork != null ? cNetwork.ZeroConfiguration : false,
                                             "ZeroConfiguration", dump);
                equal = equal && local;
                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.IPVersion6Specified : false,
                                             scNetwork != null ? scNetwork.IPVersion6 : false,
                                             cNetwork != null ? cNetwork.IPVersion6Specified : false,
                                             cNetwork != null ? cNetwork.IPVersion6 : false,
                                             "IPVersion6", dump);
                equal = equal && local;
                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.DynDNSSpecified : false,
                                             scNetwork != null ? scNetwork.DynDNS : false,
                                             cNetwork != null ? cNetwork.DynDNSSpecified : false,
                                             cNetwork != null ? cNetwork.DynDNS : false,
                                             "DynDNS", dump);
                equal = equal && local;

                bool dot11specified = false;
                bool dot11 = false;
                if (cNetwork != null && cNetwork.Extension != null)
                {
                    dot11specified = cNetwork.Extension.Dot11ConfigurationSpecified;
                    dot11 = cNetwork.Extension.Dot11Configuration;
                }

                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.Dot11ConfigurationSpecified : false,
                                             scNetwork != null ? scNetwork.Dot11Configuration : false,
                                             dot11specified,
                                             dot11,
                                             "Dot11Configuration", dump);
                equal = equal && local;
            }

            // System 

            if (serviceCapabilities.System != null || capabilities.Device.System != null)
            {
                SystemCapabilities scSystem = serviceCapabilities.System;
                SystemCapabilities1 cSystem = capabilities.Device.System;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.DiscoveryResolveSpecified : false,
                             scSystem != null ? scSystem.DiscoveryResolve : false,
                             cSystem != null,
                             cSystem != null ? cSystem.DiscoveryResolve : false,
                             "DiscoveryResolve", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.DiscoveryByeSpecified : false,
                             scSystem != null ? scSystem.DiscoveryBye : false,
                             cSystem != null,
                             cSystem != null ? cSystem.DiscoveryBye : false,
                             "DiscoveryBye", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.RemoteDiscoverySpecified : false,
                             scSystem != null ? scSystem.RemoteDiscovery : false,
                             cSystem != null,
                             cSystem != null ? cSystem.RemoteDiscovery : false,
                             "RemoteDiscovery", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.SystemBackupSpecified : false,
                             scSystem != null ? scSystem.SystemBackup : false,
                             cSystem != null,
                             cSystem != null ? cSystem.SystemBackup : false,
                             "SystemBackup", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.SystemLoggingSpecified : false,
                             scSystem != null ? scSystem.SystemLogging : false,
                             cSystem != null,
                             cSystem != null ? cSystem.SystemLogging : false,
                             "SystemLogging", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.FirmwareUpgradeSpecified : false,
                             scSystem != null ? scSystem.FirmwareUpgrade : false,
                             cSystem != null,
                             cSystem != null ? cSystem.FirmwareUpgrade : false,
                             "FirmwareUpgrade", dump);
                equal = equal && local;

                bool specified = false;
                bool value = false;
                if (cSystem != null && cSystem.Extension != null)
                {
                    specified = cSystem.Extension.HttpSystemBackupSpecified;
                    value = cSystem.Extension.HttpSystemBackup;
                }
                local = CheckCapabilitiesField(scSystem != null ? scSystem.HttpSystemBackupSpecified : false,
                             scSystem != null ? scSystem.HttpSystemBackup : false,
                             specified,
                             value,
                             "HttpSystemBackup", dump);
                equal = equal && local;

                specified = false;
                value = false;
                if (cSystem != null && cSystem.Extension != null)
                {
                    specified = cSystem.Extension.HttpSystemLoggingSpecified;
                    value = cSystem.Extension.HttpSystemLogging;
                }

                local = CheckCapabilitiesField(scSystem != null ? scSystem.HttpSystemLoggingSpecified : false,
                             scSystem != null ? scSystem.HttpSystemLogging : false,
                             specified,
                             value,
                             "HttpSystemLogging", dump);
                equal = equal && local;

                specified = false;
                value = false;
                if (cSystem != null && cSystem.Extension != null)
                {
                    specified = cSystem.Extension.HttpFirmwareUpgradeSpecified;
                    value = cSystem.Extension.HttpFirmwareUpgrade;
                }

                local = CheckCapabilitiesField(scSystem != null ? scSystem.HttpFirmwareUpgradeSpecified : false,
                             scSystem != null ? scSystem.HttpFirmwareUpgrade : false,
                             specified,
                             value,
                             "HttpFirmwareUpgrade", dump);
                equal = equal && local;
            }

            // Security 

            if (serviceCapabilities.Security != null || capabilities.Device.Security != null)
            {
                SecurityCapabilities scSecurity = serviceCapabilities.Security;
                SecurityCapabilities1 cSecurity = capabilities.Device.Security;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.TLS11Specified : false,
                                 scSecurity != null ? scSecurity.TLS11 : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.TLS11 : false,
                                 "TLS11", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.TLS12Specified : false,
                                 scSecurity != null ? scSecurity.TLS12 : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.TLS12 : false,
                                 "TLS12", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.OnboardKeyGenerationSpecified : false,
                                 scSecurity != null ? scSecurity.OnboardKeyGeneration : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.OnboardKeyGeneration : false,
                                 "OnBoardKeyGeneration", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.AccessPolicyConfigSpecified : false,
                                 scSecurity != null ? scSecurity.AccessPolicyConfig : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.AccessPolicyConfig : false,
                                 "AccessPolicyconfig", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.X509TokenSpecified : false,
                                 scSecurity != null ? scSecurity.X509Token : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.X509Token : false,
                                 "X509Token", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.SAMLTokenSpecified : false,
                                 scSecurity != null ? scSecurity.SAMLToken : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.SAMLToken : false,
                                 "SAMLToken", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.KerberosTokenSpecified : false,
                                 scSecurity != null ? scSecurity.KerberosToken : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.KerberosToken : false,
                                 "KerberosToken", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.RELTokenSpecified : false,
                                 scSecurity != null ? scSecurity.RELToken : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.RELToken : false,
                                 "RELToken", dump);
                equal = equal && local;

                bool specified = false;
                bool value = false;
                if (cSecurity != null && cSecurity.Extension != null)
                {
                    specified = true;
                    value = cSecurity.Extension.TLS10;
                }
                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.TLS10Specified : false,
                             scSecurity != null ? scSecurity.TLS10 : false,
                             specified,
                             value,
                             "TLS10", dump);
                equal = equal && local;

                // Compare Security settings 
                specified = false;
                value = false;
                if (cSecurity != null && cSecurity.Extension != null && cSecurity.Extension.Extension != null)
                {
                    specified = true;
                    value = cSecurity.Extension.Extension.Dot1X;
                }

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.Dot1XSpecified : false,
                             scSecurity != null ? scSecurity.Dot1X : false,
                             specified,
                             value,
                             "Dot1X", dump);
                equal = equal && local;

                int[] methods1 = null;

                if (scSecurity != null)
                {
                    methods1 = scSecurity.SupportedEAPMethods;
                }

                int[] methods2 = null;
                if (cSecurity != null && cSecurity.Extension != null && cSecurity.Extension.Extension != null)
                {
                    methods2 = cSecurity.Extension.Extension.SupportedEAPMethod;
                }

                TestUtils.BothNotNull(out local, "SupportedEAPMethod",
                   "got via GetServiceCapabilities", "got via GetCapabilities",
                   methods1, methods2, dump);

                if (methods1 != null && methods2 != null)
                {
                    local = CheckSupportedEAPMethods(
                        methods1,
                        methods2,
                        GETSERVICECAPABILITIES, "GetCapabilities", dump);

                    equal = equal && local;
                }


                specified = false;
                value = false;
                if (cSecurity != null && cSecurity.Extension != null && cSecurity.Extension.Extension != null)
                {
                    specified = true;
                    value = cSecurity.Extension.Extension.RemoteUserHandling;
                }

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.RemoteUserHandlingSpecified : false,
                             scSecurity != null ? scSecurity.RemoteUserHandling : false,
                             specified,
                             value,
                             "RemoteUserHandling", dump);
                equal = equal && local;

            }


            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();

        }

        void CompareCapabilities(Proxies.Event.EventServiceCapabilities serviceSapabilities, 
            Proxies.Event.EventServiceCapabilities eventServiceCapabilities)
        {
            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            List<TestUtils.CheckSettings<Proxies.Event.EventServiceCapabilities>> batch = new List<TestUtils.CheckSettings<Proxies.Event.EventServiceCapabilities>>();

            batch.Add(new TestUtils.CheckSettings<Proxies.Event.EventServiceCapabilities>() { FieldName = "WSPausableSubscriptionManagerInterfaceSupport", SpecifiedSelector = (S) => S.WSPausableSubscriptionManagerInterfaceSupportSpecified, ValueSelector = (S) => S.WSPausableSubscriptionManagerInterfaceSupport });
            batch.Add(new TestUtils.CheckSettings<Proxies.Event.EventServiceCapabilities>() { FieldName = "WSPullPointSupport", SpecifiedSelector = (S) => S.WSPullPointSupportSpecified, ValueSelector = (S) => S.WSPullPointSupport });
            batch.Add(new TestUtils.CheckSettings<Proxies.Event.EventServiceCapabilities>() { FieldName = "WSSubscriptionPolicySupport", SpecifiedSelector = (S) => S.WSSubscriptionPolicySupportSpecified, ValueSelector = (S) => S.WSSubscriptionPolicySupport });

            local = TestUtils.BatchCheckBooleanAllowMissing(serviceSapabilities, eventServiceCapabilities, GETSERVICES, GETSERVICECAPABILITIES, batch, "", dump);
            equal = equal && local;

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();
        }
        void CompareCapabilities(Proxies.Event.EventServiceCapabilities serviceSapabilities, Proxies.Onvif.Capabilities capabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            Proxies.Onvif.EventCapabilities events = capabilities.Events;


            local = CheckCapabilitiesField(serviceSapabilities.WSPausableSubscriptionManagerInterfaceSupportSpecified,
                             serviceSapabilities.WSPausableSubscriptionManagerInterfaceSupport,
                             events != null,
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
        
        void CompareCapabilities(MediaServiceCapabilities fromGetServices,
            MediaServiceCapabilities fromGetCapabilities)
        {
            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            {
                List<TestUtils.CheckSettings<MediaServiceCapabilities>> batch = new List<TestUtils.CheckSettings<MediaServiceCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<MediaServiceCapabilities>() { ValueSelector = S => S.SnapshotUri, SpecifiedSelector = S => S.SnapshotUriSpecified, FieldName = "SnapshotUri" });
                batch.Add(new TestUtils.CheckSettings<MediaServiceCapabilities>() { ValueSelector = S => S.Rotation, SpecifiedSelector = S => S.RotationSpecified, FieldName = "Rotation" });

                local = TestUtils.BatchCheckBooleanAllowMissing(fromGetServices, fromGetCapabilities, GETSERVICES, GETSERVICECAPABILITIES, batch, null, dump);
                equal = equal && local;
            }

            if (fromGetServices.ProfileCapabilities != null || fromGetCapabilities.ProfileCapabilities != null)
            {
                ProfileCapabilities1 profile1 = fromGetServices.ProfileCapabilities;
                ProfileCapabilities1 profile2 = fromGetCapabilities.ProfileCapabilities;

                local = TestUtils.CheckIntField<ProfileCapabilities1>(profile1, profile2,
                                                                      p => p != null ? p.MaximumNumberOfProfiles : 0,
                                                                      p => p != null ? p.MaximumNumberOfProfilesSpecified : false,
                                                                      "ProfileCapabilities.MaximumNumberOfProfiles", GETSERVICES,
                                                                      GETSERVICECAPABILITIES, dump);
                equal = equal && local;

            }

            if (fromGetServices.StreamingCapabilities != null || fromGetCapabilities.StreamingCapabilities != null)
            {
                StreamingCapabilities streaming1 = fromGetServices.StreamingCapabilities;
                StreamingCapabilities streaming2 = fromGetCapabilities.StreamingCapabilities;

                List<TestUtils.CheckSettings<StreamingCapabilities>> batch = new List<TestUtils.CheckSettings<StreamingCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<StreamingCapabilities>() { ValueSelector = S => S.NonAggregateControl, SpecifiedSelector = S => S.NonAggregateControlSpecified, FieldName = "NonAggregateControl" });
                batch.Add(new TestUtils.CheckSettings<StreamingCapabilities>() { ValueSelector = S => S.RTP_RTSP_TCP, SpecifiedSelector = S => S.RTP_RTSP_TCPSpecified, FieldName = "RTP_RTSP_TCP" });
                batch.Add(new TestUtils.CheckSettings<StreamingCapabilities>() { ValueSelector = S => S.RTP_TCP, SpecifiedSelector = S => S.RTP_TCPSpecified, FieldName = "RTP_TCP" });
                batch.Add(new TestUtils.CheckSettings<StreamingCapabilities>() { ValueSelector = S => S.RTPMulticast, SpecifiedSelector = S => S.RTPMulticastSpecified, FieldName = "RTPMulticast" });

                local = TestUtils.BatchCheckBooleanAllowMissing(streaming1, streaming2, GETSERVICES, GETSERVICECAPABILITIES, batch, "Streaming", dump);
                equal = equal && local;
                
            }

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();
        }
        
        void CompareCapabilities(PtzServiceCapabilities fromGetServices,
            PtzServiceCapabilities fromGetCapabilities)
        {
            // nothing to compare
            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            List<TestUtils.CheckSettings<PtzServiceCapabilities>> batch = new List<TestUtils.CheckSettings<PtzServiceCapabilities>>();

            batch.Add(new TestUtils.CheckSettings<PtzServiceCapabilities>() { ValueSelector = S => S.EFlip, SpecifiedSelector = S => S.EFlipSpecified, FieldName = "EFlip" });
            batch.Add(new TestUtils.CheckSettings<PtzServiceCapabilities>() { ValueSelector = S => S.Reverse, SpecifiedSelector = S => S.ReverseSpecified, FieldName = "Reverse" });

            bool local = TestUtils.BatchCheckBooleanAllowMissing(fromGetServices, fromGetCapabilities, GETSERVICES, GETSERVICECAPABILITIES, batch, null, dump);
            equal = equal && local;


            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();
        }
        
        void CompareCapabilities(ImagingServiceCapabilities fromGetServices, 
            ImagingServiceCapabilities fromGetCapabilities)
        {
            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            equal =
            TestUtils.CheckField(fromGetServices.ImageStabilizationSpecified, fromGetServices.ImageStabilization,
                                 fromGetCapabilities.ImageStabilizationSpecified, fromGetCapabilities.ImageStabilization,
                                 "ImageStabilization", "from GetServices", "from GetCapabilities", dump);

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();
        }
        
        bool CheckCapabilitiesField(bool specified1, bool field1,
            bool specified2, bool field2,
            string fieldName, StringBuilder dump)
        {
            return TestUtils.CheckField(specified1,
                             field1,
                             specified2,
                             field2,
                             fieldName, GETSERVICECAPABILITIES, "GetCapabilities", dump);
        }

        bool CheckSupportedEAPMethods(int[] arr1, int[] arr2, string descr1, string descr2, StringBuilder dump)
        {
            bool equal = true;

            List<int> l1 = new List<int>();
            if (arr1 != null)
            {
                l1.AddRange(arr1);
            }

            List<int> l2 = new List<int>();
            if (arr2 != null)
            {
                l2.AddRange(arr2);
            }

            List<int> notFound1 = new List<int>();
            List<int> notFound2 = new List<int>();

            Action<List<int>, List<int>, List<int>> select =
                new Action<List<int>, List<int>, List<int>>(
                    (list1, list2, notFound) =>
                    {
                        foreach (int val in list1)
                        {
                            if (!list2.Contains(val) && !notFound.Contains(val))
                            {
                                notFound.Add(val);
                            }
                        }

                    });

            select(l1, l2, notFound1);
            select(l2, l1, notFound2);

            Action<List<int>, string> dumpNotFound = new Action<List<int>, string>(
                (list, fieldName) =>
                {
                    if (list.Count > 0)
                    {
                        equal = false;

                        StringBuilder lst = new StringBuilder("Value(s) ");
                        bool first = true;
                        foreach (int val in list)
                        {
                            if (first)
                            {
                                lst.AppendFormat("{0}", val);
                                first = false;
                            }
                            else
                            {
                                lst.AppendFormat(", {0}", val);
                            }
                        }

                        lst.AppendFormat(" not found in SupportedEAPMethods in structure got via {0}{1}",
                                         fieldName, Environment.NewLine);

                        dump.Append(lst);
                    }
                });

            dumpNotFound(notFound1, descr2);
            dumpNotFound(notFound2, descr1);

            return equal;

        }
        
        #endregion
        
        #region Steps

        protected Proxies.Event.EventServiceCapabilities ParseEventCapabilities(XmlElement element)
        {
            return ExtractCapabilities<Proxies.Event.EventServiceCapabilities>(element,
                                                                   OnvifService.EVENTS);
        }

        protected DeviceServiceCapabilities ExtractDeviceCapabilities(XmlElement element)
        {
            return ExtractCapabilities<DeviceServiceCapabilities>(element,
                                                       OnvifService.DEVICE);
        }
        
        protected MediaServiceCapabilities ExtractMediaCapabilities(XmlElement element)
        {
            return ExtractCapabilities<MediaServiceCapabilities>(element,
                                                       OnvifService.MEDIA);
        }
        
        protected PtzServiceCapabilities ExtractPtzCapabilities(XmlElement element)
        {
            return ExtractCapabilities<PtzServiceCapabilities>(element,
                                                       OnvifService.PTZ);
        }
        
        protected ImagingServiceCapabilities ExtractImagingCapabilities(XmlElement element)
        {
            return ExtractCapabilities<ImagingServiceCapabilities>(element,
                                                       OnvifService.IMAGING);
        }

        protected Proxies.Event.EventServiceCapabilities GetEventCapabilities()
        {
            EventPortTypeClient client = EventClient;
            Proxies.Event.EventServiceCapabilities eventServiceCapabilities = null;
            RunStep(() => { eventServiceCapabilities = client.GetServiceCapabilities(); }, "Get Event Service Capabilities");
            return eventServiceCapabilities;
        }

        protected MediaServiceCapabilities GetMediaCapabilities()
        {
            MediaClient client = MediaClient;
            MediaServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(new GetServiceCapabilities()).Capabilities; }, "Get Service Capabilities");
            return capabilities;
        }

        protected PtzServiceCapabilities GetPtzCapabilities()
        {
            PTZClient client = PTZClient;
            PtzServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }

        protected ImagingServiceCapabilities GetImagingCapabilities()
        {
            ImagingPortClient client = ImagingClient;
            ImagingServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }

        #endregion

    }
}
