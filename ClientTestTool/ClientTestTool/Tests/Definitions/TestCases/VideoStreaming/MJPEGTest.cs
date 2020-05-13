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
    Name             = "MJPEG Video Streaming",
    Category         = Category.ProfileS,
    Id               = "1",
    FeatureUnderTest = Feature.MJPEGStreaming
    )]
  public class MJPEGTest : BaseVideoStreamingTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      ProcessConversation(conversation, "JPEG");
    }
  }
}