///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Text;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Discovery
{
  public class DiscoveryMessage : XmlContainer
  {
    public DiscoveryMessage(Frame frame, Conversation conversation, MessageType type) : base(frame, conversation, type, ContentType.WSDiscovery)
    {
    }

    public override void Validate()
    {
      Validated        = true;
      ValidationStatus = ValidationStatus.Passed;
    }

    public override String GetContent()
    {
      var sb = new StringBuilder();
      sb.AppendLine(String.Format("Frame:{0}", FrameNumber));
      sb.AppendLine();
      sb.Append(GetXmlString());
      return sb.ToString();
    }
  }
}
