///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Utils;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.Packer
{
  internal static class RtspPacker
  {
    public static void Pack(Conversation conversation, Frame frame, MessageType type, String content)
    {
      if (null == conversation)
        throw new ArgumentNullException("conversation");

      if (null == frame)
        throw new ArgumentNullException("frame");

      content = TSharkHelper.GetFrameWithoutHeader(content);

      String[] lines = content.SplitToLines();
      String cSeqStr = lines.FirstOrDefault(item => item.Contains("CSeq"));

      if (null == cSeqStr)
        return;

      cSeqStr = cSeqStr.Substring(cSeqStr.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
      int cSeq = int.Parse(cSeqStr);

      switch (type)
      {
        case MessageType.Request:
          RtspMethod method = GetMethod(lines.First(item => item.Contains("Method")));
          conversation.Add(new RequestResponsePair(new RtspRequest(frame, conversation, cSeq, method), null, frame.FoundInTrace, conversation, ContentType.Rtsp));
          break;

        case MessageType.Response:
          var pair = conversation.GetMessages(ContentType.Rtsp).FirstOrDefault(item => item.GetRequest<RtspRequest>().CSeq == cSeq && item.Response.IsEmpty);

          if (null != pair)
            pair.SetResponse(new RtspResponse(frame, conversation, cSeq));
          break;
      }

      String filename = FrameHelper.GetFrameFilename(conversation, frame, type);

      File.WriteAllText(filename, content);
    }

    private static RtspMethod GetMethod(String input)
    {
      String methodStr = input.Substring(input.IndexOf(":", StringComparison.Ordinal) + 1).Trim().ToUpper();

      foreach (RtspMethod method in Enum.GetValues(typeof(RtspMethod)))
        if (method.ToString() == methodStr)
          return method;

      throw new ArgumentException();
    }
  }

}
