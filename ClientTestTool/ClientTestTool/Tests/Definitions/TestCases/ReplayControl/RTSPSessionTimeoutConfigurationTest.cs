///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;

namespace ClientTestTool.Tests.Definitions.TestCases.ReplayControl
{
  [Test(
    Name             = "RTSP Session Timeout Configuration",
    Category         = Category.ProfileG,
    Id               = "6",
    FeatureUnderTest = Feature.RTSPSessionTimeoutConfiguration
  )]
  public class RTSPSessionTimeoutConfigurationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
        var setReplayConfigurationList = conversation.GetMessages(ContentType.Http).Where(item => item.GetRequest<HttpMessage>().ContainsTag("SetReplayConfiguration")).ToList();

        AffectedPairs.AddRange(setReplayConfigurationList);

        BeginStep("Client request contains <SetReplayConfiguration> tag", setReplayConfigurationList);

        if (0 == setReplayConfigurationList.Count)
            throw new TestNotSupportedException("Conversation does not contain requests with <SetReplayConfiguration> tag");

        StepCompleted();

        var configurationList = setReplayConfigurationList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("SetReplayConfiguration", "Configuration")).ToList();

        BeginStep("<SetReplayConfiguration> includes tag: <Configuration>", configurationList);

        if (0 == configurationList.Count)
            StepFailed("<SetReplayConfiguration> does not include tag: <Configuration>");

        StepCompleted();

        var sessionTimeoutList = configurationList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("Configuration", "SessionTimeout")).ToList();

        BeginStep("<Configuration> includes tag: <SessionTimeout>", sessionTimeoutList);

        if (0 == sessionTimeoutList.Count)
            StepFailed("<Configuration> does not include tag: <SessionTimeout>");

        StepCompleted();

        var responseList = setReplayConfigurationList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response HTTP 200 OK is not present");

        StepCompleted();

        responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("SetReplayConfigurationResponse")).ToList();

        BeginStep("Device response contains “<SetReplayConfigurationResponse>” tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<SetReplayConfigurationResponse> tag is not present");

        StepCompleted();

        var getReplayConfigurationList = conversation.GetMessages(ContentType.Http)
            .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetReplayConfiguration"))
            .ToList();

        AffectedPairs.AddRange(getReplayConfigurationList);

        BeginStep("Client request contains <GetReplayConfiguration> tag", getReplayConfigurationList);

        if (0 == getReplayConfigurationList.Count)
            throw new TestNotSupportedException("Conversation does not contain requests with <GetReplayConfiguration> tag");

        StepCompleted();

        responseList = getReplayConfigurationList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

        BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response HTTP 200 OK is not present");

        StepCompleted();

        responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("GetReplayConfigurationResponse")).ToList();

        BeginStep("Device response contains <GetReplayConfigurationResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<GetReplayConfigurationResponse> tag is not present");

        StepCompleted();
    }
  }
}
