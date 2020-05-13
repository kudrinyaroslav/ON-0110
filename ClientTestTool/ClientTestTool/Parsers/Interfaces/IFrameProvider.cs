///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Parsers.Frames.Enums;

namespace ClientTestTool.Parsers.Interfaces
{
  interface IFrameProvider
  {
    String GetContent(ContentType type);
    String GetXmlString(XmlNamespaceOption option);
  }
}
