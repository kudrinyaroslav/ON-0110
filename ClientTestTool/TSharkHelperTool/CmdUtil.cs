using System;
using System.Diagnostics;
using System.IO;

namespace TSharkHelperTool
{
  /// <summary>
  /// Command-line helper
  /// </summary>
  internal static class CmdUtil
  {
    public static Process StartProcess(String path, String args)
    {
      var startInfo = new ProcessStartInfo(path, args)
      {
        UseShellExecute        = false,
        CreateNoWindow         = true,
        WindowStyle            = ProcessWindowStyle.Hidden,
        RedirectStandardOutput = true,
        RedirectStandardError  = true
      };

      return Process.Start(startInfo);
    }

    /// <summary>
    /// Launches command-line application
    /// </summary>
    /// <param name="path">path</param>
    /// <param name="args">arguments</param>
    /// <param name="output">application output</param>
    /// <returns>true on success</returns>
    public static bool LaunchApp(String path, String args, out String output)
    {
      output = String.Empty;
      var startInfo = new ProcessStartInfo(path, args) {
                                                         UseShellExecute        = false,
                                                         CreateNoWindow         = true,
                                                         WindowStyle            = ProcessWindowStyle.Hidden,
                                                         RedirectStandardOutput = true,
                                                         RedirectStandardError  = true
                                                       };

      try
      {
        using (Process executable = Process.Start(startInfo))
        {
          Debug.Assert(executable != null, "executable != null");

          //pass standart output to out variable
          using (StreamReader reader = executable.StandardOutput)
          {
            output = reader.ReadToEnd();
          }

          // show standart error to user if any
          using (StreamReader reader = executable.StandardError)
          {
            String error = reader.ReadToEnd();

            Console.WriteLine(error);
          }

          executable.WaitForExit(); 
        }

        return true;
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message);
        return false;
      }
    }

  }
}
