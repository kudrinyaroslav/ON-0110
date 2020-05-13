using System;
using System.Collections.Generic;

namespace TSharkHelperTool.TShark.Query
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

    public String Filter
    {
      get;
      set;
    }

    public String Filename
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
