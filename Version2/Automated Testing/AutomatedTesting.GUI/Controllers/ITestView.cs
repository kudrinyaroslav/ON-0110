using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using AutomatedTesting.GUI.Data;
using TestTool.Tests.Definitions.Trace;

namespace AutomatedTesting.GUI.Controllers
{
    public enum TestCaseStatus
    { 
        Red,
        Green,
        Yellow
    }

    interface ITestView
    {
        void BeginTesting();
        void EndTesting();

        void BeginTest(TestInfo testInfo, string testCaseId);
        void EndTest(TestResult testResult);
        
        void WriteLine(string message);
        void DisplayStepResult(StepResult result);

        void DisplayTestResult(string caseId, TestCaseStatus status);

    }
}
