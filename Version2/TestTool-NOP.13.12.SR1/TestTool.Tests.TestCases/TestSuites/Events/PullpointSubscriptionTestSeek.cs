///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using System.Xml;
using System.Collections.Generic;
using TestTool.Proxies.Event;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class PullpointSubscriptionTestSeek : EventTest
    {
        public PullpointSubscriptionTestSeek(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Event Handling\\Seek";
        private const string BeginPath = "tns1:EventBuffer/Begin";
        static private TopicInfo BeginTopic = CreateBeginTopic();

        [Test(Name = "SEEK EVENTS",
            Path = PATH,
            Order = "06.01.01",
            Id = "6-1-1",
            Category = Category.EVENT,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.PersistentNotificationStorage },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.PersistentNotificationStorage })]
        public void SeekForwardTest()
        {
            SubscriptionHandler Handler = new SubscriptionHandler(this, false);
            EventsVerifyPolicy policy = new EventsVerifyPolicy(false);
            policy.VerifyDataPresence = false;
            policy.VerifyMessagesPresence = false;
            Handler.SetPolicy(policy);
            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                DateTime BeginOfBufferTime = GetBeginTime(Handler);

                Handler.Subscribe(null, -1);
                DateTime MaxTime = DateTime.Now;
                Handler.GetProxy().Seek(BeginOfBufferTime.AddSeconds(-1).ToUniversalTime(), null);

                DateTime DeviceTime;
                DateTime DeviceTimeBack = DateTime.MinValue;
                Dictionary<NotificationMessageHolderType, XmlElement> NotificationMessages;

                Handler.GetMessages(1, out NotificationMessages);
                if (NotificationMessages.Count < 1)
                {
                    Assert(false, "No messages", "Check if there are messages");
                }
                DeviceTimeBack = GetMessageTime(GetFirstMessage(NotificationMessages));
                Assert(DeviceTimeBack >= BeginOfBufferTime, 
                    "Message time is less than BeginOfBuffer time",
                    "Check if message time is after BeginOfBuffer time");
                VerifyBeginType(NotificationMessages);

                do
                {
                    Handler.GetMessages(1, out NotificationMessages);
                    if (NotificationMessages.Count < 1)
                    {
                        break;
                    }
                    DeviceTime = GetMessageTime(GetFirstMessage(NotificationMessages));
                    if (DeviceTime < DeviceTimeBack)
                    {
                        Assert(false, 
                            "Message time is less than previous message time", 
                            "Check if message time increases");
                    }
                    DeviceTimeBack = DeviceTime;
                    if (DeviceTime >= MaxTime)
                    {
                        break;
                    }
                }
                while (true);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }

        [Test(Name = "SEEK EVENTS – REVERSE",
            Path = PATH,
            Order = "06.01.02",
            Id = "6-1-2",
            Category = Category.EVENT,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.PersistentNotificationStorage },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.PersistentNotificationStorage })]
        public void SeekBackwardTest()
        {
            SubscriptionHandler Handler = new SubscriptionHandler(this, false);
            EventsVerifyPolicy policy = new EventsVerifyPolicy(false);
            policy.VerifyDataPresence = false;
            policy.VerifyMessagesPresence = false;
            Handler.SetPolicy(policy);
            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                DateTime BeginOfBufferTime = GetBeginTime(Handler);

                Handler.Subscribe(null, -1);
                DateTime MaxTime = DateTime.Now;
                Handler.GetProxy().Seek(MaxTime.ToUniversalTime(), true);

                DateTime DeviceTime;
                DateTime DeviceTimeBack = DateTime.MinValue;
                Dictionary<NotificationMessageHolderType, XmlElement> NotificationMessages;

                Handler.GetMessages(1, out NotificationMessages);
                if (NotificationMessages.Count < 1)
                {
                    Assert(false, "No messages", "Check if there are messages");
                }
                DeviceTimeBack = GetMessageTime(GetFirstMessage(NotificationMessages));
                Assert(DeviceTimeBack >= BeginOfBufferTime,
                    "Message time is less than BeginOfBuffer time",
                    "Check if message time is after BeginOfBuffer time");

                while (!IsBeginMessage(NotificationMessages))
                {
                    Handler.GetMessages(1, out NotificationMessages);
                    if (NotificationMessages.Count < 1)
                    {
                        break;
                    }
                    DeviceTime = GetMessageTime(GetFirstMessage(NotificationMessages));
                    if (DeviceTime > DeviceTimeBack)
                    {
                        Assert(false, 
                            "Message time is greater than previous message time", 
                            "Check if message time decreases");
                    }
                    DeviceTimeBack = DeviceTime;
                }

                if (NotificationMessages.Count < 1)
                {
                    Assert(false, 
                        "No messages", 
                        "Check if last message is " + BeginPath + " Message");
                }
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }
        [Test(Name = "SEEK EVENTS – BEGIN OF BUFFER",
            Path = PATH,
            Order = "06.01.03",
            Id = "6-1-3",
            Category = Category.EVENT,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.PersistentNotificationStorage },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.PersistentNotificationStorage })]
        public void SeekBufferBeginTest()
        {
            SubscriptionHandler Handler = new SubscriptionHandler(this, false);
            EventsVerifyPolicy policy = new EventsVerifyPolicy(false);
            policy.VerifyDataPresence = false;
            policy.VerifyMessagesPresence = false;
            Handler.SetPolicy(policy);
            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());

                TopicSetType topicSet = Handler.GetTopicSet();

                VerifySeekTopics(topicSet);

                SubscribeToBegin(Handler);
                DateTime RefPoint = DateTime.Now;
                Dictionary<NotificationMessageHolderType, XmlElement> NotificationMessages;
                Handler.GetProxy().Seek(RefPoint.ToUniversalTime(), true);
                Handler.GetMessages(1, out NotificationMessages);
                if (NotificationMessages.Count < 1)
                {
                    Assert(false, 
                        "No messages", 
                        "Check if there are messages");
                }
                VerifyBeginType(NotificationMessages);
                RefPoint = GetMessageTime(GetFirstMessage(NotificationMessages));

                Handler.GetMessages(1, out NotificationMessages);
                if (NotificationMessages.Count >= 1)
                {
                    Assert(false, 
                        "There was more than one Begin of Buffer event notification or another unexpected notification recieved. There should be no notifications in current response.", 
                        "Check if there are messages");
                }


                Handler.GetProxy().Seek(RefPoint.ToUniversalTime(), false);
                Handler.GetMessages(1, out NotificationMessages);
                if (NotificationMessages.Count < 1)
                {
                    Assert(false, 
                        "No messages", 
                        "Check if there are messages");
                }
                VerifyBeginType(NotificationMessages);
                RefPoint = GetMessageTime(GetFirstMessage(NotificationMessages));

                Handler.GetMessages(1, out NotificationMessages);
                if (NotificationMessages.Count >= 1)
                {
                    Assert(false,
                        "There was more than one Begin of Buffer event notification or another unexpected notification recieved. There should be no notifications in current response.",
                        "Check if there are messages");
                }

            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }

        NotificationMessageHolderType GetFirstMessage(Dictionary<NotificationMessageHolderType, XmlElement> NotificationMessages)
        {
            foreach (NotificationMessageHolderType l in NotificationMessages.Keys)
            {
                return l;
            }
            return null;
        }

        void VerifySeekTopics(TopicSetType topicSet)
        {
            TopicSetHelper Helper = new TopicSetHelper(this, topicSet, false);
            List<XmlElement> topicList = Helper.FindTopics();
            foreach (XmlElement topicElement in topicList)
            {
                if (!TopicInfo.TopicsMatch(TopicInfo.ConstructTopicInfo(topicElement), BeginTopic))
                {
                    continue;
                }
                XmlElement messageDescription = topicElement.GetMessageDescription();
                if (messageDescription != null)
                {
                    bool isProperty = false;
                    // check that it is a property
                    if (messageDescription.HasAttribute(OnvifMessage.ISPROPERTY))
                    {
                        isProperty = XmlConvert.ToBoolean(messageDescription.Attributes[OnvifMessage.ISPROPERTY].Value);
                    }
                    Assert(!isProperty, 
                        "Topic should not be property topic", 
                        "Check " + BeginPath + " topic type");
                    return;
                }
            }
            Assert(false, 
                "Topic missed", 
                "Check " + BeginPath + " topic present");
        }

        void SubscribeToBegin(SubscriptionHandler Handler)
        {
            FilterType Filter = new FilterType();
            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();
            // result is not used, but filterTopicElement is updating its namespaces
            string topicPath = TopicInfo.CreateTopicPath(filterTopicElement, BeginTopic);
            filterTopicElement.InnerText = BeginPath;
            Filter.Any = new XmlElement[] { filterTopicElement };

            Handler.Subscribe(Filter, -1);
        }

        DateTime GetBeginTime(SubscriptionHandler Handler)
        {
            SubscribeToBegin(Handler);
            Handler.GetProxy().Seek(DateTime.Now.ToUniversalTime(), true);
            Dictionary<NotificationMessageHolderType, XmlElement> NotificationMessages;
            Handler.GetMessages(1, out NotificationMessages);
            SubscriptionHandler.Unsubscribe(Handler);
            if (NotificationMessages.Count < 1)
            {
                Assert(false, 
                    "No messages", 
                    "Check if there are messages");
            }
            VerifyBeginType(NotificationMessages);
            return GetMessageTime(GetFirstMessage(NotificationMessages));
        }
        bool IsBeginMessage(Dictionary<NotificationMessageHolderType, XmlElement> NotificationMessages)
        {
            try
            {
                NotificationMessageHolderType Message = null;
                foreach (NotificationMessageHolderType l in NotificationMessages.Keys)
                {
                    Message = l;
                }
                TopicInfo Topic = TopicInfo.ExtractTopicInfoPACS(Message.Topic.Any[0].InnerText, NotificationMessages[Message]);
                bool match = TopicInfo.TopicsMatch(Topic, BeginTopic);
                return match;
            }
            catch (Exception)
            {
                return false;
            }
        }
        void VerifyBeginType(Dictionary<NotificationMessageHolderType, XmlElement> NotificationMessages)
        {
            bool match = false;
            NotificationMessageHolderType Message = null;
            try
            {
                foreach (NotificationMessageHolderType l in NotificationMessages.Keys)
                {
                    Message = l;
                }
                TopicInfo Topic = TopicInfo.ExtractTopicInfoPACS(Message.Topic.Any[0].InnerText, NotificationMessages[Message]);
                match = TopicInfo.TopicsMatch(Topic, BeginTopic);
            }
            catch (Exception)
            {
                Assert(false,
                    "Message topic undefind",
                    "Check if first message is " + BeginPath);
            }
            Assert(match,
                "Message topic is wrong (topic or namespace)",
                "Check if first message is " + BeginPath);
        }

        DateTime GetMessageTime(NotificationMessageHolderType Message)
        {
            string utcTimeValue = Message.Message.Attributes[OnvifMessage.UTCTIMEATTRIBUTE].Value;
            if (EventServiceUtils.IsValidXsdDateTime(utcTimeValue))
            {
                return DateTime.Parse(utcTimeValue);
            }
            else
            {
                return DateTime.MaxValue;
            }
        }
        static private TopicInfo CreateBeginTopic()
        {
            EventsTopicInfo BeginTopicInfo = new EventsTopicInfo();
            BeginTopicInfo.Topic = BeginPath;
            BeginTopicInfo.NamespacesDefinition = "tns1=http://www.onvif.org/ver10/topics";
            return TopicInfo.ConstructTopicInfo(BeginTopicInfo);
        }
    }
}
