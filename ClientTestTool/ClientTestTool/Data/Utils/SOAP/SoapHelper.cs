///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Xml.Serialization;
using ClientTestTool.TestCases.SoapValidation;

namespace ClientTestTool.Data.Utils.SOAP
{
  public static class SoapHelper
  {
    public static T SOAPToObject<T>(String filename)    
      where T : class
    {
      using (StreamReader stream = new StreamReader(filename))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(stream);
      }
    }

    public static Envelope<T> GetEnvelope<T>(String filename)
      where T : class
    {
      using (StreamReader stream = new StreamReader(filename))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(Envelope<T>));
        return (Envelope<T>) serializer.Deserialize(stream);
      }
    }
  }
}
