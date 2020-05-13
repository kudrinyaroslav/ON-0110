///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Tests.Definitions.Log.Steps;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.Tests.Definitions.Events
{
  public class TestLogEventArgs : EventArgs
  {
    public TestLogEventArgs(TestLog log)
    {
      Log = log;
    }

    public TestLog Log;
  }
}
