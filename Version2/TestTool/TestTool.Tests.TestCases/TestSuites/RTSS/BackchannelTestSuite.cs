///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Threading;
using System.ServiceModel;
using System.Runtime.InteropServices;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;
using System.ServiceModel;
using TestTool.Tests.Common.Media;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.TestCases.Utils;

namespace TestTool.Tests.TestCases.TestSuites
{
#if __PROFILE_A__
    [TestClass]
#else
#endif
    class BackchannelTestSuite : RtspTestSuite
    {
        //private const string DllFilePath = "C:\\Users\\alexanderr\\Downloads\\live555_20140206-master\\live\\build\\Debug\\backlib.dll";
        private const string DllFilePath = "backlib.dll";

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static int InitBackchannelCall1(int number);

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static int GetBackchannelStep(StringBuilder Step);

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static int GetBackchannelError(StringBuilder Error);

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static int Stop();

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static int InitBackchannelCall(string Netpath, string Filepath);

        private const string PATH_U = "Real Time Streaming\\Audio Backchannel\\Unicast";
        private const string PATH_M = "Real Time Streaming\\Audio Backchannel\\Multicast";

        NetworkConfiguration NetConfig;
        static int aaa;
        public BackchannelTestSuite(TestLaunchParam param)
            : base(param)
        {
            NetConfig = new NetworkConfiguration(param, this);
        }

      /// <summary>
        /// Ends the test.
        /// </summary>
        /// <param name="status"></param>
        public override void EndTest(TestStatus status)
        {
            base.EndTest(status);
            NetConfig.Release(this);
        }

      public void ReConnectTo(string ServiceAddress)
        {
            _endpointController.UpdateAddress(new EndpointAddress(ServiceAddress));
        }

      private Profile FindSuitableProfile(MediaConfigurationChangeLog changeLog, string Codec)
        {
          Profile[] Profiles = GetProfiles();
          if (Profiles == null)
          {
            return null;
          }
          foreach (Profile p in Profiles)
          {
            if (p.Extension == null)
            {
              continue;
            }
            if (p.Extension.AudioDecoderConfiguration == null || p.Extension.AudioOutputConfiguration == null)
            {
              continue;
            }
            return p;
          }
          return null;
        }

      private bool TuneBackchannelProfile(Profile profile, MediaConfigurationChangeLog changeLog, string Codec)
        {
          AudioOutputConfiguration o = profile.Extension.AudioOutputConfiguration;
          if ((o.SendPrimacy != null) && (o.SendPrimacy == "www.onvif.org/ver20/HalfDuplex/Server"))
          {
            AudioOutputConfigurationOptions opt = GetAudioOutputConfigurationOptions(o.token, null);
            if (opt != null && opt.SendPrimacyOptions != null)
            {
              string s = opt.SendPrimacyOptions.FirstOrDefault(c => c != "www.onvif.org/ver20/HalfDuplex/Server");
              if (s != null)
              {
                o.SendPrimacy = s;
                changeLog.TrackModifiedConfiguration(o);
                SetAudioOutputConfiguration(o, false);
              }
            }
          }
          if ((o.SendPrimacy != null) && (o.SendPrimacy == "www.onvif.org/ver20/HalfDuplex/Server"))
          {
            return false;
          }
          AudioDecoderConfiguration d = profile.Extension.AudioDecoderConfiguration;
          AudioDecoderConfigurationOptions Options = GetAudioDecoderConfigurationOptions(d.token, profile.token);
          if (Options == null)
          {
            return false;
          }
          if (Codec == "G711" && Options.G711DecOptions == null)
          {
            return false;
          }
          if (Codec == "G726" && Options.G726DecOptions == null)
          {
            return false;
          }
          if (Codec == "AAC" && Options.AACDecOptions == null)
          {
            return false;
          }
          return true;
        }

      private void PrepareBackchannelProfile(Profile profile, MediaConfigurationChangeLog changeLog, string Codec)
        {
          AudioOutputConfiguration[] Outputs = GetAudioOutputConfigurations();
          if (Outputs == null)
          {
            Assert(false, "No AudioOutputConfigurations available", "Checking AudioOutputConfigurations");
          }
          foreach (AudioOutputConfiguration o in Outputs)
          {
            if ((o.SendPrimacy != null) && (o.SendPrimacy == "www.onvif.org/ver20/HalfDuplex/Server"))
            {
              AudioOutputConfigurationOptions opt = GetAudioOutputConfigurationOptions(o.token, null);
              if (opt != null && opt.SendPrimacyOptions != null)
              {
                string s = opt.SendPrimacyOptions.FirstOrDefault(c => c != "www.onvif.org/ver20/HalfDuplex/Server");
                if (s != null)
                {
                  o.SendPrimacy = s;
                  changeLog.TrackModifiedConfiguration(o);
                  SetAudioOutputConfiguration(o, false);
                }
              }
            }
            if ((o.SendPrimacy != null) && (o.SendPrimacy == "www.onvif.org/ver20/HalfDuplex/Server"))
            {
              continue;
            }
            AddAudioOutputConfiguration(profile.token, o.token);
            AudioDecoderConfiguration[] Decoders = GetCompatibleAudioDecoderConfigurations(profile.token);
            if (Decoders == null)
            {
              Assert(false, "No AudioDecoderConfigurations available", "Checking AudioDecoderConfigurations");
            }
            bool DecoderFound = false;
            foreach (AudioDecoderConfiguration d in Decoders)
            {
              AudioDecoderConfigurationOptions Options = GetAudioDecoderConfigurationOptions(d.token, profile.token);
              if (Options == null)
              {
                continue;
              }
              if (Codec == "G711" && Options.G711DecOptions == null)
              {
                continue;
              }
              if (Codec == "G726" && Options.G726DecOptions == null)
              {
                continue;
              }
              if (Codec == "AAC" && Options.AACDecOptions == null)
              {
                continue;
              }
              AddAudioDecoderConfiguration(profile.token, d.token);
              DecoderFound = true;
              break;
            }
            Assert(DecoderFound, "No suitable AudioDecoder available", "Checking AudioDecoderConfigurationOptions");
          }
        }

      private void DoSequence(string codec, string filename, MediaUri Uri, StreamType streamType, TransportProtocol protocol)
        {
          if (NewGenVideo == null)
          {
            NewGenVideo = new VideoContainer2();
          }
          VideoUtils.AdjustGeneral2(NewGenVideo, _username, _password, MessageTimeout, protocol, streamType, Uri);
          VideoUtils.AdjustBackchannel2(NewGenVideo, codec, filename);
          NewGenVideo.EventSink = this;
          NewGenVideo.SetSequence(4);
          NewGenVideo.SilentRun();
          NewGenVideo.EventSink = null;
        }

      private void TestByCodecAndTransport(string codec, string filename, StreamType streamType, TransportProtocol protocol)
        {
          Profile deletedProfile = null;
          Profile createdProfile = null;
          Profile modifiedProfile = null;
          Profile profile = null;

          MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

          RunTest(
          () =>
          {
            profile = FindSuitableProfile(changeLog, codec);
            if ((profile == null) || !TuneBackchannelProfile(profile, changeLog, codec))
            {
              profile = CreateProfileByAnnex3("testprofileX", null, out deletedProfile, out createdProfile, out modifiedProfile);
              PrepareBackchannelProfile(profile, changeLog, codec);
            }
            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Stream = streamType;
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            MediaUri uri = GetStreamUri(streamSetup, profile.token);

            DoSequence(codec, filename, uri, streamType, protocol);

          },
          () =>
          {
            RestoreMediaConfiguration(changeLog);
            RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
          }
          );
        }

      // AAC
      [Test(Name = "BACKCHANNEL – AAC (RTP-Multicast/UDP, IPv4)",
        Path = PATH_M,
        Id = "6-2-3",
        LastChangedIn = "v15.06",
        Category = Category.RTSS,
        Version = 2.0,
        RequirementLevel = RequirementLevel.Optional,
        RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputAAC, Feature.RTPMulticastUDP },
        FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannelAACRtpMulticastUdp()
      {
        TestByCodecAndTransport("AAC", "test.aac", StreamType.RTPMulticast, TransportProtocol.UDP);
      }

      [Test(Name = "BACKCHANNEL – AAC (RTP-Unicast/UDP, IPv4)",
               Path = PATH_U,
               Id = "6-1-7",
               LastChangedIn = "v15.06",
               Category = Category.RTSS,
               Version = 2.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputAAC },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtBackchannelAACRtpUnicastUdp()
        {
          TestByCodecAndTransport("AAC", "test.aac", StreamType.RTPUnicast, TransportProtocol.UDP);
        }

      [Test(Name = "BACKCHANNEL – AAC (RTP-Unicast/RTSP/HTTP/TCP, IPv4)",
               Path = PATH_U,
               Id = "6-1-8",
               LastChangedIn = "v15.06",
               Category = Category.RTSS,
               Version = 2.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputAAC },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtBackchannelAACRtpUnicastHTTP()
        {
          TestByCodecAndTransport("AAC", "test.aac", StreamType.RTPUnicast, TransportProtocol.HTTP);
        }

      [Test(Name = "BACKCHANNEL – AAC (RTP/RTSP/TCP, IPv4)",
           Path = PATH_U,
           Id = "6-1-9",
           LastChangedIn = "v15.06",
           Category = Category.RTSS,
           Version = 2.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputAAC, Feature.RTPRTSPTCP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtBackchannelAACRtpUnicastRTSP()
        {
          TestByCodecAndTransport("AAC", "test.aac", StreamType.RTPUnicast, TransportProtocol.RTSP);
        }

      // 711
      [Test(Name = "BACKCHANNEL – G.711 (RTP-Multicast/UDP, IPv4)",
        Path = PATH_M,
        Id = "6-2-1",
        LastChangedIn = "v15.06",
        Category = Category.RTSS,
        Version = 2.0,
        RequirementLevel = RequirementLevel.Optional,
        RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG711, Feature.RTPMulticastUDP },
        FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel711RtpMulticastUdp()
      {
        TestByCodecAndTransport("G711", "test.711", StreamType.RTPMulticast, TransportProtocol.UDP);
      }

      [Test(Name = "BACKCHANNEL – G.711 (RTP-Unicast/UDP, IPv4)",
               Path = PATH_U,
               Id = "6-1-1",
               LastChangedIn = "v15.06",
               Category = Category.RTSS,
               Version = 2.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG711 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel711RtpUnicastUdp()
      {
        TestByCodecAndTransport("G711", "test.711", StreamType.RTPUnicast, TransportProtocol.UDP);
      }

      [Test(Name = "BACKCHANNEL – G.711 (RTP-Unicast/RTSP/HTTP/TCP, IPv4)",
               Path = PATH_U,
               Id = "6-1-2",
               LastChangedIn = "v15.06",
               Category = Category.RTSS,
               Version = 2.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG711 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel711RtpUnicastHTTP()
      {
        TestByCodecAndTransport("G711", "test.711", StreamType.RTPUnicast, TransportProtocol.HTTP);
      }

      [Test(Name = "BACKCHANNEL – G.711 (RTP/RTSP/TCP, IPv4)",
           Path = PATH_U,
           Id = "6-1-3",
           LastChangedIn = "v15.06",
           Category = Category.RTSS,
           Version = 2.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG711, Feature.RTPRTSPTCP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel711RtpUnicastRTSP()
      {
        TestByCodecAndTransport("G711", "test.711", StreamType.RTPUnicast, TransportProtocol.RTSP);
      }

      // 726
      [Test(Name = "BACKCHANNEL – G.726 (RTP-Multicast/UDP, IPv4)",
          Path = PATH_M,
          Id = "6-2-2",
          LastChangedIn = "v15.06",
          Category = Category.RTSS,
          Version = 2.0,
          RequirementLevel = RequirementLevel.Optional,
          RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG726, Feature.RTPMulticastUDP },
          FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel726RtpMulticastUdp()
      {
        TestByCodecAndTransport("G726", "test.726", StreamType.RTPMulticast, TransportProtocol.UDP);
      }

      [Test(Name = "BACKCHANNEL – G.726 (RTP-Unicast/UDP, IPv4)",
               Path = PATH_U,
               Id = "6-1-4",
               LastChangedIn = "v15.06",
               Category = Category.RTSS,
               Version = 2.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG726 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel726RtpUnicastUdp()
      {
        TestByCodecAndTransport("G726", "test.726", StreamType.RTPUnicast, TransportProtocol.UDP);
      }

      [Test(Name = "BACKCHANNEL – G.726 (RTP-Unicast/RTSP/HTTP/TCP, IPv4)",
               Path = PATH_U,
               Id = "6-1-5",
               LastChangedIn = "v15.06",
               Category = Category.RTSS,
               Version = 2.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG726 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel726RtpUnicastHTTP()
      {
        TestByCodecAndTransport("G726", "test.726", StreamType.RTPUnicast, TransportProtocol.HTTP);
      }

      [Test(Name = "BACKCHANNEL – G.726 (RTP/RTSP/TCP, IPv4)",
           Path = PATH_U,
           Id = "6-1-6",
           LastChangedIn = "v15.06",
           Category = Category.RTSS,
           Version = 2.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.AudioOutput, Feature.AudioOutputG726, Feature.RTPRTSPTCP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
      public void NvtBackchannel726RtpUnicastRTSP()
      {
        TestByCodecAndTransport("G726", "test.726", StreamType.RTPUnicast, TransportProtocol.RTSP);
      }
    }
}
