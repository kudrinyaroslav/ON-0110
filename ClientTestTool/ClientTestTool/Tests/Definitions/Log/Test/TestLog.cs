///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Conversation;

namespace ClientTestTool.Tests.Definitions.Log.Test
{
  public class TestLog
  {
    public TestLog()
    {
      mConversationLogs = new List<ConversationLog>();
    }

    private List<ConversationLog> mConversationLogs; 

    public List<ConversationLog> ConversationLogs
    {
      get
      {
        return mConversationLogs.ToList();
      }
    }

    public void Add(ConversationLog log)
    {
      mConversationLogs.Add(log);
      mConversationLogs = mConversationLogs.OrderBy(item => item.TestStatus).ToList();
    }

    public void Clear()
    {
      mConversationLogs.Clear();
    }

    public ConversationLog LogForConversation(Conversation conversation)
    {
      if (null == conversation)
        throw new ArgumentNullException("conversation");

      return ConversationLogs.SingleOrDefault(item => item.Conversation == conversation);
    }
  }

  

}
