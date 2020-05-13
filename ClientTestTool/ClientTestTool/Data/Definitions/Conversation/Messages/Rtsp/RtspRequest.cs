///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Trace;
﻿using ClientTestTool.Data.Enums;
﻿using ClientTestTool.Parsers.HeaderParser;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp
{
  public class RtspRequest : RtspMessage
  {
    public RtspRequest(Frame frame, Conversation conversation, int cSeq, RtspMethod method)
      : base(frame, conversation, cSeq, MessageType.Request)
    {
      Method = method;
    }

    #region Properties

    public RtspMethod Method
    {
      get;
      private set;
    }

    public String Uri
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).Uri;
      }
    }

    public String Transport
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).Transport;
      }
    }

    public String Range
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).Range;
      }
    }

    public String Session
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).Session;
      }
    }

    public String Request
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).Request;
      }
    }

    public String Require
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).Require;
      }
    }

    public String Scale
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).Scale;
      }
    }

    #endregion

    public override String GetDetails()
    {
      return Method.ToString();
    }

    public override void Validate()
    {
      Validated = true;
      ValidationStatus = ValidationStatus.Passed;
    }
  }
}
