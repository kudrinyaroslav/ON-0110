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

namespace ClientTestTool.Tests.Definitions.TestCases.VideoEncoderConfigurations {
  [Test(
    Name = "Modify Video Encoder Configuration",
    Category = Category.ProfileS,
    Id = "3",
    FeatureUnderTest = Feature.ModifyVideoEncoderConfiguration
    )]
  public class ModifyConfigurationTest : BaseTest 
  {
      protected override void ProcessConversation(Conversation conversation)
      {
          List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

          List<RequestResponsePair> getVideoEncoderConfigurationOptionsList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoEncoderConfigurationOptions")).ToList();
          AffectedPairs.AddRange(getVideoEncoderConfigurationOptionsList);

          //[S1] Client request contains “<GetVideoEncoderConfiguration>” tag after the “<Body>
          BeginStep("Client request contains <GetVideoEncoderConfigurationOptions> tag", getVideoEncoderConfigurationOptionsList);

          if (0 == getVideoEncoderConfigurationOptionsList.Count)
              throw new TestNotSupportedException("Conversation does not contain messages with <GetVideoEncoderConfigurationOptions> tag");

          StepCompleted();

          //[S2] Client request does not contain “<ConfigurationToken>” tag OR
          //[S2] “<GetVideoEncoderConfigurationOptions>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token

          List<RequestResponsePair> confTokenList = getVideoEncoderConfigurationOptionsList.Where(item => 
              TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();
          if (confTokenList.Count == 0)
          {
              BeginStep("Client request does not contain “<ConfigurationToken>” tag", getVideoEncoderConfigurationOptionsList);
              StepCompleted();
          }
          else
          {
              //[S2] “<GetVideoEncoderConfigurationOptions>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token
              List<RequestResponsePair> configurationTokenList = getVideoEncoderConfigurationOptionsList.Where(item =>
                  TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoEncoderConfigurationOptions", "ConfigurationToken")).ToList();
              List<String> valuesList = configurationTokenList.Select(item =>
                  TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

              BeginStep("<GetVideoEncoderConfigurationOptions> includes tag: <ConfigurationToken> with non-empty string value",
                  configurationTokenList);

              if (0 == configurationTokenList.Count)
                  StepFailed("<ConfigurationToken> tag is missing");
              else if (valuesList.All(String.IsNullOrEmpty))
                  StepFailed("Value of <ConfigurationToken> tag is empty");

              StepCompleted();
          }
          //[S3] Client request does not contain “<ProfileToken>” tag OR
          //[S3] “<GetVideoEncoderConfigurationOptions>” includes tag: “<ProfileToken>” with non-empty string value of specific token
          List<RequestResponsePair> profTokenList = getVideoEncoderConfigurationOptionsList.Where(item =>
              TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();
          if (profTokenList.Count == 0)
          {
              BeginStep("Client request does not contain “<ProfileToken>” tag", getVideoEncoderConfigurationOptionsList);
              StepCompleted();
          }
          else
          {
              //[S3] “<GetVideoEncoderConfigurationOptions>” includes tag: “<ProfileToken>” with non-empty string value of specific token
              List<RequestResponsePair> configurationTokenList = getVideoEncoderConfigurationOptionsList.Where(item =>
                  TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoEncoderConfigurationOptions", "ProfileToken")).ToList();
              List<string> valuesList = configurationTokenList.Select(item =>
                  TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

              BeginStep("<GetVideoEncoderConfigurationOptions> includes tag: <ProfileToken> with non-empty string value",
                  configurationTokenList);

              if  ((configurationTokenList.Count > 0) && (valuesList.All(String.IsNullOrEmpty)) )
                  StepFailed("Value of <ProfileToken> tag is empty"); //StepFailed("<ProfileToken> tag is missing");

              StepCompleted();
          }

          //[S4] Device response contains “HTTP/* 200 OK”
          List<RequestResponsePair> responseList = getVideoEncoderConfigurationOptionsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

          BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

          if (0 == responseList.Count)
              StepFailed("Response does not contain 200 OK");

          StepCompleted();

          //[S5] Device response contains “<GetVideoEncoderConfigurationOptionsResponse>” tag
          responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetVideoEncoderConfigurationOptionsResponse")).ToList();

          BeginStep("Device response contains <GetVideoEncoderConfigurationOptionsResponse> tag", responseList, MessageType.Response);

          if (0 == responseList.Count)
              StepFailed("<GetVideoEncoderConfigurationOptionsResponse> tag is missing"); 

          StepCompleted();

          List<RequestResponsePair> setVideoEncoderConfigurationList = filteredList.Where(item => 
              TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetVideoEncoderConfiguration")).ToList();
          AffectedPairs.AddRange(getVideoEncoderConfigurationOptionsList);

          //[S6] Client request contains “<SetVideoEncoderConfiguration>” tag after the “<Body>
          BeginStep("Client request contains <SetVideoEncoderConfiguration> tag", setVideoEncoderConfigurationList);

          if (0 == setVideoEncoderConfigurationList.Count)
              throw new TestNotSupportedException("Conversation does not contain messages with <SetVideoEncoderConfiguration> tag");

          StepCompleted();

          //[S7] “<SetVideoEncoderConfiguration>” includes tag: “<Configuration>” with non-empty string value of “token=*” parameter 
          List<RequestResponsePair> setVideoEncoderConfigurationTokenList = setVideoEncoderConfigurationList.Where(item =>
              TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetVideoEncoderConfiguration", "Configuration")).ToList();

          var configurationTokenAttributesList = setVideoEncoderConfigurationTokenList.Select(item =>
              TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "Configuration")).ToList();
          BeginStep("<SetVideoEncoderConfiguration> includes tag: <Configuration> with non-empty string value of “token=*” parameter ", setVideoEncoderConfigurationTokenList);

          if (0 == setVideoEncoderConfigurationTokenList.Count)
              StepFailed("<Configuration> tag is missing");
          else if (!configurationTokenAttributesList.Any(item =>
          {
              var x = item.FirstOrDefault(attr => "token" == attr.Name );
              return null != x && !String.IsNullOrEmpty(x.Value);
          }))
              StepFailed("Value of “token” parameter tag is empty"); //StepFailed("<Configuration> tag is missing");
          StepCompleted();

          //[S8] Is not implemented yet
          //[S9] “<SetVideoEncoderConfiguration>” includes tag: “<ForcePersistence>” with either “TRUE” OR “FALSE”
          List<RequestResponsePair> setVideoEncoderConfigurationForcePersistence = setVideoEncoderConfigurationList.Where(item =>
              TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetVideoEncoderConfiguration", "ForcePersistence")).ToList();
          List<String> valuesList3 = setVideoEncoderConfigurationForcePersistence.Select(item =>
              TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ForcePersistence")).ToList();

          valuesList3.Where( item => 
          {
             bool bTrue = item.Trim().ToUpper().Equals("TRUE");
             bool bFalse = item.Trim().ToUpper().Equals("FALSE");
             return (bFalse || bTrue);
          }).ToList();

          BeginStep("<SetVideoEncoderConfiguration> includes tag: <ForcePersistence> with either “TRUE” OR “FALSE” value", setVideoEncoderConfigurationTokenList);

          if (0 == setVideoEncoderConfigurationForcePersistence.Count)
              StepFailed("<ForcePersistence> tag is missing");
          else if (valuesList3.Count == 0)
              StepFailed("Value of <ForcePersistence> tag is not “TRUE” neither “FALSE”");

          StepCompleted();

          //[S10] Device response contains “HTTP/* 200 OK”
          responseList = setVideoEncoderConfigurationList.Where(
              item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

          BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

          if (0 == responseList.Count)
              StepFailed("Response does not contain 200 OK");

          StepCompleted();

          //[S11] Device response contains “<SetVideoEncoderConfigurationResponse>” tag
          responseList = responseList.Where(item =>
              TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetVideoEncoderConfigurationResponse")).ToList();

          BeginStep("Device response contains <SetVideoEncoderConfigurationResponse> tag", responseList, MessageType.Response);

          if (0 == responseList.Count)
              StepFailed("<SetVideoEncoderConfigurationResponse> tag is missing");

          StepCompleted();

          List<RequestResponsePair> getVideoEncoderConfigurationList = filteredList.Where(item => 
              TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoEncoderConfiguration")).ToList();
          AffectedPairs.AddRange(getVideoEncoderConfigurationList);

          //[S12] Client request contains “<GetVideoEncoderConfiguration>” tag after the “<Body>
          BeginStep("Client request contains <GetVideoEncoderConfiguration> tag", getVideoEncoderConfigurationList);

          if (0 == getVideoEncoderConfigurationList.Count)
              throw new TestNotSupportedException("Conversation does not contain messages with <GetVideoEncoderConfiguration> tag");

          StepCompleted();

          //[S13] “<GetVideoEncoderConfiguration>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token
          List<RequestResponsePair> configurationTokenList2 = getVideoEncoderConfigurationList.Where(
              item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoEncoderConfiguration", "ConfigurationToken")).ToList();
          List<string> valuesList4 = configurationTokenList2.Select(item => 
              TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

          BeginStep("<GetVideoEncoderConfiguration> includes tag: <ConfigurationToken> with non-empty string value", configurationTokenList2);

          if (0 == configurationTokenList2.Count)
              StepFailed("<ConfigurationToken> tag is missing");
          else if (valuesList4.All(String.IsNullOrEmpty))
              StepFailed("Value of <ConfigurationToken> tag is empty");

          StepCompleted();

          //[S14] Device response contains “HTTP/* 200 OK”
          responseList = getVideoEncoderConfigurationList.Where(
              item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

          BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

          if (0 == responseList.Count)
              StepFailed("Response does not contain 200 OK");

          StepCompleted();

          //[S15] Device response contains “<GetVideoEncoderConfigurationResponse>” tag
          responseList = responseList.Where(item =>
              TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetVideoEncoderConfigurationResponse")).ToList();

          BeginStep("Device response contains <GetVideoEncoderConfigurationResponse> tag", responseList, MessageType.Response);

          if (0 == responseList.Count)
              StepFailed("<GetVideoEncoderConfigurationResponse> tag is missing");

          StepCompleted();


      }
  }
}
