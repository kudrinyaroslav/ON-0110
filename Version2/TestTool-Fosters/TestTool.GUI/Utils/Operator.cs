///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.Tests.Definitions.Interfaces;
using System.Threading;
using TestTool.Tests.Definitions.Data;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// IOperator implementation.
    /// </summary>
    class Operator : IOperator
    {
        private delegate bool GetAnswerDelegate(string question);

        private Form _ownerWindow;

        /// <summary>
        /// Creates Operator instance.
        /// </summary>
        /// <param name="ownerWindow">Owner window.</param>
        public Operator(Form ownerWindow)
        {
            _ownerWindow = ownerWindow;
        }

        #region IOperator Members
        
        /// <summary>
        /// Get video source configuration token from operator
        /// </summary>
        /// <returns></returns>
        public string GetVideoConfigurationToken(List<string> tokens)
        {
            string token = null;
            _ownerWindow.Invoke(new Action(() =>
            {
                VideoSourceForm form = new VideoSourceForm();
                form.Sources.AddRange(tokens);
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog(_ownerWindow) == DialogResult.OK)
                {
                    token = form.VideoSourceToken;
                }
            }));
            return token;
        }

        /// <summary>
        /// Gets yes/no answer from operator.
        /// </summary>
        /// <param name="question"></param>
        /// <returns>True, if YES answer has been selected.</returns>
        public bool GetYesNoAnswer(string question)
        {
            IAsyncResult result = _ownerWindow.BeginInvoke(new GetAnswerDelegate(GetYesNo), question);
            return (bool)_ownerWindow.EndInvoke(result);
        }

        /// <summary>
        /// Displays message box with Yes/No buttons
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        bool GetYesNo(string question)
        {
            DialogResult result = MessageBox.Show(_ownerWindow, question, "Question", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            return (result == DialogResult.Yes);
        }
        
        /// <summary>
        /// Gets OK/Cancel answer from operator
        /// </summary>
        /// <param name="question"></param>
        /// <returns>True, if OK button has been selected.</returns>
        public bool GetOkCancelAnswer(string question)
        {
            IAsyncResult result = _ownerWindow.BeginInvoke(new GetAnswerDelegate(GetOkCancel), question);
            return (bool)_ownerWindow.EndInvoke(result);
        }

        /// <summary>
        /// Displays messagebox with OK/Cancel buttons.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        bool GetOkCancel(string question)
        {
            DialogResult result = MessageBox.Show(_ownerWindow, question, "Interaction required", MessageBoxButtons.OKCancel);

            return (result == DialogResult.OK);
        }
        
        public bool GetDelayTime(string prompt, ref int timeout)
        {
            bool bOK = false;
            int localTimeout = timeout;
            _ownerWindow.Invoke(new Action(() =>
            {
                SubscriptionTimeoutForm form = new SubscriptionTimeoutForm();
                form.Text = "Define Delay Time";
                form.Prompt = "Enter Delay Time for test (in seconds):";
                form.EventAction = "WARNING: You must check relay output state during this time.";
                form.Timeout = localTimeout.ToString();
                form.StartPosition = FormStartPosition.CenterParent;
                bOK = (form.ShowDialog(_ownerWindow) == DialogResult.OK);
                localTimeout = int.Parse(form.Timeout);
            }));

            timeout = localTimeout;
            return bOK;
        }

        Forms.MessageForm _messageForm;
        int _lastMessageFormX = -1;
        int _lastMessageFormY = -1;

        public WaitHandle ShowMessage(string message)
        {
            Func<Forms.BaseMessageForm> formInitializer =
                new Func<TestTool.GUI.Forms.BaseMessageForm>(
                    () =>
                    {
                        _messageForm = new TestTool.GUI.Forms.MessageForm();
                        _messageForm.Message = message;
                        return _messageForm;
                    });
            return DisplayBaseMessageForm(formInitializer);
        }

        public void HideMessage()
        {
            Action deinitialize = new Action(() => { _messageForm= null; });
            CloseForm(_messageForm, deinitialize);
        }

        Forms.CountdownMessageForm _countdownMessageForm;

        public WaitHandle ShowCountdownMessage(int timeout, DoorSelectionData data)
        {
            Func<Forms.BaseMessageForm> formInitializer =
                new Func<TestTool.GUI.Forms.BaseMessageForm>(
                    () =>
                    {
                        _countdownMessageForm = new TestTool.GUI.Forms.CountdownMessageForm();
                        _countdownMessageForm.Timeout = timeout/1000;
                        _countdownMessageForm.DoorSelectionData = data;
                        return _countdownMessageForm;
                    });

            return DisplayBaseMessageForm(formInitializer);
        }

        public void HideCountdownMessage()
        {
            Action deinitialize = new Action(() => { _countdownMessageForm = null; });
            CloseForm(_countdownMessageForm, deinitialize);
        }

        Forms.DoorSelectionMessageForm _doorSelectionForm;
        public WaitHandle ShowDoorSelectionMessage(DoorSelectionData data)
        {
            Func<Forms.BaseMessageForm> formInitializer =
                new Func<TestTool.GUI.Forms.BaseMessageForm>(
                    () => 
                    {
                        _doorSelectionForm = new TestTool.GUI.Forms.DoorSelectionMessageForm();
                        _doorSelectionForm.DoorSelectionData = data;                    
                        return _doorSelectionForm;
                    });
            return DisplayBaseMessageForm(formInitializer);
        }

        public void HideDoorSelectionMessage()
        {
            Action deinitialize = new Action(() => {_doorSelectionForm = null; });
            CloseForm(_doorSelectionForm, deinitialize);
        }

        void CloseForm(Forms.BaseMessageForm form, Action deinitialize)
        {
            Action action = new Action(
                () =>
                {
                    if (form != null)
                    {
                        _lastMessageFormY = form.Top;
                        _lastMessageFormX = form.Left;

                        form.Close();
                        deinitialize();
                    }
                });
            if (_ownerWindow.InvokeRequired)
            {
                _ownerWindow.Invoke(action);
            }
            else
            {
                action();
            }

        }

        WaitHandle DisplayBaseMessageForm(Func<Forms.BaseMessageForm> formInitializer)
        {
            Forms.BaseMessageForm form = null;
            Action action = new Action(
                () =>
                {
                    form = formInitializer();
                    if (_ownerWindow.WindowState == FormWindowState.Minimized)
                    {
                        _ownerWindow.WindowState = FormWindowState.Normal;
                    }
                    if (_lastMessageFormX > 0 && _lastMessageFormY > 0)
                    {
                        form.Top = _lastMessageFormY;
                        form.Left = _lastMessageFormX;
                    }
                    else
                    {
                        form.Top = _ownerWindow.Top + (_ownerWindow.Height - form.Height) / 2;
                        form.Left = _ownerWindow.Left + (_ownerWindow.Width - form.Width) / 2;
                    }
                    form.Show(_ownerWindow);
                    form.Focus();
                });
            if (_ownerWindow.InvokeRequired)
            {
                _ownerWindow.Invoke(action);
            }
            else
            {
                action();
            }
            return form.FormClosingEvent;
        }

        #endregion
    }
}
