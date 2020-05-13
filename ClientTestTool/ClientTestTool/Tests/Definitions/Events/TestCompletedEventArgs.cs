///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Tests.Definitions.Log;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.Tests.Definitions.Events
{
  public class TestCompletedEventArgs : EventArgs
  {
    public TestCompletedEventArgs(TestResult result)
    {
      Result = result;
    }

    public TestResult Result;
  }
}
