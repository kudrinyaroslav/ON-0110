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
    Name             = "H264 Video Streaming",
    Category         = Category.ProfileS,
    Id               = "3",
    FeatureUnderTest = Feature.H264Streaming
    )]
  public class H264Test : BaseVideoStreamingTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      ProcessConversation(conversation, "H264");
    }
  }
}