///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query
{
  public class QueryInfo
  {
    public QueryInfo (String filename)
    {
      Filename          = filename;
      Filter            = String.Empty;
      Fields            = new List<String>();
      DetailedProtocols = new List<String>();
    }

    #region Properties

    public String Filename
    {
      get;
      private set;
    }

    public String Filter
    {
      get;
      set;
    }

    public List<String> Fields
    {
      get;
      set;
    }

    public List<String> DetailedProtocols
    {
      get;
      set;
    }

    #endregion
  }
}
