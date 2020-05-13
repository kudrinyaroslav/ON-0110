using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using TestTool.Onvif;

namespace TestTool.Device
{
    public delegate void RequestProcessedEvent(RequestProcessingLog log);
    public delegate void SimulatorEvent(string message);

    public delegate void SimulatorStartedEvent();
    public delegate void SimulatorStoppedEvent();

    public delegate void NetworkEvent(NetworkEventData parameters);

    public class Simulator
    {
        public Simulator()
        {
        }

        #region events

        public event NetworkEvent DataTransmitted;

        public event SimulatorEvent SimulatorEvent;
        public event SimulatorStartedEvent Started;
        public event SimulatorStoppedEvent Stopped;
        public event SimulatorEvent StartFailed;

        #endregion

        private List<ServiceProxy> _proxies;

        public void Start(SimulatorStartParameters parameters)
        {
            System.Threading.Thread thread = new Thread(new ParameterizedThreadStart(StartProxy));
            thread.Start(parameters);
            //StartProxy(parameters);
        }

        public void Stop()
        {
            foreach (ServiceProxy proxy in _proxies)
            {
                proxy.Stop();
            }
            
            if (Stopped != null)
            {
                Stopped();
            }
        }

        void StartProxy(object startParameters)
        {
            try
            {
                SimulatorStartParameters parameters = (SimulatorStartParameters)startParameters;
                InitializeProxies(parameters);

                foreach (ServiceProxy proxy in _proxies)
                {
                    proxy.Start();
                }
                if (Started != null)
                {
                    Started();
                }
            }
            catch (Exception exc)
            {
                foreach (ServiceProxy proxy in _proxies)
                {
                    if (proxy.Started)
                    {
                        proxy.Stop();
                    }
                }
                if (StartFailed != null)
                {
                    StartFailed(exc.Message);
                }
            }

        }

        void Proxy_DataTransmitted(NetworkEventData parameters)
        {
            if (DataTransmitted != null)
            {
                DataTransmitted(parameters);
            }
        }

        void InitializeProxies(SimulatorStartParameters parameters)
        {
            _proxies = new List<ServiceProxy>();

            string deviceAddress = parameters.IPAddress + "onvif/device_service/";
            DeviceServiceProxy deviceProxy = new DeviceServiceProxy(deviceAddress,
                                                        parameters.DeviceAddress);
            deviceProxy.DataTransmitted += new NetworkEvent(Proxy_DataTransmitted);
            _proxies.Add(deviceProxy);

            //
            // initialize other proxies
            //

            CustomBinding custombindingSoap12 = new CustomBinding();
            custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8));
            custombindingSoap12.Elements.Add(new HttpTransportBindingElement());

            Dictionary<string, string> replacements = new Dictionary<string, string>();
            replacements.Add(OnvifService.DEVICE, deviceAddress);

            DeviceClient deviceClient = new DeviceClient(custombindingSoap12, new EndpointAddress(parameters.DeviceAddress));
            
            Service[] services;
            try
            {
                services = deviceClient.GetServices(false);
            }
            catch (FaultException exc)
            {
                deviceClient.Close();
                deviceClient = new DeviceClient(custombindingSoap12, new EndpointAddress(parameters.DeviceAddress));
                deviceClient.Endpoint.Behaviors.Add(new Transport.SecurityBehavior()
                                                        {UserName = parameters.Username, Password = parameters.Password});
                services = deviceClient.GetServices(false);
            }
            deviceClient.Close();

            foreach (Service service in services)
            {
                string localAddress = string.Empty;
                switch (service.Namespace)
                {
                    case OnvifService.ACCESSCONTROL:
                        localAddress = "onvif/access_control/";
                        break;
                    case OnvifService.DOORCONTROL:
                        localAddress = "onvif/door_control/";
                        break;
                }

                if (!string.IsNullOrEmpty(localAddress))
                {
                    string address = parameters.IPAddress + localAddress;
                    ServiceProxy proxy = new ServiceProxy(address, service.XAddr);
                    proxy.DataTransmitted += new NetworkEvent(Proxy_DataTransmitted);
                    _proxies.Add(proxy);
                    replacements.Add(service.Namespace, address);
                }
            }

            deviceProxy.SetReplacements(replacements);
        }
    }
}