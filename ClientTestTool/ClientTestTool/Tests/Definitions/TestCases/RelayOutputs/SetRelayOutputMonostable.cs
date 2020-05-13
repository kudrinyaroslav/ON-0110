///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Collections.Generic;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Definitions.TestCases.RelayOutputs.Base;

namespace ClientTestTool.Tests.Definitions.TestCases.RelayOutputs
{
    [Test(
        Name = "Set Relay Output Settings Monostable Mode",
        Category = Category.Core,
        Id = "4",
        FeatureUnderTest = Feature.SetRelayOutputMonostable
    )]

    class SetRelayOutputMonostable : BaseRelayOutputSettingsTest
    {
        protected override void ProcessConversation(Conversation conversation)
        {
            ProcessConversation(conversation, "Monostable");
        }
    }
}
