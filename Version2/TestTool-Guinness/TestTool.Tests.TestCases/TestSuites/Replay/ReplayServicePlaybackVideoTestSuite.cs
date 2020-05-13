using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Enums;
using DateTime=System.DateTime;
using TestTool.Tests.Common.Media;
using TestTool.Tests.Common.Transport;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class ReplayServicePlaybackVideoTestSuite : Base.SearchTest, IVideoFormEvent
    {
        public ReplayServicePlaybackVideoTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        protected bool VideoIsOpened = false;
        private const string CRLF = "$$";
        private const string REQUIRE_ONVIF_REPLAY = "Require: onvif-replay";
        private const int PACKET_LOG_INTERVAL_MS = 500;
        Func<string, bool> _handleLogMessage = null;

        private const string PATH = "Replay\\Playback Control\\Video Streaming";

        #region Replay Client

        private ReplayPortClient _replayClient;

        protected ReplayPortClient CreateReplayPortClient()
        {
            string address = GetReplayServiceAddress();

            BeginStep("Connect to Replay service");
            LogStepEvent(string.Format("Replay service address: {0}", address));
            if (!address.IsValidUrl())
            {
                throw new AssertException("Replay service address is invalid");
            }
            Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(ReplaySchemasSet.GetInstance()) });
            ReplayPortClient client = new ReplayPortClient(binding, new EndpointAddress(address));
            StepPassed();
            if (_replayClient != null)
            {
                CloseReplayClient();
            }
            return (_replayClient = client);
        }

        protected string GetReplayServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                Binding binding =
                    CreateBinding(false,
                    new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                DeviceClient device = new DeviceClient(binding, new EndpointAddress(_cameraAddress));

                AttachSecurity(device.Endpoint);
                SetupChannel(device.InnerChannel);

                address = device.GetReplayServiceAddress(Features);

                device.Close();

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("The DUT did not return Replay service address");
                }

            }, "Get Replay Service address");
            DoRequestDelay();
            return address;
        }

        protected ReplayPortClient ReplayClient
        {
            get
            {
                if (_replayClient == null)
                {
                    _replayClient = CreateReplayPortClient();
                }
                return _replayClient;
            }
        }

        void CloseReplayClient()
        {
            if (_replayClient != null)
            {
                ReplayClient.Close();
            }
        }

        #endregion

        #region RTP Extension Utils

        List<RtpPacket> _rtpPackets = new List<RtpPacket>();

        class RtpPacket
        {
            DateTime timeStamp;
            byte bitC;
            byte bitE;
            byte bitD;
            UInt32 CSeq;
            UInt32 sequenceNumber;
            bool forceShow = false;
            bool startsFrame;
            bool endsFrame;
            bool validLength;
            UInt32 timeStampRTP;

            DateTime timeCreated;

            public UInt32 SequenceNumber
            {
                get { return sequenceNumber; }
            }
            public UInt32 CSeqNum
            {
                get { return CSeq; }
            }
            public DateTime TimeStamp
            {
                get { return timeStamp; }
            }
            public UInt32 RTPTimeStamp
            {
                get { return timeStampRTP; }
            }
            public bool ForceShow
            {
                get { return forceShow; }
            }
            public bool IFrame
            {
                get { return (bitC == 1 || startsFrame); }
            }
            public bool IFrameStart
            {
                get { return (bitC == 1 && startsFrame && endsFrame); }
            }
            public bool OrdinaryFrameStart
            {
                get { return (bitC == 0 && startsFrame && !endsFrame); }
            }
            public DateTime TimeCreated
            {
                get { return timeCreated; }
            }

            public RtpPacket()
            {
                timeCreated = DateTime.Now;
            }

            public bool Parse(string message)
            {
                // DateTimeUnix.SecondsFraction.C.E.D.CSeq.SequenceNumber
                Match match = Regex.Match(
                    message,
                    @"^RTP=([0-9]+).([0-9]+).([0-1]).([0-1]).([0-1]).([0-9]+).([0-9]+).([0-1]).([0-1]).([0-1]).([0-1]).([0-9]+)");
                if (match.Success)
                {
                    try
                    {
                        UInt32 ntpSeconds = UInt32.Parse(match.Groups[1].Value);
                        UInt32 ntpFraction = UInt32.Parse(match.Groups[2].Value);
                        UInt32 milliseconds = (UInt32)(((Double)ntpFraction / UInt32.MaxValue) * 1000);
                        timeStamp = (new DateTime(1970, 1, 1, 0, 0, 0, 0))
                            .AddSeconds(ntpSeconds)
                            .AddMilliseconds(milliseconds);
                        bitC = Byte.Parse(match.Groups[3].Value);
                        bitE = Byte.Parse(match.Groups[4].Value);
                        bitD = Byte.Parse(match.Groups[5].Value);
                        CSeq = UInt32.Parse(match.Groups[6].Value);
                        sequenceNumber = UInt32.Parse(match.Groups[7].Value);
                        forceShow   = (Byte.Parse(match.Groups[8].Value) == 0);
                        startsFrame = (Byte.Parse(match.Groups[9].Value) == 1);
                        endsFrame   = (Byte.Parse(match.Groups[10].Value) == 1);
                        validLength = (Byte.Parse(match.Groups[11].Value) == 1);
                        timeStampRTP = UInt32.Parse(match.Groups[12].Value);
                        return true;
                    }
                    catch (Exception) { }
                }
                return false;
            }

            override public string ToString()
            {
                return string.Format("RTPtime={6} NTPtime={0} C={1} E={2} D={3} CSeq={4} SeqNum={5}",
                    timeStamp.ToString("yyyy.MM.dd HH:mm:ss.fff"),
                    bitC, bitE, bitD, CSeq, sequenceNumber, timeStampRTP, startsFrame, endsFrame);
            }

            public bool IsNextInSequence(RtpPacket rtpPacket)
            {
                return SequenceNumber > rtpPacket.SequenceNumber;
            }

            public bool IsNextInTime(RtpPacket rtpPacket)
            {
                return TimeStamp > rtpPacket.TimeStamp;
            }

            public bool IsNextInRTPTime(RtpPacket rtpPacket)
            {
                return RTPTimeStamp > rtpPacket.RTPTimeStamp;
            }

            public static bool operator ==(RtpPacket x, RtpPacket y)
            {
                // If both are null, or both are same instance, return true.
                if (System.Object.ReferenceEquals(x, y))
                {
                    return true;
                }
                // If one is null, but not both, return false.
                if (((object)x == null) || ((object)y == null))
                {
                    return false;
                }
                return ((x.TimeStamp == y.TimeStamp) && (x.SequenceNumber == y.SequenceNumber));
            }

            public static bool operator !=(RtpPacket x, RtpPacket y)
            {
                return !(x == y);
            }
        }

        #endregion

        #region Log Messages Parsing

        private RtpPacket _lastShownPacket = null;
        private RtpPacket _lastIFrame = null;

        protected bool HadleMessagePacket(string message)
        {
            RtpPacket rtpPacket = new RtpPacket();
            if (rtpPacket.Parse(message))
            {
                if ((_rtpPackets.Count == 0)
                    || (_lastShownPacket == null)
                    || ((rtpPacket.TimeCreated - _lastShownPacket.TimeCreated).TotalMilliseconds > PACKET_LOG_INTERVAL_MS)
                    || rtpPacket.ForceShow)
                {
                    _lastShownPacket = rtpPacket;
                    LogStepEvent(_lastShownPacket.ToString());
                }

                if (rtpPacket.IFrameStart)
                {
                    _lastIFrame = rtpPacket;
                }

                _rtpPackets.Add(rtpPacket);
                if ((_rtpPackets.Count > 1) 
                    && (!rtpPacket.IsNextInSequence(_rtpPackets[_rtpPackets.Count - 2]))
                    && (rtpPacket != _rtpPackets[_rtpPackets.Count - 2]))
                {
                    throw new VideoException("Error: wrong sequence numbers order in packets");
                }
                return true;
            }
            return false;
        }

        protected bool HadleMessagePause(string message)
        {
            UInt32 CSeq = 0;
            UInt32 lastTimestampNtp = 0;
            string date = string.Empty;
            Match match = Regex.Match(message, @"^PAUSE CSeq=([0-9]+) RTP=([0-9]+) Date=(.*)");
            if (match.Success)
            {
                CSeq = UInt32.Parse(match.Groups[1].Value);
                lastTimestampNtp = UInt32.Parse(match.Groups[2].Value);
                date = match.Groups[3].Value;
                DateTime lastTimestamp = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(lastTimestampNtp);

                LogStepEvent(string.Format("CSeq: {0} Date: {1}", CSeq, date));
                LogStepEvent(string.Format("RTP last timestamp: {0}", lastTimestamp.ToString("yyyy.MM.dd HH:mm:ss.fff")));

                DateTime dateParsed;
                if (!DateTime.TryParse(date, out dateParsed))
                {
                    throw new VideoException("Error: no valid Date field found");
                }
                if ((dateParsed - lastTimestamp).TotalSeconds > 1)
                {
                }
                if (CSeq == 0)
                {
                    throw new VideoException("Error: no valid CSeq field found");
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Tests Common

        protected RecordingInformation GetRecordingParameters()
        {
            RecordingInformation recInfo = null;
            RunStep(() => 
            { 
                recInfo = Client.GetRecordingInformation(_recordingToken); 
            }, "Get Recording Information");

            RunStep(() =>
            {
                if (recInfo == null)
                {
                    throw new AssertException("Recording not found");
                }
                if (recInfo.Track == null || recInfo.Track.Length == 0)
                {
                    throw new AssertException("Recording has no tracks");
                }

                if (!recInfo.EarliestRecordingSpecified)
                {
                    recInfo.EarliestRecording = recInfo.Track[0].DataFrom;
                }
                if (!recInfo.LatestRecordingSpecified)
                {
                    recInfo.LatestRecording = recInfo.Track[0].DataTo;
                }

                LogStepEvent("Recording Token = " + recInfo.RecordingToken);
                LogStepEvent("Recording Start = " + recInfo.EarliestRecording.ToUniversalTime());
                LogStepEvent("Recording End = " + recInfo.LatestRecording.ToUniversalTime());
            }, "Check Recording");


            return recInfo;
        }

        protected void AdjustVideo(string recordingToken, TransportProtocol protocol)
        {
            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            streamSetup.Stream = StreamType.RTPUnicast;

            MediaUri replayUri = new MediaUri();

            CreateReplayPortClient();

            RunStep(() => 
            { 
                replayUri.Uri = ReplayClient.GetReplayUri(streamSetup, recordingToken);
                LogStepEvent("Replay URI = " + replayUri.Uri);
            }, "Get Replay Uri");

            VideoUtils.AdjustVideo(_videoForm, _username, _password, _messageTimeout,
                                   streamSetup.Transport.Protocol, streamSetup.Stream,
                                   replayUri, null);
        }

        public void ReplaySequence(
            string setupFields, string playFields, string pauseFields, 
            Action<Action, Action, Action> actionControl)
        {
            _videoForm.CustomSetupFields = setupFields;
            _videoForm.CustomPlayFields = playFields;
            _videoForm.CustomPauseFields = pauseFields;
            _videoForm.ReplayMode = true;
            _videoForm.DoSetupOnReplay = false;
            _videoForm.ReplayMaxDuration = 5;

            _videoForm.EventSink = this;

            _videoForm.Replay((actionPlay, actionPause, actionTeardown) =>
            {
                VideoIsOpened = true;
                actionControl(
                    () => { _lastShownPacket = null; actionPlay(); Sleep(_operationDelay); },
                    () => { _lastShownPacket = null; actionPause(); Sleep(_operationDelay); },
                    () => { actionTeardown(); Sleep(_operationDelay); });
            });
        }

        public void Cleanup()
        {
            CloseReplayClient();
            _videoForm.CustomSetupFields = string.Empty;
            _videoForm.CustomPlayFields = string.Empty;
            _videoForm.CustomPauseFields = string.Empty;
            _videoForm.ReplayMode = false;
            _videoForm.DoSetupOnReplay = true;
            _videoForm.ReplayMaxDuration = 0;

            _handleLogMessage = null;
            _rtpPackets.Clear();
            _lastShownPacket = null;
            _lastIFrame = null;
            
            if (VideoIsOpened)
            {
                VideoIsOpened = false;
                _videoForm.CloseWindow();
                _videoForm.EventSink = null;
            }
        }

        private string RangeClock(System.DateTime startTime, System.DateTime? endTime, bool exactTime)
        {
            Func<System.DateTime, string> funcFormat = (time) => 
            {
                return time.ToString("yyyyMMdd") + "T" + time.ToString("HHmmss.fff") + "Z";
            };
            string startTimeStr = funcFormat(startTime);
            string endTimeStr = endTime.HasValue ? funcFormat(endTime.Value) : string.Empty;

            return string.Format("Range: clock={0}{1}{2}", startTimeStr, exactTime ? "" : "-", endTimeStr);
        }

        private string RangeClock(System.DateTime startTime, System.DateTime endTime)
        {
            return RangeClock(startTime, endTime, false);
        }

        private string RangeClock(System.DateTime startTime)
        {
            return RangeClock(startTime, null, false);
        }

        private string RangeClockExact(System.DateTime startTime)
        {
            return RangeClock(startTime, null, true);
        }

        private string MakeFieldsSet(string[] fields)
        {
            string result = "|";
            foreach (string field in fields)
            {
                result += field + CRLF;
            }
            return result;
        }

        protected void SimplePlaybackTest(bool reverse, TransportProtocol protocol)
        {
            _handleLogMessage = (message) =>
            {
                if (HadleMessagePacket(message))
                {
                    if (_rtpPackets.Count > 1)
                    {
                        RtpPacket prev = _rtpPackets[_rtpPackets.Count - 2];
                        RtpPacket curr = _rtpPackets[_rtpPackets.Count - 1];
                        if (!reverse && prev.IsNextInTime(prev))
                        {
                            throw new VideoException("Error: wrong NTP timestamps order in packets");
                        }
                        if (reverse && curr.IFrameStart && _lastIFrame != null && _lastIFrame != curr && !curr.IsNextInRTPTime(_lastIFrame))
                        {
                            throw new VideoException("Error: wrong RTP timestamps order in iframe packets");
                        }
                        if (reverse && curr.OrdinaryFrameStart && !prev.IsNextInRTPTime(curr))
                        {
                            throw new VideoException("Error: wrong RTP timestamps order in GOP packets");
                        }
                    }
                    return true;
                }
                return false;
            };

            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, protocol);

                _videoForm.ReplayReverse = reverse;

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                    MakeFieldsSet(new string[] 
                    { 
                        REQUIRE_ONVIF_REPLAY,
                        RangeClock(reverse ? recInfo.LatestRecording : recInfo.EarliestRecording),
                        reverse ? "Scale: -1.0" : string.Empty
                    }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                    });
            },
            () =>
            {
                _videoForm.ReplayReverse = false;
                Cleanup();
            });
        }

        #endregion

        /*[ Test( Name = "GETREPLAYURI COMMAND WITH INVALID RECORDING TOKEN",
                Order = "02.01.01",
                Id = "2-1-1",
                Category = Category.REPLAY,
                Path = "Replay\\General",
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]*/
        public void GetReplayUriWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                CreateReplayPortClient();

                string recordingToken = Guid.NewGuid().ToString().Substring(0, 8);

                RunStep(() =>
                    {
                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.UDP;
                        streamSetup.Stream = StreamType.RTPUnicast;
                        ReplayClient.GetReplayUri(streamSetup, recordingToken);
                    }, 
                    string.Format("Get Replay Uri with invalid recording token ({0})", recordingToken),
                    Definitions.Onvif.OnvifFaults.InvalidToken, true);
            },
            () =>
            {
                CloseReplayClient();
            });
        }

        #region Playback Tests

        [ Test( Name = "PLAYBACK VIDEO STREAMING – CONTROL MESSAGES",
                Order = "03.01.01",
                Id = "3-1-1",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingControlMessagesTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    return false;
                };

                _videoForm.OPTIONS = true;

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                    MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY, RangeClock(recInfo.EarliestRecording) }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                    });
            },
            () =>
            {
                _videoForm.OPTIONS = false;
                Cleanup();
            });
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – RTP-Unicast/UDP",
                Order = "03.01.02",
                Id = "3-1-2",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingRtpUnicastUdpTest()
        {
            SimplePlaybackTest(false, TransportProtocol.UDP);
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.01.03",
                Id = "3-1-3",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingRtpUnicastHttpTest()
        {
            SimplePlaybackTest(false, TransportProtocol.HTTP);
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – RTP/RTSP/TCP",
                Order = "03.01.04",
                Id = "3-1-4",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingRtpRtspTcpTest()
        {
            SimplePlaybackTest(false, TransportProtocol.RTSP);
        }

        [ Test( Name = "REVERSE PLAYBACK VIDEO STREAMING – RTP-Unicast/UDP",
                Order = "03.01.05",
                Id = "3-1-5",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void ReversePlayabckVideoStreamingRtpUnicastUdpTest()
        {
            SimplePlaybackTest(true, TransportProtocol.UDP);
        }

        [ Test( Name = "REVERSE PLAYBACK VIDEO STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.01.06",
                Id = "3-1-6",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void ReversePlayabckVideoStreamingRtpUnicastHttpTest()
        {
            SimplePlaybackTest(true, TransportProtocol.HTTP);
        }

        [ Test( Name = "REVERSE PLAYBACK VIDEO STREAMING – RTP/RTSP/TCP",
                Order = "03.01.07",
                Id = "3-1-7",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void Reverse()
        {
            SimplePlaybackTest(true, TransportProtocol.RTSP);
        }

        #endregion

        #region Pause Tests

        [ Test( Name = "PLAYBACK VIDEO STREAMING – PAUSE WITHOUT RANGE",
                Order = "03.01.08",
                Id = "3-1-8",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingPauseNoRangeTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    if (HadleMessagePause(message))
                    {
                        return true;
                    }
                    return false;
                };

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                      MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY, RangeClock(recInfo.EarliestRecording) })
                    + MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                        actionPause();
                        Sleep(2000);
                        actionPlay();
                    });
            },
            () =>
            {
                Cleanup();
            });
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – PAUSE WITH RANGE",
                Order = "03.01.09",
                Id = "3-1-9",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingPauseWithRangeTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    if (HadleMessagePause(message))
                    {
                        return true;
                    }
                    return false;
                };

                int delta = 8;
                _videoForm.ReplayPauseWait = delta - _videoForm.ReplayMaxDuration;

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                      MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY, RangeClock(recInfo.EarliestRecording) })
                    + MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY }),
                    MakeFieldsSet(new string[] { RangeClockExact(recInfo.EarliestRecording.AddSeconds(delta)) }),
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                        actionPause();
                        Sleep(2000);
                        actionPlay();
                    });
            },
            () =>
            {
                _videoForm.ReplayPauseWait = 0;
                Cleanup();
            });
        }

        #endregion

        [ Test( Name = "PLAYBACK VIDEO STREAMING – STOP OF PLAYING",
                Order = "03.01.10",
                Id = "3-1-10",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingStopOfPlayingTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    return false;
                };

                int delta = 5;

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                    MakeFieldsSet(new string[] 
                    { 
                        REQUIRE_ONVIF_REPLAY, 
                        RangeClock(recInfo.EarliestRecording, recInfo.EarliestRecording.AddSeconds(delta)) 
                    }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                    });
            },
            () =>
            {
                Cleanup();
            });
        }
        
        [ Test( Name = "PLAYBACK STREAMING – I-FRAMES",
                Order = "03.01.11",
                Id = "3-1-11",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingIFramesTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        if (!_rtpPackets[_rtpPackets.Count - 1].IFrame)
                        {
                            throw new VideoException("Error: only I-Frames allowed");
                        }
                        return true;
                    }
                    return false;
                };

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                    MakeFieldsSet(new string[] 
                    { 
                        REQUIRE_ONVIF_REPLAY, 
                        RangeClock(recInfo.EarliestRecording),
                        "Frames: intra"
                    }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                    });
            },
            () =>
            {
                Cleanup();
            });
        }
        
        [ Test( Name = "PLAYBACK STREAMING – RATECONTROL",
                Order = "03.01.12",
                Id = "3-1-12",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingRateControlTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    return false;
                };

                int delta = 5;

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                      MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(recInfo.EarliestRecording, recInfo.EarliestRecording.AddSeconds(delta)),
                          "Rate-Control: yes"
                      })
                    + MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(recInfo.EarliestRecording, recInfo.EarliestRecording.AddSeconds(delta)),
                          "Rate-Control: no"
                      }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                        Sleep(delta * 1000);
                        actionPlay();
                    });

                RunStep(() =>
                {
                    try
                    {
                        RtpPacket last = _rtpPackets[_rtpPackets.Count - 1];
                        RtpPacket first = _rtpPackets[0];
                        RtpPacket p1 = null;
                        RtpPacket p2 = null;
                        foreach (RtpPacket packet in _rtpPackets)
                        {
                            if (packet.CSeqNum == last.CSeqNum)
                            {
                                p2 = packet;
                                p1 = _rtpPackets[_rtpPackets.IndexOf(p2) - 1];
                                break;
                            }
                        }
                        int d1 = (p1.TimeStamp - first.TimeStamp).Seconds;
                        int d2 = (last.TimeStamp - p2.TimeStamp).Seconds;
                        LogStepEvent(string.Format("First: {0}s", d1));
                        LogStepEvent(string.Format("Second: {0}s", d2));
                        if (d1 > d2)
                        {
                            throw new VideoException("Error: second sequence is not longer then the first");
                        }
                    }
                    catch (Exception)
                    {
                        throw new VideoException("Error: second sequence is not longer then the first");
                    }
                }, "Check playback duration");

            },
            () =>
            {
                Cleanup();
            });
        }

        [ Test( Name = "PLAYBACK STREAMING – IMMEDIATE HEADER",
                Order = "03.01.13",
                Id = "3-1-13",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingImmediateHeaderTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    return false;
                };

                int delta = 5;

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                      MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(recInfo.EarliestRecording)
                      })
                    + MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(recInfo.EarliestRecording.AddSeconds(delta)),
                          "Immediate: yes"
                      }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                        Sleep(delta * 1000);
                        actionPlay();
                    });
            },
            () =>
            {
                Cleanup();
            });
        }

        [ Test( Name = "PLAYBACK STREAMING – SEEK",
                Order = "03.01.14",
                Id = "3-1-14",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { } ) ]
        public void PlayabckVideoStreamingSeekTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    return false;
                };

                int delta = 5;
                DateTime middleT1 = recInfo.EarliestRecording.AddMinutes(1);

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                      MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(recInfo.EarliestRecording),
                          "Rate-Control: no"
                      })
                    + MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(middleT1),
                          "Rate-Control: no",
                          "Immediate: yes"
                      })
                    + MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(middleT1.AddSeconds(-delta)),
                          "Rate-Control: no",
                          "Immediate: yes"
                      })
                    + MakeFieldsSet(new string[] 
                      { 
                          REQUIRE_ONVIF_REPLAY, 
                          RangeClock(middleT1.AddSeconds(delta)),
                          "Rate-Control: no",
                          "Immediate: yes"
                      }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        RunStep(
                            () => { LogStepEvent(RangeClock(recInfo.EarliestRecording)); }, 
                            "Setting Range Clock Values");
                        actionPlay();
                        RunStep(
                            () => { LogStepEvent(RangeClock(middleT1)); },
                            "Setting Range Clock Values");
                        actionPlay();
                        RunStep(
                            () => { LogStepEvent(RangeClock(middleT1.AddSeconds(-delta))); },
                            "Setting Range Clock Values");
                        actionPlay();
                        RunStep(
                            () => { LogStepEvent(RangeClock(middleT1.AddSeconds(delta))); },
                            "Setting Range Clock Values");
                        actionPlay();
                    });
            },
            () =>
            {
                Cleanup();
            });
        }

        #region IVideoFormEvent

        public void FireBeginStep(string Name)
        {
            BeginStep(Name);
        }
        public void FireStepPassed()
        {
            StepPassed();
        }
        public void FireLogStepEvent(string Message)
        {
            if (_handleLogMessage != null)
            {
                if (_handleLogMessage(Message))
                {
                    return;
                }
            }
            LogStepEvent(Message);
        }

        #endregion
    }
}
