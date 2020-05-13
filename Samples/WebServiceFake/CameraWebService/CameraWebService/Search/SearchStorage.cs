using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace CameraWebService.Search
{

    public class SearchStorage
    {
        private static SearchStorage _instance;
        public static SearchStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SearchStorage();
                }
                return _instance;
            }
        }

        List<RecordingInformation> _recordings;

        public List<RecordingInformation> Recordings
        {
            get
            {
                if (_recordings == null)
                {
                    _recordings = new List<RecordingInformation>();

                    RecordingInformation info1 = CreateVideoRecording(1);
                    _recordings.Add(info1);

                    RecordingInformation info2 = CreateRecording(2);
                    _recordings.Add(info2);

                    //if (_recordingsGenerationCrashTest)
                    {
                        RecordingInformation info3 = CreateRecording(3);
                        _recordings.Add(info3);
                    }

                    //RecordingInformation info3 = CreateIncorrectRecording(4);
                    //_recordings.Add(info3);

                    {
                        RecordingInformation info4 = CreateAudioRecording(4);
                        _recordings.Add(info4);
                    }

                    {
                        RecordingInformation info5 = CreateMetadataRecording(5);
                        _recordings.Add(info5);
                    }
                }

                return _recordings;

            }
        }

        List<GetRecordingJobsResponseItem> _recordingJobs;
        public List<GetRecordingJobsResponseItem> RecordingJobs
        {
            get
            {
                if (_recordingJobs == null)
                {
                    _recordingJobs = new List<GetRecordingJobsResponseItem>();
                    foreach (var info in Search.SearchStorage.Instance.Recordings)
                    {
                        var rand = new Random();
                        int num = rand.Next(1, 2) % 2;
                        var jobState = new GetRecordingJobsResponseItem();
                        jobState.JobToken = "job_" + Guid.NewGuid().ToString().Substring(0, 8);
                        jobState.JobConfiguration = new RecordingJobConfiguration();
                        jobState.JobConfiguration.RecordingToken = info.RecordingToken;
                        jobState.JobConfiguration.Mode = num == 0 ? "Idle" : "Active";
                        jobState.JobConfiguration.Priority = num == 0 ? 0 : 1;
                        List<RecordingJobSource> jobSourceList = new List<RecordingJobSource>();
                        for (int i = 0; i <= 3; i++)
                        {
                            var jobSource = new RecordingJobSource();
                            jobSource.SourceToken = new global::SourceReference();
                            jobSource.SourceToken.Token = "sourceToken_" + Guid.NewGuid().ToString().Substring(0, 8);
                            List<RecordingJobTrack> jobTracks = new List<RecordingJobTrack>();
                            for (int j = 0; j <= 3; j++)
                            {
                                RecordingJobTrack jobTrack = new RecordingJobTrack();
                                jobTrack.SourceTag = "sourceTag" + Guid.NewGuid().ToString().Substring(0, 8);
                                jobTrack.Destination = "destination" + Guid.NewGuid().ToString().Substring(0, 8);
                                jobTracks.Add(jobTrack);
                            }
                            jobSource.Tracks = jobTracks.ToArray();
                            jobSourceList.Add(jobSource);
                        }
                        jobState.JobConfiguration.Source = jobSourceList.ToArray();
                        _recordingJobs.Add(jobState);
                    }                
                }

                return _recordingJobs;
            }
            set
            {
                _recordingJobs = value;
            }
        }

        Dictionary<string, string> _jobStates;

        public Dictionary<string, string> JobStates
        {
            get 
            {
                if (_jobStates == null)
                {
                    _jobStates = new Dictionary<string, string>();
                                    
                
                }

                return _jobStates;
            }
        }

        public RecordingJobConfiguration GetRecordingJobConfiguration(string token)
        {
            RecordingJobConfiguration config = null;

            GetRecordingJobsResponseItem item = RecordingJobs.Where(j => j.JobToken == token).FirstOrDefault();
            if (item != null)
            {
                config = item.JobConfiguration;
            }

            return config;
        }

        List<Receiver.Receiver> _receivers;
        public List<Receiver.Receiver> Receivers
        {
            get
            {
                if (_receivers == null)
                {
                    _receivers = new List<Receiver.Receiver>();

                    _receivers.Add(CreateReceiver(1));
                    _receivers.Add(CreateReceiver(2));
                    _receivers.Add(CreateReceiver(3));
                    _receivers.Add(CreateReceiver(4));
                    _receivers.Add(CreateReceiver(5));
                }

                return _receivers;

            }
        }

        Dictionary<string, Receiver.ReceiverStateInformation> _receiverStateInformationDictionary;
        public Dictionary<string, Receiver.ReceiverStateInformation> ReceiverStateInformationDictionary
        {
            get
            {
                if (_receiverStateInformationDictionary == null)
                {
                    _receiverStateInformationDictionary = new Dictionary<string, Receiver.ReceiverStateInformation>();

                    foreach (var receiver in _receivers)
                    {
                        _receiverStateInformationDictionary.Add(receiver.Token,
                            new Receiver.ReceiverStateInformation()
                            {
                                State = CameraWebService.Receiver.ReceiverState.NotConnected,
                                AutoCreated = true
                            });
                    }
                }

                return _receiverStateInformationDictionary;

            }
        }

        public Receiver.ReceiverStateInformation GetReceiverState(string token)
        {
            return ReceiverStateInformationDictionary[token];
        }

        Receiver.Receiver CreateReceiver(int id)
        {
            Receiver.Receiver receiver = new Receiver.Receiver();
            receiver.Token = string.Format("receiver{0}", id);
            receiver.Configuration = new CameraWebService.Receiver.ReceiverConfiguration()
            {
                MediaUri = "Uri1",
                StreamSetup = new CameraWebService.Receiver.StreamSetup()
                {
                    Transport = new CameraWebService.Receiver.Transport()
                }
            };

            return receiver;
        }

        RecordingInformation CreateVideoRecording(int id)
        {
            RecordingInformation info = new RecordingInformation();
            info.Content = string.Format("Recording{0}", id);
            info.EarliestRecordingSpecified = true;
            info.EarliestRecording = new System.DateTime(2012, 05, 12, 12 + id, 00, 00);
            info.LatestRecordingSpecified = true;
            info.LatestRecording = new System.DateTime(2012, 05, 12, 13 + id, 00, 00);
            info.RecordingStatus = RecordingStatus.Stopped;
            info.RecordingToken = string.Format("recording{0}", id);
            info.Source = new RecordingSourceInformation();
            info.Source.SourceId = string.Format("Source{0}", id);
            info.Source.Name = string.Format("Source{0}", id);
            info.Source.Description = string.Format("Source {0}", id);
            info.Source.Address = string.Format("Address {0}", id);
            info.Source.Location = string.Format("Location {0}", id);

            List<TrackInformation> tracks = new List<TrackInformation>();
            tracks.Add(new TrackInformation()
                           {
                               DataFrom = new System.DateTime(2012, 05, 12, 12 + id, 00, 00),
                               DataTo = new System.DateTime(2012, 05, 12, 13 + id, 00, 00),
                               TrackToken = "track1",
                               TrackType = TrackType.Video,
                               Description = "Video Track 01",
                           });
            tracks.Add(new TrackInformation()
            {
                DataFrom = new System.DateTime(2012, 05, 12, 12 + id, 00, 00),
                DataTo = new System.DateTime(2012, 05, 12, 13 + id, 00, 00),
                TrackToken = "track2",
                TrackType = TrackType.Metadata,
                Description = "Metadata Track 01",
            });

            info.Track = tracks.ToArray();
            return info;
        }

        RecordingInformation CreateRecording(int id)
        {
            RecordingInformation info = new RecordingInformation();
            info.Content = string.Format("Recording{0}", id);
            info.EarliestRecordingSpecified = true;
            info.EarliestRecording = new System.DateTime(2012, 05, 12, 12 + id, 00, 00);
            info.LatestRecordingSpecified = true;
            info.LatestRecording = new System.DateTime(2012, 05, 12, 15 + id, 00, 00);
            info.RecordingStatus = RecordingStatus.Stopped;
            info.RecordingToken = string.Format("recording{0}", id);
            info.Source = new RecordingSourceInformation();
            info.Source.SourceId = string.Format("Source{0}", id);
            info.Source.Name = string.Format("Source{0}", id);
            info.Source.Description = string.Format("Source {0}", id);
            info.Source.Address = string.Format("Address {0}", id);
            info.Source.Location = string.Format("Location {0}", id);

            List<TrackInformation> tracks = new List<TrackInformation>();
            tracks.Add(new TrackInformation()
            {
                DataFrom = new System.DateTime(2012, 05, 12, 12 + id, 00, 00),
                DataTo = new System.DateTime(2012, 05, 12, 15 + id, 00, 00),
                TrackToken = "track1",
                TrackType = TrackType.Video,
                Description = "Video Track 01"
            });
            if (id == 2)
            {
                if (_recordingsGenerationCrashTest)
                {
                    tracks.Add(new TrackInformation()
                                   {
                                       DataFrom = new System.DateTime(2012, 05, 12, 14 + id, 00, 00),
                                       DataTo = new System.DateTime(2012, 05, 12, 15 + id, 00, 00),
                                       TrackToken = "track2",
                                       TrackType = TrackType.Video,
                                       Description = "Video Track 02"
                                   });
                }
                else
                {
                    tracks.Add(new TrackInformation()
                    {
                        DataFrom = new System.DateTime(2012, 05, 13, 14 + id, 00, 00),
                        DataTo = new System.DateTime(2012, 05, 12, 15 + id, 00, 00),
                        TrackToken = "track2",
                        TrackType = TrackType.Audio,
                        Description = "Audio Track 02 "
                    });
                }
            }
            if (id == 3 && _recordingsGenerationCrashTest)
            {
                tracks.Add(new TrackInformation()
                               {
                                   DataFrom = new System.DateTime(2012, 05, 12, 12 + id, 00, 00),
                                   DataTo = new System.DateTime(2012, 05, 12, 15 + id, 00, 00),
                                   TrackToken = "track3",
                                   TrackType = TrackType.Metadata,
                                   Description = "Metadata Track 02"
                               });
            }


            info.Track = tracks.ToArray();
            return info;
        }

        RecordingInformation CreateIncorrectRecording(int id)
        {
            RecordingInformation info = new RecordingInformation();
            info.Content = string.Format("Recording{0}", id);
            info.EarliestRecordingSpecified = true;
            info.EarliestRecording = new System.DateTime(2012, 05, 12, 12 + id, 00, 00);
            info.LatestRecordingSpecified = true;
            info.LatestRecording = new System.DateTime(2012, 05, 12, 15 + id, 00, 00);
            info.RecordingStatus = RecordingStatus.Stopped;
            info.RecordingToken = string.Format("recording{0}", id);
            info.Source = new RecordingSourceInformation();
            info.Source.SourceId = string.Format("Source{0}", id);
            info.Source.Name = string.Format("Source{0}", id);
            info.Source.Description = string.Format("Source {0}", id);
            info.Source.Address = string.Format("Address {0}", id);
            info.Source.Location = string.Format("Location {0}", id);

            List<TrackInformation> tracks = new List<TrackInformation>();
            tracks.Add(new TrackInformation()
            {
                DataFrom = new System.DateTime(2012, 05, 12, 11 + id, 00, 00),
                DataTo = new System.DateTime(2012, 05, 12, 13 + id, 00, 00),
                TrackToken = "track1",
                TrackType = TrackType.Video,
                Description = "Video Track 01"
            });
            tracks.Add(new TrackInformation()
            {
                DataFrom = new System.DateTime(2012, 05, 12, 12 + id, 00, 00),
                DataTo = new System.DateTime(2012, 05, 12, 14 + id, 00, 00),
                TrackToken = "track2",
                TrackType = TrackType.Audio,
                Description = "Audio Track 02"
            });
            info.Track = tracks.ToArray();
            return info;
        }

        RecordingInformation CreateAudioRecording(int id)
        {
            RecordingInformation info = new RecordingInformation();
            info.Content = string.Format("Recording{0}", id);
            info.EarliestRecordingSpecified = true;
            info.EarliestRecording = new System.DateTime(2012, 05, 12, 12 + id, 00, 00);
            info.LatestRecordingSpecified = true;
            info.LatestRecording = new System.DateTime(2012, 05, 12, 13 + id, 00, 00);
            info.RecordingStatus = RecordingStatus.Stopped;
            info.RecordingToken = string.Format("recording{0}", id);
            info.Source = new RecordingSourceInformation();
            info.Source.SourceId = string.Format("Source{0}", id);
            info.Source.Name = string.Format("Source{0}", id);
            info.Source.Description = string.Format("Source {0}", id);
            info.Source.Address = string.Format("Address {0}", id);
            info.Source.Location = string.Format("Location {0}", id);

            List<TrackInformation> tracks = new List<TrackInformation>();
            tracks.Add(new TrackInformation()
            {
                DataFrom = new System.DateTime(2012, 05, 12, 12 + id, 00, 00),
                DataTo = new System.DateTime(2012, 05, 12, 13 + id, 00, 00),
                TrackToken = "track1",
                TrackType = TrackType.Audio,
                Description = "Video Track 01",
            });

            info.Track = tracks.ToArray();
            return info;
        }

        RecordingInformation CreateMetadataRecording(int id)
        {
            RecordingInformation info = new RecordingInformation();
            info.Content = string.Format("Recording{0}", id);
            info.EarliestRecordingSpecified = true;
            info.EarliestRecording = new System.DateTime(2012, 05, 12, 12 + id, 00, 00);
            info.LatestRecordingSpecified = true;
            info.LatestRecording = new System.DateTime(2012, 05, 12, 13 + id, 00, 00);
            info.RecordingStatus = RecordingStatus.Stopped;
            info.RecordingToken = string.Format("recording{0}", id);
            info.Source = new RecordingSourceInformation();
            info.Source.SourceId = string.Format("Source{0}", id);
            info.Source.Name = string.Format("Source{0}", id);
            info.Source.Description = string.Format("Source {0}", id);
            info.Source.Address = string.Format("Address {0}", id);
            info.Source.Location = string.Format("Location {0}", id);

            List<TrackInformation> tracks = new List<TrackInformation>();
            tracks.Add(new TrackInformation()
            {
                DataFrom = new System.DateTime(2012, 05, 12, 12 + id, 00, 00),
                DataTo = new System.DateTime(2012, 05, 12, 13 + id, 00, 00),
                TrackToken = "track1",
                TrackType = TrackType.Metadata,
                Description = "Metadata Track 01",
            });

            info.Track = tracks.ToArray();
            return info;
        }


        List<FindEventResult> _events;

        public List<FindEventResult> Events
        {
            get
            {
                if (_events == null)
                {
                    GenerateEvents();
                }
                return _events;
            }
        }

        public void ClearEvents()
        {
            _events = null;
        }

        private bool _eventsGenerationCrashTest;
        public void ChangeEventsGeneration(bool diff)
        {
            _eventsGenerationCrashTest = diff;
        }

        public void ChangeEventsGeneration()
        {
            _eventsGenerationCrashTest = !_eventsGenerationCrashTest;
        }

        public void ClearRecordings()
        {
            _recordings = null;
        }

        private bool _recordingsGenerationCrashTest;
        public void ChangeRecordingsGeneration(bool diff)
        {
            _recordingsGenerationCrashTest = diff;
        }

        public void ChangeRecordingsGeneration()
        {
            _recordingsGenerationCrashTest = !_recordingsGenerationCrashTest;
        }

        void GenerateEvents()
        {
            XmlDocument doc = new XmlDocument();

            _events = new List<FindEventResult>();
            foreach (RecordingInformation recording in Recordings)
            {
                foreach (TrackInformation track in recording.Track)
                {
                    {
                        FindEventResult dataAppeared = GenerateTrackEvent(recording, track);
                        dataAppeared.Time = track.DataFrom;

                        XmlText topic = doc.CreateTextNode("tns1:RecordingHistory/Track/State");
                        System.Xml.Serialization.XmlSerializerNamespaces xmlns =
                            new System.Xml.Serialization.XmlSerializerNamespaces();
                        xmlns.Add("tns1", "http://www.onvif.org/ver10/topics");
                        dataAppeared.Event.Topic.Xmlns = xmlns;

                        dataAppeared.Event.Topic.Any = new XmlNode[] { topic };

                        dataAppeared.Event.Message = doc.CreateElement("tt:Message",
                                                                       "http://docs.oasis-open.org/wsn/b-2");

                        XmlElement data = doc.CreateElement("tt", "Data", "http://www.onvif.org/ver10/schema");
                        dataAppeared.Event.Message.AppendChild(data);

                        data.AppendChild(CreateSimpleItemElement(doc, "IsDataPresent", "True"));

                        if (_eventsGenerationCrashTest)
                        {
                            dataAppeared.StartStateEvent = true;
                            dataAppeared.Event.ProducerReference = new EndpointReferenceType() { Address = new AttributedURIType() { Value = "http://localhost/dut/events.asmx" } };
                        }

                        _events.Add(dataAppeared);
                    }

                    {
                        FindEventResult dataSaved = GenerateTrackEvent(recording, track);
                        dataSaved.Time = track.DataTo;

                        XmlText topic = doc.CreateTextNode("tns1:RecordingHistory/Track/State");
                        System.Xml.Serialization.XmlSerializerNamespaces xmlns =
                            new System.Xml.Serialization.XmlSerializerNamespaces();
                        xmlns.Add("tns1", "http://www.onvif.org/ver10/topics");
                        dataSaved.Event.Topic.Xmlns = xmlns;

                        dataSaved.Event.Topic.Any = new XmlNode[] { topic };

                        dataSaved.Event.Message = doc.CreateElement("tt:Message", "http://docs.oasis-open.org/wsn/b-2");

                        XmlElement data = doc.CreateElement("tt", "Data", "http://www.onvif.org/ver10/schema");
                        dataSaved.Event.Message.AppendChild(data);

                        data.AppendChild(CreateSimpleItemElement(doc, "IsDataPresent", "False"));
                        data.AppendChild(CreateSimpleItemElement(doc, "IsDataPresent", "True"));

                        //if (_eventsGenerationCrashTest)
                        //{
                        //    dataSaved.Event.ProducerReference = null;
                        //}
                        //else
                        //{
                        //    dataSaved.Event.ProducerReference = new EndpointReferenceType() { Address = new AttributedURIType() { Value = "http://localhost/dut/events1.asmx" } };
                        //}

                        _events.Add(dataSaved);

                    }
                    if (_eventsGenerationCrashTest)
                    {
                        FindEventResult parametersEvent = GenerateTrackEvent(recording, track);
                        parametersEvent.Time = recording.EarliestRecording;
                        XmlText topic = doc.CreateTextNode("tns1:RecordingHistory/Track/VideoParameters");
                        System.Xml.Serialization.XmlSerializerNamespaces xmlns =
                            new System.Xml.Serialization.XmlSerializerNamespaces();
                        xmlns.Add("tns1", "http://www.onvif.org/ver10/topics");
                        parametersEvent.Event.Topic.Xmlns = xmlns;
                        parametersEvent.Event.Topic.Any = new XmlNode[] { topic };
                        parametersEvent.Event.Message = doc.CreateElement("tt:Message",
                                                                           "http://docs.oasis-open.org/wsn/b-2");
                        _events.Add(parametersEvent);

                        //if (_eventsGenerationCrashTest)
                        //{
                        //    parametersEvent.Event.SubscriptionReference = new EndpointReferenceType() { Address = new AttributedURIType() { Value = "http://localhost/dut/events.asmx" } };
                        //}
                        //else
                        //{
                        //    parametersEvent.Event.SubscriptionReference = new EndpointReferenceType() { Address = new AttributedURIType() { Value = "http://localhost/dut/events1.asmx" } };
                        //}
                    }

                    if (!_eventsGenerationCrashTest)
                    {
                        FindEventResult parametersEvent = GenerateTrackEvent(recording, track);
                        parametersEvent.Time = recording.EarliestRecording;
                        XmlText topic = doc.CreateTextNode("tns1:RecordingHistory/Track/AudioParameters");
                        System.Xml.Serialization.XmlSerializerNamespaces xmlns =
                            new System.Xml.Serialization.XmlSerializerNamespaces();
                        xmlns.Add("tns1", "http://www.onvif.org/ver10/topics");
                        parametersEvent.Event.Topic.Xmlns = xmlns;
                        parametersEvent.Event.Topic.Any = new XmlNode[] { topic };
                        parametersEvent.Event.Message = doc.CreateElement("tt:Message",
                                                                           "http://docs.oasis-open.org/wsn/b-2");
                        _events.Add(parametersEvent);

                        //if (_eventsGenerationCrashTest)
                        //{
                        //    parametersEvent.Event.SubscriptionReference = new EndpointReferenceType() { Address = new AttributedURIType() { Value = "http://localhost/dut/events.asmx" } };
                        //}
                    }

                } // foreach Track

                {
                    FindEventResult recordingStarted = GenerateTrackEvent(recording, null);
                    recordingStarted.Time = recording.EarliestRecording;
                    XmlText topic = doc.CreateTextNode("tns1:RecordingHistory/Recording/State");
                    System.Xml.Serialization.XmlSerializerNamespaces xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
                    xmlns.Add("tns1", "http://www.onvif.org/ver10/topics");
                    recordingStarted.Event.Topic.Xmlns = xmlns;
                    recordingStarted.Event.Topic.Any = new XmlNode[] { topic };
                    recordingStarted.Event.Message = doc.CreateElement("tt:Message", "http://docs.oasis-open.org/wsn/b-2");

                    XmlElement data = doc.CreateElement("tt", "Data", "http://www.onvif.org/ver10/schema");
                    recordingStarted.Event.Message.AppendChild(data);
                    data.AppendChild(CreateSimpleItemElement(doc, "IsRecording", "True"));
                    data.AppendChild(CreateSimpleItemElement(doc, "IsRecording", "True"));

                    //if (_eventsGenerationCrashTest)
                    //{
                    //    data.AppendChild(CreateSimpleItemElement(doc, "SomeValue", "True"));
                    //}

                    _events.Add(recordingStarted);
                }

                {
                    FindEventResult recordingfinished = GenerateTrackEvent(recording, null);
                    recordingfinished.Time = recording.LatestRecording;
                    XmlText topic = doc.CreateTextNode("tns1:RecordingHistory/Recording/State");
                    System.Xml.Serialization.XmlSerializerNamespaces xmlns =
                        new System.Xml.Serialization.XmlSerializerNamespaces();
                    xmlns.Add("tns1", "http://www.onvif.org/ver10/topics");
                    recordingfinished.Event.Topic.Xmlns = xmlns;
                    recordingfinished.Event.Topic.Any = new XmlNode[] { topic };
                    recordingfinished.Event.Message = doc.CreateElement("tt:Message",
                                                                       "http://docs.oasis-open.org/wsn/b-2");


                    XmlElement data = doc.CreateElement("tt", "Data", "http://www.onvif.org/ver10/schema");
                    recordingfinished.Event.Message.AppendChild(data);
                    data.AppendChild(CreateSimpleItemElement(doc, "IsRecording", "False"));


                    //if (_eventsGenerationCrashTest)
                    //{
                    //    data.AppendChild(CreateSimpleItemElement(doc, "SomeValue", "True"));
                    //}
                    //else
                    //{
                    //    data.AppendChild(CreateSimpleItemElement(doc, "SomeValue", "False"));
                    //}
                    _events.Add(recordingfinished);
                }



            }
        }

        FindEventResult GenerateTrackEvent(RecordingInformation recording, TrackInformation track)
        {
            FindEventResult eventResult = new FindEventResult();
            eventResult.RecordingToken = recording.RecordingToken;
            if (track != null)
            {
                eventResult.TrackToken = track.TrackToken;
            }
            else
            {
                eventResult.TrackToken = "";
            }
            eventResult.Event = new NotificationMessageHolderType();
            //eventResult.Event.ProducerReference = new EndpointReferenceType();
            //eventResult.Event.ProducerReference.Address = new AttributedURIType(){Value= "http://localhost/dut.asmx"};
            eventResult.Event.Topic = new TopicExpressionType();
            eventResult.Event.Topic.Dialect = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            return eventResult;
        }

        XmlElement CreateSimpleItemElement(XmlDocument doc, string name, string value)
        {
            string ns = "http://www.onvif.org/ver10/schema";

            XmlElement itemElement = doc.CreateElement("tt", "SimpleItem", ns);
            XmlAttribute nameAttr = doc.CreateAttribute("Name", ns);
            nameAttr.Value = name;
            itemElement.Attributes.Append(nameAttr);

            XmlAttribute valueAttr = doc.CreateAttribute("Value", ns);
            valueAttr.Value = value;
            itemElement.Attributes.Append(valueAttr);

            return itemElement;
        }

        #region PTZ

        List<FindPTZPositionResult> _ptzPositions;
        public List<FindPTZPositionResult> PtzPositions
        {
            get
            {
                if (_ptzPositions == null)
                {
                    GeneratePtzPositions();
                }
                return _ptzPositions;
            }
        }

        void GeneratePtzPositions()
        {
            _ptzPositions = new List<FindPTZPositionResult>();

            {
                FindPTZPositionResult result = new FindPTZPositionResult();
                result.RecordingToken = Recordings[0].RecordingToken;
                result.TrackToken = Recordings[0].Track[0].TrackToken;
                result.Time = Recordings[0].EarliestRecording.AddMinutes(10);
                result.Position = new PTZVector();
                result.Position.PanTilt = new Vector2D();
                result.Position.PanTilt.x = 0;
                result.Position.PanTilt.y = 1;
                _ptzPositions.Add(result);
            }

            {
                FindPTZPositionResult result = new FindPTZPositionResult();
                result.RecordingToken = Recordings[0].RecordingToken;
                result.TrackToken = Recordings[0].Track[0].TrackToken;
                result.Time = Recordings[0].EarliestRecording.AddMinutes(12);
                result.Position = new PTZVector();
                result.Position.PanTilt = new Vector2D();
                result.Position.PanTilt.x = 0.5F;
                result.Position.PanTilt.y = 0.5F;
                _ptzPositions.Add(result);
            }

        }

        #endregion

        #region METADATA

        List<FindMetadataResult> _metadata;
        public List<FindMetadataResult> Metadata
        {
            get
            {
                if (_metadata == null)
                {
                    GenerateMetadata();
                }
                return _metadata;
            }
        }

        void GenerateMetadata()
        {
            _metadata = new List<FindMetadataResult>();

            {
                FindMetadataResult result = new FindMetadataResult();
                result.RecordingToken = Recordings[0].RecordingToken;
                result.TrackToken = Recordings[0].Track[0].TrackToken;
                result.Time = Recordings[0].EarliestRecording.AddMinutes(10);
                _metadata.Add(result);
            }

            {
                FindMetadataResult result = new FindMetadataResult();
                result.RecordingToken = Recordings[0].RecordingToken;
                result.TrackToken = Recordings[0].Track[0].TrackToken;
                result.Time = Recordings[0].EarliestRecording.AddMinutes(12);

                //if (_metadataGenerationCrashTest)
                // 
                //{ 
                //}

                _metadata.Add(result);
            }

            {
                FindMetadataResult result = new FindMetadataResult();
                result.RecordingToken = Recordings[1].RecordingToken;
                result.TrackToken = Recordings[1].Track[0].TrackToken;
                result.Time = Recordings[0].EarliestRecording.AddMinutes(12);
                _metadata.Add(result);
            }

        }


        public void ClearMetadata()
        {
            _metadata = null;
        }

        private bool _metadataGenerationCrashTest;
        public void ChangeMetadataGeneration(bool diff)
        {
            _metadataGenerationCrashTest = diff;
        }

        public void ChangeMetadataGeneration()
        {
            _metadataGenerationCrashTest = !_metadataGenerationCrashTest;
        }



        #endregion
    }
}
