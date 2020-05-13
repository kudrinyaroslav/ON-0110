using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TestEngineProto
{
    class TestBoxListener : IListener
    {
        private TextBox _textBox;
        public TestBoxListener(TextBox textBox)
        {
            _textBox = textBox;
        }
        
        #region IListener Members

        public void  Write(string message)
        {
            _textBox.Text = _textBox.Text + message + Environment.NewLine;
        }

        #endregion
    }
}
