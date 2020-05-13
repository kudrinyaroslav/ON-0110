///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using ClientTestTool.Parsers.HeaderParser.Base;

namespace ClientTestTool.Parsers.HeaderParser
{
  public sealed class HttpHeaderParser : BaseHeaderParser
  {
    private const String STATUS_CODE_TAG     = "Status Code";
    private const String REQUEST_METHOD_TAG  = "Request Method";
    private const String CONNECTION_TAG      = "Connection";

    public HttpHeaderParser(TextReader reader) : base(reader)
    {
    }

    public HttpHeaderParser(String filename) : base (filename)
    {
    }

    #region Properties

    public String RequestMethod
    {
      get
      {
        return LoadProperty(REQUEST_METHOD_TAG);
      }
    }

    public String StatusCode
    {
      get
      {
        return LoadProperty(STATUS_CODE_TAG);
      }
    }

    public String UserAgent
    {
      get
      {
        return LoadProperty(USER_AGENT_TAG);
      }
    }

    public String Connection
    {
      get
      {
        return LoadProperty(CONNECTION_TAG);
      }
    }

    #endregion
  }
}
