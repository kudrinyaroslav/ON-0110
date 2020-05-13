using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Interfaces;
using System.Threading;
using TestTool.Tests.Common.TestEngine;
using System.Collections.Generic;
using System;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.Common.Media
{
    public delegate StreamSetup GetStreamSetup(ref Profile prof);

    public class VideoUtils
    {
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



        class VideoStreamForm : IVideoFormEvent
        {
            public Profile profile = null;
            public StreamSetup streamSetup = null;
            public MediaUri streamUri = null;
            public string username;
            public string password;
            public int messageTimeout;
            public WaitHandle eventOpened = new AutoResetEvent(false);
            public WaitHandle eventWorkEnded = new ManualResetEvent(true);
            public Thread thread = new Thread(VideoStreamForm.VW);
            public Exception exception = null;
            public WaitHandle signalCloseWindow = null;
            public IVideoFormEvent eventConsumer = null;
            public int NICIndex = 0;
            public bool skipLog = false;
            public List<string> skippedLog = new List<string>();
            public Func<StreamSetup, string, MediaUri> getStreamUri = null;

            #region IVideoFormEvent

            public void FireBeginStep(string Name)
            {
                string str = "[Profile: " + profile.Name + "] " + Name;
                if (!skipLog) lock (eventConsumer)
                {
                    eventConsumer.FireBeginStep(str);
                }
                else
                {
                    skippedLog.Add(str);
                }
            }
            public void FireStepPassed()
            {
                if (!skipLog) lock (eventConsumer)
                {
                    eventConsumer.FireStepPassed();
                }
                else
                {
                    skippedLog.Add("PASSED");
                }
            }
            public void FireLogStepEvent(string Message)
            {
                if (!skipLog) lock (eventConsumer)
                {
                    eventConsumer.FireLogStepEvent(Message);
                }
                else
                {
                    skippedLog.Add(Message);
                }
            }

            #endregion

            public static void VW(object param)
            {
                VideoStreamForm vsf = (VideoStreamForm)param;

                try
                {
                    lock (vsf.eventConsumer)
                    {
                        vsf.streamUri = vsf.getStreamUri(vsf.streamSetup, vsf.profile.token);
                    }
                }
                catch (Exception ex)
                {
                    vsf.exception = ex;
                }

                if (vsf.exception == null)
                {
                    IVideoForm videoForm = new VideoContainer();
                    videoForm.NICIndex = vsf.NICIndex;
                    VideoUtils.AdjustVideo(
                        videoForm, vsf.username, vsf.password, vsf.messageTimeout, vsf.streamSetup.Transport.Protocol,
                        vsf.streamSetup.Stream, vsf.streamUri, vsf.profile.VideoEncoderConfiguration);
                    bool VideoIsOpened = false;
                    try
                    {
                        VideoIsOpened = true;
                        videoForm.EventSink = vsf;
                        videoForm.OpenWindow(false);

                        ((AutoResetEvent)vsf.eventOpened).Set();
                        vsf.signalCloseWindow.WaitOne();

                        videoForm.CloseWindow();
                        VideoIsOpened = false;
                    }
                    catch (Exception ex)
                    {
                        vsf.exception = ex;
                    }
                    finally
                    {
                        if (VideoIsOpened)
                        {
                            videoForm.CloseWindow();
                        }
                        videoForm = null;
                    }
                }
                ((ManualResetEvent)vsf.eventWorkEnded).Set();
                ((AutoResetEvent)vsf.signalCloseWindow).Set();
            } 
        }

        public static void ShowMultiple(
            IVideoFormEvent eventConsumer,
            WaitHandle stopEvent, 
            string username, 
            string password, 
            int timeout, 
            GetStreamSetup getStreamSetup,
            int NICIndex,
            List<Profile> profiles, 
            Func<StreamSetup, string, MediaUri> getStreamUri,
            Action<Action, string, bool> runStep,
            Action<Exception> stepFailed)
        {
            List<VideoStreamForm> streams = new List<VideoStreamForm>();
            List<WaitHandle> eventsWorkEnded = new List<WaitHandle>();

            WaitHandle signalCloseWindow = new AutoResetEvent(false);
            foreach (Profile profile in profiles)
            {
                Profile currentProfile = profile;
                StreamSetup streamSetup = getStreamSetup(ref currentProfile);
                if (currentProfile == null)
                    continue;

                VideoStreamForm vsf = new VideoStreamForm();
                vsf.streamSetup = streamSetup;
                vsf.profile = currentProfile;
                vsf.getStreamUri = getStreamUri;
                vsf.messageTimeout = timeout;
                vsf.username = username;
                vsf.password = password;
                vsf.signalCloseWindow = signalCloseWindow;
                vsf.eventConsumer = eventConsumer;
                vsf.NICIndex = NICIndex;
                streams.Add(vsf);
                eventsWorkEnded.Add(vsf.eventWorkEnded);
            }

            if (0 == streams.Count)
                return;

            bool timeoutException = false;

            try
            {
                VideoStreamForm vsfFailed = null;
                foreach (VideoStreamForm vsf in streams)
                {
                    ((ManualResetEvent)vsf.eventWorkEnded).Reset();
                    vsf.thread.Start(vsf);
                    if (0 == WaitHandle.WaitAny(new WaitHandle[] { stopEvent, vsf.eventOpened, vsf.eventWorkEnded }))
                    {
                        throw new StopEventException();
                    }
                    if (vsf.exception != null)
                    {
                        stepFailed(vsf.exception);
                        vsfFailed = vsf;
                        break;
                    }
                }

                foreach (VideoStreamForm vsf in streams)
                {
                    if (vsf.thread.ThreadState != ThreadState.Unstarted && vsf != vsfFailed)
                    {
                        vsf.skipLog = true;
                    }
                }

                ((AutoResetEvent)signalCloseWindow).Set();

                runStep(() =>
                {
                    if (!WaitHandle.WaitAll(eventsWorkEnded.ToArray(), timeout))
                    {
                        ((AutoResetEvent)signalCloseWindow).Reset();
                        timeoutException = true;
                        throw new VideoException("Waiting timeout exceeded");
                    }
                }, "Closing streams", false);

                if (timeoutException)
                {
                    throw new VideoException("Waiting timeout exceeded");
                }
            }
            // handle halt
            catch (Exception ex)
            {
                runStep(() =>
                {
                    foreach (VideoStreamForm vsf in streams)
                    {
                        if (vsf.thread.ThreadState != ThreadState.Unstarted)
                        {
                            vsf.thread.Abort();
                            vsf.thread.Join();
                        }
                    }
                }, "Cleanup", true);
                if (!timeoutException)
                {
                    throw ex;
                }
            }
            runStep(() =>
            {
                // check for exceptions
                if (timeoutException)
                {
                    throw new VideoException("Closing streams failed!");
                }
                foreach (VideoStreamForm vsf in streams)
                {
                    vsf.thread.Join();
                    if (vsf.exception != null)
                    {
                        throw new VideoException("Profile " + vsf.profile.Name + " failed!");
                    }
                }
            }, "Check for test results", true);
        }

    }
}
