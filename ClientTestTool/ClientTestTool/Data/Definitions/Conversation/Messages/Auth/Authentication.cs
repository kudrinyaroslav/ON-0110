///
/// @Author Matthew Tuusberg
///

using System;
using System.Text.RegularExpressions;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Auth
{
  public class Authentication
  {
    public static Authentication FromAuthHeader(String authHeader)
    {
      if (String.IsNullOrEmpty(authHeader))
        return null;

      return new Authentication(authHeader);
    }

    private Authentication(String authHeader)
    {
      Realm = GetHeaderVar("realm", authHeader);
      Nonce = GetHeaderVar("nonce", authHeader);
      Qop   = GetHeaderVar("qop"  , authHeader);
    }

    #region Properties

    public String Realm
    {
      get;
      private set;
    }

    public String Nonce
    {
      get;
      private set;
    }

    public String Qop
    {
      get;
      private set;
    }

    #endregion

    #region Helpers

    private String GetHeaderVar(String varName, String header)
    {
      var regHeader = new Regex(String.Format(@"{0}=""([^""]*)""", varName));
      var matchHeader = regHeader.Match(header);

      if (matchHeader.Success)
        return matchHeader.Groups[1].Value;

      return String.Empty;
    }

    #endregion
  }
}
