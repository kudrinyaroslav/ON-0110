using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;



using QuartzTypeLib;
using ONVIFRTSPFilter;

namespace DirectShow
{
    public class VideoContainer
    {
        private FilgraphManager     FilterGraph     = null;
        private IBasicAudio         BasicAudio      = null;
        private IVideoWindow        VideoWindow     = null;
        private IMediaControl       MediaControl    = null;
        private ITestControl        TestControl     = null;

        private const int WS_CHILD          = 0x40000000;
        private const int WS_CLIPCHILDREN   = 0x2000000;

        private bool FileValid = false;
        private string user = null;
        private string password = null;
        private string address = null;
        private bool http;
        private bool tcp;
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
        public bool HTTP
        {
            get { return http; }
            set { http = value; FileValid = false; }
        }
        public bool TCP
        {
            get { return tcp; }
            set { tcp = value; FileValid = false; }
        }
        private string omsdFileName = null;

        private void CreateFile()
        {
            if (FileValid) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Test Options]");
            sb.AppendLine(string.Format("Address = \"{0}\"", address));
            if (user != null)
                sb.AppendLine(string.Format("User = \"{0}\"", user));
            if (password != null)
                sb.AppendLine(string.Format("Password = \"{0}\"", password));

            sb.AppendLine(string.Format("UseTCPTunnel = {0}", tcp ? 1 : 0));
            sb.AppendLine(string.Format("UseHTTPTunnel = {0}", http ? 1 : 0));

            omsdFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".omsd");
            StreamWriter outfile = new StreamWriter(omsdFileName);
            outfile.Write(sb.ToString());
            outfile.Dispose();
            outfile = null;

            FileValid = true;
        }

        public string FileName
        {
            get 
            {
                CreateFile();
                //if (!FileValid) return null;
                return omsdFileName; 
            }
        }

        public void Connect()
        {
            SetSource(FileName);
        }

        public void Clear()
        {
            if (omsdFileName != null)
            {
                FileValid = false;
                System.IO.File.Delete(omsdFileName);
            }
            if (MediaControl != null)
                MediaControl.Stop();

            if (VideoWindow != null)
            {
                VideoWindow.Visible = 0;
                VideoWindow.Owner = 0;
            }

            TestControl     = null;
            MediaControl    = null;
            VideoWindow     = null;
            BasicAudio      = null;
            FilterGraph     = null;
        }

        public bool SetSource(string FileName)
        {
            try
            {
                FilterGraph = new FilgraphManager();
                object ppUnk = null;
                FilterGraph.AddSourceFilter(FileName, out ppUnk);
                FilterGraph.RenderFile(FileName);
                IFilterInfo Info = (IFilterInfo)ppUnk;
                TestControl = Info.Filter as ITestControl;
                BasicAudio      = FilterGraph as IBasicAudio;
                VideoWindow     = FilterGraph as IVideoWindow;
                MediaControl    = FilterGraph as IMediaControl;
            }
            catch (Exception ex)
            {
                Clear();
                return false;
            }
            return true;

        }
        public bool GetLogLine(ref string Line)
        {
            if (TestControl == null) return false;
            try
            {
                TestControl.PollLog(ref Line);
            }
            catch (Exception)
            {
                return false;
            }
            return Line.Length > 0;
        }
        public void AttachWindow(System.Windows.Forms.Control window)
        {
            if (VideoWindow == null) return;

            VideoWindow.Owner = (int)window.Handle;
            VideoWindow.WindowStyle = WS_CHILD | WS_CLIPCHILDREN;
            VideoWindow.SetWindowPosition(
                window.ClientRectangle.Left,
                window.ClientRectangle.Top,
                window.ClientRectangle.Width,
                window.ClientRectangle.Height);
        }
        public void ReflectPosition(System.Windows.Forms.Control window)
        {
            if (VideoWindow == null) return;

            VideoWindow.SetWindowPosition(
                window.ClientRectangle.Left,
                window.ClientRectangle.Top,
                window.ClientRectangle.Width,
                window.ClientRectangle.Height);
        }
        public void Run()
        {
            if (MediaControl != null)
                MediaControl.Run();
        }
        public void Pause()
        {
            if (MediaControl != null)
                MediaControl.Pause();
        }
        public void Stop()
        {
            if (MediaControl != null)
                MediaControl.Stop();
        }
        ~VideoContainer()
        {
            Clear();
        }
    }
}
