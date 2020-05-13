///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
//
using System.Collections.Generic;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.Linq;
using System;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class MediaVideoConfigurationTestSuite : Base.MediaTest
    {
        public MediaVideoConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Media Configuration\\Video Configuration\\General";

        [Test(Name = "VIDEO SOURCE CONFIGURATION",
            Path = PATH,
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetProfiles,
                                             Functionality.GetVideoSources,
                                             Functionality.GetCompatibleVideoSourceConfigurations,
                                             Functionality.GetVideoSourceConfigurations,
                                             Functionality.GetVideoSourceConfigurationOptions,
                                             Functionality.SetVideoSourceConfiguration
                                         })]
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
            Path = PATH,
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetCompatibleVideoEncoderConfigurations, 
                                             Functionality.GetVideoEncoderConfiguration
                                         })]
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
            Path = PATH,
            Order = "02.01.09",
            Id = "2-1-9",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetVideoEncoderConfigurations, 
                                             Functionality.SetVideoEncoderConfiguration
                                         })]
        public void JpegVideoEncoderConfiguration()
        {
            VideoEncoderConfiguration configBackup = null;
            RunTest(() =>
            {
                VideoEncoderConfiguration[] configs = GetVideoEncoderConfigurations();
                string reason;
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);

                VideoEncoderConfigurationOptions options = null;
                VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configs, VideoEncoding.JPEG, out options);
                configBackup = Utils.CopyMaker.CreateCopy(config);

                // invalid
                config.Encoding = VideoEncoding.JPEG;
                config.Resolution = options.JPEG.ResolutionsAvailable[0];
                config.Resolution.Height++;//invalid param
                config.Quality = options.QualityRange.Max;
                config.MPEG4 = null;
                config.H264 = null;
                if (config.RateControl != null)
                {
                    config.RateControl.FrameRateLimit = options.JPEG.FrameRateRange.Min;
                    config.RateControl.EncodingInterval = options.JPEG.EncodingIntervalRange.Min;
                }

                string details = string.Format("Setting invalid configuration (VideoEncoderConfiguration/Resolution/Height = '{0}')", config.Resolution.Height);
                SetInvalidVideoEncoderConfiguration(config, false, details);
                config.Resolution.Height--;

                VideoResolution highest = null;
                VideoResolution lowest = null;
                VideoResolution median = null;

                FindResolutions(options.JPEG.ResolutionsAvailable, out highest, out lowest, out median);

                /*
                Encoding = “JPEG” - OK
                Resolution = Highest resolution based on number of pixels , 
                Quality = QualityRange.Max, 
                FramerateLimit = FrameRateRange.Max, 
                BitrateLimit = “64000”, 
                EncodingInterval = EncodingIntervalRange.Min                 
                */

                config.Resolution = highest;
                config.Quality = options.QualityRange.Max;
                config.RateControl = new VideoRateControl();
                config.RateControl.FrameRateLimit = options.JPEG.FrameRateRange.Max;
                config.RateControl.BitrateLimit = 64000;
                config.RateControl.EncodingInterval = options.JPEG.EncodingIntervalRange.Min;

                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use max values)");

                VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);
                // 14.	DUT sends modified JPEG Video Encoder Configuration in the GetVideoEncoderConfigurationResponse message (Encoding = “JPEG”, Resolution = Highest resolution based on number of pixels ).*/
                bool ok = ConfigurationValid(newConfig, VideoEncoding.JPEG, highest, out reason);
                Assert(ok, reason, "Check that the DUT accepted values passed");

                /*
                Encoding = “JPEG”, 
                Resolution = Lowest resolution based on number of pixels, 
                Quality = QualityRange.Min, 
                FramerateLimit = FrameRateRange.Min, 
                BitrateLimit = “64000”, 
                EncodingInterval = EncodingIntervalRange.Max, 
                force persistence = false)                 
                */
                config.Resolution = lowest;
                config.Quality = options.QualityRange.Min;
                config.RateControl = new VideoRateControl();
                config.RateControl.FrameRateLimit = options.JPEG.FrameRateRange.Min;
                config.RateControl.BitrateLimit = 64000;
                config.RateControl.EncodingInterval = options.JPEG.EncodingIntervalRange.Max;

                // set
                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use min values)");
                // get modified
                newConfig = GetVideoEncoderConfiguration(config.token);
                // check
                ok = ConfigurationValid(newConfig, VideoEncoding.JPEG, lowest, out reason);
                Assert(ok, reason, "Check that the DUT accepted values passed");

                /*
                Encoding = “JPEG”, 
                Resolution = Median resolution based on number of pixels, 
                Quality = Median value of QualityRange, 
                FramerateLimit = Median value of FrameRateRange, 
                BitrateLimit = “64000”, 
                EncodingInterval = Median value of EncodingIntervalRange, 
                and force persistence = false 
                 */

                config.Resolution = median;
                config.Quality = options.QualityRange.Average();
                config.RateControl = new VideoRateControl();
                config.RateControl.FrameRateLimit = options.JPEG.FrameRateRange.Average();
                config.RateControl.BitrateLimit = 64000;
                config.RateControl.EncodingInterval = options.JPEG.EncodingIntervalRange.Average();

                // set
                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use average values)");
                // get modified
                newConfig = GetVideoEncoderConfiguration(config.token);
                // check
                ok = ConfigurationValid(newConfig, VideoEncoding.JPEG, median, out reason);
                Assert(ok, reason, "Check that the DUT accepted values passed");

            },
            () => 
            {
                if (configBackup != null)
                {
                    SetVideoEncoderConfiguration(configBackup, true);
                }
            }
            );
        }
        
        [Test(Name = "MPEG4 VIDEO ENCODER CONFIGURATION",
            Path = PATH,
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4 },
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetVideoEncoderConfigurations, 
                                             Functionality.SetVideoEncoderConfiguration
                                         })]
        public void MpegVideoEncoderConfiguration()
        {
            VideoEncoderConfiguration configBackup = null;

            RunTest(() =>
            {
                VideoEncoderConfiguration[] configs = GetVideoEncoderConfigurations();
                string reason;
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);

                VideoEncoderConfigurationOptions options = null;
                VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configs, VideoEncoding.MPEG4, out options);
                configBackup = Utils.CopyMaker.CreateCopy(config);

                config.Encoding = VideoEncoding.MPEG4;
                config.Resolution = options.MPEG4.ResolutionsAvailable[0];
                config.Quality = options.QualityRange.Max;
                config.MPEG4 = new Mpeg4Configuration();
                config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Max + 1;//invalid param
                config.MPEG4.Mpeg4Profile = Mpeg4Profile.SP;
                config.H264 = null;
                if(config.RateControl != null)
                {
                    config.RateControl.FrameRateLimit = options.MPEG4.FrameRateRange.Min;
                    config.RateControl.EncodingInterval = options.MPEG4.EncodingIntervalRange.Min;
                }
                string details = string.Format("Setting invalid configuration (/VideoEncoderConfiguration/MPEG4/GovLength = {0})", config.MPEG4.GovLength);
                SetInvalidVideoEncoderConfiguration(config, false, details);
                

                VideoResolution highest = null;
                VideoResolution lowest = null;
                VideoResolution median = null;

                VideoResolution[] resolutions = options.MPEG4.ResolutionsAvailable;
                FindResolutions(resolutions, out highest, out lowest, out median);

                /*
                Encoding = “MPEG4”, 
                Mpeg4Profile = “SP”, if “SP” is not supported “ASP”, 
                Resolution = Highest resolution based on number of pixels , 
                Quality = QualityRange.Max, 
                FramerateLimit = FrameRateRange.Max, 
                BitrateLimit = “64000”, 
                EncodingInterval = EncodingIntervalRange.Min, 
                GovLength = GovLengthRange.Min, and force persistence = false)                 
                */

                // schema requires at least one element in Mpeg4ProfilesSupported
                // as well as for other elements
                config.MPEG4.Mpeg4Profile = options.MPEG4.Mpeg4ProfilesSupported.Contains(Mpeg4Profile.SP)
                                                ? Mpeg4Profile.SP
                                                : Mpeg4Profile.ASP;
                config.Resolution = highest;
                config.Quality = options.QualityRange.Max;
                config.RateControl = new VideoRateControl();
                config.RateControl.FrameRateLimit = options.MPEG4.FrameRateRange.Max;
                config.RateControl.BitrateLimit = 64000;
                config.RateControl.EncodingInterval = options.MPEG4.EncodingIntervalRange.Min;
                config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Min;

                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use max values)");
                VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                bool ok = ConfigurationValid(newConfig, VideoEncoding.MPEG4, highest, out reason);
                
                Action checkProfile = 
                    new Action(
                        ()=>
                            {
                                string error = string.Empty;
                                if (newConfig.MPEG4 == null)
                                {
                                    ok = false;
                                    error = "MPEG4 configuration not found";
                                }
                                else
                                {
                                    if (newConfig.MPEG4.Mpeg4Profile != config.MPEG4.Mpeg4Profile)
                                    {
                                        ok = false;
                                        error = string.Format("Mpeg4Profile is incorrect. Expected: {0}, actual: {1} ",
                                                                     config.MPEG4.Mpeg4Profile, newConfig.MPEG4.Mpeg4Profile);

                                    }                                      
                                }
                              
                                if (!string.IsNullOrEmpty(error))
                                {
                                    if (string.IsNullOrEmpty(reason))
                                    {
                                        reason = error;
                                    }
                                    else
                                    {
                                        reason += System.Environment.NewLine;
                                        reason += error;
                                    }
                                }
                            });

                checkProfile();                

                Assert(ok, reason, "Check that the DUT accepted values passed");

                config.Resolution = lowest;
                config.Quality = options.QualityRange.Min;
                config.RateControl = new VideoRateControl();
                config.RateControl.FrameRateLimit = options.MPEG4.FrameRateRange.Min;
                config.RateControl.BitrateLimit = 64000;
                config.RateControl.EncodingInterval = options.MPEG4.EncodingIntervalRange.Max;
                config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Max;

                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use min values)");
                newConfig = GetVideoEncoderConfiguration(config.token);

                ok = ConfigurationValid(newConfig, VideoEncoding.MPEG4, lowest, out reason);
                checkProfile();   
                Assert(ok, reason, "Check that the DUT accepted values passed");

                // Average

                config.Resolution = median;
                config.Quality = options.QualityRange.Average();
                config.RateControl = new VideoRateControl();
                config.RateControl.FrameRateLimit = options.MPEG4.FrameRateRange.Average();
                config.RateControl.BitrateLimit = 64000;
                config.RateControl.EncodingInterval = options.MPEG4.EncodingIntervalRange.Average();
                config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Average();

                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use average values)");
                newConfig = GetVideoEncoderConfiguration(config.token);

                ok = ConfigurationValid(newConfig, VideoEncoding.MPEG4, median, out reason);
                checkProfile();
                Assert(ok, reason, "Check that the DUT accepted values passed");

            },
            () =>
            {
                if (configBackup != null)
                {
                    SetVideoEncoderConfiguration(configBackup, true);
                }
            });
        }
        
        [Test(Name = "H.264 VIDEO ENCODER CONFIGURATION",
            Path = PATH,
            Order = "02.01.11",
            Id = "2-1-11",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264 },
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetVideoEncoderConfigurations, 
                                             Functionality.SetVideoEncoderConfiguration
                                         })]
        public void H264VideoEncoderConfiguration()
        {
            VideoEncoderConfiguration configBackup = null;

            RunTest(() =>
            {
                VideoEncoderConfiguration[] configs = GetVideoEncoderConfigurations();
                string reason;
                Assert(ValidateVideoEncoderConfigs(configs, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);

                VideoEncoderConfigurationOptions options = null;
                VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configs, VideoEncoding.H264, out options);
                configBackup = Utils.CopyMaker.CreateCopy(config);

                config.Encoding = VideoEncoding.H264;
                config.Resolution = options.H264.ResolutionsAvailable[0];
                config.Quality = options.QualityRange.Max;
                config.H264 = new H264Configuration();
                config.H264.GovLength = options.H264.GovLengthRange.Max + 1;//invalid param
                config.H264.H264Profile = H264Profile.Baseline;
                config.MPEG4 = null;
                if (config.RateControl != null)
                {
                    config.RateControl.FrameRateLimit = options.H264.FrameRateRange.Min;
                    config.RateControl.EncodingInterval = options.H264.EncodingIntervalRange.Min;
                }
                string details = string.Format("Setting invalid configuration (/VideoEncoderConfiguration/H264/GovLength = {0})", config.H264.GovLength);
                SetInvalidVideoEncoderConfiguration(config, false, details);

                VideoResolution highest = null;
                VideoResolution lowest = null;
                VideoResolution median = null;

                FindResolutions(options.H264.ResolutionsAvailable, out highest, out lowest, out median);

                Func<List<H264Profile>, H264Profile> firstSupported =
                    new Func<List<H264Profile>, H264Profile>(
                        (list) =>
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    if (options.H264.H264ProfilesSupported.Contains(list[i]))
                                    {
                                        return list[i];
                                    }
                                }
                                return options.H264.H264ProfilesSupported[0];
                            });

                List<H264Profile> profiles = new List<H264Profile>();
                profiles.Add(H264Profile.Baseline);
                profiles.Add(H264Profile.Main);
                profiles.Add(H264Profile.Extended);
                profiles.Add(H264Profile.High);

                H264Profile profile = firstSupported(profiles);

              
                config.H264.H264Profile = profile;
                config.RateControl = new VideoRateControl();
                config.RateControl.BitrateLimit = 64000;

                // max

                config.Resolution = highest;
                config.Quality = options.QualityRange.Max;
                config.RateControl.FrameRateLimit = options.H264.FrameRateRange.Max;
                config.RateControl.EncodingInterval = options.H264.EncodingIntervalRange.Min;
                config.H264.GovLength = options.H264.GovLengthRange.Min;
                
                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use max values)");
                VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                bool ok = ConfigurationValid(newConfig, VideoEncoding.H264, highest, out reason);

                Action checkProfile =
                    new Action(
                        () =>
                        {
                            if (newConfig.H264.H264Profile != config.H264.H264Profile)
                            {
                                ok = false;
                                string error = string.Format("H264Profile is incorrect. Expected: {0}, actual: {1} ",
                                                             config.H264.H264Profile, newConfig.H264.H264Profile);
                                if (string.IsNullOrEmpty(reason))
                                {
                                    reason = error;
                                }
                                else
                                {
                                    reason += System.Environment.NewLine;
                                    reason += error;
                                }
                            }
                        });

                checkProfile();
                Assert(ok, reason, "Check that the DUT accepted values passed");

                // Min

                profiles.Clear();
                profiles.Add(H264Profile.Main);
                profiles.Add(H264Profile.Extended);
                profiles.Add(H264Profile.High);
                profiles.Add(H264Profile.Baseline);

                profile = firstSupported(profiles);


                config.Resolution = lowest;
                config.Quality = options.QualityRange.Min;
                config.RateControl.FrameRateLimit = options.H264.FrameRateRange.Min;
                config.RateControl.EncodingInterval = options.H264.EncodingIntervalRange.Max;
                config.H264.GovLength = options.H264.GovLengthRange.Max;
                config.H264.H264Profile = profile;

                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use min values)");
                newConfig = GetVideoEncoderConfiguration(config.token);

                ok = ConfigurationValid(newConfig, VideoEncoding.H264, lowest, out reason);
                checkProfile();
                Assert(ok, reason, "Check that the DUT accepted values passed");
                
                // Average 
                
 
                profiles.Clear();
                profiles.Add(H264Profile.Extended);
                profiles.Add(H264Profile.High);
                profiles.Add(H264Profile.Baseline);
                profiles.Add(H264Profile.Main);

                profile = firstSupported(profiles);

                config.Resolution = median;
                config.Quality = options.QualityRange.Average();
                config.RateControl.FrameRateLimit = options.H264.FrameRateRange.Average();
                config.RateControl.EncodingInterval = options.H264.EncodingIntervalRange.Average();
                config.H264.GovLength = options.H264.GovLengthRange.Average();
                config.H264.H264Profile = profile;

                SetVideoEncoderConfiguration(config, false, false, "SetVideoEncoderConfiguration (use average values)");
                newConfig = GetVideoEncoderConfiguration(config.token);

                ok = ConfigurationValid(newConfig, VideoEncoding.H264, median, out reason);
                checkProfile();
                Assert(ok, reason, "Check that the DUT accepted values passed");
                
            },
            () =>
            {
                if (configBackup != null)
                {
                    SetVideoEncoderConfiguration(configBackup, true);
                }
            });
        }
        
        [Test(Name = "GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES",
            Path = PATH,
            Order = "02.01.06",
            Id = "2-1-6",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetVideoSourceConfigurations, 
                                             Functionality.GetGuaranteedNumberOfVideoEncoderInstances
                                         })]
        public void GuaranteedNumberOfEncoders()
        {
            RunTest(() =>
            {
                VideoSourceConfiguration[] configs = GetVideoSourceConfigurations();
                string reason;
                Assert(ValidateVideoSourceConfigs(configs, out reason), reason, Resources.StepValidatingVideoSourceConfigs_Title);

                string configToken = configs[0].token;
                int? jpeg;
                int? mpeg;
                int? h264;
                int totalNumber = GetGuaranteedNumberOfVideoEncoderInstances(configToken, out jpeg, out h264, out mpeg);
                Assert(totalNumber >= 0, "The DUT did not return total number of encoder instances or value is invalid", "Validating guaranteed number of video encoder instances");
            });
        }


        // Heineken project - December 2012
        [Test(Name = "GET GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES AND GET VIDEO ENCODER CONFIGURATION OPTIONS CONSISTENCY",
            Path = PATH,
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[]
                                                 {
                                                     Functionality.GetVideoEncoderConfigurationOptions, 
                                                     Functionality.GetGuaranteedNumberOfVideoEncoderInstances
                                                 })]
        public void GuaranteedNumberOfEncodersAndEncodingConfigurationOptionsConsistency()
        {
            RunTest(() =>
            {                
                int? maxProfilesFromDevice = null;
                int? maxProfilesFromMedia = null;
                
                if (Features.Contains(Feature.GetCapabilities))
                {
                    //3.	If GetCapabilities supported by the DUT, ONVIF Client will invoke GetCapabilitiesRequest message (Category = ‘Media’) to get MaximumNumberOfProfiles capability. Otherwise skip steps 3-4 and go to the step 5.
                    //4.	Verify the GetCapabilitiesResponse message from the DUT.
                    
                    DeviceClient device = GetDeviceClient();

                    BeginStep("Get Media service capabilities from Device service");
                    Capabilities capabilities = device.GetCapabilities(new CapabilityCategory[] { CapabilityCategory.Media });
                    StepPassed();

                    Assert(capabilities != null && capabilities.Media != null, "Media capabilities were not received", "Check that the DUT returned Media capabilities");

                    if (capabilities.Media.Extension != null && capabilities.Media.Extension.ProfileCapabilities != null)
                    {
                        maxProfilesFromDevice = capabilities.Media.Extension.ProfileCapabilities.MaximumNumberOfProfiles;
                    }
                }

                if (Features.Contains(Feature.GetServices))
                {
                    //5.	If GetServices supported by the DUT, ONVIF Client will invoke GetServiceCapabilitiesRequest message to get MaximumNumberOfProfiles capability. Otherwise skip steps 5-6 and go to the step 7.
                    //6.	Verify the GetServiceCapabilitiesResponse message from the DUT.
                    
                    MediaServiceCapabilities capabilities = GetServiceCapabilities();

                    Assert(capabilities != null, "Media service capabilities were not received", "Check that the DUT returned Media service capabilities");

                    if (capabilities.ProfileCapabilities != null)
                    {
                        if (capabilities.ProfileCapabilities.MaximumNumberOfProfilesSpecified)
                        {
                            maxProfilesFromMedia = capabilities.ProfileCapabilities.MaximumNumberOfProfiles;
                        }
                    }
                }
                
                string reason;
                //7.	ONVIF Client will invoke GetVideoEncoderConfigurationsRequest message to retrieve all DUT video encoder configurations.
                //8.	Verify the GetVideoSourceConfigurationsResponse message from the DUT.
                VideoEncoderConfiguration[] encoderConfigurations = GetVideoEncoderConfigurations();
                Assert(ValidateVideoEncoderConfigs(encoderConfigurations, out reason), reason, Resources.StepValidatingVideoEncoderConfigs_Title);

                //9.	ONVIF Client will invoke GetVideoSourceConfigurationsRequest message to retrieve all DUT video source configurations.
                //10.	Verify the GetVideoSourceConfigurationsResponse message from the DUT.
                VideoSourceConfiguration[] configs = GetVideoSourceConfigurations();
                
                Assert(ValidateVideoSourceConfigs(configs, out reason), reason, Resources.StepValidatingVideoSourceConfigs_Title);
                
                bool jpegPresent = false;
                bool mpegPresent = false;
                bool h264Present = false;

                bool jpegNotSupported = true;
                bool mpegNotSupported = true;
                bool h264NotSupported = true;
                
                foreach (VideoSourceConfiguration config in configs)
                { 
                    string configToken = config.token;
                    int? jpeg;
                    int? mpeg;
                    int? h264;

                    //11.	ONVIF Client will invoke GetGuaranteedNumberOfVideoEncoderInstancesRequest message 
                    // (ConfigurationToken = “Token1”, where “Token1” is a first video source configuration 
                    // token from GetVideoSourceConfigurationsResponse message) to retrieve guaranteed number 
                    // of video encoder instances per first video source configuration.
                    //12.	Verify the GetGuaranteedNumberOfVideoEncoderInstancesResponse message from the DUT.
                    int totalNumber = GetGuaranteedNumberOfVideoEncoderInstances(configToken, out jpeg, out h264, out mpeg);
                                        
                    //11.	Verify that GetGuaranteedNumberOfVideoEncoderInstancesResponse.TotalNumber less 
                    // or equal to total number of VideoEncoderConfigurations. 

                    Assert(totalNumber <= encoderConfigurations.Length,
                        "Guaranteed total number of video encoder instances is greater than total number of video encoder configurations",
                        "Compare guaranteed total number of video encoder instances and total number of video encoder configurations");

                    //13.	Verify that GetGuaranteedNumberOfVideoEncoderInstancesResponse.TotalNumber less 
                    // or equal to total number of VideoEncoderConfigurations. 
                    //14.	If GetCapabilities supported by the DUT, Verify that 
                    // GetGuaranteedNumberOfVideoEncoderInstancesResponse.TotalNumber less or equal to total 
                    // number of GetCapabilitiesResponse.Media.Extension.ProfileCapabilities.MaximumNumberOfProfiles 
                    // if specified.
                    //15.	If GetServices supported by the DUT, Verify that GetGuaranteedNumberOfVideoEncoderInstancesResponse.TotalNumber 
                    // less or equal to total number of GetServiceCapabilitiesResponse.Capabilities.ProfileCapabilities.MaximumNumberOfProfiles 
                    // if specified.

                    if (maxProfilesFromDevice.HasValue)
                    {
                        Assert(totalNumber <= maxProfilesFromDevice,             
                            "Guaranteed total number of video encoder instances is greater than max number of profiles as defined in Device capabilities",
                            "Compare guaranteed total number of video encoder instances and maximum number of profiles");
                    }

                    if (maxProfilesFromMedia.HasValue)
                    {
                        Assert(totalNumber <= maxProfilesFromMedia,
                            "Guaranteed total number of video encoder instances is greater than max number of profiles as defined in Media service capabilities",
                            "Compare guaranteed total number of video encoder instances and maximum number of profiles");
                    }

                    // positive value: supported;
                    if (jpeg.HasValue)
                    {
                        if (jpeg.Value > 0)
                        {
                            jpegPresent = true;
                            jpegNotSupported = false;
                        }
                    }
                    else
                    {
                        jpegNotSupported = false;
                    }
                    if (mpeg.HasValue)
                    {
                        if (mpeg.Value > 0)
                        {
                            mpegPresent = true;
                            mpegNotSupported = false;
                        }
                    }
                    else
                    {
                        mpegNotSupported = false;
                    }
                    if (h264.HasValue)
                    {
                        if (h264.Value > 0)
                        {
                            h264Present = true;
                            h264NotSupported = false;
                        }
                    }
                    else
                    {
                        h264NotSupported = false;
                    }
                }
                //16.	Repeat steps 11-15 to retrieve guaranteed number of video encoder instances for all video source configuration

                //17.	ONVIF Client will invoke GetVideoEncoderConfigurationOptionsRequest message (no ConfigurationToken, no ProfileToken) to retrieve general video encoder options for the DUT.
                //18.	Verify the GetVideoEncoderConfigurationOptionsResponse message from the DUT.
                VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(null, null);

                //19.	Verify that GetVideoEncoderConfigurationOptionsResponse contains Options.JPEG, 
                // if there is at least one GetGuaranteedNumberOfVideoEncoderInstancesResponse 
                // with JPEG element having value greater than 0.
                //20.	Verify that GetVideoEncoderConfigurationOptionsResponse contains Options.MPEG4, 
                // if there is at least one GetGuaranteedNumberOfVideoEncoderInstancesResponse with 
                // MPEG4 element having value greater than 0.
                //21.	Verify that GetVideoEncoderConfigurationOptionsResponse contains Options.H264, 
                // if there is at least one GetGuaranteedNumberOfVideoEncoderInstancesResponse with H264 
                // element having value greater than 0.
                //22.	Verify that GetVideoEncoderConfigurationOptionsResponse does not contain Options.JPEG, 
                // if there are no GetGuaranteedNumberOfVideoEncoderInstancesResponses with skipped JPEG element 
                // or with JPEG element having value greater than 0.
                //23.	Verify that GetVideoEncoderConfigurationOptionsResponse does not contain Options.MPEG4, 
                // if there are no GetGuaranteedNumberOfVideoEncoderInstancesResponses with skipped MPEG4 
                // element or with MPEG4 element having value greater than 0.
                //24.	Verify that GetVideoEncoderConfigurationOptionsResponse does not contain Options.H264, 
                // if there are no GetGuaranteedNumberOfVideoEncoderInstancesResponses with skipped H264 
                // element or with H264 element having value greater than 0.
                
                Action<bool, bool, object, string> encoderCheck =
                    new Action<bool, bool, object, string>(
                        (found, notSupported, settings, name) =>
                        {
                            if (found)
                            {
                                Assert(settings != null,
                                    string.Format("{0} options not found, while GetGuaranteedNumberOfVideoEncoderInstances returns positive value for at least one configuration", name),
                                    string.Format("Check that {0} options are present", name));
                            }
                            if (notSupported) 
                            {                                                                 
                                Assert(settings == null,
                                    string.Format("{0} options are not empty, while GetGuaranteedNumberOfVideoEncoderInstances returns 0 for all configurations", name),
                                    string.Format("Check that {0} options are not present", name));

                            }
                        });

                encoderCheck(jpegPresent, jpegNotSupported, options.JPEG, "JPEG");
                encoderCheck(mpegPresent, mpegNotSupported, options.MPEG4, "MPEG4");
                encoderCheck(h264Present, h264NotSupported, options.H264, "H264");
            
            });
        }
    
        void FindResolutions(IEnumerable<VideoResolution> videoResolutions, 
            out VideoResolution highest, out VideoResolution lowest, out VideoResolution median)
        {
            BeginStep("Find highest and lowest resolutions for further testing");

            List<VideoResolution> resolutions = videoResolutions.OrderBy(VR => VR.Height * VR.Width).ToList();
            lowest = resolutions.First();
            highest = resolutions.Last();

            int cnt = resolutions.Count;
            int middle = (cnt+1)/2 - 1;
            median = resolutions[middle];
            
            LogStepEvent(string.Format("Highest resolution: width ={0}, height = {1} ", highest.Width, highest.Height));
            LogStepEvent(string.Format("Median resolution: width ={0}, height = {1} ", median.Width, median.Height));
            LogStepEvent(string.Format("Lowest resolution: width ={0}, height = {1} ", lowest.Width, lowest.Height));

            StepPassed();

        }

    }
}
