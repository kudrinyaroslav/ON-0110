///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.TestEngine;

namespace TestTool.GUI.Utils
{
    public partial class VideoWindow : Form, IVideoForm
    {
        private VideoContainer VideoCont = new VideoContainer();
        private System.Windows.Forms.Panel VideoPanel;

        //private Form _ownerWindow;
        public VideoWindow(Form ownerWindow)
        {
            //_ownerWindow = ownerWindow;
            //Owner = ownerWindow;
            InitializeComponent();
        }

        #region IVideoForm Members

        public string[] ActiveFilters { get { return VideoCont.ActiveFilters;  } }

        public int VideoWidth
        {
            get { return VideoCont.VideoWidth; }
            set { VideoCont.VideoWidth = value; }
        }
        public int VideoHeight
        {
            get { return VideoCont.VideoHeight; }
            set { VideoCont.VideoHeight = value; }
        }

        public string Address
        {
            get { return VideoCont.Address; }
            set { VideoCont.Address = value; }
        }

        public string User
        {
            get { return VideoCont.User; }
            set { VideoCont.User = value; }
        }

        public string Password
        {
            get { return VideoCont.Password; }
            set { VideoCont.Password = value; }
        }

        public int HTTPPort
        {
            get { return VideoCont.HTTPPort; }
            set { VideoCont.HTTPPort = value; }
        }

        public bool TCP
        {
            get { return VideoCont.TCP; }
            set { VideoCont.TCP = value; }
        }

        public int Timeout
        {
            get { return VideoCont.Timeout; }
            set { VideoCont.Timeout = value; }
        }

        public bool OPTIONS
        {
            get { return VideoCont.OPTIONS; }
            set { VideoCont.OPTIONS = value; }
        }

        public bool KEEPALIVE
        {
            get { return VideoCont.KEEPALIVE; }
            set { VideoCont.KEEPALIVE = value; }
        }
        
        public bool Video
        {
            get { return VideoCont.Video; }
        }
        
        public bool Audio
        {
            get { return VideoCont.Audio; }
        }

        string LastLogLine = null;
        public bool GetLogLine(out string Line)
        {
            bool ret = VideoCont.GetLogLine(out LastLogLine);
            Line = LastLogLine;
            return ret;
        }
        protected bool PeekTillError(out string Line)
        {
            DateTime Till = DateTime.Now.AddSeconds(1);
            while (VideoCont.GetLogLine(out Line) && (DateTime.Now < Till))
            {
                if (Line.Substring(0, 5) == "ERROR") return true;
            }
            return false;
        }


        public bool IsOptionsOK()
        {
            string Line;
            if (!PeekTillError(out Line)) return true;
            return Line != "ERROR in OPTIONS";
        }

        public bool IsDescribeOK()
        {
            string Line;
            if (!PeekTillError(out Line)) return true;
            return Line != "ERROR in DESCRIBE";
        }

        public bool IsSetupOK()
        {
            string Line;
            if (!PeekTillError(out Line)) return true;
            return Line != "ERROR in SETUP";
        }

        public bool IsPlayOK()
        {
            string Line;
            if (!PeekTillError(out Line)) return true;
            return Line != "ERROR in PLAY";
        }

        public bool IsTeardownOK()
        {
            string Line;
            if (!PeekTillError(out Line)) return true;
            return Line != "ERROR in TEARDOWN";
        }
        public void DropLogging()
        {
            string Line;
            DateTime Till = DateTime.Now.AddSeconds(2);
            while (VideoCont.GetLogLine(out Line) && (DateTime.Now < Till)) ;
        }

        public void OpenWindow(bool WithAudio)
        {
            VideoCont.UseAudio = WithAudio;
            VideoCont.Connect();
            //if (!Video)
            //    throw new DutPropertiesException("No compatible video decoder found");
            //if (WithAudio && !Audio)
            //    throw new DutPropertiesException("No compatible audio decoder found");

#if false
            VideoCont.Run();
            Invoke(new Action(() =>
            {
                VideoCont.ReflectPosition(this);
                BringToFront();
                Visible = true;
            }));

#else
#if false
            Invoke(new Action(() => 
            {
                VideoCont.ReflectPosition(this);
                //Video.AttachWindow(VideoPanel);
                BringToFront();
                Visible = true; }));
#else
            VideoCont.SetupWindow();
#endif
            VideoCont.Run();
#endif
        }

        public void CloseWindow()
        {
            //Invoke(new Action(() => { Visible = false;}));
            VideoCont.Clear();
            //VideoCont.Stop();
        }

        #endregion

        private void VideoWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }
    }
}
