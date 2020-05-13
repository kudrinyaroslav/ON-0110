///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Text;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Builder
{
  internal static class QueryBuilder
  {
    public static String Build(QueryInfo info)
    {
      var queryBuilder = new StringBuilder();

      queryBuilder.AppendFormat("{0} \"{1}\" ", ARG_READ_FILE, info.Filename);

      if (!String.IsNullOrEmpty(info.Filter))
        queryBuilder.AppendFormat("{0} \"{1}\" ", ARG_DISPLAY_FILTER, info.Filter);

      if (info.Fields.Any())
        queryBuilder.AppendFormat("{0} {1} ", FILTER_FIELDS, String.Join(" ", info.Fields.Select(item => String.Format("{0} {1}", FIELD, item))));

      if (info.DetailedProtocols.Any())
        queryBuilder.AppendFormat("{0} {1} ", ARG_PROTOCOL_DETAILS, String.Join(",", info.DetailedProtocols));

      return queryBuilder.ToString();
    }

    #region Fields

    private const String FILTER_FIELDS = @"-T fields";
    private const String FIELD         = @"-e";

    #endregion

    #region Args

    private const String ARG_READ_FILE        = @"-nr";
    private const String ARG_DISPLAY_FILTER   = @"-Y";
    private const String ARG_PROTOCOL_DETAILS = @"-O";

    #endregion
  }
}
