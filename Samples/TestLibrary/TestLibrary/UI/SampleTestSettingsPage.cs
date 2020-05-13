using ProfilesTestLibrary.Parameters;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.UI;

namespace ProfilesTestLibrary.UI
{
    [SettingsControl(ParametersType = typeof(TestSettings))]
    public partial class SampleTestSettingsPage : SettingsTabPage<TestSettings>
    {
        public SampleTestSettingsPage()
        {
            InitializeComponent();
        }

        TestSettings _parameters = new TestSettings();

        public override string PageName
        {
            get
            {
                return "Extra page 1";
            }
        }

        void UpdateParameters()
        {
            _parameters.Option1 = rbChoice1.Checked;
            _parameters.Text1 = tbString1.Text;
        }

 
        public override int Order
        {
            get { return 0; }
        }

        public override void Clear()
        {
            
        }

        public override object Parameters
        {
            get
            {
                UpdateParameters();
                return _parameters;
            }
            set
            {
                _parameters = (TestSettings)value;
                if (_parameters != null)
                {
                    rbChoice1.Checked = _parameters.Option1;
                    rbChoice2.Checked = !_parameters.Option1;
                    tbString1.Text = _parameters.Text1;
                }
            }
        }

        public override void Enable()
        {
            
        }

        public override void Disable()
        {
            
        }
    }
}
