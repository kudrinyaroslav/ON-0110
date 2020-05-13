///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.Data.Extensions
{
  public static class StringExtension
  {
    public static String[] SplitToLines(this String str, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
    {
      if (null == str)
        throw new ArgumentNullException("str");

      return str.Split(new[] { Environment.NewLine }, options);
    }
  }
}
