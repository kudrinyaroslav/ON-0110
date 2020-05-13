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
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class PACSServiceCapabilitiesTestSuite : DeviceManagementTest
    {
        public PACSServiceCapabilitiesTestSuite(TestLaunchParam param): base(param)
        {

        }

        protected override void Release()
        {
            base.Release();
            CloseClients();
        }

        private const string PATH_DEVICE = "Device Management\\Capabilities";

        private const string PATH_DOORCONTROL = "Door Control\\Capabilities";
        //private const string PATH_USERS = "Users\\Capabilities";
        private const string PATH_ACCESSCONTROL = "Access Control\\Capabilities";

        private const string GETSERVICES = "GetServices";
        private const string GETSERVICECAPABILITIES = "GetServiceCapabilities";
        private const string COMPARECAPABILITIESSTEP = "Compare Capabilities";
        private const string SETTINGSDONTMATCH = "Settings don't match";

        #region Clients

        private ServiceHolder<DoorControlPortClient, DoorControlPort> _doorControlServiceHolder;
        //private ServiceHolder<UserPortClient, UserPort> _userServiceHolder;
        private ServiceHolder<PACSPortClient, PACSPort> _pacsServiceHolder;

        protected DoorControlPortClient DoorControlPortClient
        {
            get
            {
                if (_doorControlServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_doorControlServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(DoorControlSchemaSet.GetInstance()),
                                                           };

                    InitServiceClient(_doorControlServiceHolder, controllers);
                }
                return _doorControlServiceHolder.Client;
            }
        }

        /*
        protected UserPortClient UserPortClient
        {
            get
            {
                if (_userServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_userServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(UsersSchemaSet.GetInstance()),
                                                           };

                    InitServiceClient(_userServiceHolder, controllers);

                }
                return _userServiceHolder.Client;
            }
        }*/

        protected PACSPortClient PACSPortClient
        {
            get
            {
                if (_pacsServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_pacsServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(AccessControlSchemaSet.GetInstance()),
                                                           };

                    InitServiceClient(_pacsServiceHolder, controllers);

                }
                return _pacsServiceHolder.Client;
            }
        }


        void InitServiceHolders()
        {
            // door control
            _doorControlServiceHolder = new ServiceHolder<DoorControlPortClient, DoorControlPort>(
                (features) => { return Client.GetServiceAddress(OnvifService.DOORCONTROL); },
                (binding, address) => { return new DoorControlPortClient(binding, address); },
                "Door Control");

            // user 
            //_userServiceHolder = new ServiceHolder<UserPortClient, UserPort>(
            //    (features) => { return Client.GetServiceAddress(OnvifService.USERSERVICE); },
            //    (binding, address) => { return new UserPortClient(binding, address); },
            //    "Users");
            
            // access control
            _pacsServiceHolder = new ServiceHolder<PACSPortClient, PACSPort>(
                (features) => { return Client.GetServiceAddress(OnvifService.ACCESSCONTROL); },
                (binding, address) => { return new PACSPortClient(binding, address); },
                "Access Control");


        }

        void InitServiceClient(ServiceHolder serviceHolder, IEnumerable<IChannelController> controllers)
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
                }, string.Format("Get {0} service address", serviceHolder.ServiceName));
                DoRequestDelay();
            }

            Assert(found,
                string.Format("{0} service address not found", serviceHolder.ServiceName),
                string.Format("Check that the DUT returned {0} service address", serviceHolder.ServiceName));

            EndpointController controller = new EndpointController(new EndpointAddress(serviceHolder.Address));

            List<IChannelController> ctrls = new List<IChannelController>();
            ctrls.Add(controller);
            ctrls.AddRange(controllers);

            Binding binding = CreateBinding(
                false,
                ctrls);

            serviceHolder.CreateClient(binding, AttachSecurity, SetupChannel);
        }

        void CloseClients()
        {
            foreach (ServiceHolder sh in new ServiceHolder[] { _doorControlServiceHolder, /*_userServiceHolder,*/ _pacsServiceHolder})
            {
                if (sh != null)
                {
                    sh.Close();
                }
            }
        }

        #endregion

        [Test(Name = "GET SERVICES – DOOR CONTROL SERVICE",
            Order = "01.01.25",
            Id = "1-1-25",
            Category = Category.DEVICE,
            Path = PATH_DEVICE,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesDoorControlServiceTest()
        {
            RunTest(() =>
            {
                CommonGetServicesTest(OnvifService.DOORCONTROL, "Door Control");
            });
        }

        /*[Test(Name = "GET SERVICES – USER SERVICE",
             Order = "01.01.20",
             Id = "1-1-20",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.UserService },
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]*/
        public void GetServicesUserServiceTest()
        {
            RunTest(() =>
            {
                CommonGetServicesTest(OnvifService.USERSERVICE, "User");
            });
        }

        [Test(Name = "GET SERVICES – ACCESS CONTROL SERVICE",
             Order = "01.01.24",
             Id = "1-1-24",
             Category = Category.DEVICE,
             Path = PATH_DEVICE,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetServices, Feature.AccessControlService },
             FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void GetServicesAccessControlServiceTest()
        {
            RunTest(() =>
            {
                CommonGetServicesTest(OnvifService.ACCESSCONTROL, "Access Control");
            });
        }

        #region Service capabilities

        [Test(Name = "DOOR CONTROL SERVICE CAPABILITIES",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorControlServiceCapabilities })]
        public void DoorControlServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Proxies.Onvif.DoorControlPortClient client = DoorControlPortClient;

                DoorControlServiceCapabilities capabilities = GetDoorControlCapabilities();

                Assert(capabilities.MaxLimit >= 0, 
                    string.Format("MaxLimit is {0}", capabilities.MaxLimit), 
                    "Check that MaxLimit is greater than zero");
    
            });
        }

        /*[Test(Name = "USER SERVICE CAPABILITIES",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.USER,
            Path = PATH_USERS,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.UserService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetUserServiceCapabilities })]*/
        /*public void UserServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                UserPortClient client = UserPortClient;

                UserServiceCapabilities capabilities = GetUserServiceCapabilities();
            });
        }*/

        [Test(Name = "ACCESS CONTROL SERVICE CAPABILITIES",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessControlServiceCapabilities })]
        public void AccessControlServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                PACSPortClient client = PACSPortClient;

                AccessControlServiceCapabilities capabilities = GetAccessControlCapabilities();
            });
        }

        #endregion

        #region Services and service capabilities

        [Test(Name = "GET SERVICES AND GET DOOR CONTROL SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorControlServiceCapabilities, Functionality.GetServices })]
        public void DoorControlServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {

                DoorControlPortClient client = DoorControlPortClient;

                XmlElement capabilitiesElement = GetServiceCapabilities(OnvifService.DOORCONTROL, "Door Control");

                DoorControlServiceCapabilities capabilities = GetDoorControlCapabilities();

                DoorControlServiceCapabilities serviceCapabilities = ParseDoorControlCapabilities(capabilitiesElement);

                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }

        /*[Test(Name = "GET SERVICES AND GET USER SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.USER,
            Path = PATH_USERS,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.UserService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetUserServiceCapabilities })]*/
        /*public void UserServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                XmlElement capabilitiesElement = GetServiceCapabilities(OnvifService.USERSERVICE, "User");

                UserServiceCapabilities capabilities = GetUserServiceCapabilities();

                UserServiceCapabilities serviceCapabilities = ParseUserServiceCapabilities(capabilitiesElement);

                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }*/

        [Test(Name = "GET SERVICES AND GET ACCESS CONTROL SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetAccessControlServiceCapabilities })]
        public void AccessControlServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                PACSPortClient client = PACSPortClient;

                XmlElement capabilitiesElement = GetServiceCapabilities(OnvifService.ACCESSCONTROL, "Access Control");

                AccessControlServiceCapabilities capabilities = GetAccessControlCapabilities();

                AccessControlServiceCapabilities serviceCapabilities = ParseAccessControlCapabilities(capabilitiesElement);

                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }

        #endregion

        #region Utils

        /// <summary>
        /// Common "GET SERVICES" test
        /// </summary>
        /// <param name="ns">Service namespace</param>
        /// <param name="name">Service name</param>
        void CommonGetServicesTest(string ns, string name)
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

            ValidateServiceCapabilitiesElement(service, name, ns, version);
        }
        
        /// <summary>
        /// Queries services and gets capabilities XML element for service defined by namespace
        /// </summary>
        /// <param name="ns">Service namespace</param>
        /// <param name="name">Service name</param>
        /// <returns></returns>
        XmlElement GetServiceCapabilities(string ns, string name)
        {
            Service[] services = GetServices(true);

            Service service = services.FindService(ns);

            Assert(service != null, 
                string.Format("No {0} service information returned", name), 
                string.Format("Check that the DUT returned {0} service information", name));

            string version = service.Version == null
             ? "missing"
             : string.Format("{0}.{1}", service.Version.Major, service.Version.Minor);

            Assert(service.Capabilities != null,
                string.Format("Capabilities are not included in entry for {0} service version {1}", name, version),
                "Check that the DUT returned Capabilities element");

            ValidateServiceCapabilitiesElement(service, name, ns, version);

            return service.Capabilities;
        }

        void ValidateServiceCapabilitiesElement(Service service, string name, string ns, string version)
        {
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

        void CompareCapabilities(DoorControlServiceCapabilities fromGetServices,
            DoorControlServiceCapabilities fromGetCapabilities)
        {
            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            //equal = TestUtils.CheckIntField(fromGetServices, fromGetCapabilities,
            //                                     C => C.MaxLimit, C => true, "MaxLimit", GETSERVICES,
            //                                     GETSERVICECAPABILITIES, dump);
            equal = TestUtils.CheckIntField(fromGetServices, fromGetCapabilities,
                                                 C => (int)C.MaxLimit, C => true, "MaxLimit", GETSERVICES,
                                                 GETSERVICECAPABILITIES, dump);          
            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();
        }

        /*
        void CompareCapabilities(UserServiceCapabilities fromGetServices,
            UserServiceCapabilities fromGetCapabilities)
        {
            // nothing to compare
            BeginStep(COMPARECAPABILITIESSTEP);

            StringBuilder dump = new StringBuilder();
            bool equal = true;


            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException(SETTINGSDONTMATCH);
            }

            StepPassed();
        }
        */

        /// <summary>
        /// Compares service capabilities
        /// </summary>
        /// <param name="fromGetServices">Structure received via GetServices</param>
        /// <param name="fromGetCapabilities">Structure received via GetServiceCapabilities</param>
        void CompareCapabilities(AccessControlServiceCapabilities fromGetServices,
            AccessControlServiceCapabilities fromGetCapabilities)
        {
            BeginStep(COMPARECAPABILITIESSTEP);
            
            StringBuilder dump = new StringBuilder();
            bool equal = true;

            //bool local = TestUtils.CheckIntField(fromGetServices, fromGetCapabilities,
            //                                     C => C.MaxLimit, C => true, "MaxLimit", GETSERVICES,
            //                                     GETSERVICECAPABILITIES, dump);
            bool local = TestUtils.CheckIntField(fromGetServices, fromGetCapabilities,
                                     C => (int)C.MaxLimit, C => true, "MaxLimit", GETSERVICES,
                                     GETSERVICECAPABILITIES, dump);
            equal = equal && local;

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
        

        #endregion

        #region Steps

        protected DoorControlServiceCapabilities ParseDoorControlCapabilities(XmlElement element)
        {
            return ExtractCapabilities<DoorControlServiceCapabilities>(element,
                                                                   OnvifService.DOORCONTROL);
        }

        /*
        protected UserServiceCapabilities ParseUserServiceCapabilities(XmlElement element)
        {
            return ExtractCapabilities<UserServiceCapabilities>(element,
                                                       OnvifService.USERSERVICE );
        }
        */

        protected AccessControlServiceCapabilities ParseAccessControlCapabilities(XmlElement element)
        {
            return ExtractCapabilities<AccessControlServiceCapabilities>(element,
                                                       OnvifService.ACCESSCONTROL);
        }


        protected DoorControlServiceCapabilities GetDoorControlCapabilities()
        {
            DoorControlPortClient client = DoorControlPortClient;
            DoorControlServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }

        /*
        protected UserServiceCapabilities GetUserServiceCapabilities()
        {
            UserPortClient client = UserPortClient;
            UserServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }
        */

        protected AccessControlServiceCapabilities GetAccessControlCapabilities()
        {
            PACSPortClient client = PACSPortClient;
            AccessControlServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }
        
        #endregion

    }
}
