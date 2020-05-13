using System;
using System.Collections.Generic;
using TestTool.GUI.Data;
using TestTool.Services;
using TestTool.Device.Data;

namespace TestTool.GUI.Views
{
    internal interface ITestView : IView
    {
        void SwitchToWorkingState();
        void SwitchToIdleState();

        void LogSimulatorEvent(string message);
        void Clear();
        void DisplayRequestProcessingLog(RequestProcessingLog log);

        void ShowServiceInformation(List<ServiceContractInfo> infos);
        void DisplayConfigurations(IEnumerable<ConfigurationFactory> configurations);

        void UpdateOperationUsage(string serviceName, string operationName, Data.OperationUsage usage);
    }
}
