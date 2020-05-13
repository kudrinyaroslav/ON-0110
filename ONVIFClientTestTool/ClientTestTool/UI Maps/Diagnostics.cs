namespace ClientTestTool.UIMaps.DiagnosticsClasses
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Input;
    using System.CodeDom.Compiler;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    public partial class Diagnostics
    {
        public Test test = new Test();
        public List<LogData> logData = new List<LogData>();
        public List<LogData> logDataFeatures = new List<LogData>();
        public LogFile logFile = new LogFile();
        public string testPath;
        public string treeName;
        public Dictionary<string, string> macAndDevice = new Dictionary<string,string>();

        public Diagnostics(Test test, string testPath)
        {
            this.test = test;
            this.testPath = testPath;
        }
    }
}
