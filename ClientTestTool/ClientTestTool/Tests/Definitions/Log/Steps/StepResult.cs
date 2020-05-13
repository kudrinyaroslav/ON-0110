///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.Tests.Definitions.Log.Steps
{
  /// <summary>
  /// Step results.
  /// </summary>
  public class StepResult 
  {
    public StepResult(int number, String message, IEnumerable<RequestResponsePair> affectedPairs, MessageType type)
    {
      Number        = number;
      Message       = message;
      Type          = type;
      AffectedPairs = new List<RequestResponsePair>(affectedPairs);

      Name          = String.Format("[S{0}]", Number);
    }

    public int Number
    {
      get;
      private set;
    }

    public String Name
    {
      get;
      private set;
    }

    public String Message
    {
      get;
      set;
    }

    public List<RequestResponsePair> AffectedPairs
    {
      get;
      private set;
    }

    public MessageType Type
    {
      get;
      private set;
    }

    public StepStatus Status
    {
      get;
      set;
    }

    public String ErrorMessage
    {
      get;
      set;
    }

    public Exception Exception
    {
      get;
      set;
    }
  }
}
