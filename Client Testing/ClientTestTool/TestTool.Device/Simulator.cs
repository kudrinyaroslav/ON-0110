using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Linq;
using System.ServiceModel.Security;
using TestTool.Device.Data;
using TestTool.Services;
using TestTool.Services.Services;
using TestTool.Transport;

namespace TestTool.Device
{
    public delegate void RequestProcessedEvent(RequestProcessingLog log);
    public delegate void SimulatorEvent(string message);

    public delegate void SimulatorStartedEvent();
    public delegate void SimulatorStoppedEvent();
    
    public class Simulator
    {
        public Simulator()
        {
            _logger = new Logger();
            _logger.EventLogged += new MessageEvent(_logger_EventLogged);

            InitializeServices();

            _requestsHandlingManager = new RequestsHandlingManager(_logger, _services.Values);

            _requestsHandlingManager.RequestProcessed += Service_RequestProcessed;
            _requestsHandlingManager.RequestReceived += new TrafficEvent(_logger_RequestReceived);
            _requestsHandlingManager.ResponseSent += new TrafficEvent(_logger_ResponseSent);
            _requestsHandlingManager.MethodStarted += new ServiceEvent(Service_MethodStarted);
            _requestsHandlingManager.MethodCompleted += new ServiceEvent(Service_MethodCompleted);

        }



        ServiceHost _deviceServiceHost;
        private DeviceService _deviceService;

        ServiceHost _accessControlServiceHost;
        private AccessControlService _accessControlService;

        ServiceHost _doorControlServiceHost;
        private DoorControlService _doorControlService;

        private bool _running;

        private Dictionary<string, BaseService> _services;

        private Logger _logger;
        private RequestsHandlingManager _requestsHandlingManager;

        private List<RequestProcessingLog> _totalLog;

        #region events

        public event RequestProcessedEvent RequestProcessed;
        public event TrafficEvent RequestReceived;
        public event TrafficEvent ResponseSent;
        public event SimulatorEvent SimulatorEvent;
        public event MessageEvent EventLogged;

        public event SimulatorStartedEvent Started;
        public event SimulatorStoppedEvent Stopped;

        #endregion
        
        public List<ServiceContractInfo> GetServicesInfo()
        {
            List<Data.ServiceContractInfo> infos = new List<ServiceContractInfo>();
            foreach (BaseService service in _services.Values)
            {
                ServiceContractInfo info = new ServiceContractInfo() {ServiceName = service.GetServiceName()};
                info.OperationsList.AddRange(service.GetOperationsList().OrderBy(s=>s));
                infos.Add(info); 
            }

            return infos;
        }

        public void Start(SimulatorStartParameters parameters)
        {
            _totalLog = new List<RequestProcessingLog>();
            
            ServiceConfiguration configuration = new ServiceConfiguration();
            configuration.BaseAddress = parameters.IPAddress;
            
            CustomBinding custombindingSoap12 = ServiceBinding();
            
            // update simulator configuration
            BaseService.UpdateConfiguration(parameters.SimulatorConfiguration);

            _deviceService.UpdateParameters(configuration);
            _deviceServiceHost = new ServiceHost(_deviceService, new Uri(parameters.IPAddress + _deviceService.GetLocalAddress()));
            _deviceServiceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
            ServiceEndpoint deviceEndpoint = _deviceServiceHost.AddServiceEndpoint(typeof(Onvif.Device), custombindingSoap12, String.Empty);

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            behavior.HttpGetUrl = deviceEndpoint.ListenUri;
            behavior.MetadataExporter = new WsdlExporter();
            _deviceServiceHost.Description.Behaviors.Add(behavior);
            //_deviceServiceHost.AddServiceEndpoint(typeof(IMetadataExchange),
            //                                      MetadataExchangeBindings.CreateMexHttpBinding(), "/mex/");

            
            _doorControlService.UpdateParameters(configuration);
            _doorControlServiceHost = new ServiceHost(_doorControlService, new Uri(parameters.IPAddress + _doorControlService.GetLocalAddress()));
            _doorControlServiceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
            ServiceEndpoint doorEndpoint = _doorControlServiceHost.AddServiceEndpoint(typeof(Onvif.DoorControlPort), custombindingSoap12, String.Empty);

            _accessControlService.UpdateParameters(configuration);
            _accessControlServiceHost = new ServiceHost(_accessControlService, new Uri(parameters.IPAddress + _accessControlService.GetLocalAddress()));
            _accessControlServiceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
            ServiceEndpoint accessControlEndpoint = _accessControlServiceHost.AddServiceEndpoint(typeof(Onvif.PACSPort), custombindingSoap12, String.Empty);
            

            if (parameters.AuthenticationMode == AuthenticationMode.WS)
            {
                Transport.Security.UsersList.Current.AddUser(parameters.Username, parameters.Password);

                Transport.Security.SecurityBehavior security = new Transport.Security.SecurityBehavior(_logger);

                deviceEndpoint.Behaviors.Add(security );
                doorEndpoint.Behaviors.Add(security);
                accessControlEndpoint.Behaviors.Add(security);
            }

            _deviceServiceHost.Open();
            _doorControlServiceHost.Open();
            _accessControlServiceHost.Open();

            _running = true;
            if (Started != null)
            {
                Started();
            }

        }

        public void Stop()
        {
            _running = false;
            _deviceServiceHost.Close();
            _doorControlServiceHost.Close();
            _accessControlServiceHost.Close();

            if (Stopped != null)
            {
                Stopped();
            }
        }
        
        public bool Running
        {
            get { return _running; }
        }

        void InitializeServices()
        {
            _services = new Dictionary<string, BaseService>();
            
            EngineParameters parameters= new EngineParameters();
            parameters.Logger = _logger;

            _deviceService = new DeviceService();
            _deviceService.AttachLogger(_logger);
            _services.Add(Common.Definitions.OnvifService.DEVICE, _deviceService);
            
            _doorControlService = new DoorControlService();
            _doorControlService.AttachLogger(_logger);
            _services.Add(Common.Definitions.OnvifService.DOORCONTROL, _doorControlService);
            
            _accessControlService = new AccessControlService();
            _accessControlService.AttachLogger(_logger);
            _services.Add(Common.Definitions.OnvifService.ACCESSCONTROL, _accessControlService);

        }

        private CustomBinding _binding;
        CustomBinding ServiceBinding()
        {
            if (_binding == null)
            {
                _binding = new CustomBinding();
            
                MessageVersion version = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);

                CustomTextMessageBindingElement ctmbe = new CustomTextMessageBindingElement("utf-8", "application/soap+xml", version);
                ctmbe.AddController(_logger);
                _binding.Elements.Add(ctmbe);

                HttpTransportBindingElement htbe = new HttpTransportBindingElement();
                //htbe.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;
                _binding.Elements.Add(htbe);

            }

            return _binding;
        }
        
        #region Events from services

        void Service_MethodCompleted(BaseService service, string message)
        {
            if (SimulatorEvent != null)
            {
                SimulatorEvent(string.Format("Processing {0} request completed{1}", message, Environment.NewLine));
            }
        }

        void Service_MethodStarted(BaseService service, string message)
        {
            if (SimulatorEvent != null)
            {
                SimulatorEvent(string.Format("{0} service is processing {1} request{2}", 
                    service.GetServiceName(), message, Environment.NewLine));
            }
        }

        void Service_RequestProcessed(RequestProcessingLog log)
        {
            _totalLog.Add(log);

            if (RequestProcessed != null)
            {
                RequestProcessed(log);
            }
        }
        
        void _logger_ResponseSent(string log)
        {
            if (ResponseSent != null)
            {
                ResponseSent(log);
            }
        }

        void _logger_RequestReceived(string log)
        {
            if (RequestReceived != null)
            {
                RequestReceived(log);
            }
        }
        
        void _logger_EventLogged(string log)
        {
            if (EventLogged != null)
            {
                EventLogged(log);
            }
        }

        #endregion

    }

}
