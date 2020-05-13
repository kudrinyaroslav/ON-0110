using System;
using System.Collections.Generic;
using System.Linq;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Onvif;

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
            IEnumerable<Feature> features,
            CapabilityCategory category,
            Func<Capabilities, string> addressSelector, 
            string serviceNs)
        {
            string address = string.Empty;
            if (features.Contains(Definitions.Enums.Feature.GetServices))
            {
                address = device.GetServiceAddress(serviceNs);
            }
            else
            {
                address = device.GetServiceAddress(category, addressSelector);
            }
            return address;
        }

        /*
        public static string GetMediaServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(
                features,
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
                    }, Common.Definitions.OnvifService.MEDIA);
        }

        public static string GetPtzServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
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
                }, Common.Definitions.OnvifService.PTZ);
        }

        public static string GetEventServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(
                features,
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
                }, Common.Definitions.OnvifService.EVENTS);
        }

        public static string GetImagingServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
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
                }, Common.Definitions.OnvifService.IMAGING);
        }

        public static string GetIoServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
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
                }, Common.Definitions.OnvifService.IO);
        }
        */


        public static string GetMediaServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(
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
                });
        }

        public static string GetPtzServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(
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
                });
        }

        public static string GetEventServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(
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
                });
        }

        public static string GetImagingServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(
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
                });
        }

        public static string GetIoServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
        {
            return device.GetServiceAddress(
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
                });
        }

        public static string GetSearchServiceAddress(this DeviceClient device,
            IEnumerable<Feature> features)
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
        IEnumerable<Feature> features)
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
                IEnumerable<Feature> features)
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


    }
}
