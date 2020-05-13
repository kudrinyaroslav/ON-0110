using System;
using System.Web;
using System.Web.Services.Protocols;
using System.IO;

namespace DUT.PACS.Simulator
{
    // XmlStreamSoapExtension exposes raw SOAP messages to
    // an ASP.NET Web service
    public class RequestListenerExtension : SoapExtension
    {
        private bool input = true;
        Stream httpInputStream;    

        // ChainStream replaces original stream with
        // extension’s stream
        public override Stream ChainStream(Stream stream)
        {
            Stream result = stream;
            if (input)
            {
                httpInputStream = stream;
                input = false;
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

        }

        // ProcessMessage is called to process SOAP messages
        // after inbound messages are deserialized to input
        // parameters and output parameters are serialized to
        // outbound messages
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeDeserialize:
                    {
                        StreamReader rdr = new StreamReader(httpInputStream);
                        string str = rdr.ReadToEnd();
                        RequestListener.Save(str);
                        httpInputStream.Seek(0, SeekOrigin.Begin);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Request storage
    /// </summary>
    public class RequestListener
    {
        private static string _currentRequest;
        public static void Save(string request)
        {
            _currentRequest = request;
        }
        public static string Take()
        {
            string request = _currentRequest;
            _currentRequest = string.Empty;
            return request;
        }
    }

    /// <summary>
    /// Attribute for attaching extension to methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestListenerExtensionAttribute : SoapExtensionAttribute
    {
        public RequestListenerExtensionAttribute()
        {

        }

        private int priority = 1;
        public override Type ExtensionType
        {
            get { return typeof(RequestListenerExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = 1; }
        }
    }

}
