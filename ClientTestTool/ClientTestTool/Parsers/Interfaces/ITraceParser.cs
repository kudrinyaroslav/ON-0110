///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Parsers.NetworkTraceParser;

namespace ClientTestTool.Parsers.Interfaces
{
  public interface ITraceParser
  {
    NetworkTraceInfo NetworkTrace
    {
      get;
    }

    NTFrameList FrameList
    {
      get;
    }

    String ConversationsFolder
    {
      get;
    }

    String OutputFolder
    {
      get;
    }
  }
}
