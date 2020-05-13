using System;
using ProfilesTestLibrary.Parameters;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.UI;

namespace ProfilesTestLibrary.UI
{
    [SettingsControl(ParametersType = typeof(Parameters.SecurityTestSettings2))]
    public partial class TestSettingsPage2 : SettingsTabPage<Parameters.SecurityTestSettings2>
    {
        public TestSettingsPage2()
        {
            InitializeComponent();
            _parameters = new SecurityTestSettings2();
        }

        public override string PageName
        {
            get { return "Extra page 2"; }
        }
        
        public override int Order
        {
            get { return 1; }
        }

        public override void Clear()
        {
            
        }

        public override void Enable()
        {
            label1.Enabled = true;
        }

        public override void Disable()
        {
            label1.Enabled = false;
        }
        
        private Parameters.SecurityTestSettings2 _parameters;
        public override object Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                if (value is Parameters.SecurityTestSettings2)
                {
                    _parameters = (Parameters.SecurityTestSettings2)value;
                }
            }
        }
 
    }
}
