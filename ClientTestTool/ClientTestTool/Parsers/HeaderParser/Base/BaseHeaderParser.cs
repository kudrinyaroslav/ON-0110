///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClientTestTool.Parsers.HeaderParser.Base
{
  public abstract class BaseHeaderParser
  {
    protected BaseHeaderParser(TextReader reader)
    {
      mLines = GetLines(reader).Reverse().ToArray();
    }

    protected BaseHeaderParser(String filename)
    {
      mLines = GetLines(filename).Reverse().ToArray();
    }

    #region Properties

    public String RequestAuthHeader
    {
      get
      {
        return LoadProperty(REQUEST_AUTH_TAG);
      }
    }

    public String ResponseAuthHeader
    {
      get
      {
        return LoadProperty(RESPONSE_AUTH_TAG);
      }
    }

    #endregion

    #region Logic

    protected virtual String LoadProperty(String tag)
    {
      foreach (String line in mLines.Reverse()) // optimization
        if (line.Contains(tag))
        {
          String result = line.Substring(tag.Length + 1); // ':' included
          int indexOfNewLine = result.IndexOf("\\r", StringComparison.Ordinal);

          if (-1 != indexOfNewLine)
            result = result.Remove(indexOfNewLine);

          return result.Trim();
        }

      return String.Empty;
    }

    protected virtual String[] LoadProperties(String tag)
    {
      var result = new List<String>();

      foreach (String line in mLines.Reverse()) // optimization
        if (line.Contains(tag))
        {
          String value = line.Substring(tag.Length + 1); // ':' included
          int indexOfNewLine = value.IndexOf("\\r", StringComparison.Ordinal);

          if (-1 != indexOfNewLine)
            value = value.Remove(indexOfNewLine);

          result.Add(value.Trim());
        }

      return result.ToArray();
    }

    #endregion

    #region Helpers

    private IEnumerable<String> GetLines(TextReader reader)
    {
      return reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }

    private IEnumerable<String> GetLines(String filename)
    {
      return File.ReadAllLines(filename);
    }

    #endregion

    private readonly String[] mLines;

    protected const String USER_AGENT_TAG    = "User-Agent";
    protected const String REQUEST_AUTH_TAG  = "Authorization";
    protected const String RESPONSE_AUTH_TAG = "WWW-Authenticate";
  }
}
