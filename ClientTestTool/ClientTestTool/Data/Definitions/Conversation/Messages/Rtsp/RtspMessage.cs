///
/// @Author Matthew Tuusberg
///

﻿using System;
﻿using System.IO;
﻿using System.Text;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Parsers.Frames;
﻿using ClientTestTool.Parsers.HeaderParser;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp
{
  public abstract class RtspMessage : BaseMessage
  {
    protected RtspMessage(Frame frame, Conversation conversation, int cSeq, MessageType type)
      : base(frame, conversation, type, ContentType.Rtsp)
    {
      CSeq = cSeq;
    }

    #region Properties

    public int CSeq
    {
      get;
      private set;
    }

    public String AuthHeader
    {
      get
      {
        var parser = new HttpHeaderParser(new StringReader(GetRtspString()));

        if (MessageType.Request == Type)
          return parser.RequestAuthHeader;

        return parser.ResponseAuthHeader;
      }
    }

    #endregion

    public String GetRtspString()
    {
      if (null == mFrame)
        return String.Empty;

      return new FrameLoader(mConversation, mFrame, Type).GetContent(ContentType.Rtsp);
    }

    public override String GetContent()
    {
      var sb = new StringBuilder();
      sb.AppendLine(String.Format("Frame:{0}", FrameNumber));
      sb.AppendLine();
      sb.AppendLine(GetRtspString());

      return sb.ToString();
    }
  }
}
