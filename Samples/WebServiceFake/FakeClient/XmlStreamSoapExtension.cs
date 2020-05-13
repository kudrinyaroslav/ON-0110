using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;

namespace FakeClient
{
    // XmlStreamSoapExtension exposes raw SOAP messages to
    // an ASP.NET Web service
    public class XmlStreamSoapExtension : SoapExtension
    {
        bool output = true;		// flag indicating input or output
        Stream httpInputStream;    // HTTP output stream to send
        // real output to
        Stream chainedInputStream; // output stream for ASP.NET
        // plumbing to write to

        private Stream httpOutputStream;
        private Stream chainedOutputStream;

        // ChainStream replaces original stream with
        // extension’s stream
        public override Stream ChainStream(Stream stream)
        {
            Stream result = stream;
            // only replace output stream with memory stream
            if (output)
            {
                output = false;
                httpOutputStream = stream;
                result = chainedOutputStream = new MemoryStream();
            }
            else
            {
                httpInputStream = stream;
                result = chainedInputStream = new MemoryStream();
            }
            return result;
        }

        public override object GetInitializer(System.Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(System.Web.Services.Protocols.LogicalMethodInfo methodInfo, System.Web.Services.Protocols.SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {

        }

        // ProcessMessage is called to process SOAP messages
        // after inbound messages are deserialized to input
        // parameters and output parameters are serialized to
        // outbound messages
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.AfterDeserialize:
                    {
                    }
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    {

                        StreamReader reader = new StreamReader(httpInputStream);
                        String str = reader.ReadToEnd();
                        StreamWriter sw = new StreamWriter(chainedInputStream);
                        sw.Write(str);
                        sw.Flush();
                        chainedInputStream.Position = 0;
                    }
                    break;
                case SoapMessageStage.AfterSerialize:
                    {
                        chainedOutputStream.Position = 0;
                        StreamReader reader = new StreamReader(chainedOutputStream);
                        String str = reader.ReadToEnd();
                        StreamWriter sw = new StreamWriter(httpOutputStream);
                        sw.Write(str);
                        sw.Flush();
                    }
                    break;
                case SoapMessageStage.BeforeSerialize:
                    {
                        SoapHeader header;
                        header = new SoapUnknownHeader();
                        header.MustUnderstand = true;
                        message.Headers.Add(header);
                    }
                    break;
            }
        }

        // CopyStream copies the contents of a source stream
        // to a destination stream
        private void CopyStream(Stream src, Stream dest)
        {
            StreamReader reader = new StreamReader(src);
            StreamWriter writer = new StreamWriter(dest);
            writer.Write(reader.ReadToEnd());
            writer.Flush();
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class XmlStreamSoapExtensionAttribute : SoapExtensionAttribute
    {
        private int priority = 1;
        public override Type ExtensionType
        {
            get { return typeof(XmlStreamSoapExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = 1; }
        }
    }

}
