///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;
using ClientTestTool.Parsers.NetworkTraceParser.Base;

namespace ClientTestTool.Parsers.NetworkTraceParser
{
  class OutputParser : BaseNTParser
  {
    public OutputParser(NetworkTraceInfo networkTrace) : base(networkTrace, ApplicationState.ParserRunning)
    {
    }

    public override void Run()
    {
      LoadFrameList(@"");

      foreach (var frame in FrameList)
      {
        Console.WriteLine(frame);
      }
    }

    protected override void ParseFrameList()
    {
      throw new NotImplementedException();
    }

    protected override void Parse()
    {
      throw new NotImplementedException();
    }
  }
}
