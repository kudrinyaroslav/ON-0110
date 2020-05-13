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

namespace ClientTestTool.Tests.Definitions.TestCases.AudioStreaming
{
  [Test(
    Name             = "Configure Media Profile",
    Id               = "1",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.AudioStreamingConfigureMediaProfile
  )]
  public class ConfigureMediaProfileASTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var getCompatibleAudioSourceConfigurationsList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetCompatibleAudioSourceConfigurations")).ToList();
      AffectedPairs.AddRange(getCompatibleAudioSourceConfigurationsList);

      //[S1] Client request contains <getCompatibleAudioSourceConfigurations> tag after the <Body>
      BeginStep("Client request contains <GetCompatibleAudioSourceConfigurations> tag", getCompatibleAudioSourceConfigurationsList);

      if (0 == getCompatibleAudioSourceConfigurationsList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetCompatibleAudioSourceConfigurations> tag");

      StepCompleted();

      //[S2] <GetCompatibleAudioSourceConfigurations> includes tag: <﻿ProfileToken> with non-empty string value of specific token
      var profileTokenList = getCompatibleAudioSourceConfigurationsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetCompatibleAudioSourceConfigurations", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<GetCompatibleAudioSourceConfigurations> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S3] Device response contains HTTP/* 200 OK
      var responseList = getCompatibleAudioSourceConfigurationsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S4] Device response contains <GetCompatibleAudioSourceConfigurationsResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetCompatibleAudioSourceConfigurationsResponse")).ToList();

      BeginStep("Device response contains <GetCompatibleAudioSourceConfigurationsResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetCompatibleAudioSourceConfigurationsResponse> tag is missing");

      StepCompleted();

      ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

      var addAudioSourceConfigurationList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddAudioSourceConfiguration")).ToList();
      AffectedPairs.AddRange(addAudioSourceConfigurationList);

      //[S5] Client request contains <AddAudioSourceConfiguration> tag after the <Body>
      BeginStep("Client request contains <AddAudioSourceConfiguration> tag", addAudioSourceConfigurationList);

      if (0 == addAudioSourceConfigurationList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <AddAudioSourceConfiguration> tag");

      StepCompleted();

      //[S6] <AddAudioSourceConfiguration> includes tag: <﻿ProfileToken> with non-empty string value of specific token
      profileTokenList = addAudioSourceConfigurationList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddAudioSourceConfiguration", "ProfileToken")).ToList();
      valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<AddAudioSourceConfiguration> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S7] <AddAudioSourceConfiguration> includes tag: <﻿ConfigurationToken> with non-empty string value of specific token
      var configurationTokenList = addAudioSourceConfigurationList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddAudioSourceConfiguration", "ConfigurationToken")).ToList();
      valuesList = configurationTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

      BeginStep("<AddAudioSourceConfiguration> includes tag: <ConfigurationToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ConfigurationToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ConfigurationToken> tag is empty");

      StepCompleted();


      //[S8] Device response contains HTTP/* 200 OK
      responseList = addAudioSourceConfigurationList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S9] Device response contains <GetCompatibleAudioSourceConfigurationsResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "AddAudioSourceConfigurationResponse")).ToList();

      BeginStep("Device response contains <AddAudioSourceConfigurationResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<AddAudioSourceConfigurationResponse> tag is missing");

      StepCompleted();

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////
      var getCompatibleAudioEncoderConfigurationsList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetCompatibleAudioEncoderConfigurations")).ToList();
      AffectedPairs.AddRange(getCompatibleAudioEncoderConfigurationsList);

      //[S10] Client request contains <GetCompatibleAudioEncoderConfigurations> tag after the <Body>
      BeginStep("Client request contains <GetCompatibleAudioEncoderConfigurations> tag", getCompatibleAudioEncoderConfigurationsList);

      if (0 == getCompatibleAudioEncoderConfigurationsList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetCompatibleAudioEncoderConfigurations> tag");

      StepCompleted();

      //[S12] <GetCompatibleAudioEncoderConfigurations> includes tag: <﻿ProfileToken> with non-empty string value of specific token
      profileTokenList = getCompatibleAudioEncoderConfigurationsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetCompatibleAudioEncoderConfigurations", "ProfileToken")).ToList();
      valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<AddAudioSourceConfiguration> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S13] Device response contains HTTP/* 200 OK
      responseList = getCompatibleAudioEncoderConfigurationsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S14] Device response contains <GetCompatibleAudioSourceConfigurationsResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetCompatibleAudioEncoderConfigurationsResponse")).ToList();

      BeginStep("Device response contains <GetCompatibleAudioEncoderConfigurationsResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetCompatibleAudioEncoderConfigurationsResponse> tag is missing");

      StepCompleted();

      ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
      var addAudioEncoderConfigurationList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddAudioEncoderConfiguration")).ToList();
      AffectedPairs.AddRange(addAudioEncoderConfigurationList);

      //[S15] Client request contains <AddAudioEncoderConfiguration> tag after the <Body>
      BeginStep("Client request contains <AddAudioEncoderConfiguration> tag", addAudioEncoderConfigurationList);

      if (0 == addAudioEncoderConfigurationList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <AddAudioEncoderConfiguration> tag");

      StepCompleted();

      //[S16] <AddAudioEncoderConfiguration> includes tag: <﻿ProfileToken> with non-empty string value of specific token
      profileTokenList = addAudioEncoderConfigurationList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddAudioEncoderConfiguration", "ProfileToken")).ToList();
      valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<AddAudioEncoderConfiguration> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S17] <AddAudioEncoderConfiguration> includes tag: <﻿ConfigurationToken> with non-empty string value of specific token
      configurationTokenList = addAudioEncoderConfigurationList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AddAudioEncoderConfiguration", "ConfigurationToken")).ToList();
      valuesList = configurationTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

      BeginStep("<AddAudioEncoderConfiguration> includes tag: <ConfigurationToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ConfigurationToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ConfigurationToken> tag is empty");

      StepCompleted();


      //[S18] Device response contains HTTP/* 200 OK
      responseList = addAudioEncoderConfigurationList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S19] Device response contains <GetCompatibleAudioSourceConfigurationsResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "AddAudioEncoderConfigurationResponse")).ToList();

      BeginStep("Device response contains <AddAudioEncoderConfigurationResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<AddAudioEncoderConfigurationResponse> tag is missing");

      StepCompleted();

    }
  }
}
