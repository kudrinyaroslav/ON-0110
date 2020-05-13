#define GETSERVICES 
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.Base
{
    static class DeviceExtension
    {
        public static string GetServiceAddress(this DeviceClient device, string serviceNs)
        {
            string address = string.Empty;

            Service[] services = device.GetServices(false);
            Service service = services.FindService(serviceNs);
            if (service != null)
            {
                address = service.XAddr;
            }
            return address;
        }

        public static string GetServiceAddress(this DeviceClient device,
            CapabilityCategory category,
            Func<Capabilities, string> addressSelector)
        {
            string address = string.Empty;
            Capabilities capabilities = device.GetCapabilities(new CapabilityCategory[] { category });
            return addressSelector(capabilities);
        }

        public static string GetServiceAddress(this DeviceClient device, 
                                               FeaturesList features,
                                               CapabilityCategory category,
                                               Func<Capabilities, string> addressSelector, 
                                               string serviceNs)
        {
            string address = string.Empty;

            if (features.Contains(Feature.GetServices))
                address = device.GetServiceAddress(serviceNs);
            else
                address = device.GetServiceAddress(category, addressSelector);

            return address;
        }
        

        public static string GetMediaServiceAddress(this DeviceClient device,
                                                    FeaturesList features)
        {
            return device.GetServiceAddress(features,
                                            CapabilityCategory.Media,
                                            (capabilities) =>
                                            {
                                                if (capabilities.Media != null)
                                                {
                                                    return capabilities.Media.XAddr;
                                                }
                                                else
                                                {
                                                    return string.Empty;
                                                }
                                            },
                                            OnvifService.MEDIA);
        }



        public static string GetPtzServiceAddress(this DeviceClient device,
            FeaturesList features)
        {
            return device.GetServiceAddress(
                features,
                CapabilityCategory.PTZ,
                (capabilities) =>
                {
                    if (capabilities.PTZ != null)
                    {
                        return capabilities.PTZ.XAddr;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }, OnvifService.PTZ);
        }

 
        public static string GetEventServiceAddress(this DeviceClient device,
                                                    FeaturesList features)
        {
            return device.GetServiceAddress(features,
                                            CapabilityCategory.Events,
                                            (capabilities) =>
                                            {
                                                if (capabilities.Events != null)
                                                {
                                                    return capabilities.Events.XAddr;
                                                }
                                                else
                                                {
                                                    return string.Empty;
                                                }
                                            }, 
                                            OnvifService.EVENTS);
        }
        
        public static string GetImagingServiceAddress(this DeviceClient device,
            FeaturesList features)
        {
            return device.GetServiceAddress(
                features,
                CapabilityCategory.Imaging,
                (capabilities) =>
                {
                    if (capabilities.Imaging != null)
                    {
                        return capabilities.Imaging.XAddr;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }, OnvifService.IMAGING);
        }
        
        public static string GetIoServiceAddress(this DeviceClient device,
            FeaturesList features)
        {
            return device.GetServiceAddress(
                features,
                CapabilityCategory.All,
                (capabilities) =>
                {
                    if (capabilities.Extension != null)
                    {
                        if (capabilities.Extension.DeviceIO != null)
                        {
                            return capabilities.Extension.DeviceIO.XAddr;
                        }
                    }
                    return string.Empty;
                }, OnvifService.IO);
        }
        
        

        public static string GetDoorControlServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(OnvifService.DOORCONTROL );
        }

        public static string GetUsersServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(OnvifService.USERSERVICE);
        }

        public static string GetAccessControlServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(OnvifService.ACCESSCONTROL);
        }


        public static string GetSearchServiceAddress(this DeviceClient device,
            FeaturesList features)
        {
            return device.GetServiceAddress(features,
                CapabilityCategory.All,
                (capabilities) =>
                {
                    if (capabilities.Extension != null)
                    {
                        if (capabilities.Extension.Search != null)
                        {
                            return capabilities.Extension.Search.XAddr;
                        }
                    }
                    return string.Empty;
                }, OnvifService.SEARCH);
        }
        
        public static string GetReplayServiceAddress(this DeviceClient device,
            FeaturesList features)
        {
            return device.GetServiceAddress(features,
                CapabilityCategory.All,
                (capabilities) =>
                {
                    if (capabilities.Extension != null)
                    {
                        if (capabilities.Extension.Replay != null)
                        {
                            return capabilities.Extension.Replay.XAddr;
                        }
                    }
                    return string.Empty;
                }, OnvifService.REPLAY);
        }


        public static string GetRecordingsServiceAddress(this DeviceClient device,
                FeaturesList features)
        {
            return device.GetServiceAddress(features,
                CapabilityCategory.All,
                (capabilities) =>
                {
                    if (capabilities.Extension != null)
                    {
                        if (capabilities.Extension.Recording != null)
                        {
                            return capabilities.Extension.Recording.XAddr;
                        }
                    }
                    return string.Empty;
                }, OnvifService.RECORIDING);
        }

        public static string GetReceiverServiceAddress(this DeviceClient device,
        FeaturesList features)
        {
            return device.GetServiceAddress(features,
                CapabilityCategory.All,
                (capabilities) =>
                {
                    if (capabilities.Extension != null)
                    {
                        if (capabilities.Extension.Receiver != null)
                        {
                            return capabilities.Extension.Receiver.XAddr;
                        }
                    }
                    return string.Empty;
                }, OnvifService.RECEIVER);
        }

    }
}
