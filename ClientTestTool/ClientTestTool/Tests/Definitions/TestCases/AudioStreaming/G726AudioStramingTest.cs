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
        Name = "G.726",
        Id = "3",
        Category = Category.ProfileS,
        FeatureUnderTest = Feature.AudioStreamingG726
    )]
    public class G726AudioStramingTest : BaseAudioStreamingTest
    {
         protected override void ProcessConversation(Conversation conversation)
         {
             ProcessConversation(conversation, "726-");
         }

    }
}
