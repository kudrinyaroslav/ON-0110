///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
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

namespace ClientTestTool.Tests.Definitions.TestCases.NetworkConfiguration
{
  [Test(
    Name             = "Get Network Default Gateway",
    Category         = Category.Core,
    Id               = "3",
    FeatureUnderTest = Feature.GetNetworkDefaultGateway
  )]
  public class GetNetworkDefaultGatewayTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
        var filteredList = conversation.GetMessages(ContentType.Http);

        //[S1] Client request contains “<GetNetworkDefaultGateway>” tag after the “<Body>” tag 
        var getNetworkDefaultGatewayList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetNetworkDefaultGateway")).ToList();
        AffectedPairs.AddRange(getNetworkDefaultGatewayList);

        BeginStep("Client request contains <GetNetworkDefaultGateway> tag", getNetworkDefaultGatewayList);

        if (0 == getNetworkDefaultGatewayList.Count)
            throw new TestNotSupportedException("Conversation does not contain messages with <GetNetworkDefaultGateway> tag");

        StepCompleted();

        //[S2] Device response contains “HTTP/* 200 OK” 
        var responseList = getNetworkDefaultGatewayList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response does not contain 200 OK");

        StepCompleted();

        //[S3] Device response contains “<GetNetworkDefaultGatewayResponse>
        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetNetworkDefaultGatewayResponse")).ToList();

        BeginStep("Device response contains <GetNetworkDefaultGatewayResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<GetNetworkDefaultGatewayResponse> tag is missing");

        StepCompleted();
    }
  }
}
