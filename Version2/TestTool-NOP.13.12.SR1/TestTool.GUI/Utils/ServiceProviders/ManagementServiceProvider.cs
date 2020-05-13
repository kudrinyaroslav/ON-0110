///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Proxies.Onvif;
using System.ServiceModel.Channels;
using System.ServiceModel;
using DateTime = TestTool.Proxies.Onvif.DateTime;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Onvif;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Wrapper for Device service proxy
    /// </summary>
    class ManagementServiceProvider : BaseServiceProvider<DeviceClient, Device>
    {
        public ManagementServiceProvider(string serviceAddress, int messageTimeout)
            : base(serviceAddress, messageTimeout)
        {
            EnableLogResponse = true;
        }
        
        /// <summary>
        /// Definition for OnDeviceInformationReceived event.
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="firmwareVersion"></param>
        /// <param name="serial"></param>
        /// <param name="hardwareId"></param>
        public delegate void DeviceInformationReceived(string manufacturer, string model, string firmwareVersion, string serial, string hardwareId);
        
        /// <summary>
        /// Is raised when device information is got.
        /// </summary>
        public event DeviceInformationReceived OnDeviceInformationReceived;

        /// <summary>
        /// Creates proxy-class.
        /// </summary>
        /// <param name="binding">Binding information.</param>
        /// <param name="address">Service address.</param>
        /// <returns></returns>
        public override DeviceClient CreateClient(Binding binding, EndpointAddress address)
        {
            return new DeviceClient(binding, address);
        }

        /// <summary>
        /// Starts getting device information.
        /// </summary>
        public void GetDeviceInformation()
        {
            RunInBackground(GetDeviceInformationInternal);
        }

        public void GetDeviceInformationEx()
        {

            Action action = ConstructSecurityTolerantAction(
                () => { GetDeviceInformationInternal(); });
            
            RunInBackground(action);
        }

        void GetDeviceInformationInternal()
        {
            string model;
            string manufacturer;
            string hardwareId;
            string serial;
            string firmwareVersion;
            manufacturer = Client.GetDeviceInformation(out model,
                                                        out firmwareVersion,
                                                        out serial,
                                                        out hardwareId);

            if (OnDeviceInformationReceived != null)
            {
                OnDeviceInformationReceived(manufacturer, model, firmwareVersion, serial, hardwareId);
            }

        }

        /// <summary>
        /// Starts getting hostname.
        /// </summary>
        public void GetHostname()
        {
            RunInBackground(new Action(() => { Client.GetHostname(); })); 
        }
        
        /// <summary>
        /// Starts getting interfaces.
        /// </summary>
        public void GetInterfaces()
        {
            RunInBackground(new Action(() => { Client.GetNetworkInterfaces(); }));
        }

        /// <summary>
        /// Starts getting scopes.
        /// </summary>
        public void GetScopes()
        {
            RunInBackground(new Action(() => { Client.GetScopes(); }));
        }

        /// <summary>
        /// Starts synchronizing time.
        /// </summary>
        public void SyncTime()
        {
            RunInBackground( () =>
                                 {
                                     SystemDateTime dt = Client.GetSystemDateAndTime();

                                     System.DateTime now = System.DateTime.Now.ToUniversalTime();
                                     DateTime deviceTime = new DateTime();
                                     deviceTime.Date = new Date();
                                     deviceTime.Time = new Time();

                                     deviceTime.Date.Year = now.Year;
                                     deviceTime.Date.Month = now.Month;
                                     deviceTime.Date.Day = now.Day;
                                     deviceTime.Time.Hour = now.Hour;
                                     deviceTime.Time.Minute = now.Minute;
                                     deviceTime.Time.Second = now.Second;

                                     Client.SetSystemDateAndTime(SetDateTimeType.Manual, dt.DaylightSavings,
                                                                 null, deviceTime);
                                 });
        }

        /// <summary>
        /// Starts setting factiry defaults.
        /// </summary>
        public void HardReset()
        {
            RunInBackground(() => 
            {
                Client.SetSystemFactoryDefault(FactoryDefaultType.Hard);
            } );
        }

        /// <summary>
        /// Starts deice rebooting
        /// </summary>
        public void Reboot()
        {
            RunInBackground(new Action(() => { Client.SystemReboot(); }));
        }

        /// <summary>
        /// Starts settings IP address
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="address">Address</param>
        /// <param name="prefixLength">Prefix length</param>
        public void SetIPAddress(string token, string address, int prefixLength)
        {
            RunInBackground(new Action(() => { SetIPInternal(token, address, prefixLength); }));
        }

        void SetIPInternal(string token, string ip, int prefix)
        {
            EnableLogResponse = false;
            NetworkInterface[] interfaces;
            try
            {
                interfaces = Client.GetNetworkInterfaces();
            }
            catch (Exception exc)
            {
                ReportException(exc, "An exception was thrown during getting network interface: ");
                ReportOperationCompleted();
                return;
            }
            finally
            {
                EnableLogResponse = true;
            }

            foreach (NetworkInterface networkInterface in interfaces)
            {
                if (networkInterface.token == token)
                {
                    PrefixedIPv4Address address = new PrefixedIPv4Address();
                    address.Address = ip;
                    address.PrefixLength = prefix;

                    NetworkInterfaceSetConfiguration nisc = new NetworkInterfaceSetConfiguration();
                    nisc.IPv4 = new IPv4NetworkInterfaceSetConfiguration();

                    nisc.IPv4.DHCP = false;
                    nisc.IPv4.DHCPSpecified = false;
                    nisc.IPv4.Enabled = true;
                    nisc.IPv4.EnabledSpecified = true;
                    
                    nisc.IPv4.Manual = new PrefixedIPv4Address[1];
                    nisc.IPv4.Manual[0] = address;

                    nisc.MTU = networkInterface.Info.MTU;
                    nisc.MTUSpecified = true;

                    nisc.Link = networkInterface.Link.AdminSettings;

                    nisc.Enabled = true;
                    nisc.EnabledSpecified = true;

                    Client.SetNetworkInterfaces(token, nisc);

                    return;
                }
            }

            throw new Exception(string.Format("Network interface matching Token \"{0}\" not found", token));

        }

        /// <summary>
        /// Starts setting default gateway
        /// </summary>
        /// <param name="gateway"></param>
        public void SetGateway(string gateway)
        {
            RunInBackground(() =>
                    {
                        System.Net.IPAddress address = System.Net.IPAddress.Parse(gateway);

                        string[] ipv4Address = null;
                        string[] ipv6Address = null;
                        if (address.GetAddressBytes().Length == 4)
                        {
                            ipv4Address = new string[] { gateway };
                        }
                        else
                        {
                            ipv6Address = new string[] { gateway };
                        }
                        Client.SetNetworkDefaultGateway(ipv4Address, ipv6Address);
                        
                    });

        }

        public delegate void CapabilitiesReceived(Capabilities capabilities);

        public event CapabilitiesReceived OnCapabilitiesReceived;

        public delegate void ServicesInfoReceived(Capabilities capabilities, Service[] services);

        public event ServicesInfoReceived OnServicesInfoReceived;

        /// <summary>
        /// STarts getting capabilities.
        /// </summary>
        /// <param name="capabilities">Capability categories to be got.</param>
        public void GetCapabilities(CapabilityCategory[] capabilities)
        {
            RunInBackground(() => {
                Capabilities res = Client.GetCapabilities(capabilities); 
                if(OnCapabilitiesReceived != null)
                {
                    OnCapabilitiesReceived(res);
                }
            });
        }

        public void GetServiceAddresses(bool capabilitiesStyle, CapabilityCategory[] categories)
        {
            RunInBackground(() =>
            {
                Capabilities capabilities = null;
                Service[] services = null;

                if (capabilitiesStyle)
                {
                    capabilities = Client.GetCapabilities(categories);
                }
                else
                {
                    services = Client.GetServices(false);
                }
                if (OnServicesInfoReceived != null)
                {
                    OnServicesInfoReceived(capabilities, services);
                }
            });
        }

        /// <summary>
        /// Gets services
        /// </summary>
        public void GetServices()
        {
            RunInBackground(() => { Client.GetServices(true); });
        }

        public DeviceServicesInfo GetCapabilitiesDefineSecurity(CapabilityCategory[] categories)
        {
            DeviceServicesInfo info = new DeviceServicesInfo();
            Capabilities capabilities = null;
            Service[] services = null;

            bool getServicesNotSupported = false;

            Action action = ConstructSecurityTolerantAction(
                () =>
                    {
                        if (!getServicesNotSupported)
                        {
                            System.Diagnostics.Debug.WriteLine("---------  GetServices");
                            try
                            {
                                services = Client.GetServices(false);
                                return;
                            }
                            catch (FaultException exc)
                            {
                                if (exc.IsValidOnvifFault(OnvifFaults.NotAuthorized)||
                                    exc.IsValidOnvifFault(OnvifFaults.SenderNotAuthorized))
                                {
                                    throw exc;
                                }
                                else
                                {
                                    if (exc.IsValidOnvifFault(OnvifFaults.NoSuchService))
                                    {
                                        System.Diagnostics.Debug.WriteLine("---------  GetServices not supported");
                                        getServicesNotSupported = true;
                                    }
                                }
                            }
                        }

                        try
                        {
                            if (services == null)
                            {
                                System.Diagnostics.Debug.WriteLine("---------  GetCapabilities");
                                capabilities = Client.GetCapabilities(categories);
                            }
                        }
                        catch (FaultException ex)
                        {
                            // An error "unable to get service address" will be shown.
                            if (ex.IsValidOnvifFault(OnvifFaults.NotAuthorized) ||
                                ex.IsValidOnvifFault(OnvifFaults.SenderNotAuthorized))
                            {
                                throw ex;
                            }
                        }
                    });
            action();
            info.Capabilities = capabilities;
            info.Services = services;
            return info;
        }
    }

    public class DeviceServicesInfo
    {
        public Capabilities Capabilities { get; set; }
        public Service[] Services { get; set; }
    }
}
