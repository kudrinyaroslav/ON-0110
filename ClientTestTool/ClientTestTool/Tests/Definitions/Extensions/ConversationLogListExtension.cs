///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.Tests.Definitions.Extensions
{
  public static class ConversationLogListExtension
  {
    public static TestStatus GetAverageTestStatus(this IEnumerable<ConversationLog> logs)
    {
      var logList = logs.ToList();

      if (0 == logList.Count)
        return TestStatus.NotDetected;

      if (logList.All(item => TestStatus.NotDetected == item.TestStatus))
        return TestStatus.NotDetected;

      if (logList.All(item => TestStatus.Passed == item.TestStatus))
        return TestStatus.Passed;

      if (logList.Any(item => TestStatus.Passed == item.TestStatus) && logList.All(item => TestStatus.Failed != item.TestStatus))
        return TestStatus.Passed;

        return TestStatus.Failed;
    }
  }
}
