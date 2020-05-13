///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Diagnostics;
using System.IO;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Interfaces;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace TSharkHelperTool.TShark
{
  public class TSharkProcess : IDisposable
  {
    public TSharkProcess(IQuery query)
    {
      Run(query);
    }

    public StreamReader StandartOutput
    {
      get
      {
        return mTsharkProcess.StandardOutput;
      }
    }

    public StreamReader StandartError
    {
      get
      {
        return mTsharkProcess.StandardError;
      }
    }

    public void Dispose()
    {
      mTsharkProcess.Dispose();
    }

    private void Run(IQuery query)
    {
      var timer = new Stopwatch();
      timer.Start();

      String args = query.Build();

      mTsharkProcess = CmdUtil.StartProcess(TSharkHelper.GetTSharkPath(), args);

      timer.Stop();
      Console.WriteLine("TShark query:\"{0}\" {1} ms ellapsed", args, timer.ElapsedMilliseconds);
    }

    private Process mTsharkProcess = null;
  }
}
