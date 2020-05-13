///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
//using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Exceptions;
using DS = QuartzTypeLib;

using ONVIFRTSPFilter1506;
using System.Threading;
using TestTool.Tests.Definitions.Interfaces;
using Microsoft.Win32;

namespace TestTool.Tests.Common.TestEngine
{
    public class VideoContainer2
    {
        private const string DllFilePath = "onvifrtsp1506.dll";
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        public extern static int RunTestSession(string Filepath);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        public extern static int StartTestSession(string Filepath);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        public extern static int IsTestSequence(int Session);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ClearTestSequence(int Session);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        public extern static int CallTestSequence(int Session, int Sequence);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        public extern static int CallTestCommand(int Session, int Command);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        public extern static int CloseTestSession(int Session);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void SharpStepCallback(int ObjectId, int StepNumber, int Type, string Text);

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetGlobalStepCallback([MarshalAs(UnmanagedType.FunctionPtr)] SharpStepCallback callbackPointer);

        static bool TriedRegistrationIssue = false;

        private static string GetRegisteredFilterPath()
        {
          string str = "";
          RegistryKey regKeyAppRoot = Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\CLSID\{F37C8D57-E7F1-41B9-98D3-D98FAE3618D8}\InprocServer32");
          if (regKeyAppRoot == null)
          {
            regKeyAppRoot = Registry.ClassesRoot.OpenSubKey(@"Wow32*32Node\CLSID\{F37C8D57-E7F1-41B9-98D3-D98FAE3618D8}\InprocServer32");
          }
          if (regKeyAppRoot == null)
          {
            return "";
          }
          str = (string)regKeyAppRoot.GetValue("");
          Console.WriteLine(str);
          return str;
        }
        public static bool VerifyFilterRegistration()
        {
          string str = GetRegisteredFilterPath();
          string file = System.Windows.Forms.Application.ExecutablePath;
          int pos = file.LastIndexOf('\\');
          file = file.Substring(0, pos) + "\\" + DllFilePath;
          if ((file != str) && !TriedRegistrationIssue)
          {
            TriedRegistrationIssue = true;
            if (System.Windows.Forms.MessageBox.Show(
              "System is using another RTSP filter than our current application.\n" +
              "It will not be possible to obtain rtsp logs, and some tests may work incorrectly.\n" +
              "Please approve selecting current application filter as system filter!",
              "RTSP Filter Message", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
              System.Diagnostics.Process proc = new System.Diagnostics.Process();
              // running register service
              try
              {
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = "regsvr32.exe";
                proc.StartInfo.Arguments = "\"" + file + "\"";
                proc.StartInfo.RedirectStandardOutput = false;
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
              }
              catch (Exception e)
              {
                System.Windows.Forms.MessageBox.Show("RTSP filter had not been registered properly", "Error registering filter");
              }
            }
          }
          return true;
        }
        private bool Failed = false;
        private string FailedText = null;
        public static void RegisterCallbacks()
        {
            VerifyFilterRegistration();
            SharpStepCallback callback =
                  (ObjectId, StepNumber, Type, Text) =>
                  {
                    VideoContainer2 v = null;
                    IVideoFormEvent EventConsumer = null;
                    lock (AllContainers)
                    {
                      v = AllContainers[ObjectId];
                    }
                    if (v != null)
                    {
                      EventConsumer = v.EventConsumer;
                    }
                    try
                    {
                      switch (Type)
                      {
                        case 0:
                          if (EventConsumer != null) { EventConsumer.FireBeginStep(Text); };
                          Console.WriteLine("\nFilter = {0}, step{1}: Begin {2}", ObjectId, StepNumber, Text);
                          break;
                        case 1:
                          if (EventConsumer != null) { EventConsumer.FireStepRequest(Text); };
                          Console.WriteLine("\nFilter = {0}, step{1}: Send [{2}]", ObjectId, StepNumber, Text);
                          break;
                        case 2:
                          if (EventConsumer != null) { EventConsumer.FireStepResponse(Text); };
                          Console.WriteLine("\nFilter = {0}, step{1}: Recv [{2}]", ObjectId, StepNumber, Text);
                          break;
                        case 3:
                          if (EventConsumer != null) { EventConsumer.FireLogStepEvent(Text); };
                          Console.WriteLine("\nFilter = {0}, step{1}: Log  [{2}]", ObjectId, StepNumber, Text);
                          break;
                        case 4:
                          if (EventConsumer != null) { EventConsumer.FireStepPassed(); };
                          Console.WriteLine("\nFilter = {0}, step{1}: Success {2}", ObjectId, StepNumber, Text);
                          break;
                        case 5:
                          v.Failed = true;
                          v.FailedText = Text;
                          //if (EventConsumer != null) { EventConsumer.FireStepFailed(Text); };
                          Console.WriteLine("\nFilter = {0}, step{1}: Failure {2}", ObjectId, StepNumber, Text);
                          break;
                        default:
                          Console.WriteLine("\nFilter = {0}, step{1}: Undefined type {3}, Text=[{2}]", ObjectId, StepNumber, Text, Type);
                          break;
                      }
                    }
                    catch (Exception ex)
                    {
                      v.Failed = true;
                      v.FailedText = ex.Message;
                    }
                  };
            SetGlobalStepCallback(callback);
        }
      
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

        private const int WS_NORMALWINDOW   = WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
        private const int WS_TOOLWINDOW     = WS_DLGFRAME;

        private const int WS_EX_TOOLWINDOW  = 0x00000080;
        private const int WS_EX_APPWINDOW   = 0x00040000;
        private const int WS_EX_NOACTIVATE  = 0x08000000;
        #endregion

        #region Filter Variables
        private DS.FilgraphManager FilterGraph = null;
        private DS.IVideoWindow     VideoWindow     = null;
        private DS.IMediaControl    MediaControl    = null;
        private ITestControl        TestControl     = null;
        #endregion

        #region Interface backend
        private string omsdFileName = null;
        private bool FileValid = false;

        #endregion


        private string[] activeFilters = null;

        #endregion

        #region Interface

        private string Uri = null;
        private string User = null;
        private string Password = null;
        private int Transport = 0;
        private int Port = 0;
        private int NICIndex = 0;
        private int Timeout = 0;
        public void ConfigureConnection(string Uri, string User = "", string Password = "", int Transport = 0, int Port = 0, int NICIndex = 0, int Timeout = 0)
        {
          this.Uri = Uri;
          this.User = User;
          this.Password = Password;
          this.Transport = Transport;
          this.Port = Port;
          this.NICIndex = NICIndex;
          this.Timeout = Timeout;
          FileValid = false;
        }
        private bool UseVideo = false;
        private string VideoCodec = null;
        private int VideoWidth = 0;
        private int VideoHeight = 0;
        private int VideoFPS = 0;
        private string VideoMulticastAddress = null;
        private int VideoMulticastRtpPort = 0;
        private int VideoMulticastTTL = 0;
        public void ConfigureVideo(string Codec, int Width, int Height, int FPS = 1, string Multicast = "", int Port = 0, int TTL = 0)
        {
          UseVideo = true;
          this.VideoCodec = Codec;
          this.VideoWidth = Width;
          this.VideoHeight = Height;
          this.VideoFPS = FPS;
          this.VideoMulticastAddress = Multicast;
          this.VideoMulticastRtpPort = Port;
          this.VideoMulticastTTL = TTL;
          FileValid = false;
        }
        private bool UseAudio = false;
        private string AudioCodec = null;
        private int AudioChannels = 0;
        private int AudioFrequency = 0;
        private string AudioMulticastAddress = null;
        private int AudioMulticastRtpPort = 0;
        private int AudioMulticastTTL = 0;
        public void ConfigureAudio(string Codec, int Channels, int Frequency, string Multicast = "", int Port = 0, int TTL = 0)
        {
          UseAudio = true;
          this.AudioCodec = Codec;
          this.AudioChannels = Channels;
          this.AudioFrequency = Frequency;
          this.AudioMulticastAddress = Multicast;
          this.AudioMulticastRtpPort = Port;
          this.AudioMulticastTTL = TTL;
          FileValid = false;
        }
        private bool UseMeta = false;
        private string MetaMulticastAddress = null;
        private int MetaMulticastRtpPort = 0;
        private int MetaMulticastTTL = 0;
        public void ConfigureMeta(string Multicast = "", int Port = 0, int TTL = 0)
        {
          UseMeta = true;
          this.MetaMulticastAddress = Multicast;
          this.MetaMulticastRtpPort = Port;
          this.MetaMulticastTTL = TTL;
          FileValid = false;
        }

        private bool UseBackchannel = false;
        private string BackchannelCodec = null;
        private string BackchannelFile = null;
        public void ConfigureBackchannel(string Codec, string Filename)
        {
          this.UseBackchannel = true;
          this.BackchannelCodec = Codec;
          this.BackchannelFile = Filename;
        }

        int SequenceNumber = 0;
        bool CheckOptions = false;
        bool CheckActualResolution = false;
        bool CheckJPEGExtension = false;
        int SequenceTimeout = 0;
        string CustomSetupFields = "";
        string CustomPlayFields = "";
        string CustomPauseFields = "";
        public void SetSequence(int SequenceNumber, int Timeout = -1, bool CheckOptions = false, bool CheckActualResolution = false, bool CheckJPEGExtension = false,
          string CustomSetupFields = "", string CustomPlayFields = "", string CustomPauseFields = "")
        {
          this.SequenceNumber = SequenceNumber;
          this.SequenceTimeout = (Timeout >= 0) ? Timeout : this.Timeout;
          this.CheckOptions = CheckOptions;
          this.CheckActualResolution = CheckActualResolution;
          this.CheckJPEGExtension = CheckJPEGExtension;
          this.CustomSetupFields = CustomSetupFields;
          this.CustomPlayFields = CustomPlayFields;
          this.CustomPauseFields = CustomPauseFields;
          FileValid = false;
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




        public void OpenWindow(bool WithAudio, bool WithVideo)
        {
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
            Clear(true);
        }

        #endregion

      
        #region Graph Processing
        // fill list of directshow filters in FilterGraph
        void ScanGraph()
        {
            activeFilters = null;
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
            {"ffdshow Video Decoder",               new CodecData(CodecType.VideoDecoder,   CodecState.Recommended) }, 
            {"ffdshow Audio Decoder",               new CodecData(CodecType.AudioDecoder,   CodecState.Recommended) }, 
            {"Default DirectSound Device",          new CodecData(CodecType.AudioRenderer,  CodecState.Recommended) }, 
            {"Video Renderer",                      new CodecData(CodecType.VideoRenderer,  CodecState.Recommended) },
            {"MJPEG Decompressor",                  new CodecData(CodecType.VideoDecoder,   CodecState.Known) },
            {"Color Space Converter",               new CodecData(CodecType.VideoProcessor, CodecState.Known) },
            {"AXIS AAC Audio Decoder",              new CodecData(CodecType.AudioDecoder,   CodecState.Known) },
            {"AXIS H.264 Video Decoder",            new CodecData(CodecType.VideoDecoder,   CodecState.Known) },
            {"MainConcept AVC/H.264 Video Decoder", new CodecData(CodecType.VideoDecoder,   CodecState.Known) },
            {"Microsoft DTV-DVD Video Decoder",     new CodecData(CodecType.VideoDecoder,   CodecState.Known) },
            {"Microsoft DTV-DVD Audio Decoder",     new CodecData(CodecType.AudioDecoder,   CodecState.Known) },
            {"ACM Wrapper",                         new CodecData(CodecType.AudioDecoder,   CodecState.Known) },
            {"AVI Decompressor",                    new CodecData(CodecType.VideoProcessor, CodecState.Unrecommended) },
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
                //System.Diagnostics.Debug.WriteLine(Out);
                if (EventConsumer != null) { EventConsumer.FireLogStepEvent(Out); };
            }
            else
            {
              //System.Diagnostics.Debug.WriteLine("Unknown filter used [" + Name + "], please report to tech support");
              if (EventConsumer != null) { EventConsumer.FireLogStepEvent("Unknown filter used [" + Name + "], please report to tech support"); }
            }
        }

        private void StepCodecsCheck()
        {
          if (EventConsumer != null) { EventConsumer.FireBeginStep("Checking filters"); }
          if (activeFilters != null)
          {
            foreach (string s in activeFilters)
            {
              LogCodecState(s);
            }
          }
          else
          {
            //throw new AssertException("No Codecs in graph!");
            EventConsumer.FireLogStepEvent("Warning: no codecs in ffdshow graph - no media will be played!");
          }
          if (EventConsumer != null) { EventConsumer.FireStepPassed(); };
        }
        #endregion

        #endregion

        #region Main staff

        // indicates if it's silent mode or not
        // to hide video window in streaming tests
        static public bool SilentMode = false;

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
            sb.AppendLine("[ControlConnection]");
            sb.AppendLine(string.Format("Uri = \"{0}\"", Uri));
            sb.AppendLine(string.Format("User = \"{0}\"", User));
            sb.AppendLine(string.Format("Password = \"{0}\"", Password));
            sb.AppendLine(string.Format("Transport = {0}", Transport));
            sb.AppendLine(string.Format("Port = {0}", Port));
            sb.AppendLine(string.Format("NICIndex = {0}", NICIndex));
            sb.AppendLine(string.Format("Timeout = {0}", Timeout));

            sb.AppendLine("[TestSequence]");
            sb.AppendLine(string.Format("ObjectId = {0}", MyFilterObjectId));
            sb.AppendLine(string.Format("SequenceNumber = {0}", SequenceNumber));
            sb.AppendLine(string.Format("Timeout = {0}", SequenceTimeout));
            sb.AppendLine(string.Format("CheckOptions = {0}", CheckOptions ? 1 : 0));
            sb.AppendLine(string.Format("CheckActualResolution = {0}", CheckActualResolution ? 1 : 0));
            sb.AppendLine(string.Format("CheckJPEGExtension = {0}", CheckJPEGExtension ? 1 : 0));
            sb.AppendLine(string.Format("CustomSetupFields = {0}", CustomSetupFields));
            sb.AppendLine(string.Format("CustomPlayFields = {0}", CustomPlayFields));
            sb.AppendLine(string.Format("CustomPauseFields = {0}", CustomPauseFields));
            sb.AppendLine(string.Format("UseVideo = {0}", UseVideo ? 1 : 0));
            sb.AppendLine(string.Format("UseAudio = {0}", UseAudio ? 1 : 0));
            sb.AppendLine(string.Format("UseMetadata = {0}", UseMeta ? 1 : 0));
            sb.AppendLine(string.Format("UseBackchannel = {0}", UseBackchannel ? 1 : 0));

            if (UseVideo)
            {
              sb.AppendLine("[Video]");
              sb.AppendLine(string.Format("MulticastAddress = \"{0}\"", VideoMulticastAddress));
              sb.AppendLine(string.Format("MulticastRtpPort = {0}", VideoMulticastRtpPort));
              sb.AppendLine(string.Format("MulticastTTL = {0}", VideoMulticastTTL));
              sb.AppendLine(string.Format("Codec = \"{0}\"", VideoCodec));
              sb.AppendLine(string.Format("Width = {0}", VideoWidth));
              sb.AppendLine(string.Format("Height = {0}", VideoHeight));
              sb.AppendLine(string.Format("FPS = {0}", VideoFPS));
            }

            if (UseAudio)
            {
              sb.AppendLine("[Audio]");
              sb.AppendLine(string.Format("MulticastAddress = \"{0}\"", AudioMulticastAddress));
              sb.AppendLine(string.Format("MulticastRtpPort = {0}", AudioMulticastRtpPort));
              sb.AppendLine(string.Format("MulticastTTL = {0}", AudioMulticastTTL));
              sb.AppendLine(string.Format("Codec = \"{0}\"", AudioCodec));
              sb.AppendLine(string.Format("Channels = {0}", AudioChannels));
              sb.AppendLine(string.Format("Frequency = {0}", AudioFrequency));
            }

            if (UseMeta)
            {
              sb.AppendLine("[Metadata]");
              sb.AppendLine(string.Format("MulticastAddress = \"{0}\"", MetaMulticastAddress));
              sb.AppendLine(string.Format("MulticastRtpPort = {0}", MetaMulticastRtpPort));
              sb.AppendLine(string.Format("MulticastTTL = {0}", MetaMulticastTTL));
            }

            if (UseBackchannel)
            {
              sb.AppendLine("[Backchannel]");
              sb.AppendLine(string.Format("MulticastAddress = \"{0}\"", BackchannelFile));
              sb.AppendLine(string.Format("Codec = \"{0}\"", BackchannelCodec));
            }

            omsdFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".omsd1506");
            StreamWriter outfile = new StreamWriter(omsdFileName);
            outfile.Write(sb.ToString());
            outfile.Close();

            FileValid = true;
        }

        private void SetSource(string FileName)
        {
            try
            {
                FilterGraph = new DS.FilgraphManager();
                try
                {
                    FilterGraph.RenderFile(FileName);
                }
                catch (Exception ex)
                {
                    throw (ex is COMException ? ex : new COMException(ex.Message)); 
                }
                if (UseVideo)
                {
                  VideoWindow = FilterGraph as DS.IVideoWindow;
                }
                MediaControl = FilterGraph as DS.IMediaControl;
                ScanGraph();
            }
            catch (COMException ex)
            {
                // if (EventConsumer != null) return; ??? why is it here?
                string ErrorLine = null;
                System.Threading.Thread.Sleep(1000);
            }
        }


        public void Connect()
        {
          SetSource(FileName);
          StepCodecsCheck();
        }

        public int SilentRun()
        {
          Failed = false;
          int Res = RunTestSession(FileName);
          ClearName();
          if (Failed)
          {
            throw (new VideoException(FailedText));
          }
          return Res;
        }
        private int SilentSession = 0;
        public bool SilentStart()
        {
          Failed = false;
          bool Ret = false;
          if (SilentSession != 0)
          {
            Ret = CallTestSequence(SilentSession, 5) != 0;
          }
          else
          {
            SilentSession = StartTestSession(FileName);
            Ret = SilentSession != 0;
          }
          if (Ret)
          {
            do
            {
              System.Threading.Thread.Sleep(100);
              if (Failed)
              {
                ClearTestSequence(SilentSession);
                throw (new VideoException(FailedText));
              }
              if (UseToTerminate)
              {
                ClearTestSequence(SilentSession);
                throw (new StopEventException("Halt button used"));
              }
            }
            while (IsTestSequence(SilentSession) != 0);
          }
          return Ret;
        }
        public bool SilentPause()
        {
          return CallTestCommand(SilentSession, 8) != 0;
        }
        public bool SilentStop()
        {
          //CallTestCommand(SilentSession, 10);
          CloseTestSession(SilentSession);
          SilentSession = 0;
          return true;
        }
        public void Replay(Action<Action, Action, Action> actionControl)
        {
          actionControl(() => { SilentStart(); }, () => { SilentPause(); }, () => { SilentStop(); });
        }

        private bool InLongCycle = false;
        private bool UseToTerminate = false;
        public bool Terminate(bool Halt)
        {
          if (InLongCycle)
          {
            UseToTerminate = true;
            return true;
          }
          return false;
        }
        public void Run()
        {
          if (TestControl == null)
          {
            SilentRun();
            return;
          }
          UseToTerminate = false;
          Failed = false;
          FailedText = null;
          MediaControl.Run();
          int Activity = 0;
          InLongCycle = true;
          do
          {
            System.Threading.Thread.Sleep(100);
            if (TestControl != null) { TestControl.GetActivity(ref Activity); };
            if (UseToTerminate)
            {
              if (TestControl != null) { TestControl.StopActivity(); };
            }
          }
          while (Activity != 0);
          InLongCycle = false;
          if (UseToTerminate)
          {
             throw (new StopEventException("Halt button used"));
          }
          if (Failed)
          {
            throw (new VideoException(FailedText));
          }

        }


        private void ClearName()
        {
            if (omsdFileName != null)
            {
                System.IO.File.Delete(omsdFileName);
                omsdFileName = null;
                FileValid = false;
            }
        }
        public void Clear()
        {
            Clear(true);
        }

        public void Clear(bool doStop)
        {
            if (doStop)
            {
              if (TestControl != null)
              {
                TestControl.StopActivity();
              }
              if (MediaControl != null)
              {
                if (!UseToTerminate)
                {
                  MediaControl.Stop();
                }
              }
            }
            if (VideoWindow != null)
            {
                VideoWindow.Visible = 0;
            }

            FileValid = false;
            TestControl = null;
            MediaControl = null;
            VideoWindow = null;
            try
            {
              if (FilterGraph != null)
              {
                Marshal.ReleaseComObject(FilterGraph);
              }
            }
            catch (Exception)
            {
            }
            FilterGraph = null;
            ClearName();
        }
        public void SetupWindow()
        {
            if (VideoWindow == null) return;

            VideoWindow.Caption = "ONVIF Video Stream" + string.Format(" {0}", MyFilterObjectId);
            VideoWindow.WindowStyle = VideoWindow.WindowStyle & (~(WS_NORMALWINDOW)) | WS_TOOLWINDOW;
            VideoWindow.WindowStyleEx = VideoWindow.WindowStyleEx | WS_EX_TOOLWINDOW | WS_EX_APPWINDOW;// | WS_EX_NOACTIVATE;

            // hide video window in silent mode for streaming tests
            if (SilentMode)
                VideoWindow.AutoShow = 0;
        }
        private static int FilterObjectId = 0;
        private int MyFilterObjectId = 0;

        static Dictionary<int, VideoContainer2> AllContainers = new Dictionary<int, VideoContainer2>();

        public VideoContainer2()
        {
          FilterObjectId++;
          MyFilterObjectId = FilterObjectId;
          lock (AllContainers)
          {
            AllContainers[MyFilterObjectId] = this;
          }
        }
        ~VideoContainer2()
        {
            Clear();
        }

        public void Reset()
        {
            try
            {
                Clear(false);
            }
            catch (Exception)
            {
            }

            activeFilters           = null;
            omsdFileName            = null;
            FileValid               = false;
        }

        #endregion
        private IVideoFormEvent EventConsumer = null;
        public IVideoFormEvent EventSink
        {
          set { EventConsumer = value; }
        }

    }
}
