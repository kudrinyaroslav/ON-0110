﻿using System;
using System.Threading;
using TestTool.Proxies.Onvif;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace TestTool.GUI.Utils
{
    class PTZServiceProvider : BaseServiceProvider<PTZClient, PTZ>
    {
        public delegate void PTZConfigurationReceived(PTZConfiguration[] configs);
        public event PTZConfigurationReceived OnPTZConfigurationsReceived;

        public PTZServiceProvider(string serviceAddress, int messageTimeout) : 
            base(serviceAddress, messageTimeout)
        {
            EnableLogResponse = true;
        }
        public override PTZClient CreateClient(Binding binding, EndpointAddress address)
        {
            return new PTZClient(binding, address);
        }
        public void AbsoluteMove(string profile, PTZVector position)
        {
            RunInBackground(() =>
            {
                Client.AbsoluteMove(profile, position, null);
            });
        }
        public void RelativeMove(string profile, PTZVector translation)
        {
            RunInBackground(() =>
            {
                Client.RelativeMove(profile, translation, null);
            });
        }
        public void AbosuteRelativeIncrementalMove(
            bool absolute,
            string profile, 
            decimal xmin, 
            decimal xmax, 
            decimal ymin,
            decimal ymax,
            decimal zmin,
            decimal zmax)
        {
            RunInBackground(() =>
            {
                PTZVector vector = new PTZVector();
                vector.PanTilt = new Vector2D();
                vector.PanTilt.space = absolute ? 
                    "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace" :
                    "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace";
                vector.Zoom = new Vector1D();
                vector.Zoom.space = absolute ? 
                    "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace" : 
                    "http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace";

                for (decimal x = xmin; x <= xmax; x += (decimal)0.2)
                {
                    for (decimal y = ymin; y <= ymax; y += (decimal)0.2)
                    {
                        for (decimal z = zmin; z <= zmax; z += (decimal)0.2)
                        {
                            vector.PanTilt.x = (float)x;
                            vector.PanTilt.y = (float)y;
                            vector.Zoom.x = (float)z;
                            if (absolute)
                            {
                                Client.AbsoluteMove(profile, vector, null);
                            }
                            else
                            {
                                Client.RelativeMove(profile, vector, null);
                            }
                            Thread.Sleep(300);
                        }
                    }
                }
            });
        }
        public void GetConfigurations()
        {
            RunInBackground(() =>
            {
                PTZConfiguration[] configs = Client.GetConfigurations();
                if (OnPTZConfigurationsReceived != null)
                {
                    OnPTZConfigurationsReceived(configs);
                }
            });
        }
        public void ContinuousMove(string profile, PTZSpeed speed, string timeout)
        {
            RunInBackground(() =>
            {
                Client.ContinuousMove(profile, speed, timeout);
            });
        }
        public void Stop(string profile, bool panTilt, bool zoom)
        {
            RunInBackground(() =>
            {
                Client.Stop(profile, panTilt, zoom);
            });
        }
        public PTZNode[] GetNodes()
        {
            PTZNode[] nodes = null;
            if (Security == HttpTransport.Interfaces.Security.None)
            {
                Action action = ConstructSecurityTolerantAction(
                    () =>
                        {
                            nodes = Client.GetNodes();
                        }
                    );
                action();
            }
            else
            {
                nodes = Client.GetNodes();
            }
            return nodes;
        }
    }
}
