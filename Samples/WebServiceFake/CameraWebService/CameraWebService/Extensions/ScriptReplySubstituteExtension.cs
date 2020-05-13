﻿using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;

namespace CameraWebService
{
    // XmlStreamSoapExtension exposes raw SOAP messages to
    // an ASP.NET Web service
    public class ScriptReplySubstituteExtension : SoapExtension
    {
        bool output = false;		// flag indicating input or output
        Stream httpOutputStream;    // HTTP output stream to send
        // real output to
        Stream chainedOutputStream; // output stream for ASP.NET
        // plumbing to write to
        Stream appOutputStream;     // output stream for method
        // to write to

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
            ScriptDrivenAttribute attribute = (ScriptDrivenAttribute)initializer;
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

                        HttpContext.Current.Response.ContentType = "application/soap+xml; boundary=\"==Hr5qBwn7N66Cwrtq128ZSPCj3b7Cm6+tm3Ynn/i15c3pHkT8aPVseYQWzLYZ==\"; type=\"application/xop+xml\"; start=\"<SOAP-ENV:Envelope>\"; start-info=\"application/soap+xml; charset=utf-8\";";



                        chainedOutputStream.Position = 0;

                        StreamWriter sw = new StreamWriter(httpOutputStream);

                        HttpApplicationState state = HttpContext.Current.Application;
                        Script script;
                        if (state["script"] == null)
                        {
                            script = new Script();
                            state["script"] = script;
                        }
                        else
                        {
                            script = (Script)state["script"];
                        }

                        int step = 1;
                        if (state["step"] == null)
                        {
                            state["step"] = step;
                        }
                        else
                        {
                            step = (int) state["step"];
                            step++;
                            state["step"] = step;
                        }

                        string responseToSubstitute = script.GetResponse(step);
                        if (!string.IsNullOrEmpty(responseToSubstitute))
                        {
                            sw.Write(responseToSubstitute);
                        }
                        else
                        {
                            byte[] bytes = new byte[chainedOutputStream.Length];
                            chainedOutputStream.Read(bytes, 0, (int)chainedOutputStream.Length);
                            Decoder d = Encoding.Default.GetDecoder();
                            int count = d.GetCharCount(bytes, 0, bytes.Length);
                            char[] cs = new char[count];
                            d.GetChars(bytes, 0, bytes.Length, cs, 0);
                            string dest = new string(cs);
                            sw.Write(dest);
                        }
                        sw.Flush();

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
    public class ScriptDrivenAttribute : SoapExtensionAttribute
    {
        public ScriptDrivenAttribute()
        {

        }
        
        private int priority = 1;
        public override Type ExtensionType
        {
            get { return typeof(ScriptReplySubstituteExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = 1; }
        }
    }

}
