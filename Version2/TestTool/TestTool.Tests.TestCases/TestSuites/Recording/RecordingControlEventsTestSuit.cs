using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml.Serialization;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using Event = TestTool.Proxies.Event;
using TestTool.Tests.Definitions.Exceptions;
using System.Xml;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.TestSuites.Events;

namespace TestTool.Tests.TestCases.TestSuites.Recording
{
    [TestClass]
    partial class RecordingControlEventsTestSuite : PacsEventsTestSuite
    {
        private const string ACTIVE = "Active";
        private const string IDLE = "Idle";
        private const string CONNECTED = "Connected";

        private const string TNS1NAMESPACE = "http://www.onvif.org/ver10/topics";

        const string STATESIMPLEITEM = "State";  // Recording Control Specification, p. 5.21.1
        const string NEWSTATESIMPLEITEM = "NewState";
        const string RECORDINGJOBTOKENSIMPLEITEM = "RecordingJobToken";
        const string RECEIVERTOKENSIMPLEITEM = "ReceiverToken";

        private string _retentionTime = "PT0S";

        private delegate void ActionRef(ref string token1, string token2, string token3);

        RTSPSimulator _simulator = null;

        public RecordingControlEventsTestSuite(TestLaunchParam param): base(param)
        {
            if (!string.IsNullOrEmpty(param.RetentionTime))
                _retentionTime = param.RetentionTime;
        }

        protected override void Release()
        {
            if (null != _recordingServiceHolder)
                _recordingServiceHolder.Close();

            if (null != _deviceClient)
                _deviceClient.Close();

            if (_simulator != null)
            {
                StopSimulator(_simulator);
            }
            base.Release();
        }

        private const string PATH_GENERAL = "Recording Control\\Events";


        [Test(Name = "RECORDING CONTROL – JOB STATE EVENT",
            Order = "05.01.18",
            Id = "5-1-18",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.MediaOrReceiver, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] {Functionality.GetRecordingJobState, Functionality.RecordingJobStateChangeEvent })]
        public void jobStateEventTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string recordingToken = string.Empty;
            string profileToken = string.Empty;
            string jobToken = string.Empty;
            RecordingJobConfiguration config = null;
            bool recordingCreated = false;
            List<string> lstJobToken = null;

            RunTest(() =>
            {
                Assert(null != recordingPortClient,
                        "Can't connect to Recording service",
                        "Check that Recording service is accessible");

                GetRecordingJobsResponseItem[] jobs = GetRecordingJobs();
                if (jobs == null || jobs.Length == 0)
                {
                    if (Features.ContainsFeature(Feature.ReceiverService))
                    {
                        // A.12
                        GetRecordingForJobCreation(out recordingToken, out recordingCreated, 0);

                        // pass test if there is no recording token
                        if (string.IsNullOrEmpty(recordingToken))
                            return;

                        config = new RecordingJobConfiguration();
                        config.Mode = "Idle";
                        config.Priority = 1;
                        config.RecordingToken = recordingToken;
                        RecordingJobSource source = new RecordingJobSource();
                        source.AutoCreateReceiverSpecified = true;
                        source.AutoCreateReceiver = true;

                        config.Source = new RecordingJobSource[] { source };
                    }
                    else
                    {
                        // A.15
                        GetRecordingForJobCreationMediaProfile(out recordingToken, out profileToken, out recordingCreated, 0);

                        // pass test if there is no recording token and profile token
                        if (string.IsNullOrEmpty(recordingToken) && string.IsNullOrEmpty(profileToken))
                            return;

                        config = new RecordingJobConfiguration();
                        config.Mode = "Idle";
                        config.Priority = 1;
                        config.RecordingToken = recordingToken;
                        RecordingJobSource source = new RecordingJobSource();
                        source.AutoCreateReceiver = false;
                        source.SourceToken = new SourceReference();
                        source.SourceToken.Type = "http://www.onvif.org/ver10/schema/Profile";
                        source.SourceToken.Token = profileToken;

                        config.Source = new RecordingJobSource[] { source };
                    }

                    if (config != null)
                        jobToken = CreateRecordingJob(ref config);

                    lstJobToken = new List<string>();
                    lstJobToken.Add(jobToken);
                }
                else
                {
                    lstJobToken =  new List<string>();

                    for (int i = 0; i < jobs.Length; i++)
                        lstJobToken.Add(jobs[i].JobToken);
                }

                //Step 12.
                var topicSet = GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                Assert(topics.Any(),
                        "The DUT provides no topics.",
                        "Check that the DUT provides event's topics.");

                const string eventTopic = "tns1:RecordingConfig/JobState";
                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                        string.Format("Event with topic {0} is not supported", eventTopic),
                        string.Format("Check that event with topic {0} is present", eventTopic));

                var eventDescription = new RecordingControlEventDescription(){ isProperty = true };
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingJobToken", "RecordingJobReference", OnvifMessage.ONVIF));
                eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "State", "string", XSNAMESPACE));
                eventDescription.addItemDescription(new EventItemDescription("Data/ElementItemDescription", "Information", "RecordingJobStateInformation", OnvifMessage.ONVIF));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger), 
                        logger.ToString(), 
                        string.Format("Checking description of event with topic {0}", eventTopic));

                const int messageLimit = 2;

                var filter = CreateSubscriptionFilter(topicInfo);

                subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                subscriptionHandler.Subscribe(filter, subscriptionTimeout);

                //Pull until notifications for all recording jobs are received
                var pullingCondition = new WaitNotificationsForAllJobsPollingCondition(pullingTimeout, lstJobToken);
                Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                LogTestEvent(string.Format("Waiting for messages with PropertyOperation='{0}'...{1}", OnvifMessage.INITIALIZED, Environment.NewLine));

                Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                        pullingCondition.Reason,
                        "Checking that all required notifications are received");

                var notificationDescription = new RecordingControlNotificationDescription();
                notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingJobToken", "RecordingJobReference", OnvifMessage.ONVIF) { AllowedValues = lstJobToken });

                notificationDescription.addItemDescription(new NotificationItemDescription("Data/SimpleItem", "State", "string", XSNAMESPACE));
                notificationDescription.addItemDescription(new NotificationItemDescription("Data/ElementItem", "Information", "RecordingJobStateInformation", OnvifMessage.ONVIF));

                ValidateNotificationMessages(messages, topicInfo, notificationDescription, OnvifMessage.INITIALIZED);

                var information = GetRecordingJobStateInformationFromMessages(messages.Select(e => e.Key.Message));

                foreach (var recordingJob in information)
                {
                    var jobState = GetRecordingJobState(recordingJob.Key);

                    CompareRecordingJobStates(recordingJob.Key, recordingJob.Value, jobState);
                }
            },
        () =>
            {
                SubscriptionHandler.Unsubscribe(subscriptionHandler);

                if (!string.IsNullOrEmpty(jobToken))
                    DeleteRecordingJob(jobToken);

                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                    DeleteRecording(recordingToken);
            });
        }

        [Test(Name = "RECORDING CONTROL – JOB STATE CHANGE EVENT",
            Order = "05.01.19",
            Id = "5-1-19",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.MediaOrReceiver, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingJobState, Functionality.RecordingJobStateChangeEvent })]
        public void jobStateChangeEventTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string recordingToken = string.Empty;
            string profileToken = string.Empty;
            string jobToken = string.Empty;
            string jobModeInitial = string.Empty;

            RecordingJobConfiguration jobConfig = null;

            bool recordingCreated = false;
            bool jobCreated = false;

            List<string> lstJobToken = null;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        GetRecordingJobsResponseItem[] jobs = GetRecordingJobs();
                        if (jobs == null || jobs.Length == 0)
                        {
                            if (Features.ContainsFeature(Feature.ReceiverService))
                            {
                                // A.12
                                GetRecordingForJobCreation(out recordingToken, out recordingCreated, 0);

                                // pass test if there is no recording token
                                if (string.IsNullOrEmpty(recordingToken))
                                    return;

                                jobConfig = new RecordingJobConfiguration();
                                jobConfig.Mode = "Idle";
                                jobConfig.Priority = 1;
                                jobConfig.RecordingToken = recordingToken;
                                RecordingJobSource source = new RecordingJobSource();
                                source.AutoCreateReceiverSpecified = true;
                                source.AutoCreateReceiver = true;

                                jobConfig.Source = new RecordingJobSource[] { source };
                            }
                            else
                            {
                                // A.15
                                GetRecordingForJobCreationMediaProfile(out recordingToken, out profileToken, out recordingCreated, 0);

                                // pass test if there is no recording token and profile token
                                if (string.IsNullOrEmpty(recordingToken) && string.IsNullOrEmpty(profileToken))
                                    return;

                                jobConfig = new RecordingJobConfiguration();
                                jobConfig.Mode = "Idle";
                                jobConfig.Priority = 1;
                                jobConfig.RecordingToken = recordingToken;
                                RecordingJobSource source = new RecordingJobSource();
                                source.AutoCreateReceiver = false;
                                source.SourceToken = new SourceReference();
                                source.SourceToken.Type = "http://www.onvif.org/ver10/schema/Profile";
                                source.SourceToken.Token = profileToken;

                                jobConfig.Source = new RecordingJobSource[] { source };
                            }

                            if (jobConfig != null)
                            {
                                jobToken = CreateRecordingJob(ref jobConfig);
                                jobCreated = true;
                                lstJobToken = new List<string>();
                                lstJobToken.Add(jobToken);
                            }
                        }
                        else
                        {
                            jobToken = jobs[0].JobToken;
                            jobConfig = jobs[0].JobConfiguration;

                            lstJobToken = new List<string>();
                            for (int i = 0; i < jobs.Length; i++)
                                lstJobToken.Add(jobs[i].JobToken);
                        }

                        const string eventTopic = "tns1:RecordingConfig/JobState";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 2;
                        System.DateTime localSubscribeStarted = System.DateTime.MaxValue;

                        System.DateTime TerminationExpectedTime = System.DateTime.Now;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);


                        //Pull until notifications for all recording jobs are received
                        var pullingCondition = new WaitNotificationsForAllJobsPollingCondition(pullingTimeout, new[] { jobToken });
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        LogTestEvent(string.Format("Waiting for messages with PropertyOperation='{0}'...{1}", OnvifMessage.INITIALIZED, Environment.NewLine));

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var jobMode = (IDLE == jobConfig.Mode) ? ACTIVE : IDLE;
                        SetRecordingJobMode(jobToken, jobMode);
                        jobModeInitial = jobConfig.Mode;

                        pullingCondition = new WaitNotificationsForAllJobsPollingCondition(pullingTimeout, new[] { jobToken });

                        LogTestEvent(string.Format("Waiting for messages with PropertyOperation='{0}'...{1}", OnvifMessage.CHANGED, Environment.NewLine));

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        //Analize only notifications for selected recording job
                        Func<XmlElement, bool> filterPredicate = msg => jobToken == GetAttributeValueOfItemFromMessage(msg, "Source", "SimpleItem", "RecordingJobToken");

                        var filteredMessages = messages.Where(e => filterPredicate(e.Key.Message)).ToDictionary(e => e.Key, e => e.Value);

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingJobToken", "RecordingJobReference", OnvifMessage.ONVIF) { AllowedValues = lstJobToken });
                        notificationDescription.addItemDescription(new NotificationItemDescription("Data/SimpleItem", "State", "string", XSNAMESPACE));
                        notificationDescription.addItemDescription(new NotificationItemDescription("Data/ElementItem", "Information", "RecordingJobStateInformation", OnvifMessage.ONVIF));

                        ValidateNotificationMessages(filteredMessages, topicInfo, notificationDescription, OnvifMessage.CHANGED);


                        {
                            var jobStateFromNotification = GetElementDataItemFromMessage<RecordingJobStateInformation>(filteredMessages.Last().Key.Message, "Information");
                            var jobState = GetRecordingJobState(jobToken);

                            CompareRecordingJobStates(jobToken, jobStateFromNotification, jobState);

                            var dataSimpleItems = filteredMessages.Last().Key.Message.GetMessageDataSimpleItems();
                            bool flag = true;
                            string reason = "Ok";

                            if (null == dataSimpleItems)
                            {
                                flag = false;
                                reason = string.Format("No DataSimple items in notification for Recording Job with Token = '{0}'", jobToken);
                            }
                            else if (!dataSimpleItems.ContainsKey("State"))
                            {
                                flag = false;
                                reason = string.Format("No DataSimple item with Name = 'State' in notification for Recording Job with Token = '{0}'", jobToken);
                            }
                            else if (dataSimpleItems["State"] != jobState.State)
                            {
                                flag = false;
                                reason = string.Format("Value of DataSimple item with Name = 'State' in notification is different from value 'State' field in GetRecordingJobStateResponse for Recording Job with Token = '{0}'", jobToken);
                            }

                            Assert(flag, reason, "Checking recording job state");
                        }
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (jobCreated && !string.IsNullOrEmpty(jobToken))
                            DeleteRecordingJob(jobToken);
                        else if (!string.IsNullOrEmpty(jobModeInitial))
                            SetRecordingJobMode(jobToken, jobModeInitial);

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                    });
        }

        [Test(Name = "RECORDING CONTROL – RECORDING CONFIGURATION EVENT",
             Order = "05.01.03",
             Id = "5-1-3",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService },
             FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecordingJob, Functionality.CreateRecording, Functionality.RecordingConfigRecordingConfigEvent })]
        public void recordingConfigurationEventTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            GetRecordingsResponseItem restoreRecordingConfig = null;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        var recordings = GetRecordings();
                        Assert(recordings.Any(), 
                               "The DUT has no recordings", 
                               "Check that the DUT has any recordings");

                        var selectedRecording = recordings.First();

                        //Step 12.
                        var topicSet = GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        Assert(topics.Any(),
                               "The DUT provides no topics.",
                               "Check that the DUT provides event's topics.");

                        const string eventTopic = "tns1:RecordingConfig/RecordingConfiguration";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));

                        var eventDescription = new RecordingControlEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new EventItemDescription("Data/ElementItemDescription", "Configuration", "RecordingConfiguration", OnvifMessage.ONVIF));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);


                        var newConfig = deepCopyXmlSerializableObject(selectedRecording.Configuration);
                        if (null == newConfig.Source)
                            newConfig.Source = new RecordingSourceInformation()
                                               {
                                                   SourceId = CameraAddress.Trim(),
                                                   Name = "CameraName",
                                                   Location = "LocationDescription",
                                                   Description = "SourceDescription",
                                                   Address = CameraAddress.Trim()
                                               };
                        else
                        {
                            if (!newConfig.Source.Name.Any())
                                newConfig.Source.Name = "A";
                            else newConfig.Source.Name = newConfig.Source.Name == "A" ? "B" : "A";
                        }

                        SetRecordingConfiguration(selectedRecording.RecordingToken, newConfig);
                        restoreRecordingConfig = selectedRecording;

                        //Pull until notifications for all recording jobs are received
                        var pullingCondition = new WaitNotificationsForAllRecordingsPollingCondition(pullingTimeout, new[] {selectedRecording.RecordingToken});
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF) 
                                                                   { AllowedValues = new List<string>() { selectedRecording.RecordingToken } });
                        notificationDescription.addItemDescription(new NotificationItemDescription("Data/ElementItem", "Configuration", "RecordingConfiguration", OnvifMessage.ONVIF));

                        ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);

                        //Dictionary: RecordingJobToken => RecordingConfiguration
                        var informationNodes = messages.Select(e => e.Key.Message).
                                                        ToDictionary(e => GetAttributeValueOfItemFromMessage(e, "Source", "SimpleItem", "RecordingToken"),
                                                                     e => GetElementDataItemFromMessage<RecordingConfiguration>(e, "Configuration"));

                        foreach (var e in informationNodes)
                        {
                            var recordingConfiguration = GetRecordingConfiguration(e.Key);

                            CompareConfigurations(recordingConfiguration, e.Value, e.Key);
                        }
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (null != restoreRecordingConfig)
                            SetRecordingConfiguration(restoreRecordingConfig.RecordingToken, restoreRecordingConfig.Configuration, 
                                                      "Restore initial recording configuration");
                    });
        }

        [Test(Name = "RECORDING CONTROL – TRACK CONFIGURATION EVENT",
             Order = "05.01.04",
             Id = "5-1-4",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService },
             FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecordingJob, Functionality.CreateRecording, 
                                                            Functionality.SetTrackConfiguration, Functionality.RecordingConfigTrackConfigEvent })]
        public void trackConfigurationEventTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string selectedTrackToken = null;
            string selectedRecordingToken = null;
            TrackConfiguration restoreTrackConfiguration = null;

            RunTest(() =>
                {
                    Assert(null != recordingPortClient,
                           "Can't connect to Recording service",
                           "Check that Recording service is accessible");

                    var recordings = GetRecordings();

                    Assert(recordings.Any(),
                           "The device has no recordings",
                           "Check that device has recordings");

                    var selectedRecording =
                        recordings.FirstOrDefault(e => null != e.Tracks.Track && e.Tracks.Track.Any());

                    Assert(null != selectedRecording,
                           "The device has no recordings with tracks",
                           "Check that device has recordings with tracks");

                    //Step 12.
                    var topicSet = GetTopicSet();

                    var topics = new List<XmlElement>();
                    foreach (XmlElement element in topicSet.Any)
                    {
                        FindTopics(element, topics);
                    }

                    Assert(topics.Any(),
                           "The DUT provides no topics.",
                           "Check that the DUT provides event's topics.");

                    const string eventTopic = "tns1:RecordingConfig/TrackConfiguration";
                    var plainTopicInfo = new EventsTopicInfo()
                        {
                            Topic = eventTopic,
                            NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE)
                        };
                    var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                    var eventNode = GetTopicElement(topics, topicInfo);
                    Assert(null != eventNode,
                           string.Format("Event with topic {0} is not supported", eventTopic),
                           string.Format("Check that event with topic {0} is present", eventTopic));

                    var eventDescription = new RecordingControlEventDescription() {isProperty = false};
                    eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription",
                                                                                 "RecordingToken", "RecordingReference",
                                                                                 OnvifMessage.ONVIF));
                    eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription",
                                                                                 "TrackToken", "TrackReference",
                                                                                 OnvifMessage.ONVIF));
                    eventDescription.addItemDescription(new EventItemDescription("Data/ElementItemDescription",
                                                                                 "Configuration", "TrackConfiguration",
                                                                                 OnvifMessage.ONVIF));

                    var logger = new StringBuilder();
                    Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                           logger.ToString(),
                           string.Format("Checking description of event with topic {0}", eventTopic));

                    const int messageLimit = 1;

                    var filter = CreateSubscriptionFilter(topicInfo);

                    subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                    subscriptionHandler.Subscribe(filter, subscriptionTimeout);

                    var selectedTrack = selectedRecording.Tracks.Track.First();
                    var newConfig = deepCopyXmlSerializableObject(selectedTrack.Configuration);
                    newConfig.Description = ("A" == newConfig.Description) ? "B" : "A";
                    SetTrackConfiguration(selectedRecording.RecordingToken, selectedTrack.TrackToken, newConfig);
                    selectedRecordingToken = selectedRecording.RecordingToken;
                    selectedTrackToken = selectedTrack.TrackToken;
                    restoreTrackConfiguration = selectedTrack.Configuration;

                    //Pull until notifications for all recording jobs are received
                    var pullingCondition = new WaitNotificationsForAllTracksPollingCondition(pullingTimeout, new[]{ new KeyValuePair<string, string>(selectedRecording.RecordingToken, selectedTrack.TrackToken) });
                    Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                    Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                           pullingCondition.Reason,
                           "Checking that all required notifications are received");

                    var notificationDescription = new RecordingControlNotificationDescription();
                    notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem",
                                                                                               "RecordingToken",
                                                                                               "RecordingReference",
                                                                                               OnvifMessage.ONVIF)
                                                               {
                                                                   AllowedValues = new List<string>() { selectedRecording.RecordingToken}
                                                               });
                    notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem",
                                                                                               "TrackToken",
                                                                                               "TrackReference",
                                                                                               OnvifMessage.ONVIF)
                                                               {
                                                                   AllowedValues = new List<string>() { selectedTrack.TrackToken }
                                                               });
                    notificationDescription.addItemDescription(new NotificationItemDescription("Data/ElementItem",
                                                                                               "Configuration",
                                                                                               "TrackConfiguration",
                                                                                               OnvifMessage.ONVIF));

                    ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);

                    var trackConfiguration = GetTrackConfiguration(selectedRecording.RecordingToken,
                                                                   selectedTrack.TrackToken);

                    //Notification message for selected recording and selected track
                    var msg = messages.Select(e => e.Key.Message).
                                       Where(e => selectedRecording.RecordingToken == GetAttributeValueOfItemFromMessage(e, "Source", "SimpleItem", "RecordingToken")).
                                       FirstOrDefault(e =>selectedTrack.TrackToken == GetAttributeValueOfItemFromMessage(e, "Source", "SimpleItem", "TrackToken"));

                    CompareTrackConfigurations(GetElementDataItemFromMessage<TrackConfiguration>(msg, "Configuration"),
                                               trackConfiguration);
                },
                    () =>
                        {
                            SubscriptionHandler.Unsubscribe(subscriptionHandler);

                            if (null != restoreTrackConfiguration)
                                SetTrackConfiguration(selectedRecordingToken, selectedTrackToken, restoreTrackConfiguration, 
                                                      "Restore initial track configuration");
                        });
        }

        [Test(Name = "RECORDING CONTROL – RECORDING JOB CONFIGURATION EVENT",
             Order = "05.01.20",
             Id = "5-1-20",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.MediaOrReceiver, Feature.RecordingOptions },
             FunctionalityUnderTest = new Functionality[] {Functionality.GetRecordingJobConfiguration , 
                                                                                                 Functionality.SetRecordingJobConfiguration, 
                                                                                                 Functionality.RecordingConfigRecordingJobConfigEvent })]
        public void jobConfigurationEventTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string recordingToken = string.Empty;
            string profileToken = string.Empty;
            string jobToken = string.Empty;
            string jobModeInitial = string.Empty;

            RecordingJobConfiguration jobConfig = null;

            bool recordingCreated = false;
            bool jobCreated = false;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        GetRecordingJobsResponseItem[] jobs = GetRecordingJobs();
                        if (jobs == null || jobs.Length == 0)
                        {
                            if (Features.ContainsFeature(Feature.ReceiverService))
                            {
                                // A.12
                                GetRecordingForJobCreation(out recordingToken, out recordingCreated, 0);

                                // pass test if there is no recording token
                                if (string.IsNullOrEmpty(recordingToken))
                                    return;

                                jobConfig = new RecordingJobConfiguration();
                                jobConfig.Mode = "Idle";
                                jobConfig.Priority = 1;
                                jobConfig.RecordingToken = recordingToken;
                                RecordingJobSource source = new RecordingJobSource();
                                source.AutoCreateReceiverSpecified = true;
                                source.AutoCreateReceiver = true;

                                jobConfig.Source = new RecordingJobSource[] { source };
                            }
                            else
                            {
                                // A.15
                                GetRecordingForJobCreationMediaProfile(out recordingToken, out profileToken, out recordingCreated, 0);

                                // pass test if there is no recording token and profile token
                                if (string.IsNullOrEmpty(recordingToken) && string.IsNullOrEmpty(profileToken))
                                    return;

                                jobConfig = new RecordingJobConfiguration();
                                jobConfig.Mode = "Idle";
                                jobConfig.Priority = 1;
                                jobConfig.RecordingToken = recordingToken;
                                RecordingJobSource source = new RecordingJobSource();
                                source.AutoCreateReceiver = false;
                                source.SourceToken = new SourceReference();
                                source.SourceToken.Type = "http://www.onvif.org/ver10/schema/Profile";
                                source.SourceToken.Token = profileToken;

                                jobConfig.Source = new RecordingJobSource[] { source };
                            }

                            if (jobConfig != null)
                            {
                                jobToken = CreateRecordingJob(ref jobConfig);
                                jobCreated = true;
                            }
                        }
                        else
                        {
                            jobToken = jobs[0].JobToken;
                            jobConfig = jobs[0].JobConfiguration;
                        }

                        //Step 12.
                        var topicSet = GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        Assert(topics.Any(),
                               "The DUT provides no topics.",
                               "Check that the DUT provides event's topics.");

                        const string eventTopic = "tns1:RecordingConfig/RecordingJobConfiguration";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));

                        var eventDescription = new RecordingControlEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingJobToken", "RecordingJobReference", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new EventItemDescription("Data/ElementItemDescription", "Configuration", "RecordingJobConfiguration", OnvifMessage.ONVIF));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);


                        jobModeInitial = jobConfig.Mode;
                        var newConfig = deepCopyXmlSerializableObject(jobConfig);
                        newConfig.Mode = (IDLE == newConfig.Mode) ? ACTIVE : IDLE;
                        SetRecordingJobConfiguration(jobToken, newConfig);

                        //Pull until notifications for all recording jobs are received
                        var pullingCondition = new WaitNotificationsForAllJobsPollingCondition(pullingTimeout, new[] { jobToken });
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingJobToken", "RecordingJobReference", OnvifMessage.ONVIF) { AllowedValues = new List<string>() { jobToken } });
                        notificationDescription.addItemDescription(new NotificationItemDescription("Data/ElementItem", "Configuration", "RecordingJobConfiguration", OnvifMessage.ONVIF));

                        ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);

                        var jobConfiguration = GetRecordingJobConfiguration(jobToken);

                        //Notification message for selected recording job
                        var msg = messages.Select(e => e.Key.Message).
                                           FirstOrDefault(e => jobToken == GetAttributeValueOfItemFromMessage(e, "Source", "SimpleItem", "RecordingJobToken"));

                        CompareConfigurations(GetElementDataItemFromMessage<RecordingJobConfiguration>(msg, "Configuration"), jobConfiguration, "Notification", "GetRecordingJobConfiguration");
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (!string.IsNullOrEmpty(jobToken))
                        {
                            if (jobCreated)
                                DeleteRecordingJob(jobToken);
                            else if (!string.IsNullOrEmpty(jobModeInitial))
                                SetRecordingJobMode(jobToken, jobModeInitial);
                        }

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);
                    });
        }

        [Test(Name = "RECORDING CONTROL – CREATE RECORDING EVENT",
         Order = "05.01.06",
         Id = "5-1-6",
         Category = Category.RECORDING,
         Path = PATH_GENERAL,
         Version = 1.0,
         RequirementLevel = RequirementLevel.Must,
         RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordings },
         FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecording, Functionality.CreateRecordingEvent })]
        public void createRecordingEventTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string createdRecordingName = null;
            GetRecordingsResponseItem deletedRecording = null;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        List<GetRecordingsResponseItem> recordingsNames;
                        var possibleToCreateRecording = Annex10Prerequisities(out recordingsNames, out deletedRecording);

                        //Step 12.
                        var topicSet = GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        Assert(topics.Any(),
                               "The DUT provides no topics.",
                               "Check that the DUT provides event's topics.");

                        const string eventTopic = "tns1:RecordingConfig/CreateRecording";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));

                        var eventDescription = new RecordingControlEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));

                        const int messageLimit = 1;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);


                        RecordingConfiguration config;
                        createdRecordingName = Annex10CreateRecording(possibleToCreateRecording, recordingsNames, _retentionTime, 
                                                                      out config, out deletedRecording);

                        //Pull until notifications for all recording jobs are received
                        var pullingCondition = new WaitNotificationsForAllRecordingsPollingCondition(pullingTimeout, new[] { createdRecordingName });
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF) 
                                                                   { AllowedValues = new List<string>() { createdRecordingName } });

                        ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (null != createdRecordingName)
                            DeleteRecording(createdRecordingName);

                        if (null != deletedRecording)
                            CreateRecording(deletedRecording.Configuration);
                    });
        }

        [Test(Name = "RECORDING CONTROL – DELETE RECORDING EVENT",
             Order = "05.01.17",
             Id = "5-1-17",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordings },
             FunctionalityUnderTest = new Functionality[] { Functionality.DeleteRecording, Functionality.DeleteRecordingEvent })]
        public void deleteRecordingEventTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string selectedRecording = null;
            RecordingConfiguration initialConfig = null;

            bool recordingCreated = false;

            RunTest(() =>
            {
                Assert(null != recordingPortClient,
                        "Can't connect to Recording service",
                        "Check that Recording service is accessible");

                try
                {
                    // prepare recording configuration
                    RecordingConfiguration conf = new RecordingConfiguration();
                    conf.Source = new RecordingSourceInformation();
                    conf.Source.Description = "SourceDescription";
                    conf.Source.SourceId = "http://localhost/sourceID";
                    conf.Source.Location = "LocationDescription";
                    conf.Source.Name = "CameraName";
                    conf.Source.Address = "http://localhost/address";
                    conf.MaximumRetentionTime = "PT0S";
                    conf.Content = "Recording from device";

                    selectedRecording = CreateRecording(conf);
                    recordingCreated = true;
                }
                catch (FaultException exp)
                {
                    GetRecordingsResponseItem[] recordings = GetRecordings();
                    Assert(recordings != null && recordings.Length != 0, "Recording list is empty", "Check that recording list is not empty");

                    selectedRecording = recordings[0].RecordingToken;
                    initialConfig = recordings[0].Configuration;
                }

                //Step 12.
                var topicSet = GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                Assert(topics.Any(),
                        "The DUT provides no topics.",
                        "Check that the DUT provides event's topics.");

                const string eventTopic = "tns1:RecordingConfig/DeleteRecording";
                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                        string.Format("Event with topic {0} is not supported", eventTopic),
                        string.Format("Check that event with topic {0} is present", eventTopic));

                var eventDescription = new RecordingControlEventDescription() { isProperty = false };
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                        logger.ToString(),
                        string.Format("Checking description of event with topic {0}", eventTopic));

                const int messageLimit = 1;

                var filter = CreateSubscriptionFilter(topicInfo);

                subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                subscriptionHandler.Subscribe(filter, subscriptionTimeout);

                DeleteRecording(selectedRecording);
                recordingCreated = false;

                //Pull until notifications for all recording jobs are received
                var pullingCondition = new WaitNotificationsForAllRecordingsPollingCondition(pullingTimeout, new[] { selectedRecording });
                Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                        pullingCondition.Reason,
                        "Checking that all required notifications are received");

                var notificationDescription = new RecordingControlNotificationDescription();
                notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF) 
                                                            { AllowedValues = new List<string>() { selectedRecording } });

                ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(subscriptionHandler);

                if (null != initialConfig)
                {
                    CreateRecording(initialConfig);
                }

                if (recordingCreated && !string.IsNullOrEmpty(selectedRecording))
                {
                    DeleteRecording(selectedRecording);
                }

            });
        }

        [Test(Name = "RECORDING CONTROL – CREATE TRACK EVENT",
             Order = "05.01.08",
             Id = "5-1-8",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordingsOrDynamicTracks },
             FunctionalityUnderTest = new Functionality[] { Functionality.CreateTrackEvent })]
        public void createTrackEventTest()
        {
            RunTest(() =>
                    {
                        var topicSet = GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        Assert(topics.Any(),
                               "The DUT provides no topics.",
                               "Check that the DUT provides event's topics.");

                        const string eventTopic = "tns1:RecordingConfig/CreateTrack";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));

                        var eventDescription = new RecordingControlEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "TrackToken", "TrackReference", OnvifMessage.ONVIF));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));
                    });
        }

        [Test(Name = "RECORDING CONTROL – DELETE TRACK EVENT",
             Order = "05.01.09",
             Id = "5-1-9",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordingsOrDynamicTracks },
             FunctionalityUnderTest = new Functionality[] { Functionality.DeleteTrackEvent })]
        public void deleteTrackEventTest()
        {
            RunTest(() =>
                    {
                        var topicSet = GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        Assert(topics.Any(),
                               "The DUT provides no topics.",
                               "Check that the DUT provides event's topics.");

                        const string eventTopic = "tns1:RecordingConfig/DeleteTrack";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));

                        var eventDescription = new RecordingControlEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "TrackToken", "TrackReference", OnvifMessage.ONVIF));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));
                    });
        }

        [Test(Name = "RECORDING CONTROL – CREATE TRACK EVENT (CREATE RECORDING)",
             Order = "05.01.10",
             Id = "5-1-10",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordings, Feature.DynamicTracks },
             FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecording, Functionality.CreateTrackEvent })]
        public void createTrackEvenTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string createdRecordingName = null;
            GetRecordingsResponseItem deletedRecording = null;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        List<GetRecordingsResponseItem> recordingsNames;
                        var possibleToCreateRecording = Annex10Prerequisities(out recordingsNames, out deletedRecording);

                        const string eventTopic = "tns1:RecordingConfig/CreateTrack";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 2;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);

                        RecordingConfiguration config;
                        createdRecordingName = Annex10CreateRecording(possibleToCreateRecording, recordingsNames, _retentionTime,
                                                                      out config, out deletedRecording);


                        //Pull until notifications for all recording jobs are received
                        var defaultVideoTrack = new KeyValuePair<string, string>(createdRecordingName, "VIDEO001");
                        var defaultAudioTrack = new KeyValuePair<string, string>(createdRecordingName, "AUDIO001");
                        var defaultMetaDataTrack = new KeyValuePair<string, string>(createdRecordingName, "META001");
                        var defaultTracks = new[] {defaultVideoTrack, defaultAudioTrack, defaultMetaDataTrack};

                        var pullingCondition = new WaitNotificationsForAllTracksPollingCondition(pullingTimeout, defaultTracks);
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF) 
                                                                   { AllowedValues = new List<string>() { createdRecordingName } });

                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "TrackToken", "TrackReference", OnvifMessage.ONVIF) 
                                                                   { AllowedValues = defaultTracks.Select(e => e.Value).ToList() });

                        ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (null != createdRecordingName)
                            DeleteRecording(createdRecordingName);

                        if (null != deletedRecording)
                            CreateRecording(deletedRecording.Configuration);
                    });
        }

        [Test(Name = "RECORDING CONTROL – DELETE TRACK EVENT (DELETE RECORDING)",
         Order = "05.01.11",
         Id = "5-1-11",
         Category = Category.RECORDING,
         Path = PATH_GENERAL,
         Version = 1.0,
         RequirementLevel = RequirementLevel.Must,
         RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordings, Feature.DynamicTracks },
         FunctionalityUnderTest = new Functionality[] { Functionality.DeleteRecording, Functionality.DeleteTrackEvent })]
        public void deleteTrackEventDeleteRecordingTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay/1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string createdRecordingName = null;
            bool createdRecordingDeleted = false;

            GetRecordingsResponseItem deletedRecording = null;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        List<GetRecordingsResponseItem> recordingsNames;
                        var possibleToCreateRecording = Annex10Prerequisities(out recordingsNames, out deletedRecording);

                        RecordingConfiguration config;
                        createdRecordingName = Annex10CreateRecording(possibleToCreateRecording, recordingsNames, _retentionTime,
                                                                      out config, out deletedRecording);

                        const string eventTopic = "tns1:RecordingConfig/DeleteTrack";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 2;
                        System.DateTime localSubscribeStarted = System.DateTime.MaxValue;

                        System.DateTime TerminationExpectedTime = System.DateTime.Now;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);

                        DeleteRecording(createdRecordingName);
                        createdRecordingDeleted = true;

                        //Pull until notifications for all recording jobs are received
                        var defaultVideoTrack = new KeyValuePair<string, string>(createdRecordingName, "VIDEO001");
                        var defaultAudioTrack = new KeyValuePair<string, string>(createdRecordingName, "AUDIO001");
                        var defaultMetaDataTrack = new KeyValuePair<string, string>(createdRecordingName, "META001");
                        var defaultTracks = new[] { defaultVideoTrack, defaultAudioTrack, defaultMetaDataTrack };

                        var pullingCondition = new WaitNotificationsForAllTracksPollingCondition(pullingTimeout, defaultTracks);
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF) 
                                                                   { AllowedValues = new List<string>() { createdRecordingName } });

                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "TrackToken", "TrackReference", OnvifMessage.ONVIF) 
                                                                   { AllowedValues = defaultTracks.Select(e => e.Value).ToList() });

                        ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (null != createdRecordingName && !createdRecordingDeleted)
                            DeleteRecording(createdRecordingName);

                        if (null != deletedRecording)
                            CreateRecording(deletedRecording.Configuration);
                    });
        }

        [Test(Name = "RECORDING CONTROL – CREATE TRACK EVENT (CREATE TRACK)",
             Order = "05.01.15",
             Id = "5-1-15",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicTracks, Feature.RecordingOptions },
             FunctionalityUnderTest = new Functionality[] { Functionality.CreateTrack, Functionality.CreateTrackEvent })
        ]
        public void createTrackEventCreateTrackTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay / 1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string recordingToken = null;
            string trackToken = null;
            string trackTypeStr = null;

            bool isRecordingCreated = false;
            bool isTrackDeleted = false;
            bool isTrackCreated = false;

            TrackConfiguration deletedTrackConf = null;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        GetRecordingForTrackCreation(out recordingToken,
                                                     out trackTypeStr,
                                                     out isRecordingCreated, 
                                                     out isTrackDeleted,
                                                     out deletedTrackConf);

                        const string eventTopic = "tns1:RecordingConfig/CreateTrack";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 2;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);

                        TrackType trackType = (trackTypeStr == "Video" ? TrackType.Video :
                                                                  (trackTypeStr == "Audio" ? TrackType.Audio :
                                                                  (trackTypeStr == "Metadata" ? TrackType.Metadata : TrackType.Extended)));

                        // prepare track configuration
                        var trackConf = new TrackConfiguration();
                        trackConf.Description = "New Track";
                        trackConf.TrackType = trackType;

                        // create track
                        trackToken = CreateTrack(recordingToken, trackConf);
                        isTrackCreated = true;

                        //Pull until notifications for all recording jobs are received
                        var createdTrackSet = new[] { new KeyValuePair<string, string>(recordingToken, trackToken) };
                        var pullingCondition = new WaitNotificationsForAllTracksPollingCondition(pullingTimeout, createdTrackSet);
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF) { AllowedValues = new List<string>() { recordingToken } });

                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "TrackToken", "TrackReference", OnvifMessage.ONVIF) { AllowedValues = createdTrackSet.Select(e => e.Value).ToList() });

                        ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (isTrackCreated || isRecordingCreated || isTrackDeleted)
                            LogTestEvent(string.Format("Restoring the initial settings...{0}", Environment.NewLine));

                        // reverting changes made during test
                        if (isTrackCreated && !string.IsNullOrEmpty(trackToken))
                            DeleteTrack(recordingToken, trackToken);

                        // reverting changes made during A.14
                        if (isRecordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        // reverting changes made during A.14
                        if (isTrackDeleted && !string.IsNullOrEmpty(recordingToken) && deletedTrackConf != null)
                            CreateTrack(recordingToken, deletedTrackConf);

                    });
        }

        [Test(Name = "RECORDING CONTROL – DELETE TRACK EVENT (DELETE TRACK)",
            Order = "05.01.16",
            Id = "5-1-16",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicTracks, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteTrack, Functionality.DeleteTrackEvent })]
        public void deleteTrackEventDeleteTrackTest()
        {
            SubscriptionHandler subscriptionHandler = null;

            int pullingTimeout = _operationDelay / 1000;
            int subscriptionTimeout = _eventSubscriptionTimeout;

            string recordingToken = null;
            string trackToken = null;
            string trackTypeStr = null;

            bool isRecordingCreated = false;
            bool isTrackDeleted = false;
            bool isTrackCreated = false;

            TrackConfiguration deletedTrackConf = null;

            RunTest(() =>
                    {
                        Assert(null != recordingPortClient,
                               "Can't connect to Recording service",
                               "Check that Recording service is accessible");

                        GetRecordingForTrackCreation(out recordingToken,
                                                     out trackTypeStr,
                                                     out isRecordingCreated,
                                                     out isTrackDeleted,
                                                     out deletedTrackConf);

                        TrackType trackType = (trackTypeStr == "Video" ? TrackType.Video :
                                                  (trackTypeStr == "Audio" ? TrackType.Audio :
                                                  (trackTypeStr == "Metadata" ? TrackType.Metadata : TrackType.Extended)));

                        // prepare track configuration
                        var trackConf = new TrackConfiguration();
                        trackConf.Description = "New Track";
                        trackConf.TrackType = trackType;

                        // create track
                        trackToken = CreateTrack(recordingToken, trackConf);
                        isTrackCreated = true;

                        const string eventTopic = "tns1:RecordingConfig/DeleteTrack";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 2;

                        var filter = CreateSubscriptionFilter(topicInfo);

                        subscriptionHandler = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
                        subscriptionHandler.Subscribe(filter, subscriptionTimeout);

                        DeleteTrack(recordingToken, trackToken);
                        isTrackCreated = false;

                        //Pull until notifications for all recording jobs are received
                        var trackSet = new[] { new KeyValuePair<string, string>(recordingToken, trackToken) };
                        var pullingCondition = new WaitNotificationsForAllTracksPollingCondition(pullingTimeout, trackSet);
                        Dictionary<Event.NotificationMessageHolderType, XmlElement> messages;

                        Assert(subscriptionHandler.WaitMessages(messageLimit, pullingCondition, out messages),
                               pullingCondition.Reason,
                               "Checking that all required notifications are received");

                        var notificationDescription = new RecordingControlNotificationDescription();
                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF) { AllowedValues = new List<string>() { recordingToken } });

                        notificationDescription.addItemDescription(new NotificationItemDescription("Source/SimpleItem", "TrackToken", "TrackReference", OnvifMessage.ONVIF) { AllowedValues = trackSet.Select(e => e.Value).ToList() });

                        ValidateNotificationMessages(messages, topicInfo, notificationDescription, null);
                    },
                    () =>
                    {
                        SubscriptionHandler.Unsubscribe(subscriptionHandler);

                        if (isTrackCreated || isRecordingCreated || isTrackDeleted)
                            LogTestEvent(string.Format("Restoring the initial settings...{0}", Environment.NewLine));

                        // reverting changes made during test
                        if (isTrackCreated && !string.IsNullOrEmpty(trackToken))
                            DeleteTrack(recordingToken, trackToken);

                        // reverting changes made during A.14
                        if (isRecordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        // reverting changes made during A.14
                        if (isTrackDeleted && !string.IsNullOrEmpty(recordingToken) && deletedTrackConf != null)
                            CreateTrack(recordingToken, deletedTrackConf);
                    });
        }

        [Test(Name = "RECORDING CONTROL – DELETE TRACK DATA EVENT",
             Order = "05.01.14",
             Id = "5-1-14",
             Category = Category.RECORDING,
             Path = PATH_GENERAL,
             Version = 1.0,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.RecordingConfigDeleteTrackDataEvent,  },
             FunctionalityUnderTest = new Functionality[] { Functionality.DeleteTrackDataEvent })]
        public void deleteTrackDataEventTest()
        {
            RunTest(() =>
                    {
                        var topicSet = GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        Assert(topics.Any(),
                               "The DUT provides no topics.",
                               "Check that the DUT provides event's topics.");

                        const string eventTopic = "tns1:RecordingConfig/DeleteTrackData";
                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));

                        var eventDescription = new RecordingControlEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "RecordingToken", "RecordingReference", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "TrackToken", "TrackReference", OnvifMessage.ONVIF));
                        eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "StartTime", "dateTime", XSNAMESPACE));
                        eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "EndTime", "dateTime", XSNAMESPACE));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, topicSet, logger),
                               logger.ToString(),
                               string.Format("Checking description of event with topic {0}", eventTopic));
                    });
        }
    }
}
