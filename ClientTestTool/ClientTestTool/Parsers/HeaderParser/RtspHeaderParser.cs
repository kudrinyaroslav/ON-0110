///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using ClientTestTool.Parsers.HeaderParser.Base;

namespace ClientTestTool.Parsers.HeaderParser
{
  public class RtspHeaderParser : BaseHeaderParser
  {
    public RtspHeaderParser(TextReader reader) : base(reader)
    {
    }

    public RtspHeaderParser(String filename) : base(filename)
    {
    }

    public String CSeq
    {
      get
      {
        return LoadProperty(CSEQ_TAG);
      }
    }

    public String Request
    {
      get
      {
        return LoadProperty(REQUEST_TAG);
      }
    }

    public String Method
    {
      get
      {
        return LoadProperty(METHOD_TAG);
      }
    }

    public String StatusCode
    {
      get
      {
        return LoadProperty(STATUS_CODE_TAG);
      }
    }

    public String Uri
    {
      get
      {
        return LoadProperty(URL_TAG);
      }
    }

    public String Transport
    {
      get
      {
        return LoadProperty(TRANSPORT_TAG);
      }
    }

    public String Range
    {
      get
      {
        return LoadProperty(RANGE_TAG);
      }
    }

    public String Session
    {
      get
      {
        return LoadProperty(SESSION_TAG);
      }
    }

    public String Require
    {
      get
      {
        return LoadProperty(REQUIRE_TAG);
      }
    }

    public String Scale
    {
      get
      {
        return LoadProperty(SCALE_TAG);
      }
    }

    public String[] MIMETypes
    {
      get
      {
        return LoadProperties(MIME_TYPE_TAG);
      }
    }

    public String[] MediaTypes
    {
      get
      {
        return LoadProperties(MEDIA_TYPE_TAG);
      }
    }

    private const String REQUEST_TAG     = "Request";
    private const String METHOD_TAG      = "Method";
    private const String STATUS_CODE_TAG = "Status";
    private const String CSEQ_TAG        = "CSeq";
    private const String TRANSPORT_TAG   = "Transport";
    private const String URL_TAG         = "URL";
    private const String RANGE_TAG       = "Range";
    private const String SESSION_TAG     = "Session";
    private const String REQUIRE_TAG     = "Require";
    private const String SCALE_TAG       = "Scale";
    private const String MEDIA_TYPE_TAG  = "Media Type";

    private const String MIME_TYPE_TAG   = "MIME Type";
  }
}
