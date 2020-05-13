using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Xml;
using System.IO;
using System.Reflection;
using Vlc.DotNet.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;

namespace RTSPClient
{
    public partial class Form1 : Form
    {
        private VlcControl vlcControl;
        private StreamingClient rtspClient; 

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            vlcControl = new VlcControl();
            vlcControl.Width = 0;
            vlcControl.Height = 0;
            vlcControl.Manager.AutoStart = true;
            vlcControl.Manager.FullScreen = false;
            vlcControl.Manager.MediaLibrary.Loop = false;
            vlcControl.Manager.MediaLibrary.Random = false;

            this.Controls.Add(vlcControl);
        }

        private void buttonStartVLC_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri(textIpAddress.Text);
            rtspClient = new StreamingClient();
            rtspClient.Connect(uri);
            string rtp = rtspClient.Play();

            //vlcControl.Manager.VlcLibPath = textVLCFolder.Text;
            //vlcControl.Manager.MediaLibrary.MediaItems.Add(new CustomMedia(rtp));
            //vlcControl.Manager.Play();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            //vlcControl.Manager.Stop();
            //vlcControl.Manager.MediaLibrary.MediaItems.Clear();
            rtspClient.Stop();
            rtspClient.Disconnect();
        }
        private void buttonBrowseVLC_Click(object sender, EventArgs e)
        {
            vlcFolderBrowserDialog.SelectedPath = textVLCFolder.Text;
            vlcFolderBrowserDialog.ShowDialog();
            textVLCFolder.Text = vlcFolderBrowserDialog.SelectedPath;
        }
    }
}
