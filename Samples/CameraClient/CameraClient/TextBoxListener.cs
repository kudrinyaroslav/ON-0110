using System;
using System.Windows.Forms;

namespace CameraClient
{
    class TextBoxListener : IListener, CameraClient.TrafficTrace.ITrafficListener
    {
        private RichTextBox _textBox;
        public TextBoxListener(RichTextBox textBox)
        {
            _textBox = textBox;
        }
        
        #region IListener Members

        public void  WriteLine(string message)
        {
            WriteLine(message, InformationType.Service);
        }

        public void WriteLine(string message, InformationType type)
        {
            int begin = _textBox.Text.Length;
            _textBox.AppendText(message + Environment.NewLine);
            int end = _textBox.Text.Length;
            _textBox.Select(begin, end - begin);

            System.Drawing.Color color = _textBox.ForeColor;
            switch (type)
            {
                case InformationType.Request:
                    {
                        color = System.Drawing.Color.DarkGreen;
                    }
                    break;
                case InformationType.Error:
                    {
                        color = System.Drawing.Color.Red;
                    }
                    break;
                case InformationType.Response:
                    {
                        color = System.Drawing.Color.Blue;
                    }
                    break;
            }

            _textBox.SelectionColor = color;
            _textBox.Invalidate();
            _textBox.Select(0, 0);
        }


        #endregion

        #region ITrafficListener Members

        public void LogRequest(string request)
        {
            WriteLine("Request: ");
            WriteLine(request, InformationType.Request);
            WriteLine(Environment.NewLine);
        }

        public void LogResponse(string response)
        {
            WriteLine("Response: ");
            WriteLine(response, InformationType.Response);
            WriteLine(Environment.NewLine);
        }

        #endregion
    }
}
