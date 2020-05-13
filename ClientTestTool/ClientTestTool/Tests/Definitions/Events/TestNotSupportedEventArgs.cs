///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.Tests.Definitions.Events
{
  public class TestNotSupportedEventArgs : EventArgs
  {
    public TestNotSupportedEventArgs(String message)
    {
      Message = message;
    }

    public String Message
    {
      get;
      private set;
    }
  }
}
