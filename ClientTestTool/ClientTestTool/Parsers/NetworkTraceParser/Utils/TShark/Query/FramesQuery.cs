///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Base;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query
{
  internal sealed class FramesQuery : BaseQuery
  {
    public FramesQuery(String filename, String[] protocols) : base(new QueryInfo(filename))
    {
      QueryInfo.Filter = String.Join(" or ", protocols);

      QueryInfo.DetailedProtocols.Add("http");
      QueryInfo.DetailedProtocols.Add("xml");
      QueryInfo.DetailedProtocols.Add("rtsp");
    }
  }
}
