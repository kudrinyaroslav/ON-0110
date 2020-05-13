///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Extensions;

namespace ClientTestTool.Tests.Definitions.Log.Test
{
  /// <summary>
  /// Test execution result.
  /// </summary>
  public class TestResult
  {
    public TestResult(TestInfo info, TestLog log)
    {
      if (null == info)
        throw new ArgumentNullException("info");

      if (null == log)
        throw new ArgumentNullException("log");

      TestInfo = info;
      Log      = log;
    }

    public TestStatus TestStatus
    {
      get
      {
        return Log.ConversationLogs.GetAverageTestStatus();
      }
    }

    public TestInfo TestInfo
    {
      get;
      set;
    }

    public TestLog Log
    {
      get;
      private set;
    }
  }
}
