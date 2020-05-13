///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Conversations.Enums;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark
{
  public struct TSFilter
  {
    public TSFilter(String protocol, String pType, MessageType type)
      : this()
    {
      Protocol   = protocol;
      PacketType = pType;
      Type       = type;
    }

    public String Protocol
    {
      get;
      set;
    }

    public String PacketType
    {
      get;
      set;
    }

    public MessageType Type
    {
      get;
      set;
    }

    public String Filter
    {
      get
      {
        return Type.ToString().ToLower(); // TShark HACK
      }
    }
  }
}
