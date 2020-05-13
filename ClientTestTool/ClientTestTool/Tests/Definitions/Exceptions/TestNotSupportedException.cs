///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.Tests.Definitions.Exceptions
{
  public class TestNotSupportedException : Exception
  {
    public TestNotSupportedException(String message) : base(message)
    {}

  }
}
