using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTool.GUI.Forms
{
    public partial class BlinkingPanel : UserControl
    {
        public BlinkingPanel()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }
        
        Color _blinkColor = Color.DarkRed;
        public Color BlinkColor
        {
            get { return _blinkColor; }
            set { _blinkColor = value; }
        }

        bool _colored = true;
        private void timerBlink_Tick(object sender, EventArgs e)
        {
            panelBackground.BackColor = _colored ? _blinkColor : SystemColors.ButtonFace;
            _colored = !_colored;
        }

        public bool TimerEnabled
        {
            get { return timerBlink.Enabled; }
            set { timerBlink.Enabled = value; }
        }
    }
}
