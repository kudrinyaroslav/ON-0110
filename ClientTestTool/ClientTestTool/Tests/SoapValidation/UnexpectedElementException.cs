///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace ClientTestTool.Tests.SoapValidation
{
  internal class UnexpectedElementException : XmlException
  {
    public List<XmlElement> Headers
    {
      get;
      protected set;
    }
    public UnexpectedElementException(String message, List<XmlElement> headers)
      : base(message)
    {
      Headers = headers;
    }
  }
}
