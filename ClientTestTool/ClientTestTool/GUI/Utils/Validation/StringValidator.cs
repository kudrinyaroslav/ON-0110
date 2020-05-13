///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.GUI.Utils.Validation
{
  public static class StringValidator
  {
    public static bool IsValidUri(String uriString)
    {
      Uri uri;

      if (!uriString.Contains("."))
        return false;

      if (!uriString.Contains("://"))
        uriString = "http://" + uriString;
      if (Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out uri))
        return true;

      return false;
    }

    public static bool IsValidEmailAddress(String emailAddress)
    {
      if (0 == emailAddress.Length)
        return false;

      int indexOfAt = emailAddress.IndexOf("@", StringComparison.Ordinal);
      if (-1 != indexOfAt)
        if (emailAddress.IndexOf(".", indexOfAt, StringComparison.Ordinal) > indexOfAt)
          return true;

      return false;
    }
  }
}
