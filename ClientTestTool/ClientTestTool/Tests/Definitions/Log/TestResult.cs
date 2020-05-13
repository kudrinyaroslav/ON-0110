///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Tests.Definitions.Data;

namespace ClientTestTool.Tests.Definitions.Log
{
    /// <summary>
    /// Test execution result.
    /// </summary>
    public class TestResult
    {
      public TestResult(TestInfo info, TestLog log)
      {
        TestInfo = info;
        Log = log;
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
