///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Text;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Parsers.Frames;
using ClientTestTool.Parsers.HeaderParser;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Http
{
  public abstract class HttpMessage : XmlContainer
  {
    protected HttpMessage(Frame frame, Conversation conversation, MessageType type)
      : base(frame, conversation, type, ContentType.Http)
    {
    }

    #region Properties

    public String AuthHeader
    {
      get
      {
        var parser = new HttpHeaderParser(new StringReader(GetHttpString()));

        if (MessageType.Request == Type)
          return parser.RequestAuthHeader;

        return parser.ResponseAuthHeader;
      }
    }

    #endregion

    /// <summary>
    /// Whole content of the message (http + xml)
    /// </summary>
    /// <returns></returns>
    public override String GetContent()
    {
      if (null == mFrame)
        return String.Empty;

      var sb = new StringBuilder();
      sb.AppendLine(String.Format("Frame:{0}", FrameNumber));
      sb.AppendLine();
      sb.Append(GetHttpString());
      sb.AppendLine();
      sb.AppendLine();
      sb.Append(GetXmlString());

      return sb.ToString();
    }

    /// <summary>
    /// Http content
    /// </summary>
    protected String GetHttpString()
    {
      if (null == mFrame)
        return String.Empty;

      return new FrameLoader(mConversation, mFrame, Type).GetContent(ContentType.Http);
    }

    public override String ToString()
    {
      return GetContent();
    }
  }
}
