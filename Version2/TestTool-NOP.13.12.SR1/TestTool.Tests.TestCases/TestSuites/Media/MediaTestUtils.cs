using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    class MediaTestUtils
    {

        public static string DumpList(IEnumerable<int> values)
        {
            List<string> strValues = new List<string>();
            foreach (int value in values)
            {
                strValues.Add(value.ToString());
            }

            return string.Join(",", strValues.ToArray());
        }

        public static List<T> SelectConfigurations<T>(IList<T> configurations)
        {
            List<T> selectedConfigs = new List<T>();
            selectedConfigs.Add(configurations[0]);
            if (configurations.Count > 2)
            {
                int idx = configurations.Count / 2; // 3 -> 1, 4 -> 2, 5-> 2
                selectedConfigs.Add(configurations[idx]);
            }
            if (configurations.Count > 1)
            {
                selectedConfigs.Add(configurations[configurations.Count - 1]);
            }
            return selectedConfigs;
        }

        public static void UpdateVideoEncoderConfiguration(VideoEncoderConfiguration vec, 
            VideoEncoding encoding,
            VideoEncoderConfigurationOptions videoOptions)
        {
            vec.Encoding = encoding;

            switch (encoding)
            {             
                case VideoEncoding.JPEG:

                    VideoResolution resolution = videoOptions.JPEG.ResolutionsAvailable.OrderBy(R => R.Height * R.Width).Last();

                    vec.H264 = null;
                    vec.MPEG4 = null;
                    vec.Resolution = videoOptions.JPEG.ResolutionsAvailable.First();
                    vec.Quality = videoOptions.QualityRange.Average();

                    if (vec.RateControl == null && (videoOptions.JPEG.EncodingIntervalRange != null
                        || videoOptions.JPEG.FrameRateRange != null ||
                        (videoOptions.Extension != null && videoOptions.Extension.JPEG != null && videoOptions.Extension.JPEG.BitrateRange != null)))
                    {
                        vec.RateControl = new VideoRateControl();
                    }

                    if (videoOptions.JPEG.EncodingIntervalRange != null)
                    {
                        vec.RateControl.EncodingInterval = videoOptions.JPEG.EncodingIntervalRange.Average();
                    }
                    if (videoOptions.JPEG.FrameRateRange != null)
                    {
                        vec.RateControl.FrameRateLimit = videoOptions.JPEG.FrameRateRange.Average();
                    }
                    if (videoOptions.Extension != null && videoOptions.Extension.JPEG != null && videoOptions.Extension.JPEG.BitrateRange != null)
                    {
                        vec.RateControl.BitrateLimit = videoOptions.Extension.JPEG.BitrateRange.Average();
                    }
                    break;
                case VideoEncoding.MPEG4:
                    vec.H264 = null;
                    if (vec.MPEG4 == null)
                    {
                        vec.MPEG4 = new Mpeg4Configuration();
                    }
                    vec.Quality = videoOptions.QualityRange.Average();
                    vec.MPEG4.Mpeg4Profile = videoOptions.MPEG4.Mpeg4ProfilesSupported.Contains(Mpeg4Profile.SP)
                                ? Mpeg4Profile.SP
                                : Mpeg4Profile.ASP;

                    vec.Resolution = videoOptions.MPEG4.ResolutionsAvailable[0];

                    if (vec.RateControl == null && (videoOptions.MPEG4.EncodingIntervalRange != null
                        || videoOptions.MPEG4.FrameRateRange != null ||
                        (videoOptions.Extension != null && videoOptions.Extension.MPEG4 != null && videoOptions.Extension.MPEG4.BitrateRange != null)))
                    {
                        vec.RateControl = new VideoRateControl();
                    }

                    if (videoOptions.MPEG4.EncodingIntervalRange != null)
                    {
                        vec.RateControl.EncodingInterval = videoOptions.MPEG4.EncodingIntervalRange.Average();
                    }
                    if (videoOptions.MPEG4.FrameRateRange != null)
                    {
                        vec.RateControl.FrameRateLimit = videoOptions.MPEG4.FrameRateRange.Average();
                    }
                    if (videoOptions.Extension != null && videoOptions.Extension.MPEG4 != null && videoOptions.Extension.MPEG4.BitrateRange != null)
                    {
                        vec.RateControl.BitrateLimit = videoOptions.Extension.MPEG4.BitrateRange.Average();
                    }

                    if (videoOptions.MPEG4.GovLengthRange != null)
                    {
                        vec.MPEG4.GovLength = videoOptions.MPEG4.GovLengthRange.Min > 30 
                                              ? 
                                              videoOptions.MPEG4.GovLengthRange.Min
                                              :
                                              (videoOptions.MPEG4.GovLengthRange.Max < 30) ? videoOptions.MPEG4.GovLengthRange.Max : 30;
                    }
                    else
                    {
                        vec.MPEG4.GovLength = 30;
                    }

                    break;
                case VideoEncoding.H264:
                    vec.MPEG4 = null;
                    if (vec.H264 == null)
                    {
                        vec.H264 = new H264Configuration();
                    }
                    vec.Quality = videoOptions.QualityRange.Average();
                   
                    H264Profile h264Profile = H264Profile.High;
                    if (videoOptions.H264.H264ProfilesSupported.Contains(H264Profile.Baseline))
                    {
                        h264Profile = H264Profile.Baseline;
                    }
                    else if (videoOptions.H264.H264ProfilesSupported.Contains(H264Profile.Main))
                    {
                        h264Profile = H264Profile.Main;
                    }
                    else if (videoOptions.H264.H264ProfilesSupported.Contains(H264Profile.Extended))
                    {
                        h264Profile = H264Profile.Extended;
                    }
                    vec.H264.H264Profile = h264Profile;

                    vec.Resolution = videoOptions.MPEG4.ResolutionsAvailable[0];

                    if (vec.RateControl == null && (videoOptions.H264.EncodingIntervalRange != null
                        || videoOptions.H264.FrameRateRange != null ||
                        (videoOptions.Extension != null && videoOptions.Extension.H264 != null && videoOptions.Extension.H264.BitrateRange != null)))
                    {
                        vec.RateControl = new VideoRateControl();
                    }

                    if (videoOptions.H264.EncodingIntervalRange != null)
                    {
                        vec.RateControl.EncodingInterval = videoOptions.H264.EncodingIntervalRange.Average();
                    }
                    if (videoOptions.H264.FrameRateRange != null)
                    {
                        vec.RateControl.FrameRateLimit = videoOptions.H264.FrameRateRange.Average();
                    }
                    if (videoOptions.Extension != null && videoOptions.Extension.H264 != null && videoOptions.Extension.H264.BitrateRange != null)
                    {
                        vec.RateControl.BitrateLimit = videoOptions.Extension.H264.BitrateRange.Average();
                    }

                    if (videoOptions.H264.GovLengthRange != null)
                    {
                        vec.H264.GovLength = videoOptions.H264.GovLengthRange.Min > 30 
                                             ?
                                             videoOptions.H264.GovLengthRange.Min
                                             :
                                             (videoOptions.H264.GovLengthRange.Max < 30) ? videoOptions.H264.GovLengthRange.Max : 30;
                    }
                    else
                    {
                        vec.H264.GovLength = 30;
                    }

 
                    break;
            }        
        }
    }
}
