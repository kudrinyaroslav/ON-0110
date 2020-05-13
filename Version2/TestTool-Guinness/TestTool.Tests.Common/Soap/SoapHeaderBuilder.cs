///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////

using System.Xml;

namespace TestTool.Tests.Common.Soap
{
    public interface ISoapHeaderBuilder
    {
        void WriteHeader(XmlWriter writer, object message);
    }
}
