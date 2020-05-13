///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Worker;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Utils;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.Tests.Engine
{
  public class TestExecutor : Worker
  {
    internal TestExecutor()
      : base(ApplicationState.TestRunning)
    {
      TestUnderExecution = null;
    }

    #region Logic

    public override void Run()
    {
      Run(() => Run(TestCaseMapper.GetMappedTests()));
    }

    private void Run(Dictionary<Conversation, HashSet<TestInfo>> mappedTests)
    {
      var testList = new HashSet<TestInfo>(mappedTests.Values.SelectMany(item => item.ToList())).ToList().OrderedList();

      for (int i = 0; i < testList.Count; ++i)
      {
        var testInfo = testList[i];
        TestUnderExecution = testInfo;

        var conversations = mappedTests.Keys.Where(key => mappedTests[key].Contains(testInfo));

        testInfo.Test.Start(conversations);

        Progress = i * 100 / testList.Count;
      }

      TestUnderExecution = null;
    }

    #endregion

    #region Properties

    public TestInfo TestUnderExecution
    {
      get;
      private set;
    }

    #endregion
  }
}
