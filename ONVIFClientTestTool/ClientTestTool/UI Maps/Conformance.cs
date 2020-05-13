namespace ClientTestTool.UIMaps.ConformanceClasses
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
    
    
    public partial class Conformance
    {
        public Test test = new Test();
        public List<LogData> logDataProfiles = new List<LogData>();
        public string testPath;

        public Conformance(Test test, string testPath)
        {
            this.test = test;
            this.testPath = testPath;
        }
    }
}
