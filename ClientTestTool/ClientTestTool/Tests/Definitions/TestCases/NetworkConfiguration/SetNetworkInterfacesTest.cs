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
    Name             = "Set Network Interfaces",
    Category         = Category.Core,
    Id               = "2",
    FeatureUnderTest = Feature.SetNetworkInterfaces
  )]
  public class SetNetworkInterfacesTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
        var filteredList = conversation.GetMessages(ContentType.Http);

        var setNetworkInterfacesList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNetworkInterfaces")).ToList();
        AffectedPairs.AddRange(setNetworkInterfacesList);

        //[S1] Client request contains “<SetNetworkInterfaces>” tag after the “<Body>
        BeginStep("Client request contains <SetNetworkInterfaces> tag", setNetworkInterfacesList);

        if (0 == setNetworkInterfacesList.Count)
            throw new TestNotSupportedException("Conversation does not contain messages with <SetNetworkInterfaces> tag");

        StepCompleted();

        //[S2] “<SetNetworkInterfaces>” includes tag: “<InterfaceToken>” with non-empty string value of specific token
        var interfaceTokenList = setNetworkInterfacesList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNetworkInterfaces", "InterfaceToken")).ToList();
        var valuesList = interfaceTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "InterfaceToken")).ToList();

        BeginStep("<SetNetworkInterfaces> includes tag: <ProfileToken> with non-empty string value of specific token", interfaceTokenList);

        if (0 == interfaceTokenList.Count)
            StepFailed("<InterfaceToken> tag is missing");
        else if (valuesList.All(String.IsNullOrEmpty))
            StepFailed("Value of <InterfaceToken> tag is empty");

        StepCompleted();

        //[S3] “<setNetworkInterfaces>” includes tag: “<NetworkInterface>” 
        var networkInterfaceList = setNetworkInterfacesList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNetworkInterfaces", "NetworkInterface")).ToList();
        AffectedPairs.AddRange(networkInterfaceList);

        BeginStep("<SetNetworkInterfaces> includes tag: <NetworkInterface>", networkInterfaceList);

        if (0 == networkInterfaceList.Count)
            StepFailed("<NetworkInterface> tag is missing");

        StepCompleted();

        //[S4] Device response contains “HTTP/* 200 OK” 
        var responseList = setNetworkInterfacesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response does not contain 200 OK");

        StepCompleted();

        //[S5] Device response contains “<SetNetworkInterfacesResponse>” tag
        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetNetworkInterfacesResponse")).ToList();

        BeginStep("Device response contains <SetNetworkInterfacesResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<SetNetworkInterfacesResponse> tag is missing");

        StepCompleted();
    }
  }
}
