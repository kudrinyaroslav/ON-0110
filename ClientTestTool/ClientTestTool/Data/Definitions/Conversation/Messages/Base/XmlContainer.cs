///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Xml.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Utils.SOAP;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Parsers.Frames;
using ClientTestTool.Parsers.Frames.Enums;

namespace ClientTestTool.Data.Definitions.Conversation.Messages.Base
{
  public abstract class XmlContainer : BaseMessage
  {
    protected XmlContainer(Frame frame, Conversation conversation, MessageType type, ContentType contentType)
      : base(frame, conversation, type, contentType)
    {
      mContainsXml = new Lazy<bool>(() => null != mFrame && 0 != FrameNumber && new FrameLoader(mConversation, mFrame, Type).ContainsXml());
    }

    #region Properties

    private readonly Lazy<bool> mContainsXml;

    public bool ContainsXml
    {
      get
      {
        return mContainsXml.Value;
      }
    }

    #endregion

    /// <summary>
    /// Xml content
    /// </summary>
    public String GetXmlString(XmlNamespaceOption option = XmlNamespaceOption.None)
    {
      return !ContainsXml ? String.Empty : new FrameLoader(mConversation, mFrame, Type).GetXmlString(option);
    }

    public override String GetContent()
    {
      return GetXmlString();
    }

    /// <summary>
    /// Details to show (first tag after SOAP Body)
    /// </summary>
    public override String GetDetails()
    {
      if (!ContainsXml)
        return String.Empty;

      if (null != mDetails)
        return mDetails;

      try
      {
        XElement doc = XElement.Parse(GetXmlString(XmlNamespaceOption.IgnoreNamespaces));

        var element = doc.Element(SoapOptions.BODY_TAG);
        mDetails = element.Elements().First().Name.LocalName;
      }
      catch (Exception e)
      {
        Logger.WriteLine(String.Format("Frame:{0}{1}Conversation:{2}{1}{3}", FrameNumber, Environment.NewLine,
          ConversationList.GetConversations().IndexOf(mConversation) + 1, e.Message));

        mDetails = String.Empty;
      }

      return mDetails;
    }
  }
}
