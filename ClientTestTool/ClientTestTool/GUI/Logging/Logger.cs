///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using ClientTestTool.Properties;

namespace ClientTestTool.GUI.Logging
{
  /// <summary>
  /// CTT logging handler
  /// </summary>
  public static class Logger
  {
    private class FileLogger
    {
      internal FileLogger(String filename)
      {
        mFilename       = filename;
        mIsAccessDenied = IsAccessDenied();
      }

      private static readonly object mLocker = new Object();

      private readonly String mFilename;
      private readonly bool   mIsAccessDenied;

      public void WriteLine(String message)
      {
        Console.WriteLine(message);

        if (mIsAccessDenied)
          return;

        lock (mLocker)
        {
          using (var file = new FileStream(mFilename, FileMode.Append, FileAccess.Write, FileShare.Read))
          using (var writer = new StreamWriter(file, Encoding.Unicode))
          {
            writer.WriteLine("{0} : {1}", DateTime.Now, message);
          }
        }
      }

      private bool IsAccessDenied()
      {
        if (String.IsNullOrEmpty(mFilename))
          return true;

        try
        {
          var stream = new FileStream(mFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
          stream.SetLength(0);
          stream.Close();

          return false;
        }
        catch
        {
          return true;
        }
      }
    }

    private static HashSet<String>     mErrors = null;
    private static readonly FileLogger mLogger = new FileLogger(Settings.Default.LogFilename);

    public static void AddError(String error)
    {
      if (String.IsNullOrEmpty(error))
        return;

      if (null == mErrors)
        mErrors = new HashSet<String>();

      mErrors.Add(error);
    }

    public static void ShowErrorList()
    {
      if (null == mErrors)
        return;

      String msg = String.Format("{0} errors:{1}", mErrors.Count, Environment.NewLine);

      msg = mErrors.Aggregate(msg, (current, error) => current + String.Format("{0}{1}", error, Environment.NewLine));

      MessageBox.Show(msg);

      mLogger.WriteLine(msg);

      mErrors.Clear();
    }

    public static void WriteLine(String text, [CallerFilePath] String filename = "", [CallerLineNumber] int lineNumber = 0)
    {
      String message = String.Format("{0}{1}at {2}: line {3}", text, Environment.NewLine, filename, lineNumber);

      Console.WriteLine(message);
      mLogger.WriteLine(text);
    }

    public static void LogException(String message, Exception e)
    {
      message = String.Format("{0}{1}Exception:{2}", message, Environment.NewLine, e.Message);

      Console.WriteLine(message);
      mLogger.WriteLine(message);
    }
  }
}
