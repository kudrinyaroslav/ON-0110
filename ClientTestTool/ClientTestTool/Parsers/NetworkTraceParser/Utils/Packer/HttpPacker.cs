///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Data.Utils;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.Packer
{
  internal static class HttpPacker
  {
    public static void Pack(Conversation conversation, Frame frame, MessageType type, String content)
    {
      if (null == conversation)
        throw new ArgumentNullException("conversation");

      if (null == frame)
        throw new ArgumentNullException("frame");

      String httpFilename = FrameHelper.GetFrameFilename(conversation, frame, type);
      String http         = TSharkHelper.GetProtocolDetails(content, TSharkHelper.HEADER_HTTP);

      File.WriteAllText(httpFilename, http);

      String folder   = FrameHelper.GetFrameFolder(conversation, frame, type);
      String filename = String.Join(".", Path.GetFileNameWithoutExtension(httpFilename), CTTSettings.EXTENSION_XML);

      String xmlFilename = Path.Combine(folder, filename);
      String xml         = TSharkHelper.GetProtocolDetails(content, TSharkHelper.HEADER_XML);

      File.WriteAllText(xmlFilename, xml);
    }
  }
}
