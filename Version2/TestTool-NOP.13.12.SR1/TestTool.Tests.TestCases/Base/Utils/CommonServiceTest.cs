using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.Base
{
    /// <summary>
    /// Test with Device client
    /// </summary>
    /// <typeparam name="TContract"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public abstract class CommonServiceTest<TContract, TClient> : BaseServiceTest<TContract, TClient>
        where TContract : class
        where TClient : ClientBase<TContract>, new()
    {
                
        /// <summary>
        /// Creates CommonServiceTest instance.
        /// </summary>
        /// <param name="param">Parameters for test run.</param>
        protected CommonServiceTest(TestLaunchParam param)
            : base(param)
        {
            
        }

        protected override TClient CreateClient()
        {
            string address = GetServiceAddress();

            BeginStep(string.Format("Connect to {0} service", ServiceName));
            LogStepEvent(string.Format("{0} service address: {1}", ServiceName, address));
            if (string.IsNullOrEmpty(address))
            {
                throw new AssertException(string.Format("{0} service not supported", ServiceName));
            }
            else
            {
                if (!address.IsValidUrl())
                {
                    throw new AssertException(string.Format("{0} service address is invalid", ServiceName));
                }
            }

            TClient client = CreateClient(address);
            StepPassed();
            return client;
        }


        private DeviceClient _deviceClient;

        protected DeviceClient DeviceClient
        {
            get
            {
                if (_deviceClient == null)
                {
                    Binding binding =
                        CreateBinding(false,
                        new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient =
                        new DeviceClient(binding, new EndpointAddress(_cameraAddress));

                    AttachSecurity(_deviceClient.Endpoint);
                    SetupChannel(_deviceClient.InnerChannel);

                }

                return _deviceClient;
            }
        }

        protected override void Release()
        {
            if (_deviceClient != null)
            {
                if (_deviceClient.State == CommunicationState.Opened)
                {
                    _deviceClient.Close();
                }
            }
            base.Release();
        }

        #region service-specific

        protected abstract string ServiceName { get; }

        protected abstract string GetServiceAddress();

        protected abstract TClient CreateClient(string address);

        #endregion

        protected Service[] GetServices(bool includeCapabilities)
        {
            return CommonMethodsProvider.GetServices(this, DeviceClient, includeCapabilities);
        }


        protected T ExtractCapabilities<T>(XmlElement element, string ns)
        {
            BeginStep("Parse Capabilities element in GetServices response");

            System.Xml.Serialization.XmlRootAttribute xRoot = new System.Xml.Serialization.XmlRootAttribute();
            xRoot.ElementName = "Capabilities";
            xRoot.IsNullable = true;
            xRoot.Namespace = ns;

            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);

            XmlReader reader = new XmlNodeReader(element);

            T capabilities;
            try
            {
                capabilities = (T)serializer.Deserialize(reader);
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
            StepPassed();
            return capabilities;
        }


    }
}
