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

namespace ClientTestTool.Tests.Definitions.TestCases.VideoSourceConfigurations
{
  [Test(
    Name = "Add Video Source Configuration",
    Category = Category.ProfileS,
    Id = "4",
    FeatureUnderTest = Feature.AddVideoSourceConfiguration
    )]
  public class AddVSConfigurationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
        List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

        List<RequestResponsePair> addVideoSourceConfigurationList = filteredList.Where(item =>
            TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddVideoSourceConfiguration")).ToList();
        AffectedPairs.AddRange(addVideoSourceConfigurationList);

        //[S1] Client request contains “<GetVideoEncoderConfiguration>” tag after the “<Body>
        BeginStep("Client request contains <AddVideoSourceConfiguration> tag", addVideoSourceConfigurationList);

        if (0 == addVideoSourceConfigurationList.Count)
            throw new TestNotSupportedException("Conversation does not contain messages with <AddVideoSourceConfiguration> tag");

        StepCompleted();

        //[S3] “<AddVideoSourceConfiguration>” includes tag: “<ProfileToken>” with non-empty string value of specific token
        List<RequestResponsePair> profileTokenList = addVideoSourceConfigurationList.Where(item =>
            TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddVideoSourceConfiguration", "ProfileToken")).ToList();
        List<String> valuesList = profileTokenList.Select(item =>
            TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

        BeginStep("<AddVideoSourceConfiguration> includes tag: <ProfileToken> with non-empty string value", profileTokenList);

        if (0 == profileTokenList.Count)
            StepFailed("<ProfileToken> tag is missing");
        else if (valuesList.All(String.IsNullOrEmpty))
            StepFailed("Value of <ProfileToken> tag is empty");

        StepCompleted();


        //[S3] “<AddVideoSourceConfiguration>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token
        List<RequestResponsePair> configurationTokenList = addVideoSourceConfigurationList.Where(item =>
            TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddVideoSourceConfiguration", "ConfigurationToken")).ToList();
        valuesList = configurationTokenList.Select(item => 
            TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

        BeginStep("<AddVideoSourceConfiguration> includes tag: <ConfigurationToken> with non-empty string value", configurationTokenList);

        if (0 == configurationTokenList.Count)
            StepFailed("<ConfigurationToken> tag is missing");
        else if (valuesList.All(String.IsNullOrEmpty))
            StepFailed("Value of <ConfigurationToken> tag is empty");

        StepCompleted();

        //[S4] Device response contains “HTTP/* 200 OK” 
        List<RequestResponsePair> responseList = configurationTokenList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response does not contain 200 OK");

        StepCompleted();

        //[S5] Device response contains “<GetVideoEncoderConfigurationResponse>” tag
        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "AddVideoSourceConfigurationResponse")).ToList();

        BeginStep("Device response contains <AddVideoSourceConfigurationResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<AddVideoSourceConfigurationResponse> tag is missing");

        StepCompleted();

    }
  }
}