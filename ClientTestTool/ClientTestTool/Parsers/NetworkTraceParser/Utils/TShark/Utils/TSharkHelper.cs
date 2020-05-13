///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Logging;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils
{
  public static class TSharkHelper
  {
    //TODO DEPRECATED
    public static bool RunTShark(String args, out String output)
    {
      var timer = new Stopwatch();
      timer.Start();

      bool result = CmdUtil.LaunchApp(GetTSharkPath(), args, out output);

      timer.Stop();
      Logger.WriteLine(String.Format("TShark query:\"{0}\" {1} ms ellapsed", args, timer.ElapsedMilliseconds));

      return result;
    }

    /// <summary>
    /// Returns packet data without tshark header
    /// </summary>
    public static String GetFrameWithoutHeader(String frame)
    {
      var lines = frame.SplitToLines().ToList().SkipWhile(item => item != HEADER_HTTP && item != HEADER_RTSP).ToList();

      //mHeaders.ToList().ForEach(header => lines.RemoveAll(line => line.Contains(header)));

      return String.Join(Environment.NewLine, lines);
    }

    public static String GetProtocolDetails(String frame, String protocol)
    {
      var lines = frame.SplitToLines().ToList().SkipWhile(item => item != protocol)
                                               .TakeWhile(item => item == protocol || !mHeaders.Contains(item)).ToList();

      mHeaders.ToList().ForEach(header => lines.RemoveAll(line => line.Contains(header)));

      return String.Join(Environment.NewLine, lines);
    }

    #region Helpers

    /// <summary>
    /// Runs TShark and redirecting stdout to output variable
    /// </summary>
    internal static String GetTSharkPath()
    {
      return Path.Combine(GetTSharkDir(), "tshark.exe");
    }

    private static String GetTSharkDir()
    {
      return Path.Combine(Directory.GetCurrentDirectory(), Properties.Settings.Default.LibFolder);
    }

    #region Separators

    public const Char OUTPUT_SEPARATOR = '\t';

    #endregion

    #region Filters

    public const String FILTER_FIELDS      = @"-T fields";
    public const String FIELD              = @"-e";
    public const String FIELD_DATA         = @"data.data";
    public const String FIELD_XML_TAG      = @"xml.tag";
    public const String FIELD_FRAME_NUMBER = @"frame.number";
    public const String FIELD_MAC_SRC      = @"eth.src";
    public const String FIELD_MAC_DST      = @"eth.dst";
    public const String FIELD_IP_SRC       = @"ip.src";
    public const String FIELD_IP_DST       = @"ip.dst";
    public const String FIELD_PROTOCOL     = @"frame.protocols";
    public const String FIELD_REQUEST_IN   = @"http.request_in";

    #endregion

    #region Args

    public const String ARG_READ_FILE        = @"-nr";
    public const String ARG_DISPLAY_FILTER   = @"-Y";
    public const String ARG_PROTOCOL_DETAILS = @"-O";

    #endregion

    #region Protocols

    public const String PROTOCOL_HTTP     = @"eth:ip:tcp:http";
    public const String PROTOCOL_HTTP_XML = @"eth:ip:tcp:http:xml";
    public const String PROTOCOL_WSD      = @"ws-discovery";
    public const String PROTOCOL_RTSP     = @"eth:ip:tcp:rtsp";
    public const String PROTOCOL_SDP      = @"eth:ip:tcp:rtsp:sdp";

    private static readonly String[] mRespectProtocols =
    {
      PROTOCOL_HTTP, PROTOCOL_HTTP_XML, PROTOCOL_RTSP
    };

    #endregion

    #region Headers

    public  const String HEADER_HTTP = @"Hypertext Transfer Protocol";
    public  const String HEADER_XML  = @"eXtensible Markup Language";
    private const String HEADER_RTSP = @"Real Time Streaming Protocol";
    private const String HEADER_LBTD = @"Line-based text data";
    private const String HEADER_JSON = @"JavaScript Object Notation";

    private static readonly String[] mHeaders =
    {
      HEADER_HTTP, HEADER_XML, HEADER_RTSP, HEADER_LBTD, HEADER_JSON
    };

    #endregion

    #endregion

  }
}
