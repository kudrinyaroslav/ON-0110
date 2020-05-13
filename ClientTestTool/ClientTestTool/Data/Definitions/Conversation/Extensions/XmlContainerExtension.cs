///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Xml;
using System.Xml.Linq;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Parsers.Frames.Enums;

namespace ClientTestTool.Data.Definitions.Conversation.Extensions
{
  internal static class XmlContainerExtension
  {
    /// <summary>
    /// Determines whether the specified message contains tag.
    /// </summary>
    public static bool ContainsTag(this XmlContainer message, String tag)
    {
      if (String.IsNullOrEmpty(tag))
        throw new ArgumentException("tag");

      if (!message.ContainsXml)
        return false;

      try
      {
        XElement doc = XElement.Parse(message.GetXmlString(XmlNamespaceOption.IgnoreNamespaces));

        return null != doc.GetElementWithName(tag);
      }
      catch (XmlException e)
      {
        Logger.LogException(String.Empty, e);
        return false;
      }
    }

    /// <summary>
    /// Determines whether the specified message contains tag under the parentTag.
    /// </summary>
    public static bool ContainsTag(this XmlContainer message, String parentTag, String tag)
    {
      if (String.IsNullOrEmpty(parentTag))
        throw new ArgumentException("parentTag");

      if (String.IsNullOrEmpty(tag))
        throw new ArgumentException("tag");

      try
      {
        XElement doc = XElement.Parse(message.GetXmlString(XmlNamespaceOption.IgnoreNamespaces));

        var parentElement = doc.GetElementWithName(parentTag);

        return null != parentElement && null != parentElement.GetElementWithName(tag);
      }
      catch (XmlException e)
      {
        Logger.LogException(String.Empty, e);
        return false;
      }
    }
  }
}
