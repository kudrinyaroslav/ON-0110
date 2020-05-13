///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.TestCases;
using TestTool.Proxies.Media;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class MediaVideoConfigurationTestSuite : Base.MediaTest
    {
        public MediaVideoConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }
        [Test(Name = "VIDEO SOURCE CONFIGURATION",
            Path = "Media Configuration\\Video Configuration",
            Order = "07.02.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
        public void VideoSourceConfiguration()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();
                string reason;
                Profile profile;

                Assert(
                    ValidateProfiles(profiles, out reason, out profile),
                    reason,
                    Resources.StepValidatingProfiles_Title,
                    profile != null ? string.Format(Resources.StepValidatingProfilesDetails_Format, profile.token) : null);

                VideoSource[] sources = GetVideoSources();
                Assert(ValidateVideoSources(sources, out reason), reason, Resources.StepValidatingVideoSources_Title);

                VideoSourceConfiguration[] configs = GetCompatibleVideoSourceConfigurations(profile.token);
                Assert(ValidateVideoSourceConfigs(configs, out reason), reason, Resources.StepValidatingVideoSourceConfigs_Title);
                VideoSourceConfiguration config = configs[0];

                configs = GetVideoSourceConfigurations();
                Assert(ValidateVideoSourceConfigs(configs, out reason), reason, Resources.StepValidatingVideoSourceConfigs_Title);

                VideoSourceConfigurationOptions options = GetVideoSourceConfigurationOptions(profile.token, config.token);

                config.Bounds.width = options.BoundsRange.WidthRange.Max;
                config.Bounds.height = options.BoundsRange.HeightRange.Max + 1;
                string details = string.Format("Setting invalid configuration (/VideoSourceConfiguration/Bounds/height = '{0}')", config.Bounds.height);
                SetInvalidVideoSourceConfiguration(config, false, details);

                config.Bounds.height = options.BoundsRange.HeightRange.Max;
                SetVideoSourceConfiguration(config, false);

                VideoSourceConfiguration newConfig = GetVideoSourceConfiguration(config.token);
                Assert(EqualConfigurations(config, newConfig, out reason), 
                    string.Format(Resources.ErrorVideoConfigNotEqual_Format, reason),
                    Resources.StepCompareVideoSourceConfigs_Title);
            });
        }
        
        [Test(Name = "VIDEO ENCODER CONFIGURATION",
            Path = "Media Configuration\\Video Configuration",
            Order = "07.02.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
        public void VideoEncoderConfiguration()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();
                string reason;
                Profile profile;
                Assert(ValidateProfiles(profiles, out reason, out profile), reason, Resources.StepValidatingProfiles_Title);

                VideoEncoderConfiguration[] configs = GetCompatibleVideoEncoderConfigurations(profile.token);
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);
                VideoEncoderConfiguration config = configs[0];

                configs = GetVideoEncoderConfigurations();
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);
            });
        }

        [Test(Name = "JPEG VIDEO ENCODER CONFIGURATION",
            Path = "Media Configuration\\Video Configuration",
            Order = "07.02.03",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
        public void JpegVideoEncoderConfiguration()
        {
            RunTest(() =>
            {
                VideoEncoderConfiguration[] configs = GetVideoEncoderConfigurations();
                string reason;
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);

                VideoEncoderConfigurationOptions options = null;
                VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configs, VideoEncoding.JPEG, out options);

                config.Encoding = VideoEncoding.JPEG;
                config.Resolution = options.JPEG.ResolutionsAvailable[0];
                config.Resolution.Height++;//invalid param
                config.Quality = options.QualityRange.Max;
                config.MPEG4 = null;
                config.H264 = null;
                //config.SessionTimeout = "PT600S";//send the same as received
                if (config.RateControl != null)
                {
                    config.RateControl.FrameRateLimit = options.JPEG.FrameRateRange.Min;
                    config.RateControl.EncodingInterval = options.JPEG.EncodingIntervalRange.Min;
                }

                string details = string.Format("Setting invalid configuration (VideoEncoderConfiguration/Resolution/Height = '{0}')", config.Resolution.Height);
                SetInvalidVideoEncoderConfiguration(config, false, details);

                config.Resolution.Height--;
                SetVideoEncoderConfiguration(config, false);

                VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);
                Assert(
                    EqualConfigurations(config, newConfig, out reason),
                    string.Format(Resources.ErrorVideoEncoderConfigNotEqual_Format, reason),
                    Resources.StepCompareVideoEncoderConfigs_Title);
            });
        }
        
        [Test(Name = "MPEG4 VIDEO ENCODER CONFIGURATION",
            Path = "Media Configuration\\Video Configuration",
            Order = "07.02.04",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.MPEG4 })]
        public void MpegVideoEncoderConfiguration()
        {
            RunTest(() =>
            {
                VideoEncoderConfiguration[] configs = GetVideoEncoderConfigurations();
                string reason;
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);

                VideoEncoderConfigurationOptions options = null;
                VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configs, VideoEncoding.MPEG4, out options);

                config.Encoding = VideoEncoding.MPEG4;
                config.Resolution = options.MPEG4.ResolutionsAvailable[0];
                config.Quality = options.QualityRange.Max;
                config.MPEG4 = new Mpeg4Configuration();
                config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Max + 1;//invalid param
                config.MPEG4.Mpeg4Profile = Mpeg4Profile.SP;
                config.H264 = null;
                //config.SessionTimeout = "PT600S";//send the same as received
                if(config.RateControl != null)
                {
                    config.RateControl.FrameRateLimit = options.MPEG4.FrameRateRange.Min;
                    config.RateControl.EncodingInterval = options.MPEG4.EncodingIntervalRange.Min;
                }
                string details = string.Format("Setting invalid configuration (/VideoEncoderConfiguration/MPEG4/GovLength = {0})", config.MPEG4.GovLength);
                SetInvalidVideoEncoderConfiguration(config, false, details);

                config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Max;
                SetVideoEncoderConfiguration(config, false);

                VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);
                Assert(EqualConfigurations(config, newConfig, out reason), 
                    string.Format(Resources.ErrorVideoEncoderConfigNotEqual_Format, reason), 
                    Resources.StepCompareVideoEncoderConfigs_Title);
            });
        }
        
        [Test(Name = "H.264 VIDEO ENCODER CONFIGURATION",
            Path = "Media Configuration\\Video Configuration",
            Order = "07.02.05",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.H264 })]
        public void H264VideoEncoderConfiguration()
        {
            RunTest(() =>
            {
                VideoEncoderConfiguration[] configs = GetVideoEncoderConfigurations();
                string reason;
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);

                VideoEncoderConfigurationOptions options = null;
                VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configs, VideoEncoding.H264, out options);

                config.Encoding = VideoEncoding.H264;
                config.Resolution = options.H264.ResolutionsAvailable[0];
                config.Quality = options.QualityRange.Max;
                config.H264 = new H264Configuration();
                config.H264.GovLength = options.H264.GovLengthRange.Max + 1;//invalid param
                config.H264.H264Profile = H264Profile.Baseline;
                config.MPEG4 = null;
                //config.SessionTimeout = "PT600S";////send the same as received
                if (config.RateControl != null)
                {
                    config.RateControl.FrameRateLimit = options.H264.FrameRateRange.Min;
                    config.RateControl.EncodingInterval = options.H264.EncodingIntervalRange.Min;
                }
                string details = string.Format("Setting invalid configuration (/VideoEncoderConfiguration/H264/GovLength = {0})", config.H264.GovLength);
                SetInvalidVideoEncoderConfiguration(config, false, details);

                config.H264.GovLength = options.H264.GovLengthRange.Max;
                SetVideoEncoderConfiguration(config, false);

                VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);
                Assert(EqualConfigurations(config, newConfig, out reason), 
                    string.Format(Resources.ErrorVideoEncoderConfigNotEqual_Format, reason),
                    Resources.StepCompareVideoEncoderConfigs_Title);
            });
        }
        
        [Test(Name = "GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES",
            Path = "Media Configuration\\Video Configuration",
            Order = "07.02.06",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
        public void GuaranteedNumberOfEncoders()
        {
            RunTest(() =>
            {
                VideoSourceConfiguration[] configs = GetVideoSourceConfigurations();
                string reason;
                Assert(ValidateVideoSourceConfigs(configs, out reason), reason, Resources.StepValidatingVideoSourceConfigs_Title);

                string configToken = configs[0].token;
                int jpeg;
                int mpeg;
                int h264;
                int totalNumber = GetGuaranteedNumberOfVideoEncoderInstances(configToken, out jpeg, out h264, out mpeg);
                Assert(totalNumber >= 0, "Device did not return total number of encoder instances or value is invalid", "Validating guaranteed number of video encoder instances");
            });
        }
    }
}
