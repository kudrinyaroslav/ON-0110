

using System;
using System.Linq;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Auth;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Enums;

namespace ClientTestTool.Data.Definitions.Conversation.Extensions
{
  internal static class HttpResponseExtension
  {
    public static ValidationStatus ValidateDigest(this HttpResponse response)
    {
      var messages = response.Conversation.GetMessages(ContentType.Http);
      var details = messages.First(item => item.Response == response).Request.GetDetails();

      var pairs = messages.Where(item => item.Request.GetDetails() == details);

      var responseAuth = Authentication.FromAuthHeader(response.AuthHeader);

      if (pairs.Any(item =>
      {
        String authHeader = item.GetRequest<HttpMessage>().AuthHeader;

        if (String.IsNullOrEmpty(authHeader))
          return false;

        var requestAuth = Authentication.FromAuthHeader(authHeader);

        return requestAuth.Nonce == responseAuth.Nonce &&
               requestAuth.Realm == responseAuth.Realm &&
               "200" == item.GetResponse<HttpResponse>().StatusCode;
      }))
        return ValidationStatus.HttpDigest;


      return ValidationStatus.Failed;
    }
  }
}
