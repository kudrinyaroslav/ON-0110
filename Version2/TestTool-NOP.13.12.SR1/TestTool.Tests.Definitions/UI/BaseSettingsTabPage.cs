using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTool.Tests.Definitions.UI
{
    public abstract class SettingsTabPage : UserControl
    {
        public abstract string PageName { get; }
        public abstract int Order { get; }
        public abstract void Clear();
        public abstract void Enable();
        public abstract void Disable();

        public abstract object Parameters { get; set; }
        public abstract Type ParametersType { get; }
    }
}
