using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using System;

namespace TestUpdateLib
{
    /*[TestClass]*/
    public class RTSSTest : BaseTest
    {
        public RTSSTest(TestLaunchParam param)
            : base(param)
        {
        }

        #region Common

        /// <summary>
        /// Perform common initialization/finalization. Run test action.
        /// </summary>
        /// <param name="action">Test "body"</param>
        protected void RunTest(Action action)
        {
            _halted = false;
            Exception exc = null;

            try
            {
                ResetLog();

                action();

                EndTest(TestStatus.Passed);
            }
            catch (StopEventException)
            {
                LogStepEvent("Halted");
                _halted = true;
            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
                return;

            }
        }

        /// <summary>
        /// Ends the test.
        /// </summary>
        /// <param name="status"></param>
        protected void EndTest(TestStatus status)
        {
            SetTestStatus(status);
            TestCompleted();
        }

        void DoNothing()
        {
            RunTest(
                () =>
                    {
                        LogTestEvent("THIS VERSION DOES NOTHING");
                    }
                );
        }

        #endregion



        private const string RTSS12 = "Real Time Streaming\\Video Streaming\\Multicast";
        private const string RTSS21 = "Real Time Streaming\\Audio Streaming\\Unicast";



        /*
        RTSS-1-2-1	MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv4)
        RTSS-1-2-2	MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv4)
        RTSS-1-2-3	MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv4)
        RTSS-1-2-4	MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv6)
        RTSS-1-2-5	MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv6)
        RTSS-1-2-6	MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv6)
        */

        [Test(Name = "MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv4)",
            Path = RTSS12,
            Order = "01.02.01",
            Id = "1-2-1",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPMulticastUDP },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegRtpMulticastUdpIPv4()
        {
            DoNothing();
        }
        
        [Test(Name = "MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv4)",
               Path = RTSS12,
               Order = "01.02.02",
               Id = "1-2-2",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPMulticastUDP, Feature.MPEG4 },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingMpeg4RtpMulticastUdpIPv4()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv4)",
               Path = RTSS12,
               Order = "01.02.03",
               Id = "1-2-3",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPMulticastUDP, Feature.H264 },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingH264RtpMulticastUdpIPv4()
        {
            DoNothing();

        }
        
        [Test(Name = "MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv6)",
               Path = RTSS12,
               Order = "01.02.04",
               Id = "1-2-4",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.IPv6 })]
        public void NvtMediaStreamingJpegRtpMulticastUdpIPv6()
        {
            DoNothing();
        }
        
        [Test(Name = "MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv6)",
               Path = RTSS12,
               Order = "01.02.05",
               Id = "1-2-5",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4, Feature.IPv6 })]
        public void NvtMediaStreamingMpeg4RtpMulticastUdpIPv6()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv6)",
               Path = RTSS12,
               Order = "01.02.06",
               Id = "1-2-6",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264, Feature.IPv6 })]
        public void NvtMediaStreamingH264RtpMulticastUdpIPv6()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – G.711 (RTP-Unicast/UDP)",
               Path = RTSS21,
               Order = "02.01.01",
               Id = "2-1-1",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpUnicastUdp()
        {
            DoNothing();
        }

        /*
RTSS-2-1-1	MEDIA STREAMING – G.711 (RTP-Unicast/UDP)
RTSS-2-1-2	MEDIA STREAMING – G.711 (RTP-Unicast/RTSP/HTTP/TCP)
RTSS-2-1-3	MEDIA STREAMING – G.711 (RTP/RTSP/TCP)
RTSS-2-1-4	MEDIA STREAMING – G.726 (RTP-Unicast/UDP)
RTSS-2-1-5	MEDIA STREAMING – G.726 (RTP-Unicast/RTSP/HTTP/TCP)
RTSS-2-1-6	MEDIA STREAMING – G.726 (RTP/RTSP/TCP)
RTSS-2-1-7	MEDIA STREAMING – AAC (RTP-Unicast/UDP)
RTSS-2-1-8	MEDIA STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP)
RTSS-2-1-9	MEDIA STREAMING – AAC (RTP/RTSP/TCP)
*/

        [Test(Name = "MEDIA STREAMING – G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
               Path = RTSS21,
               Order = "02.01.02",
               Id = "2-1-2",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpUnicastRtspHttpTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – G.711 (RTP/RTSP/TCP)",
               Path = RTSS21,
               Order = "02.01.03",
               Id = "2-1-3",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpRtspTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Unicast/UDP)",
               Path = RTSS21,
               Order = "02.01.07",
               Id = "2-1-7",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastUdp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP)",
               Path = RTSS21,
               Order = "02.01.08",
               Id = "2-1-8",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastRtspHttpTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP/RTSP/TCP)",
               Path = RTSS21,
               Order = "02.01.09",
               Id = "2-1-9",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP, Feature.AAC },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpRtspTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – G.726 (RTP-Unicast/UDP)",
               Path = RTSS21,
               Order = "02.01.04",
               Id = "2-1-4",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726 },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpUnicastUdp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
               Path = RTSS21,
               Order = "02.01.05",
               Id = "2-1-5",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726 },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpUnicastRtspHttpTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – G.726 (RTP/RTSP/TCP)",
               Path = RTSS21,
               Order = "02.01.06",
               Id = "2-1-6",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP, Feature.G726 },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpRtspTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Unicast/UDP)",
       Path = RTSS21,
       Order = "02.01.07",
       Id = "2-1-7",
       Category = Category.RTSS,
       Version = 5.0,
       RequirementLevel = RequirementLevel.Must,
       RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
       FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastUdp21()
        {
            DoNothing();

        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP)",
               Path = RTSS21,
               Order = "02.01.08",
               Id = "2-1-8",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastRtspHttpTcp21()
        {
            DoNothing();

        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP/RTSP/TCP)",
               Path = RTSS21,
               Order = "02.01.09",
               Id = "2-1-9",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP, Feature.AAC },
               FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpRtspTcp21()
        {
            DoNothing();
        }

        private const string RTSS31 = "Real Time Streaming\\Audio & Video Streaming\\Unicast";
        
        /* 
        RTSS-3-1-1	MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/ UDP)
        RTSS-3-1-2	MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/RTSP/HTTP/TCP)
        RTSS-3-1-3	MEDIA STREAMING – JPEG/G.711 (RTP/RTSP/TCP)
        RTSS-3-1-4	MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/ UDP)
        RTSS-3-1-5	MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/RTSP/HTTP/TCP)
        RTSS-3-1-6	MEDIA STREAMING – JPEG/G.726 (RTP/RTSP/TCP)
        RTSS-3-1-7	MEDIA STREAMING – JPEG/AAC (RTP-Unicast/ UDP)
        RTSS-3-1-8	MEDIA STREAMING – JPEG/AAC (RTP-Unicast/RTSP/HTTP/TCP)
        RTSS-3-1-9	MEDIA STREAMING – JPEG/AAC (RTP/RTSP/TCP)
        */

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/UDP)",
            Path = RTSS31,
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpUnicastUdp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = RTSS31,
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpUnicastRtspHttpTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP/RTSP/TCP)",
            Path = RTSS31,
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpRtspTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/UDP)",
            Path = RTSS31,
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG726RTPUnicastUdp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = RTSS31,
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG726RtpUnicastRtspHttpTcp()
        {
            DoNothing();
        }


        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP/RTSP/TCP)",
            Path = RTSS31,
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG726RtpRtspTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/UDP)",
            Path = RTSS31,
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegAacRtpUnicastUdp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = RTSS31,
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegAacRtpUnicastRtspHttpTcp()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP/RTSP/TCP)",
            Path = RTSS31,
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.RTSS,
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegAacRtpRtspTcp()
        {
            DoNothing();
        }
        
        /* 
        RTSS-4-1-4	MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv4)
        RTSS-4-1-5	MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv6)
        RTSS-4-1-9	MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv4)
        RTSS-4-1-10	MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv6)
        RTSS-4-1-14	MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv4)
        RTSS-4-1-15	MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv6)
         * 
         * CHANGE : this is 3.2
         * 
         */

        private const string RTSS41 = "Real Time Streaming\\Audio & Video Streaming\\Multicast";

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv4)",
           Path = RTSS41,
           Order = "03.02.04",
           Id = "3-2-4",
           Category = Category.RTSS,
           Version = 5.0,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPMulticastUDP },
           FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpMulticastUdpIPv4()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv6)",
               Path = RTSS41,
               Order = "03.02.05",
               Id = "3-2-5",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.IPv6, Feature.RTPMulticastUDP })]
        public void NvtMediaStreamingJpegG711RtpMulticastUdpIPv6()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv4)",
               Path = RTSS41,
               Order = "03.02.09",
               Id = "3-2-9",
               Category = Category.RTSS,
               Version = 5.0,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726, Feature.RTPMulticastUDP })]
        public void NvtMediaStreamingJpegG726RtpMulticastUdpIPv4()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv6)",
               Path = RTSS41,
               Order = "03.02.10",
               Id = "3-2-10",
               Category = Category.RTSS,
               Version = 5.0,
               Interactive = true,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726, Feature.IPv6, Feature.RTPMulticastUDP })]
        public void NvtMediaStreamingJpegG726RtpMulticastUdpIPv6()
        {
            DoNothing();
        }


        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv4)",
               Path = RTSS41,
               Order = "03.02.14",
               Id = "3-2-14",
               Category = Category.RTSS,
               Version = 5.0,
               Interactive = true,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC, Feature.RTPMulticastUDP })]
        public void NvtMediaStreamingJpegAACRtpMulticastUdpIPv4()
        {
            DoNothing();
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv6)",
               Path = RTSS41,
               Order = "03.02.15",
               Id = "3-2-15",
               Category = Category.RTSS,
               Version = 5.0,
               Interactive = true,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC, Feature.IPv6, Feature.RTPMulticastUDP })]
        public void NvtMediaStreamingJpegAACRtpMulticastUdpIPv6()
        {
            DoNothing();
        }

        [Test(Name = "START AND STOP MULTICAST STREAMING – JPEG (IPv4)",
           Path = "Real Time Streaming\\Start and Stop Multicast Streaming",
           Order = "05.01.01",
           Id = "5-1-1",
           Category = Category.RTSS,
           Version = 2.0,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPMulticastUDP },
           FunctionalityUnderTest = new Functionality[] { Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void NvtStartAndStopMulticastStreamingJpegIPv4()
        {
            DoNothing();
        }

    }
}
