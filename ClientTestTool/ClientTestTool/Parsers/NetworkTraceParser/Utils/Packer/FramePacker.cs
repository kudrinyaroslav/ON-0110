///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.Packer
{
  public class FramePacker
  {
    public FramePacker(NTFrameList frameList)
    {
      if (null == frameList)
        throw new ArgumentNullException("framelist");

      mFrameList = frameList;
    }

    public bool PackFrame(String content)
    {
      int frameNumber = GetFrameNumber(content);

      if (-1 == frameNumber)
        return false;

      Frame frame = mFrameList.GetFrame(frameNumber);

      if (null == frame)
        return false;

      //content = TSharkHelper.GetFrameWithoutHeader(content);

      //frame.Content = content;

      //return true;

      Conversation conversation = FindConversation(frame, frame.Protocol);

      if (null == conversation)
        return false;
     

      MessageType type = GetMessageType(conversation, frame);

      switch (frame.Protocol)
      {
        case TSharkHelper.PROTOCOL_RTSP:
        case TSharkHelper.PROTOCOL_SDP:
          RtspPacker.Pack(conversation, frame, type, content);
          break;

        case TSharkHelper.PROTOCOL_HTTP:
        case TSharkHelper.PROTOCOL_HTTP_XML:
          HttpPacker.Pack(conversation, frame, type, content);
          break;

        default:
          return false;
      }

      return true;
    }

    private MessageType GetMessageType(Conversation conversation, Frame frame)
    {
      MessageType? result;
      conversation.ContainsFrame(frame, out result);
      if (null != result)
        return result.Value;

      throw new ArgumentException("frame");
    }

    private Conversation FindConversation(Frame frame, String protocol)
    {
      if (protocol.Contains(TSharkHelper.PROTOCOL_RTSP)) // HACK //TODO
        return
          ConversationList.GetConversations(
            item => (item.Client.Mac == frame.SourceMac      && item.Device.Mac == frame.DestinationMac) ||
                     item.Client.Mac == frame.DestinationMac && item.Device.Mac == frame.SourceMac).FirstOrDefault();

      return ConversationList.GetConversations(item => item.ContainsFrame(frame)).FirstOrDefault();
    }

    private int GetFrameNumber(String input)
    {
      try
      {
        String result = input.Substring(0, input.IndexOf(":", StringComparison.Ordinal));
        return int.Parse(result.Substring(result.IndexOf(" ", StringComparison.Ordinal)).Trim());
      }
      catch (FormatException e)
      {
        Logger.WriteLine(e.Message);
        return -1;
      }
    }

    private readonly NTFrameList mFrameList;
  }
}
