using System;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Devices.Base;

namespace ClientTestTool.Data.Definitions.Interfaces
{
  interface IMessage
  {
    Conversation.Conversation Conversation
    {
      get;
    }

    Unit Sender
    {
      get;
    }

    bool IsEmpty
    {
      get;
    }

    int FrameNumber
    {
      get;
    }

    ContentType ContentType
    {
      get;
    }

    MessageType Type
    {
      get;
    }

    /// <summary>
    /// Method used in GUI to show message content
    /// </summary>
    String GetContent();

    /// <summary>
    /// Method used in GUI to show message details (main tag, rtsp method, etc)
    /// </summary>
    String GetDetails();
  }
}
