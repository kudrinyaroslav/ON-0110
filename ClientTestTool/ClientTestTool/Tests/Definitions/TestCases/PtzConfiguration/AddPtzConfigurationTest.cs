///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.PtzConfiguration
{
  [Test(
    Name             = "Add PTZ Configuration",
    Id               = "1",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.AddPtzConfiguration
  )]
  public class AddPtzConfigurationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var getConfigurationsList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetConfigurations")).ToList();
      AffectedPairs.AddRange(getConfigurationsList);

      BeginStep("Client request contains <GetConfigurations> tag", getConfigurationsList);

      if (0 == getConfigurationsList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetConfigurations> tag");

      StepCompleted();

      var responseList = getConfigurationsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode)
                       .ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = getConfigurationsList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("GetConfigurationsResponse")).ToList();

      BeginStep("Device response contains <GetConfigurationsResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain <GetConfigurationsResponse> tag");

      StepCompleted();

      var addPtzConfigurationList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddPTZConfiguration")).ToList();
      AffectedPairs.AddRange(addPtzConfigurationList);

      //  [S4] Client request contains “<AddPTZConfiguration>” tag after the “<Body>” tag 
      BeginStep("Client request contains <AddPTZConfiguration> tag", addPtzConfigurationList);

      if (0 == addPtzConfigurationList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <AddPTZConfiguration> tag");

      StepCompleted();

      var profileTokenList = addPtzConfigurationList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddPTZConfiguration", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<AddPTZConfiguration> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      var configurationTokenList = addPtzConfigurationList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddPTZConfiguration", "ConfigurationToken")).ToList();
      valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

      BeginStep("<AddPTZConfiguration> includes tag: <ConfigurationToken> with non-empty string value of specific token", configurationTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ConfigurationToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ConfigurationToken> tag is empty");

      StepCompleted();

      responseList = addPtzConfigurationList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "AddPTZConfigurationResponse")).ToList();

      BeginStep("Device response contains <AddPTZConfigurationResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<AddPTZConfigurationResponse> tag is missing");

      StepCompleted();
    }
  }
}
