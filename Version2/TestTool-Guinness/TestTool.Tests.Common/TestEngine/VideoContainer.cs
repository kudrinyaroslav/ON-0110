///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Exceptions;
using DS = QuartzTypeLib;

using ONVIFRTSPFilter;

namespace TestTool.Tests.Common.TestEngine
{
    public class VideoContainer : IVideoForm
    {
        #region Constants and Variables

        #region Window Constants
        // consts to wrapper window
        private const int WS_CHILD          = 0x40000000;
        private const int WS_CLIPCHILDREN   = 0x02000000;
        // consts to AM vindow
        private const int WS_MAXIMIZEBOX    = 0x00010000;
        private const int WS_MINIMIZEBOX    = 0x00020000;
        private const int WS_THICKFRAME     = 0x00040000;
        private const int WS_SYSMENU        = 0x00080000;
        private const int WS_DLGFRAME       = 0x00400000;
        private const int WS_BORDER         = 0x00800000;

        private const int WS_NORWALWINDOW   = WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
        private const int WS_TOOLWINDOW     = WS_DLGFRAME;

        private const int WS_EX_TOOLWINDOW  = 0x00000080;
        private const int WS_EX_APPWINDOW   = 0x00040000;
        #endregion

        #region Filter Variables
        private DS.FilgraphManager FilterGraph = null;
        private DS.IVideoWindow     VideoWindow     = null;
        private DS.IMediaControl    MediaControl    = null;
        private ITestControl        TestControl     = null;
        #endregion

        #region Interface backend
        private IVideoFormEvent EventConsumer = null;
        private string omsdFileName = null;
        private bool FileValid = false;
        private string user = null;
        private string password = null;
        private string address = null;
        private int httpport = 0;
        private bool tcp = false;
        private bool multicast = false;
        private bool proceedOptions = false;
        private bool proceedAudio = false;
        private int AssumeWidth = 0;
        private int AssumeHeight = 0;
        private int AssumeTimeout = 0;
        private bool proceedParameter = false;
        private bool proceedMetadata = false;
        private bool proceedSyncPoint = false;
        private bool proceedVideo = true;

        private bool   rtsp                  = true;
        private int    videoFPS              = 0;
        private string videoCodecName        = null;
        private string audioCodecName        = null;
        private string multicastAddress      = null;
        private int    multicastRtpPortVideo = 0;
        private int    multicastRtpPortAudio = 0;
        private int    multicastTTL          = 0;

        private bool   keepAliveOptions      = false;

        private string customSetupFields     = null;
        private string customPlayFields      = null;
        private string customPauseFields     = null;
        private bool   doSetupOnReplay       = true;
        private bool   replayMode            = false;
        private int    replayMaxDuration     = 0;
        private int    replayPauseWait       = 0;
        private bool   replayReverse         = false;

        #endregion

        #region Filter Constansts
        private const int step_Constructed        = 0;
        private const int step_InitEnvironment    = 1;
        private const int step_OPTIONS            = 2;
        private const int step_CheckOptions       = 3;
        private const int step_DESCRIBE           = 4;
        private const int step_OpenStream         = 5;
        private const int step_SETUP              = 6;
        private const int step_PLAY               = 7;
        private const int step_WaitStream         = 8;
        private const int step_PAUSE              = 9;
        private const int step_StopThread         =10;
        private const int step_TEARDOWN           =11;
        private const int step_HaltEnvironment    =12;
        private const int step_End                =13;
        private string[] StepNames = {
            "Constructed",
            "Init Environment",
            "OPTIONS",
            "Check Options",
            "DESCRIBE",
            "Open Stream",
            "SETUP",
            "PLAY",
            "Wait Stream",
            "PAUSE",
            "Stop Thread",
            "TEARDOWN",
            "Halt Environment"
                                     };
        #endregion

        private string[] activeFilters = null;

        #endregion

        #region Interface

        public IVideoFormEvent EventSink
        { 
            set { EventConsumer = value; }
        }
        public int VideoWidth
        {
            get { return AssumeWidth; }
            set { AssumeWidth = value; FileValid = false; }
        }
        public int VideoHeight
        {
            get { return AssumeHeight; }
            set { AssumeHeight = value; FileValid = false; }
        }
        public int Timeout
        {
            get { return AssumeTimeout; }
            set { 
                AssumeTimeout = value; 
                FileValid = false;
                if (AssumeTimeout <   2000) AssumeTimeout =   2000;
                if (AssumeTimeout > 600000) AssumeTimeout = 600000;
            }
        }

        public string Address
        {
            get { return address; }
            set { address = value; FileValid = false; }
        }
        public string User
        {
            get { return user; }
            set { user = value; FileValid = false; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; FileValid = false; }
        }
        public int HTTPPort
        {
            get { return httpport; }
            set { httpport = value; FileValid = false; }
        }
        public bool TCP
        {
            get { return tcp; }
            set { tcp = value; FileValid = false; }
        }
        public bool Multicast
        {
            get { return multicast; }
            set { multicast = value; FileValid = false; }
        }
        public bool UseVideo
        {
            get { return proceedVideo; }
            set { proceedVideo = value; FileValid = false; }
        }
        public bool OPTIONS
        {
            get { return proceedOptions; }
            set { proceedOptions = value; FileValid = false; }
        }
        public bool KEEPALIVE
        {
            get { return proceedParameter; }
            set { proceedParameter = value; FileValid = false; }
        }
        public bool UseKeepAliveOptions
        {
            get { return keepAliveOptions; }
            set { keepAliveOptions = value; FileValid = false; }
        }
        public bool UseAudio
        {
            get { return proceedAudio; }
            set { proceedAudio = value; FileValid = false; }
        }
        public bool EVENTS
        {
            get { return proceedMetadata; }
            set { proceedMetadata = value; FileValid = false; }
        }

        public bool RTSP
        {
            get { return rtsp; }
            set { rtsp = value; FileValid = false; }
        }
        public int VideoFPS
        {
            get { return videoFPS; }
            set { videoFPS = value; FileValid = false; }
        }
        public string VideoCodecName
        {
            get { return videoCodecName; }
            set { videoCodecName = value; FileValid = false; }
        }
        public string AudioCodecName
        {
            get { return audioCodecName; }
            set { audioCodecName = value; FileValid = false; }
        }
        public string MulticastAddress
        {
            get { return multicastAddress; }
            set { multicastAddress = value; FileValid = false; }
        }
        public int MulticastRtpPortVideo
        {
            get { return multicastRtpPortVideo; }
            set { multicastRtpPortVideo = value; FileValid = false; }
        }
        public int MulticastRtpPortAudio
        {
            get { return multicastRtpPortAudio; }
            set { multicastRtpPortAudio = value; FileValid = false; }
        }
        public int MulticastTTL
        {
            get { return multicastTTL; }
            set { multicastTTL = value; FileValid = false; }
        }

        public string CustomSetupFields
        {
            get { return customSetupFields; }
            set { customSetupFields = value; FileValid = false; }
        }
        public string CustomPlayFields
        {
            get { return customPlayFields; }
            set { customPlayFields = value; FileValid = false; }
        }
        public string CustomPauseFields
        {
            get { return customPauseFields; }
            set { customPauseFields = value; FileValid = false; }
        }
        public bool DoSetupOnReplay
        {
            get { return doSetupOnReplay; }
            set { doSetupOnReplay = value; FileValid = false; }
        }
        public bool ReplayMode
        {
            get { return replayMode; }
            set { replayMode = value; FileValid = false; }
        }
        public int ReplayMaxDuration
        {
            get { return replayMaxDuration; }
            set { replayMaxDuration = value; FileValid = false; }
        }
        public int ReplayPauseWait
        {
            get { return replayPauseWait; }
            set { replayPauseWait = value; FileValid = false; }
        }
        public bool ReplayReverse
        {
            get { return replayReverse; }
            set { replayReverse = value; FileValid = false; }
        }    

        public string GetEvents()
        {
            string st = null;
            try
            {
                TestControl.GetEvents(ref st);
            }
            catch (Exception)
            {

            }
            return st;
        }

        public bool SYNC
        {
            get { return proceedSyncPoint; }
            set { proceedSyncPoint = value; FileValid = false; }
        }
        public bool WaitForStableKey(ref double Length)
        {
            try
            {
                TestControl.WaitForStableKey(ref Length);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool WaitForSync(ref int Length)
        {
            try
            {
                TestControl.WaitForSync(ref Length);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void OpenWindow(bool WithAudio, bool WithVideo)
        {
            UseAudio = WithAudio;
            UseVideo = WithVideo;
            Connect();
            SetupWindow();
            Run();
        }
        public void OpenWindow(bool WithAudio) 
        {
            OpenWindow(WithAudio, true);
        }
        public void CloseWindow() 
        {
            Stop();
            Clear();
        }
        public void Replay(Action<Action, Action, Action> actionControl)
        {
            UseAudio = false;
            UseVideo = true;
            Connect();
            SetupWindow();

            actionControl(() => { Play(); }, () => { Pause(); }, () => { Teardown(); }); 
        }
        #endregion

        #region External Ini

    private class IniFileClass
	{
		private String IniFile = "";

        // Import methods from system DLLs
		[DllImport("kernel32")] private static extern long WritePrivateProfileInt(String Section, String Key, int Value, String FilePath);
		[DllImport("kernel32")] private static extern long WritePrivateProfileString(String Section, String Key, String Value, String FilePath);
		[DllImport("kernel32")] private static extern int GetPrivateProfileInt(String Section, String Key, int Default, String FilePath);
		[DllImport("kernel32")] private static extern int GetPrivateProfileString(String Section, String Key, String Default, StringBuilder retVal, int Size, String FilePath);

		public IniFileClass(String IniFile)
		{
            // Save the path to the INI file
			this.IniFile = IniFile;
		}

		public String ReadString(String Section, String Key, String Default)
		{
            // Create the StringBuilder to store strings
			StringBuilder stringBuilder = new StringBuilder(255);
            // Try to read the required parameter
            GetPrivateProfileString(Section, Key, Default, stringBuilder, 255, IniFile);
            // Return the parameter read
            return stringBuilder.ToString();
		}

		public int ReadInt(String Section, String Key, int Default)
		{
            // Returns the value of the parameter read
			return GetPrivateProfileInt(Section, Key, Default, IniFile); 
		}

		public void WriteString(String Section, String Key, String Value)
		{
            // Unless the value of the parameter set
			WritePrivateProfileString(Section, Key, Value, IniFile);
		}

		public void WriteInt(String Section, String Key, int Value)
		{
            // Unless the value of the parameter set
			WritePrivateProfileInt(Section, Key, Value, IniFile);
		}
	}
    // class for reading status and log messages from SourceFilter
    private class OMSDIO : IniFileClass
    {
        public OMSDIO(String IniFile) : base(IniFile) {}
        private const string ReplyID = "Test State";

        public int GetCurrentStep()
        {
            return ReadInt(ReplyID, "CurrentStep", 0);
        }
        public int GetPassBits()
        {
            return ReadInt(ReplyID, "PassBits", 0);
        }
        public int GetFailBits()
        {
            return ReadInt(ReplyID, "FailBits", 0);
        }
        public int GetMessageCount()
        {
            return ReadInt(ReplyID, "MessageCount", 0);
        }
        public int GetMessageOfs(int Step)
        {
            return ReadInt(ReplyID, string.Format("Step{0:d03}", Step), 0);
        }
        public string GetMessage(int Num)
        {
            return ReadString(ReplyID, string.Format("Log{0:d03}", Num), "");
        }
    }

        #endregion

        #region Log Parsing

        private bool InStep = false;
        bool RollActivityPart(ref OMSDIO ini, ref int MessID, ref int LogStep, int ToStep)
        {
            if (LogStep > ToStep) return false;

            if (MessID <= ini.GetMessageCount())
            {
                string s = ini.GetMessage(MessID);
                if (!string.IsNullOrEmpty(s))
                {
                    EventConsumer.FireLogStepEvent(s);
                }
                MessID++;
                int MC = ini.GetMessageOfs(LogStep + 1);
                if ((MC > 0) && (MessID >= MC))
                {
                    if ((ini.GetFailBits() & (1 << LogStep)) == 0)
                    //if ((ini.GetPassBits() & (1 << LogStep)) != 0)
                    {
                        EventConsumer.FireStepPassed();
                        InStep = false;
                    }
                    else
                    {
                        string ErrorLine;
                        GetErrorDescription(out ErrorLine);
                        throw new VideoException(ErrorLine);
                    }

                    do 
                    {   // bypassing steps
                        LogStep++;
                        MC = ini.GetMessageOfs(LogStep);
                        System.Threading.Thread.Sleep(1);
                    }
                    while ((MC > 0) && (MessID >= MC));
                    LogStep--;

                    if (LogStep <= ToStep)
                    {
                        EventConsumer.FireBeginStep(StepNames[LogStep]);
                        InStep = true;
                    }
                }
                return true;
            }
            else
            {
                System.Threading.Thread.Sleep(100);
                return false;
            }
        }

        public bool LogActivity(int Timeout, int FromStep, int ToStep, IAsyncResult Result)
        {
            InStep = false;
            DateTime Till = DateTime.Now.AddSeconds(Timeout / 1000);
            OMSDIO ini = new OMSDIO(omsdFileName);
            while ((ini.GetCurrentStep() < FromStep) && (DateTime.Now < Till))
            {
                System.Threading.Thread.Sleep(100);
            };

            EventConsumer.FireBeginStep(StepNames[FromStep]);
            InStep = true;

            int LogStep = FromStep;
            int MessID = ini.GetMessageOfs(LogStep);
            while ((!Result.IsCompleted) && (DateTime.Now < Till))
            {
                RollActivityPart(ref ini, ref MessID, ref LogStep, ToStep);
                System.Threading.Thread.Sleep(1);
            }
            if (Result.IsCompleted && (ToStep <= ini.GetCurrentStep()))
            {
                while (LogStep <= ToStep)
                {
                    if (!RollActivityPart(ref ini, ref MessID, ref LogStep, ToStep)) break;
                    System.Threading.Thread.Sleep(1);
                }
            }
            return LogStep >= ToStep;
        }


        private bool GetErrorDescription(out string Error)
        {
            Error = "Internal error";
            OMSDIO ini = new OMSDIO(FileName);
            int FailStep = ini.GetCurrentStep();
            if (FailStep != 0)
            {
                int FailBits = ini.GetFailBits();
                for (FailStep = 0; ((FailBits & (1 << FailStep)) == 0) && (FailStep < step_End); FailStep++) { };
                if (FailStep >= step_End) return false;
            }
            switch (FailStep)
            {
                case step_Constructed:
                    Error = "RTSP filter not found, please check your installation";
                    break;
                case step_InitEnvironment:
                    Error = "RTSP filter internal error, initialization failed";
                    break;
                case step_OPTIONS:
                    Error = "OPTIONS command error, please check your connection";
                    break;
                case step_CheckOptions:
                    Error = "OPTIONS validation error";//, OPTIONS = [" + ini.GetMessage(ini.GetMessageOfs(4) - 2) + "]";
                    break;
                case step_DESCRIBE:
                    Error = "DESCRIBE command error, please check your connection";
                    break;
                case step_OpenStream:
                    Error = "No known media sources found";
                    break;
                case step_SETUP:
                    Error = "SETUP command error";
                    break;
                case step_PLAY:
                    Error = "PLAY command error";
                    break;
                case step_WaitStream:
                    Error = "No media frames within timeout, please check your connection";
                    break;
                case step_PAUSE:
                    Error = "PAUSE command error";
                    break;
                case step_StopThread:
                    Error = "RTSP filter internal error - race conditions";
                    break;
                case step_TEARDOWN:
                    Error = "TEARDOWN command error";
                    break;
                case step_HaltEnvironment:
                    Error = "RTSP filter internal error - finalization failed";
                    break;
                    //default :
                    //Error = "ddd";
                    //break;
            }
            return true;
        }

        #region Graph Processing
        // fill list of directshow filters in FilterGraph
        void ScanGraph()
        {
            TestControl = null;
            if (FilterGraph == null) return;

            DS.IAMCollection coll = FilterGraph.FilterCollection as DS.IAMCollection;
            if (coll == null) return;

            int c = coll.Count;
            if (c <= 0)
            {
                activeFilters = null;
                return;
            }
            object ppUnk = null;

            activeFilters = new string[c - 1];
            int j = 0;
            for (int i = c - 1; i >= 0; i--)
            {
                coll.Item(i, out ppUnk);
                DS.IFilterInfo Info = ppUnk as DS.IFilterInfo;
                if ((Info.Filter as ITestControl) != null)
                {
                    TestControl = Info.Filter as ITestControl;
                    continue;
                }
                activeFilters[j] = Info.Name; j++;
            }
        }

        #region Graph Processing Filters
        private enum CodecType
        {
            Source,
            VideoDecoder,
            AudioDecoder,
            VideoRenderer,
            AudioRenderer,
            VideoProcessor,
            AudioProcessor
        }
        private enum CodecState
        {
            Recommended,
            Known,
            Unrecommended,
        }

        private class CodecData
        {
            public CodecType Type;
            public CodecState State;
            public CodecData(CodecType T, CodecState S)
            {
                Type = T;
                State = S;
            }
        };

        private Hashtable KnownCodecs = new Hashtable
        {
            {"ffdshow Video Decoder",       new CodecData(CodecType.VideoDecoder,   CodecState.Recommended) }, 
            {"ffdshow Audio Decoder",       new CodecData(CodecType.AudioDecoder,   CodecState.Recommended) }, 
            {"Default DirectSound Device",  new CodecData(CodecType.AudioRenderer,  CodecState.Recommended) }, 
            {"Video Renderer",              new CodecData(CodecType.VideoRenderer,  CodecState.Recommended) },
            {"MJPEG Decompressor",          new CodecData(CodecType.VideoDecoder,   CodecState.Known) },
            {"Color Space Converter",       new CodecData(CodecType.VideoProcessor, CodecState.Known) },
            {"AXIS AAC Audio Decoder",      new CodecData(CodecType.AudioDecoder,   CodecState.Known) },
            {"AXIS H.264 Video Decoder",    new CodecData(CodecType.VideoDecoder,   CodecState.Known) },
            {"MainConcept AVC/H.264 Video Decoder",    new CodecData(CodecType.VideoDecoder,   CodecState.Known) },
            {"AVI Decompressor",            new CodecData(CodecType.VideoProcessor, CodecState.Unrecommended) },
        };

        void LogCodecState(string Name)
        {
            if (KnownCodecs.ContainsKey(Name))
            {
                CodecData Data = KnownCodecs[Name] as CodecData;
                string Out = "";
                switch (Data.State)
                {
                    case CodecState.Recommended:
                        Out += "Recommended";
                        break;
                    case CodecState.Known:
                        Out += "Known";
                        break;
                    case CodecState.Unrecommended:
                        Out += "Unrecommended";
                        break;
                }
                Out += " ";
                switch (Data.Type)
                {
                    case CodecType.Source:
                        Out += "Source";
                        break;
                    case CodecType.VideoDecoder:
                        Out += "Video Decoder";
                        break;
                    case CodecType.AudioDecoder:
                        Out += "Audio Decoder";
                        break;
                    case CodecType.VideoRenderer:
                        Out += "Video Renderer";
                        break;
                    case CodecType.AudioRenderer:
                        Out += "Audio Renderer";
                        break;
                    case CodecType.VideoProcessor:
                        Out += "Video Processor";
                        break;
                    case CodecType.AudioProcessor:
                        Out += "Audio Processor";
                        break;
                }
                Out += " used: " + Name;
                EventConsumer.FireLogStepEvent(Out);
            }
            else
            {
                EventConsumer.FireLogStepEvent("Unknown filter used [" + Name + "], please report to tech support");
            }
        }

        void StepCodecsCheck()
        {
            if (EventConsumer == null) return;
            
            // AT, 06/10/2011
            // there was a bug - no step end after "Init environement".
            // such hack should be not dangerous
            if (InStep)
            {
                EventConsumer.FireStepPassed();
                InStep = false;
            }
            EventConsumer.FireBeginStep("Checking filters");
            if (activeFilters != null)
            {
                foreach (string s in activeFilters)
                {
                    LogCodecState(s);
                }
            }
            else
            {
                throw new AssertException("No Codecs in graph!");
                //EventConsumer.FireLogStepEvent("No Codecs in graph!");
            }
            EventConsumer.FireStepPassed();
        }

        #endregion

        #endregion

        #endregion

        #region ToCopy
        #endregion

        #region Main staff

        enum ContainerState
        {
            CS_Empty,
            CS_Connected,
            CS_Running,
            CS_Paused,
            CS_Stopped
        }

        ContainerState State = ContainerState.CS_Empty;

        public string FileName
        {
            get
            {
                CreateFile();
                return omsdFileName;
            }
        }

        // create source file for SourceFilter with all streaming parameters
        private void CreateFile()
        {
            if (FileValid) return;

            if (omsdFileName != null)
            {
                System.IO.File.Delete(omsdFileName);
                omsdFileName = null;
            }

            //LastMortLine = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Test Options]");
            sb.AppendLine(string.Format("Address = \"{0}\"", address));
            if (user != null)
                sb.AppendLine(string.Format("User = \"{0}\"", user));
            if (password != null)
                sb.AppendLine(string.Format("Password = \"{0}\"", password));

            sb.AppendLine(string.Format("UseTCPTunnel = {0}", tcp ? 1 : 0));
            //sb.AppendLine(string.Format("UseHTTPTunnel = {0}", http ? 1 : 0));
            sb.AppendLine(string.Format("TunnelPortNumber = {0}", httpport));

            // HACK begin
            sb.AppendLine(string.Format("VideoWidth = {0}", AssumeWidth));
            sb.AppendLine(string.Format("VideoHeight = {0}", AssumeHeight));
            sb.AppendLine(string.Format("UseAudio = {0}", proceedAudio ? 1 : 0));
            // HACK end
            sb.AppendLine(string.Format("Timeout = {0}", AssumeTimeout));

            sb.AppendLine(string.Format("UseOptions = {0}", proceedOptions ? 1 : 0));

            sb.AppendLine(string.Format("UseKeepAlive = {0}", proceedParameter ? 1 : 0));

            sb.AppendLine(string.Format("UseKeepAliveOptions = {0}", keepAliveOptions ? 1 : 0));

            sb.AppendLine(string.Format("UseMetadata = {0}", proceedMetadata ? 1 : 0));

            sb.AppendLine(string.Format("Multicast = {0}", multicast ? 1 : 0));

            sb.AppendLine(string.Format("UseVideo = {0}", proceedVideo ? 1 : 0));

            sb.AppendLine(string.Format("RTSP = {0}", rtsp ? 1 : 0));
            sb.AppendLine(string.Format("VideoFPS = {0}", videoFPS));
            sb.AppendLine(string.Format("VideoCodecName = \"{0}\"", videoCodecName));
            sb.AppendLine(string.Format("AudioCodecName = \"{0}\"", audioCodecName));
            sb.AppendLine(string.Format("MulticastAddress = \"{0}\"", multicastAddress));
            sb.AppendLine(string.Format("MulticastRtpPortVideo = {0}", multicastRtpPortVideo));
            sb.AppendLine(string.Format("MulticastRtpPortAudio = {0}", multicastRtpPortAudio));
            sb.AppendLine(string.Format("MulticastTTL = {0}", multicastTTL));

            sb.AppendLine(string.Format("CustomSetupFields = \"{0}\"", customSetupFields));
            sb.AppendLine(string.Format("CustomPlayFields = \"{0}\"", customPlayFields));
            sb.AppendLine(string.Format("CustomPauseFields = \"{0}\"", customPauseFields));
            sb.AppendLine(string.Format("DoSetupOnReplay = {0}", doSetupOnReplay ? 1 : 0));
            sb.AppendLine(string.Format("ReplayMode = {0}", replayMode ? 1 : 0));
            sb.AppendLine(string.Format("ReplayMaxDuration = {0}", replayMaxDuration));
            sb.AppendLine(string.Format("ReplayPauseWait = {0}", replayPauseWait));
            sb.AppendLine(string.Format("ReplayReverse = {0}", replayReverse ? 1 : 0));

            omsdFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".omsd");
            StreamWriter outfile = new StreamWriter(omsdFileName);
            outfile.Write(sb.ToString());
            outfile.Close();

            FileValid = true;
        }

        private void SetSource(string FileName)
        {
            try
            {
                State = ContainerState.CS_Empty;
                FilterGraph = new DS.FilgraphManager();
                FilterGraph.RenderFile(FileName);
                VideoWindow = FilterGraph as DS.IVideoWindow;
                MediaControl = FilterGraph as DS.IMediaControl;
                ScanGraph();
                State = ContainerState.CS_Connected;
            }
            catch (COMException ex)
            {
                if (EventConsumer != null) return;
                string ErrorLine = null;
                bool Internal = GetErrorDescription(out ErrorLine);
                if (Internal)
                {
                    throw new VideoException(ErrorLine);
                }
                else
                {
                    throw new VideoException(string.Format("General AV error #{0:X08}, please check MSDN", (uint)ex.ErrorCode), ex);
                }
            }
        }

        private void ProceedAction(int From, int To, IAsyncResult Result, ref Exception ex)
        {
            try
            {
                if (EventConsumer != null)
                {   // can throw in step
                    LogActivity(Timeout, From, To, Result);
                }
                else
                {
                    Result.AsyncWaitHandle.WaitOne(Timeout);
                }
                if (!Result.IsCompleted)
                {   // timeout, no exception in stack
                    throw new VideoException("Connection Timeout");
                }
                if (ex != null)
                {   // action ends with exception
                    if (!InStep && (EventConsumer != null))
                    {
                        EventConsumer.FireBeginStep("Verifying connection state");
                    }
                    throw ex;
                }
            }
            catch (Exception exp)
            {
                Clear();
                if (exp.Message == "Error HRESULT E_FAIL has been returned from a call to a COM component.")
                {
                    throw new Exception("Operation failed");
                }
                else
                {
                    throw exp;
                }
            }
        }

        private void Connect()
        {
            Exception ex = null;
            string FN = FileName;
            Action SetSourceAction = new Action(() => { try { SetSource(FN); } catch (Exception e) { ex = e; } });
            IAsyncResult SetSourceResult = SetSourceAction.BeginInvoke(null, null);

            ProceedAction(step_InitEnvironment, step_OpenStream, SetSourceResult, ref ex);

            StepCodecsCheck();
        }

        public void Run()
        {
            if (!((State == ContainerState.CS_Connected) || (State == ContainerState.CS_Stopped)))
            {
                return;
            }
            if ((TestControl == null) || (MediaControl == null))
            {
                throw new VideoException("Internal error in AV engine");
            }
            Exception ex = null;
            Action RunAction = new Action(() => 
            { 
                try 
                {
                    TestControl.ControlStream(1);
                    MediaControl.Run();
                    State = ContainerState.CS_Running;
                } 
                catch (Exception e) 
                { 
                    ex = e; 
                } 
            });
            IAsyncResult RunResult = RunAction.BeginInvoke(null, null);

            if (RTSP)
            {
                ProceedAction(step_SETUP, step_WaitStream, RunResult, ref ex);
            }
            else
            {
                // skip SETUP and PLAY steps
                ProceedAction(step_WaitStream, step_WaitStream, RunResult, ref ex);
            }
        }
        public void Stop()
        {
            if ((State != ContainerState.CS_Running) && (State != ContainerState.CS_Paused))
            {
                return;
            }
            if ((TestControl == null) || (MediaControl == null))
            {
                throw new VideoException("Internal error in AV engine");
            }
            Exception ex = null;
            Action StopAction = new Action(() =>
            {
                try
                {
                    int StreamState = 0;
                    State = ContainerState.CS_Stopped;
                    VideoWindow.Visible = 0;
                    TestControl.GetStreamHealth(ref StreamState);
                    TestControl.ControlStream(2);
                    //if (StreamState > 0)// TODO why it works fine that way? why it locks instead sometimes?
                    {
                        System.Diagnostics.Trace.WriteLine("Trying to close DirectShow " + StreamState.ToString());
                        MediaControl.Stop();
                        System.Diagnostics.Trace.WriteLine("DirectShow closed");
                    }
                    //else
                    {
                        //System.Diagnostics.Trace.WriteLine("Bypassing DirectShow close as it is not safe " + StreamState.ToString());
                        //MediaControl.StopWhenReady();
                        //System.Diagnostics.Trace.WriteLine("DirectShow lazy close initiated");
                    }
                    TestControl.ControlStream(0);
                }
                catch (Exception e)
                {
                    ex = e;
                }
            });
            IAsyncResult StopResult = StopAction.BeginInvoke(null, null);

            if (RTSP)
            {
                ProceedAction(step_StopThread, step_TEARDOWN, StopResult, ref ex);
            }
            else
            {
                // skip TEARDOWN step
                ProceedAction(step_StopThread, step_StopThread, StopResult, ref ex);
            }
        }

        private void LogOutput(ref int MsgFrom, OMSDIO Ini, int LogStep)
        {
            int MsgTo = Ini.GetMessageOfs(LogStep + 1);
            if ((MsgTo == 0) || (MsgTo < MsgFrom))
            {
                MsgTo = Ini.GetMessageCount() + 1;
            }
            while (MsgFrom < MsgTo)
            {
                string s = Ini.GetMessage(MsgFrom++);
                if (!string.IsNullOrEmpty(s))
                {
                    EventConsumer.FireLogStepEvent(s);
                }
            }
        }

        private void DoCommand(
            bool condition, Action commandAction, int stepFrom, int stepTo,
            Action successAction, ContainerState successState, int stepToSetState)
        {
            if (!condition)
            {
                return;
            }
            if ((TestControl == null) || (MediaControl == null))
            {
                throw new VideoException("Internal error in AV engine");
            }
            Exception ex = null;
            Action CommandAction = new Action(() =>
            {
                try
                {
                    commandAction();
                }
                catch (Exception e)
                {
                    ex = e;
                }
            });

            OMSDIO ini = new OMSDIO(omsdFileName);
            ini.WriteString("Test State", "StepEnded", "0");
            int MsgFirst = ini.GetMessageCount() + 1;

            int StepEnded = 0;
            int LogStep = stepFrom;
            int StepState = 0;
            int MsgCurr = ini.GetMessageCount() + 1;

            IAsyncResult Result = CommandAction.BeginInvoke(null, null);

            try
            {
                DateTime Till = DateTime.Now.AddSeconds(Timeout / 1000);
                if (EventConsumer != null)
                {
                    EventConsumer.FireBeginStep(StepNames[LogStep]);

                    while (StepState != stepTo && LogStep <= stepTo && DateTime.Now < Till)
                    {
                        StepState = ini.ReadInt("Test State", "StepEnded", 0);
                        StepEnded = Math.Min(Math.Abs(StepState), stepTo);
                        // pass steps which has been ended till this moment
                        while (LogStep < StepEnded)
                        {
                            LogOutput(ref MsgCurr, ini, LogStep);

                            EventConsumer.FireStepPassed();
                            EventConsumer.FireBeginStep(StepNames[++LogStep]);
                        }
                        // check if step is ended
                        if (StepEnded != LogStep)
                        {
                            System.Threading.Thread.Sleep(100);
                            // output current messages from this step
                            LogOutput(ref MsgCurr, ini, LogStep);
                            continue;
                        }
                        // output remaining messages from this step
                        LogOutput(ref MsgCurr, ini, LogStep);

                        // step passed
                        if (StepState > 0) 
                        {
                            EventConsumer.FireStepPassed();
                        }
                        // step failed
                        if (StepState < 0)
                        {
                            string ErrorLine;
                            GetErrorDescription(out ErrorLine);
                            throw new VideoException(ErrorLine);
                        }
                        // begin next step
                        if (LogStep < stepTo)
                        {
                            EventConsumer.FireBeginStep(StepNames[++LogStep]);
                        }
                    }
                }
                else
                {
                    Result.AsyncWaitHandle.WaitOne(Timeout);
                }
                if (!Result.IsCompleted)
                {   // timeout, no exception in stack
                    throw new VideoException("Connection Timeout");
                }
                if (ex != null)
                {   // action ends with exception
                    if (!InStep && (EventConsumer != null))
                    {
                        EventConsumer.FireBeginStep("Verifying connection state");
                    }
                    throw ex;
                }
            }
            catch (Exception exp)
            {
                if (StepEnded > stepToSetState || (StepEnded == stepToSetState && StepState > 0))
                {
                    State = successState;
                }
                if (exp.Message == "Error HRESULT E_FAIL has been returned from a call to a COM component.")
                {
                    throw new Exception("Operation failed");
                }
                else
                {
                    throw exp;
                }
            }

            successAction();
            State = successState;
        }
        public void Play()
        {
            DoCommand(
                (State == ContainerState.CS_Connected) || (State == ContainerState.CS_Stopped)
                || (State == ContainerState.CS_Paused) || (State == ContainerState.CS_Running),
                () =>
                {
                    if (State != ContainerState.CS_Running)
                    {
                        MediaControl.Run();
                    }
                    TestControl.ControlStream(1);
                },
                (State == ContainerState.CS_Connected) 
                || ((State == ContainerState.CS_Stopped) && DoSetupOnReplay) ? step_SETUP : step_PLAY, step_WaitStream,
                () =>
                {
                },
                ContainerState.CS_Running, step_SETUP);
        }
        public void Pause()
        {
            DoCommand(
                (State == ContainerState.CS_Running),
                () =>
                {
                    TestControl.ControlStream(3);
                },
                step_PAUSE, step_PAUSE,
                () =>
                {
                    MediaControl.Pause();
                },
                ContainerState.CS_Paused, step_PAUSE);
        }
        public void Teardown()
        {
            DoCommand(
                (State == ContainerState.CS_Running || State == ContainerState.CS_Paused),
                () =>
                {
                    //MediaControl.Stop();
                    //TestControl.ControlStream(2);

                    VideoWindow.Visible = 0;
                    TestControl.ControlStream(2);
                    MediaControl.Stop();
                    TestControl.ControlStream(0);

                },
                step_StopThread, step_TEARDOWN,
                () =>
                {
                    VideoWindow.Visible = 0;
                },
                ContainerState.CS_Stopped, step_TEARDOWN);
        }

        public void Clear()
        {
            Stop();
            if (VideoWindow != null)
            {
                VideoWindow.Visible = 0;
            }

            FileValid = false;
            TestControl = null;
            MediaControl = null;
            VideoWindow = null;
            FilterGraph = null;
            if (omsdFileName != null)
            {
                System.IO.File.Delete(omsdFileName);
                FileValid = false;
            }
            State = ContainerState.CS_Empty;
        }
        public void SetupWindow()
        {
            if (VideoWindow == null) return;

            VideoWindow.Caption = "ONVIF Video Stream";
            VideoWindow.WindowStyle = VideoWindow.WindowStyle & (~(WS_NORWALWINDOW)) | WS_TOOLWINDOW;
            VideoWindow.WindowStyleEx = VideoWindow.WindowStyleEx | WS_EX_TOOLWINDOW | WS_EX_APPWINDOW;
        }
        ~VideoContainer()
        {
            Clear();
        }
        #endregion

    }
}
