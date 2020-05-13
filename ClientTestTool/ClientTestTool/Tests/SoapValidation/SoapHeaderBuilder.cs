///
/// @Author Matthew Tuusberg
///

﻿using System.Xml;

namespace ClientTestTool.Tests.SoapValidation
{
    public interface ISoapHeaderBuilder
    {
        void WriteHeader(XmlWriter writer, object message);
    }
}
