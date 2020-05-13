using System;
using System.Collections.Generic;
using System.Linq;
using TestTool.Common.Attributes;
using TestTool.Device;
using TestTool.GUI.Data;
using TestTool.Services;
using TestTool.Common;
using System.Reflection;
using System.IO;

namespace TestTool.GUI.Controllers
{
    class TestController : Controller<Views.ITestView>
    {

        private Simulator _simulator;

        public TestController(Views.ITestView view)
            :base(view)
        {

            _simulator = new Simulator();
            _simulator.RequestProcessed += _simulator_RequestProcessed;
            _simulator.RequestReceived += new TrafficEvent(_simulator_RequestReceived);
            _simulator.ResponseSent += new TrafficEvent(_simulator_ResponseSent);
            _simulator.SimulatorEvent += new SimulatorEvent(_simulator_SimulatorEvent);
            _simulator.EventLogged += new MessageEvent(_simulator_EventLogged);
            InitializeServicesList();
            InitializeConfigurations();
        }


        List<ServiceUsageInformation> _usageInfo = new List<ServiceUsageInformation>();

        #region Trees

        void InitializeServicesList()
        {
            List<Device.Data.ServiceContractInfo> infos = _simulator.GetServicesInfo();
            View.ShowServiceInformation(infos);
            
            foreach (Device.Data.ServiceContractInfo  info in infos)
            {
                ServiceUsageInformation information = new ServiceUsageInformation();
                information.ServiceName = info.ServiceName;
                foreach (string operation in info.OperationsList)
                {
                    OperationInfo operationInfo = new OperationInfo();
                    operationInfo.OperationName = operation;
                    operationInfo.UsageInfo = OperationUsage.NotCovered;
                    information.OperationsList.Add(operationInfo);
                }
                _usageInfo.Add(information);
            }
        }

        private List<ConfigurationFactory> _configurations;
        void InitializeConfigurations()
        {
            _configurations = new List<ConfigurationFactory>();
            

            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);

            foreach (string file in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    System.Reflection.Assembly assembly = Assembly.LoadFile(file);
                    if (assembly.GetCustomAttributes(
                        typeof(TestAssemblyAttribute),
                        false).Length > 0)
                    {
                        // Test assembly

                        foreach (Type t in assembly.GetTypes())
                        {
                            object[] attrs = t.GetCustomAttributes(typeof(TestClassAttribute), true);
                            if (attrs.Length > 0)
                            {
                                object initializer = Activator.CreateInstance(t);

                                foreach (MethodInfo mi in t.GetMethods())
                                {
                                    object[] testAttributes = mi.GetCustomAttributes(typeof(TestAttribute), true);

                                    if (testAttributes.Length > 0)
                                    {
                                        TestAttribute attribute = (TestAttribute)testAttributes[0];

                                        ConfigurationFactory factory = new ConfigurationFactory();
                                        factory.Id = attribute.Id;
                                        factory.Name = attribute.Name;
                                        factory.Path = attribute.Path;
                                        factory.Method = mi;
                                        factory.Initializer = initializer;

                                        _configurations.Add(factory);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    View.ReportError(exc.Message);
                }
            }


            View.DisplayConfigurations(_configurations);


        }

        #endregion

        #region Simulator

        private Common.Configuration.SimulatorConfiguration _currentConfig;
        private string _currentConfigId;

        public void Start(Data.ConfigurationFactory factory)
        {
            try
            {
                Common.Configuration.SimulatorConfiguration config = factory.CreateConfiguration();
                
                SimulatorStartParameters parameters = new SimulatorStartParameters();
                parameters.IPAddress = Context.Instance.ServicesEnvironment.BaseAddress;
                parameters.AuthenticationMode = Context.Instance.ServicesEnvironment.AuthenticationMode;
                parameters.Username = Context.Instance.ServicesEnvironment.Username;
                parameters.Password = Context.Instance.ServicesEnvironment.Password;

                parameters.SimulatorConfiguration = config;

                _simulator.Start(parameters);

                View.SwitchToWorkingState();
                _currentConfig = config;
                _currentConfigId = factory.Id;

                if (Started != null)
                {
                    Started();
                }
            }
            catch (Exception exc)
            {
                View.ReportError(string.Format("Unable to start services: {0}", exc.Message));
            }

        }

        public void Stop()
        {
            try
            {
                _simulator.Stop();
                View.SwitchToIdleState();
                if (Stopped != null)
                {
                    Stopped();
                }
            }
            catch (Exception exc)
            {
                View.ReportError(string.Format("Unable to stop services: {0}", exc.Message));
            }
        }

        public event SimulatorStartedEvent Started;
        
        public event SimulatorStoppedEvent Stopped;

        void _simulator_RequestProcessed(RequestProcessingLog log)
        {
            ServiceUsageInformation info = _usageInfo.Where(U => U.ServiceName == log.Service).FirstOrDefault();
            if (info != null)
            {
                OperationInfo operationInfo =
                    info.OperationsList.Where(o => o.OperationName == log.Method).FirstOrDefault();
                operationInfo.UsageInfo = OperationUsage.Passed;
                View.UpdateOperationUsage(info.ServiceName, operationInfo.OperationName, operationInfo.UsageInfo);
            }
            View.DisplayRequestProcessingLog(log);
        }

        void _simulator_SimulatorEvent(string message)
        {
            View.LogSimulatorEvent(message);
        }

        void _simulator_ResponseSent(string log)
        {
            View.LogSimulatorEvent(string.Format("Response sent{0}{0}", Environment.NewLine));
        }

        void _simulator_RequestReceived(string log)
        {
            View.LogSimulatorEvent(string.Format("Request received{0}", Environment.NewLine));
        }
        
        void _simulator_EventLogged(string log)
        {
            View.LogSimulatorEvent(log);
        }
        
        #endregion

        public bool Running
        {
            get { return _simulator.Running; }
        }

        public string CurrentConfig
        {
            get
            {
                if (_simulator.Running)
                {
                    return _currentConfigId;
                }
                else
                {
                    return string.Empty;
                }
                
            }
        }

        public Common.Configuration.SimulatorConfiguration GetConfiguration(string id)
        {
            ConfigurationFactory factory = _configurations.Where(C => C.Id == id).FirstOrDefault();
            if (factory == null)
            {
                return null;
            }
            else
            {
                return factory.CreateConfiguration();
            }
        }

    }
}
