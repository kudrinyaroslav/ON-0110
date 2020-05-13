using System.IO;
using System.Text;
using System.Xml;

namespace TestTool.GUI.Utils
{
    class XmlFragmentWriter : XmlTextWriter
    {
        public XmlFragmentWriter(TextWriter w) : base(w) { }
        public XmlFragmentWriter(Stream w) : base(w, Encoding.UTF8) { }
        public XmlFragmentWriter(Stream w, Encoding encoding) : base(w, encoding) { }
        public XmlFragmentWriter(string filename, Encoding encoding) :
            base(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None), encoding) { }

        private bool _start = true;

        public override void WriteStartDocument()
        {
            if (_start)
            {
                base.WriteStartDocument();
                base.WriteStartElement("Advanced");
                _start = false;
            }
        }

        public override void WriteEndDocument()
        {
            base.WriteEndElement();
            base.WriteEndDocument();
        }

    }
}
