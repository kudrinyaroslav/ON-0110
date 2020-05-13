///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
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
      Name             = "Get Network Interfaces",
      Id               = "1",
      Category         = Category.Core,
      FeatureUnderTest = Feature.GetNetworkInterfaces
  )]
  public class GetNetworkInterfacesTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
        var filteredList = conversation.GetMessages(ContentType.Http);

        //[S1] Client request contains “<GetNetworkInterfaces>” tag after the “<Body>” tag 
        var getNetworkInterfacesList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetNetworkInterfaces")).ToList();
        AffectedPairs.AddRange(getNetworkInterfacesList);

        BeginStep("Client request contains <GetNetworkInterfaces> tag", getNetworkInterfacesList);

        if (0 == getNetworkInterfacesList.Count)
            throw new TestNotSupportedException("Conversation does not contain messages with <GetNetworkInterfaces> tag");

        StepCompleted();

        //[S2] Device response contains “HTTP/* 200 OK” 
        var responseList = getNetworkInterfacesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response does not contain 200 OK");

        StepCompleted();

        //[S3] Device response contains “<GetNetworkInterfacesResponse>
        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetNetworkInterfacesResponse")).ToList();

        BeginStep("Device response contains <GetNetworkInterfacesResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<GetNetworkInterfacesResponse> tag is missing");

        StepCompleted();

    }
  }
}
