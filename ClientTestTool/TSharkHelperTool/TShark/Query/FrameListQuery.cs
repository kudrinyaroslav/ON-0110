using System;
using TSharkHelperTool.TShark.Query.Base;

namespace TSharkHelperTool.TShark.Query
{
  public sealed class FrameListQuery : BaseQuery
  {
    public FrameListQuery(String filename, String[] protocols) : base(new QueryInfo(filename))
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
