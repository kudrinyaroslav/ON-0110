///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
 
namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Contains extension method for create tooltips etc. strings.
    /// </summary>
    static class Converter
    {
        /// <summary>
        /// Creates requirement description for TestInfo (e.g. requirement level and required features) 
        /// given.
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        /// <returns>Requirement description.</returns>
        public static string GetRequirementString(this TestInfo testInfo)
        {
            string info = string.Empty;
            switch (testInfo.RequirementLevel)
            {
                case RequirementLevel.Must:
                    {
                        info = "MUST";
                    }
                    break;
                case RequirementLevel.ConditionalMust:
                    {
                        List<Feature> toBeSupported = new List<Feature>();
                        List<Feature> toBeImplemented = new List<Feature>();

                        toBeSupported.AddRange(testInfo.RequiredFeatures.Where( f => FeaturesHelper.FeatureRealization(f) == FeaturesHelper.FeatureRealizationType.Supported ) );
                        toBeImplemented.AddRange(testInfo.RequiredFeatures.Where(f => FeaturesHelper.FeatureRealization(f) == FeaturesHelper.FeatureRealizationType.Implemented));

                        string supportedFeatures = toBeSupported.GetFeaturesString();
                        string implementedFeatures = toBeImplemented.GetFeaturesString();

                        if (toBeImplemented.Count == 0)
                        {
                            info = string.Format("MUST IF SUPPORTED ({0})", supportedFeatures);
                        }
                        else
                        {
                            if (toBeSupported.Count == 0)
                            {
                                info = string.Format("MUST IF IMPLEMENTED ({0})", implementedFeatures);
                            }
                            else
                            {
                                if (toBeImplemented.Contains(Feature.PTZAbsoluteOrRelative))
                                {
                                    List<Feature> toBeImplementedLite =
                                        toBeImplemented.Where(f => f != Feature.PTZAbsoluteOrRelative).ToList();

                                    implementedFeatures = toBeImplementedLite.GetFeaturesString();

                                    info = string.Format("MUST IF SUPPORTED ({0}) AND IMPLEMENTED ({1}) AND IMPLEMENTED ({2})",
                                        supportedFeatures,
                                        implementedFeatures,
                                        FeaturesHelper.GetDisplayName(Feature.PTZAbsoluteOrRelative));

                                }
                                else
                                {
                                    info = string.Format("MUST IF SUPPORTED ({0}) AND IMPLEMENTED ({1})",
                                        supportedFeatures,
                                        implementedFeatures);
                                }

                            }
                        }

                    };
                    break;
                case RequirementLevel.ConditionalShould:
                    {
                        List<Feature> toBeSupported = new List<Feature>();
                        List<Feature> toBeImplemented = new List<Feature>();

                        toBeSupported.AddRange(testInfo.RequiredFeatures.Where(f => FeaturesHelper.FeatureRealization(f) == FeaturesHelper.FeatureRealizationType.Supported));
                        toBeImplemented.AddRange(testInfo.RequiredFeatures.Where(f => FeaturesHelper.FeatureRealization(f) == FeaturesHelper.FeatureRealizationType.Implemented));

                        string supportedFeatures = toBeSupported.GetFeaturesString();
                        string implementedFeatures = toBeImplemented.GetFeaturesString();

                        if (toBeImplemented.Count == 0)
                        {
                            info = string.Format("SHOULD IF SUPPORTED ({0})", supportedFeatures);
                        }
                        else
                        {
                            if (toBeSupported.Count == 0)
                            {
                                info = string.Format("SHOULD IF IMPLEMENTED ({0})", implementedFeatures);
                            }
                            else
                            {
                                if (toBeImplemented.Contains(Feature.PTZAbsoluteOrRelative))
                                {
                                    List<Feature> toBeImplementedLite =
                                        toBeImplemented.Where(f => f != Feature.PTZAbsoluteOrRelative).ToList();

                                    implementedFeatures = toBeImplementedLite.GetFeaturesString();

                                    info = string.Format("SHOULD IF SUPPORTED ({0}) AND IMPLEMENTED ({1}) AND IMPLEMENTED ({2})",
                                        supportedFeatures,
                                        implementedFeatures, 
                                        FeaturesHelper.GetDisplayName(Feature.PTZAbsoluteOrRelative));

                                }
                                else
                                {
                                    info = string.Format("SHOULD IF SUPPORTED ({0}) AND IMPLEMENTED ({1})",
                                        supportedFeatures,
                                        implementedFeatures);
                                }
                            }
                        }

                    };
                    break;
                case RequirementLevel.Should:
                    {
                        info = "SHOULD";
                    }
                    break;                
                case RequirementLevel.Optional:
                    {
                        info = "OPTIONAL";
                    }
                    break;                
            }
            return info;

        }

        static string GetFeaturesString(this TestInfo testInfo)
        {
            return GetFeaturesString(testInfo.RequiredFeatures);
        }

        /// <summary>
        /// Creates list of features user-friendly names.
        /// </summary>
        /// <param name="features">list of features.</param>
        /// <returns></returns>
        static string GetFeaturesString(this IList<Feature> features)
        {
            string feature;
            if (features.Count > 0)
            {
                StringBuilder sb = new StringBuilder(FeaturesHelper.GetDisplayName(features[0]));
                for (int i = 1; i < features.Count; i++)
                {
                    sb.AppendFormat(", {0}", FeaturesHelper.GetDisplayName(features[i]));
                }
                feature = sb.ToString();
            }
            else
            {
                feature = "UNDEFINED";
            }
            return feature;
        }

    }
}
