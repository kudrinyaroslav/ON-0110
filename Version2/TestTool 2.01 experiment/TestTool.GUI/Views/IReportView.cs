﻿///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.GUI.Views
{
    /// <summary>
    /// Report page interface
    /// </summary>
    interface IReportView : IView
    {
        string FileName { get; set; }

        void WriteLine(string entry);
        void Clear();

        void EnableSaveReport(bool bEnable);

        void ReportException(Exception exception);
        void ReportOperationCompleted();
    }
}
