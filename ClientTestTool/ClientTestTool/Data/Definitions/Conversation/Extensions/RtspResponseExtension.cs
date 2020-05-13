using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Auth;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Data.Enums;

namespace ClientTestTool.Data.Definitions.Conversation.Extensions
{
  internal static class RtspResponseExtension
  {
    public static ValidationStatus ValidateDigest(this RtspResponse response)
    {
      var messages = response.Conversation.GetMessages(ContentType.Rtsp);
      var details = messages.First(item => item.Response == response).Request.GetDetails();

      var pairs = messages.Where(item => item.Request.GetDetails() == details);

      var responseAuth = Authentication.FromAuthHeader(response.AuthHeader);

      if (pairs.Any(item =>
      {
        String authHeader = item.GetRequest<RtspMessage>().AuthHeader;

        if (String.IsNullOrEmpty(authHeader))
          return false;

        var requestAuth = Authentication.FromAuthHeader(authHeader);

        return requestAuth.Nonce == responseAuth.Nonce &&
               requestAuth.Realm == responseAuth.Realm &&
               "200" == item.GetResponse<RtspResponse>().StatusCode;
      }))
        return ValidationStatus.RtspDigest;


      return ValidationStatus.Failed;
    }
  }
}
