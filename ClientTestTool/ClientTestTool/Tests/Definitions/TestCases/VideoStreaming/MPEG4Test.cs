///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Definitions.TestCases.VideoStreaming.Base;

namespace ClientTestTool.Tests.Definitions.TestCases.VideoStreaming
{
  [Test(
    Name             = "MPEG4 Video Streaming",
    Category         = Category.ProfileS,
    Id               = "2",
    FeatureUnderTest = Feature.MPEG4Streaming
   )]
  public class MPEG4Test : BaseVideoStreamingTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      ProcessConversation(conversation, "MPEG4");
    }
  }
}
