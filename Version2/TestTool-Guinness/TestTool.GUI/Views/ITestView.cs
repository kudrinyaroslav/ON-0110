///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
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

        void ClearTestResults();

        void ReportTestSuiteCompleted();


        void EnableTestRun(bool enable);

        Form Window { get; }
        IVideoForm GetVideoForm();

        void ReportError(string message);

        ITestTreeView TestTreeView { get; }
        IProfilesView ProfilesView { get; }
        IFeaturesView FeaturesView { get; }
        ITestResultView TestResultView { get; }
    }

    public interface IProfilesView : IView
    {
        void DisplayProfiles(IEnumerable<IProfileDefinition> profiles);
        void DisplayProfiles(IEnumerable<IProfileDefinition> supported,
            IEnumerable<IProfileDefinition> failed, IEnumerable<IProfileDefinition> notSupported);
        void DisplaySupportedFunctionality(Dictionary<Functionality, bool> functionality);
        void DisplayFunctionalityWithoutTestsInSuite(List<Functionality> functionality);
        void DisplayFunctionalityToBeTested(List<Functionality> functionality);
        void DisplaySkippedByFeaturesFunctionality(Dictionary<IProfileDefinition, List<Functionality>> functionality);
        void DisplayFailedByFeaturesFunctionality(Dictionary<IProfileDefinition, List<Functionality>> functionality);
        void DisplayScope(string scope, bool supported);
        void ClearProfiles();

    }

    public interface IFeaturesView : IView
    {
        void DisplayFeature(Feature feature, bool supported);
        void DisplayUndefinedFeature(Feature feature);
        void Clear();

    }

    public interface ITestResultView : IView
    {
        void BeginTest();
        void WriteLine(string logEntry);
        void DisplayStepResult(StepResult result);

    }

    public interface ITestTreeView : IView
    {
        void DisplayTests(IEnumerable<TestInfo> tests);
        List<TestInfo> SelectedTests { get; }
        List<string> SelectedGroups { get; }
        void ApplyProfileOptions(Profile profile);
        void SelectTests(IEnumerable<TestInfo> testInfos);
    }

}
