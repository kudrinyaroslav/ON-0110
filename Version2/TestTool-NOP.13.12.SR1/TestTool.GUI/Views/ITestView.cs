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
using TestTool.Tests.Engine.Data;

namespace TestTool.GUI.Views
{
    interface ITestView : IView
    {
        void BeginTest(TestInfo testInfo);
        void EndTest(TestResult testResult);
        
        void ClearTestResults();
        void ReportTestSuiteCompleted();
        
        void EnableTestRun(bool enable);

        bool Repeat { get; set; }

        Form Window { get; }
        IVideoForm GetVideoForm();
        
        ITestTreeView TestTreeView { get; }
        IProfilesView ProfilesView { get; }
        IFeaturesView FeaturesView { get; }
        ITestResultView TestResultView { get; }
    }

    /// <summary>
    /// Interface for direct access to profiles tree
    /// </summary>
    public interface IProfilesView
    {
        void DisplayProfiles(IEnumerable<IProfileDefinition> profiles);
        void DisplayProfiles(IEnumerable<IProfileDefinition> supported,
            IEnumerable<IProfileDefinition> failed, IEnumerable<IProfileDefinition> notSupported);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionality"></param>
        /// <remarks>By results of tests execution</remarks>
        void DisplaySupportedFunctionality(Dictionary<Functionality, bool> functionality);
        void DisplayFunctionalityWithoutTestsInSuite(List<Functionality> functionality);
        void DisplayFunctionalityToBeTested(List<Functionality> functionality);
        void DisplayOptionalFunctionalityWithoutFeatures(Dictionary<IProfileDefinition, List<Functionality>> functionality);
        void DisplayMandatoryFunctionalityWithoutFeatures(Dictionary<IProfileDefinition, List<Functionality>> functionality);
        void DisplayScope(string scope, bool supported);
        void DisplayDiscoveryType(Feature dt, bool supported);
        void ClearProfiles();

    }

    /// <summary>
    /// Interface for direct access to features tree
    /// </summary>
    public interface IFeaturesView 
    {
        void DisplayFeature(Feature feature, bool supported);
        void DisplayUndefinedFeature(Feature feature);
        void Clear();
        
    }

    /// <summary>
    /// Interface for direct access to test results view
    /// </summary>
    public interface ITestResultView 
    {
        void BeginTest();
        void WriteLine(string logEntry);
        void DisplayStepResult(StepResult result);

    }
    /// <summary>
    /// Interface for directa access to tests tree.
    /// </summary>
    public interface ITestTreeView 
    {
        void DisplayTests(IEnumerable<TestInfo> tests);
        ExecutableTestList SelectedTests { get; }
        List<string> SelectedGroups { get; }
        void ApplyProfileOptions(Profile profile);
        void SelectTests(IEnumerable<TestInfo> testInfos);
    }

}
