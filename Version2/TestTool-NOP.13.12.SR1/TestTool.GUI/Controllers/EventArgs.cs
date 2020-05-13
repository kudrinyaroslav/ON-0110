using System;
using System.Collections.Generic;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Argumnts for an event raised when settings are missing.
    /// </summary>
    public class SettingsMissingEventArgs : EventArgs
    {
        public List<string> Settings { get; set; }
    }
}
