///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Engine;
using ClientTestTool.Tests.Engine.Builder;

namespace ClientTestTool.Tests.Definitions.TestCaseSet.Base
{
  public abstract class BaseTestCaseSet
  {
    protected BaseTestCaseSet()
    {
      TestExecutor = new TestExecutor();
      mTests = new Lazy<List<TestInfo>>(() => new TestInfoListBuilder().Build());
    }

    #region Properties

    public TestExecutor TestExecutor
    {
      get;
      private set;
    }

    #endregion

    protected Lazy<List<TestInfo>> mTests;
  }
}
