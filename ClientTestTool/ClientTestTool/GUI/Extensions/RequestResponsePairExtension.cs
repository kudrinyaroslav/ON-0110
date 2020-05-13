///
/// @Author Matthew Tuusberg
///

using System;
using System.Text;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;

namespace ClientTestTool.GUI.Extensions
{
  internal static class RequestResponsePairExtension
  {
    public static String GetValidationDetails(this RequestResponsePair pair)
    {
      switch (pair.ContentType)
      {
        case ContentType.Http:
          return GetValidationDetails(pair.GetRequest<HttpRequest>(), pair.GetResponse<HttpResponse>());

        case ContentType.Rtsp:
          return GetValidationDetails(pair.GetRequest<RtspRequest>(), pair.GetResponse<RtspResponse>());

        case ContentType.WSDiscovery:
          return String.Empty;

        default:
          throw new ArgumentOutOfRangeException("pair.ContentType");
      }
    }

    private static String GetValidationDetails(HttpRequest request, HttpResponse response)
    {
      var sb = new StringBuilder();

      if (null != request && request.Validated)
      {
        sb.AppendLine("First-level validation:");
        sb.AppendLine(String.Format("\tWSDL validation of SOAP request:{0}", request.GetStatusString()));

        if (!String.IsNullOrEmpty(request.ValidationError))
          sb.AppendLine(String.Format("\tError:{0}", request.ValidationError));
        sb.AppendLine();
      }

      if (null != response && response.Validated)
      {
        sb.AppendLine(String.Format("Second-level validation:"));
        sb.AppendLine(String.Format("\tResponse code of unit:{0}", response.GetStatusString()));
      }

      return sb.ToString();
    }

    private static String GetValidationDetails(RtspRequest request, RtspResponse response)
    {
      //rtsp request validation does not exist
      var sb = new StringBuilder();

      if (null != response && response.Validated)
      {
        sb.AppendLine(String.Format("Second-level validation:"));
        sb.AppendLine(String.Format("\tResponse code of unit:{0}", response.GetStatusString()));
      }

      return sb.ToString();
    }
  }
}
