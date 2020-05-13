///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.IpAddressFiltering
{
    using ClientTestTool.Tests.Definitions.TestCases.IpAddressFiltering.Base;

    [Test(
      Name = "Remove IPv6 Address Filter",
      Id = "7",
      Category = Category.Core,
      FeatureUnderTest = Feature.RemoveIpV6AddressFilter
    )]

    public class RemoveIpV6AddressFilterTest : BaseIpAddressFiltering
    {
        protected override void ProcessConversation(Conversation conversation)
        {
            ProcessConversation(conversation, "v6", "Remove", 128);
        }

    }
}
