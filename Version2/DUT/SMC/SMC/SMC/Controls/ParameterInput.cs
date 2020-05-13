using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMC.Controls
{

    public abstract class ParameterInput : UserControl
    {
        public ParameterInput(string parameterName)
        {
            _name = parameterName;
        }

        private string _name;
        public string ParameterName
        {
            get { return _name; }
        }

        public abstract string GetText();

    }

    public partial class ParameterInput<T> : ParameterInput
        where T : Control
    {
        public ParameterInput(string parameterName, string displayName, T ctrl)
            :base(parameterName)
        {
            InitializeComponent();

            lblParameterName.Text = displayName;
            _input = ctrl;
            tlp.Controls.Add(ctrl);
        }

        private T _input;

        public T Input
        {
            get { return _input; }
        }

        public string DisplayName
        {
            get { return lblParameterName.Text; }
            set { lblParameterName.Text = value;}
        }
        
        public string InputText
        {
            get
            {
                CheckBox chk = _input as CheckBox;
                if (chk != null)
                {
                    return chk.Checked.ToString().ToLower();
                }
                else
                {
                    return _input.Text;
                }
            }
        }



        public override string GetText()
        {
            return InputText;
        }
    }
}
