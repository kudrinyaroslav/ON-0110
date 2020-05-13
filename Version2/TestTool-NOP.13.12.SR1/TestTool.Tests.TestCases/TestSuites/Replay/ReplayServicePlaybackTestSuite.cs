using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Enums;
using DateTime = System.DateTime;
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
using System.Linq;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class ReplayServicePlaybackTestSuite : Base.SearchTest, IVideoFormEvent
    {
        public ReplayServicePlaybackTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        protected bool VideoIsOpened = false;
        protected const string CRLF = "$$";
        protected const string REQUIRE_ONVIF_REPLAY = "Require: onvif-replay";
        protected const int PACKET_LOG_INTERVAL_MS = 500;
        protected const int MIN_PACKETS_NUM_TO_SHOW = 10;
        protected int packetsShowed = 0;
        protected Func<string, bool> _handleLogMessage = null;

        const string stepWaitStream = "Wait Stream";
        bool waitStreamStarted = false;

        #region Replay Client

        private ReplayPortClient _replayClient;

        protected ReplayPortClient CreateReplayPortClient()
        {
            string address = GetReplayServiceAddress();

            BeginStep("Connect to Replay service");
            LogStepEvent(string.Format("Replay service address: {0}", address));
            if (string.IsNullOrEmpty(address))
            {
                throw new AssertException("Replay service not supported");
            }
            else
            {
                if (!address.IsValidUrl())
                {
                    throw new AssertException("Replay service address is invalid");
                }
            }
            Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(ReplaySchemasSet.GetInstance()) });
            ReplayPortClient client = new ReplayPortClient(binding, new EndpointAddress(address));

            AttachSecurity(client.Endpoint);
            SetupChannel(client.InnerChannel);

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

        protected Dictionary<byte, List<RtpPacket>> _rtpPackets = new Dictionary<byte, List<RtpPacket>>();

        protected class RtpPacket
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
            byte payload;

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
            public byte Payload
            {
                get { return payload; }
            }
            public bool Discontinuity
            {
                get { return (bitD == 1); }
            }

            public RtpPacket()
            {
                timeCreated = DateTime.Now;
            }

            public bool Parse(string message)
            {
                // Payload:DateTimeUnix.SecondsFraction.C.E.D.CSeq.SequenceNumber.CanSkip.StartsFrame.EndsFrame.ValidLength.RTPTimeStamp.CreationTime
                Match match = Regex.Match(
                    message,
                    @"^([0-9]+):([0-9]+).([0-9]+).([0-1]).([0-1]).([0-1]).([0-9]+).([0-9]+).([0-1]).([0-1]).([0-1]).([0-1]).([0-9]+).([0-9]+)");
                if (match.Success)
                {
                    try
                    {
                        int m = 0;
                        payload = Byte.Parse(match.Groups[++m].Value);
                        UInt32 ntpSeconds = UInt32.Parse(match.Groups[++m].Value);
                        UInt32 ntpFraction = UInt32.Parse(match.Groups[++m].Value);
                        UInt32 milliseconds = (UInt32)(((Double)ntpFraction / UInt32.MaxValue) * 1000);
                        timeStamp = (new DateTime(1970, 1, 1, 0, 0, 0, 0))
                            .AddSeconds(ntpSeconds)
                            .AddMilliseconds(milliseconds);
                        bitC = Byte.Parse(match.Groups[++m].Value);
                        bitE = Byte.Parse(match.Groups[++m].Value);
                        bitD = Byte.Parse(match.Groups[++m].Value);
                        CSeq = UInt32.Parse(match.Groups[++m].Value);
                        sequenceNumber = UInt32.Parse(match.Groups[++m].Value);
                        forceShow = (Byte.Parse(match.Groups[++m].Value) == 0);
                        startsFrame = (Byte.Parse(match.Groups[++m].Value) == 1);
                        endsFrame = (Byte.Parse(match.Groups[++m].Value) == 1);
                        validLength = (Byte.Parse(match.Groups[++m].Value) == 1);
                        timeStampRTP = UInt32.Parse(match.Groups[++m].Value);
                        UInt64 creationTime = UInt64.Parse(match.Groups[++m].Value);
                        timeCreated = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(0.001 * creationTime);
                        return true;
                    }
                    catch (Exception) { }
                }
                return false;
            }

            override public string ToString()
            {
                return string.Format("RTPtime={6} NTPtime={0} C={1} E={2} D={3} CSeq={4} SeqNum={5} Payload={7}",
                    timeStamp.ToString("yyyy.MM.dd HH:mm:ss.fff"),
                    bitC, bitE, bitD, CSeq, sequenceNumber, timeStampRTP, payload);
            }

            public bool IsNextInSequence(RtpPacket rtpPacket)
            {
                UInt32 Tolerance = 2000;
                if (SequenceNumber > rtpPacket.SequenceNumber)
                {
                    return true;
                }
                if (rtpPacket.SequenceNumber > 65536 - Tolerance)
                {
                    return SequenceNumber < Tolerance;
                }
                return false;
                //return SequenceNumber > rtpPacket.SequenceNumber;
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

        private Dictionary<byte, RtpPacket> _lastShownPacket = new Dictionary<byte,RtpPacket>();
        private Dictionary<byte, RtpPacket> _lastIFrame = new Dictionary<byte, RtpPacket>();

        void ShowLastPackets()
        {
          foreach (byte i in _rtpPackets.Keys)
          {
            List<RtpPacket> rtpPackets = _rtpPackets[i];
            if (rtpPackets == null)
            {
              continue;
            }
            if (rtpPackets.Count < 1)
            {
              continue;
            }
            RtpPacket rtpPacket = rtpPackets[rtpPackets.Count - 1];
            LogStepEvent(rtpPacket.ToString() + string.Format(", Captured as # {0}", rtpPackets.Count) + Environment.NewLine);
          }
        }
        protected bool HadleMessagePacket(string message, out RtpPacket packetOut)
        {
            RtpPacket rtpPacket = new RtpPacket();
            if (rtpPacket.Parse(message))
            {
                byte payload = rtpPacket.Payload;
                if (!_rtpPackets.ContainsKey(payload))
                {
                    _rtpPackets.Add(payload, new List<RtpPacket>());
                }
                List<RtpPacket> rtpPackets = _rtpPackets[payload];
                if ((rtpPackets.Count == 0)
                    || !_lastShownPacket.ContainsKey(payload)
                    || (packetsShowed < MIN_PACKETS_NUM_TO_SHOW)
                    || ((rtpPacket.TimeCreated - _lastShownPacket[payload].TimeCreated).TotalMilliseconds > PACKET_LOG_INTERVAL_MS)
                    || rtpPacket.ForceShow)
                {
                    if (waitStreamStarted && !(_lastShownPacket.ContainsKey(payload) && (_lastShownPacket[payload] == rtpPacket)))
                    {
                        _lastShownPacket[payload] = rtpPacket;
                        LogStepEvent(rtpPacket.ToString() + string.Format(", Captured as # {0}", rtpPackets.Count));
                        ++packetsShowed;
                    }
                }

                if (rtpPacket.IFrameStart)
                {
                    _lastIFrame[payload] = rtpPacket;
                }

                rtpPackets.Add(rtpPacket);
                if ((rtpPackets.Count > 1)
                    && (!rtpPacket.IsNextInSequence(rtpPackets[rtpPackets.Count - 2]))
                    && (rtpPacket != rtpPackets[rtpPackets.Count - 2]))
                {
                    throw new VideoException("Error: wrong sequence numbers order in packets");
                }
                packetOut = rtpPacket;
                return true;
            }
            packetOut = null;
            return false;
        }

        protected bool HadleMessagePacket(string message)
        {
            RtpPacket rtpPacket;
            return HadleMessagePacket(message, out rtpPacket);
        }

        protected bool HadleMessagePause(string message, DateTime? pauseTime)
        {
            UInt32 CSeq = 0;
            string date = string.Empty;
            Match match = Regex.Match(message, @"^PAUSE CSeq=([0-9]+) NTP=([0-9]+) NTPSec=([0-9]+) Date=(.*)");
            if (match.Success)
            {
                CSeq = UInt32.Parse(match.Groups[1].Value);
                UInt32 ntpSeconds = UInt32.Parse(match.Groups[2].Value);
                UInt32 ntpFraction = UInt32.Parse(match.Groups[3].Value);
                UInt32 milliseconds = (UInt32)(((Double)ntpFraction / UInt32.MaxValue) * 1000);
                DateTime lastTimestamp = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(ntpSeconds).AddMilliseconds(milliseconds);
                date = match.Groups[4].Value;

                LogStepEvent(string.Format("CSeq: {0} Date: {1}", CSeq, date));
                LogStepEvent(string.Format("Last packet NTP timestamp: {0}", lastTimestamp.ToString("yyyy.MM.dd HH:mm:ss.fff")));

                DateTime dateParsed = DateTime.MinValue;
                //If "Date" field is present but has invalid value.
                if (!string.IsNullOrEmpty(date) && !DateTime.TryParse(date, out dateParsed))
                {
                    throw new VideoException("Error: no valid Date field found");
                }
                if (CSeq == 0)
                {
                    throw new VideoException("Error: no valid CSeq field found");
                }
                if ((pauseTime.HasValue || DateTime.MinValue != dateParsed) && PacketsAfterTime(pauseTime.HasValue ? pauseTime.Value : dateParsed))
                {
                    throw new VideoException("Error: The received stream NTP timestamp exceeds the time specified in the PAUSE command");
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

        private void ReplaySequence(
            string setupFields, string playFields, string pauseFields,
            Action<Action, Action, Action> actionControl,
            bool useVideo, bool useAudio, bool useMeta)
        {
            _videoForm.CustomSetupFields = setupFields;
            _videoForm.CustomPlayFields = playFields;
            _videoForm.CustomPauseFields = pauseFields;
            _videoForm.ReplayMode = true;
            _videoForm.DoSetupOnReplay = false;
            _videoForm.ReplayMaxDuration = 8;

            _videoForm.EventSink = this;

            _videoForm.Replay((actionPlay, actionPause, actionTeardown) =>
            {
                VideoIsOpened = true;
                actionControl(
                    () => { _lastShownPacket.Clear(); packetsShowed = 0; actionPlay(); },
                    () => { _lastShownPacket.Clear(); actionPause(); },
                    () => { actionTeardown(); });
            }, useVideo, useAudio, useMeta);
        }

        protected void ReplayVideoSequence(
            string setupFields, string playFields, string pauseFields,
            Action<Action, Action, Action> actionControl)
        {
            ReplaySequence(setupFields, playFields, pauseFields, actionControl, true, false, false);
        }

        protected void ReplayAudioSequence(
            string setupFields, string playFields, string pauseFields,
            Action<Action, Action, Action> actionControl)
        {
            ReplaySequence(setupFields, playFields, pauseFields, actionControl, false, true, false);
        }

        protected void ReplayMetadataSequence(
            string setupFields, string playFields, string pauseFields,
            Action<Action, Action, Action> actionControl)
        {
            ReplaySequence(setupFields, playFields, pauseFields, actionControl, false, false, true);
        }

        protected void ReplayAllSequence(
            string setupFields, string playFields, string pauseFields,
            Action<Action, Action, Action> actionControl)
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);

            ReplaySequence(setupFields, playFields, pauseFields, actionControl, true, audioSupported, metadataSupported);
        }

        public void ClearPackets()
        {
          _rtpPackets.Clear();
          _lastShownPacket.Clear();
          _lastIFrame.Clear();
        }
        public void Cleanup()
        {
            CloseReplayClient();

            _handleLogMessage = null;
            _rtpPackets.Clear();
            _lastShownPacket.Clear();
            _lastIFrame.Clear();

            if (VideoIsOpened)
            {
                VideoIsOpened = false;
                _videoForm.CloseWindow();
                _videoForm.EventSink = null;
            }
        }

        protected string RangeClock(System.DateTime startTime, System.DateTime? endTime, bool exactTime)
        {
            Func<System.DateTime, string> funcFormat = (time) =>
            {
                return time.ToString("yyyyMMdd") + "T" + time.ToString("HHmmss.fff") + "Z";
            };
            string startTimeStr = funcFormat(startTime);
            string endTimeStr = endTime.HasValue ? funcFormat(endTime.Value) : string.Empty;

            return string.Format("Range: clock={0}{1}{2}", startTimeStr, exactTime ? "" : "-", endTimeStr);
        }

        protected string RangeClock(System.DateTime startTime, System.DateTime endTime)
        {
            return RangeClock(startTime, endTime, false);
        }

        protected string RangeClock(System.DateTime startTime)
        {
            return RangeClock(startTime, null, false);
        }

        protected string RangeClockExact(System.DateTime startTime)
        {
            // fix by AR for https://wush.net/trac/onvif-ext/ticket/197
            return RangeClock(startTime, null, false);
            //return RangeClock(startTime, null, true);
        }

        protected string MakeFieldsSet(string[] fields)
        {
            string result = "|";
            foreach (string field in fields)
            {
                result += field + CRLF;
            }
            return result;
        }

        protected bool PacketsAfterTime(DateTime dateTime)
        {
            foreach (List<RtpPacket> rtpPackets in _rtpPackets.Values)
            {
                RtpPacket last = rtpPackets[rtpPackets.Count - 1];
                if (last.TimeStamp > dateTime)
                {
                    return true;
                }
            }
            return false;
        }
        
        private void SimplePlaybackTest(bool reverse, TransportProtocol protocol, bool useVideo, bool useAudio, bool useMeta)
        {
            _handleLogMessage = (message) =>
            {
                RtpPacket rtpPacket;
                if (HadleMessagePacket(message, out rtpPacket))
                {
                    byte payload = rtpPacket.Payload;
                    List<RtpPacket> rtpPackets = _rtpPackets[payload];
                    if (rtpPackets.Count > 1)
                    {
                        RtpPacket prev = rtpPackets[rtpPackets.Count - 2];
                        RtpPacket curr = rtpPackets[rtpPackets.Count - 1];
                        if (!reverse && prev.IsNextInTime(prev))
                        {
                            throw new VideoException("Error: wrong NTP timestamps order in packets");
                        }
                        if (reverse && curr.IFrameStart && _lastIFrame.ContainsKey(payload)
                            && _lastIFrame[payload] != curr && !curr.IsNextInRTPTime(_lastIFrame[payload]))
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
                    }, useVideo, useAudio, useMeta);
            },
            () =>
            {
                Cleanup();
            });
        }

        protected void SimpleVideoPlaybackTest(bool reverse, TransportProtocol protocol)
        {
            SimplePlaybackTest(reverse, protocol, true, false, false);
        }

        protected void SimpleAudioPlaybackTest(bool reverse, TransportProtocol protocol)
        {
            SimplePlaybackTest(reverse, protocol, false, true, false);
        }

        protected void SimpleMetaPlaybackTest(bool reverse, TransportProtocol protocol)
        {
            SimplePlaybackTest(reverse, protocol, false, false, true);
        }

        protected void SimpleAllPlaybackTest(bool reverse, TransportProtocol protocol)
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);
            SimplePlaybackTest(reverse, protocol, true, audioSupported, metadataSupported);
        }

        protected void ReplayPauseTest(bool withRange, bool useVideo, bool useAudio, bool useMeta)
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                int delta = 8;

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    if (withRange 
                        ? HadleMessagePause(message, recInfo.EarliestRecording.AddSeconds(delta))
                        : HadleMessagePause(message, null))
                    {
                        return true;
                    }
                    return false;
                };

                if (withRange)
                {
                    _videoForm.ReplayPauseWait = delta - _videoForm.ReplayMaxDuration;

                    RunStep(() =>
                    {
                        LogStepEvent(string.Format("From: {0}", recInfo.EarliestRecording));
                        LogStepEvent(string.Format("To: {0}", recInfo.EarliestRecording.AddSeconds(delta)));
                    }, "Set range");
                }

                ReplaySequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                      MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY, RangeClock(recInfo.EarliestRecording) })
                    + MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY }),
                    (withRange ? MakeFieldsSet(new string[] { RangeClockExact(recInfo.EarliestRecording.AddSeconds(delta)) }) : ""),
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                        actionPause();
                        Sleep(2000);
                        actionPlay();
                    },
                    useVideo, useAudio, useMeta);
            },
            () =>
            {
                Cleanup();
            });
        }

        protected void ReplayStopOfPlayingTest(bool useVideo, bool useAudio, bool useMeta)
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

                int delta = 4;

                RunStep(() =>
                {
                    LogStepEvent(string.Format("From: {0}", recInfo.EarliestRecording));
                    LogStepEvent(string.Format("To: {0}", recInfo.EarliestRecording.AddSeconds(delta)));
                }, "Set range");

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
                    },
                    useVideo, useAudio, useMeta);

                RunStep(() =>
                {
                    if (PacketsAfterTime(recInfo.EarliestRecording.AddSeconds(delta)))
                    {
                        throw new VideoException("Error: The received stream NTP timestamp is out of requested range (received NTP timestamp > end time of the requested range)");
                    }
                }, "Timestamp range check");
            },
            () =>
            {
                Cleanup();
            });
        }

        protected void ReplayIFramesTest(bool useVideo, bool useAudio, bool useMeta)
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    RtpPacket rtpPacket;
                    if (HadleMessagePacket(message, out rtpPacket))
                    {
                        if (!rtpPacket.IFrame)
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
                    },
                    useVideo, useAudio, useMeta);
            },
            () =>
            {
                Cleanup();
            });
        }

        protected void ReplayRateControlTest(bool useVideo, bool useAudio, bool useMeta)
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

                int delta = 3;
                if (!useVideo && !useAudio)
                {
                    delta = 15;
                }

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
                    },
                    useVideo, useAudio, useMeta);

                RunStep(() =>
                {
                    double d1 = 1;
                    double d2 = 0;
                    try
                    {
                        List<RtpPacket> rtpPackets = _rtpPackets.Values.First();
                        RtpPacket last = rtpPackets[rtpPackets.Count - 1];
                        RtpPacket first = rtpPackets[0];
                        RtpPacket p1 = null;
                        RtpPacket p2 = null;
                        foreach (RtpPacket packet in rtpPackets)
                        {
                            if (packet.CSeqNum == last.CSeqNum)
                            {
                                p2 = packet;
                                p1 = rtpPackets[rtpPackets.IndexOf(p2) - 1];
                                if (first == p1 || p2 == last)
                                {
                                    throw new VideoException("Impossible to calculate playback duration from one frame!");
                                }
                                break;
                            }
                        }
                        d1 = (p1.TimeCreated - first.TimeCreated).TotalSeconds;
                        d2 = (last.TimeCreated - p2.TimeCreated).TotalSeconds;
                        LogStepEvent(string.Format("First: {0:F2}s", d1));
                        LogStepEvent(string.Format("Second: {0:F2}s", d2));
                    }
                    catch (VideoException ve)
                    {
                        throw ve;
                    }
                    catch (Exception)
                    {
                        throw new VideoException("Error: not enough packets for checking");
                    }
                    if (!(d2 < d1))
                    {
                        throw new VideoException("Error: second sequence is not less than the first");
                    }
                }, "Check playback duration");

            },
            () =>
            {
                Cleanup();
            });
        }

        protected void ReplayImmediateHeaderTest(bool useVideo, bool useAudio, bool useMeta)
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
                    },
                    useVideo, useAudio, useMeta);

                RunStep(() =>
                {
                    List<RtpPacket> rtpPackets = _rtpPackets.Values.First();
                    RtpPacket first = rtpPackets[0];
                    foreach (RtpPacket packet in rtpPackets)
                    {
                        // Check first packet of second play sequence
                        if (packet.CSeqNum > first.CSeqNum)
                        {
                            if (!packet.Discontinuity)
                            {
                                throw new VideoException("Error: first packet did not have the D (discontinuity) bit set");
                            }
                            break;
                        }
                    }
                }, "Check first packet from the new location");
            },
            () =>
            {
                Cleanup();
            });
        }

        protected void ReplaySeekTest(bool useVideo, bool useAudio, bool useMeta)
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
                        ClearPackets();
                        actionPlay();
                        ShowLastPackets();
                        RunStep(
                            () => { LogStepEvent(RangeClock(middleT1)); },
                            "Setting Range Clock Values");
                        ClearPackets();
                        actionPlay();
                        ShowLastPackets();
                        RunStep(
                            () => { LogStepEvent(RangeClock(middleT1.AddSeconds(-delta))); },
                            "Setting Range Clock Values");
                        ClearPackets();
                        actionPlay();
                        ShowLastPackets();
                        RunStep(
                            () => { LogStepEvent(RangeClock(middleT1.AddSeconds(delta))); },
                            "Setting Range Clock Values");
                        ClearPackets();
                        actionPlay();
                        ShowLastPackets();
                    },
                    useVideo, useAudio, useMeta);
            },
            () =>
            {
                Cleanup();
            });
        }

        #endregion

        #region IVideoFormEvent

        public void FireBeginStep(string Name)
        {
            BeginStep(Name);
            waitStreamStarted = (stepWaitStream == Name);
        }
        public void FireStepPassed()
        {
            StepPassed();
        }
        public void FireLogStepEvent(string Message)
        {
            bool rtspCommand = false;

            if (Message.StartsWith("DESCRIBE_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "DESCRIBE_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("DESCRIBE_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "DESCRIBE_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("SETUP_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "SETUP_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("SETUP_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "SETUP_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PLAY_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PLAY_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PLAY_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PLAY_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("TEARDOWN_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "TEARDOWN_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("TEARDOWN_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "TEARDOWN_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PAUSE_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PAUSE_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PAUSE_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PAUSE_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("OPTIONS_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "OPTIONS_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("OPTIONS_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "OPTIONS_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }

            if (rtspCommand) return;

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
