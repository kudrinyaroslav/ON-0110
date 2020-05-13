using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;

namespace CameraWebService
{

    // XmlStreamSoapExtension exposes raw SOAP messages to
    // an ASP.NET Web service
    public class SecurityExtension : SoapExtension
    {

        private string _responseToSubstitute;

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
            SecurityExtensionAttribute attribute = (SecurityExtensionAttribute)initializer;
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
                        
                    }
                    break;
                case SoapMessageStage.AfterDeserialize:
                    {
                        SecurityCheck.Check(message.Headers);
                    }
                    break;
                case SoapMessageStage.AfterSerialize:
                    {

                    }
                    break;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SecurityExtensionAttribute : SoapExtensionAttribute
    {
        public SecurityExtensionAttribute()
        {

        }

        private int priority = 1;
        public override Type ExtensionType
        {
            get { return typeof(SecurityExtension); }
        }
        
        public override int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
            }
        }
    }
}
