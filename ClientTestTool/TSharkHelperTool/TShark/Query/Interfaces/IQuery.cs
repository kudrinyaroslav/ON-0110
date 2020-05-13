using System;

namespace TSharkHelperTool.TShark.Query.Interfaces
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
