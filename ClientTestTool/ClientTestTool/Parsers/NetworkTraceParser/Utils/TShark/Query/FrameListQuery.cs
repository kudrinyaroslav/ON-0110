///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Base;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query
{
  internal sealed class FrameListQuery : BaseQuery
  {
    public FrameListQuery(String filename, IEnumerable<String> protocols) : base(new QueryInfo(filename))
    {
      QueryInfo.Filter = String.Join(" or ", protocols);

      QueryInfo.Fields.Add(TSharkHelper.FIELD_FRAME_NUMBER);
      QueryInfo.Fields.Add(TSharkHelper.FIELD_MAC_SRC);
      QueryInfo.Fields.Add(TSharkHelper.FIELD_MAC_DST);
      QueryInfo.Fields.Add(TSharkHelper.FIELD_IP_SRC);
      QueryInfo.Fields.Add(TSharkHelper.FIELD_IP_DST);
      QueryInfo.Fields.Add(TSharkHelper.FIELD_PROTOCOL);
    }
  }
}
