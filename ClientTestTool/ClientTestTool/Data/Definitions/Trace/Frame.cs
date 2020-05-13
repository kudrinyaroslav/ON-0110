///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Interfaces;

namespace ClientTestTool.Data.Definitions.Trace
{
  public sealed class Frame : ITraceItem
  {
    /// <summary>
    /// ctor
    /// </summary>
    public Frame(int number, NetworkTraceInfo trace, String srcMac, String dstMac, String srcIP, String dstIp, String protocol)
    {
      Number         = number;
      FoundInTrace   = trace;
      SourceMac      = srcMac;
      DestinationMac = dstMac;
      SourceIp       = srcIP;
      DestinationIp  = dstIp;
      Protocol       = protocol;
    }

    public int Number
    {
      get;
      private set;
    }

    public NetworkTraceInfo FoundInTrace
    {
      get;
      private set;
    }

    public String SourceMac
    {
      get;
      private set;
    }

    public String DestinationMac
    {
      get;
      private set;
    }

    public String SourceIp
    {
      get;
      private set;
    }

    public String DestinationIp
    {
      get;
      private set;
    }

    public String Protocol
    {
      get;
      private set;
    }
  }
}
