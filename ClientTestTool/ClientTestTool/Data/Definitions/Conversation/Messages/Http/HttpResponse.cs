///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation.Enums;
﻿using ClientTestTool.Data.Definitions.Conversation.Extensions;
﻿using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Parsers.HeaderParser;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Http
{
  public sealed class HttpResponse : HttpMessage
  {
    public HttpResponse(Frame frame, Conversation conversation)
      : base(frame, conversation, MessageType.Response)
    {
    }

    #region Properties

    public String StatusCode
    {
      get
      {
        var parser = new HttpHeaderParser(new StringReader(GetHttpString()));
        return parser.StatusCode;
      }
    }

    #endregion

    #region Validation

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
        Logger.LogException("Exception thrown while validating HttpResponse", e);
        ValidationError  = e.Message;
        ValidationStatus = ValidationStatus.Failed;
      }

      Validated = true;
    }

    #endregion
  }
}
