using System;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Common.TestEngine;

namespace TestTool.GUI
{
    /// <summary>
    /// TestView implementation for silent run
    /// </summary>
    class SilentTestView : Views.ITestView
    {
        private CompactProcessingForm _form;
        public SilentTestView(CompactProcessingForm form)
        {
            _form = form;
        }

        private int _testsCount;
        private int _completedTests;
        private int _failedTests;

        public void DefineTestCount(int count)
        {
            _testsCount = count;
            _form.FeatureDefinitionCompleted(count);
        }


        #region ITestView Members

        #region needed for silent mode
        
        public void ReportTestSuiteCompleted()
        {
        }

        #endregion
        
        #region Logging

        //
        // MAY BE it will be usefull to add possibility to log all these lines.
        // This should be controlled by some switch, e.g. -log <filename>
        //
        //
        //
        //
        
        public void BeginTest(Tests.Definitions.Data.TestInfo testInfo)
        {
            
        }

        public void EndTest(Data.TestResult testResult)
        {
            bool isTest = testResult.TestInfo.ProcessType == ProcessType.Test;
            if (isTest)
            {
                _completedTests++;
                if (testResult.Log.TestStatus == Tests.Definitions.Trace.TestStatus.Failed)
                {
                    _failedTests++;
                }
                _form.ReportProgress(_completedTests, _failedTests);
            }
            else
            {
            }
        }
        
        #endregion

        #region Display tests, profiles, features
       
        public void ClearTestResults()
        {
            
        }
        
        #endregion
        
        public void ClearCurrentLog()
        {
            
        }

        public System.Windows.Forms.Form Window
        {
            get { return null; }
        }


        private VideoContainer _videoWindow;


        /// <summary>
        /// Gets video form.
        /// </summary>
        /// <returns></returns>
        public Tests.Definitions.Interfaces.IVideoForm GetVideoForm()
        {
            if (_videoWindow == null)
            {
                VideoContainer wnd = new VideoContainer();
                VideoContainer.SilentMode = true;
                _videoWindow = wnd;
            }
            return _videoWindow;
        }

        public void EnableTestRun(bool enable)
        {
            
        }

        public void ReportError(string message)
        {
            
        }

        #endregion

        #region IView Members

        public void SwitchToState(Enums.ApplicationState state)
        {
            
        }

        public Controllers.IController GetController()
        {
            return null;
        }

        #endregion

        #region ITestView Members


        public Views.IProfilesView ProfilesView
        {
            get { return null; }
        }

        public Views.IFeaturesView FeaturesView
        {
            get { return null; }
        }
        
        ConsoleTestResultView _testResultView = new ConsoleTestResultView();

        public Views.ITestResultView TestResultView
        {
            get { return _testResultView; }
        }
        
        public Views.ITestTreeView TestTreeView
        {
            get { return null; }
        }
        
        public bool Repeat
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        #endregion

        #region IView Members


        public void ShowError(Exception e)
        {
        }

        public void ShowError(string message)
        {
        }

        #endregion
    }

    class ConsoleTestResultView: Views.ITestResultView
    {

        #region ITestResultView Members

        public void WriteLine(string logEntry)
        {
            Console.WriteLine(logEntry);
        }

        public void DisplayStepResult(TestTool.Tests.Definitions.Trace.StepResult result)
        {
            
        }
        
        public void BeginTest()
        {

        }

        #endregion
    }
}
