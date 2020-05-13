using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Soap
{
    public interface ISoapHeaderBuilder
    {
        void WriteHeader(XmlWriter writer, object message);
    }
}
