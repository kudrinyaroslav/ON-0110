using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace TestTool.Tests.Common.TestEngine
{
    public class MessageXmlTransformer : HttpTransport.ISoapMessageMutator
    {

        private Action<XmlDocument> _transformAction;

        public MessageXmlTransformer(Action<XmlDocument> action)
        {
            _transformAction = action;
        }


        #region ISoapMessageMutator Members

        public byte[] ProcessMessage(byte[] original)
        {
            string content = Encoding.UTF8.GetString(original);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            _transformAction(xmlDoc);

            MemoryStream stream = new MemoryStream();

            Encoding encoding = new UTF8Encoding(false);

            XmlWriter writer = new XmlTextWriter(stream, encoding);
            xmlDoc.WriteTo(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            return messageBytes;


        }

        #endregion
    }
}
