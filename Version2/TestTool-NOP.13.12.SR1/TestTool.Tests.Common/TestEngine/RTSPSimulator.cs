using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.Common.TestEngine
{
    public class RTSPSimulator
    {
        [DllImport(@"RTSPSimulator.dll")]
        static extern void Start(uint codecs, int rtspPort, int httpPort);
        [DllImport(@"RTSPSimulator.dll")]
        static extern void Stop();
        [DllImport(@"RTSPSimulator.dll")]
        static extern IntPtr GetRtspUrl(int codec, string name);
        string GetRtspUrlWrapper(int codec, string name)
        {
            //The return value is marshalled as a C string, that is a pointer to a null-terminated array of characters. 
            //Because the data is coming from native to managed, and was not allocated in the managed code, 
            //the framework assumes that it is not responsible for deallocating it.
            //The native code cannot deallocate it since it is no longer executing.
            //
            //The policy therefore is that the p/invoke marshaller assumes that the character array was allocated on the shared COM heap. 
            //And so it calls CoTaskMemFree. But the array was not allocated on the shared COM heap. 
            //
            //In older versions of .net the call to CoTaskMemFree happened to fail silently. 
            //In the latest versions it is failing with an error.
            var r = GetRtspUrl(codec, name);

            return Marshal.PtrToStringAnsi(r);
        }
        [DllImport(@"RTSPSimulator.dll")]
        static extern int IsStarted();
        [DllImport(@"RTSPSimulator.dll")]
        static extern int IsRtspOverHttp();
        [DllImport(@"RTSPSimulator.dll")]
        static extern void AddSampleFile(int codec, string file, string name);

        Action _runAction;
        IAsyncResult _runResult;

        Dictionary<Codecs, Dictionary<string, string>> _streams = new Dictionary<Codecs, Dictionary<string, string>>();
        Dictionary<Codecs, Dictionary<string, string>> _rtspUrls = new Dictionary<Codecs, Dictionary<string, string>>();
        Action<int> _waitSeverStarted = null;
        int _timeout = 0;
        string _hostIP = null;
        int _rtspPort = 8554;
        int _httpPort = 8000;

        public enum Codecs
        {
            JPEG = 0,
            MPEG4 = 1,
            H264 = 2,
            G711 = 3,
            G726 = 4,
            AAC = 5
        }

        public RTSPSimulator(string hostIP, Action<int> waitSeverStarted, int timeout)
        {
            _waitSeverStarted = waitSeverStarted;
            _timeout = timeout;
            _hostIP = hostIP;
        }

        public bool StartRTSP()
        {
            StopRTSP();
            _rtspUrls.Clear();
            int useCodecs = 0;
            foreach (Codecs codec in _streams.Keys)
            {
                useCodecs |= (1 << (int)codec);
                foreach (string name in _streams[codec].Keys)
                {
                    AddSampleFile((int)codec, _streams[codec][name], name);
                }
            }
            if (useCodecs != 0)
            {
                _runAction = () => Start((uint)useCodecs, _rtspPort, _httpPort);
                _runResult = _runAction.BeginInvoke(null, null);
                while (IsStarted() != 1)
                {
                    _waitSeverStarted(10);
                }
                foreach (Codecs codec in _streams.Keys)
                {
                    foreach (string name in _streams[codec].Keys)
                    {
                        string url = GetRtspUrlWrapper((int)codec, name);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string ip = url.Substring(url.IndexOf("://") + 3);
                            ip = ip.Substring(0, ip.IndexOfAny(new char[] { ':', '/' }));
                            url = url.Replace(ip, _hostIP);
                        }
                        if (!_rtspUrls.ContainsKey(codec))
                        {
                            _rtspUrls[codec] = new Dictionary<string, string>();
                        }
                        _rtspUrls[codec][name] = url;
                    }
                }
                return true;
            }
            return false;
        }

        public void StopRTSP()
        {
            Stop();
            if (_runResult != null)
            {
                if (!_runResult.IsCompleted)
                {
                    _runResult.AsyncWaitHandle.WaitOne(_timeout);
                }

                if (null != _runAction)
                {
                    _runAction.EndInvoke(_runResult);
                    _runAction = null;
                }
                _runResult.AsyncWaitHandle.Close();
            }
        }

        public string GetUrl(Codecs codec, string name)
        {
            if (_rtspUrls.ContainsKey(codec))
            {
                if (_rtspUrls[codec].ContainsKey(name))
                {
                    return _rtspUrls[codec][name];
                }
            }
            return String.Empty;
        }

        public string[] GetUrls(Codecs codec)
        {
            List<string> urls = new List<string>();
            if (_rtspUrls.ContainsKey(codec))
            {
                foreach (string url in _rtspUrls[codec].Values)
                {
                    urls.Add(url);
                }
            }
            return urls.ToArray();
        }


        public void Add(Codecs codec, string file, string name)
        {
            if (!_streams.ContainsKey(codec))
            {
                _streams[codec] = new Dictionary<string, string>();
            }
            _streams[codec][name] = file;
        }
    }
}
