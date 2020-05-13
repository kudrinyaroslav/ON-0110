using System.Collections.Generic;
using System.Linq;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;

namespace TestTool.GUI.Utils.Profiles
{
    /// <summary>
    /// Handles functionality support
    /// </summary>
    public class ProfilesSupportInfo
    {

        public List<Functionality> FunctionalityUnderTests { get; private set; }
        public List<Functionality> FunctionalityWithoutTests { get; private set; }
        public Dictionary<Functionality, bool> FunctionalitiesDefined { get; private set; }
        
        // these are profile-dependent!!!
        public Dictionary<IProfileDefinition, List<Functionality>> OptionalFunctionalityUnderSkippedTests { get; private set; }
        public Dictionary<IProfileDefinition, List<Functionality> > MandatoryFunctionalityWithoutFeatures{ get; private set; }

        /// <summary>
        /// Creates list of functionalities supported using test results 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static Dictionary<Functionality, bool> CheckTestResults(Dictionary<TestInfo, TestStatus> log)
        {
            List<TestInfo> selectedTests = new List<TestInfo>();
            selectedTests.AddRange(log.Keys);

            Dictionary<Functionality, bool> functionalities = new Dictionary<Functionality, bool>();

            foreach (TestInfo info in log.Keys)
            {
                bool passed = log[info] == TestStatus.Passed;

                foreach (Functionality f in info.FunctionalityUnderTest)
                {
                    if (functionalities.ContainsKey(f))
                    {
                        if (!passed && functionalities[f])
                        {
                            functionalities[f] = false;
                        }
                    }
                    else
                    {
                        functionalities.Add(f, passed);
                    }
                }
            }
            
            foreach (Functionality f in functionalities.Keys)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", f, functionalities[f]));
            }

            return functionalities;
        }

        /// <summary>
        /// Creates list of profiles supported.
        /// </summary>
        /// <param name="profiles"></param>
        /// <param name="allTests"></param>
        /// <param name="selectedTests"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public static ProfilesSupportInfo LoadPreliminary(IEnumerable<IProfileDefinition> profiles,
                                                          IEnumerable<TestInfo> allTests,
                                                          IEnumerable<TestInfo> selectedTests,
                                                          IEnumerable<Feature> features,
                                                          Dictionary<string, object> parameters)
        {
            // all functionality to be tested
            List<Functionality> functionalitiesUnderTests = new List<Functionality>();
            // no tests in suite at all
            List<Functionality> noTests = new List<Functionality>();

            Dictionary<IProfileDefinition, List<Functionality>> allSkippedByFeatures = new Dictionary<IProfileDefinition, List<Functionality>>();
            Dictionary<IProfileDefinition, List<Functionality>> allMandatorySkippedByFeatures = new Dictionary<IProfileDefinition, List<Functionality>>();

            // functionalities under tests
            foreach (TestInfo info in selectedTests)
            {
                foreach (Functionality f in info.FunctionalityUnderTest)
                {
                    if (!functionalitiesUnderTests.Contains(f))
                    {
                        functionalitiesUnderTests.Add(f);
                    }
                }
            }

            foreach (IProfileDefinition profile in profiles)
            {
                List<Functionality> skippedByFeatures = new List<Functionality>();
                List<Functionality> mandatorySkippedByFeatures = new List<Functionality>();
                allSkippedByFeatures.Add(profile, skippedByFeatures);
                allMandatorySkippedByFeatures.Add(profile, mandatorySkippedByFeatures);

                System.Diagnostics.Debug.WriteLine("Profile: " + profile.GetProfileName());

                foreach (FunctionalityItem item in profile.Functionalities)
                {
                    System.Diagnostics.Debug.Write(item.Functionality.ToString());

                    if (item.Features != null && item.Features.Length > 0)
                    {
                        bool featureMissing = false;
                        foreach (Feature f in item.Features)
                        {
                            if (!features.ContainsFeature(f))
                            {
                                featureMissing = true;
                                break;
                            }
                        }
                        if (featureMissing)
                        {
                            if (!profile.IsFunctionalityOptional(item.Functionality, features))
                            {
                                mandatorySkippedByFeatures.Add(item.Functionality);
                            }
                            else
                            {
                                skippedByFeatures.Add(item.Functionality);
                            }
                            continue;
                        }
                        else
                        {
                            //Feature is supported
                            if (Functionality.AtLeastTwoPullPointSubscription == item.Functionality)
                            {
                                var maxPullPointsValue = (int) parameters["MaxPullPoints"];

                                if (maxPullPointsValue < 2)
                                {
                                    if (!profile.IsFunctionalityOptional(item.Functionality, features))
                                    {
                                        mandatorySkippedByFeatures.Add(item.Functionality);
                                    }
                                    else
                                    {
                                        skippedByFeatures.Add(item.Functionality);
                                    }
                                    continue;
                                }
                            }
                        }
                    }

                    if (functionalitiesUnderTests.Contains(item.Functionality))
                    {
                        // definetely not skipped
                        continue;
                    }
                    
                    //
                    // Skipped. It can mean:
                    // - no tests at all
                    // - no tests since feature not supported
                    //     this can be OK or NOK for profile
                    
                    if (noTests.Contains(item.Functionality))
                    {
                        System.Diagnostics.Debug.WriteLine(" not under test at all ");
                        // status is known
                        continue;
                    }

                    bool hasTest = selectedTests.Any(ti => ti.FunctionalityUnderTest != null && ti.FunctionalityUnderTest.Contains(item.Functionality));
                    if (!hasTest)
                    {
                        noTests.Add(item.Functionality);
                    }
                }
            }

            ProfilesSupportInfo psi = new ProfilesSupportInfo();
            psi.FunctionalityUnderTests = functionalitiesUnderTests;
            psi.FunctionalityWithoutTests = noTests;
            psi.OptionalFunctionalityUnderSkippedTests = allSkippedByFeatures;
            psi.MandatoryFunctionalityWithoutFeatures = allMandatorySkippedByFeatures;

            return psi;
        }

    }
}
