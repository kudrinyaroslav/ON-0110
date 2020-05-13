using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Reflection;
using TestTool.Common.Configuration;

namespace TestTool.Services
{
    public delegate void RequestProcessedEvent(RequestProcessingLog log);

    public delegate void ServiceEvent(BaseService service, string message);

    public abstract class BaseService
    {
        public BaseService()
        {
        }

        protected SimulatorConfiguration SimulatorConfiguration
        {
            get { return Configuration.Storage.Current; }
        }

        public static void UpdateConfiguration(SimulatorConfiguration configuration)
        {
            Configuration.Storage.Load(configuration);
        }

        public virtual void UpdateParameters(ServiceConfiguration configuration)
        {

        }

        internal event ServiceEvent MethodStarted;
        internal event ServiceEvent MethodCompleted;
        
        public abstract string GetServiceName();

        public abstract string GetLocalAddress();

        protected abstract Type GetContractType();
        
        public List<string> GetOperationsList()
        {
            List<string> operations = new List<string>();

            Type contractType = GetContractType();

            foreach (MethodInfo method in contractType.GetMethods())
            {
                object[] testAttributes = method.GetCustomAttributes(typeof(OperationContractAttribute), true);

                if (testAttributes.Length > 0)
                {
                    OperationContractAttribute attr = (OperationContractAttribute)testAttributes[0];
                    if (!string.IsNullOrEmpty(attr.Action))
                    {
                        operations.Add(method.Name);
                    }
                }
            }


            return operations;
        }

        #region Logging

        private bool _working;
        private string _currentMethod;

        protected void BeginMethod(string method)
        {
            _currentMethod = method;
            _working = true;
            if (MethodStarted != null)
            {
                MethodStarted(this, method);
            }
        }
        
        protected void EndMethod()
        {
            if (MethodCompleted != null && _working)
            {
                MethodCompleted(this, _currentMethod);
            }
        }


        #endregion

        private Transport.ILogger _logger;
        public void AttachLogger(Transport.ILogger logger)
        {
            _logger = logger;
        }

        protected void Log(string message)
        {
            if (_logger != null)
            {
                _logger.LogEvent(message+Environment.NewLine);
            }
        }
    }
}
