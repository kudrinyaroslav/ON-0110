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
        Name = "AAC",
        Id = "4",
        Category = Category.ProfileS,
        FeatureUnderTest = Feature.AudioStreamingAAC
    )]
    public class AacAudioStreamingTest : BaseAudioStreamingTest
    {
         protected override void ProcessConversation(Conversation conversation)
         {
             ProcessConversation(conversation, "MPEG4-GENERIC");
         }
    }
}
