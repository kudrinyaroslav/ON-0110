using System;
using System.Collections.Generic;
using TestTool.GUI.Enums;
using TestTool.GUI.Utils.Profiles;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine;

namespace TestTool.GUI.Controllers
{
    partial class TestController
    {
        #region Output

        /// <summary>
        /// Raises TestSuiteStarted event if handler is not empty
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="conformance"></param>
        void ReportTestSuiteStarted(TestSuiteParameters parameters, bool conformance)
        {
            if (TestSuiteStarted != null)
            {
                TestSuiteStarted(parameters, conformance);
            }
        }

        /// <summary>
        /// Creates log entry and appends to log where it is necessary
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="level"></param>
        void WriteLog(string entry, LogEntryLevel level)
        {
            string indent = string.Empty;
            switch (level)
            {
                case LogEntryLevel.Step:
                    indent = "   ";
                    break;
                case LogEntryLevel.StepDetails:
                    indent = "      ";
                    break;
            }

            string offset = Environment.NewLine + indent;
            string line = string.Format("{0}{1}", indent, entry.Replace(Environment.NewLine, offset));

            _plainTextLog.AppendLine(line);
            if (level == LogEntryLevel.Test || level == LogEntryLevel.Step)
            {
                _shortTestLog.AppendLine(line);
            }

            // add every warning in warning's log
            if (entry.ToLower().Contains("warning:"))
                _warningsLog.Add(line);

            DisplayLogLine(line);
        }

        /// <summary>
        /// Displays at the profiles tree functionality
        ///   - to be tested;
        ///   - mandatory without features support
        ///   - optional without fetures support
        ///   - without tests in suite 
        /// </summary>
        /// <param name="tests"></param>
        void DisplayFunctionalitySupport(IEnumerable<TestInfo> tests)
        {
            if (View.ProfilesView != null)
            {
                List<Feature> features = new List<Feature>(_td.Features);
                features.AddRange(_td.UndefinedFeatures);

                var parameters = new Dictionary<string, object>();
                parameters.Add("MaxPullPoints", _td.MaxPullPoints);              

                ProfilesSupportInfo info = ProfilesSupportInfo.LoadPreliminary(_onvifProfiles, _testInfos, tests, features, parameters);

                View.ProfilesView.DisplayFunctionalityWithoutTestsInSuite(info.FunctionalityWithoutTests);
                View.ProfilesView.DisplayFunctionalityToBeTested(info.FunctionalityUnderTests);
                View.ProfilesView.DisplayMandatoryFunctionalityWithoutFeatures(info.MandatoryFunctionalityWithoutFeatures);
                View.ProfilesView.DisplayOptionalFunctionalityWithoutFeatures(info.OptionalFunctionalityUnderSkippedTests);

                DisplayProfileScopes();
                DisplayDiscoveryTypes();
            }
        }

        /// <summary>
        /// Displays device scopes at the profiels tree
        /// </summary>
        void DisplayProfileScopes()
        {
            if (View.ProfilesView != null)
            {
                List<String> scopes = new List<String>();
                foreach (IProfileDefinition profile in _onvifProfiles)
                {
                    foreach (string f in profile.MandatoryScopes)
                    {
                        if (!scopes.Contains(f))
                        {
                            scopes.Add(f);
                        }
                    }
                }
                foreach (string f in scopes)
                {
                    View.ProfilesView.DisplayScope(f, _td.Scopes.Contains(f));
                }
            }
        }

        void DisplayDiscoveryTypes()
        {
            if (View.ProfilesView != null)
            {
                var discoveryTypes = new List<Feature>();
                discoveryTypes.Add(Feature.DiscoveryTypesDnNetworkVideoTransmitter);
                discoveryTypes.Add(Feature.DiscoveryTypesTdsDevice);

                foreach (var f in discoveryTypes)
                {
                    View.ProfilesView.DisplayDiscoveryType(f, _td.Features.Contains(f));
                }
            }
        }

        /// <summary>
        /// Udates test log
        /// </summary>
        /// <param name="line"></param>
        void DisplayLogLine(string line)
        {
            if (_scrollingEnabled)
            {
                if (View.TestResultView != null)
                {
                    View.TestResultView.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Adds step
        /// </summary>
        /// <param name="result"></param>
        void DisplayStepResult(StepResult result)
        {
            if (_scrollingEnabled)
            {
                if (View.TestResultView != null)
                {
                    View.TestResultView.DisplayStepResult(result);
                }
            }
        }

        /// <summary>
        /// Reports that test suite is completed (for situations when tests are executed NOT as "run single")
        /// </summary>
        void ReportTestSuiteCompleted()
        {
            if (_testsCount > 1 || !_runningSingle)
            {
                View.ReportTestSuiteCompleted();
            }
        }

        #endregion
    }
}
