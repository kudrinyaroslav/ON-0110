///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.TestEngine;
using System.Windows.Forms;

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
        /// Get operation that requires authentication
        /// </summary>
        /// <returns>Name of operation</returns>
        public string GetSecureAPI()
        {
            string operation = null;
            _ownerWindow.Invoke(new Action(() =>
            {
                SecureAPIForm form = new SecureAPIForm();
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog(_ownerWindow) == DialogResult.OK)
                {
                    operation = form.Operation;
                }
            }));
            return operation;
        }
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

        public bool GetEventsTopic(List<EventsTopicInfo> predefinedFilters, out EventsTopicInfo topic)
        {
            bool bOK = false;
            EventsTopicInfo localTopic = null;
            _ownerWindow.Invoke(new Action(() =>
            {
                EventsTopicForm form = new EventsTopicForm();
                form.SetFilters(predefinedFilters);
                form.StartPosition = FormStartPosition.CenterParent;
                bOK = (form.ShowDialog(_ownerWindow) == DialogResult.OK);
                localTopic = form.Topic;
            }));

            topic = localTopic;
            return bOK;
        }

        public bool GetSubscriptionTimeout(string prompt, string eventAction, ref int timeout)
        {
            bool bOK = false;
            int localTimeout = timeout;
            _ownerWindow.Invoke(new Action(() =>
            {
                SubscriptionTimeoutForm form = new SubscriptionTimeoutForm();
                form.Prompt = prompt;
                form.Timeout = localTimeout.ToString();
                form.EventAction = eventAction;
                form.StartPosition = FormStartPosition.CenterParent;
                bOK = (form.ShowDialog(_ownerWindow) == DialogResult.OK);
                localTimeout = int.Parse(form.Timeout);
            }));

            timeout = localTimeout; 
            return bOK;
        }

        #endregion
    }
}
