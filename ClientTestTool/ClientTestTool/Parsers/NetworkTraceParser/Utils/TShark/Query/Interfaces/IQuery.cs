///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query.Interfaces
{
  public interface IQuery
  {
    QueryInfo QueryInfo
    {
      get;
    }
      
    /// <summary>
    /// Builds query from arguments
    /// </summary>
    /// <returns></returns>
    String Build();
  }
}
