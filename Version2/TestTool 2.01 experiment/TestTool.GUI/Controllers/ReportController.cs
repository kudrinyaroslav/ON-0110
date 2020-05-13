///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.GUI.Views;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// GUI logic for the Request tab.
    /// </summary>
    class ReportController : Controller<IReportView>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public ReportController(IReportView view)
            :base(view)
        {
        }

        /// <summary>
        /// Adds line to the report
        /// </summary>
        /// <param name="entry">Log entry.</param>
        public void LogEvent(string entry)
        {
            View.WriteLine(entry);
        }

        /// <summary>
        /// Clears log.
        /// </summary>
        public void Clear()
        {
            View.Clear();
        }

        /// <summary>
        /// Updates available actions.
        /// </summary>
        public override void UpdateViewFunctions()
        {
            View.EnableSaveReport(ReportNotEmpty() 
                && CurrentState == Enums.ApplicationState.Idle
                && _certificationMode);

            base.UpdateViewFunctions();
        }

        /// <summary>
        /// Check if report is not empty.
        /// </summary>
        /// <returns>True if log is not empty.</returns>
        public bool ReportNotEmpty()
        {
            Data.TestLog log = ContextController.GetTestLog();
            return log.TestResults != null && log.TestResults.Count > 0;
        }

        /// <summary>
        /// Checks if all information required is entered at the Setup tab.
        /// </summary>
        /// <returns>True if user filled all the fields required.</returns>
        public bool IsTestInfoFull()
        {
            SetupInfo setupInfo = ContextController.GetSetupInfo();
            
            if (string.IsNullOrEmpty(setupInfo.TesterInfo.Operator) ||
                string.IsNullOrEmpty(setupInfo.TesterInfo.Organization) ||
                string.IsNullOrEmpty(setupInfo.TesterInfo.Address) ||
                string.IsNullOrEmpty(setupInfo.DevInfo.FirmwareVersion) ||
                string.IsNullOrEmpty(setupInfo.DevInfo.Manufacturer) ||
                string.IsNullOrEmpty(setupInfo.DevInfo.Model) ||
                string.IsNullOrEmpty(setupInfo.DevInfo.SerialNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool _certificationMode;

        /// <summary>
        /// Enters/exits certification mode.
        /// </summary>
        /// <param name="bOn"></param>
        public void SetCertificationMode(bool bOn)
        {
            _certificationMode = bOn;
            View.EnableSaveReport(ReportNotEmpty() && CurrentState == Enums.ApplicationState.Idle && bOn);
        }

        /// <summary>
        /// Saves test results.
        /// </summary>
        /// <param name="filename">Path to the file to write.</param>
        public void SaveTestResults(string filename)
        {
            Utils.PdfReportGenerator reportGenerator = new PdfReportGenerator();
            reportGenerator.OnException += reportGenerator_OnException;
            reportGenerator.OnReportSaved += reportGenerator_OnReportSaved;
            
            Data.TestLogFull testLog = new TestLogFull();
            Data.TestLog log = ContextController.GetTestLog();
            testLog.TestResults = log.TestResults;
            testLog.Tests = log.Tests;
            if (log.TestExecutionTime != DateTime.MinValue)
            {
                testLog.TestExecutionTime = log.TestExecutionTime;
            }
            else
            {
                testLog.TestExecutionTime = DateTime.Now;
            }
            SetupInfo info = ContextController.GetSetupInfo();
            testLog.TesterInfo = info.TesterInfo;
            testLog.Application = ContextController.GetApplicationInfo();
            testLog.DeviceInfo = info.DevInfo;
            testLog.OtherInformation = info.OtherInfo;

            testLog.DeviceEnvironment = ContextController.GetDeviceEnvironment();

            testLog.CoreSpecification = info.CoreSpecification;

            reportGenerator.CreateReport(filename, testLog);

        }

        /// <summary>
        /// Handles operation competion.
        /// </summary>
        void reportGenerator_OnReportSaved()
        {
            View.ReportOperationCompleted();    
        }

        /// <summary>
        /// Handles exception in view generation.
        /// </summary>
        /// <param name="ex">Exception data.</param>
        void reportGenerator_OnException(Exception ex)
        {
            View.ReportException(ex);
        }


        ///////////////////////////////////////////////////////////////////////////
        //!  @author        Ivan Vagunin
        ////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void LoadSavedContext(SavedContext context)
        {
            if (context.ReportInfo != null)
            {
                View.FileName = context.ReportInfo.FileName;
            }
        }
        public override void UpdateContext()
        {
            ReportInfo info = new ReportInfo();
            info.FileName = View.FileName;
            ContextController.UpdateReportInfo(info);
        }
    }
}
