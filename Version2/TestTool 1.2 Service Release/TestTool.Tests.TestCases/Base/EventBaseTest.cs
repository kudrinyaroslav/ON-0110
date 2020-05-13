///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.SoapValidation;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Event;
using System.ServiceModel.Description;
using TestTool.HttpTransport;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.TestCases.Utils.SoapValidation;

namespace TestTool.Tests.TestCases.Base
{

    /// <summary>
    /// Base class for Events test classes.
    /// Contains methods for manipulating service clients (we need several for this group of tests).
    /// </summary>
    public abstract class EventBaseTest : BaseMultiInterfacesTest
    {
        protected EventBaseTest(TestLaunchParam param)
            : base(param)
        {
            
        }

        /// <summary>
        /// Event service address.
        /// </summary>
        protected string _eventServiceAddress = null;


        /// <summary>
        /// Initializes (and returns) event service address (using GetCapabilities method).
        /// </summary>
        /// <returns></returns>
        protected string GetEventServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                HttpTransport.HttpBinding binding = 
                    (HttpTransport.HttpBinding)CreateBinding(true, 
                    new IChannelController[]{new SoapValidator(DeviceManagementSchemasSet.GetInstance())});

                DeviceClient device = new DeviceClient(binding, new EndpointAddress(_cameraAddress));
                AddSecurityBehaviour(device.Endpoint);
                Capabilities capabilities = device.GetCapabilities(new CapabilityCategory[] { CapabilityCategory.Events });
                DoRequestDelay();

                if (capabilities.Events != null)
                {
                    address = capabilities.Events.XAddr;
                }
                if (string.IsNullOrEmpty(address))
                {
                    throw new DutPropertiesException("Event capabilities not found");
                }
                Uri uri;
                if(!Uri.TryCreate(address, UriKind.Absolute, out uri))
                {
                    throw new AssertException(string.Format("Event service address [{0}] is invalid", address));
                }

            }, "Get Event service address");
            _eventServiceAddress = address;
            return address;
        }
    
        /// <summary>
        /// Checks that service address is already initialized. If no, initializes.
        /// </summary>
        void EnsureServiceAddressNotEmpty()
        {
            if (string.IsNullOrEmpty(_eventServiceAddress))
            {
                GetEventServiceAddress();
            }
        }

        #region services

        /// <summary>
        /// SubscriptionManager client (providing Renew and unsubscribe methods).
        /// </summary>
        protected SubscriptionManagerClient _subscriptionManagerClient;

        /// <summary>
        /// EventPortType client (providing GetEventProperties and CreatePullPointSubscription methods)
        /// </summary>
        protected EventPortTypeClient _eventPortTypeClient;

        /// <summary>
        /// NotificationProducer client (providing Subscribe method).
        /// </summary>
        protected NotificationProducerClient _notificationProducerClient;

        /// <summary>
        /// PullPointSubscription client (providing PullMessages and SetSynchronizationPoint methods).
        /// </summary>
        protected PullPointSubscriptionClient _pullPointSubscriptionClient;

        #endregion

        /// <summary>
        /// Creates EventPortType client (using endpoint address got via GetCapabiliites)
        /// </summary>
        /// <returns></returns>
        EventPortTypeClient CreateEventPortTypeClient()
        {
            EnsureServiceAddressNotEmpty();

            HttpBinding binding = CreateEventServiceBinding(_eventServiceAddress);

            _eventPortTypeClient = new EventPortTypeClient(binding, new EndpointAddress(_eventServiceAddress));

            System.Net.ServicePointManager.Expect100Continue = false;

            AddSecurityBehaviour(_eventPortTypeClient.Endpoint);

            SetupTimeout(_eventPortTypeClient.InnerChannel);
            
            return _eventPortTypeClient;
        }

        /// <summary>
        /// Creates NotificationProducer client (using endpoint address got via GetCapabiliites)
        /// </summary>
        /// <returns></returns>
        NotificationProducerClient CreateNotificationProducerClient()
        {
            EnsureServiceAddressNotEmpty();

            HttpBinding binding = CreateEventServiceBinding(_eventServiceAddress);

            _notificationProducerClient = new NotificationProducerClient(binding, new EndpointAddress(_eventServiceAddress));

            System.Net.ServicePointManager.Expect100Continue = false;
            AddSecurityBehaviour(_notificationProducerClient.Endpoint);

            SetupTimeout(_notificationProducerClient.InnerChannel);
            
            return _notificationProducerClient;
        }

        /// <summary>
        /// Creates SubscriptionManager client using address passed.
        /// </summary>
        /// <param name="endpointAddress">Service address.</param>
        /// <returns></returns>
        protected SubscriptionManagerClient CreateSubscriptionManagerClient(EndpointReferenceType endpointReference)
        {
            HttpBinding binding = CreateEventServiceBinding(endpointReference.Address.Value);

            _subscriptionManagerClient = new SubscriptionManagerClient(binding, new EndpointAddress(endpointReference.Address.Value));

            System.Net.ServicePointManager.Expect100Continue = false;

            AddSecurityBehaviour(_subscriptionManagerClient.Endpoint);
            AttachAddressing(_subscriptionManagerClient.Endpoint, endpointReference);

            SetupTimeout(_subscriptionManagerClient.InnerChannel);
            
            return _subscriptionManagerClient;
        }

        /// <summary>
        /// Creates PullPointSubscription client using address passed.
        /// </summary>
        /// <param name="address">Service address.</param>
        /// <returns></returns>
        protected PullPointSubscriptionClient CreatePullPointSubscriptionClient(EndpointReferenceType endpointReference)
        {
            HttpBinding binding = CreateEventServiceBinding(endpointReference.Address.Value);

            _pullPointSubscriptionClient = new PullPointSubscriptionClient(binding, new EndpointAddress(endpointReference.Address.Value));
            
            AddSecurityBehaviour(_pullPointSubscriptionClient.Endpoint);
            AttachAddressing(_pullPointSubscriptionClient.Endpoint, endpointReference);

            SetupTimeout(_pullPointSubscriptionClient.InnerChannel);
            
            return _pullPointSubscriptionClient;
        }   

        /// <summary>
        /// Checks if NotificationProducer client is created. Creates NotificationProducer client,
        /// if necessary. 
        /// </summary>
        protected void EnsureNotificationProducerClientCreated()
        {
            if (_notificationProducerClient == null)
            {
                CreateNotificationProducerClient();
            }
        }

        /// <summary>
        /// Checks if EventPortType client is created. Creates NotificationProducer client,
        /// if necessary. 
        /// </summary>
        protected void EnsureEventPortTypeClientCreated()
        {
            if (_eventPortTypeClient == null)
            {
                CreateEventPortTypeClient();
            }
        }
     
        /// <summary>
        /// Adds security behaviour to the proxy-class. 
        /// </summary>
        /// <param name="endpoint"></param>
        protected void AddSecurityBehaviour(ServiceEndpoint endpoint)
        {
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
            {
                SecurityBehavior securityBehavior = new SecurityBehavior();
                securityBehavior.UserName = _username;
                securityBehavior.Password = _password;
                securityBehavior.UseUTCTimestamp = _useUTCTimestamp;
                endpoint.Behaviors.Add(securityBehavior);
            }
        }

        /// <summary>
        /// Sets up timeouts.
        /// </summary>
        /// <param name="channel"></param>
        void SetupTimeout(IClientChannel channel)
        {
            channel.OperationTimeout = new TimeSpan(0, 0, 0, 0, _messageTimeout);
        }

        /// <summary>
        /// Closes all open connections.
        /// </summary>
        protected override void CloseConnections()
        {
            if (_eventPortTypeClient != null)
            {
                _eventPortTypeClient.Close();
            }

            if (_notificationProducerClient != null)
            {
                _notificationProducerClient.Close();
            }

            if (_subscriptionManagerClient != null)
            {
                _subscriptionManagerClient.Close();
            }

            if (_pullPointSubscriptionClient != null)
            {
                _pullPointSubscriptionClient.Close();
            }

        }

        private HttpBinding CreateEventServiceBinding(string address)
        {
            IChannelController[] controllers;
            // add mandatory controllers.
            // _trafficListener is used to monitor data sent and received via Client.
            // _semaphore is used to stop waiting for the answer.
            
            EndpointController controller = new EndpointController(new EndpointAddress(address));
            controller.WsaEnabled = true;

            controllers = new IChannelController[] { _trafficListener, _semaphore, _credentialsProvider, controller, new SoapValidator(EventsSchemasSet.GetInstance()) };

            HttpBinding binding = new HttpBinding(controllers);
            
            return binding;

        }

        protected void AttachAddressing(System.ServiceModel.Description.ServiceEndpoint endpoint,
            TestTool.Proxies.Event.EndpointReferenceType endpointReference)
        {
            if (endpointReference.ReferenceParameters != null && endpointReference.ReferenceParameters.Any != null)
            {
                Utils.EndpointReferenceBehaviour behaviour = new EndpointReferenceBehaviour(endpointReference);
                endpoint.Behaviors.Add(behaviour);
            }

        }

    }
}
