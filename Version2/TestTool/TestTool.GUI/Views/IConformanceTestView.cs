///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Data;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Data;
//using System.Collections.Generic;

namespace TestTool.GUI.Views
{
    interface IConformanceTestView : IView
    {
        string Brand { get; set; }
        string Model { get; set; }
        string OnvifProductName { get; set; }
        string ProductTypes { get; set; }
        string ProductTypesAll { get; set; }
        string ProductTypesOther { get; set; }
        string OtherInformation { get; set; }
        string OperatorName { get; set; }
        string OrganizationName { get; set; }
        string OrganizationAddress { get; set; }
        string MemberName { get; set; }
        string MemberAddress { get; set; }

        string InternationalAddress { get; set; }
        string RegionalAddress { get; set; }
        string SupportUrl { get; set; }
        string SupportEmail { get; set; }
        string SupportPhone { get; set; }
        
        void EndFeatureDefinition(Tests.Definitions.Trace.TestStatus status);
        void ReportFeatureDefinitionCompleted(bool finished);
        void BeginTest(TestInfo testInfo);
        void EndTest(string testId, string testName, Tests.Definitions.Trace.TestStatus status);
        void ReportTestSuiteCompleted(bool preliminaryPassed, int passed, int failed, bool completedNormally);
        void ClearInfo();

        void DefineTestsCount(int total);
        void ReportProgress(int done, int total, int failed);

        void ReportDocumentCreationCompleted();
        void EnableSaveReport(bool bEnable);
        void EnableGenerateDoc(bool bEnable);

        void Log(string entry);
        void ClearLog();

    }
}
