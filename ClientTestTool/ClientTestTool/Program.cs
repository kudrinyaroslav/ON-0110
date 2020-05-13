///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ClientTestTool.GUI.Forms;
﻿using ClientTestTool.GUI.MessageFilters;
﻿using ClientTestTool.GUI.Utils;

namespace ClientTestTool
{
  static class Program
  {
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      bool createdNew;
      using (var mutex = new Mutex(true, Properties.Settings.Default.ToolName, out createdNew))
      {
        if (createdNew)
        {
          Application.AddMessageFilter(new MouseWheelMessageFilter());

          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);
          Application.Run(new MainForm());
        }
        else
        {
          Process current = Process.GetCurrentProcess();
          foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            if (process.Id == current.Id)
            {
              String message = String.Format("Another instance of the {0} is already running.",
                Properties.Settings.Default.ToolName);

              DialogHelper.ShowError(message);

              SetForegroundWindow(process.MainWindowHandle);
              break;
            }
        }
      }
    }
  }
}
