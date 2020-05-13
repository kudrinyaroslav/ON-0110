using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TestTool.Crypto;
using TestTool.HttpTransport.Interfaces;
using TestTool.HttpTransport.Interfaces.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.BaseOnvifService;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.Utils.IBaseOnvifService
{
    public enum OnvifServiceInitializationPriority
    {
        DeviceService = 1,
    }

    public interface IBaseOnvifService
    {
        BaseOnvifTest Test { get; }
    }

    public interface IBaseOnvifService<ServicePort, ServicePortClient>: IBaseOnvifService
        where ServicePort: class
        where ServicePortClient:  ClientBase<ServicePort>, ServicePort
    {
        ServicePortClient Port { get; }
    }

    public interface IBaseOnvifService2<ServicePort, ServicePortClient>: IBaseOnvifService
        where ServicePort: class
        where ServicePortClient:  ClientBase<ServicePort>, ServicePort
    {
        OnvifServiceClient<ServicePort, ServicePortClient> ServiceClient { get; }
    }

    #region OnvifServiceClient

    public delegate string ServiceAddressRetrievalMethod(FeaturesList feature);

    public delegate void SetupSecurity(ServiceEndpoint endpoint);

    public delegate void SetupChannel(IClientChannel channel);

    public delegate T CreateClient<T>(Binding binding, EndpointAddress address);

    public class OnvifServiceClient<ServicePort, ServicePortClient>
        where ServicePort: class
        where ServicePortClient:  ClientBase<ServicePort>, ServicePort
    {
        #region properties/fields
        public ServicePortClient Port { get; set; }

        protected ServiceAddressRetrievalMethod ServiceAddressRetrievalAction;

        private SetupSecurity m_SetupSecurityAction;
        protected SetupSecurity SetupSecurityAction
        {
            get
            {
                if (null == m_SetupSecurityAction)
                    m_SetupSecurityAction = endpoint => this.Test.AttachSecurity(endpoint);

                return m_SetupSecurityAction;
            }

            set { m_SetupSecurityAction = value; }
        }

        private SetupChannel m_SetupChannelAction;
        protected SetupChannel SetupChannelAction
        {
            get
            {
                if (null == m_SetupChannelAction)
                    m_SetupChannelAction = channel => this.Test.SetupChannel(channel);

                return m_SetupChannelAction;
            }

            set { m_SetupChannelAction = value; }
        }

        private CreateClient<ServicePortClient> m_CreateClientAction;
        protected CreateClient<ServicePortClient> CreateClientAction
        {
            get
            {
                if (null == m_CreateClientAction)
                    m_CreateClientAction = (binding, address) => (ServicePortClient)Activator.CreateInstance(typeof(ServicePortClient), new object[] { binding, address });

                return m_CreateClientAction;
            }

            set { m_CreateClientAction = value; }
        }

        protected string ServiceAddress { get; set; }

        protected string ServiceName;

        protected BaseOnvifTest Test;
        #endregion

        #region Events

        public EventHandler<EventArgs> ClientClosed;
        private void OnClientClosed()
        {
            if (null != ClientClosed)
                ClientClosed(this, EventArgs.Empty);
        }

        #endregion

        public OnvifServiceClient(BaseOnvifTest test_,
                                  string serviceName_,
                                  ServiceAddressRetrievalMethod serviceAddressRetrievalAction_,
                                  SetupSecurity setupSecurityAction_,
                                  SetupChannel setupChannelAction_,
                                  CreateClient<ServicePortClient> createClientAction_)
        {
            Test = test_;
            ServiceName = serviceName_;
            ServiceAddressRetrievalAction = serviceAddressRetrievalAction_;
            SetupSecurityAction = setupSecurityAction_;
            SetupChannelAction = setupChannelAction_;
            CreateClientAction = createClientAction_;

            Test.SecurityChangedEvent        += e => this.Close();
            Test.NetworkSettingsChangedEvent += address => this.Close();
        }

        public OnvifServiceClient(BaseOnvifTest test_,
                                  string serviceName_,
                                  ServiceAddressRetrievalMethod serviceAddressRetrievalAction_): 
            this(test_, serviceName_, serviceAddressRetrievalAction_, null, null, null)
        {}

        public void Close()
        {
            if (Port != null && Port.State == CommunicationState.Opened)
                Port.Close();

            Port = null;

            OnClientClosed();
        }
        
        public void CreateClient(Binding binding, SetupSecurity securitySetup, SetupChannel channelSetup)
        {
            Port = CreateClientAction(binding, new EndpointAddress(ServiceAddress));
            securitySetup(Port.Endpoint);
            channelSetup(Port.InnerChannel);
        }

        public void InitServiceClient(IEnumerable<IChannelController> controllers, bool includeAddressController = false)
        {
            bool found = false;
            if (string.IsNullOrEmpty(ServiceAddress))
            {
                ServiceAddress = ServiceAddressRetrievalAction(Test.Features);
                Test.RunStep(() =>
                             {
                                 //Failed to retrive service's address
                                 if (string.IsNullOrEmpty(ServiceAddress))
                                 {
                                     throw new AssertException(string.Format("{0} service not found", ServiceName));
                                 }
                                 else
                                 {
                                     found = true;
                                     Test.LogStepEvent(ServiceAddress);
                                 }
                             },
                             string.Format("Get {0} service address", ServiceName), OnvifFaults.NoSuchService, true, true);
                Test.DoRequestDelay();
            }

            Test.Assert(found,
                        string.Format("{0} service address not found", ServiceName),
                        string.Format("Check that the DUT returned {0} service address", ServiceName));

            if (found)
            {
                var controller = new EndpointController(new EndpointAddress(ServiceAddress));

                var ctrls = new List<IChannelController>();
                ctrls.Add(controller);
                ctrls.AddRange(controllers);

                Binding binding = Test.CreateBinding(includeAddressController, ctrls);

                CreateClient(binding, Test.AttachSecurity, Test.SetupChannel);
            }
        }
    }

    #endregion

    public static class ServiceExtensions
    {
        public static void CheckCryptoLibary()
        {
            bool cryptoIsAvailable = CryptoUtils.CheckCryptoLibary();
            var msg = new StringBuilder();
            if (!cryptoIsAvailable)
            {
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                msg.AppendLine("ONVIF TT unable to load Bouncy Castle Crypto library.");
                msg.AppendLine("Please install it using the following instruction:");
                msg.AppendLine(string.Format(@"{0}\{1}", directory, "BouncyCastle.rtf"));
            }

            if (!cryptoIsAvailable)
                throw new AssertException(msg.ToString().TrimEnd());
        }

        public static void CheckCryptoLibary(this IBaseOnvifService s)
        {
            var msg = string.Empty;
            var cryptoIsAvailable = true;
            try
            {
                CheckCryptoLibary();
            }
            catch (AssertException e)
            {
                cryptoIsAvailable = false;
                msg = e.Message;
            }

            s.Test.Assert(cryptoIsAvailable,
                          msg,
                          "Check crypto library is available");
        }

        public static bool IsInitialized<ServicePort, ServicePortClient>(this OnvifServiceClient<ServicePort, ServicePortClient> s)
                where ServicePort : class
                where ServicePortClient : ClientBase<ServicePort>, ServicePort
        {
            return null != s && s.Port != null;
        }


        public static CapabilitiesType ExtractCapabilities<CapabilitiesType, ServicePort, ServicePortClient>(this IBaseOnvifService2<ServicePort, ServicePortClient> s, Service service, string serviceName)
            where ServicePort: class
            where ServicePortClient:  ClientBase<ServicePort>, ServicePort
        {
            s.Test.Assert(null != service.Capabilities,
                          string.Format("GetServices returned no service capabilities for {0} service", serviceName),
                          string.Format("Check service capabilities is present for {0} service", serviceName));

            CapabilitiesType capabilities;
            try
            {
                s.Test.BeginStep("Parse Capabilities element in GetServices response");

                var xRoot = new XmlRootAttribute {ElementName = "Capabilities", IsNullable = true, Namespace = service.Capabilities.NamespaceURI};

                var serializer = new XmlSerializer(typeof(CapabilitiesType), xRoot);

                var reader = new XmlNodeReader(service.Capabilities);

                capabilities = (CapabilitiesType)serializer.Deserialize(reader);
            }
            catch (Exception exc)
            {
                string message;
                if (exc.InnerException != null)
                {
                    message = string.Format("{0} {1}", exc.Message, exc.InnerException.Message);
                }
                else
                {
                    message = exc.Message;
                }
                throw new ApplicationException(message);
            }

            s.Test.StepPassed();
            return capabilities;
        }

        public static void InitServiceClient<ServicePortClient, ServicePort>(this ServiceHolder<ServicePortClient, ServicePort> s, IEnumerable<IChannelController> controllers, BaseOnvifTest test, bool includeAddressController = false)
            where ServicePort: class
            where ServicePortClient: ClientBase<ServicePort>, ServicePort
        {
            bool found = false;
            if (!s.HasAddress)
            {
                s.Retrieve(test.Features);
                test.RunStep(() =>
                             {
                                 if (!s.HasAddress)
                                 {
                                     throw new AssertException(string.Format("{0} service not found", s.ServiceName));
                                 }
                                 else
                                 {
                                     found = true;
                                     test.LogStepEvent(s.Address);
                                 }
                             },
                             string.Format("Get {0} service address", s.ServiceName), OnvifFaults.NoSuchService, true, true);
                test.DoRequestDelay();
            }

            test.Assert(found,
                        string.Format("{0} service address not found", s.ServiceName),
                        string.Format("Check that the DUT returned {0} service address", s.ServiceName));

            if (found)
            {
                var controller = new EndpointController(new EndpointAddress(s.Address));

                var ctrls = new List<IChannelController>();
                ctrls.Add(controller);
                ctrls.AddRange(controllers);

                Binding binding = test.CreateBinding(includeAddressController, ctrls);

                s.CreateClient(binding, test.AttachSecurity, test.SetupChannel);
            }
        }

        internal static IEnumerable<Type> GetImplementedOnvifServices(this IBaseOnvifService s)
        {
            var Base = typeof(IBaseOnvifService2<,>);
            return s.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == Base);
        }

        //Implement an extension method "InitializeService" for derived interface and it will be called by BaseOnvifTest.RunTest at the start of the test
        public static void GeneralInitialize(this IBaseOnvifService s)
        {
            var Current = s.GetImplementedOnvifServices();

            Func<Assembly, Type[]> typesRetriever = assembly =>
                                                    {
                                                        //If BouncyCastle.Crypto.dll is absent then GetAssemblies().SelectMany() will fail.
                                                        try
                                                        {
                                                            return assembly.GetTypes();
                                                        }
                                                        catch (Exception)
                                                        {
                                                            return new Type[0];
                                                        }
                                                    };

            var allExtensionClasses = AppDomain.CurrentDomain.GetAssemblies().SelectMany(typesRetriever).Where(T => null != T.GetCustomAttribute(typeof(ExtensionAttribute)));
            var allExtensionMethods = allExtensionClasses.SelectMany(t => t.GetMethods()).Where(m => null != m.GetCustomAttribute<ExtensionAttribute>());
            var allServiceInitializationMethods = allExtensionMethods.Where(m => "InitializeService" == m.Name);
            var currentServiceInitializationMethods = allServiceInitializationMethods.Where(m => m.GetParameters().First().ParameterType.GetInterfaces().Any(i => i.IsGenericType && Current.Any(e => e.IsAssignableFrom(i))));
            
            Func<Type, int> orderBy = type => null != type.GetCustomAttribute<OnvifServiceAttribute>() ? (int)type.GetCustomAttribute<OnvifServiceAttribute>().InitializationPriority : int.MaxValue;

            try
            {
                foreach (var method in currentServiceInitializationMethods.OrderBy(e => orderBy(e.GetParameters().First().ParameterType)))
                {
                    method.Invoke(null, new object[]{ s });
                }
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is AssertException || e.InnerException is AccessDeniedException || e.InnerException is FaultException)
                    throw e.InnerException;

                throw;
            }
        }

        //#region Update Security
        //public static void UpdateSecurity(this IBaseOnvifService s, bool updateGUI = false)
        //{
        //    s.Test.UpdateCredentials();

        //    if (updateGUI)
        //        s.Test.RaiseSecurityChangedEvent(s.Test.Credentials.Username, s.Test.Credentials.Password);
            
        //    foreach (var implementedOnvifService in s.GetImplementedOnvifServices())
        //    {
        //        if (implementedOnvifService.IsInstanceOfType(s))
        //            implementedOnvifService.GetMethod("Clear").Invoke(s, null);
        //    }

        //    //_client = CreateClient();
        //    //System.Net.ServicePointManager.Expect100Continue = false;
        //    //AttachSecurity(_client.Endpoint);
        //    //SetupChannel(_client.InnerChannel);
        //}
        //#endregion
    }
}
