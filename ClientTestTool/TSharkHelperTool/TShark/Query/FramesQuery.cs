using System;
using TSharkHelperTool.TShark.Query.Base;

namespace TSharkHelperTool.TShark.Query
{
  class FramesQuery : BaseQuery
  {
    public FramesQuery(String filename) : base(new QueryInfo(filename))
    {
      QueryInfo.Filter = String.Format("{0} or {1}", "http" , "rtsp");

      QueryInfo.DetailedProtocols.Add("http");
      QueryInfo.DetailedProtocols.Add("xml");
      QueryInfo.DetailedProtocols.Add("rtsp");
    }
  }
}
