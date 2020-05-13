///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Base;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query
{
  internal sealed class WSDiscoveryQuery : BaseQuery
  {
    public WSDiscoveryQuery(IEnumerable<int> frameNumbers, String filename) : base(new QueryInfo(filename))
    {
      QueryInfo.Filter = String.Join(" or ", frameNumbers.Select(number => String.Format("frame.number eq {0}", number)));
      QueryInfo.Fields.Add(TSharkHelper.FIELD_DATA);
    }
  }
}
