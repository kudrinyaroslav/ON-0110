using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;

namespace DUT.CameraWebService
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
            XmlReplySubstituteExtensionAttribute attribute = (XmlReplySubstituteExtensionAttribute)initializer;
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
                case SoapMessageStage.AfterDeserialize:
                    {
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

                        /*
                        byte[] bytes = new byte[chainedOutputStream.Length];
                        chainedOutputStream.Read(bytes, 0, (int)chainedOutputStream.Length);
                        Decoder d = Encoding.Default.GetDecoder(); 
                        int count = d.GetCharCount(bytes, 0, bytes.Length);  
                        char[] cs = new char[count]; 
                        d.GetChars(bytes, 0, bytes.Length, cs, 0); 
                        string dest = new string(cs);
                        */

                        StreamWriter sw = new StreamWriter(httpOutputStream);

                        HttpContext.Current.Response.StatusCode = 200;
                        HttpContext.Current.Response.StatusDescription = "OK";
                        HttpContext.Current.Response.ContentType = "application/soap+xml; charset=utf-8";
                        sw.Write(_responseToSubstitute);
                        sw.Flush();

                        if (appOutputStream != null)
                        {
                            appOutputStream.Close();
                        }

                        //HttpResponse response = HttpContext.Current.Response;
                        //response.ClearContent();
                        //response.Write(_responseToSubstitute);

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
