using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TestTool.GUI.Forms
{
    public class BaseMessageForm : Form
    {
        public BaseMessageForm()
        {
            _formClosingEvent = new AutoResetEvent(false);
            FormClosing += new FormClosingEventHandler(BaseMessageForm_FormClosing);
        }

        void BaseMessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formClosingEvent.Set();
        }        
        
        AutoResetEvent _formClosingEvent;

        public AutoResetEvent FormClosingEvent
        {
            get { return _formClosingEvent; }
        }



    }
}
