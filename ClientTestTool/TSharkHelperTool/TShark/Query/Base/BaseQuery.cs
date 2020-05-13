using System;
using TSharkHelperTool.TShark.Query.Builder;
using TSharkHelperTool.TShark.Query.Interfaces;

namespace TSharkHelperTool.TShark.Query.Base
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
