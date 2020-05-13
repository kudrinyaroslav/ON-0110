using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using TestTool.HttpTransport.Interfaces;
using TestTool.HttpTransport.Interfaces.Exceptions;
using TestTool.HttpTransport.Internals.Http;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.TestSuites.Recording;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using DateTime = TestTool.Proxies.Onvif.DateTime;
using TimeZone = TestTool.Proxies.Onvif.TimeZone;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.TestSuites
{

    [TestClass]
    partial class DeviceManagementQuickInstallTestSuit : DeviceDiscoveryTest, IEventService
    {
        //private const string ROOT_PATH = "Device Management\\Quick Install";
        private const string ROOT_PATH = "Quick Install";
        private const string GENERAL_PATH = ROOT_PATH + "\\General";
        private const string DEFAULT_ACCESS_POLICY_PATH = ROOT_PATH + "\\Default access policy";
        private const string MONITORING_EVENTS_PATH = ROOT_PATH + "\\Monitoring Events";
        private const string SYSTEM_PATH = ROOT_PATH + "\\System";

        public DeviceManagementQuickInstallTestSuit(TestLaunchParam param) : base(param)
        {
        }

        #region 1-1-*
        [Test(Name = "Factory Default state verification",
            Id = "1-1-1",
            Category = Category.QUICK_INSTALL,
            Path = GENERAL_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.DefaultAccessPolicy, Feature.Digest, Feature.ZeroConfiguration, Feature.MaximumUsernameLength, Feature.MaximumPasswordLength, Feature.ProfileQSupported },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemFactoryDefault, Functionality.CreateUsers, Functionality.SetUser, Functionality.FactoryDefaultStateIsSignaledByTheScope,
                                                           Functionality.AnonymousAccessInFactoryDefaultState, Functionality.UserConfigurationInFactoryDefaultState,
                                                           Functionality.DynamicIPConfigurationEnabledInFactoryDefaultState, Functionality.IPv4DHCPEnabledInFactoryDefaultState,
                                                           Functionality.OperationalStateIsSignaledByTheScope})]
        public void SetSystemFactoryDefaultAndCreateUserCombinationVerificationTest()
        {
            RunTest(() =>
                    {
                        HardResetDevice();

                        HelperTimeSynchronizationA6();

                        var serviceCapabilities = this.GetServiceCapabilities();

                        var netwotkInterfaces = GetNetworkInterfaces();
                        var currentNetworkInterface = DetectDUTCurrentNetworkInterface(netwotkInterfaces);

                        Assert(null != currentNetworkInterface,
                               "There is no settings for network interface using to connect to the DUT",
                               "Checking current network interface settings are returned");

                        Assert(null != currentNetworkInterface.IPv4.Config.DHCP && null != currentNetworkInterface.IPv4 && currentNetworkInterface.IPv4.Config.DHCP,
                               string.Format("DHCP for network interface with token '{0}' is not turned on", currentNetworkInterface.token ?? string.Empty),
                               "Checking network settings");

                        if (null != serviceCapabilities.Network && serviceCapabilities.Network.IPVersion6Specified && serviceCapabilities.Network.IPVersion6)
                        {
                            Assert(null != currentNetworkInterface.IPv6 && currentNetworkInterface.IPv6.Enabled,
                                   "IPv6 isn't turned ON in current network interface settings",
                                   "Checking IPv6 is turned ON in current network interface settings");
                            
                            Assert(null != currentNetworkInterface.IPv6 && null != currentNetworkInterface.IPv6.Config &&
                                   null != currentNetworkInterface.IPv6.Config.LinkLocal &&
                                   currentNetworkInterface.IPv6.Config.LinkLocal.Any(addr => !string.IsNullOrEmpty(addr.Address)),
                                   "Current network interface settings has no IPv6 LinkLocal addresses",
                                   "Checking current network interface settings has at least one IPv6 LinkLocal address");
                        }

                        Assert(GetZeroConfiguration().Enabled, 
                               "The zero-configuration is not enabled", 
                               "Checking zero-configuration");

                        var scopes = GetScopes().Select(e => e.ScopeItem);
                        assertScope(scopes, "onvif://www.onvif.org/Profile/Q/FactoryDefault", true);
                        assertScope(scopes, "onvif://www.onvif.org/Profile/Q/Operational", false);


                        var probe = sendUnicastProbeRequest();
                        Assert(null != probe,
                               "The DUT did not send PROBE MATCH message.",
                               "Checking PROBE MATCH is received");
                        scopes = GetProbeScopes(probe.ProbeMatch);

                        assertScope(scopes, "onvif://www.onvif.org/Profile/Q/FactoryDefault", true);
                        assertScope(scopes, "onvif://www.onvif.org/Profile/Q/Operational", false);


                        User user = CreateUserA1(UserLevel.Administrator);
                       
                        _security = Security.Digest;
                        _username = user.Username;
                        _password = user.Password;
                        UpdateSecurity(true);

                        discoverDevice();


                        IEnumerable<User> users = null;
                        AssertUnauthorizedAndAuthorized(() => users = GetUsers(), "GetUsers", user);

                        var adminUser = users.FirstOrDefault(u => u.Username == user.Username);
                        Assert(null != adminUser && UserLevel.Administrator == adminUser.UserLevel, 
                               null != adminUser 
                               ?
                               string.Format("There is a user with name '{0}' on the received user list but it has UserLevel = '{1}' instead of expected '{2}'", user.Username, adminUser.UserLevel, UserLevel.Administrator)
                               :
                               string.Format("There is no user with name '{0}' on the received user list", user.Username), 
                               "Checking user with Administrator-level is added to user list correctly");


                        AssertUnauthorizedAndAuthorized(() => scopes = GetScopes().Select(e => e.ScopeItem), "GetScopes", user);

                        assertScope(scopes, "onvif://www.onvif.org/Profile/Q/FactoryDefault", false);
                        assertScope(scopes, "onvif://www.onvif.org/Profile/Q/Operational", true);
                });
        }

        //[Test(Name = "Scope verification",
        //    Id = "1-1-3",
        //    Category = Category.QUICK_INSTALL,
        //    Path = GENERAL_PATH,
        //    Version = 1.0,
        //    RequiredFeatures = new Feature[] { Feature.GetServices, Feature.DefaultAccessPolicy, Feature.Digest },
        //    RequirementLevel = RequirementLevel.Optional,
        //    FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemFactoryDefault, Functionality.CreateUsers, Functionality.GetScopes })]
        //public void ScopeVerificationTest()
        //{
        //    RunTest(() =>
        //            {
        //                HardResetDevice();

        //                var netwotkInterfaces = GetNetworkInterfaces();
        //                var currentNetworkInterface = DetectDUTCurrentNetworkInterface(netwotkInterfaces);

        //                Assert(null != currentNetworkInterface,
        //                       "There is no settings for network interface using to connect to the DUT",
        //                       "Checking current network interface settings are returned");

        //                Assert(null != currentNetworkInterface.IPv4.Config.DHCP && null != currentNetworkInterface.IPv4 && currentNetworkInterface.IPv4.Config.DHCP,
        //                       string.Format("DHCP for network interface with token '{0}' is not turned on", currentNetworkInterface.token ?? string.Empty),
        //                       "Checking network settings");

        //                var scopes = GetScopes().Select(e => e.ScopeItem);
        //                Action<string, bool> assertScope = (s, present) => Assert(present ? scopes.Any(scope => scope == s) : scopes.All(scope => scope != s),
        //                                                                          string.Format("There is no scope '{0}' in received scope's set", s),
        //                                                                          string.Format("Checking received scopes contains scope '{0}'", s));

        //                assertScope("onvif://www.onvif.org/Profile/Q/FactoryDefault", true);
        //                assertScope("onvif://www.onvif.org/Profile/Q/Operational", false);

        //                var probe = sendUnicastProbeRequest();
        //                scopes = probe.ProbeMatch.SelectMany(e => e.Scopes.Text);

        //                assertScope("onvif://www.onvif.org/Profile/Q/FactoryDefault", true);
        //                assertScope("onvif://www.onvif.org/Profile/Q/Operational", false);

                        
        //                var users = GetUsers();
        //                var userName = users.Select(u => u.Username).GetNonMatchingAlphabeticalString(10);
            
        //                var user = new User() { Username = userName, Password = "1234567890", UserLevel = UserLevel.Administrator, Extension = null };
        //                CreateUsers(new [] { user });

        //                RunStep(() => Sleep(5000), "5 seconds timeout after CreateUsers");

        //                discoverDevice();

        //                AssertUnauthorized(() => GetScopes(), "GetScopes");

        //                _security = Security.Digest;
        //                _username = user.Username;
        //                _password = user.Password;
        //                UpdateSecurity(true);

                       
        //                scopes = GetScopes().Select(e => e.ScopeItem);

        //                assertScope("onvif://www.onvif.org/Profile/Q/FactoryDefault", false);
        //                assertScope("onvif://www.onvif.org/Profile/Q/Operational", true);

                        
        //                probe = sendUnicastProbeRequest();
        //                scopes = probe.ProbeMatch.SelectMany(e => e.Scopes.Text);

        //                assertScope("onvif://www.onvif.org/Profile/Q/FactoryDefault", false);
        //                assertScope("onvif://www.onvif.org/Profile/Q/Operational", true);
        //            });
        //}
        #endregion

        #region 2-1-*
        [Test(Name = "Default access policy - Anonymous",
            Id = "2-1-1",
            Category = Category.QUICK_INSTALL,
            Path = DEFAULT_ACCESS_POLICY_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.DefaultAccessPolicy, Feature.Digest, Feature.ProfileQSupported },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetDeviceServiceCapabilities, Functionality.GetHostname, Functionality.GetSystemDateAndTime, Functionality.DefaultAccessPolicy })]
        public void DefaultAccessPolicyAnonymousTest()
        {
            RunTest(() =>
                    {
                        //RestoreDefaultAccessPolicyA2();

                        _security = Security.None;
                        _username = _password = null;
                        UpdateSecurity();

                        GetServices(false);
                        GetServiceCapabilities();
                        GetHostname();
                        GetSystemDateAndTime();
                    });
        }

        [Test(Name = "Default access policy - User",
            Id = "2-1-2",
            Category = Category.QUICK_INSTALL,
            Path = DEFAULT_ACCESS_POLICY_PATH,
            Version = 1.0,
            RequiredFeatures = new [] { Feature.GetServices, Feature.DefaultAccessPolicy, Feature.Digest, Feature.MaximumUsernameLength, Feature.MaximumPasswordLength, Feature.ProfileQSupported },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNTP, Functionality.GetNetworkInterfaces, Functionality.GetScopes, Functionality.GetDiscoveryMode, Functionality.GetEventProperties, Functionality.DefaultAccessPolicy })]
        public void DefaultAccessPolicyUserTest()
        {
            RunTest(() =>
                    {
                        //RestoreDefaultAccessPolicyA2();

                        var user = CreateUserA1(UserLevel.User);

                        _security = Security.Digest;
                        _username = user.Username;
                        _password = user.Password;
                        UpdateSecurity();

                        if (Features.ContainsFeature(Feature.NTP))
                        {
                            AssertUnauthorizedAndAuthorized(() => GetNTP(), "GetNTP", user);
                        }

                        AssertUnauthorizedAndAuthorized(() => GetNetworkInterfaces(), "GetNetworkInterfaces", user);
                        AssertUnauthorizedAndAuthorized(() => GetScopes(), "GetScopes", user);
                        AssertUnauthorizedAndAuthorized(() => GetDiscoveryMode(), "GetDiscoveryMode", user);

                        AssertUnauthorizedAndAuthorized(() => this.GetTopicSet(), "GetEventProperties", user);
                    });
        }

        //[Test(Name = "Default access policy - Operator",
        //    Id = "2-1-3",
        //    Category = Category.QUICK_INSTALL,
        //    Path = DEFAULT_ACCESS_POLICY_PATH,
        //    Version = 1.0,
        //    RequiredFeatures = new [] { Feature.GetServices, Feature.DefaultAccessPolicy, Feature.Digest, Feature.MaximumUsernameLength, Feature.MaximumPasswordLength },
        //    RequirementLevel = RequirementLevel.Optional,
        //    LastChangedIn = "v14.12",
        //    FunctionalityUnderTest = new Functionality[] { Functionality.Reboot, Functionality.DefaultAccessPolicy })]
        public void DefaultAccessPolicyOperatorTest()
        {
            RunTest(() =>
                    {
                        //RestoreDefaultAccessPolicyA2();

                        var user = CreateUserA1(UserLevel.Operator);

                        AssertUnauthorizedAndAuthorized(() =>
                                                        {
                                                            Reboot();

                                                            ReceiveHelloWithNonLinkLocalIPv4AddressFromClientSubnetwork();
                                                        }, 
                                                        "SystemReboot", 
                                                        user);
                    });
        }

        [Test(Name = "Default access policy - Administrator and Anonymous",
            Id = "2-1-4",
            Category = Category.QUICK_INSTALL,
            Path = DEFAULT_ACCESS_POLICY_PATH,
            Version = 1.0,
            RequiredFeatures = new [] { Feature.GetServices, Feature.DefaultAccessPolicy, Feature.Digest, Feature.ProfileQSupported },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetScopes, Functionality.SetDiscoveryMode, Functionality.GetAccessPolicy, Functionality.CreateUsers, Functionality.SetSystemDateAndTime, Functionality.DefaultAccessPolicy })]
        public void DefaultAccessPolicyAdministratorAndAnonymousTest()
        {
            RunTest(() =>
                    {
                        //RestoreDefaultAccessPolicyA2();
                        var serviceCapabilities = GetServiceCapabilities();

                        _security = Security.None;
                        _username = _password = null;
                        UpdateSecurity();

                        UnauthorizedCommandSequence(serviceCapabilities);
                    });
        }

        [Test(Name = "Default access policy - Administrator and User/Operator",
            Id = "2-1-5",
            Category = Category.QUICK_INSTALL,
            Path = DEFAULT_ACCESS_POLICY_PATH,
            Version = 1.0,
            RequiredFeatures = new [] { Feature.GetServices, Feature.DefaultAccessPolicy, Feature.Digest, Feature.MaximumUsernameLength, Feature.MaximumPasswordLength, Feature.ProfileQSupported },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetScopes, Functionality.SetDiscoveryMode, Functionality.GetAccessPolicy, Functionality.CreateUsers, Functionality.SetSystemDateAndTime, Functionality.DefaultAccessPolicy })]
        public void DefaultAccessPolicyAdministratorAndUserOrOperatorTest()
        {
            RunTest(() =>
                    {
                        //var admin = RestoreDefaultAccessPolicyA2();
                        var adminUser     = _username;
                        var adminPassword = _password;

                        var serviceCapabilities = GetServiceCapabilities();


                        var user = CreateUserA1(UserLevel.User, serviceCapabilities);

                        _security = Security.Digest;
                        _username = user.Username;
                        _password = user.Password;
                        UpdateSecurity();

                        UnauthorizedCommandSequence(serviceCapabilities);

                        _security = Security.Digest;
                        _username = adminUser;
                        _password = adminPassword;
                        UpdateSecurity();
                       
                        user = CreateUserA1(UserLevel.Operator, serviceCapabilities);

                        _security = Security.Digest;
                        _username = user.Username;
                        _password = user.Password;
                        UpdateSecurity();

                        UnauthorizedCommandSequence(serviceCapabilities);
                    });
        }
        #endregion

        #region 3-1-*
        [Test(Name = "Processor Usage event",
            Id = "3-1-1",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringProcessorUsageEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringProcessorUsageEvent })]
        public void ProcessorUsageEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Monitoring/ProcessorUsage";
                        
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Source/SimpleItemDescription", "Token", "ReferenceToken", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Value", "float", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = CreateFilter(topicInfo, eventNode);

                        var timeout = _operationDelay/1000;
                        subscription.Subscribe(filter.Filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                                               { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.ProcessorUsageEvent.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Last Reset event",
            Id = "3-1-2",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringOperatingTimeLastResetEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringOperatingTimeLastResetEvent })]
        public void OperatingTimeLastLastRebootEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Monitoring/OperatingTime/LastReset";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Status", "dateTime", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = CreateFilter(topicInfo, eventNode);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter.Filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.OperatingTime.LastReset.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Last Reboot event",
            Id = "3-1-3",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringOperatingTimeLastRebootEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringOperatingTimeLastRebootEvent })]
        public void OperatingTimeLastResetEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Monitoring/OperatingTime/LastReboot";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Checking that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Status", "dateTime", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = CreateFilter(topicInfo, eventNode);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter.Filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.OperatingTime.LastReboot.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Last Reboot event (Status Change)",
            Id = "3-1-4",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringOperatingTimeLastRebootEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringOperatingTimeLastRebootEvent })]
        public void OperatingTimeLastRebootEventStatusChangeTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        const string eventTopic = "tns1:Monitoring/OperatingTime/LastReboot";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 1;
                        var filter = CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.OperatingTime.LastReboot.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");

                        var local = subscription;

                        //Don't try to delete second time in case of fail.
                        subscription = null;
                        SubscriptionHandler.Unsubscribe(local);

                        //use the same object
                        subscription = local;

                        Func<Proxies.Event.NotificationMessageHolderType, System.DateTime> statusExtractor = (msg) =>
                            {
                                var items = msg.Message.GetMessageDataSimpleItems();

                                if (null != items && items.ContainsKey("Status"))
                                    return System.DateTime.Parse(items["Status"]);

                                return System.DateTime.MinValue;
                            };

                        var firstStatus = statusExtractor(notifications.First().Key);

                        Reboot();
                        ReceiveHelloWithNonLinkLocalIPv4AddressFromClientSubnetwork();

                        subscription.Subscribe(filter, -1);

                        pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.OperatingTime.LastReboot.xq") };

                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");

                        var secondStatus = statusExtractor(notifications.First().Key);

                        Assert(secondStatus > firstStatus, "Field 'Status' in second notification is less or equal than in the first one", "Check field 'Status' of received notifications");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Last Clock Synchronization event",
            Id = "3-1-5",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringOperatingTimeLastClockSynchronizationEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringOperatingTimeLastClockSynchronizationEvent })]
        public void OperatingTimeLastClockSynchronizationEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Monitoring/OperatingTime/LastClockSynchronization";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Checking that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Status", "dateTime", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = CreateFilter(topicInfo, eventNode);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter.Filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.OperatingTime.LastClockSynchronization.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Last Clock Synchronization change event (SetSystemDateAndTime)",
            Id = "3-1-6",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringOperatingTimeLastClockSynchronizationEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringOperatingTimeLastClockSynchronizationEvent })]
        public void OperatingTimeLastClockSynchronizationEventStatusChangeSetSystemDateAndTimeTest()
        {
            SubscriptionHandler subscription = null;

            SystemDateTime initialSystemDateTime = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var localInitialSystemDateTime = GetSystemDateAndTime();

                        const string eventTopic = "tns1:Monitoring/OperatingTime/LastClockSynchronization";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        Func<Proxies.Event.NotificationMessageHolderType, System.DateTime> statusExtractor = (msg) =>
                        {
                            var items = msg.Message.GetMessageDataSimpleItems();

                            if (null != items && items.ContainsKey("Status"))
                                return System.DateTime.Parse(items["Status"]);

                            return System.DateTime.MinValue;
                        };

                        const int messageLimit = 1;
                        var filter = CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        {
                            Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.OperatingTime.LastClockSynchronization.xq")
                        };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");

                        var initialClockSynchronization = statusExtractor(notifications.First().Key).ToUniversalTime();

                        Sleep(1000);

                        SetSystemDateAndTime(new SystemDateTime(){ DateTimeType = SetDateTimeType.Manual, 
                                                                   DaylightSavings = false, 
                                                                   UTCDateTime = new DateTime(){ Date = new Date() { Day = System.DateTime.UtcNow.Day,
                                                                                                                     Month = System.DateTime.UtcNow.Month,
                                                                                                                     Year = System.DateTime.UtcNow.Year },
                                                                                                 Time = new Time() { Second = System.DateTime.UtcNow.Second,
                                                                                                                     Minute = System.DateTime.UtcNow.Minute,
                                                                                                                     Hour = System.DateTime.UtcNow.Hour }
                                                                                               }
                                                                 });

                        initialSystemDateTime = localInitialSystemDateTime;

                        pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        {
                            Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.CHANGED, "Monitoring.OperatingTime.LastClockSynchronization.xq")
                        };

                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");

                        var lastClockSynchronization = statusExtractor(notifications.First().Key).ToUniversalTime();
                        Assert(lastClockSynchronization > initialClockSynchronization,
                               "The value of 'Status' field in the last notification is less or equal than in the first one", 
                               "Check field 'Status' of received notifications");
                    },
                    () =>
                    {
                        if (null != initialSystemDateTime)
                            SetSystemDateAndTime(initialSystemDateTime);
                        
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Last Clock Synchronization change event (NTP message)",
            Id = "3-1-7",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringOperatingTimeLastClockSynchronizationEvent, Feature.NTP, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringOperatingTimeLastClockSynchronizationEvent })]
        public void OperatingTimeLastClockSynchronizationEventStatusChangeNTPMessageTest()
        {
            SubscriptionHandler subscription = null;

            SystemDateTime initialSystemDateTime = null;
            NTPInformation initialNTPStatus = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var localInitialSystemDateTime = GetSystemDateAndTime();

                        const string eventTopic = "tns1:Monitoring/OperatingTime/LastClockSynchronization";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        Func<Proxies.Event.NotificationMessageHolderType, System.DateTime> statusExtractor = (msg) =>
                        {
                            var items = msg.Message.GetMessageDataSimpleItems();

                            if (null != items && items.ContainsKey("Status"))
                                return System.DateTime.Parse(items["Status"]);

                            return System.DateTime.MinValue;
                        };

                        var localIinitialNTPStatus = GetNTP();

                        if (SetDateTimeType.NTP == localInitialSystemDateTime.DateTimeType)
                        {
                            SetSystemDateAndTime(new SystemDateTime() { DateTimeType = SetDateTimeType.Manual,
                                                                        DaylightSavings = false,
                                                                        UTCDateTime = new DateTime() { Date = new Date()
                                                                                                              {
                                                                                                                  Day = System.DateTime.UtcNow.Day,
                                                                                                                  Month = System.DateTime.UtcNow.Month,
                                                                                                                  Year = System.DateTime.UtcNow.Year
                                                                                                              },
                                                                                                              Time = new Time()
                                                                                                              {
                                                                                                                  Second = System.DateTime.UtcNow.Second,
                                                                                                                  Minute = System.DateTime.UtcNow.Minute,
                                                                                                                  Hour = System.DateTime.UtcNow.Hour
                                                                                                              }
                                                                                                     }
                                                                      });
                        }

                        var testInformation = new NTPInformation
                                              {
                                                  FromDHCP = false,
                                                  NTPManual = new [] { new NetworkHost() { Type = NetworkHostType.IPv4, IPv4Address = _environmentSettings.NtpIpv4 } }
                                              };

                        SetNTP(testInformation);

                        initialSystemDateTime = localInitialSystemDateTime;
                        initialNTPStatus = localIinitialNTPStatus;

                        const int messageLimit = 1;
                        var filter = CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.OperatingTime.LastClockSynchronization.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");

                        var initialClockSynchronization = statusExtractor(notifications.First().Key).ToUniversalTime();

                        Sleep(1000);

                        SetSystemDateAndTime(new SystemDateTime() { DaylightSavings = false, DateTimeType = SetDateTimeType.NTP, TimeZone = new TimeZone() { TZ = "PST8PDT,M3.2.0,M11.1.0" }, UTCDateTime = null });

                        pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.CHANGED, "Monitoring.OperatingTime.LastClockSynchronization.xq") };

                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");

                        var lastClockSynchronization = statusExtractor(notifications.First().Key).ToUniversalTime();

                        Assert(lastClockSynchronization > initialClockSynchronization,
                               "Field 'Status' in the last notification is less or equal than in the first one",
                               "Checking field 'Status' of received notification");
                    },
                    () =>
                    {
                        if (null != initialSystemDateTime)
                            SetSystemDateAndTime(initialSystemDateTime);

                        if (null != initialNTPStatus)
                            SetNTP(initialNTPStatus);

                        if (null != subscription)
                            SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Last Backup event",
            Id = "3-1-8",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.MonitoringBackupLastEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.MonitoringBackupLastEvent })]
        public void LastBackupEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Monitoring/Backup/Last";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Checking that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Status", "dateTime", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Monitoring.Backup.Last.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Fan Failure event",
            Id = "3-1-9",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.DeviceHardwareFailureFanFailureEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.DeviceHardwareFailureFanFailureEvent })]
        public void FanFailedEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Device/HardwareFailure/FanFailure";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Checking that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Source/SimpleItemDescription", "Token", "ReferenceToken", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Failed", "boolean", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Device.HardwareFailure.FanFailure.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }
        
        [Test(Name = "Power Supply Failure event",
            Id = "3-1-10",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.DeviceHardwareFailurePowerSupplyFailureEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.DeviceHardwareFailurePowerSupplyFailureEvent })]
        public void PowerSupplyFailedEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Device/HardwareFailure/PowerSupplyFailure";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Checking that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Source/SimpleItemDescription", "Token", "ReferenceToken", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Failed", "boolean", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Device.HardwareFailure.PowerSupplyFailure.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Storage Failure event",
            Id = "3-1-11",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.DeviceHardwareFailureStorageFailureEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.DeviceHardwareFailureStorageFailureEvent })]
        public void StorageFailedEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Device/HardwareFailure/StorageFailure";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Checking that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Source/SimpleItemDescription", "Token", "ReferenceToken", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Failed", "boolean", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Device.HardwareFailure.StorageFailure.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "Critical Temperature event",
            Id = "3-1-12",
            Category = Category.QUICK_INSTALL,
            Path = MONITORING_EVENTS_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.DeviceHardwareFailureTemperatureCriticalEvent, Feature.GetServices },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices, Functionality.GetEventProperties, Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.DeviceHardwareFailureTemperatureCriticalEvent })]
        public void CriticalTemperatureEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Device/HardwareFailure/TemperatureCritical";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Checking that event with topic {0} is present", eventTopic));


                        var eventDescription = new RecordingControlEventsTestSuite.RecordingControlEventDescription() { isProperty = true };
                        eventDescription.addItemDescription(new RecordingControlEventsTestSuite.EventItemDescription("Data/SimpleItemDescription", "Critical", "boolean", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                        { Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "Device.HardwareFailure.TemperatureCritical.xq") };

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Checking that required notification is received");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }
        #endregion

        #region 4-1-*
        [Test(Name = "Firmware Upload",
            Id = "4-1-1",
            Category = Category.QUICK_INSTALL,
            Path = SYSTEM_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.HttpFirmwareUpgrade, Feature.ProfileQSupported },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.StartFirmwareUgrade })]
        public void FirmwareUploadTest()
        {
            RunTest(() =>
                    {
                        var adminUser = new User() { UserLevel = UserLevel.Administrator, Username = _username, Password = _password };
                        AssertUnauthorizedAndAuthorized(() => GetUsers(), "GetUsers", adminUser);


                        var uploadDelay = string.Empty;
                        var expectedDownTime = string.Empty;
                        var uploadURI = StartFirmwareUpgrade(out uploadDelay, out expectedDownTime);

                        var downTime = XmlConvert.ToTimeSpan(uploadDelay);
                        RunStep(() => Sleep((int)downTime.TotalMilliseconds), 
                                string.Format("{0} seconds timeout after StartFirmwareUpgrade", (int)downTime.TotalSeconds));

                        HttpWebResponse httpResponse = null;
                        RunStep(() =>
                                {
                                    var sender = new DigestAuthFixer(_username, _password);
                                    httpResponse = sender.GrabResponse(uploadURI, "application/octet-stream", File.ReadAllBytes(FirmwareFilePath), OperationDelay);
                                }, 
                                "Invoke HTTP POST request on returned URI");

                        Assert(httpResponse.StatusCode == HttpStatusCode.OK, 
                               "HTTP Status is not '200 OK'", 
                               "Check HTTP status code", 
                               string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));

                        if (Features.ContainsFeature(Feature.BYE))
                        {
                            var bye = ReceiveByeMessage(false, true, () => {});

                            var endpointReferencePresent = null != bye.Object && null != bye.Object.EndpointReference && null != bye.Object.EndpointReference.Address;
                            var flag = endpointReferencePresent && bye.Object.EndpointReference.Address.Value == _cameraId;
                            Assert(flag,
                                   endpointReferencePresent 
                                   ? 
                                   string.Format("The endpoint reference in BYE message has unexpected value '{0}'. Expected: '{1}'", bye.Object.EndpointReference.Address.Value, _cameraId)
                                   :
                                   "The received BYE message has no 'EndpointReference' item",
                                   "Check endpoint reference");
                        }

                        var hello = ReceiveHelloWithNonLinkLocalIPv4AddressFromClientSubnetwork();

                        UpdateServiceAddress(hello);

                        AssertUnauthorizedAndAuthorized(() => GetUsers(), "GetUsers", adminUser);
                    });
        }

        [Test(Name = "Invalid Firmware Upload",
            Id = "4-1-2",
            Category = Category.QUICK_INSTALL,
            Path = SYSTEM_PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.HttpFirmwareUpgrade },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.StartFirmwareUgrade })]
        public void InvalidFirmwareUploadTest()
        {
            RunTest(() =>
                    {
                        var uploadDelay = string.Empty;
                        var expectedDownTime = string.Empty;
                        var uploadURI = StartFirmwareUpgrade(out uploadDelay, out expectedDownTime);

                        var downTime = XmlConvert.ToTimeSpan(uploadDelay);
                        RunStep(() => Sleep((int)downTime.TotalMilliseconds), 
                                string.Format("{0} seconds timeout after StartFirmwareUpgrade", (int)downTime.TotalSeconds));

                        HttpWebResponse httpResponse = null;

                        try
                        {
                            RunStep(() =>
                            {
                                var sender = new DigestAuthFixer(_username, _password);
                                httpResponse = sender.GrabResponse(uploadURI, "application/octet-stream", new byte[] { 1, 2, 3 });
                            },
                            string.Format("Invoke HTTP POST request on URI '{0}'", uploadURI));
                        }
                        catch (WebException e)
                        {
                            if (null == (e.Response as HttpWebResponse))
                            {
                                StepFailed(e);
                                return;
                            }

                            StepPassed();

                            httpResponse = (HttpWebResponse)e.Response;
                        }
                        catch (Exception e)
                        {
                            StepFailed(e);
                            return;
                        }

                        Assert(httpResponse.StatusCode == HttpStatusCode.UnsupportedMediaType,
                               "HTTP Status is not '415 Unsupported Media Type'",
                               "Check HTTP status code",
                               string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));
                    });
        }
        #endregion
    }
}
