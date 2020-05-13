///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
 
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

            if (testInfo.RequiredFeatures.Count == 0)
            {
                if (testInfo.RequirementLevel == RequirementLevel.Must)
                {
                    info = "RUN ALWAYS";
                }
                else
                {
                    info = "RUN IN DIAGNOSTIC ONLY";
                }
            }
            else
            {

                string supportedFeatures = testInfo.RequiredFeatures.GetFeaturesString();

                string run = testInfo.RequirementLevel == RequirementLevel.Must ? "RUN" : "RUN IN DIAGNOSTIC ONLY";
                    
                info += string.Format("{0} IF SUPPORTED ({1})", run, supportedFeatures);
                         
            }

            return info;

        }

        /// <summary>
        /// Creates string with functionality under test values
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        /// <returns>String with functionality under test values.</returns>
        public static string GetFunctionalityString(this TestInfo testInfo)
        {
            if (testInfo.FunctionalityUnderTest != null && testInfo.FunctionalityUnderTest.Any())
                return string.Join(", ", testInfo.FunctionalityUnderTest.Select(F => FunctionalityHelper.GetDisplayName(F, false)));
            else
                return "None";
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
