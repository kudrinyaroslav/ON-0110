///
/// @Author Matthew Tuusberg
///

﻿namespace ClientTestTool.Data.Conversations.Events
{
  public class ConversationElementEventArgs : ElementEventArgs
  {
    public ConversationElementEventArgs(int conversationIndex, int elementIndex) : base(elementIndex)
    {
      ConversationIndex = conversationIndex;
    }

    public int ConversationIndex
    {
      get;
      private set;
    }

  }
}
