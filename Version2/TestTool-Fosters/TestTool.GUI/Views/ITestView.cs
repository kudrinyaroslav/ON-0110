///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using TestTool.Tests.Common.TestEngine;
using TestTool.GUI.Data;
using System.Windows.Forms;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;

namespace TestTool.GUI.Views
{
    interface ITestView : IView
    {
        void BeginTest(TestInfo testInfo);
        void EndTest(TestResult testResult);
        void WriteLine(string logEntry);
        void DisplayStepResult(StepResult result);
        void ReportTestSuiteCompleted();
        void ClearCurrentLog();

        void DisplayTests(IEnumerable<TestInfo> tests);
        void DisplayProfiles(IEnumerable<IProfileDefinition> profiles);
        void DisplayProfiles(IEnumerable<IProfileDefinition> supported,
            IEnumerable<IProfileDefinition> failed, IEnumerable<IProfileDefinition> notSupported);
        void DisplaySupportedFunctionality(Dictionary<Functionality, bool> functionality);
        void DisplayFunctionalityWithoutTestsInSuite(List<Functionality> functionality);
        void DisplayFunctionalityToBeTested(List<Functionality> functionality);
        void DisplaySkippedByFeaturesFunctionality(Dictionary<IProfileDefinition, List<Functionality>> functionality);
        void DisplayFailedByFeaturesFunctionality(Dictionary<IProfileDefinition, List<Functionality>> functionality);

        Form Window { get; }
        IVideoForm GetVideoForm();
        
        void EnableTestRun(bool enable);

        List<TestInfo> SelectedTests { get; }
        List<string> SelectedGroups { get; }
        //bool InteractiveFirst { get; }

        void ApplyProfileOptions(Profile profile);
        void SelectTests(IEnumerable<TestInfo> testInfos);
        
        void DisplayFeature(Feature feature, bool supported);
        void DisplayUndefinedFeature(Feature feature);
        void DisplayScope(string scope, bool supported);

        void ClearFeatures();
        void ClearTestResults();
        void ClearProfiles();

        void ReportError(string message);

    }
}
