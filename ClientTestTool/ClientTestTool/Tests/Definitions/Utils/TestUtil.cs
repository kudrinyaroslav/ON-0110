///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Parsers.Frames.Enums;

namespace ClientTestTool.Tests.Definitions.Utils
{
  public static class TestUtil
  {
    //TODO
    public static List<RequestResponsePair> GetMessages(Conversation conversation, ContentType contentType)
    {
      if (ContentType.Http == contentType)
        return conversation.GetMessages(contentType)
                           .Where(item => ValidationStatus.Passed == item.GetRequest<HttpMessage>().ValidationStatus &&
                                          ValidationStatus.Passed == item.GetResponse<HttpMessage>().ValidationStatus)
                           .ToList();

      if (ContentType.Rtsp == contentType)
        return conversation.GetMessages(contentType)
                           .Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode)
                           .ToList();

      return new List<RequestResponsePair>();
    }

    /// <summary>
    /// /// Returns true if message contains tag under the parent tag
    /// </summary>
    public static bool ContainsTag(XmlContainer message, String tag)
    {
      if (null == message)
        throw new ArgumentNullException();

      if (String.IsNullOrEmpty(tag))
        throw new ArgumentException();

      return message.ContainsTag(tag);
    }

    /// <summary>
    /// Returns true if message contains tag under the parent tag
    /// </summary>
    public static bool ContainsTag(XmlContainer message, String parentTag, String tag)
    {
      if (null == message)
        throw new ArgumentNullException();

      if (String.IsNullOrEmpty(parentTag))
        throw new ArgumentException();

      if (String.IsNullOrEmpty(tag))
        throw new ArgumentException();

      return message.ContainsTag(parentTag, tag);
    }

    /// <summary>
    /// Returns value of given tag in given message, null otherwise
    /// </summary>
    public static String ValueOf(XmlContainer message, String tag)
    {
      if (null == message)
        throw new ArgumentNullException();

      if (String.IsNullOrEmpty(tag))
        throw new ArgumentException();

      XElement doc = XElement.Parse(message.GetXmlString(XmlNamespaceOption.IgnoreNamespaces));

      var element = doc.GetElementWithName(tag);

      return null == element ? null : element.Value;
    }

    /// <summary>
    /// Returns attributes of given tag in given message, null otherwise
    /// </summary>
    public static XAttribute[] AttributesOf(XmlContainer message, String tag)
    {
      if (null == message)
        throw new ArgumentNullException();

      if (String.IsNullOrEmpty(tag))
        throw new ArgumentException();

      XElement doc = XElement.Parse(message.GetXmlString(XmlNamespaceOption.IgnoreNamespaces));

      var element = doc.Descendants().FirstOrDefault(item => item.Name.LocalName == tag);

      return null == element ? null : element.Attributes().ToArray();
    }
  }
}
