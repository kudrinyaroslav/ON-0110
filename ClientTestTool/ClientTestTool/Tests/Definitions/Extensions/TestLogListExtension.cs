///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.Tests.Definitions.Extensions
{
  public static class TestLogListExtension
  {
    public static TestStatus GetAverageTestStatus(this IList<TestLog> list)
    {
      if (0 == list.Count)
        return TestStatus.NotDetected;

      if (list.All(item => TestStatus.NotDetected == item.TestStatus))
        return TestStatus.NotDetected;

      if (list.All(item => TestStatus.Passed == item.TestStatus))
        return TestStatus.Passed;

      if (list.Any(item => TestStatus.Passed == item.TestStatus) && list.All(item => TestStatus.Failed != item.TestStatus))
        return TestStatus.Passed;

        return TestStatus.Failed;
    }
  }
}
