using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using TestTool.Tests.Definitions.Data;

namespace TestTool.GUI.Forms
{
    public partial class CountdownMessageForm : BaseMessageForm
    {
        public CountdownMessageForm()
        {
            InitializeComponent();

            blinkingPanel.TimerEnabled = true;
            this.FormClosing += new FormClosingEventHandler(MessageForm_FormClosing);           
        }

        int _timeout;
        public int Timeout
        {
            get 
            { 
                return _timeout; 
            }
            set 
            { 
                _timeout = value; 
                UpdateCountdown();
            }
        }

        DoorSelectionData _doorSelectionData;
        public DoorSelectionData DoorSelectionData
        {
            get 
            {
                return _doorSelectionData; }
            set 
            { 
                _doorSelectionData = value;
                cmbDoor.DisplayMember = "DisplayName";
                cmbDoor.ValueMember = "Token";
                cmbDoor.DataSource = _doorSelectionData.Doors;
                cmbDoor.SelectedValue = _doorSelectionData.SelectedToken;
                UpdateMessage();
            }
        }
        
        void UpdateMessage()
        {
            blinkingPanel.Message = string.Format(_doorSelectionData.MessageTemplate, _doorSelectionData.SelectedToken);
        }

        void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            blinkingPanel.TimerEnabled = false;
        }


        private void timerBlink_Tick(object sender, EventArgs e)
        {
            Timeout -= 1;
            if (Timeout == 0)
            {
                Close();
            }
        }

        void UpdateCountdown()
        {
            lblCountdown.Text = string.Format("Window will be closed automatically in {0} seconds", _timeout);
        }

        private void cmbDoor_SelectedIndexChanged(object sender, EventArgs e)
        {
            _doorSelectionData.SelectedToken = (string)cmbDoor.SelectedValue;
            UpdateMessage();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
