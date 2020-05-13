///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.TestBase;

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
        private bool proceedOptions = false;
        private bool proceedAudio = false;
        private int AssumeWidth = 0;
        private int AssumeHeight = 0;
        private int AssumeTimeout = 0;
        private bool proceedParameter = false;
        private bool proceedMetadata = false;
        private bool proceedSyncPoint = false;
        #endregion

        #region Filter Constansts
        private int step_Constructed = 0;
        private int step_InitEnvironment    = 1;
        private int step_OPTIONS            = 2;
        private int step_CheckOptions       = 3;
        private int step_DESCRIBE           = 4;
        private int step_OpenStream         = 5;
        private int step_SETUP              = 6;
        private int step_PLAY               = 7;
        private int step_WaitStream         = 8;
        private int step_StopThread         = 9;
        private int step_TEARDOWN           =10;
        private int step_HaltEnvironment    =11;
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

        public void OpenWindow(bool WithAudio) 
        {
            UseAudio = WithAudio;
            Connect();
            SetupWindow();
            Run();
        }
        public void CloseWindow() 
        {
            Stop();
            Clear();
        }
        #endregion

        #region External Ini

    private class IniFileClass
	{
		private String IniFile = "";
		
		// Importazione dei metodi dalle DLL di sistema
		[DllImport("kernel32")] private static extern long WritePrivateProfileInt(String Section, String Key, int Value, String FilePath);
		[DllImport("kernel32")] private static extern long WritePrivateProfileString(String Section, String Key, String Value, String FilePath);
		[DllImport("kernel32")] private static extern int GetPrivateProfileInt(String Section, String Key, int Default, String FilePath);
		[DllImport("kernel32")] private static extern int GetPrivateProfileString(String Section, String Key, String Default, StringBuilder retVal, int Size, String FilePath);

		public IniFileClass(String IniFile)
		{
			// Mi salvo il percorso del file INI
			this.IniFile = IniFile;
		}

		public String ReadString(String Section, String Key, String Default)
		{
			// Creo lo StringBuilder che conterrà la stringa
			StringBuilder StrBu = new StringBuilder(255);
			// Provo a leggere il parametro richiesto
			GetPrivateProfileString(Section, Key, Default, StrBu, 255, IniFile);
			// Restituisco il parametro letto
			return StrBu.ToString();
		}

		public int ReadInt(String Section, String Key, int Default)
		{
			// Restituisco il valore del parametro letto
			return GetPrivateProfileInt(Section, Key, Default, IniFile); 
		}

		public void WriteString(String Section, String Key, String Value)
		{
			// Salvo il valore del parametro impostato
			WritePrivateProfileString(Section, Key, Value, IniFile);
		}

		public void WriteInt(String Section, String Key, int Value)
		{
			// Salvo il valore del parametro impostato
			WritePrivateProfileInt(Section, Key, Value, IniFile);
		}
	}
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
            }
            if (Result.IsCompleted && (ToStep <= ini.GetCurrentStep()))
            {
                while (LogStep <= ToStep)
                {
                    if (!RollActivityPart(ref ini, ref MessID, ref LogStep, ToStep)) break;
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
                for (FailStep = 0; ((FailBits & (1 << FailStep)) == 0) && (FailStep < 12); FailStep++) { };
                if (FailStep >= 12) return false;
            }
            switch (FailStep)
            {
                case 0:
                    Error = "RTSP filter not found, please check your installation";
                    break;
                case 1:
                    Error = "RTSP filter internal error, initialization failed";
                    break;
                case 2:
                    Error = "OPTIONS command error, please check your connection";
                    break;
                case 3:
                    Error = "OPTIONS validation error";//, OPTIONS = [" + ini.GetMessage(ini.GetMessageOfs(4) - 2) + "]";
                    break;
                case 4:
                    Error = "DESCRIBE command error, please check your connection";
                    break;
                case 5:
                    Error = "No known media sources found";
                    break;
                case 6:
                    Error = "SETUP command error";
                    break;
                case 7:
                    Error = "PLAY command error";
                    break;
                case 8:
                    Error = "No media frames within timeout, please check your connection";
                    break;
                case 9:
                    Error = "RTSP filter internal error - race conditions";
                    break;
                case 10:
                    Error = "TEARDOWN command error";
                    break;
                case 11:
                    Error = "RTSP filter internal error - finalization failed";
                    break;
                    //default :
                    //Error = "ddd";
                    //break;
            }
            return true;
        }

        #region Graph Processing
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
                EventConsumer.FireLogStepEvent("No Codecs in graph!");
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

            sb.AppendLine(string.Format("UseMetadata = {0}", proceedMetadata ? 1 : 0));

            omsdFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".omsd");
            StreamWriter outfile = new StreamWriter(omsdFileName);
            outfile.Write(sb.ToString());
            outfile.Close();

            FileValid = true;
        }
#if false
        private void ProceedEx(Exception ex)
        {
            string ErrorLine = null;
            bool Internal = GetErrorDescription(out ErrorLine);
            if (Internal)
            {
                throw new VideoException(ErrorLine);
            }
            else
            {
                throw ex;
            }
        }
#endif
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
#if true
            ProceedAction(1, 5, SetSourceResult, ref ex);
#else
            try
            {
                if (EventConsumer != null)
                {   // can throw in step
                    LogActivity(Timeout, 1, 5, SetSourceResult);
                }
                else
                {
                    SetSourceResult.AsyncWaitHandle.WaitOne(Timeout);
                }
                if (!SetSourceResult.IsCompleted)
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
            catch (Exception)
            {
                Clear();
            	throw;
            }
#endif
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
#if true
            ProceedAction(6, 8, RunResult, ref ex);
#else
            if (EventConsumer != null)
            {
                LogActivity(Timeout, 6, 8, RunResult);
            }
            else
            {
                RunResult.AsyncWaitHandle.WaitOne(Timeout);
            }
            if (!RunResult.IsCompleted)
            {
                throw new VideoException("Connection Timeout");
            }
            if (ex != null)
            {
                ProceedEx(ex);
                //throw ex;
            }
#endif
        }
        public void Stop()
        {
            if (State != ContainerState.CS_Running)
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
#if true
            ProceedAction(9, 10, StopResult, ref ex);
#else
            if (EventConsumer != null)
            {
                LogActivity(Timeout, 9, 10, StopResult);
            }
            else
            {
                StopResult.AsyncWaitHandle.WaitOne(Timeout);
            }
            if (!StopResult.IsCompleted)
            {
                throw new VideoException("Connection Timeout");
            }
            if (ex != null)
            {
                ProceedEx(ex);
                //throw ex;
            }
#endif
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
