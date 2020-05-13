using System;
using System.Text;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.Decoders
{
  public static class Base64ToASCIIConverter
  {
    public static String Convert(String base64Encoded)
    {
      var base64EncodedBytes = System.Convert.FromBase64String(base64Encoded);
      return Encoding.UTF8.GetString(base64EncodedBytes);
    }
  }
}
