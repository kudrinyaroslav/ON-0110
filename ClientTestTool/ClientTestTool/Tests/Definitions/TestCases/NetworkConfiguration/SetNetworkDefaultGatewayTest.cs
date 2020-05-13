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
    Name             = "Set Network Default Gateway",
    Category         = Category.Core,
    Id               = "4",
    FeatureUnderTest = Feature.SetNetworkDefaultGateway
  )]
  public class SetNetworkDefaultGatewayTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
        var filteredList = conversation.GetMessages(ContentType.Http);

        var setNetworkDefaultGatewayList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNetworkDefaultGateway")).ToList();
        AffectedPairs.AddRange(setNetworkDefaultGatewayList);

        //[S1] Client request contains “<SetNetworkDefaultGateway>” tag after the “<Body>
        BeginStep("Client request contains <SetNetworkDefaultGateway> tag", setNetworkDefaultGatewayList);

        if (0 == setNetworkDefaultGatewayList.Count)
            throw new TestNotSupportedException("Conversation does not contain messages with <SetNetworkDefaultGateway> tag");

        StepCompleted();

        //[S2] [S2] “<SetNetworkDefaultGateway>” includes tag: EITHER “<IPv4Address>” OR “<IPv6Address>” with specific IP address value 
        var iPv4AddressList = setNetworkDefaultGatewayList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNetworkDefaultGateway", "IPv4Address")).ToList();
        var valuesList = iPv4AddressList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "IPv4Address")).ToList();
        var iPv6AddressList = setNetworkDefaultGatewayList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNetworkDefaultGateway", "IPv6Address")).ToList();
        var valuesList2 = iPv6AddressList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "IPv6Address")).ToList();

        BeginStep("<SetNetworkDefaultGateway> includes tag: <ProfileToken> with non-empty string value of specific token", iPv4AddressList);

        if ((0 == iPv4AddressList.Count) && (0 == iPv6AddressList.Count))
            StepFailed("<IPv4Address> or <IPv6Address> tag is missing");
        else if (  (valuesList.All(String.IsNullOrEmpty)) && (valuesList2.All(String.IsNullOrEmpty)) )
            StepFailed("Value of <IPv4Address> or <IPv6Address> tag is empty");

        StepCompleted();

        //[S4] Device response contains “HTTP/* 200 OK” 
        var responseList = setNetworkDefaultGatewayList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response does not contain 200 OK");

        StepCompleted();

        //[S5] Device response contains “<SetNetworkDefaultGatewayResponse>” tag
        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetNetworkDefaultGatewayResponse")).ToList();

        BeginStep("Device response contains <SetNetworkDefaultGatewayResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<SetNetworkDefaultGatewayResponse> tag is missing");

        StepCompleted();

    }
  }
}
