using System;
using System.Windows.Forms;

namespace TestTool.Tests.Definitions.UI
{
#if false
    public abstract class SettingsTabPage<T>  : SettingsTabPage
            where T: class
    {
        public override Type ParametersType
        {
            get { return typeof (T); }
        }

    }
#else
    public class SettingsTabPage<T>  : SettingsTabPage
            where T: class
    {
        public override Type ParametersType
        {
            get { return typeof (T); }
        }
    
        public override string PageName
        {
            get { return string.Empty; }
        }

        public override int Order
        {
            get { return 0; }
        }

        public override void Clear()
        {
        }

        public override void Enable()
        {
        }

        public override void Disable()
        {
        }

        public override object Parameters
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
    }
#endif
}
