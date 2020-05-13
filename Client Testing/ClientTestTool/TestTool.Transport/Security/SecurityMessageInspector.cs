using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;
using System.Xml.Serialization;

namespace TestTool.Transport.Security
{
    class SecurityMessageInspector : IDispatchMessageInspector
    {
        private ILogger _logger;
        public SecurityMessageInspector(ILogger logger)
        {
            _logger = logger;
        }

        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            OperationContext ctx = OperationContext.Current;

            
            int headerIndex = -1;
            MessageHeaders headers = OperationContext.Current.IncomingMessageHeaders;
            for (int i = 0; i< headers.Count; i++ )
            {
                MessageHeaderInfo header = headers[i];
                if (header.Name == UsernameTokenHeader.NAME && header.Namespace == UsernameTokenHeader.NAMESPACE)
                {
                    headerIndex = i;
                    break;
                }
            }

            if (headerIndex != -1)
            {
                XmlReader reader = OperationContext.Current.IncomingMessageHeaders.GetReaderAtHeader(headerIndex);

                XmlSerializer serializer = new XmlSerializer(typeof(UsernameTokenHeader));
                UsernameTokenHeader header = (UsernameTokenHeader)serializer.Deserialize(reader);
                // header is deserialized OK

                if (header.UsernameToken == null)
                {
                    SafeLog("Authentication failed: no username information in header");
                    CommonUtils.ReturnFault("Sender", "NotAuthorized", "No username token in request");
                }
                else
                {
                    if (!UsernameTokenValidator.Validate(header.UsernameToken))
                    {
                        SafeLog("Authentication failed: no such combination of username and password");
                        CommonUtils.ReturnFault("Sender", "NotAuthorized");
                    }
                }
            }
            else
            {
                SafeLog("Authentication failed: no header in request");
                CommonUtils.ReturnFault("Sender", "NotAuthorized");
            }
            return null;
        }


        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            
        }

        #endregion

        void SafeLog(string message)
        {
            if (_logger != null)
            {
                _logger.LogEvent(message + Environment.NewLine);
            }

        }
    }
}
