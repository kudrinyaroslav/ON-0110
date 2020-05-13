using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
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
    public partial class RecordingControlRecordingTestSuite : RecordingTest
    {
        private const string ACTIVE = "Active";
        private const string PARTIALLY_ACTIVE = "PartiallyActive";
        private const string IDLE = "Idle";
        private const string CONNECTED = "Connected";

        private const string TNS1NAMESPACE = "http://www.onvif.org/ver10/topics";

        const string STATESIMPLEITEM = "State";  // Recording Control Specification, p. 5.21.1
        const string NEWSTATESIMPLEITEM = "NewState";
        const string RECORDINGJOBTOKENSIMPLEITEM = "RecordingJobToken";
        const string RECEIVERTOKENSIMPLEITEM = "ReceiverToken";

        private const string msgNoNotification = "Expected notification has not been received within predefined timeout \"Operation Delay\".";
        private readonly string msgNoNotificationHeaderFormat = msgNoNotification
                                                                + Environment.NewLine
                                                                + "The details of the expected notification are the following: {0}";

        private readonly string msgBeforePullMessagesHeaderFormat = "Send PullMessages requests until an event with {0} is received" + Environment.NewLine;

        private delegate void ActionRef(ref string token1, string token2, string token3);

        RTSPSimulator _simulator = null;
        protected int _eventSubscriptionTimeout;

        public RecordingControlRecordingTestSuite(TestLaunchParam param)
            : base(param)
        {
            _eventSubscriptionTimeout = param.SubscriptionTimeout;
        }

        protected override void Release()
        {
            if (_simulator != null)
            {
                StopSimulator(_simulator);
            }
            base.Release();
        } 

        private const string PATH_GENERAL = "Recording Control\\Recording";


        [Test(Name = "START RECORDING ON MEDIA PROFILE",
            Id = "2-1-28",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingControlService, Feature.MediaService, Feature.RecordingOptions },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecordingJob, Functionality.RecordingJobStateChangeEvent })]
        public void StartRecordingOnLocalStorageTest()
        {
            StartRecordingOnMediaProfileBis(null, null);
        }

        [Test(Name = "START RECORDING ON RECEIVER",
            Order = "02.01.20",
            Id = "2-1-20",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.ReceiverService, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecordingJob, Functionality.RecordingJobStateChangeEvent })]
        public void StartRecordingOnRemoteStorageTest()
        {
            StartRecordingOnReceiverBis(null, null);
        }

        [Test(Name = "STOP RECORDING ON MEDIA PROFILE - PUT JOB IN IDLE STATE",
            Id = "2-1-29",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingControlService, Feature.MediaService, Feature.RecordingOptions },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRecordingJobMode, Functionality.RecordingJobStateChangeEvent })]
        public void StopRecordingOnLocalStorageTest()
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay/1000;
            SubscriptionHandler Handler = null;

            StartRecordingOnMediaProfileBis((jobToken, recordingToken, profileToken) =>
            {
                TopicInfo topicInfo = new TopicInfo();
                topicInfo.ParentTopic = new TopicInfo();
                topicInfo.ParentTopic.Name = "RecordingConfig";
                topicInfo.ParentTopic.Namespace = TNS1NAMESPACE;
                topicInfo.ParentTopic.NamespacePrefix = "tns1";
                topicInfo.Name = "JobState";
                topicInfo.Namespace = TNS1NAMESPACE;

                TestTool.Proxies.Event.FilterType Filter = CreateFilter(topicInfo);
                    
                Handler = new SubscriptionHandler(this, false, GetEventServiceAddress());
                Handler.Subscribe(Filter, actualTerminationTime);

                SetRecordingJobMode(jobToken, IDLE);

                var eventDetails = string.Format("topic=\"tns1:RecordingConfig/JobState\", {0} = '{1}', 'JobToken' Simple Item with value='{2}' and 'State' Simple Item with value='{3}'", 
                                                 OnvifMessage.PROPERTYOPERATIONTYPE, OnvifMessage.CHANGED, jobToken, IDLE);
                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                Func<Event.NotificationMessageHolderType, bool> eventCheck = (m) => CheckJobMessage(m, OnvifMessage.CHANGED, jobToken, IDLE);

                Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications;
                var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = e => eventCheck(e) };

                // Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                // Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, true);

                // Validate ElementItem element
                Event.NotificationMessageHolderType message = notifications.Keys.First();

                // Validate that notification contains only one element item.
                XmlElement infoElement = ValidateElementItem(message.Message);

                // validate content of element item
                ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                // Parse RecordingJobStateInformation from notification
                RecordingJobStateInformation infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                // Validate that state is "Active"
                ValidateJobState(infoFromMessage, recordingToken, IDLE);

                RecordingJobStateInformation info = GetRecordingJobState(jobToken);

                ValidateJobState(info, recordingToken, IDLE, profileToken, IDLE);

                return false;
            },
            () => SubscriptionHandler.Unsubscribe(Handler)
            );
        }

        [Test(Name = "STOP RECORDING ON RECEIVER - PUT JOB IN IDLE STATE",
            Order = "02.01.22",
            Id = "2-1-22",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.ReceiverService, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRecordingJobMode, Functionality.RecordingJobStateChangeEvent })]
        public void StopRecordingOnRemoteStorageTest()
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay/1000;

            SubscriptionHandler Handler = null;

            StartRecordingOnReceiverBis((string jobToken, string recordingToken, string receiverToken, IEnumerable<string> encodings, out bool ret) =>
            {
                ret = false;
                TopicInfo topicInfo = new TopicInfo();
                topicInfo.ParentTopic = new TopicInfo();
                topicInfo.ParentTopic.Name = "RecordingConfig";
                topicInfo.ParentTopic.Namespace = TNS1NAMESPACE;
                topicInfo.ParentTopic.NamespacePrefix = "tns1";
                topicInfo.Name = "JobState";
                topicInfo.Namespace = TNS1NAMESPACE;

                TestTool.Proxies.Event.FilterType Filter = CreateFilter(topicInfo);
                Handler = new SubscriptionHandler(this, false, GetEventServiceAddress());
                Handler.Subscribe(Filter, actualTerminationTime);
                //subscription = CreatePullPointSubscription(topicInfo, out subscribed);

                var eventDetails = string.Format("topic=\"tns1:RecordingConfig/JobState\", {0} = '{1}', 'JobToken' Simple Item with value='{2}' and 'State' Simple Item with value='{3}'",
                                                 OnvifMessage.PROPERTYOPERATIONTYPE, OnvifMessage.CHANGED, jobToken, IDLE);
                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                Func<Event.NotificationMessageHolderType, bool> eventCheck = (m) => CheckJobMessage(m, OnvifMessage.CHANGED, jobToken, IDLE);

                SetRecordingJobMode(jobToken, IDLE);
                //13.	Execute Annex A.7 for catching event with Active state.
                //Dictionary<Event.NotificationMessageHolderType, string> notifications = GetMessages(subscription, T1, eventCheck);
                Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications;

                var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = e => eventCheck(e) };

                // Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");
                               
                // Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, true);

                // Validate ElementItem element
                Event.NotificationMessageHolderType message = notifications.Keys.First();

                // Validate that notification contains only one element item.
                XmlElement infoElement = ValidateElementItem(message.Message);

                // validate content of element item
                ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                // Parse RecordingJobStateInformation from notification
                RecordingJobStateInformation infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                // Validate that state is "Active"
                ValidateJobState(infoFromMessage, recordingToken, IDLE);

                //14.	ONVIF Client will invoke GetRecordingJobStateRequest message with JobToken= "JobToken1 ".
                //15.	Verify the GetRecordingJobStateResponse message from the DUT. Check that 
                // State.State="Active".

                RecordingJobStateInformation info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, IDLE);
            },
                () => SubscriptionHandler.Unsubscribe(Handler)
            );
        }

        [Test(Name = "STOP RECORDING ON RECEIVER - NEVER CONNECTED MODE",
            Order = "02.01.23",
            Id = "2-1-23",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.ReceiverService, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRecordingJobMode })]
        public void StopRecordingOnRemoteStorageNeverConnectedModeTestBis()
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;

            SubscriptionHandler Handler = null;
            SubscriptionHandler HandlerSec = null;

            string recordingToken = null;
            string jobToken = null;

            RecordingJobConfiguration jobConfiguration = null;
            bool recordingCreated = false;

            RunTest(() =>
            {
                RecordingServiceCapabilities capabilities = GetServiceCapabilities();

                //3.	Execute Annex A.12 to create or select of Recording.
                //recordingToken = GetRecordingForTest(out replacedConfiguration, out recordingCreated);
                GetRecordingForJobCreation(out recordingToken, out recordingCreated, 0);

                // pass test if there is no recording token
                if (recordingToken == string.Empty)
                    return;

                //4.	Execute Annex A.13  for Auto creation of receiver by create recording job with Idle mode.
                AutoCreationReceiver(recordingToken, out jobToken, out jobConfiguration);

                //5.	ONVIF Client will invoke CreatePullPointSubscriptionRequest message with tns1:RecordingConfig/JobState Topic as Filter and an InitialTerminationTime=TerminationTime1 to check Recording Job State changing.
                //6.	Verify that the DUT sends a CreatePullPointSubscriptionResponse message.
                // Create topic info
                TopicInfo topicInfo = new TopicInfo
                {
                    ParentTopic = new TopicInfo
                    {
                        Name = "Receiver",
                        Namespace = TNS1NAMESPACE,
                        NamespacePrefix = "tns1"
                    },
                    Name = "ChangeState",
                    Namespace = TNS1NAMESPACE
                };

                TestTool.Proxies.Event.FilterType Filter = CreateFilter(topicInfo);
                Handler = new SubscriptionHandler(this, false, GetEventServiceAddress());
                Handler.Subscribe(Filter, actualTerminationTime);

                _simulator = new RTSPSimulator(_nic.IP.ToString(), Sleep, _messageTimeout);
                CreateStreams(_simulator, capabilities.Encoding);
                StartSimulator(_simulator);

                /// null's should be validated in ValidateRecordingJobForRemoteStorageTests
                string receiverToken = jobConfiguration.Source[0].SourceToken.Token;
                System.DateTime T1 = System.DateTime.MinValue;

                {
                    ReceiverConfiguration config = new ReceiverConfiguration
                    {
                        MediaUri = GetUrl(_simulator, capabilities.Encoding, STREAM1),
                        Mode = ReceiverMode.AlwaysConnect,
                        StreamSetup = new StreamSetup
                        {
                            Stream = StreamType.RTPUnicast,
                            Transport = new Transport {Protocol = TransportProtocol.RTSP, Tunnel = null}
                        }
                    };

                    // URI for RTSP simulator
                    //7.	ONVIF Client will invoke ConfigureReceiverRequest message (ReceiverToken=ReceiverToken1, Configuration.Mode=”AlwaysConnect”, Configuration.MediaUri as stream_uri of RTSP Simulator, Configuration.StreamSetup.Stream=”RTP-Unicast”, StreamSetup.Transport.Tunnel.Protocol=“UDP”, no StreamSetup.Transport.Tunnel.Tunnel) to configure the receiver to receive media from RTSP Simulator.
                    //8.	Verify ConfigureReceiverResponse message from the DUT.

                    ConfigureReceiver(receiverToken, config);

                    //9.	ONVIF Client will invoke GetReceiverRequest message with ReceiverToken=ReceiverToken1.
                    //10.	Verify GetReceiverResponse message from the DUT. Check that GetReceiverResponse message contains the same parameters values as were changed in ConfigureReceiverRequest message.

                    Receiver actualReceiver = GetReceiver(receiverToken);

                    CheckReceiverConfigurationApplied(config, actualReceiver);
                }

                //11.	ONVIF Client will invoke SetRecordingJobModeRequest message (JobToken=”JobToken1”, Mode=”Active”) to start recording.
                //12.	Verify SetRecordingJobModeResponse message from the DUT. Mark time of CreateRecordingJobResponse as T1.
                SetRecordingJobMode(jobToken, ACTIVE);

                var eventDetails = string.Format("topic=\"tns1:Receiver/ChangeState\", 'ReceiverToken' Simple Item with value='{0}' and 'NewState' Simple Item with value='{1}'", receiverToken, "Connected");
                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                SubscriptionHandler.PollingConditionBase.MessageFilter eventCheck = (m) => CheckReceiverChangedMessage(m, receiverToken, ReceiverState.Connected);
                //13.	Execute Annex A.9 for catching event with Active state.
                //Dictionary<Event.NotificationMessageHolderType, string> notifications =
                //    GetMessages(subscription, T1, eventCheck);
                Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications;

                var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                // Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                // Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, false);

                //14.	ONVIF Client will invoke GetRecordingJobStateRequest message with JobToken= "JobToken1 ".
                //15.	Verify the GetRecordingJobStateResponse message from the DUT. Check that 
                // State.State="Active".
                var receiverInfo = GetReceiverState(receiverToken);
                ValidateReceiverState(receiverInfo, ReceiverState.Connected);

                RecordingJobStateInformation info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, new []{ ACTIVE, PARTIALLY_ACTIVE });

                TestTool.Proxies.Event.FilterType FilterSec = CreateFilter(topicInfo);
                HandlerSec = new SubscriptionHandler(this, false, GetEventServiceAddress());
                HandlerSec.Subscribe(FilterSec, actualTerminationTime);

                SetReceiverMode(receiverToken, ReceiverMode.NeverConnect);


                eventDetails = string.Format("topic=\"tns1:Receiver/ChangeState\", 'ReceiverToken' Simple Item with value='{0}' and 'NewState' Simple Item with value='{1}'", receiverToken, "NotConnected");
                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                eventCheck = (m) => CheckReceiverChangedMessage(m, receiverToken, ReceiverState.NotConnected);
                pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                // Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                // Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, false);

                receiverInfo = GetReceiverState(receiverToken);
                ValidateReceiverState(receiverInfo, ReceiverState.NotConnected);

                info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, IDLE);

            },
                () =>
                {
                    if (_simulator != null)
                    {
                        StopSimulator(_simulator);
                    }

                    if (!string.IsNullOrEmpty(jobToken))
                    {
                        DeleteRecordingJob(jobToken);
                        // Receiver should be deleted automatically.
                    }

                    if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                    {
                        DeleteRecording(recordingToken);
                    }

                    SubscriptionHandler.Unsubscribe(Handler);
                    SubscriptionHandler.Unsubscribe(HandlerSec);
                });

        }

       [Test(Name = "STOP RECORDING ON MEDIA PROFILE - DELETE JOB",
            Id = "2-1-30",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingControlService, Feature.MediaService, Feature.RecordingOptions },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteRecordingJob })]
        public void StopRecordingOnLocalStorageDeleteJobTest()
        {
            StartRecordingOnMediaProfileBis((jobToken, recordingToken, profileToken) =>
            {
                DeleteRecordingJob(jobToken);
                    
                var jobs = GetRecordingJobs();
                if (jobs != null)
                {
                    Assert(jobs.FirstOrDefault(j => j.JobToken == jobToken) == null,
                                 "RecordingJob wasn't deleted",
                                 string.Format("Check that RecordingJob (token='{0}') was deleted", jobToken));
                }
                return true;
            }, null);
        }

       [Test(Name = "STOP RECORDING ON RECEIVER - DELETE JOB",
            Order = "02.01.25",
            Id = "2-1-25",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.ReceiverService, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteRecordingJob })]
        public void StopRecordingOnRemoteStorageDeleteJobTest()
        {
            StartRecordingOnReceiverBis((string jobToken, string recordingToken, string receiverToken, IEnumerable<string> encodings, out bool ret) =>
                {
                    DeleteRecordingJob(jobToken);
                    ret = true;
                    var jobs = GetRecordingJobs();
                    /*Assert(jobs != null && jobs.Length > 0,
                        "RecordingJobs list wasn't returned",
                            "Check that RecordingJobs list was returned");*/
                    if (jobs != null)
                    {
                        Assert(jobs.FirstOrDefault(j => j.JobToken == jobToken) == null,
                        "RecordingJob wasn't deleted",
                            string.Format("Check that RecordingJob (token='{0}') was deleted", jobToken));
                    }
                }, null);
        }

        [Test(Name = "MODIFY MEDIA ATTRIBUTE WHILE RECORDING - MEDIA PROFILE",
            Id = "2-1-31",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingControlService, Feature.MediaService, Feature.RecordingOptions },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteRecordingJob })]
        public void ModifyMediaAttributeLocalStorageTest()
        {
            VideoEncoderConfiguration prevConf = null;

            StartRecordingOnMediaProfileBis2((jobToken, recordingToken, profileToken) =>
                {

                    Profile profile = null;
                    RunStep(() =>
                    {
                        profile = MediaClient.GetProfile(profileToken);
                    }, string.Format("Get Media profile (token = '{0}')", profileToken));
                    DoRequestDelay();
                    VerifyProfile(profile);

                VideoEncoderConfigurationOptions options = null;
                    RunStep(() =>
                    {
                        options = MediaClient.GetVideoEncoderConfigurationOptions(profile.VideoEncoderConfiguration.token, profile.token);
                    }, string.Format("Get video encoder configuration options (profile token = '{0}')", profileToken));
                    DoRequestDelay();
                    VideoEncoding encoding = profile.VideoEncoderConfiguration.Encoding;
                    VerifyOptions(options, encoding);
                    prevConf = profile.VideoEncoderConfiguration;

                var conf = (VideoEncoderConfiguration)CopyObject(prevConf);
                    SetNewVideoEncoderConfiguration(conf, options);

                RunStep(() =>
                    {
                        MediaClient.SetVideoEncoderConfiguration(conf, false);
                    }, "Set Video Encoder Configuration");
                    DoRequestDelay();

                VideoEncoderConfiguration newConf = null;
                    RunStep(() =>
                    {
                        newConf = MediaClient.GetVideoEncoderConfiguration(conf.token);
                    }, "Get Video Encoder Configuration");

                VerifyVideoConfiguration(conf, newConf);
                    DoRequestDelay();

                    return false;
                }, () =>
            {
                if (prevConf != null)
                {
                    RunStep(() =>
                    {
                        MediaClient.SetVideoEncoderConfiguration(prevConf, false);
                    }, "Set Video Encoder Configuration");
                }
            });
        }

        [Test(Name = "MODIFY MEDIA ATTRIBUTE WHILE RECORDING - RECEIVER",
            Order = "02.01.27",
            Id = "2-1-27",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.ReceiverService, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteRecordingJob })]
        public void ModifyMediaAttributeReceiverTest()
        {
            StartRecordingOnReceiverBis2((string jobToken, string recordingToken, string receiverToken, IEnumerable<string> encodings, out bool ret) =>
            {
                ret = false;
                ReceiverConfiguration config = new ReceiverConfiguration();

                // URI for RTSP simulator
                config.MediaUri = GetUrl(_simulator, encodings, STREAM2);
                config.Mode = ReceiverMode.AlwaysConnect;
                config.StreamSetup = new StreamSetup();
                config.StreamSetup.Stream = StreamType.RTPUnicast;
                config.StreamSetup.Transport = new Transport();
                config.StreamSetup.Transport.Protocol = TransportProtocol.RTSP;
                config.StreamSetup.Transport.Tunnel = null;
                //7.	ONVIF Client will invoke ConfigureReceiverRequest message (ReceiverToken=ReceiverToken1, Configuration.Mode=”AlwaysConnect”, Configuration.MediaUri as stream_uri of RTSP Simulator, Configuration.StreamSetup.Stream=”RTP-Unicast”, StreamSetup.Transport.Tunnel.Protocol=“UDP”, no StreamSetup.Transport.Tunnel.Tunnel) to configure the receiver to receive media from RTSP Simulator.
                //8.	Verify ConfigureReceiverResponse message from the DUT.

                ConfigureReceiver(receiverToken, config);
                    
                //Receiver actualReceiver = GetReceiver(receiverToken);
                //CheckReceiverConfigurationApplied(config, actualReceiver);

            }, null);
        }

        #region CONFIGURATION tests

        [Test(Name = "GET RECORDING JOB CONFIGURATION WITH INVALID TOKEN",
            Order = "04.01.13",
            Id = "4-1-13",
            Category = Category.RECORDING,
            Path = "Recording Control\\General",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.MediaOrReceiver, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingJobConfiguration })]
        public void RecordingJobConfigurationInvalidTokenTest()
        {
            string recordingToken = null;
            string profileToken = null;
            string jobToken = null;
            RecordingJobConfiguration config = null;
            bool recordingCreated = false;
           
            RunTest(() =>
            {

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
                        source.SourceToken.Type = PROFILESOURCETYPE;
                        source.SourceToken.Token = profileToken;
                        config.Source = new RecordingJobSource[] { source };
                    }

                    if (config != null)
                        jobToken = CreateRecordingJob(ref config);
                }


                this.InvalidTokenTestBody((s) => Client.GetRecordingJobConfiguration(s),
                    //RunStep, "Recording Job Configuration", OnvifFaults.NotFound);
                    // [AR] fix for wush 188
                    RunStep, "Recording Job Configuration", OnvifFaults.NoRecordingJob);
            },
            () =>
            {
                if (!string.IsNullOrEmpty(jobToken))
                {
                    DeleteRecordingJob(jobToken);
                }

                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                    DeleteRecording(recordingToken);
            });
        }

        [Test(Name = "GET RECORDING JOB STATE WITH INVALID TOKEN",
            Order = "04.01.14",
            Id = "4-1-14",
            Category = Category.RECORDING,
            Path = "Recording Control\\General",
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.MediaOrReceiver, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingJobState })]
        public void GetRecordingJobStateInvalidTokenTest()
        {
            string recordingToken = string.Empty;
            string profileToken = string.Empty;
            string jobToken = string.Empty;
            RecordingJobConfiguration config = null;
            bool recordingCreated = false;

            RunTest(() =>
            {
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
                        source.SourceToken.Type = PROFILESOURCETYPE;
                        source.SourceToken.Token = profileToken;

                        config.Source = new RecordingJobSource[] { source };
                    }

                    if (config != null)
                        jobToken = CreateRecordingJob(ref config);
                }

                this.InvalidTokenTestBody<object>((s, T) => Client.GetRecordingJobState(s), null,
                    RunStep, "Get Recording Job State", null, OnvifFaults.NoRecordingJob);
            }, 
            () =>
            {
                if (!string.IsNullOrEmpty(jobToken))
                {
                    DeleteRecordingJob(jobToken);
                }

                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                    DeleteRecording(recordingToken);
            });
        }



        #endregion

        private void StartRecordingOnMediaProfileBis(Func<string, string, string, bool> action, Action complete)
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay/1000;

            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            string recordingToken = null;
            string profileToken = null;
            bool recordingCreated = false;
            string jobToken = null;

            // For unsubscribe
            SubscriptionHandler Handler = null;


            bool isJobTokenDeleted = false;
            RunTest(() =>
                    {
                        // get Media service address 
                        MediaClient client = MediaClient;

                        // A.15 - Selection or Creation of Recording for recording job creation on a Media profile
                        GetRecordingForJobCreationMediaProfile(out recordingToken, out profileToken, out recordingCreated, 0);

                        // pass test if there is no recording token and profil token
                        if (recordingToken == string.Empty && profileToken == string.Empty)
                            return;

                        //5.	ONVIF Client will invoke CreatePullPointSubscriptionRequest message with tns1:RecordingConfig/JobState Topic as Filter and an InitialTerminationTime=TerminationTime1  to check Recording Job State changing.
                        //6.	Verify that the DUT sends a CreatePullPointSubscriptionResponse message.

                        // Create topic info
                        TopicInfo topicInfo = new TopicInfo
                            {
                                ParentTopic = new TopicInfo
                                    {
                                        Name = "RecordingConfig",
                                        Namespace = TNS1NAMESPACE,
                                        NamespacePrefix = "tns1"
                                    },
                                Name = "JobState",
                                Namespace = TNS1NAMESPACE
                            };

                        TestTool.Proxies.Event.FilterType Filter = CreateFilter(topicInfo);
                        Handler = new SubscriptionHandler(this, false, GetEventServiceAddress());
                        Handler.Subscribe(Filter, actualTerminationTime);


                        // 7.	ONVIF Client will invoke CreateRecordingJobRequest message 
                        // (JobConfiguration.RecordingToken = RecordingToken1, JobConfiguration.Mode = Active, 
                        // JobConfiguration.Priority = 1, JobConfiguration.Source.SourceToken.Token = “ProfileToken1”, 
                        // where ProfileToken1 is token of MediaProfile configured for recording, 
                        // JobConfiguration.Source.SourceToken.Type = ”http://www.onvif.org/ver10/schema/Profile”, 
                        // JobConfiguration.Source.AutoCreateReceiver is not present) to create a recording job 
                        // for configured recording with Active mode.
                        // 8.	Verify the CreateRecordingJobResponse message (JobToken = JobToken1). 
                        // Mark time of CreateRecordingJobResponse as T1.
                        // 9.	Verify that JobConfiguration in CreateRecordingJobResponse message contains 
                        // the same parameters values as was sent in CreateRecordingJobRequest message.

                        RecordingJobConfiguration jobConfiguration = new RecordingJobConfiguration
                        {
                            RecordingToken = recordingToken,
                            Mode = ACTIVE,
                            Priority = 1
                        };
                        RecordingJobSource source = new RecordingJobSource
                        {
                            SourceToken = new SourceReference {Token = profileToken, Type = PROFILESOURCETYPE}
                        };
                        source.AutoCreateReceiverSpecified = false;
                        jobConfiguration.Source = new[] { source };

                        string sourceToken = null;
                        int expectedPriority = -1;
                        if (jobConfiguration != null && jobConfiguration.Source != null && jobConfiguration.Source.Length > 0
                            && jobConfiguration.Source[0].SourceToken != null)
                        {
                            sourceToken = jobConfiguration.Source[0].SourceToken.Token;
                            expectedPriority = jobConfiguration.Priority;
                        }

                        System.DateTime T1 = System.DateTime.MinValue;
                        jobToken = CreateRecordingJob(ref jobConfiguration);
                        T1 = System.DateTime.Now;

                        Assert(!string.IsNullOrEmpty(jobToken),
                                     "Job token wasn't returned",
                                     "Check that job token was returned");

                        StringBuilder logger = new StringBuilder("Recording job configuration validation failed:" + Environment.NewLine);
                        bool ok = ValidateRecordingJob(jobConfiguration, logger, recordingToken, sourceToken, ACTIVE, expectedPriority, PROFILESOURCETYPE);
                        Assert(ok, logger.ToStringTrimNewLine(), "Validate recording job configuration");

                        var eventDetails = string.Format("topic=\"{0}:{1}/{2}\", '{3}' Simple Item with value='{4}' and 'State' Simple Item with value={5}",
                                                         topicInfo.ParentTopic.NamespacePrefix,
                                                         topicInfo.ParentTopic.Name,
                                                         topicInfo.Name,
                                                         RECORDINGJOBTOKENSIMPLEITEM,
                                                         jobToken, 
                                                         string.Format("'{0}' or '{1}'", ACTIVE, PARTIALLY_ACTIVE));
                        LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                        //14.	Execute Annex A.7 for catching event with Active state.
                        SubscriptionHandler.PollingConditionBase.MessageFilter eventCheck = (m) => CheckJobMessage(m, null, jobToken, new []{ ACTIVE, PARTIALLY_ACTIVE });

                        Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications;

                        var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                        /// Check that notifications have been received
                        Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                               string.Format(msgNoNotificationHeaderFormat, eventDetails),
                               "Check that the message with requested topic has been received so far");

                        /// Validate notification messages
                        ValidateNotificationMessages(notifications, topicInfo, true);
                        //Helper.ValidateMessages(notifications, Filter);


                        /// Validate ElementItem element
                        Event.NotificationMessageHolderType message = notifications.Keys.First();

                        /// Validate that notification contains only one element item.
                        XmlElement infoElement = ValidateElementItem(message.Message);

                        /// validate content of element item
                        ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                        /// Parse RecordingJobStateInformation from notification
                        RecordingJobStateInformation infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement,
                                                                                                                                "RecordingJobStateInformation",
                                                                                                                                OnvifMessage.ONVIF);

                        /// Validate that state is "Active"
                        ValidateJobState(infoFromMessage, recordingToken, new []{ ACTIVE, PARTIALLY_ACTIVE });

                        //15.	ONVIF Client will invoke GetRecordingJobStateRequest message with JobToken= "JobToken1 ".
                        //16.	Verify the GetRecordingJobStateResponse message from the DUT. Check that State.State and State.Sources.State parameter values are equal to "Active".

                        RecordingJobStateInformation info = GetRecordingJobState(jobToken);

                        //var sourceFromMessage = null != infoFromMessage.Sources ? infoFromMessage.Sources.FirstOrDefault(e => e.SourceToken.Token == source.SourceToken.Token) : null;
                        //if (null != sourceFromMessage)
                        ValidateJobState(info, recordingToken, infoFromMessage.State, source.SourceToken.Token, ACTIVE);
                        //else
                        //    ValidateJobState(info, recordingToken, infoFromMessage.State);

                        if (action != null)
                            isJobTokenDeleted = action(jobToken, recordingToken, profileToken);
                    },
                    () =>
                    {
                        // restore media configuration
                        if (_mediaClient != null)
                        {
                            CommonMethodsProvider.RollbackMediaConfiguration(this, MediaClient, changeLog);
                        }

                        if (!string.IsNullOrEmpty(jobToken) && !isJobTokenDeleted)
                        {
                            DeleteRecordingJob(jobToken);
                        }

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        SubscriptionHandler.Unsubscribe(Handler);

                        if (complete != null)
                            complete();
                    });
        }

        // same as StartRecordingOnMediaProfileBis 
        // in the end events waiting is added after comment "WAIT FOR EVENTS "
        private void StartRecordingOnMediaProfileBis2(Func<string, string, string, bool> action, Action complete)
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;

            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            string recordingToken = null;
            string profileToken = null;
            bool recordingCreated = false;
            string jobToken = null;

            // For unsubscribe
            SubscriptionHandler Handler = null;


            bool isJobTokenDeleted = false;
            RunTest(() =>
            {
                // get Media service address 
                MediaClient client = MediaClient;

                // A.15 - Selection or Creation of Recording for recording job creation on a Media profile
                GetRecordingForJobCreationMediaProfile(out recordingToken, out profileToken, out recordingCreated, 0);

                // pass test if there is no recording token and profil token
                if (recordingToken == string.Empty && profileToken == string.Empty)
                    return;

                //5.	ONVIF Client will invoke CreatePullPointSubscriptionRequest message with tns1:RecordingConfig/JobState Topic as Filter and an InitialTerminationTime=TerminationTime1  to check Recording Job State changing.
                //6.	Verify that the DUT sends a CreatePullPointSubscriptionResponse message.

                // Create topic info
                TopicInfo topicInfo = new TopicInfo
                {
                    ParentTopic = new TopicInfo
                    {
                        Name = "RecordingConfig",
                        Namespace = TNS1NAMESPACE,
                        NamespacePrefix = "tns1"
                    },
                    Name = "JobState",
                    Namespace = TNS1NAMESPACE
                };

                TestTool.Proxies.Event.FilterType Filter = CreateFilter(topicInfo);
                Handler = new SubscriptionHandler(this, false, GetEventServiceAddress());
                Handler.Subscribe(Filter, actualTerminationTime);


                // 7.	ONVIF Client will invoke CreateRecordingJobRequest message 
                // (JobConfiguration.RecordingToken = RecordingToken1, JobConfiguration.Mode = Active, 
                // JobConfiguration.Priority = 1, JobConfiguration.Source.SourceToken.Token = “ProfileToken1”, 
                // where ProfileToken1 is token of MediaProfile configured for recording, 
                // JobConfiguration.Source.SourceToken.Type = ”http://www.onvif.org/ver10/schema/Profile”, 
                // JobConfiguration.Source.AutoCreateReceiver is not present) to create a recording job 
                // for configured recording with Active mode.
                // 8.	Verify the CreateRecordingJobResponse message (JobToken = JobToken1). 
                // Mark time of CreateRecordingJobResponse as T1.
                // 9.	Verify that JobConfiguration in CreateRecordingJobResponse message contains 
                // the same parameters values as was sent in CreateRecordingJobRequest message.

                RecordingJobConfiguration jobConfiguration = new RecordingJobConfiguration
                {
                    RecordingToken = recordingToken,
                    Mode = ACTIVE,
                    Priority = 1
                };
                RecordingJobSource source = new RecordingJobSource
                {
                    SourceToken = new SourceReference { Token = profileToken, Type = PROFILESOURCETYPE }
                };
                source.AutoCreateReceiverSpecified = false;
                jobConfiguration.Source = new[] { source };

                string sourceToken = null;
                int expectedPriority = -1;
                if (jobConfiguration != null && jobConfiguration.Source != null && jobConfiguration.Source.Length > 0
                    && jobConfiguration.Source[0].SourceToken != null)
                {
                    sourceToken = jobConfiguration.Source[0].SourceToken.Token;
                    expectedPriority = jobConfiguration.Priority;
                }

                System.DateTime T1 = System.DateTime.MinValue;
                jobToken = CreateRecordingJob(ref jobConfiguration);
                T1 = System.DateTime.Now;

                Assert(!string.IsNullOrEmpty(jobToken),
                             "Job token wasn't returned",
                             "Check that job token was returned");

                StringBuilder logger = new StringBuilder("Recording job configuration validation failed:" + Environment.NewLine);
                bool ok = ValidateRecordingJob(jobConfiguration, logger, recordingToken, sourceToken, ACTIVE, expectedPriority, PROFILESOURCETYPE);
                Assert(ok, logger.ToStringTrimNewLine(), "Validate recording job configuration");

                var eventDetails = string.Format("topic=\"{0}:{1}/{2}\", '{3}' Simple Item with value='{4}' and 'State' Simple Item with value='{5}' or '{6}'",
                                                 topicInfo.ParentTopic.NamespacePrefix,
                                                 topicInfo.ParentTopic.Name,
                                                 topicInfo.Name,
                                                 RECORDINGJOBTOKENSIMPLEITEM,
                                                 jobToken,
                                                 ACTIVE, 
                                                 PARTIALLY_ACTIVE);
                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                //14.	Execute Annex A.7 for catching event with Active state.
                SubscriptionHandler.PollingConditionBase.MessageFilter eventCheck = (m) => CheckJobMessage(m, null, jobToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

                Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications;

                var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                /// Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                /// Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, true);
                //Helper.ValidateMessages(notifications, Filter);


                /// Validate ElementItem element
                Event.NotificationMessageHolderType message = notifications.Keys.First();

                /// Validate that notification contains only one element item.
                XmlElement infoElement = ValidateElementItem(message.Message);

                /// validate content of element item
                ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Parse RecordingJobStateInformation from notification
                RecordingJobStateInformation infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement,
                                                                                                                        "RecordingJobStateInformation",
                                                                                                                        OnvifMessage.ONVIF);

                /// Validate that state is "Active"
                ValidateJobState(infoFromMessage, recordingToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

                //15.	ONVIF Client will invoke GetRecordingJobStateRequest message with JobToken= "JobToken1 ".
                //16.	Verify the GetRecordingJobStateResponse message from the DUT. Check that State.State and State.Sources.State parameter values are equal to "Active".

                RecordingJobStateInformation info = GetRecordingJobState(jobToken);

                //var sourceFromMessage = null != infoFromMessage.Sources ? infoFromMessage.Sources.FirstOrDefault(e => e.SourceToken.Token == source.SourceToken.Token) : null;
                //if (null != sourceFromMessage)
                ValidateJobState(info, recordingToken, infoFromMessage.State, source.SourceToken.Token, ACTIVE);
                //else
                //    ValidateJobState(info, recordingToken, infoFromMessage.State);

                if (action != null)
                    isJobTokenDeleted = action(jobToken, recordingToken, profileToken);

                // WAIT FOR EVENTS 

                var jobStateInfo = GetRecordingJobState(jobToken);
                bool ok2 = ValidateJobStateSimple(jobStateInfo, recordingToken, new[] { ACTIVE, PARTIALLY_ACTIVE });
                if (ok2)
                    return;

                eventDetails = string.Format("topic=\"{0}:{1}/{2}\", {3}='{4}', '{5}' Simple Item with value='{6}' and 'State' Simple Item with value='{7}' or '{8}'",
                                             topicInfo.ParentTopic.NamespacePrefix,
                                             topicInfo.ParentTopic.Name,
                                             topicInfo.Name,
                                             OnvifMessage.PROPERTYOPERATIONTYPE, OnvifMessage.CHANGED,
                                             RECORDINGJOBTOKENSIMPLEITEM,
                                             jobToken,
                                             ACTIVE,
                                             PARTIALLY_ACTIVE);

                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                eventCheck = (m) => CheckJobMessage(m, OnvifMessage.CHANGED, jobToken, new[] { ACTIVE, PARTIALLY_ACTIVE });
                pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                /// Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                /// Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, true);

                /// Validate ElementItem element
                message = notifications.Keys.First();

                /// Validate that notification contains only one element item.
                infoElement = ValidateElementItem(message.Message);

                /// validate content of element item
                ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Parse RecordingJobStateInformation from notification
                 infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement,
                                                     "RecordingJobStateInformation",
                                                       OnvifMessage.ONVIF);

                /// Validate that state is "Active"
                ValidateJobState(infoFromMessage, recordingToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

                info = GetRecordingJobState(jobToken);

                ValidateJobState(info, recordingToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

            },
                    () =>
                    {
                        // restore media configuration
                        if (_mediaClient != null)
                        {
                            CommonMethodsProvider.RollbackMediaConfiguration(this, MediaClient, changeLog);
                        }

                        if (!string.IsNullOrEmpty(jobToken) && !isJobTokenDeleted)
                        {
                            DeleteRecordingJob(jobToken);
                        }

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        SubscriptionHandler.Unsubscribe(Handler);

                        if (complete != null)
                            complete();
                    });
        }

        public delegate void FinalAction(string s1, string s2, string s3, IEnumerable<string> e, out bool returnflag);
        private void StartRecordingOnReceiverBis(FinalAction action, Action complete)
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay/1000;

            string recordingToken = null;
            string jobToken = null;

            RecordingJobConfiguration jobConfiguration = null;
            SubscriptionHandler Handler = null;

            bool recordingCreated = false;
            bool isJobTokenDeleted = false;

            RunTest(() =>
            {
                //3.	Execute Annex A.12 to create or select of Recording.
                GetRecordingForJobCreation(out recordingToken, out recordingCreated, 0);

                // pass test if there is no recording token
                if (recordingToken == string.Empty)
                    return;

                RecordingServiceCapabilities capabilities = GetServiceCapabilities();

                //4.	Execute Annex A.13  for Auto creation of receiver by create recording job with Idle mode.
                var receiverSourceToken = AutoCreationReceiver(recordingToken, out jobToken, out jobConfiguration);

                //5.	ONVIF Client will invoke CreatePullPointSubscriptionRequest message with tns1:RecordingConfig/JobState Topic as Filter and an InitialTerminationTime=TerminationTime1 to check Recording Job State changing.
                //6.	Verify that the DUT sends a CreatePullPointSubscriptionResponse message.
                // Create topic info
                TopicInfo topicInfo = new TopicInfo
                {
                    ParentTopic = new TopicInfo
                        {
                            Name = "RecordingConfig",
                            Namespace = TNS1NAMESPACE,
                            NamespacePrefix = "tns1"
                        },
                    Name = "JobState",
                    Namespace = TNS1NAMESPACE
                };

                //subscription = CreatePullPointSubscription(topicInfo, out subscribed);
                TestTool.Proxies.Event.FilterType Filter = CreateFilter(topicInfo);

                Handler = new SubscriptionHandler(this, false, GetEventServiceAddress());
                Handler.Subscribe(Filter, actualTerminationTime);

                _simulator = new RTSPSimulator(_nic.IP.ToString(), Sleep, _messageTimeout);
                CreateStreams(_simulator, capabilities.Encoding);
                StartSimulator(_simulator);

                /// null's should be validated in ValidateRecordingJobForRemoteStorageTests
                string receiverToken = jobConfiguration.Source[0].SourceToken.Token;
                System.DateTime T1 = System.DateTime.MinValue;

                {
                    ReceiverConfiguration config = new ReceiverConfiguration
                    {
                        MediaUri = GetUrl(_simulator, capabilities.Encoding, STREAM1),
                        Mode = ReceiverMode.AlwaysConnect,
                        StreamSetup = new StreamSetup
                        {
                            Stream = StreamType.RTPUnicast,
                            Transport = new Transport {Protocol = TransportProtocol.RTSP, Tunnel = null}
                        }
                    };

                    // URI for RTSP simulator
                    //7.	ONVIF Client will invoke ConfigureReceiverRequest message (ReceiverToken=ReceiverToken1, Configuration.Mode=”AlwaysConnect”, Configuration.MediaUri as stream_uri of RTSP Simulator, Configuration.StreamSetup.Stream=”RTP-Unicast”, StreamSetup.Transport.Tunnel.Protocol=“UDP”, no StreamSetup.Transport.Tunnel.Tunnel) to configure the receiver to receive media from RTSP Simulator.
                    //8.	Verify ConfigureReceiverResponse message from the DUT.

                    ConfigureReceiver(receiverToken, config);

                    //9.	ONVIF Client will invoke GetReceiverRequest message with ReceiverToken=ReceiverToken1.
                    //10.	Verify GetReceiverResponse message from the DUT. Check that GetReceiverResponse message contains the same parameters values as were changed in ConfigureReceiverRequest message.

                    Receiver actualReceiver = GetReceiver(receiverToken);

                    CheckReceiverConfigurationApplied(config, actualReceiver);
                }

                //11.	ONVIF Client will invoke SetRecordingJobModeRequest message (JobToken=”JobToken1”, Mode=”Active”) to start recording.
                //12.	Verify SetRecordingJobModeResponse message from the DUT. Mark time of CreateRecordingJobResponse as T1.
                SetRecordingJobMode(jobToken, ACTIVE);

                var eventDetails = string.Format("topic=\"tns1:RecordingConfig/JobState\", {0}='{1}', 'JobToken' Simple Item with value='{2}' and 'State' Simple Item with value='{3}' or '{4}'",
                                                 OnvifMessage.PROPERTYOPERATIONTYPE, OnvifMessage.CHANGED,
                                                 jobToken, ACTIVE, PARTIALLY_ACTIVE);
                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                SubscriptionHandler.PollingConditionBase.MessageFilter eventCheck = (m) => CheckJobMessage(m, OnvifMessage.CHANGED, jobToken, new []{ ACTIVE, PARTIALLY_ACTIVE });

                //13.	Execute Annex A.7 for catching event with Active state.
                Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications;

                var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                /// Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                /// Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, true);

                /// Validate ElementItem element
                Event.NotificationMessageHolderType message = notifications.Keys.First();

                /// Validate that notification contains only one element item.
                XmlElement infoElement = ValidateElementItem(message.Message);

                /// validate content of element item
                ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Parse RecordingJobStateInformation from notification
                RecordingJobStateInformation infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Validate that state is "Active"
                ValidateJobState(infoFromMessage, recordingToken, new []{ ACTIVE, PARTIALLY_ACTIVE });

                //14.	ONVIF Client will invoke GetRecordingJobStateRequest message with JobToken= "JobToken1 ".
                //15.	Verify the GetRecordingJobStateResponse message from the DUT. Check that 
                // State.State="Active".

                RecordingJobStateInformation info = GetRecordingJobState(jobToken);
                //var sourceFromMessage = null != infoFromMessage.Sources ? infoFromMessage.Sources.FirstOrDefault(e => e.SourceToken.Token == receiverSourceToken) : null;
                //if (null != sourceFromMessage)
                ValidateJobState(info, recordingToken, infoFromMessage.State, receiverSourceToken, ACTIVE);
                //else
                //    ValidateJobState(info, recordingToken, infoFromMessage.State);
                if (action != null)
                    action(jobToken, recordingToken, receiverToken, capabilities.Encoding, out isJobTokenDeleted);
            },
            () =>
            {
                if (!string.IsNullOrEmpty(jobToken) && !isJobTokenDeleted)
                {
                    DeleteRecordingJob(jobToken);
                    // Receiver should be deleted automatically.
                }

                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                {
                    DeleteRecording(recordingToken);
                }

                SubscriptionHandler.Unsubscribe(Handler);

                if (complete != null)
                    complete();
             });
        }

       // same as StartRecordingOnReceiverBis
       // in the end events waiting is added after comment "WAIT FOR EVENTS "
       private void StartRecordingOnReceiverBis2(FinalAction action, Action complete)
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay/1000;

            string recordingToken = null;
            string jobToken = null;

            RecordingJobConfiguration jobConfiguration = null;
            SubscriptionHandler Handler = null;

            bool recordingCreated = false;
            bool isJobTokenDeleted = false;

            RunTest(() =>
            {
                //3.	Execute Annex A.12 to create or select of Recording.
                GetRecordingForJobCreation(out recordingToken, out recordingCreated, 0);

                // pass test if there is no recording token
                if (recordingToken == string.Empty)
                    return;

                RecordingServiceCapabilities capabilities = GetServiceCapabilities();

                //4.	Execute Annex A.13  for Auto creation of receiver by create recording job with Idle mode.
                var receiverSourceToken = AutoCreationReceiver(recordingToken, out jobToken, out jobConfiguration);

                //5.	ONVIF Client will invoke CreatePullPointSubscriptionRequest message with tns1:RecordingConfig/JobState Topic as Filter and an InitialTerminationTime=TerminationTime1 to check Recording Job State changing.
                //6.	Verify that the DUT sends a CreatePullPointSubscriptionResponse message.
                // Create topic info
                TopicInfo topicInfo = new TopicInfo
                {
                    ParentTopic = new TopicInfo
                        {
                            Name = "RecordingConfig",
                            Namespace = TNS1NAMESPACE,
                            NamespacePrefix = "tns1"
                        },
                    Name = "JobState",
                    Namespace = TNS1NAMESPACE
                };

                //subscription = CreatePullPointSubscription(topicInfo, out subscribed);
                TestTool.Proxies.Event.FilterType Filter = CreateFilter(topicInfo);

                Handler = new SubscriptionHandler(this, false, GetEventServiceAddress());
                Handler.Subscribe(Filter, actualTerminationTime);

                _simulator = new RTSPSimulator(_nic.IP.ToString(), Sleep, _messageTimeout);
                CreateStreams(_simulator, capabilities.Encoding);
                StartSimulator(_simulator);

                /// null's should be validated in ValidateRecordingJobForRemoteStorageTests
                string receiverToken = jobConfiguration.Source[0].SourceToken.Token;
                System.DateTime T1 = System.DateTime.MinValue;

                {
                    ReceiverConfiguration config = new ReceiverConfiguration
                    {
                        MediaUri = GetUrl(_simulator, capabilities.Encoding, STREAM1),
                        Mode = ReceiverMode.AlwaysConnect,
                        StreamSetup = new StreamSetup
                        {
                            Stream = StreamType.RTPUnicast,
                            Transport = new Transport {Protocol = TransportProtocol.RTSP, Tunnel = null}
                        }
                    };

                    // URI for RTSP simulator
                    //7.	ONVIF Client will invoke ConfigureReceiverRequest message (ReceiverToken=ReceiverToken1, Configuration.Mode=”AlwaysConnect”, Configuration.MediaUri as stream_uri of RTSP Simulator, Configuration.StreamSetup.Stream=”RTP-Unicast”, StreamSetup.Transport.Tunnel.Protocol=“UDP”, no StreamSetup.Transport.Tunnel.Tunnel) to configure the receiver to receive media from RTSP Simulator.
                    //8.	Verify ConfigureReceiverResponse message from the DUT.

                    ConfigureReceiver(receiverToken, config);

                    //9.	ONVIF Client will invoke GetReceiverRequest message with ReceiverToken=ReceiverToken1.
                    //10.	Verify GetReceiverResponse message from the DUT. Check that GetReceiverResponse message contains the same parameters values as were changed in ConfigureReceiverRequest message.

                    Receiver actualReceiver = GetReceiver(receiverToken);

                    CheckReceiverConfigurationApplied(config, actualReceiver);
                }

                //11.	ONVIF Client will invoke SetRecordingJobModeRequest message (JobToken=”JobToken1”, Mode=”Active”) to start recording.
                //12.	Verify SetRecordingJobModeResponse message from the DUT. Mark time of CreateRecordingJobResponse as T1.
                SetRecordingJobMode(jobToken, ACTIVE);

                var eventDetails = string.Format("topic=\"tns1:RecordingConfig/JobState\", {0}='{1}', 'JobToken' Simple Item with value='{2}' and 'State' Simple Item with value='{3}' or '{4}'",
                                                 OnvifMessage.PROPERTYOPERATIONTYPE, OnvifMessage.CHANGED,
                                                 jobToken, ACTIVE, PARTIALLY_ACTIVE);
                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                SubscriptionHandler.PollingConditionBase.MessageFilter eventCheck = (m) => CheckJobMessage(m, OnvifMessage.CHANGED, jobToken, new []{ ACTIVE, PARTIALLY_ACTIVE });

                //13.	Execute Annex A.7 for catching event with Active state.
                Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications;

                var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                /// Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                /// Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, true);

                /// Validate ElementItem element
                Event.NotificationMessageHolderType message = notifications.Keys.First();

                /// Validate that notification contains only one element item.
                XmlElement infoElement = ValidateElementItem(message.Message);

                /// validate content of element item
                ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Parse RecordingJobStateInformation from notification
                RecordingJobStateInformation infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Validate that state is "Active"
                ValidateJobState(infoFromMessage, recordingToken, new []{ ACTIVE, PARTIALLY_ACTIVE });

                //14.	ONVIF Client will invoke GetRecordingJobStateRequest message with JobToken= "JobToken1 ".
                //15.	Verify the GetRecordingJobStateResponse message from the DUT. Check that 
                // State.State="Active".

                RecordingJobStateInformation info = GetRecordingJobState(jobToken);
                //var sourceFromMessage = null != infoFromMessage.Sources ? infoFromMessage.Sources.FirstOrDefault(e => e.SourceToken.Token == receiverSourceToken) : null;
                //if (null != sourceFromMessage)
                ValidateJobState(info, recordingToken, infoFromMessage.State, receiverSourceToken, ACTIVE);
                //else
                //    ValidateJobState(info, recordingToken, infoFromMessage.State);
                if (action != null)
                    action(jobToken, recordingToken, receiverToken, capabilities.Encoding, out isJobTokenDeleted);

                // WAIT FOR EVENTS

                var jobStateInfo = GetRecordingJobState(jobToken);
                bool ok2 = ValidateJobStateSimple(jobStateInfo, recordingToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

                if (ok2)
                    return;

                eventDetails = string.Format("topic=\"tns1:RecordingConfig/JobState\", {0}='{1}', 'JobToken' Simple Item with value='{2}' and 'State' Simple Item with value='{3}' or '{4}'",
                                             OnvifMessage.PROPERTYOPERATIONTYPE, OnvifMessage.CHANGED,
                                             jobToken, ACTIVE, PARTIALLY_ACTIVE);

                LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                eventCheck = (m) => CheckJobMessage(m, OnvifMessage.CHANGED, jobToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

                pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = eventCheck };

                /// Check that notifications have been received
                Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                       string.Format(msgNoNotificationHeaderFormat, eventDetails),
                       "Check that the message with requested topic has been received so far");

                /// Validate notification messages
                ValidateNotificationMessages(notifications, topicInfo, true);

                /// Validate ElementItem element
                message = notifications.Keys.First();

                /// Validate that notification contains only one element item.
                infoElement = ValidateElementItem(message.Message);

                /// validate content of element item
                ValidateElementItemContent(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Parse RecordingJobStateInformation from notification
                infoFromMessage = ExtractElementItemsContent<RecordingJobStateInformation>(infoElement, "RecordingJobStateInformation", OnvifMessage.ONVIF);

                /// Validate that state is "Active"
                ValidateJobState(infoFromMessage, recordingToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

                info = GetRecordingJobState(jobToken);

                ValidateJobState(info, recordingToken, new[] { ACTIVE, PARTIALLY_ACTIVE });

            },
            () =>
            {
                if (!string.IsNullOrEmpty(jobToken) && !isJobTokenDeleted)
                {
                    DeleteRecordingJob(jobToken);
                    // Receiver should be deleted automatically.
                }

                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                {
                    DeleteRecording(recordingToken);
                }

                SubscriptionHandler.Unsubscribe(Handler);

                if (complete != null)
                    complete();
             });
        }

    }
}
