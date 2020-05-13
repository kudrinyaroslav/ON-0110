///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.Tests.Engine.Interfaces
{
  public interface ITest
  {
    #region Properties

    bool IsCompleted
    {
      get;
    }

    TestInfo TestInfo
    {
      get;
    }

    TestLog Log
    {
      get;
    }

    HashSet<RequestResponsePair> AffectedPairs
    {
      get;
    }

    #endregion

    void Start(IEnumerable<Conversation> conversations);
    void ClearTestResults();
  }
}