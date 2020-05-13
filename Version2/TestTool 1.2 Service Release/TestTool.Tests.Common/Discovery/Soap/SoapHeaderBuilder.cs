///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TestTool.Tests.Common.Discovery
{
    public interface ISoapHeaderBuilder
    {
        void WriteHeader(XmlWriter writer, object message);
    }
}
