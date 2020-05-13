///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClientTestTool.Parsers.NetworkTraceParser.Extensions
{
  internal static class StreamReaderExtension
  {
    public static IEnumerable<String> ReadAllLines(this StreamReader reader)
    {
      if (null == reader)
        throw new ArgumentNullException("reader");

      while (!reader.EndOfStream)
      {
        var line = reader.ReadLine();

        if (!String.IsNullOrEmpty(line))
          yield return line;
      }  
    }

    public static IEnumerable<String> ReadAllFrames(this StreamReader reader)
    {
      String line;
      var frameBuilder = new StringBuilder();

      while ((line = reader.ReadLine()) != null)
      {
        if (String.Empty == line && 0 != frameBuilder.Length)
        {
          String result = frameBuilder.ToString();
          frameBuilder.Clear();
          line = String.Empty;

          yield return result;
        }

        frameBuilder.AppendLine(line.Trim().Replace("\\r\\n", String.Empty));
      }
    }
  }
}
