///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Trace;

namespace ClientTestTool.Parsers.NetworkTraceParser.Extensions
{
  internal static class NTFrameListExtension
  {
    /// <summary>
    /// Find all unique MAC addresses in frames
    /// </summary>
    /// <returns>List of unique MAC-IP pairs</returns>
    public static List<Tuple<String, String>> GetDevices(this NTFrameList frameList)
    {
      var frames = frameList.ToList();
      var uniqueValues = new HashSet<Tuple<String, String>>();
      foreach (Frame frame in frames)
      {
        uniqueValues.Add(Tuple.Create(frame.SourceMac, frame.SourceIp));
        uniqueValues.Add(Tuple.Create(frame.DestinationMac, frame.DestinationIp));
      }

      return uniqueValues.ToList();
    }

    /// <summary>
    /// Gets frames related to conversation
    /// </summary>
    public static List<Frame> GetConversationFrames(this NTFrameList frameList, String srcMac, String dstMac)
    {
      return frameList.Where(item => (item.SourceMac == srcMac && item.DestinationMac == dstMac) ||
                                     (item.SourceMac == dstMac && item.DestinationMac == srcMac))
                      .ToList();

    }

  }
}
