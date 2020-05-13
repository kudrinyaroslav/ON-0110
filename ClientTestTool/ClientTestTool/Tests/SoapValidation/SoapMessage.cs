///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace ClientTestTool.Tests.SoapValidation
{
  public class SoapMessage<T> : ICloneable
      where T : class
  {
    public List<XmlElement> Header
    {
      get;
      protected set;
    }
    public T Object
    {
      get;
      protected set;
    }

    public SoapMessage(IEnumerable<XmlElement> header, T obj)
    {
      Header = new List<XmlElement>();
      Header.AddRange(header);

      Object = obj;
    }

    public SoapMessage<T> ToSoapMessage<T>()
        where T : class
    {
      return new SoapMessage<T>(Header, Object as T);
    }

    public object Clone()
    {
      return new SoapMessage<T>(Header, Object);
    }
  }
}
