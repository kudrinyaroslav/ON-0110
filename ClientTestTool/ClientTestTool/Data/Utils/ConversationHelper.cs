///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Trace;

namespace ClientTestTool.Data.Utils
{
  public static class ConversationHelper
  {
    public static String GetConversationFolder(NetworkTraceInfo trace, Conversation conversation)
    {
      if (null == conversation)
        throw new ArgumentNullException();

      String client = conversation.Client.Mac.Replace(":", String.Empty); // replacing invalid filename characters
      String device = conversation.Device.Mac.Replace(":", String.Empty);

      String folderName = String.Format("{0}_{1}", client, device);
      return Path.Combine(trace.Parser.ConversationsFolder, folderName);
    }
  }
}
