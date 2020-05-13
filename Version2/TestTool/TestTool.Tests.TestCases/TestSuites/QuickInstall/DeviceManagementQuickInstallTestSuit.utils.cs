using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using TestTool.HttpTransport.Interfaces;
using TestTool.HttpTransport.Interfaces.Exceptions;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.TestSuites.Recording;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class DeviceManagementQuickInstallTestSuit
    {
        private OnvifServiceClient<EventPortType, EventPortTypeClient> m_EventServiceClient;
        OnvifServiceClient<EventPortType, EventPortTypeClient> IBaseOnvifService2<EventPortType, EventPortTypeClient>.ServiceClient        
        {
            get
            {
                if (!m_EventServiceClient.IsInitialized())
                {
                    m_EventServiceClient = new OnvifServiceClient<EventPortType, EventPortTypeClient>(this, "Event", feature => this.GetEventServiceAddress());
                    m_EventServiceClient.InitServiceClient(new [] { new SoapValidator(EventsSchemasSet.GetInstance()) });
                }

                return m_EventServiceClient;
            }
        }

        public BaseOnvifTest Test { get { return this; } }

        protected string GetEventServiceAddress()
        {
            string address = string.Empty;

            Service[] services = CommonMethodsProvider.GetServices(this, Client, false);
            Service service = services.FindService(OnvifService.EVENTS);
            if (service != null)
            {
                address = service.XAddr;
            }
            return address;            
        }


        protected XmlElement GetTopicElement(IEnumerable<XmlElement> topics, TopicInfo topicInfo)
        {
            // check if "our" topic is present
            XmlElement topicElement = null;
            foreach (XmlElement el in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(el);
                if (TopicInfo.TopicsMatch(info, topicInfo))
                {
                    topicElement = el;
                    break;
                }
            }
            return topicElement;
        }

        protected void FindTopics(XmlElement element, List<XmlElement> topics)
        {
            if (element.RepresentsTopic())
            {
                topics.Add(element);
            }

            // If not a topic - enumerate child elements.
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                }
                FindTopics(child, topics);
            }
        }

        #region Validating utils
        bool checkEventDescription(XmlElement eventNode, RecordingControlEventsTestSuite.RecordingControlEventDescription eventDescription, IXmlNamespaceResolver namespaceResolver, StringBuilder logger)
        {
            var descriptionNode = eventNode.GetMessageDescription();
            if (null == descriptionNode)
            {
                logger.AppendLine("MessageDescription element is absent.");
                return false;
            }

            var manager = new XmlNamespaceManager(eventNode.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            var isPropertyAttribute = descriptionNode.Attributes[OnvifMessage.ISPROPERTY];
            if (null == isPropertyAttribute)
            {
                logger.AppendLine("The 'IsProperty' attribute is absent.");
                return false;
            }

            var isPropertyValueXmlString = XmlConvert.ToString(eventDescription.isProperty);
            if (isPropertyAttribute.Value != isPropertyValueXmlString)
            {
                logger.AppendLine(string.Format("The 'IsProperty' attribute is incorrect. Expected: {0}. Actual: {1}",
                                                isPropertyValueXmlString,
                                                isPropertyAttribute.Value));
                return false;
            }

            bool flag = true;
            foreach (var itemDescription in eventDescription.itemDescriptions)
            {
                var path = itemDescription.Path.Split('/').Select(e => "tt:" + e).Aggregate("", (s, s1) => s + s1 + "/").Trim('/');
                var nodes = descriptionNode.SelectNodes(path, manager).OfType<XmlElement>();
                var itemNode = nodes.FirstOrDefault(e => null != e.Attributes[OnvifMessage.NAME]
                                                         && e.Attributes[OnvifMessage.NAME].Value == itemDescription.Name);
                if (null == itemNode)
                {
                    if (itemDescription.Mandatory)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("Mandatory element {0} of type '{1}' is absent", itemDescription.Name, itemDescription.Type));
                    }
                }
                else
                {
                    XmlAttribute type = itemNode.Attributes[OnvifMessage.TYPE];
                    if (type == null)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("'Type' attribute is missing for '{0}' simple item", itemDescription.Name));
                    }
                    else
                    {
                        string error = string.Empty;
                        if (!type.IsCorrectQName(itemDescription.Type, itemDescription.Namespace, namespaceResolver, ref error))
                        {
                            flag = false;
                            logger.AppendLine(string.Format("'Type' attribute is incorrect for '{0}' simple item: {1}", itemDescription.Name, error));
                        }
                    }
                }
            }

            return flag;
        }

        bool messageFilterBase(Proxies.Event.NotificationMessageHolderType message,
                               TopicInfo topicInfo,
                               string expectedPropertyOperation,
                               string validationScript)
        {
            if (!string.IsNullOrEmpty(expectedPropertyOperation))
            {
                if (!message.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    return false;

                XmlAttribute propertyOperationType = message.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                if (expectedPropertyOperation != propertyOperationType.Value)
                    return false;
            }


            var logger = new StringBuilder();
            string validationScriptPath = string.Format("TestTool.Tests.TestCases.TestSuites.QuickInstall.Scripts.{0}", validationScript);
            Assert(ValidationEngine.GetInstance().Validate(message,
                                                           topicInfo,
                                                           validationScriptPath,
                                                           logger),
                   logger.ToStringTrimNewLine(),
                   "Validate received notification(s)");

            return true;
        }
        #endregion

        #region Filter utils
        protected FilterInfo CreateFilter(TopicInfo topicInfo, XmlElement messageDescription)
        {
            FilterInfo filter = new FilterInfo();

            filter.Filter = CreateSubscriptionFilter(topicInfo);

            filter.MessageDescription = messageDescription;

            return filter;
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(TopicInfo topicInfo)
        {
            return CreateSubscriptionFilter(new TopicInfo[] { topicInfo });
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(IEnumerable<TopicInfo> topicInfos)
        {
            Proxies.Event.FilterType filter = new Proxies.Event.FilterType();

            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();

            string topicPath = string.Empty;
            foreach (TopicInfo topicInfo in topicInfos)
            {
                string topicExpression = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                if (string.IsNullOrEmpty(topicPath))
                {
                    topicPath = topicExpression;
                }
                else
                {
                    topicPath = string.Format("{0}|{1}", topicPath, topicExpression);
                }
            }

            filterTopicElement.InnerText = topicPath;

            filter.Any = new XmlElement[] { filterTopicElement };

            return filter;
        }
        #endregion

        protected User CreateUserA1(UserLevel lvl, DeviceServiceCapabilities serviceCapabilities = null)
        {
            if (null == serviceCapabilities)
                serviceCapabilities = GetServiceCapabilities();

            Assert(serviceCapabilities.Security.MaxUserNameLengthSpecified && serviceCapabilities.Security.MaxPasswordLengthSpecified,
                   "The DUT didn't send Security.MaxUserNameLength and/or Security.MaxPasswordLength capabilitie(s)",
                   "Checking service capabilities Security.MaxUserNameLength and Security.MaxPasswordLength are received");

            var users = GetUsers();

            var existedUser = users.FirstOrDefault(u => u.UserLevel == lvl);
            User user = null;

            while (true)
            {
                try
                {
                    if (null != existedUser)
                    {
                        var passwordLength = serviceCapabilities.Security.MaxPasswordLength;
                        user = new User() { Username = existedUser.Username, Password = Extensions.RandomString(passwordLength), UserLevel = lvl, Extension = null };
                        SetUser(new[] { user });
                    }
                    else
                    {
                        var usernameLength = serviceCapabilities.Security.MaxUserNameLength;
                        var userName = users.Select(u => u.Username).GetNonMatchingAlphabeticalString(usernameLength);
                        var passwordLength = serviceCapabilities.Security.MaxPasswordLength;
                        user = new User() { Username = userName, Password = Extensions.RandomString(passwordLength), UserLevel = lvl, Extension = null };
                        CreateUsers(new[] { user });
                    }
                    break;
                }
                catch (FaultException e)
                {
                    if (!e.IsValidOnvifFault("Sender/OperationProhibited/Password"))
                        //Break cycle if received fault is not "Sender/OperationProhibited/Password"
                        throw;
                    else
                    {
                        LogFault(e);
                        StepPassed();

                        StopRequested();
                    }
                }
            }

            return user;
        }

        protected void HelperTimeSynchronizationA6()
        {
            _security = Security.None;
            _username = _password = null;
            UpdateSecurity();

            SetNTP(new NTPInformation() { FromDHCP = true, NTPManual = null });

            var dateTime = GetSystemDateAndTime();
            SetSystemDateAndTime(new SystemDateTime() { DateTimeType = SetDateTimeType.NTP, 
                                                        DaylightSavings = dateTime.DaylightSavings, 
                                                        TimeZone = dateTime.TimeZone,
                                                        UTCDateTime = null });
        }

        protected void UpdateServiceAddress(SoapMessage<HelloType> msg)
        {
            if (!string.IsNullOrEmpty(msg.Object.XAddrs))
            {
                var newAddr = FirstNonLinkLocalIPv4AddressFromClientSubnetwork(msg.Object.XAddrs.Split(' ').Where(addr => addr.StartsWith("http:")));

                if (null == newAddr)
                    newAddr = FirstNonLinkLocalIPv4AddressFromClientSubnetwork(msg.Object.XAddrs.Split(' ').Where(addr => addr.StartsWith("https:")));

                RaiseNetworkSettingsChangedEvent(newAddr);
            }
        }

        protected string FirstNonLinkLocalIPv4AddressFromClientSubnetwork(IEnumerable<string> addresses)
        {
            foreach (var address in addresses)
            {
                var IPs = Dns.GetHostAddresses(new Uri(address).Host);
                foreach (var ipAddress in IPs)
                {
                    if (AddressFamily.InterNetwork == ipAddress.AddressFamily && !ipAddress.IsIPv4LinkLocal() && _nic.IP.IsInSameSubnet(ipAddress, _nic.IPv4Mask))
                        return address;
                }
            }

            return string.Empty;
        }

        protected bool HasNonLinkLocalIPv4Address(SoapMessage<HelloType> helloMsg)
        {
            if (null == helloMsg || null == helloMsg.Object)
                return false;

            try
            {
                var addresses = null == helloMsg.Object.XAddrs ? new string[0] : helloMsg.Object.XAddrs.Split(' ');
                if (_nic.IP.AddressFamily == AddressFamily.InterNetworkV6)
                    //IPv6: return first interface with IPv6
                    return addresses.Any(e => System.Net.Dns.GetHostAddresses(new Uri(e).Host).Any(ip => ip.AddressFamily == AddressFamily.InterNetworkV6));
                else
                    //IPv4: return first non-link local interface from the same subnet as ODTT client
                    return !string.IsNullOrEmpty(FirstNonLinkLocalIPv4AddressFromClientSubnetwork(addresses));
            }
            catch (Exception)
            { return false; }
        }

        protected SoapMessage<HelloType> ReceiveHelloWithNonLinkLocalIPv4AddressFromClientSubnetwork()
        {
            return ReceiveHelloMessage(false, true, () => {}, msg => HasNonLinkLocalIPv4Address(msg.ToSoapMessage<HelloType>()));
        }

        protected void assertScope(IEnumerable<string> scopes, string scope, bool present)
        {
            Assert(present ? scopes.Any(s => scope == s) : scopes.All(s => scope != s),
                   string.Format("There is {0} scope '{1}' in received scope's set", present ? "no" : "", scope),
                   string.Format("Checking received scopes contains {0} scope '{1}'", present ? "" : "no", scope));
        }

        protected void HardResetDevice()
        {
            SetSystemFactoryDefault(FactoryDefaultType.Hard);
            var r = ReceiveHelloWithNonLinkLocalIPv4AddressFromClientSubnetwork();

            var scopes = GetHelloScopes(r.Object);
            assertScope(scopes, "onvif://www.onvif.org/Profile/Q/FactoryDefault", true);
            assertScope(scopes, "onvif://www.onvif.org/Profile/Q/Operational", false);

            UpdateServiceAddress(r);

            _security = Security.None;
            _username = _password = null;
            UpdateSecurity();
        }

        protected ProbeMatchesType sendUnicastProbeRequest()
        {
            ProbeMatchesType r = null;

            var discoveryFinished = new EventWaitHandle(false, EventResetMode.AutoReset);
            var helloReceivedFlag = false;
            var discovery = new Discovery(_nic.IP);
            discovery.MessageSent += (sender, args) => LogRequest(args.Message);
            discovery.Discovered += (sender, args) =>
                                    {
                                        LogResponse(string.Join("", args.Message.Raw.Select(e => Convert.ToChar(e).ToString()).ToArray()));

                                        var response = args.Message.ToSoapMessage<ProbeMatchesType>();
                                        if (null != response)
                                        {
                                            r = response.Object;
                                            helloReceivedFlag = true;
                                        }
                                    };
            discovery.DiscoveryFinished += (sender, args) => discoveryFinished.Set();

            try
            {
                BeginStep("Sending Unicast Probe request");
                var address = System.Net.IPAddress.IsLoopback(_cameraIp) ? _nic.IP : _cameraIp;
                discovery.Probe(address, null, null);

                WaitForResponse(new WaitHandle[] { discoveryFinished });
            }
            catch (FaultException e)
            {
                LogFault(e);
            }
            finally
            {
                StepPassed();
            }

            return r;
        }

        protected IEnumerable<string> GetProbeScopes(IEnumerable<ProbeMatchType> probeResposne)
        {
            var emptyProbMatch = new ProbeMatchType[0];
            Func<IEnumerable<string>, IEnumerable<string>> scopesSplitter = (text) => null != text ? text.SelectMany(s => s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())) : new string[0];
            return (probeResposne ?? emptyProbMatch).SelectMany(e => null != e.Scopes ? scopesSplitter(e.Scopes.Text) : new string[0]);
        }

        protected IEnumerable<string> GetHelloScopes(HelloType helloResponse)
        {
            var emptyHelloMatch = new HelloType[0];
            Func<IEnumerable<string>, IEnumerable<string>> scopesSplitter = (text) => null != text ? text.SelectMany(s => s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())) : new string[0];
            return (helloResponse.Scopes != null ? scopesSplitter(helloResponse.Scopes.Text) : new string[0]);
        }

        protected ProbeMatchesType discoverDevice()
        {
            ProbeMatchesType r = null;
            var T = _rebootTimeout;

            var start = System.DateTime.Now;
            var end = start.AddMilliseconds(T);

            while (System.DateTime.Now <= end && (null == r || !GetProbeScopes(r.ProbeMatch).Contains("onvif://www.onvif.org/Profile/Q/Operational")))
            {
                r = sendUnicastProbeRequest();
            }

            var flag = false;
            var msg = string.Empty;
            if (null == r)
                msg = "The DUT did not send PROBE MATCH message within the timeout.";
            else if (!GetProbeScopes(r.ProbeMatch).Contains("onvif://www.onvif.org/Profile/Q/Operational"))
                msg = "The DUT sent PROBE MATCH message but it has no scope 'onvif://www.onvif.org/Profile/Q/Operational'.";
            else if (GetProbeScopes(r.ProbeMatch).Contains("onvif://www.onvif.org/Profile/Q/FactoryDefault"))
                msg = "The DUT sent PROBE MATCH message but it has scope 'onvif://www.onvif.org/Profile/Q/FactoryDefault'.";
            else
                flag = true;

            Assert(flag, msg, "Checking PROBE MATCH is received");

            return r;
        }

        protected void AssertUnauthorized(Action action, string commandTitle)
        {
            try
            {
                action();

                Assert(false,
                       string.Format("The command {0} is processed successfully by the DUT but HTTP 401 error(Unauthorized) is expected", commandTitle),
                       "Waiting for HTTP 401 error(Unauthorized)");
            }
            catch (AccessDeniedException)
            { StepPassed(); }
        }

        protected void UnauthorizedCommandSequence(DeviceServiceCapabilities serviceCapabilities)
        {
            AssertUnauthorized(() => SetScopes(new[] { "onvif://www.onvif.org/location/test" }), "SetScopes");

            AssertUnauthorized(() => SetDiscoveryMode(DiscoveryMode.Discoverable), "SetDiscoveryMode");

            if (null != serviceCapabilities && null != serviceCapabilities.Security && serviceCapabilities.Security.AccessPolicyConfigSpecified && serviceCapabilities.Security.AccessPolicyConfig)
                AssertUnauthorized(() => GetAccessPolicy(), "GetAccessPolicy");

            AssertUnauthorized(() => CreateUsers(new[] { new User() { Username = "Test", Password = "Test", UserLevel = UserLevel.Administrator, Extension = null } }), "CreateUsers");

            AssertUnauthorized(() => SetSystemDateAndTime(new SystemDateTime() { DateTimeType = SetDateTimeType.NTP, DaylightSavings = true, TimeZone = null }), "SetSystemDateAndTime");
        }

        protected void AssertUnauthorizedAndAuthorized(Action action, string commandTitle, User user)
        {
            _security = Security.None;
            _username = _password = null;
            UpdateSecurity();
            AssertUnauthorized(action, commandTitle);

            _security = Security.Digest;
            _username = user.Username;
            _password = user.Password;
            UpdateSecurity();

            action();
        }

        protected NetworkInterface DetectDUTCurrentNetworkInterface(IEnumerable<NetworkInterface> netwotkInterfaces)
        {
            Func<string, bool> goodAddr = addr =>
                                          {
                                              try
                                              {
                                                  if (System.Net.IPAddress.Parse(addr).Equals(_cameraIp))
                                                      return true;
                                              }
                                              catch (Exception)
                                              { }
                                              return false;
                                          };

            Func<NetworkInterface, bool> interfaceFilter = i =>
                                                           {
                                                               if (i.Enabled && null != i.IPv4)
                                                               {
                                                                   if (i.IPv4.Enabled && null != i.IPv4.Config)
                                                                   {
                                                                       if (i.IPv4.Config.DHCP)
                                                                       {
                                                                           return goodAddr(i.IPv4.Config.FromDHCP.Address);
                                                                       }
                                                                       else if (null != i.IPv4.Config.Manual && i.IPv4.Config.Manual.Any(e => goodAddr(e.Address)))
                                                                       {
                                                                           return true;
                                                                       }
                                                                       else if (null != i.IPv4.Config.LinkLocal)
                                                                           return goodAddr(i.IPv4.Config.LinkLocal.Address);
                                                                   }
                                                               }

                                                               return false;
                                                           };

            return netwotkInterfaces.FirstOrDefault(e => interfaceFilter(e));
        }
    }
}
