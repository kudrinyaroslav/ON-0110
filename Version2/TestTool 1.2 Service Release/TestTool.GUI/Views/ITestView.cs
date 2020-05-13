///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using TestTool.Tests.Common.Trace;
using TestTool.Tests.Common.TestEngine;
using TestTool.GUI.Data;
using System.Windows.Forms;

namespace TestTool.GUI.Views
{
    interface ITestView : IView
    {
        void BeginTest(TestInfo testInfo);
        void EndTest(TestResult testResult);

        void DisplayTests(List<TestInfo> tests);

        void ReportTestSuiteCompleted();

        void WriteLine(string logEntry);
        void DisplayStepResult(StepResult result);

        Form Window { get; }
        
        void EnableTestRun(bool enable);

        List<TestInfo> SelectedTests { get; }
        List<string> SelectedGroups { get; }
        bool InteractiveFirst { get; }

        void ApplyProfileOptions(Profile profile);
        void SetCertificationMode(bool bOn);
        void SelectFeatureDependentTests(IEnumerable<TestInfo> testInfos);
        
        IVideoForm GetVideoForm();

    
    }
}
