///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Definitions.TestCases.IpAddressFiltering.Base;

namespace ClientTestTool.Tests.Definitions.TestCases.IpAddressFiltering
{
    [Test(
      Name = "Add IPv4 Address Filter",
      Id = "4",
      Category = Category.Core,
      FeatureUnderTest = Feature.AddIpV4AddressFilter
    )]

    public class AddIpV4AddressFilterTest : BaseIpAddressFiltering
    {
        protected override void ProcessConversation(Conversation conversation)
        {
            ProcessConversation(conversation, "v4", "Add", 32);
        }

    }
}
