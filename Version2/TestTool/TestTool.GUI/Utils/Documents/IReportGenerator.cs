using System;

namespace TestTool.GUI.Utils
{
    public interface IReportGenerator
    {
        event Action<Exception> OnException;

        event Action OnReportSaved;

        void CreateReport(string fileName, Data.TestLogFull log);
    }
}