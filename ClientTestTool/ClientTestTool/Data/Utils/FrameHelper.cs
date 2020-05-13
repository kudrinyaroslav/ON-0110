///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Globalization;
using System.IO;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Data.Utils
{
  public static class FrameHelper
  {
    public static String GetFrameFolder(Conversation conversation, Frame frame, MessageType type)
    {
      if (null == conversation)
        throw new ArgumentNullException("conversation");

      if (null == frame)
        throw new ArgumentNullException("frame");

      String folder = Path.Combine(ConversationHelper.GetConversationFolder(frame.FoundInTrace, conversation), type.ToString(), frame.Number.ToString(CultureInfo.InvariantCulture));
      return Directory.CreateDirectory(folder).FullName;
    }

    public static String GetFrameFilename(Conversation conversation, Frame frame, MessageType type)
    {
      if (null == conversation)
        throw new ArgumentNullException("conversation");

      if (null == frame)
        throw new ArgumentNullException("frame");

      String folder   = GetFrameFolder(conversation, frame, type);
      String filename = String.Join(".", frame.Number.ToString(CultureInfo.InvariantCulture), GetFrameExtension(frame));

      return Path.Combine(folder, filename);
    }

    public static String GetFrameExtension(Frame frame)
    {
      switch (frame.Protocol)
      {
        case TSharkHelper.PROTOCOL_HTTP:
        case TSharkHelper.PROTOCOL_HTTP_XML:
          return CTTSettings.PROTOCOL_HTTP;

        case TSharkHelper.PROTOCOL_RTSP:
        case TSharkHelper.PROTOCOL_SDP:
          return CTTSettings.PROTOCOL_RTSP;

        case "ws-discovery":
          return CTTSettings.EXTENSION_XML;

        default:
          throw new ArgumentException("frame");
      }
    }
  }
}
