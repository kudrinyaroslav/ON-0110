using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.Tests.CommonUtils.XmlTransformation
{
    public class XmlNamespacesGenerator : ISoapMessageMutator
    {
        #region ISoapMessageMutator Members

        public byte[] ProcessMessage(byte[] original)
        {
            string content = Encoding.UTF8.GetString(original);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);


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
