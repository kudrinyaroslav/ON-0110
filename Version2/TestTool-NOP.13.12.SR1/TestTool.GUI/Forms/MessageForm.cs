using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TestTool.GUI.Forms
{
    public partial class MessageForm : BaseMessageForm
    {
        public MessageForm()
        {
            InitializeComponent();

            blinkingPanel.TimerEnabled = true;
            this.FormClosing += new FormClosingEventHandler(MessageForm_FormClosing);           
        }

        void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            blinkingPanel.TimerEnabled = false;
        }

        public string Message
        {
            get { return blinkingPanel.Message; }
            set { blinkingPanel.Message = value; }
        }

    }
}
