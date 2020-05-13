///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Data.Conversations;

namespace ClientTestTool.Tests.Definitions.Log.Steps
{
  public sealed class Step
  {
    public Step(String name, int number)
    {
      Name         = name;
      Number       = number;
      mStepResults = new Dictionary<Conversation, StepResult>();
    }

    /// <summary>
    /// Step name
    /// </summary>
    public String Name
    {
      get;
      set;
    }

    /// <summary>
    /// Step number
    /// </summary>
    public int Number
    {
      get;
      set;
    }

    public StepResult GetResult(Conversation conversation)
    {
      if (null == conversation)
        throw new ArgumentNullException();

      if (!mStepResults.ContainsKey(conversation))
        return null;

      return mStepResults[conversation];
    }

    public void SetResult(Conversation conversation)
    {
      
    }

    private readonly Dictionary<Conversation, StepResult> mStepResults;
  }
}
