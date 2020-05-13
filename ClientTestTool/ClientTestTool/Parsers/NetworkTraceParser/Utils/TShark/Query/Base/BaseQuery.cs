///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Builder;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Interfaces;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Base
{
  public abstract class BaseQuery : IQuery
  {
    protected BaseQuery(QueryInfo info)
    {
      if (null == info)
        throw new ArgumentNullException("info");

      QueryInfo = info;
    }

    public QueryInfo QueryInfo
    {
      get;
      private set;
    }

    public String Build()
    {
      return QueryBuilder.Build(QueryInfo);
    }
  }
}
