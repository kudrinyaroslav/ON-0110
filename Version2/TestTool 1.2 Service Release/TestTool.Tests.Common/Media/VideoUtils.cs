using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.TestEngine;

namespace TestTool.Tests.Common.Media
{
    public class VideoUtils
    {
        /*
        public static void AdjustVideo(
            IVideoForm form, 
            string username,
            string password,
            int messageTimeout,
            TransportProtocol protocol,
            StreamType streamType,
            MediaUri streamUri,
            VideoEncoderConfiguration conf)
        {
            if (!string.IsNullOrEmpty(username))
            {
                form.User = username;
            }
            else
            {
                form.User = null;
            }
            if (!string.IsNullOrEmpty(password))
            {
                form.Password = password;
            }
            else
            {
                form.Password = null;
            }

            string uri = (streamUri != null) ? streamUri.Uri : null;
            //uri = "http://[195.145.107.77]:90/rtsp_tunnel?h26x=0&line=1&enableaudio=1";
            switch (protocol)
            {
                case TransportProtocol.UDP:
                    form.HTTPPort = 0;
                    form.TCP = false;
                    break;
                case TransportProtocol.TCP:
                    form.HTTPPort = 0;
                    form.TCP = true;
                    break;
                case TransportProtocol.RTSP:
                    form.HTTPPort = 0;
                    form.TCP = true;
                    break;
                case TransportProtocol.HTTP:
                    {
                        form.HTTPPort = 80;
                        try
                        {
                            int pos = uri.IndexOf(']', 8);
                            if (pos > 0) // IPv6
                            {
                                pos++;
                            }
                            else
                            {
                                pos = uri.IndexOf(':', 8);
                            }
                            if (uri.Substring(pos, 1) == ":") // port specified
                            {
                                pos++;
                                int pos2 = uri.IndexOf('/', pos);
                                pos = int.Parse(uri.Substring(pos, pos2 - pos));
                                if (pos > 0) form.HTTPPort = pos;
                            }
                        }
                        catch (System.Exception)
                        {

                        }
                        form.TCP = true;
                    }
                    break;
            };

            form.Multicast = streamType == StreamType.RTPMulticast;
            form.Address = uri;
            form.RTSP = !string.IsNullOrEmpty(uri);
            form.Timeout = messageTimeout;
            if ((conf != null) && (conf.Resolution != null))
            {
                form.VideoWidth = conf.Resolution.Width;
                form.VideoHeight = conf.Resolution.Height;
            }
        }
        */

        public static void AdjustVideo(
            IVideoForm form,
            string username,
            string password,
            int messageTimeout,
            TransportProtocol protocol,
            StreamType streamType,
            MediaUri streamUri,
            VideoEncoderConfiguration conf)
        {
            if (!string.IsNullOrEmpty(username))
            {
                form.User = username;
            }
            else
            {
                form.User = null;
            }
            if (!string.IsNullOrEmpty(password))
            {
                form.Password = password;
            }
            else
            {
                form.Password = null;
            }

            string uri = (streamUri != null) ? streamUri.Uri : null;
            //uri = "http://[195.145.107.77]:90/rtsp_tunnel?h26x=0&line=1&enableaudio=1";
            switch (protocol)
            {
                case TransportProtocol.UDP:
                    form.HTTPPort = 0;
                    form.TCP = false;
                    break;
                case TransportProtocol.TCP:
                    form.HTTPPort = 0;
                    form.TCP = true;
                    break;
                case TransportProtocol.RTSP:
                    form.HTTPPort = 0;
                    form.TCP = true;
                    break;
                case TransportProtocol.HTTP:
                    {
                        form.HTTPPort = 80;
                        try
                        {
                            int pos = uri.IndexOf(']', 8);
                            if (pos > 0) // IPv6
                            {
                                pos++;
                            }
                            else
                            {
                                pos = uri.IndexOf(':', 8);
                            }
                            if (uri.Substring(pos, 1) == ":") // port specified
                            {
                                pos++;
                                int pos2 = uri.IndexOf('/', pos);
                                pos = int.Parse(uri.Substring(pos, pos2 - pos));
                                if (pos > 0) form.HTTPPort = pos;
                            }
                        }
                        catch (System.Exception)
                        {

                        }
                        form.TCP = true;
                    }
                    break;
            };

            form.Multicast = streamType == StreamType.RTPMulticast;
            form.Address = uri;
            form.RTSP = !string.IsNullOrEmpty(uri);
            form.Timeout = messageTimeout;
            if ((conf != null) && (conf.Resolution != null))
            {
                form.VideoWidth = conf.Resolution.Width;
                form.VideoHeight = conf.Resolution.Height;
            }
        }
    

    
    
    }
}
