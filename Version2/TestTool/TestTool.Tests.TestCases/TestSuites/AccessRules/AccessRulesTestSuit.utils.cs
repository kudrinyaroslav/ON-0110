using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.TestSuites.AccessRules
{
    partial class AccessRulesTestSuit
    {
        public static bool equalAccessRulesCapabilities(AccessRulesServiceCapabilities fromGetServiceCapabilities,
                                                        AccessRulesServiceCapabilities fromGetServices,
                                                        StringBuilder logger)
        {
            bool flag = true;

            const string msgHeader = "Value of '{0}' field is inconsistent. GetServiceCapabilities: '{1}'. GetServices: '{2}'";

            if (fromGetServiceCapabilities.MaxLimit != fromGetServices.MaxLimit)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxLimit", fromGetServiceCapabilities.MaxLimit, fromGetServices.MaxLimit));
            }

            if (fromGetServiceCapabilities.MaxAccessProfiles != fromGetServices.MaxAccessProfiles)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxAccessProfiles", fromGetServiceCapabilities.MaxAccessProfiles, fromGetServices.MaxAccessProfiles));
            }

            if (fromGetServiceCapabilities.MaxAccessPoliciesPerAccessProfile != fromGetServices.MaxAccessPoliciesPerAccessProfile)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxAccessPoliciesPerAccessProfile", fromGetServiceCapabilities.MaxAccessPoliciesPerAccessProfile, fromGetServices.MaxAccessPoliciesPerAccessProfile));
            }

            if (fromGetServiceCapabilities.MultipleSchedulesPerAccessPointSupported != fromGetServices.MultipleSchedulesPerAccessPointSupported)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MultipleSchedulesPerAccessPointSupported", fromGetServiceCapabilities.MultipleSchedulesPerAccessPointSupported, fromGetServices.MultipleSchedulesPerAccessPointSupported));
            }

            return flag;
        }

        public static bool equalAccessProfileInfo(AccessProfileInfo fromGetAccessProfileInfoList,
                                                  AccessProfileInfo fromGetAccessProfileInfo,
                                                  StringBuilder logger,
                                                  string headerFirst = "GetAccessProfileInfoList",
                                                  string headerSecond = "GetAccessProfileInfo")
        {
            bool flag = true;

            var msgHeader = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'.", headerFirst, headerSecond);

            if (fromGetAccessProfileInfoList.token != fromGetAccessProfileInfo.token)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "token", fromGetAccessProfileInfoList.token, fromGetAccessProfileInfo.token));
            }

            if (fromGetAccessProfileInfoList.Name != fromGetAccessProfileInfo.Name)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Name", fromGetAccessProfileInfoList.Name, fromGetAccessProfileInfo.Name));
            }

            if (fromGetAccessProfileInfoList.Description != fromGetAccessProfileInfo.Description)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Description", fromGetAccessProfileInfoList.Description, fromGetAccessProfileInfo.Description));
            }

            return flag;
        }

        public static bool equalAccessProfile(AccessProfile fromGetAccessProfileList,
                                              AccessProfile fromGetAccessProfiles,
                                              StringBuilder logger,
                                              string headerFirst = "GetAccessProfileList",
                                              string headerSecond = "GetAccessProfiles")
        {
            return equalAccessProfileInfo(fromGetAccessProfileList, fromGetAccessProfiles, logger, headerFirst, headerSecond) 
                && equalAccessPolicyList(fromGetAccessProfileList.AccessPolicy, fromGetAccessProfiles.AccessPolicy, logger, headerFirst, headerSecond);
        }

        public static bool equalAccessPolicyList(IEnumerable<AccessPolicy> fromGetAccessProfileList,
                                                 IEnumerable<AccessPolicy> fromGetAccessProfiles,
                                                 StringBuilder logger,
                                                 string headerFirst = "GetAccessProfileList",
                                                 string headerSecond = "GetAccessProfiles")
        {
            if (null == fromGetAccessProfileList)
                fromGetAccessProfileList = new AccessPolicy[0];
            if (null == fromGetAccessProfiles)
                fromGetAccessProfiles = new AccessPolicy[0];

            if (fromGetAccessProfileList.Count() != fromGetAccessProfiles.Count())
            {
                logger.AppendLine(string.Format("AccessPolicy item's list sent in {0} command has different size than the one received via {1}", headerFirst, headerSecond));
                return false;
            }

            var msgHeader = string.Format("Value of field '{{0}}' of AccessPolicy item with ScheduleToken = '{{1}}' is inconsistent. {0}: '{{2}}'. {1}: '{{3}}'.",
                                          headerFirst, headerSecond);

            bool flag = true;
            foreach (var accessPolicy in fromGetAccessProfileList)
            {
                var twins = fromGetAccessProfiles.Where(e => e.ScheduleToken == accessPolicy.ScheduleToken);
                if (!twins.Any())
                {
                    flag = false;
                    logger.AppendLine(string.Format("AccessPolicy item with ScheduleToken = '{0}' sent in {1} command have no corresponding item(s) in the list received via {2}", 
                                                    accessPolicy.ScheduleToken, headerFirst, headerSecond));
                }
                else
                {
                    var twin = twins.FirstOrDefault(e => e.Entity == accessPolicy.Entity);
                    if (null == twin)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("AccessPolicy item with ScheduleToken = '{0}' and Entity = '{1}' sent in {2} command have no corresponding item in the list received via {3}",
                                                        accessPolicy.ScheduleToken, accessPolicy.Entity, headerFirst, headerSecond));
                    }
                    else
                    {
                        if (accessPolicy.Entity != twin.Entity)
                        {
                            flag = false;
                            logger.AppendLine(string.Format(msgHeader, "Entity", accessPolicy.ScheduleToken, accessPolicy.Entity, twin.Entity));
                        }

                        var expectedET = new XmlQualifiedName("AccessPointInfo", "http://www.onvif.org/ver10/accesscontrol/wsdl");
                        var equalFlag = accessPolicy.EntityType == twin.EntityType
                                        ||
                                        (null == accessPolicy.EntityType || accessPolicy.EntityType.IsEmpty) && expectedET == twin.EntityType
                                        ||
                                        (null == twin.EntityType || twin.EntityType.IsEmpty) && expectedET == accessPolicy.EntityType;
                        if (!equalFlag)
                        {
                            flag = false;
                            logger.AppendLine(string.Format(msgHeader, "EntityType", accessPolicy.ScheduleToken, accessPolicy.EntityType, twin.EntityType));
                        }
                    }
                }
            }

            return flag;
        }

        public static bool validateListFromGetAccessProfileInfo(IEnumerable<AccessProfileInfo> fromGetAccessProfileInfo,
                                                                IEnumerable<string> tokens,
                                                                StringBuilder logger)
        {
            var notRequested = fromGetAccessProfileInfo.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of Access ProfileInfo items received through GetAccessProfileInfo contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + string.Format(", '{0}'", e.token)).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetAccessProfileInfo.Where(e => fromGetAccessProfileInfo.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of Access ProfileInfo items received through GetAccessProfileInfo contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetAccessProfileInfo.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of Access ProfileInfo items received through GetAccessProfileInfo doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }

        public static bool validateListFromGetAccessProfiles(IEnumerable<AccessProfile> fromGetAccessProfiles,
                                                             IEnumerable<string> tokens,
                                                             StringBuilder logger)
        {
            var notRequested = fromGetAccessProfiles.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of Access Profile items received through GetAccessProfiles contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + string.Format(", '{0}'", e.token)).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetAccessProfiles.Where(e => fromGetAccessProfiles.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of Access Profile items received through GetAccessProfiles contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetAccessProfiles.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of Access Profile items received through GetAccessProfiles doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }

        public List<AccessProfileInfo> receiveAndValidateAccessProfileInfoList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of Access ProfileInfo items";
            var msgHeader = "The received list of Access ProfileInfo items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullAccessProfileInfoList = new List<AccessProfileInfo>();

            do
            {
                AccessProfileInfo[] accessProfileInfoList1;
                nextStartRef = this.GetAccessProfileInfoList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out accessProfileInfoList1);

                Assert(accessProfileInfoList1.Count() <= limit,
                       string.Format(msgHeader, accessProfileInfoList1.Count(), limit),
                       stepTitle);

                fullAccessProfileInfoList.AddRange(accessProfileInfoList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullAccessProfileInfoList.Select(e => e.token);
            var duplicates = token.Where(t => fullAccessProfileInfoList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of Access ProfileInfo items received through GetAccessProfileInfoList contains items with the following tokens more than once: {0}", 
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of Access ProfileInfo items");

            return fullAccessProfileInfoList;
        }

        public List<AccessProfile> receiveAndValidateAccessProfileList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of Access Profile items";
            var msgHeader = "The received list of Access Profile items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullAccessProfileList = new List<AccessProfile>();

            do
            {
                AccessProfile[] accessProfileList1;
                nextStartRef = this.GetAccessProfileList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out accessProfileList1);

                Assert(accessProfileList1.Count() <= limit,
                       string.Format(msgHeader, accessProfileList1.Count(), limit),
                       stepTitle);

                fullAccessProfileList.AddRange(accessProfileList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullAccessProfileList.Select(e => e.token);
            var duplicates = token.Where(t => fullAccessProfileList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of Access Profile items received through GetAccessProfileList contains items with the following tokens more than once: {0}", 
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of Access Profile items");

            return fullAccessProfileList;
        }

        public bool equalAccessProfileInfoLists(IEnumerable<AccessProfileInfo> accessProfileInfoFirst,
                                                IEnumerable<AccessProfileInfo> accessProfileInfoSecond)
        {
            return accessProfileInfoFirst.Select(e => e.token).OrderBy(e => e).SequenceEqual(accessProfileInfoSecond.Select(e => e.token).OrderBy(e => e));
        }

        public static bool validateListFromGetAccessProfileAndGetAccessProfilesConsistency(IEnumerable<AccessProfileInfo> fromGetAccessProfileInfo,
                                                                                           IEnumerable<AccessProfile> fromGetAccessProfiles,
                                                                                           StringBuilder logger)
        {
            if (!fromGetAccessProfileInfo.Select(e => e.token).OrderBy(e => e).SequenceEqual(fromGetAccessProfiles.Select(e => e.token).OrderBy(e => e)))
            {
                logger.AppendLine("The DUT returned list of Access Profile items inconsistent with list of Access ProfileInfo items");

                return false;
            }

            bool flag = true;
            foreach (var accessProfileInfo in fromGetAccessProfileInfo)
            {
                var n = fromGetAccessProfiles.Count(e => e.token == accessProfileInfo.token);
                if (1 != n)
                {
                    logger.AppendLine(string.Format("The DUT returned {0} Access Profile item{1} for Access ProfileInfo item with token '{2}'",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    accessProfileInfo.token));

                    flag = false;
                }
                else
                {
                    var ap = fromGetAccessProfiles.FirstOrDefault(e => e.token == accessProfileInfo.token);

                    var internalLogger = new StringBuilder();
                    if (!equalAccessProfileInfo(accessProfileInfo, ap, internalLogger))
                    {
                        flag = false;
                        logger.AppendLine(string.Format("Access ProfileInfo item with token '{0}' is inconsistent with Access Profile item for the same token", accessProfileInfo.token));
                        logger.Append(internalLogger);
                    }
                }
            }

            return flag;
        }

        bool messageFilterBase(Proxies.Event.NotificationMessageHolderType message,
                               TopicInfo topicInfo,
                               string expectedPropertyOperation,
                               string validationScript,
                               IEnumerable<AccessProfile> accessProfileList)
        {
            if (!string.IsNullOrEmpty(expectedPropertyOperation))
            {
                if (!message.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    return true;

                XmlAttribute propertyOperationType = message.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                if (expectedPropertyOperation != propertyOperationType.Value)
                    return false;
            }

            var variables = new Dictionary<string, string>();
            variables.Add("accessProfileList", string.Format(@"{{{0}}}", string.Join(",", accessProfileList.Select(s => string.Format(@"""{0}"" : ""{1}""", s.token, s.Description)))));


            var logger = new StringBuilder();
            string validationScriptPath = string.Format("TestTool.Tests.TestCases.TestSuites.AccessRules.Script.{0}", validationScript);
            Assert(ValidationEngine.GetInstance().Validate(message,
                                                           topicInfo,
                                                           validationScriptPath,
                                                           logger,
                                                           variables),
                   logger.ToStringTrimNewLine(),
                   "Validate received notification(s)");

            return true;
        }
        bool messageFilterBase (Proxies.Event.NotificationMessageHolderType msg, TopicInfo topicInfo, string  expectedPropertyOperation, string expectedAccessProfileToken, Func<StringBuilder, bool> action)
        {
            var invalidFlag = false;
            var log = new StringBuilder();
            try
            {
                if (!EventServiceExtensions.NotificationTopicMatch(msg, topicInfo))
                {
                    log.AppendLine(string.Format("Received notification has unexpected topic '{0}' while the expected one should have with topic '{1}'", 
                                                 EventServiceExtensions.GetTopicInfo(msg).GetDescription(), topicInfo.GetDescription()));
                    invalidFlag = true;
                }

                if (null != expectedPropertyOperation)
                {
                    if (!msg.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    {
                        log.AppendLine("Received notification has no 'PropertyOperation' field");
                        invalidFlag = true;
                    }
                    else
                    {
                        XmlAttribute propertyOperationType = msg.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                        //Skip non-changed messages
                        if (propertyOperationType.Value != expectedPropertyOperation)
                        {
                            log.AppendLine(string.Format("Received notification has 'PropertyOperation' field with value = '{0}' but value = '{1}' is expected", propertyOperationType.Value, expectedPropertyOperation));
                            invalidFlag = true;
                        }
                    }
                }

                var source = msg.Message.GetMessageSourceSimpleItems();
                if (!source.ContainsKey("AccessProfileToken"))
                {
                    log.AppendLine("Received notification has no Source/SimpleItem with Name = 'AccessProfileToken'");
                    invalidFlag = true;
                }
                else
                {
                    var token = source["AccessProfileToken"];
                    if (token != expectedAccessProfileToken)
                    {
                        log.AppendLine(string.Format("Received notification has Source/SimpleItem with Name = 'AccessProfileToken' and Value = '{0}' but notification for AccessProfile item with token = '{1}' is expected", token, expectedAccessProfileToken));
                        invalidFlag = true;
                    }
                }

                if (null != action && action(log))
                    invalidFlag = true;

                return true;
            }
            finally
            {
                Assert(!invalidFlag, log.ToStringTrimNewLine(), "Validation of received notification");
            }
        }

        #region Validate Consistency
        public void ValidateConsistency(AccessProfile initial,
                                        AccessProfile received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The AccessProfile item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalAccessProfile(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no AccessProfileInfo item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received AccessProfile item");
        }

        public void ValidateConsistency(AccessProfile initial,
                                        IEnumerable<AccessProfile> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The AccessProfile item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalAccessProfile(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received AccessProfile list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received AccessProfile item");
        }

        public void ValidateConsistency(AccessProfileInfo initial,
                                        AccessProfileInfo received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The AccessProfileInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalAccessProfileInfo(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no AccessProfileInfo item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received AccessProfileInfo item");
        }

        public void ValidateConsistency(AccessProfileInfo initial,
                                        IEnumerable<AccessProfileInfo> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The AccessProfileInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalAccessProfileInfo(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received AccessProfileInfo list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received AccessProfileInfo item");
        }
        #endregion

        #region Notification validation
        protected bool ValidateNotifications(Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement> messages, TopicInfo topic, string accessProfileToken, StringBuilder logger)
        {
            var filtered = messages.Where(pair => EventServiceExtensions.NotificationTopicMatch(pair.Key, pair.Value, topic)).Select(k => k.Key);

            if (!filtered.Any())
            {
                logger.AppendLine(string.Format("There is no notification with topic '{0}'", topic.GetDescription()));
                return false;
            }

            foreach (var msg in filtered)
            {
                var source = msg.Message.GetMessageSourceSimpleItems();

                if (source.Any(s => s.Key == "AccessProfileToken" && s.Value == accessProfileToken))
                    return true;
            }

            logger.AppendLine(string.Format("There is no notification containing 'Source/SimpleItem' element with Name = 'AccessProfileToken' and Value = '{0}'", accessProfileToken));
            return false;
        }
        #endregion
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

        public void CheckRequestedInfo<T>(IEnumerable<T> list,
            string token, string itemName, Func<T, string> tokenSelector)
        {
            string error = string.Empty;

            int count = 0;
            if (list != null)
            {
                count = list.Count();
            }

            if (count == 0)
            {
                error = string.Format("No {0} information returned", itemName);
            }
            else
            {
                if (count > 1)
                {
                    error = "More than one entry returned";
                }
                else
                {
                    T item = list.First();
                    if (tokenSelector(item) != token)
                    {
                        error = "Entry for other token returned";
                    }
                }
            }
            Assert(string.IsNullOrEmpty(error), error, "Check response");

        }
    }
}
