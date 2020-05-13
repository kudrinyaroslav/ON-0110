///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Definitions.TestCases.AudioStreaming.Base;

namespace ClientTestTool.Tests.Definitions.TestCases.AudioStreaming
{
  [Test(
      Name             = "G.711",
      Id               = "2",
      Category         = Category.ProfileS,
      FeatureUnderTest = Feature.AudioStreamingG711
  )]
  public class G711AudioStreamingTest : BaseAudioStreamingTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      ProcessConversation(conversation, "PCMU");
    }
  }
}
