///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation.Base;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Devices.Base;
﻿using ClientTestTool.Data.Definitions.Interfaces;
﻿using ClientTestTool.Data.Definitions.Trace;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Base
{
  public abstract class BaseMessage : ValidatableItem, IMessage
  {
    protected BaseMessage(Frame frame, Conversation conversation, MessageType type, ContentType contentType)
    {
      if (null != frame)
        FrameNumber = frame.Number;
        
      mDetails      = null;
      mConversation = conversation;
      mFrame        = frame;
      Type          = type;
      ContentType   = contentType;
    }

    #region Properies

    public Conversation Conversation
    {
      get
      {
        return mConversation;
      }
    }

    public Unit Sender
    {
      get
      {
        return MessageType.Request == Type ? mConversation.Client : mConversation.Device;
      }
    }

    public int FrameNumber
    {
      get;
      private set;
    }

    public ContentType ContentType
    {
      get;
      private set;
    }

    public MessageType Type
    {
      get;
      private set;
    }

    public bool IsEmpty
    {
      get
      {
        return 0 == FrameNumber;
      }
    }

    #endregion

    /// <summary>
    /// Method used in GUI to show message content
    /// </summary>
    public abstract String GetContent();

    /// <summary>
    /// Method used in GUI to show message details (main tag, rtsp method, etc)
    /// </summary>
    public abstract String GetDetails();

    protected          String       mDetails;
    protected readonly Conversation mConversation;
    protected readonly Frame        mFrame;

  }
}