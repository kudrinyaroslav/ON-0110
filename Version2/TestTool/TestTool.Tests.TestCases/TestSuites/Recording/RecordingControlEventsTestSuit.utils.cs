using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Exceptions;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using TestTool.Tests.TestCases.TestSuites.Events;
using Event = TestTool.Proxies.Event;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using System.Threading;
using TestTool.Tests.Common.Trace;
using System.IO;
using TestTool.Proxies.Event;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.TestCases.Utils;
using System.Reflection;
using System.Xml.Serialization;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Enums;
using DateTime = System.DateTime;

namespace TestTool.Tests.TestCases.TestSuites.Recording
{
    partial class RecordingControlEventsTestSuite
    {
        #region Recording Client
        private ServiceHolder<RecordingPortClient, RecordingPort> _recordingServiceHolder;

        private RecordingPortClient recordingPortClient
        {
            get
            {
                if (null == _recordingServiceHolder)
                {
                    _recordingServiceHolder = new ServiceHolder<RecordingPortClient, RecordingPort>((features) => DeviceClient.GetServiceAddress(OnvifService.RECORIDING),
                                                                                                    (binding, address) => new RecordingPortClient(binding, address),
                                                                                                    "Recording Control");

                    if (null == _recordingServiceHolder.Client)
                    {
                        InitServiceClient(_recordingServiceHolder,
                                          new IChannelController[] { new SoapValidator(RecordingSchemasSet.GetInstance()) });
                    }
                }

                return _recordingServiceHolder.Client;
            }
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
                },
                        string.Format("Get {0} service address", serviceHolder.ServiceName),
                        OnvifFaults.NoSuchService, true, true);
                DoRequestDelay();
            }

            Assert(found,
                   string.Format("{0} service address not found", serviceHolder.ServiceName),
                   string.Format("Check that the DUT returned {0} service address", serviceHolder.ServiceName));

            if (found)
            {
                EndpointController controller = new EndpointController(new EndpointAddress(serviceHolder.Address));

                List<IChannelController> ctrls = new List<IChannelController>();
                ctrls.Add(controller);
                ctrls.AddRange(controllers);

                Binding binding = CreateBinding(false, ctrls);

                serviceHolder.CreateClient(binding, AttachSecurity, SetupChannel);
            }
        }

        public GetRecordingJobsResponseItem[] GetRecordingJobs()
        {
            GetRecordingJobsResponseItem[] result = null;
            RunStep(() => { result = recordingPortClient.GetRecordingJobs(); },
                    "Get Recording Jobs");
            DoRequestDelay();
            return result ?? new GetRecordingJobsResponseItem[] {};
        }

        public GetRecordingsResponseItem[] GetRecordings()
        {
            GetRecordingsResponseItem[] result = null;
            RunStep(() => { result = recordingPortClient.GetRecordings(); },
                    "Get Recordings");
            DoRequestDelay();
            return result ?? new GetRecordingsResponseItem[] {};
        }

        public string CreateRecording(RecordingConfiguration RecordingConfiguration)
        {
            string result = string.Empty;
            RunStep(() => { result = recordingPortClient.CreateRecording(RecordingConfiguration); },
                    "Create Recording");
            DoRequestDelay();
            return result;
        }

        public string CreateRecordingJob(ref RecordingJobConfiguration jobConfiguration)
        {
            string result = string.Empty;
            var config = jobConfiguration;
            RunStep(() => { result = recordingPortClient.CreateRecordingJob(ref config); },
                    "Create Recording Job");
            DoRequestDelay();
            jobConfiguration = config;
            return result;
        }

        public string CreateTrack(string recordingToken, TrackConfiguration config)
        {
            string result = string.Empty;
            RunStep(() => { result = recordingPortClient.CreateTrack(recordingToken, config); },
                    string.Format("Create track for recording with token = '{0}'", recordingToken));
            DoRequestDelay();
            return result;
        }

        protected void DeleteRecordingJob(string jobToken)
        {
            RunStep(() => recordingPortClient.DeleteRecordingJob(jobToken),
                    string.Format("Delete Recording Job (jobToken = '{0}')", jobToken));
            DoRequestDelay();
        }

        public void DeleteTrack(string recordingToken, string trackToken)
        {
            RunStep(() => { recordingPortClient.DeleteTrack(recordingToken, trackToken); },
                    string.Format("Delete track with token = '{0}' for recording with token = '{1}'", trackToken, recordingToken));
            DoRequestDelay();
        }

        protected RecordingServiceCapabilities GetServiceCapabilities()
        {
            RecordingServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = recordingPortClient.GetServiceCapabilities(); }, "Get Service Capabilities");
            DoRequestDelay();
            return capabilities;
        }

        protected RecordingJobStateInformation GetRecordingJobState(string token)
        {
            RecordingJobStateInformation stateInformation = null;
            RunStep(() => { stateInformation = recordingPortClient.GetRecordingJobState(token); },
                string.Format("Get Recording Job State (token = '{0}')", token));
            DoRequestDelay();
            return stateInformation;
        }

        protected RecordingConfiguration GetRecordingConfiguration(string token)
        {
            RecordingConfiguration response = null;
            RunStep(() => { response = recordingPortClient.GetRecordingConfiguration(token); },
                    string.Format("Get Recording Configuration (token = '{0}')", token));
            DoRequestDelay();
            return response;
        }

        protected TrackConfiguration GetTrackConfiguration(string recordingToken, string trackToken)
        {
            TrackConfiguration trackConfiguration = null;
            RunStep(() => { trackConfiguration = recordingPortClient.GetTrackConfiguration(recordingToken, trackToken); },
                    string.Format("Get track configuration (recording token = '{0}', track token = '{1}')", recordingToken, trackToken));
            DoRequestDelay();

            return trackConfiguration;
        }

        protected void SetRecordingConfiguration(string recordingToken, RecordingConfiguration recordingConfiguration)
        {
            SetRecordingConfiguration(recordingToken, recordingConfiguration, "Set Recording Configuration");
        }

        protected void SetRecordingConfiguration(string recordingToken, RecordingConfiguration recordingConfiguration, string stepName)
        {
            RunStep(() => { recordingPortClient.SetRecordingConfiguration(recordingToken, recordingConfiguration); }, stepName);
            DoRequestDelay();
        }

        protected RecordingJobConfiguration GetRecordingJobConfiguration(string token)
        {
            RecordingJobConfiguration configuration = null;
            RunStep(() => { configuration = recordingPortClient.GetRecordingJobConfiguration(token); },
                    string.Format("Get Recording Job Configuration (token = '{0}')", token));
            DoRequestDelay();
            return configuration;
        }

        protected void SetRecordingJobConfiguration(string jobToken, RecordingJobConfiguration config)
        {
            RunStep(() => { recordingPortClient.SetRecordingJobConfiguration(jobToken, ref config); },
                    string.Format("Set Recording Job Configuration (jobToken = '{0}')", jobToken));
            DoRequestDelay();
        }

        protected void SetRecordingJobMode(string jobToken, string mode)
        {
            RunStep(() => { recordingPortClient.SetRecordingJobMode(jobToken, mode); },
                    string.Format("Set Recording Job Mode (jobToken = '{0}') to '{1}'", jobToken, mode));
            DoRequestDelay();
        }

        protected void SetTrackConfiguration(string recordingToken, string trackToken, TrackConfiguration config)
        {
            SetTrackConfiguration(recordingToken, trackToken, config,
                                  string.Format("Set Track Configuration (recordingToken = '{0}', trackToken = '{1}')", recordingToken, trackToken));
        }

        protected void SetTrackConfiguration(string recordingToken, string trackToken, TrackConfiguration config, string stepName)
        {
            RunStep(() => { recordingPortClient.SetTrackConfiguration(recordingToken, trackToken, config); },
                    stepName);
            DoRequestDelay();
        }


        protected void DeleteRecording(string token)
        {
            DeleteRecording(token, string.Format("Delete recording '{0}'", token));
        }

        protected void DeleteRecording(string token, string stepName)
        {
            RunStep(() => { recordingPortClient.DeleteRecording(token); }, stepName);
            DoRequestDelay();
        }

        protected RecordingOptions GetRecordingOptions(string token)
        {
            RecordingOptions options = null;
            RunStep(() => { options = recordingPortClient.GetRecordingOptions(token); },
                string.Format("Get Recording Options (token = '{0}')", token));
            DoRequestDelay();
            return options;
        }

        #endregion

        #region Device Client
        private DeviceClient _deviceClient;
        DeviceClient DeviceClient
        {
            get
            {
                if (_deviceClient == null)
                {
                    Binding binding = CreateBinding(true, new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient = new DeviceClient(binding, new EndpointAddress(CameraAddress));
                    AttachSecurity(_deviceClient.Endpoint);
                    SetupChannel(_deviceClient.InnerChannel);
                }
                return _deviceClient;
            }

        }

        #endregion

        #region Media Client

        MediaClient _mediaClient;
        string _mediaAddress;

        protected MediaClient MediaClient
        {
            get
            {
                if (_mediaClient == null)
                {

                    BeginStep("Get Media service address");
                    _mediaAddress = DeviceClient.GetMediaServiceAddress(Features);
                    LogStepEvent(string.Format("Media service address: {0}", _mediaAddress));
                    if (string.IsNullOrEmpty(_mediaAddress))
                    {
                        throw new AssertException("Media service not supported");
                    }
                    else
                    {
                        if (!_mediaAddress.IsValidUrl())
                        {
                            throw new AssertException("Media service address is invalid");
                        }
                    }

                    StepPassed();

                    Binding binding = CreateBinding(
                        false,
                        new IChannelController[] { new SoapValidator(MediaSchemasSet.GetInstance()) });
                    _mediaClient = new MediaClient(binding, new EndpointAddress(_mediaAddress));
                    AttachSecurity(_mediaClient.Endpoint);

                }

                return _mediaClient;
            }
        }

        protected Profile[] GetProfiles()
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetProfiles(this, client);
        }

        protected Profile GetProfile(string token)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetProfile(this, client, token);
        }

        /// <summary>
        /// Retrieves lists of video encoder configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of video encoder configurations</returns>
        protected VideoEncoderConfiguration[] GetCompatibleVideoEncoderConfigurations(string profile)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetCompatibleVideoEncoderConfigurations(this, client, profile);
        }

        protected VideoSourceConfiguration[] GetCompatibleVideoSourceConfigurations(string profile)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetCompatibleVideoSourceConfigurations(this, client, profile);
        }

        /// <summary>
        /// Adds video encoder configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddVideoEncoderConfiguration(string profile, string configuration)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.AddVideoEncoderConfiguration(this, client, profile, configuration);
        }

        /// <summary>
        /// Removes video encoder configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemoveVideoEncoderConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveVideoEncoderConfiguration(this, client, profile);
        }

        protected void RemoveVideoSourceConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveVideoSourceConfiguration(this, client, profile);
        }

        protected void RemoveAudioEncoderConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveAudioEncoderConfiguration(this, client, profile);
        }

        protected void RemoveAudioSourceConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveAudioSourceConfiguration(this, client, profile);
        }

        protected VideoEncoderConfigurationOptions GetVideoEncoderConfigurationOptions(string configuration, string profile)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetVideoEncoderConfigurationOptions(this, client, configuration, profile);
        }

        protected void SetVideoEncoderConfiguration(VideoEncoderConfiguration config)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.SetVideoEncoderConfiguration(this, client, config, true);
        }

        #endregion

        #region Create recording

        protected Event.EndpointReferenceType CreatePullPointSubscription(TopicInfo topicInfo,
            out System.DateTime subscribed)
        {
            // Get event service address

            BeginStep("Connect to Event service");
            string eventServiceAddress = DeviceClient.GetEventServiceAddress(Features);
            LogStepEvent(string.Format("Event service address: {0}", eventServiceAddress));
            if (!eventServiceAddress.IsValidUrl())
            {
                throw new AssertException("Event service address is invalid");
            }
            StepPassed();

            // create EventPortTypeClient 
            Binding eventServiceBinding = CreateEventServiceBinding(eventServiceAddress);

            Event.EventPortTypeClient eventPortTypeClient = new Event.EventPortTypeClient(eventServiceBinding, new EndpointAddress(eventServiceAddress));

            System.Net.ServicePointManager.Expect100Continue = false;

            AttachSecurity(eventPortTypeClient.Endpoint);

            SetupChannel(eventPortTypeClient.InnerChannel);

            // Create filter from TopicInfo

            Event.FilterType filter = CreateFilter(topicInfo);

            // Create subscription
            int subscriptionTimeout = 60; // from specification

            string terminationTimeString = string.Format("PT{0}S", subscriptionTimeout);

            XmlElement[] any = null;
            System.DateTime currentTime = System.DateTime.MinValue;
            System.DateTime? terminationTime = null;

            Event.EndpointReferenceType subscription = null;

            try
            {
                RunStep(() =>
                {
                    subscription = eventPortTypeClient.CreatePullPointSubscription(
                        filter,
                        terminationTimeString,
                        null,
                        ref any,
                        out currentTime,
                        out terminationTime);
                },
                 "Create Pull Point Subscription");
            }
            catch (Exception exc)
            {
                throw;
            }
            finally
            {
                eventPortTypeClient.Close();
            }
            // Validate Subscription
            subscribed = System.DateTime.Now;

            Utils.EventServiceUtils.ValidateSubscription(terminationTime,
                currentTime,
                subscriptionTimeout,
                subscription,
                Assert);

            return subscription;
        }

        /// <summary>
        /// Delegate definition for GetMessages.
        /// </summary>
        /// <returns></returns>
        private delegate System.DateTime PullMessageDelegate(ref System.DateTime localCurrentTime,
            ref System.DateTime localTerminationTime);

        private class PullMessagesData
        {
            public Event.NotificationMessageHolderType[] NotificationMessages;
            public string PullMessagesResponseData;
        }

        protected Dictionary<Event.NotificationMessageHolderType, string> GetMessages(
            Event.EndpointReferenceType subscription, System.DateTime operationTime)
        {
            return GetMessages(subscription, operationTime, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="operationTime">UTC time</param>
        /// <param name="exitCheck"></param>
        /// <returns></returns>
        protected Dictionary<Event.NotificationMessageHolderType, string> GetMessages(
            Event.EndpointReferenceType subscription, System.DateTime operationTime,
            Func<Event.NotificationMessageHolderType, bool> exitCheck)
        {
            Event.PullPointSubscriptionClient pullPointClient = null;

            try
            {
                // Create new service client to pass "local" traffic listener
                IChannelController[] controllers;

                EndpointAddress address = new EndpointAddress(subscription.Address.Value);
                EndpointController controller = new EndpointController(address);
                WsaController wsaController = new WsaController();

                controllers = new IChannelController[]
                              {
                                  _trafficListener, 
                                  controller, 
                                  wsaController,
                                  _semaphore, 
                                  _credentialsProvider,
                                  new SoapValidator(EventsSchemasSet.GetInstance())
                              };
                Binding binding = CreateBinding(controllers);

                pullPointClient = new TestTool.Proxies.Event.PullPointSubscriptionClient(binding, new EndpointAddress(subscription.Address.Value));

                AttachSecurity(pullPointClient.Endpoint);
                SetupChannel(pullPointClient.InnerChannel);
                AttachAddressing(pullPointClient.Endpoint, subscription);

                // from spec
                int messagesLimit = 1;
                string timeString = "PT60S";
                //string timeString = "PT20S";

                // Total list of notifications
                Dictionary<Event.NotificationMessageHolderType, string> totalMessagesList =
                    new Dictionary<TestTool.Proxies.Event.NotificationMessageHolderType, string>();


                PullMessagesData pullMessagesData = null;

                AutoResetEvent requestSentErrorEvent = new AutoResetEvent(false);

                // initialize delegate
                PullMessageDelegate del =
                    new PullMessageDelegate(
                        (ref System.DateTime localCurrentTime, ref System.DateTime localTerminationTime) =>
                        {
                            Event.NotificationMessageHolderType[] notificationMessageCopy = null;
                            System.DateTime terminationTimeCopy = System.DateTime.MinValue;
                            System.DateTime result = System.DateTime.MinValue;

                            try
                            {
                                result = pullPointClient.PullMessages(timeString,
                                                                           messagesLimit,
                                                                           null,
                                                                           out terminationTimeCopy,
                                                                           out notificationMessageCopy);
                            }
                            catch (System.Net.Sockets.SocketException exc)
                            {
                                if (InStep)
                                {
                                    StepFailed(exc);
                                }
                                requestSentErrorEvent.Set();
                            }
                            localTerminationTime = terminationTimeCopy.ToUniversalTime();
                            pullMessagesData.NotificationMessages = notificationMessageCopy;
                            localCurrentTime = result.ToUniversalTime();

                            return localCurrentTime;
                        });


                // create event handler to save response
                _trafficListener.ResponseReceived += new Action<string>((data) =>
                {
                    pullMessagesData.PullMessagesResponseData = data;
                });

                System.DateTime eventLastTime = operationTime.AddSeconds(_operationDelay / 1000).ToUniversalTime();

                while (true)
                {
                    pullMessagesData = new PullMessagesData();
                    System.DateTime dutCurrentTime = GetMessages(messagesLimit, pullMessagesData, del);


                    BeginStep("Check that more PullMessages requests are needed");
                    if (pullMessagesData.NotificationMessages.Length == 0)
                    {
                        if (dutCurrentTime > eventLastTime)
                        {
                            LogStepEvent("Allowed interval for event generation is expired, stop getting notifications");
                            StepPassed();
                            break;
                        }
                    }
                    else
                    {
                        TestTool.Proxies.Event.NotificationMessageHolderType message = pullMessagesData.NotificationMessages[0];

                        string utcTimeValue = message.Message.Attributes[OnvifMessage.UTCTIMEATTRIBUTE].Value;
                        // xs:dateTime string

                        System.DateTime messageTime = XmlConvert.ToDateTime(utcTimeValue, XmlDateTimeSerializationMode.Utc);

                        if (messageTime > eventLastTime)
                        {
                            LogStepEvent("Last message received is out of interval of interest, stop getting messages");
                            StepPassed();
                            break;
                        }

                        string rawSoapPacket = Utils.EventServiceUtils.GetSoapPacket(pullMessagesData.PullMessagesResponseData);

                        if (exitCheck != null)
                        {
                            bool messageFound = exitCheck(message);
                            if (messageFound)
                            {
                                LogStepEvent("Expected message found, stop getting results with PullMessages");

                                foreach (TestTool.Proxies.Event.NotificationMessageHolderType m in pullMessagesData.NotificationMessages)
                                {
                                    totalMessagesList.Add(m, rawSoapPacket);
                                }

                                StepPassed();
                                break;
                            }
                        }
                        else
                        {
                            foreach (TestTool.Proxies.Event.NotificationMessageHolderType m in pullMessagesData.NotificationMessages)
                            {
                                totalMessagesList.Add(m, rawSoapPacket);
                            }
                        }
                    }

                    StepPassed();
                }

                return totalMessagesList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                pullPointClient.Close();
            }
        }

        private System.DateTime GetMessages(int messagesLimit,
            PullMessagesData pullMessagesData,
            PullMessageDelegate del)
        {
            // declare parameters
            System.DateTime localTerminationTime = System.DateTime.MinValue;
            System.DateTime localCurrentTime = System.DateTime.MinValue;

            //
            // Send PullMessages request
            BeginStep("PullMessages");
            System.DateTime dateTime = del.Invoke(ref localCurrentTime, ref localTerminationTime);
            localCurrentTime = dateTime;
            StepPassed();
            //                

            Assert(localCurrentTime < localTerminationTime,
                "TerminationTime <= CurrentTime",
                "Validate CurrentTime and TerminationTime", null);

            Assert(pullMessagesData.NotificationMessages.Length <= messagesLimit,
                "Maximum number of messages exceeded",
                string.Format("Check that a maximum number of {0} Notification Messages is included in PullMessagesResponse", messagesLimit),
                null);

            return localCurrentTime;
        }

        /// <summary>
        /// Creates filter element for Subscribe request.
        /// </summary>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="messageDescription">Message description</param>
        /// <returns></returns>
        TestTool.Proxies.Event.FilterType CreateFilter(TopicInfo topicInfo)
        {
            Event.FilterType filter = new Event.FilterType();

            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();

            string topicPath = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

            filterTopicElement.InnerText = topicPath;

            filter.Any = new XmlElement[] { filterTopicElement };

            return filter;
        }

        #endregion

        #region Select recording for track creation

        // A.14: copy paste from \TestSuites\Recording\RecordingControlRecordingTestSuite.utils.cs
        // all changes made there should be inserted here and vise versa
        protected void GetRecordingForTrackCreation(out string recordingToken,
                                                                                            out string trackType,
                                                                                            out bool recordingCreated,
                                                                                            out bool trackDeleted,
                                                                                            out TrackConfiguration deletedTrackConf)
        {
            recordingToken = string.Empty;
            trackType = string.Empty;

            recordingCreated = false;
            trackDeleted = false;

            deletedTrackConf = null;

            // get recordings
            GetRecordingsResponseItem[] recordings = GetRecordings();

            // in case of empty recordings list try to create recording
            if (recordings == null || recordings.Length == 0)
            {
                // check for DynamicRecordings capability
                bool dynamicRecordingSupported = false;
                if (Features.ContainsFeature(Feature.GetServices))
                {
                    RecordingServiceCapabilities capabilities = GetServiceCapabilities();
                    dynamicRecordingSupported = capabilities.DynamicRecordingsSpecified && capabilities.DynamicRecordings;
                }
                else
                {
                    TestTool.Proxies.Onvif.Capabilities capabilities = DeviceClient.GetCapabilities(null);

                    Assert(capabilities.Extension != null && capabilities.Extension.Recording != null,
                           "No Recording service capabilities found",
                           "Check if the DUT returned Recording service capabilities");

                    dynamicRecordingSupported = capabilities.Extension.Recording.DynamicRecordings;
                }

                Assert(dynamicRecordingSupported, "Can't create recording because DynamicRecordings isn't supported",
                                                                                  "Check for DynamicRecordings capability");

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

                // create recording
                recordingToken = CreateRecording(conf);

                recordingCreated = true;

                // refresh recordings after creation
                recordings = GetRecordings();
                Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");
            }

            bool spareTotal = false;

            // search for recording with possibility to create track in it
            SearchForSpareTrack(recordings, out recordingToken, out spareTotal, out trackType);

            bool noTracks = true;

            // if all recorings doesn't have SpareTotal > 0
            if (!spareTotal)
            {
                LogTestEvent(string.Format("There is no any spare track in any recording so we need to delete some track ...{0}", Environment.NewLine));

                for (int i = 0; i < recordings.Length; i++)
                {
                    recordingToken = recordings[i].RecordingToken;

                    if (recordings[i].Tracks.Track == null)
                        continue;

                    noTracks = false;

                    for (int j = 0; j < recordings[i].Tracks.Track.Length; j++)
                    {
                        try
                        {
                            DeleteTrack(recordingToken, recordings[i].Tracks.Track[j].TrackToken);
                            deletedTrackConf = recordings[i].Tracks.Track[j].Configuration;
                            trackDeleted = true;
                            break;
                        }
                        catch (FaultException fault)
                        {
                            if (fault.IsValidOnvifFault("Receiver/Action/CannotDelete"))
                            {
                                StepPassed();
                                continue;
                            }
                        }
                    }

                    if (trackDeleted)
                        break;
                }

                Assert(!noTracks, "There is no any tracks in any recording for deletion", "Check that track for deletion was found");

                Assert(!(!spareTotal && !trackDeleted), "Can't delete any track from any existing recording", "Check that track was deleted successfully");

                // refresh recordings after deletion
                recordings = GetRecordings();
                Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");

                // search for recording with possibility to create track in it after DeleteTrack
                SearchForSpareTrack(recordings, out recordingToken, out spareTotal, out trackType);

                Assert(spareTotal, "Can't find recording with spare track after track deletion", "Search for recording with spare track");

            }
        }

        // search for recording with possibility to create track in it
        private void SearchForSpareTrack(GetRecordingsResponseItem[] recordings,
                                         out string recordingToken,
                                         out bool spareTotal,
                                         out string trackType)
        {
            recordingToken = string.Empty;
            trackType = string.Empty;
            spareTotal = false;

            //var ro = new RecordingOptions(){ Job = new JobOptions(){ Spare = 1, SpareSpecified = true }, Track = new TrackOptions() { SpareTotal = 1, SpareTotalSpecified = true, SpareAudio = 1, SpareAudioSpecified = true } };
            //var s = new XmlSerializer(ro.GetType());

            //var dst = new StringBuilder();
            //s.Serialize(new StringWriter(dst), ro);

            //LogStepEvent(dst.ToString());

            for (int i = 0; i < recordings.Length; i++)
            {
                recordingToken = recordings[i].RecordingToken;
                RecordingOptions recOptions = GetRecordingOptions(recordingToken);

                if (recOptions.Track.SpareTotal > 0)
                {
                    spareTotal = true;

                    if (recOptions.Track.SpareVideo > 0)
                    {
                        trackType = "Video";
                    }
                    else if (recOptions.Track.SpareAudio > 0)
                    {
                        trackType = "Audio";
                    }
                    else if (recOptions.Track.SpareMetadata > 0)
                    {
                        trackType = "Metadata";
                    }
                    else
                    {
                        Assert(!(recOptions.Track.SpareTotal > 0 && recOptions.Track.SpareVideo + recOptions.Track.SpareAudio + recOptions.Track.SpareMetadata == 0),
                               String.Format("There should be any Spare Video, or Audio, or Metadata tracks because Total Spare tracks number is {0} (RecordingToken = {1})", recOptions.Track.SpareTotal, recordingToken),
                               "Check for spare tracks correctness");
                    }

                    break;
                }
            }
        }

        #endregion

        #region A.12 - Selection or Creation of Recording for recording job creation

        // A.12: copy paste from \TestSuites\Recording\RecordingControlRecordingTestSuite.utils.cs
        // all changes made there should be inserted here and vise versa
        protected void GetRecordingForJobCreation(out string recordingToken, out bool recordingCreated, int jobsNumber)
        {
            recordingToken = string.Empty;
            recordingCreated = false;

            bool recordingFound = false;

            // check for DynamicRecordings capability
            bool dynamicRecordingSupported = false;
            DynamicRecordingsSupported(ref dynamicRecordingSupported);

            // dynamic recording supported
            if (dynamicRecordingSupported)
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

                // create recording
                try
                {
                    recordingToken = CreateRecording(conf);
                    recordingCreated = true;
                }
                catch (FaultException e)
                {
                    StepPassed();
                    FindExistingRecording(ref recordingToken, ref recordingFound, ref recordingCreated, jobsNumber);
                    return;
                }

                // get recording options
                RecordingOptions recOptions = GetRecordingOptions(recordingToken);

                // if there is 2 spare jobs then finish annex
                if (recOptions.Job.Spare > jobsNumber)
                    return;
                // else delete recording jobs
                else
                {
                    GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                    if (jobsNumber == 0)
                        Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                    // there are no recording jobs for deletion, pass test
                    else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                    {
                        LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        recordingToken = string.Empty;
                        recordingCreated = false;
                        return;
                    }

                    if (recordingJobs != null && recordingJobs.Length != 0)
                    {
                        foreach (var item in recordingJobs)
                        {
                            DeleteRecordingJob(item.JobToken);
                            recOptions = GetRecordingOptions(recordingToken);
                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                recordingFound = true;
                                break;
                            }
                        }

                        // pass test, if after deletion of all jobs there is no Job.Spare > 1
                        // fail test, if after deletion of all jobs there is no Job.Spare > 0
                        if (!recordingFound)
                        {
                            if (jobsNumber == 1)
                            {
                                LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                    DeleteRecording(recordingToken);

                                recordingToken = string.Empty;
                                recordingCreated = false;
                                return;
                            }

                            if (jobsNumber == 0)
                            {
                                Assert(recordingFound,
                                    "There wasn't found any recording with possibility to create 1 recording job",
                                    "Analyzing for possibility to create 1 recording job");
                            }
                        }
                    }
                }
            }
            // dynamic recording isn't supported
            else
            {
                FindExistingRecording(ref recordingToken, ref recordingFound, ref recordingCreated, jobsNumber);
            }
        }

        private void DynamicRecordingsSupported(ref bool dynamicRecordingSupported)
        {
            // check for DynamicRecordings capability
            dynamicRecordingSupported = false;
            if (Features.ContainsFeature(Feature.GetServices))
            {
                RecordingServiceCapabilities capabilities = GetServiceCapabilities();
                dynamicRecordingSupported = capabilities.DynamicRecordingsSpecified && capabilities.DynamicRecordings;
            }
            else
            {
                TestTool.Proxies.Onvif.Capabilities capabilities = DeviceClient.GetCapabilities(null);

                Assert(capabilities.Extension != null && capabilities.Extension.Recording != null,
                        "No Recording service capabilities found",
                        "Check if the DUT returned Recording service capabilities");

                dynamicRecordingSupported = capabilities.Extension.Recording.DynamicRecordings;
            }
        }

        private void FindExistingRecording(ref string recordingToken, ref bool recordingFound, ref bool recordingCreated, int jobsNumber)
        {
            GetRecordingsResponseItem[] recordings = GetRecordings();
            Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");

            // create recording list with video tracks
            List<GetRecordingsResponseItem> recordingsVideoTrack = new List<GetRecordingsResponseItem>();
            foreach (var rec in recordings)
            {
                if (rec.Tracks.Track != null && rec.Tracks.Track.Length != 0)
                {
                    foreach (var track in rec.Tracks.Track)
                    {
                        if (track.Configuration.TrackType == TrackType.Video)
                            recordingsVideoTrack.Add(rec);
                    }
                }
            }

            Assert(recordingsVideoTrack != null && recordingsVideoTrack.Count != 0,
                          "There are no recordings with video track. Please, configure recording with video track manually and rerun test",
                          "Search for recordings with video track");

            foreach (var item in recordingsVideoTrack)
            {
                RecordingOptions recOptions = GetRecordingOptions(item.RecordingToken);
                if (recOptions.Job.Spare > jobsNumber)
                {
                    recordingFound = true;
                    recordingToken = item.RecordingToken;
                    break;
                }
            }

            // if recording wasn't found then delete recording jobs
            if (!recordingFound)
            {
                GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                if (jobsNumber == 0)
                    Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                {
                    LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                    if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                        DeleteRecording(recordingToken);

                    recordingToken = string.Empty;
                    recordingCreated = false;
                    return;
                }

                if (recordingJobs != null && recordingJobs.Length != 0)
                {
                    foreach (var job in recordingJobs)
                    {
                        DeleteRecordingJob(job.JobToken);
                        foreach (var rec in recordingsVideoTrack)
                        {
                            RecordingOptions recOptions = GetRecordingOptions(rec.RecordingToken);
                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                recordingToken = rec.RecordingToken;
                                recordingFound = true;
                                break;
                            }
                        }

                        if (recordingFound)
                            break;
                    }

                    // pass test if after deletion of all jobs there is no Job.Spare > 1
                    // fail test, if after deletion of all jobs there is no Job.Spare > 0
                    if (!recordingFound)
                    {
                        if (jobsNumber == 1)
                        {
                            LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                            if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                DeleteRecording(recordingToken);

                            recordingToken = string.Empty;
                            recordingCreated = false;
                            return;
                        }

                        if (jobsNumber == 0)
                        {
                            Assert(recordingFound,
                                "There wasn't found any recording with possibility to create 1 recording job",
                                "Analyzing for possibility to create 1 recording job");
                        }
                    }
                }
            }
        }

        #endregion

        #region A.15 - Selection or Creation of Recording for recording job creation on a Media profile

        // A.15: copy paste from \TestSuites\Recording\RecordingControlRecordingTestSuite.utils.cs
        // all changes here should be copied there and vise versa
        protected void GetRecordingForJobCreationMediaProfile(out string recordingToken, out string profileToken,
                                                                                                               out bool recordingCreated, int jobsNumber)
        {
            recordingToken = string.Empty;
            profileToken = string.Empty;
            recordingCreated = false;

            bool recordingFound = false;

            // check for DynamicRecordings capability
            bool dynamicRecordingSupported = false;
            DynamicRecordingsSupported(ref dynamicRecordingSupported);

            // dynamic recording supported
            if (dynamicRecordingSupported)
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

                // create recording
                try
                {
                    recordingToken = CreateRecording(conf);
                    recordingCreated = true;
                }
                catch (FaultException e)
                {
                    StepPassed();

                    FindExistingRecording2(ref recordingToken, ref profileToken,
                                                                ref recordingFound, ref recordingCreated, jobsNumber);
                    return;
                }

                // get recording options
                RecordingOptions recOptions = GetRecordingOptions(recordingToken);
                Assert(recOptions.Job.CompatibleSources != null && recOptions.Job.CompatibleSources.Length != 0,
                             "Compatible sources list is empty", "Compatible sources list is not empty");


                if (recOptions.Job.Spare > jobsNumber)
                {
                    profileToken = recOptions.Job.CompatibleSources[0];
                    return;
                }
                // else delete recording jobs
                else
                {
                    GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                    if (jobsNumber == 0)
                        Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                    // there are no recording jobs for deletion, pass test
                    else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                    {
                        LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        recordingToken = string.Empty;
                        profileToken = string.Empty;
                        recordingCreated = false;
                        return;
                    }

                    if (recordingJobs != null && recordingJobs.Length != 0)
                    {
                        foreach (var item in recordingJobs)
                        {
                            DeleteRecordingJob(item.JobToken);
                            recOptions = GetRecordingOptions(recordingToken);
                            Assert(recOptions.Job.CompatibleSources != null && recOptions.Job.CompatibleSources.Length != 0,
                                         "Compatible sources list is empty", "Check that compatible sources list is not empty");
                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                recordingFound = true;
                                profileToken = recOptions.Job.CompatibleSources[0];
                                break;
                            }
                        }

                        // pass test, if after deletion of all jobs there is no Job.Spare > 1
                        // fail test, if after deletion of all jobs there is no Job.Spare > 0
                        if (!recordingFound)
                        {
                            if (jobsNumber == 1)
                            {
                                LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                    DeleteRecording(recordingToken);

                                recordingToken = string.Empty;
                                profileToken = string.Empty;
                                recordingCreated = false;
                                return;
                            }

                            if (jobsNumber == 0)
                            {
                                Assert(recordingFound,
                                    "There wasn't found any recording with possibility to create 1 recording job",
                                    "Analyzing for possibility to create 1 recording job");
                            }
                        }
                    }
                }
            }
            else
            {
                FindExistingRecording2(ref recordingToken, ref profileToken,
                                                            ref recordingFound, ref recordingCreated, jobsNumber);
            }
        }

        private void FindExistingRecording2(ref string recordingToken, ref string profileToken,
                                                                           ref bool recordingFound, ref bool recordingCreated, int jobsNumber)
        {

            bool emptyCompatibleSources = true;

            GetRecordingsResponseItem[] recordings = GetRecordings();
            Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");

            // create dictionary with recordings which have not empy compatible sources
            Dictionary<string, string> compatibleSources = new Dictionary<string, string>();
            foreach (var rec in recordings)
            {
                RecordingOptions recOptions = GetRecordingOptions(rec.RecordingToken);
                if (recOptions.Job.CompatibleSources != null && recOptions.Job.CompatibleSources.Length != 0)
                {
                    emptyCompatibleSources = false;
                    compatibleSources.Add(rec.RecordingToken, recOptions.Job.CompatibleSources[0]);
                }
            }

            // fail test if there are no any recording with not empty compatible sources list
            Assert(!emptyCompatibleSources && (compatibleSources.Count != 0),
              string.Format("There wasn't found any recording with not empty compatible sources list{0}", Environment.NewLine),
              "Search for recordings with not empty compatible sources list");

            foreach (var compatibleSource in compatibleSources)
            {
                RecordingOptions recOptions = GetRecordingOptions(compatibleSource.Key);
                if (recOptions.Job.Spare > jobsNumber)
                {
                    recordingFound = true;
                    recordingToken = compatibleSource.Key;
                    profileToken = compatibleSource.Value;
                    break;
                }
            }

            // if recording wasn't found then delete recording jobs
            if (!recordingFound)
            {
                GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                if (jobsNumber == 0)
                    Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                {
                    LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                    if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                        DeleteRecording(recordingToken);

                    recordingToken = string.Empty;
                    recordingCreated = false;
                    return;
                }

                if (recordingJobs != null && recordingJobs.Length != 0)
                {
                    foreach (var job in recordingJobs)
                    {
                        DeleteRecordingJob(job.JobToken);
                        foreach (var compatibleSource in compatibleSources)
                        {
                            RecordingOptions recOptions = GetRecordingOptions(compatibleSource.Key);
                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                recordingToken = compatibleSource.Key;
                                profileToken = compatibleSource.Value;
                                recordingFound = true;
                                break;
                            }
                        }

                        if (recordingFound)
                            break;
                    }

                    // pass test, if after deletion of all jobs there is no Job.Spare > 1
                    // fail test, if after deletion of all jobs there is no Job.Spare > 0
                    if (!recordingFound)
                    {
                        if (jobsNumber == 1)
                        {
                            LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                            if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                DeleteRecording(recordingToken);

                            recordingToken = string.Empty;
                            profileToken = string.Empty;
                            recordingCreated = false;
                            return;
                        }

                        if (jobsNumber == 0)
                        {
                            Assert(recordingFound,
                                "There wasn't found any recording with possibility to create 1 recording job",
                                "Analyzing for possibility to create 1 recording job");
                        }
                    }
                }
            }
        }

        #endregion

        #region Validate messages


        /// <summary>
        /// This is test-specific. I.E. this method checks presence of exactly ONE element item,
        /// as specified for the topic of interest!
        /// </summary>
        /// <param name="messageElement"></param>
        /// <returns></returns>
        XmlElement ValidateElementItem(XmlElement messageElement, string elementItemName, StringBuilder logger)
        {
            List<XmlElement> elementItems = BaseNotificationUtils.GetMessageElementItems("Data", messageElement);

            XmlElement infoElement = null;
            {
                bool ok = true;
                if (elementItems.Count == 0)
                {
                    ok = false;
                    logger.AppendLine("Notification message does not contain any ElementItems");
                }
                else if (elementItems.Count > 1)
                {
                    ok = false;
                    logger.AppendLine("Notification message contains more than one ElementItems");
                }
                else
                {
                    XmlElement elementItem = elementItems[0];
                    if (elementItem.ChildNodes.OfType<XmlElement>().Count() != 1)
                    {
                        ok = false;
                        // Core spec, p. 9.5.2
                        // In the case of an ElementItem, the value is expressed by one XML element within the ElementItem element.
                        logger.AppendLine("Element item should contain only one XML Element inside");
                    }
                    else
                    {
                        var nameAttribute = elementItem.HasAttribute("Name") ? elementItem.Attributes["Name"] : null;

                        if (null == nameAttribute || nameAttribute.Value != elementItemName)
                        {
                            ok = false;

                            if (null == nameAttribute)
                                logger.AppendLine("Element item should contain attribute 'Name'");
                            else
                                logger.AppendLine(string.Format("Element item with unexpected name is received. Expected: '{0}'. Actual: '{1}'",
                                                                elementItemName, nameAttribute.Value));
                        }
                        else
                        {
                            infoElement = elementItem.ChildNodes.OfType<XmlElement>().First();
                        }
                    }
                }
                if (!ok) 
                    throw new AssertException(logger.ToStringTrimNewLine());
            }

            return infoElement;
        }

        void ValidateElementItemContent(XmlElement element, string name, string ns, StringBuilder logger)
        {
            {
                // validator 
                XmlElementValidator validator = null;

                bool hasErrors = false;

                if (element.LocalName != name ||
                    element.NamespaceURI.ToLower() != ns.ToLower())
                {
                    hasErrors = true;

                    logger.AppendFormat("Content of ElementItem element is incorrect: expected '{0}' from '{1}' namespace, actual '{2}' from '{3}' namespace",
                                        name, ns, element.LocalName, element.NamespaceURI);
                }
                else
                {
                    // schema validation will be performed automatically only for Device service
                    BaseSchemaSet schemaSet = TypesSchemaSet.GetInstance();
                    validator = new XmlElementValidator(schemaSet);

                    //validate
                    string error = string.Empty;
                    try
                    {
                        validator.Validate(element);
                    }
                    catch (Exception exc)
                    {
                        hasErrors = true;
                        error = exc.Message;

                        logger.AppendFormat("Content of ElementItem element is not valid according to the schema: '{0}'", error);
                    }
                }
                string errDump = logger.ToStringTrimNewLine();
                if (hasErrors)
                    throw new AssertException(errDump);
            }
        }

        protected T ExtractElementItemsContent<T>(XmlElement element, string name, string ns)
        {
            BeginStep("Parse ElementItem content");

            System.Xml.Serialization.XmlRootAttribute xRoot = new System.Xml.Serialization.XmlRootAttribute();
            xRoot.ElementName = name;
            xRoot.IsNullable = true;
            xRoot.Namespace = ns;

            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);

            XmlReader reader = new XmlNodeReader(element);

            T capabilities;
            try
            {
                capabilities = (T)serializer.Deserialize(reader);
            }
            catch (Exception exc)
            {
                string message;
                if (exc.InnerException != null)
                {
                    message = string.Format("{0} {1}", exc.Message, exc.InnerException.Message);
                }
                else
                {
                    message = exc.Message;
                }
                throw new ApplicationException(message);
            }
            StepPassed();
            return capabilities;
        }

        #endregion

        private void CompareRecordingJobStates(string jobToken, RecordingJobStateInformation jobInfoFromNotification, RecordingJobStateInformation jobInfoSecond)
        {
            StringBuilder logger = new StringBuilder();
            bool ok = true;

            if (jobInfoFromNotification.RecordingToken != jobInfoSecond.RecordingToken)
            {
                ok = false;
                logger.AppendLine(string.Format("   Job tokens are different: {0} in received notification, {1} in GetRecordingJobStateResponse",
                                                jobInfoFromNotification.RecordingToken, jobInfoSecond.RecordingToken));
            }

            if (jobInfoFromNotification.State != jobInfoSecond.State)
            {
                ok = false;
                logger.AppendLine(string.Format("   State's values are different: {0} in received notification, {1} in GetRecordingJobStateResponse",
                                                jobInfoFromNotification.State, jobInfoSecond.State));
            }

            if (jobInfoFromNotification.Sources != null && jobInfoSecond.Sources != null)
            {
                var common = new Dictionary<RecordingJobStateSource, RecordingJobStateSource>();
                ok = ok & CompareRecordingJobStateSourceLists(jobInfoFromNotification.Sources, jobInfoSecond.Sources, common, 
                                                              " notification",
                                                              " GetRecordingJobState",
                                                              logger);

                ok = ok && CompareJobStateSources(common, logger);
            }
            else
            {
                if (jobInfoFromNotification.Sources == null && jobInfoSecond.Sources != null)
                {
                    ok = false;
                    logger.AppendLine(string.Format("   Notification for job with token = '{0}' doesn't contain Source list", jobToken));
                }
                if (jobInfoFromNotification.Sources != null && jobInfoSecond.Sources == null)
                {
                    ok = false;
                    logger.AppendLine(string.Format("   Job with token = '{0}' in GetRecordingJobStateResponse doesn't contain Source list", jobToken));
                }
            }

            Assert(ok, logger.ToStringTrimNewLine(), string.Format("Validate RecordingJobStateResponse(JobToken = '{0}')", jobToken));
        }


        #region RTSP simulator

        void StopSimulator(RTSPSimulator sim)
        {
            RunStep(() => { sim.StopRTSP(); }, "Stop simulator");
        }

        string GetCurrentDirectory()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);
            return path;
        }

        const string STREAM1 = "Stream1";
        const string STREAM2 = "Stream2";

        string GetStreamName(RTSPSimulator.Codecs codec, string streamName)
        {
            return string.Format("{0}_{1}", codec, streamName);
        }

        #endregion

        #region Pulling Condition
        public class WaitNotificationsForAllJobsPollingCondition: SubscriptionHandler.PollingConditionBase
        {
            public WaitNotificationsForAllJobsPollingCondition(int timeout, IEnumerable<string> waitingNotificationsFor): base(timeout)
            {
                m_WaitingNotificationsFor = new HashSet<string>(waitingNotificationsFor);
            }

            public override bool StopPulling
            {
                get { return !m_WaitingNotificationsFor.Any(); }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        var tokens = string.Join(", ", m_WaitingNotificationsFor.Select(e => "'" + e + "'").ToArray()).Trim(new[] { ' ', ',' });
                        if (m_WaitingNotificationsFor.Count() > 1)
                            log.AppendFormat("No notifications for recording jobs with tokens {0}", tokens);
                        else
                            log.AppendFormat("No notification for recording job with token {0}", tokens);

                        return log.ToString();
                    }
                    else
                        return "Notifications for all jobs are received";
                }
            }

            public override void Update(Dictionary<Event.NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (var msg in messages.Keys)
                    {
                        string recordingToken = null;
                        if (null != msg.Message.GetMessageSourceSimpleItems()
                            && msg.Message.GetMessageSourceSimpleItems().ContainsKey("RecordingJobToken"))
                            recordingToken = msg.Message.GetMessageSourceSimpleItems()["RecordingJobToken"];
                        if (null != recordingToken)
                            m_WaitingNotificationsFor.RemoveWhere(q => q == recordingToken);
                    }
            }

            private readonly HashSet<string> m_WaitingNotificationsFor;
        }

        public class WaitNotificationsForAllRecordingsPollingCondition : SubscriptionHandler.PollingConditionBase
        {
            public WaitNotificationsForAllRecordingsPollingCondition(int timeout, IEnumerable<string> waitingNotificationsFor): base(timeout)
            {
                m_WaitingNotificationsFor = new HashSet<string>(waitingNotificationsFor);
            }

            public override bool StopPulling
            {
                get { return !m_WaitingNotificationsFor.Any(); }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        var tokens = string.Join(", ", m_WaitingNotificationsFor.Select(e => "'" + e + "'").ToArray()).Trim(new[] { ' ', ',' });
                        if (m_WaitingNotificationsFor.Count() > 1)
                            log.AppendFormat("No notifications for recordings with tokens {0}", tokens);
                        else
                            log.AppendFormat("No notification for recording with token {0}", tokens);

                        return log.ToString();
                    }
                    else
                        return "Notifications for all recordings are received";
                }
            }

            public override void Update(Dictionary<Event.NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (var msg in messages.Keys)
                    {
                        string recordingToken = null;
                        if (null != msg.Message.GetMessageSourceSimpleItems()
                            && msg.Message.GetMessageSourceSimpleItems().ContainsKey("RecordingToken"))
                            recordingToken = msg.Message.GetMessageSourceSimpleItems()["RecordingToken"];
                        if (null != recordingToken)
                            m_WaitingNotificationsFor.RemoveWhere(q => q == recordingToken);
                    }
            }

            private readonly HashSet<string> m_WaitingNotificationsFor;
        }

        public class WaitNotificationsForAllTracksPollingCondition : SubscriptionHandler.PollingConditionBase
        {
            public WaitNotificationsForAllTracksPollingCondition(int timeout, IEnumerable<KeyValuePair<string, string>> waitingNotificationsFor): base(timeout)
            {
                m_WaitingNotificationsFor = new HashSet<KeyValuePair<string, string>>(waitingNotificationsFor);
            }

            public override bool StopPulling
            {
                get { return !m_WaitingNotificationsFor.Any(); }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        var tokens = string.Join(", ", m_WaitingNotificationsFor.
                                                        Select(e => string.Format("RecordingToken = '{0}', TrackToken = '{1}'", e.Key, e.Value)).
                                                        ToArray()).Trim(new[] { ' ', ',' });
                        if (m_WaitingNotificationsFor.Count() > 1)
                            log.AppendFormat("No notifications for the following tracks {0}", tokens);
                        else
                            log.AppendFormat("No notification for the following track {0}", tokens);

                        return log.ToString();
                    }
                    else
                        return "Notifications for all tracks are received";
                }
            }

            public override void Update(Dictionary<Event.NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (var msg in messages.Keys)
                    {
                        string recordingToken = null;
                        string trackToken = null;
                        if (null != msg.Message.GetMessageSourceSimpleItems()
                            && msg.Message.GetMessageSourceSimpleItems().ContainsKey("RecordingToken"))
                            recordingToken = msg.Message.GetMessageSourceSimpleItems()["RecordingToken"];

                        if (null != msg.Message.GetMessageSourceSimpleItems()
                            && msg.Message.GetMessageSourceSimpleItems().ContainsKey("TrackToken"))
                            trackToken = msg.Message.GetMessageSourceSimpleItems()["TrackToken"];
                        if (null != recordingToken && null != trackToken)
                            m_WaitingNotificationsFor.RemoveWhere(q => q.Key == recordingToken && q.Value == trackToken);
                    }
            }

            private readonly HashSet<KeyValuePair<string, string>> m_WaitingNotificationsFor;
        }
        #endregion

        void CompareConfigurations(RecordingConfiguration configuration1, RecordingConfiguration configuration2, string recordingToken)
        {
            BeginStep(string.Format("Compare Recording Configurations of recording with token = '{0}'", recordingToken));
            StringBuilder dump = new StringBuilder("Configurations are different" + Environment.NewLine);
            bool equal = StorageTestsUtils.CompareConfigurations(configuration1, configuration2, dump, "PullMessages", "GetRecordingConfiguration");

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Configurations don't match to each other");
            }

            StepPassed();
        }

        private void CompareTrackConfigurations(TrackConfiguration recordingTrackConfig, TrackConfiguration trackConfig)
        {
            var logger = new StringBuilder();
            bool ok = true;
            if (recordingTrackConfig.Description != trackConfig.Description)
            {
                ok = false;
                logger.Append(string.Format("Description are different", Environment.NewLine));
            }
            if (recordingTrackConfig.TrackType != trackConfig.TrackType)
            {
                ok = false;
                logger.Append(string.Format("Track types are different", Environment.NewLine));
            }
            Assert(ok, logger.ToStringTrimNewLine(), "Verify track configuration");
        }

        void CompareConfigurations(RecordingJobConfiguration configuration1,
                                   RecordingJobConfiguration configuration2, 
                                   string descr1, string descr2)
        {
            BeginStep("Compare Recording Job Configurations");
            StringBuilder dump = new StringBuilder("Configurations are different" + Environment.NewLine);
            bool equal = true;
            bool local;

            if (configuration1.Mode != configuration2.Mode)
            {
                equal = false;
                dump.AppendFormat("   Mode field is different{0}", Environment.NewLine);
            }

            if (configuration1.Priority != configuration2.Priority)
            {
                equal = false;
                dump.AppendFormat("   Priority field is different{0}", Environment.NewLine);
            }

            if (configuration1.RecordingToken != configuration2.RecordingToken)
            {
                equal = false;
                dump.AppendFormat("   RecordingToken field is different{0}", Environment.NewLine);
            }

            // Compare Source 

            local = true;
            RecordingJobSource[] source1 = configuration1.Source;
            RecordingJobSource[] source2 = configuration2.Source;

            if (source1 != null && source2 != null)
            {
                // get intersection;
                // compare items in intersection

                Dictionary<RecordingJobSource, RecordingJobSource> common = new Dictionary<RecordingJobSource, RecordingJobSource>();
                bool ok;

                ok = CompareRecordingJobSourceLists(source1, source2,
                    common, descr1, descr2, dump);

                // for common only
                bool ok1 = CompareJobSources(common, dump);

                local = ok && ok1;
            }
            else
            {
                string messageFormat = "   Source information is skipped when information is received via {0}" +
                                       Environment.NewLine;
                if (source1 == null && source2 != null)
                {
                    local = false;
                    dump.AppendFormat(messageFormat, descr1);
                }
                if (source2 == null && source1 != null)
                {
                    local = false;
                    dump.AppendFormat(messageFormat, descr2);
                }
                // both null is OK
            }

            equal = equal && local;

            // There is also Extensio field.
            // Extension contains only Any...

            // Dump total result 

            if (!equal)
            {
                throw new AssertException(dump.ToStringTrimNewLine());
            }

            StepPassed();

        }

        /// <summary>
        /// Checks that for all items in first list item with the same token is presented in 
        /// second list and vice versa.
        /// </summary>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <param name="common">List of common tokens (actually out parameters)</param>
        /// <param name="description1">Description of the first list</param>
        /// <param name="description2">Description of the second list</param>
        /// <param name="logger">Logger to append error description, if needed.</param>
        /// <returns>True if set of tokens is the same in both lists.</returns>
        bool CompareRecordingJobSourceLists(IEnumerable<RecordingJobSource> list1,
                                            IEnumerable<RecordingJobSource> list2,
                                            Dictionary<RecordingJobSource, RecordingJobSource> common,
                                            string description1,
                                            string description2,
                                            StringBuilder logger)
        {
            bool ok = true;

            foreach (RecordingJobSource info in list1)
            {
                if (info.SourceToken != null)
                {
                    string token = info.SourceToken.Token;
                    string type = info.SourceToken.Type;

                    RecordingJobSource[] foundItems =
                        list2.Where(
                            I =>
                            I.SourceToken != null &&
                            I.SourceToken.Token == token &&
                            I.SourceToken.Type == type).ToArray();

                    if (foundItems.Length == 0)
                    {
                        logger.AppendFormat(
                            "      RecordingJobSource with SourceToken '{0}', Type = '{1}' not found in list received from{2}{3}",
                            token, type, description2, Environment.NewLine);
                        ok = false;
                    }
                    else
                    {
                        common.Add(info, foundItems[0]);
                    }
                }
            }

            foreach (RecordingJobSource info in list2)
            {
                if (info.SourceToken != null)
                {
                    string token = info.SourceToken.Token;
                    string type = info.SourceToken.Type;

                    RecordingJobSource[] foundItems =
                        list1.Where(
                            I =>
                            I.SourceToken != null &&
                            I.SourceToken.Token == token &&
                            I.SourceToken.Type == type).ToArray();

                    if (foundItems.Length == 0)
                    {
                        logger.AppendFormat(
                            "      RecordingJobSource with SourceToken '{0}', Type = '{1}' not found in list received from{2}{3}",
                            token, type, description1, Environment.NewLine);
                        ok = false;
                    }
                }
            }


            return ok;
        }

        bool CompareRecordingJobStateSourceLists(IEnumerable<RecordingJobStateSource> list1,
                                                 IEnumerable<RecordingJobStateSource> list2,
                                                 Dictionary<RecordingJobStateSource, RecordingJobStateSource> common,
                                                 string description1,
                                                 string description2,
                                                 StringBuilder logger)
        {
            bool ok = true;

            foreach (RecordingJobStateSource info in list1)
            {
                if (info.SourceToken != null)
                {
                    string token = info.SourceToken.Token;
                    string type = info.SourceToken.Type;

                    RecordingJobStateSource[] foundItems = list2.Where(I =>
                                                                       I.SourceToken != null &&
                                                                       I.SourceToken.Token == token &&
                                                                       I.SourceToken.Type == type).ToArray();

                    if (foundItems.Length == 0)
                    {
                        logger.AppendLine(string.Format("      RecordingJobSource with SourceToken '{0}', Type = '{1}' not found in list received from {2}",
                                                        token, type, description2));
                        ok = false;
                    }
                    else
                    {
                        common.Add(info, foundItems[0]);
                    }
                }
            }

            foreach (RecordingJobStateSource info in list2)
            {
                if (info.SourceToken != null)
                {
                    string token = info.SourceToken.Token;
                    string type = info.SourceToken.Type;

                    RecordingJobStateSource[] foundItems = list1.Where(I =>
                                                                       I.SourceToken != null &&
                                                                       I.SourceToken.Token == token &&
                                                                       I.SourceToken.Type == type).ToArray();

                    if (foundItems.Length == 0)
                    {
                        logger.AppendLine(string.Format("      RecordingJobSource with SourceToken '{0}', Type = '{1}' not found in list received from {2}",
                                                        token, type, description1));
                        ok = false;
                    }
                }
            }


            return ok;
        }

        bool CompareJobSources(Dictionary<RecordingJobSource, 
                               RecordingJobSource> common, 
                               StringBuilder logger)
        {
            bool ok = true;

            foreach (RecordingJobSource info1 in common.Keys)
            {
                string token = info1.SourceToken.Token;
                string type = info1.SourceToken.Type;
                RecordingJobSource info2 = common[info1];

                StringBuilder dump =
                    new StringBuilder(string.Format("      Information for RecordingJobSource with token '{0}' (type '{1}') is different:{2}",
                        token, type, Environment.NewLine));

                bool localOk = CompareJobSourceInformation(info1, info2, dump);

                if (!localOk)
                {
                    logger.Append(dump.ToString());
                    ok = false;
                }
            }

            return ok;
        }


        bool CompareJobSourceInformation(RecordingJobSource source1, 
                                         RecordingJobSource source2, 
                                         StringBuilder logger)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            // AutoCreateReceiver is not specified, if information is valid.

            // Extension - SKIP ?

            // Token - as ID ?


            // Tracks...

            bool list1empty = source1.Tracks == null || source1.Tracks.Length == 0;
            bool list2empty = source2.Tracks == null || source2.Tracks.Length == 0;

            if (!list1empty && !list2empty)
            {

                List<string> notUnique1 = new List<string>();
                List<string> notUnique2 = new List<string>();

                foreach (RecordingJobTrack track in source1.Tracks)
                {
                    int cnt1 = source1.Tracks.Count(t => t.SourceTag == track.SourceTag);
                    if (cnt1 > 1)
                    {
                        notUnique1.Add(track.SourceTag);
                    }
                }
                foreach (RecordingJobTrack track in source2.Tracks)
                {
                    int cnt2 = source2.Tracks.Count(t => t.SourceTag == track.SourceTag);
                    if (cnt2 > 1)
                    {
                        notUnique2.Add(track.SourceTag);
                    }
                }

                if (notUnique1.Count > 0)
                {
                    dump.AppendFormat("         Tracks list is invalid when information is received from GetRecordingJobs. The following SourceTags are not unique: {0}{1}",
                                      string.Join(",", notUnique1.ToArray()), Environment.NewLine);

                    ok = false;
                }
                if (notUnique2.Count > 0)
                {
                    dump.AppendFormat("         Tracks list is invalid when information is received from GetRecordingJobConfiguration. The following SourceTags are not unique: {0}{1}",
                                      string.Join(",", notUnique2.ToArray()), Environment.NewLine);

                    ok = false;
                }

                if (notUnique1.Count == 0 && notUnique2.Count == 0)
                {
                    List<string> commonTags = new List<string>();

                    foreach (RecordingJobTrack track in source1.Tracks)
                    {
                        RecordingJobTrack second = source2.Tracks.FirstOrDefault(t => t.SourceTag == track.SourceTag && t.Destination == track.Destination);
                        if (second == null)
                        {
                            dump.AppendFormat(
                                string.Format(
                                    "         Track with SourceTag = '{0}', Destination = '{1}' not found in tracks list in structure received from GetRecordingJobConfiguration{2}",
                                    track.SourceTag, track.Destination, Environment.NewLine));
                            ok = false;
                        }
                    }
                    foreach (RecordingJobTrack track in source2.Tracks)
                    {
                        int cnt = source1.Tracks.Count(t => t.SourceTag == track.SourceTag && t.Destination == track.Destination);
                        if (cnt == 0)
                        {
                            dump.AppendFormat(
                                string.Format(
                                    "         Track with SourceTag = '{0}', Destination = '{1}' not found in tracks list in structure received from GetRecordingJobs{2}",
                                    track.SourceTag, track.Destination, Environment.NewLine));
                            ok = false;
                        }
                    }
                }
            }
            else
            {
                if (!(list1empty && list2empty))
                {
                    dump.AppendLine("         Tracks list is present only in one structure");
                    ok = false;
                }
            }


            if (!ok)
            {
                logger.Append(dump.ToString());
            }

            return ok;
        }

        bool CompareJobStateSources(Dictionary<RecordingJobStateSource, RecordingJobStateSource> common, StringBuilder logger)
        {
            bool ok = true;

            foreach (RecordingJobStateSource info1 in common.Keys)
            {
                string token = info1.SourceToken.Token;
                string type = info1.SourceToken.Type;
                RecordingJobStateSource info2 = common[info1];

                var dump = new StringBuilder(string.Format("      Information for RecordingJobStateSource with token '{0}' (type '{1}') is different: {2}",
                                                           token, type, Environment.NewLine));

                bool localOk = CompareJobStateSourceInformation(info1, info2, dump);

                if (!localOk)
                {
                    logger.Append(dump);
                    ok = false;
                }
            }

            return ok;
        }


        bool CompareJobStateSourceInformation(RecordingJobStateSource source1,
                                              RecordingJobStateSource source2,
                                              StringBuilder logger)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            // AutoCreateReceiver is not specified, if information is valid.

            // Extension - SKIP ?

            // Token - as ID ?


            // Tracks...

            bool list1empty = source1.Tracks == null || null == source1.Tracks.Track || !source1.Tracks.Track.Any();
            bool list2empty = source2.Tracks == null || null == source2.Tracks.Track || !source2.Tracks.Track.Any();

            if (!list1empty && !list2empty)
            {

                List<string> notUnique1 = new List<string>();
                List<string> notUnique2 = new List<string>();

                foreach (RecordingJobStateTrack track in source1.Tracks.Track.Distinct())
                {
                    int cnt1 = source1.Tracks.Track.Count(t => t.SourceTag == track.SourceTag);
                    if (cnt1 > 1)
                    {
                        notUnique1.Add(track.SourceTag);
                    }
                }
                foreach (RecordingJobStateTrack track in source2.Tracks.Track.Distinct())
                {
                    int cnt2 = source2.Tracks.Track.Count(t => t.SourceTag == track.SourceTag);
                    if (cnt2 > 1)
                    {
                        notUnique2.Add(track.SourceTag);
                    }
                }

                if (notUnique1.Count > 0)
                {
                    dump.AppendLine(string.Format("         Tracks list is invalid when information is received from notification. The following SourceTags are not unique: {0}",
                                                  string.Join(",", notUnique1.ToArray())));

                    ok = false;
                }
                if (notUnique2.Count > 0)
                {
                    dump.AppendLine(string.Format("         Tracks list is invalid when information is received from GetRecordingJobState. The following SourceTags are not unique: {0}",
                                                  string.Join(",", notUnique2.ToArray())));

                    ok = false;
                }

                if (notUnique1.Count == 0 && notUnique2.Count == 0)
                {
                    var commonTags = new List<string>();

                    foreach (RecordingJobStateTrack track in source1.Tracks.Track)
                    {
                        var second = source2.Tracks.Track.FirstOrDefault(t => t.SourceTag == track.SourceTag && t.Destination == track.Destination);
                        if (second == null)
                        {
                            dump.AppendLine(string.Format("         Track with SourceTag = '{0}', Destination = '{1}' not found in tracks list in structure received from GetRecordingJobState",
                                                          track.SourceTag, track.Destination));
                            ok = false;
                        }
                    }
                    foreach (RecordingJobStateTrack track in source2.Tracks.Track)
                    {
                        int cnt = source1.Tracks.Track.Count(t => t.SourceTag == track.SourceTag && t.Destination == track.Destination);
                        if (cnt == 0)
                        {
                            dump.AppendLine(string.Format("         Track with SourceTag = '{0}', Destination = '{1}' not found in tracks list in structure received from notification",
                                                          track.SourceTag, track.Destination));
                            ok = false;
                        }
                    }
                }
            }
            else
            {
                if (!(list1empty && list2empty))
                {
                    dump.AppendLine("         Tracks list is present only in one structure");
                    ok = false;
                }
            }


            if (!ok)
            {
                logger.Append(dump);
            }

            return ok;
        }


        #region Recording Control event description

        internal class EventItemDescription
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Namespace { get; set; }
            public bool Mandatory { get; set; }

            public EventItemDescription(): this(null, null, null, null)
            {}

            public EventItemDescription(string path, string name, string type, string ns): this(path, name, type, ns, true)
            {}

            public EventItemDescription(string path, string name, string type, string ns, bool mandatory)
            {
                Path = path;
                Name = name;
                Namespace = ns;
                Type = type;
                Mandatory = mandatory;
            }
        }

        internal class RecordingControlEventDescription
        {
            public List<EventItemDescription> itemDescriptions { get; private set; }

            public RecordingControlEventDescription()
            { itemDescriptions = new List<EventItemDescription>(); }

            public void addItemDescription(EventItemDescription description) { itemDescriptions.Add(description); }

            public bool isProperty { get; set; }
        }

        internal class NotificationItemDescription
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Namespace { get; set; }
            public bool Mandatory { get; set; }
            public List<string> AllowedValues { get; set; }

            public NotificationItemDescription(): this(null, null, null, null)
            {}

            public NotificationItemDescription(string path, string name, string type, string ns): this(path, name, type, ns, true)
            {}

            public NotificationItemDescription(string path, string name, string type, string ns, bool mandatory)
            {
                Path = path;
                Name = name;
                Namespace = ns;
                Type = type;
                Mandatory = mandatory;
            }
        }
        internal class RecordingControlNotificationDescription
        {
            public List<NotificationItemDescription> itemDescriptions { get; private set; }

            public RecordingControlNotificationDescription()
            { itemDescriptions = new List<NotificationItemDescription>(); }

            public void addItemDescription(NotificationItemDescription description) { itemDescriptions.Add(description); }
        }

        #endregion

        #region Validating utils
        bool checkEventDescription(XmlElement eventNode, RecordingControlEventDescription eventDescription, IXmlNamespaceResolver namespaceResolver, StringBuilder logger)
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

        bool checkNotification(XmlElement notificationNode, RecordingControlNotificationDescription notificationDescription, StringBuilder logger)
        {
            var messageNode = notificationNode.GetMessageContentElement();
            if (null == messageNode)
            {
                logger.AppendLine("MessageDescription element is absent.");
                return false;
            }

            var manager = new XmlNamespaceManager(notificationNode.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            bool flag = true;
            foreach (var itemDescription in notificationDescription.itemDescriptions)
            {
                var path = itemDescription.Path.Split('/').Select(e => "tt:" + e).Aggregate("", (s, s1) => s + s1 + "/").Trim('/');
                //Validate "ElemtnItem" as a separate case
                if (path.EndsWith("ElementItem"))
                {
                    //Need to clone node because function ValidateElementItem works correctly only when its argument is a root node
                    ValidateElementItemContent(ValidateElementItem(messageNode.CloneNode(true) as XmlElement, itemDescription.Name, logger), 
                                               itemDescription.Type, itemDescription.Namespace, logger);
                }
                else
                {
                    var nodes = messageNode.SelectNodes(path, manager).OfType<XmlElement>();
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
                        //XmlAttribute type = itemNode.Attributes[OnvifMessage.TYPE];
                        //if (type == null)
                        //{
                        //    flag = false;
                        //    logger.AppendFormat("'Type' attribute is missing for '{0}' simple item\n", itemDescription.Name);
                        //}
                        //else
                        //{
                        //    string error = string.Empty;
                        //    if (!type.IsCorrectQName(itemDescription.Type, itemDescription.Namespace, itemNode, ref error))
                        //    {
                        //        flag = false;
                        //        logger.AppendFormat("'Type' attribute is incorrect for '{0}' simple item: {1}\n", itemDescription.Name, error);
                        //    }
                        //}
                        if (null != itemDescription.AllowedValues)
                        {
                            XmlAttribute value = itemNode.Attributes[OnvifMessage.VALUE];
                            if (null == value)
                            {
                                flag = false;
                                logger.AppendLine(string.Format("'Value' attribute is missing for '{0}' simple item.", itemDescription.Name));
                            }
                            else
                            {
                                if (!itemDescription.AllowedValues.Contains(value.Value))
                                {
                                    flag = false;
                                    logger.AppendFormat("'Value' attribute has invalid value for '{0}' simple item.", itemDescription.Name);
                                    if (1 == itemDescription.AllowedValues.Count())
                                        logger.AppendLine(string.Format("Expected: {0}, actual: {1}.", itemDescription.AllowedValues.First(), value.Value));
                                    else
                                    {
                                        var allowedValues = string.Join(", ", itemDescription.AllowedValues.ToArray()).Trim(new[] { ' ', ',' });
                                        logger.AppendLine(string.Format("Expected: {0}, actual: {1}.", allowedValues, value.Value));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return flag;
        }

        void ValidateNotificationMessages(Dictionary<Event.NotificationMessageHolderType, XmlElement> notifications,
                                          TopicInfo topic,
                                          RecordingControlNotificationDescription notificationDescription,
                                          string propertyOperation)
        {
            ValidateMessageFunction validateMessage = (notification, settings, log) =>
            {
                var dump = new StringBuilder();
                var flag = ValidateMessageCommonElements(notification,
                                                         settings.RawMessageElements[notification],
                                                         settings.ExpectedTopic,
                                                         settings.ExpectedPropertyOperation,
                                                         settings.NamespaceManager, dump);


                if (flag)
                {
                    //dump = new StringBuilder(string.Format("Notification with Topic {0}:", notification.Topic));
                    flag = checkNotification(settings.RawMessageElements[notification], notificationDescription, dump);
                }

                if (!flag)
                    log.Append(dump.ToString());

                return flag;
            };

            ValidateMessages(notifications, topic, propertyOperation, null, validateMessage);
        }

        //Makes dictionary: RecordingJobToken -> RecordingJobStateInformation
        static Dictionary<string, RecordingJobStateInformation> GetRecordingJobStateInformationFromMessages(IEnumerable<XmlElement> messages)
        {
            return messages.ToDictionary(e => GetAttributeValueOfItemFromMessage(e, "Source", "SimpleItem", "RecordingJobToken"),
                                         e => GetElementDataItemFromMessage<RecordingJobStateInformation>(e, "Information"));
        }

        static T GetElementDataItemFromMessage<T>(XmlElement msg, string elementName) where T : class
        {
            return GetItemFromMessageGeneral<T>(msg, "Data", "ElementItem", elementName);
        }

        static T GetItemFromMessageGeneral<T>(XmlElement msg, string section, string itemType, string itemName) where T : class
        {
            var manager = new XmlNamespaceManager(msg.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            var path = string.Format("tt:{0}/tt:{1}", section, itemType);
            var nodes = msg.SelectNodes(path, manager).OfType<XmlElement>();
            var node = nodes.FirstOrDefault(e => null != e.Attributes[OnvifMessage.NAME]
                                                 && e.Attributes[OnvifMessage.NAME].Value == itemName);

            if (null != node)
            {
                //Children of type XmlElement
                var e = node.OfType<XmlElement>().FirstOrDefault();

                if (null != e)
                {
                    var xRoot = new XmlRootAttribute() { Namespace = OnvifMessage.ONVIF };

                    var s = new XmlSerializer(typeof (T), xRoot);
                    return s.Deserialize(new XmlNodeReader(e)) as T;
                }

                return null;
            }

            return null;
        }

        static string GetAttributeValueOfItemFromMessage(XmlElement msg, string section, string itemType, string itemName)
        {
            var manager = new XmlNamespaceManager(msg.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            var path = string.Format("tt:{0}/tt:{1}", section, itemType);
            var nodes = msg.SelectNodes(path, manager).OfType<XmlElement>();
            var node = nodes.FirstOrDefault(e => null != e.Attributes[OnvifMessage.NAME]
                                                 && e.Attributes[OnvifMessage.NAME].Value == itemName);

            if (null != node && node.HasAttribute(OnvifMessage.VALUE))
                return node.Attributes[OnvifMessage.VALUE].Value;

            return null;
        }
        #endregion

        static T deepCopyXmlSerializableObject<T>(T o)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream = new MemoryStream();
            serializer.Serialize(stream, o);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return (T)serializer.Deserialize(stream);
        }

        class AnnexException: AssertException
        {
            public AnnexException(string message): base(message) {}
        }

        bool Annex10Prerequisities(out List<GetRecordingsResponseItem> recordingsNames, out GetRecordingsResponseItem deletedRecording)
        {
            var capabilities = GetServiceCapabilities();
            var recordings = GetRecordings();

            recordingsNames = recordings.ToList();

            deletedRecording = null;

            if (capabilities.MaxRecordingsSpecified)
            {
                if ((int) capabilities.MaxRecordings == recordings.Count())
                {
                    //Return after first successfull removing of recording
                    foreach (var recording in recordings)
                    {
                        try
                        {
                            DeleteRecording(recording.RecordingToken);
                            recordingsNames = recordings.ToList();
                            deletedRecording = recording;

                            return true;
                        }
                        catch (FaultException e)
                        {
                            StepPassed();
                        }
                    }

                    //throw new AnnexException("Failed to remove recording according to Annex.10");
                    Assert(false, "Failed to remove recording according to Annex.10");
                }

                return true;
            }

            return false;
        }

        string Annex10CreateRecording(bool possibleToCreateRecording, IEnumerable<GetRecordingsResponseItem> recordings, string retentionTime,
                                      out RecordingConfiguration config, out GetRecordingsResponseItem deletedRecording)
        {
            deletedRecording = null;

            config = new RecordingConfiguration
            {
                MaximumRetentionTime = retentionTime,
                Content = "Create recording event test",
                Source = new RecordingSourceInformation
                         {
                             SourceId = CameraAddress.Trim(),
                             Name = "CameraName",
                             Location = "LocationDescription",
                             Description = "SourceDescription",
                             Address = CameraAddress.Trim(),
                         }
            };
            if (possibleToCreateRecording)
            {
                return CreateRecording(config);
            }
            else
            {
                //Try to create new recording
                try
                { return CreateRecording(config); }
                catch (FaultException)
                {
                    StepPassed();

                    //In case of fail try to delete one of existing recordings and create new one
                    foreach (var recording in recordings)
                    {
                        try
                        {
                            DeleteRecording(recording.RecordingToken);
                            deletedRecording = recording;

                            return CreateRecording(config);

                        }
                        catch (FaultException)
                        {
                            StepPassed();
                        }
                    }
                }

                //throw new AssertException("Failed to create recording according to Annex.10");
                Assert(false, "Failed to create recording according to Annex.10");
                
                return null;
            }
        }

        protected string SelectOrCreateRecordingAnnex6(out RecordingConfiguration replacedConfiguration, 
                                                       out List<KeyValuePair<string, TrackConfiguration>> selectedTracks, 
                                                       out bool recordingCreated)
        {
            string recordingToken = null;
            replacedConfiguration = null;
            recordingCreated = false;
            GetRecordingsResponseItem deletedRecording = null;

            var defaultsTracks = new List<KeyValuePair<string, TrackConfiguration>>() 
                                 { new KeyValuePair<string, TrackConfiguration>("VIDEO001", new TrackConfiguration(){ TrackType = TrackType.Video }),
                                   new KeyValuePair<string, TrackConfiguration>("AUDIO001", new TrackConfiguration(){ TrackType = TrackType.Audio }),
                                   new KeyValuePair<string, TrackConfiguration>("META001", new TrackConfiguration(){ TrackType = TrackType.Metadata })};

            selectedTracks = defaultsTracks;

            //1. If GetServices is not supported, go to step 3.
            //2. If GetServices supported, get maxRecordings and dynamicRecording from GetServiceCapabilities and go to step 4
            //3. Get dynamicRecording from GetCapabilities.

            bool dynamicRecordingSupported = false;
            int? maxNumberOfRecordings = null;

            if (Features.ContainsFeature(Feature.GetServices))
            {
                RecordingServiceCapabilities capabilities = GetServiceCapabilities();
                dynamicRecordingSupported = capabilities.DynamicRecordingsSpecified && capabilities.DynamicRecordings;
                maxNumberOfRecordings = (int)capabilities.MaxRecordings;
            }
            else
            {
                Proxies.Onvif.Capabilities commonCapabilities = DeviceClient.GetCapabilities(null);
                Assert(commonCapabilities != null && commonCapabilities.Extension != null && commonCapabilities.Extension.Recording != null,
                       "Recording capabilities not found",
                       "Check that the DUT returned Recording capabilities");
                dynamicRecordingSupported = commonCapabilities.Extension.Recording.DynamicRecordings;
            }

            var config = new RecordingConfiguration
                {
                    MaximumRetentionTime = _retentionTime,
                    Content = "Recording from device",
                    Source = new RecordingSourceInformation
                             {
                                 SourceId = CameraAddress.Trim(),
                                 Name = "CameraName",
                                 Location = "LocationDescription",
                                 Description = "SourceDescription",
                                 Address = CameraAddress.Trim()
                             }
                };

            GetRecordingsResponseItem[] recordings = null;
            bool reconfigure = false;

            //4. If dynamicRecording is supported, go to step 6
            if (dynamicRecordingSupported)
            {
                bool execDelete = true;

                //6. if GetServices is not supported and maxRecording is unknown, go to step 8
                //7. if maxRecordings == total number of recordings, go to step 8.5, else go to step 11.
                //8. Try to create recording with required properties. If the recording is created successsfully, skip other steps and use it for test
                if (!maxNumberOfRecordings.HasValue)
                {
                    try
                    {
                        string token = CreateRecording(config);
                        recordingCreated = true;
                        return token;
                    }
                    catch (FaultException exc)
                    {
                        StepPassed();
                    }
                }

                //8.5 Call GetRecordings 
                //9. Try to delete first recording. If deletion succeeds, go to step 11
                //10 Try to delete second recording. If there are no second recording, skip other steps and fail the test. If deletion fails, skip other steps and fail the test.
                //11. Try to create recording with required properties. If the recording is created successsfully, skip other steps and use it for test.
                //12. Select recording with track of type video
                //13. Reconfigure this recording
                recordings = GetRecordings();

                if (recordings != null)
                {
                    execDelete = recordings.Length == maxNumberOfRecordings.Value;
                }
                else
                {
                    // if no maxNumberOfRecordings is known, we have already tried to create recording
                    reconfigure = !maxNumberOfRecordings.HasValue;
                    execDelete = false;
                }

                if (!reconfigure)
                {
                    if (execDelete)
                    {
                        foreach (GetRecordingsResponseItem item in recordings)
                        {
                            try
                            {
                                DeleteRecording(item.RecordingToken);
                                deletedRecording = item;
                                break;
                            }
                            catch (FaultException exc)
                            {
                                //LogFault(exc);
                                StepPassed();
                            }
                        }
                        if (deletedRecording == null)
                        {
                            reconfigure = true;
                        }
                    }
                }

                if (!reconfigure)
                {
                    //[08.05.2013] AKS: according to Annex.6 we should fail procedure in case of SOAP Fault on CreateRecording step
                    //TODO: this is copied procedure, make the same fix in other places.
                    //try
                    //{
                        string token = CreateRecording(config);
                        recordingCreated = true;
                        return token;
                    //}
                    //catch (FaultException exc)
                    //{
                    //    StepPassed();
                    //}
                }
            }
            else
            {
                //5. Call GetRecordings and go to step 12
                recordings = GetRecordings();

            }

            Assert(recordings != null && recordings.Length > 0,
                   "Recordings list is empty",
                   "Check that recordings list is not empty");
            // reconfigure existing recording


            // select recording with video track
            RecordingConfiguration localReplacedConfiguration = null;
            foreach (GetRecordingsResponseItem recording in recordings)
            {
                if (recording.Tracks != null)
                {
                    if (recording.Tracks.Track.Where(track => track.Configuration != null).Any(track => track.Configuration.TrackType == TrackType.Video))
                    {
                        recordingToken = recording.RecordingToken;
                    }
                }
                if (!string.IsNullOrEmpty(recordingToken))
                {
                    localReplacedConfiguration = recording.Configuration;
                    selectedTracks = recording.Tracks.Track.Select(e => new KeyValuePair<string, TrackConfiguration>(e.TrackToken, e.Configuration)).ToList();
                    break;
                }
            }

            Assert(!string.IsNullOrEmpty(recordingToken),
                   "Recording with track of type 'Video' not found",
                   "Check that existing recording can be used for test");

            // set configuration
            SetRecordingConfiguration(recordingToken, config);
            replacedConfiguration = localReplacedConfiguration;

            return recordingToken;
        }
    }
}
