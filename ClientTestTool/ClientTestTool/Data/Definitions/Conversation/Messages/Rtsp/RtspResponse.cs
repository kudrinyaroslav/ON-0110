///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using ClientTestTool.Data.Conversations.Enums;
﻿using ClientTestTool.Data.Definitions.Conversation.Extensions;
﻿using ClientTestTool.Data.Definitions.Trace;
﻿using ClientTestTool.Data.Enums;
﻿using ClientTestTool.GUI.Logging;
﻿using ClientTestTool.Parsers.HeaderParser;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp
{
  public class RtspResponse : RtspMessage
  {
    public RtspResponse(Conversation conversation)
      : base(null, conversation, 0, MessageType.Response)
    {

    }

    public RtspResponse(Frame frame, Conversation conversation, int cSeq)
      : base(frame, conversation, cSeq, MessageType.Response)
    {
    }

    #region Properties

    public String StatusCode
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).StatusCode;
      }
    }

    public String[] MIMETypes
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).MIMETypes;
      }
    }

    public String[] MediaTypes
    {
      get
      {
        return new RtspHeaderParser(new StringReader(GetRtspString())).MediaTypes;
      }
    }

    #endregion

    public override String GetDetails()
    {
      return String.Empty;
    }

    public override void Validate()
    {
      try
      {
        int statusCode = int.Parse(StatusCode);

        switch (statusCode)
        {
          case 200:
            ValidationStatus = ValidationStatus.Passed;
            break;
          case 401:
            ValidationStatus = this.ValidateDigest();
            break;
          default:
            ValidationStatus = ValidationStatus.Failed;
            break;
        }
      }
      catch (FormatException e)
      {
        Logger.LogException("Exception thrown while validating RtspResponse", e);
        ValidationError = e.Message;
        ValidationStatus = ValidationStatus.Failed;
      }

      Validated = true;
    }
  }
}
