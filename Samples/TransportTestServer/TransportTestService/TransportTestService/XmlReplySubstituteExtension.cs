using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;

namespace TransportTestService
{

    // XmlStreamSoapExtension exposes raw SOAP messages to
    // an ASP.NET Web service
    public class XmlReplySubstituteExtension : SoapExtension
    {
        bool output = false;		// flag indicating input or output
        Stream httpOutputStream;    // HTTP output stream to send
        // real output to
        Stream chainedOutputStream; // output stream for ASP.NET
        // plumbing to write to
        Stream appOutputStream;     // output stream for method
        // to write to


        private string _responseToSubstitute;

        // ChainStream replaces original stream with
        // extension’s stream
        public override Stream ChainStream(Stream stream)
        {
            Stream result = stream;
            // only replace output stream with memory stream
            if (output)
            {
                httpOutputStream = stream;
                result = chainedOutputStream = new MemoryStream();
            }
            else
            {
                output = true;
            }
            return result;
        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return attribute;
        }

        public override void Initialize(object initializer)
        {
            XmlReplySubstituteExtensionAttribute attribute = (XmlReplySubstituteExtensionAttribute) initializer;
            _responseToSubstitute = attribute.ResponseToSubstitute;
        }

        // ProcessMessage is called to process SOAP messages
        // after inbound messages are deserialized to input
        // parameters and output parameters are serialized to
        // outbound messages
        public override void ProcessMessage(SoapMessage message)
        {
            HttpApplicationState state = HttpContext.Current.Application;

            switch (message.Stage)
            {
                case SoapMessageStage.BeforeDeserialize:
                    {
                        message.Headers.Clear();
                    }
                    break;
                case SoapMessageStage.AfterDeserialize:
                    {
                        message.Headers.Clear();
                        // rewind HTTP input stream to start and store
                        // reference in current HTTP context
                        HttpContext.Current.Request.InputStream.Position = 0;
                        HttpContext.Current.Items["SoapInputStream"] =
                            HttpContext.Current.Request.InputStream;
                        // create new memory stream for method to write
                        // output message to and store reference in
                        // current HTTP context
                        appOutputStream = new MemoryStream();
                        HttpContext.Current.Items["SoapOutputStream"] =
                            appOutputStream;

                        break;
                    }
                case SoapMessageStage.AfterSerialize:
                    {
                        chainedOutputStream.Position = 0;
                        StreamWriter sw = new StreamWriter(httpOutputStream);
                        sw.Write(_responseToSubstitute);
                        sw.Flush();

                        //message.ContentEncoding = "";
                        //message.ContentType = "";

                        if (appOutputStream != null)
                        {
                            appOutputStream.Close();
                        }

                        break;
                    }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class XmlReplySubstituteExtensionAttribute : SoapExtensionAttribute
    {
        public XmlReplySubstituteExtensionAttribute()
        {

        }
        
        public XmlReplySubstituteExtensionAttribute(string response)
        {
            _responseToSubstitute = response;
        }
        
        private int priority = 1;
        public override Type ExtensionType
        {
            get { return typeof(XmlReplySubstituteExtension); }
        }
        
        private string _responseToSubstitute;
        public string ResponseToSubstitute
        {
            get { return _responseToSubstitute; }
            set { _responseToSubstitute = value; }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = 1; }
        }
    }
}
