using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.HttpTransport.Interfaces;
using System.Xml;

namespace TestTool.Tests.Common.Transport
{
    public class WsaController : IWsaController
    {
        #region IWsaController Members

        private UniqueId _messageId;
        private Uri _replyTo;
        
        public void ProcessRequest(System.ServiceModel.Channels.Message message)
        {
            _messageId = message.Headers.MessageId;

            message.Headers.ReplyTo = new EndpointAddress("http://www.w3.org/2005/08/addressing/anonymous");
            _replyTo = message.Headers.ReplyTo.Uri;
        }

        public void Validate(System.ServiceModel.Channels.Message message)
        {
            if (message.IsFault)
            {
                return;
            }

            if (string.IsNullOrEmpty(message.Headers.Action))
            {
                //[27.03.2013] AKS: distinguish between empty header and header's absence
                if (null == message.Headers.Action)
                    throw new ApplicationException("WS-Addressing \"Action\" header is missing");
                else
                    throw new ApplicationException("WS-Addressing \"Action\" header is empty");
            }

            //if (message.Headers.RelatesTo != null)
            //{
            //    UniqueId relatesTo = message.Headers.RelatesTo;

            //    Guid relatesToId = new Guid();
            //    if (!relatesTo.TryGetGuid(out relatesToId))
            //    {
            //        throw new ApplicationException(string.Format("WS-Addressing \"RelatesTo\" header ({0}) is not correct",
            //                                                     message.Headers.RelatesTo.ToString()));
            //    }

            //    if (_messageId != null)
            //    {
            //        // must match
            //        Guid messageId = new Guid();
            //        if (_messageId.TryGetGuid(out messageId))
            //        {
            //            if (messageId != relatesToId)
            //            {
            //                throw new ApplicationException(string.Format("WS-Addressing \"RelatesTo\" header ({0}) does not match MessageId sent ({1})",
            //                                               relatesToId.ToString(), messageId.ToString()));
            //            }
            //        }
            //    }
            //}

            //Uri to = null;
            //try
            //{
            //    to = message.Headers.To;
            //}
            //catch (Exception exc)
            //{
            //    throw new ApplicationException(string.Format("Failed to understand WS-Addressing \"To\" header: {0}",
            //                                   exc.Message));
            //}
            
            //if (to != null)
            //{
            //    if (_replyTo != null)
            //    {
            //        Uri requestReplyTo = _replyTo;
            //        if (to != requestReplyTo)
            //        {
            //            throw new ApplicationException(string.Format("WS-Addressing \"To\" header ({0}) does not match ReplyTo ({1}) sent",
            //                                                         to.ToString(), requestReplyTo.ToString()));
            //        }
            //    }
            //}

        }

        #endregion

    }
}
