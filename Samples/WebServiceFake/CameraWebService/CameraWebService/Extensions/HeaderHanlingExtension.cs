using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;

namespace CameraWebService
{
    // XmlStreamSoapExtension exposes raw SOAP messages to
    // an ASP.NET Web service
    public class HeaderHandlingExtension : SoapExtension
    {
        // ChainStream replaces original stream with
        // extension’s stream
        public override Stream ChainStream(Stream stream)
        {
            Stream result = stream;
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
                case SoapMessageStage.AfterDeserialize:
                    {
                        foreach (SoapHeader header in message.Headers)
                        {
                            header.DidUnderstand = true;
                        }
                        break;
                    }
                case SoapMessageStage.BeforeSerialize:
                    {
                        foreach (SoapHeader hdr in message.Headers)
                        {
                            string name = hdr.GetType().FullName;
                        }
                    }
                    break;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HeaderHandlingAttribute : SoapExtensionAttribute
    {
        public HeaderHandlingAttribute()
        {

        }
        
        private int priority = 1;
        public override Type ExtensionType
        {
            get { return typeof(HeaderHandlingExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = 1; }
        }
    }

}
