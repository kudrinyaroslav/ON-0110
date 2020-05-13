///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////

using System.Xml;

namespace DUT.PACS.Simulator.Discovery.Soap
{
    public interface ISoapHeaderBuilder
    {
        void WriteHeader(XmlWriter writer, object message);
    }
}
