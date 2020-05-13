using System;
using System.Diagnostics;

namespace TSharkHelperTool
{
  public static class TSharkHelper
  {
    public static bool RunTShark(String args, out String output)
    {
      var timer = new Stopwatch();
      timer.Start();

      bool result = CmdUtil.LaunchApp(GetTSharkPath(), args, out output);

      timer.Stop();
      Console.WriteLine("TShark query:\"{0}\" {1} ms ellapsed", args, timer.ElapsedMilliseconds);

      return result;
    }
    #region Helpers

    /// <summary>
    /// Runs TShark and redirecting stdout to output variable
    /// </summary>
    public static String GetTSharkPath()
    {
      return Properties.Settings.Default.PathTShark;
    }

    #region Filters

    public const String FILTER_FIELDS      = @"-T fields";
    public const String FIELD              = @"-e";
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
