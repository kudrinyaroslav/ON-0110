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

namespace ClientTestTool.Tests.Definitions.TestCases.VideoSourceConfigurations {
  [Test(
    Name = "Get Specific Video Source Configuration",
    Category = Category.ProfileS,
    Id = "2",
    FeatureUnderTest = Feature.GetVideoSourceConfiguration
    )]
  public class GetVSConfigurationTest : BaseTest 
  {
    protected override void ProcessConversation(Conversation conversation) 
    {
        List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

        List<RequestResponsePair> getVideoSourceConfigurationList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoSourceConfiguration")).ToList();
        AffectedPairs.AddRange(getVideoSourceConfigurationList);

        //[S1] Client request contains “<GetVideoSourceConfiguration>” tag after the “<Body>
        BeginStep("Client request contains <GetVideoSourceConfiguration> tag", getVideoSourceConfigurationList);

        if (0 == getVideoSourceConfigurationList.Count)
            throw new TestNotSupportedException("Conversation does not contain messages with <GetVideoSourceConfiguration> tag");

        StepCompleted();

        //[S2] “<GetVideoSourceConfiguration>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token
        List<RequestResponsePair> configurationTokenList = getVideoSourceConfigurationList.Where(item =>
            TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoSourceConfiguration", "ConfigurationToken")).ToList();
        List<string> valuesList = configurationTokenList.Select(item => 
            TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

        BeginStep("<GetVideoSourceConfiguration> includes tag: <ConfigurationToken> with non-empty string value", configurationTokenList);

        if (0 == configurationTokenList.Count)
            StepFailed("<ConfigurationToken> tag is missing");
        else if (valuesList.All(String.IsNullOrEmpty))
            StepFailed("Value of <ConfigurationToken> tag is empty");

        StepCompleted();

        //[S3] Device response contains “HTTP/* 200 OK” 
        List<RequestResponsePair> responseList = configurationTokenList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response does not contain 200 OK");

        StepCompleted();

        //[S4] Device response contains “<GetVideoSourceConfigurationResponse>” tag
        responseList = responseList.Where(item => 
            TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetVideoSourceConfigurationResponse")).ToList();

        BeginStep("Device response contains <GetVideoSourceConfigurationResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<GetVideoSourceConfigurationResponse> tag is missing");

        StepCompleted();

    }
  }
}