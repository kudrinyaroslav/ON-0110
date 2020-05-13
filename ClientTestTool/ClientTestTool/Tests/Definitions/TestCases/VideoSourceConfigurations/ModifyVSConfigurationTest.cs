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
    Name = "Modify Video Source Configuration",
    Category = Category.ProfileS,
    Id = "3",
    FeatureUnderTest = Feature.ModifyVideoSourceConfiguration
    )]
  public class ModifyVSConfigurationTest : BaseTest
  {
   protected override void ProcessConversation(Conversation conversation)
   {
       List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

       List<RequestResponsePair> getGetCompatibleVideoSourceConfigurationsList = filteredList.Where(item =>
           TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetCompatibleVideoSourceConfigurations")).ToList();
       AffectedPairs.AddRange(getGetCompatibleVideoSourceConfigurationsList);

       //[S1] Client request contains “<GetCompatibleVideoSourceConfigurations>” tag after the “<Body>
       BeginStep("Client request contains <GetCompatibleVideoSourceConfigurations> tag", getGetCompatibleVideoSourceConfigurationsList);

       if (0 == getGetCompatibleVideoSourceConfigurationsList.Count)
           throw new TestNotSupportedException("Conversation does not contain messages with <GetCompatibleVideoSourceConfigurations> tag");

       StepCompleted();

       //[S2] “<GetCompatibleVideoSourceConfigurations>” includes tag: “<ProfileToken>” with non-empty string value of specific token
       List<RequestResponsePair> profileTokenList = getGetCompatibleVideoSourceConfigurationsList.Where(item =>
           TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetCompatibleVideoSourceConfigurations", "ProfileToken")).ToList();
       List<string> valuesList = profileTokenList.Select(item =>
           TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

       BeginStep("<GetCompatibleVideoSourceConfigurations> includes tag: <ProfileToken> with non-empty string value", profileTokenList);

       if (0 == profileTokenList.Count)
           StepFailed("<ProfileToken> tag is missing");
       else if (valuesList.All(String.IsNullOrEmpty))
           StepFailed("Value of <ProfileToken> tag is empty");

       StepCompleted();

       //[S3] Device response contains “HTTP/* 200 OK” 
       List<RequestResponsePair> responseList = profileTokenList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

       BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("Response does not contain 200 OK");

       StepCompleted();

       //[S4] Device response contains “<GetCompatibleVideoSourceConfigurationsResponse>” tag
       responseList = responseList.Where(item =>
           TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetCompatibleVideoSourceConfigurationsResponse")).ToList();

       BeginStep("Device response contains <GetCompatibleVideoSourceConfigurationsResponse> tag", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("<GetCompatibleVideoSourceConfigurationsResponse> tag is missing");

       StepCompleted();


       //[S5] Client request contains “<GetVideoSourceConfigurationOptions>” tag after the “<Body>
       List<RequestResponsePair> getVideoSourceConfigurationOptionsList = filteredList.Where(item =>
           TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoSourceConfigurationOptions")).ToList();
       AffectedPairs.AddRange(getVideoSourceConfigurationOptionsList);
       BeginStep("Client request contains <GetVideoSourceConfigurationOptions> tag", getVideoSourceConfigurationOptionsList);

       if (0 == getVideoSourceConfigurationOptionsList.Count)
           throw new TestNotSupportedException("Conversation does not contain messages with <GetVideoSourceConfigurationOptions> tag");

       StepCompleted();

       //[S6] Client request does not contain “<ConfigurationToken>” tag OR
       //[S6] “<GetVideoSourceConfigurationOptions>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token

       List<RequestResponsePair> confTokenList = getVideoSourceConfigurationOptionsList.Where(item =>
           TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();
       if (confTokenList.Count == 0)
       {
           BeginStep("Client request does not contain “<ConfigurationToken>” tag", getVideoSourceConfigurationOptionsList);
           StepCompleted();
       }
       else
       {
           //[S6] “<GetVideoSourceConfigurationOptions>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token
           List<RequestResponsePair> configurationTokenList = getVideoSourceConfigurationOptionsList.Where(item =>
               TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoSourceConfigurationOptions", "ConfigurationToken")).ToList();
           List<String> valuesList2 = configurationTokenList.Select(item =>
               TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

           BeginStep("<GetVideoSourceConfigurationOptions> includes tag: <ConfigurationToken> with non-empty string value",
               configurationTokenList);

           if (0 == configurationTokenList.Count)
               StepFailed("<ConfigurationToken> tag is missing");
           else if (valuesList2.All(String.IsNullOrEmpty))
               StepFailed("Value of <ConfigurationToken> tag is empty");

           StepCompleted();
       }

       //[S7] Client request does not contain “<ProfileToken>” tag OR
       //[S7] “<GetVideoSourceConfigurationOptions>” includes tag: “<ProfileToken>” with non-empty string value of specific token
       List<RequestResponsePair> profTokenList = getVideoSourceConfigurationOptionsList.Where(item =>
           TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();
       if (profTokenList.Count == 0)
       {
           BeginStep("Client request does not contain “<ProfileToken>” tag", getVideoSourceConfigurationOptionsList);
           StepCompleted();
       }
       else
       {
           //[S7] “<GetVideoSourceConfigurationOptions>” includes tag: “<ProfileToken>” with non-empty string value of specific token
           List<RequestResponsePair> configurationTokenList = getVideoSourceConfigurationOptionsList.Where(item =>
               TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoSourceConfigurationOptions", "ProfileToken")).ToList();
           List<string> valuesList3 = configurationTokenList.Select(item =>
               TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

           BeginStep("<GetVideoSourceConfigurationOptions> includes tag: <ProfileToken> with non-empty string value of specific token",
               configurationTokenList);

           if (0 == configurationTokenList.Count)
               StepFailed("<ProfileToken> tag is missing");
           else if (valuesList3.All(String.IsNullOrEmpty))
               StepFailed("Value of <ProfileToken> tag is empty");

           StepCompleted();
       }


       //[S8] Device response contains “HTTP/* 200 OK”
       responseList = getVideoSourceConfigurationOptionsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

       BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("Response does not contain 200 OK");

       StepCompleted();

       //[S9] Device response contains “<GetVideoSourceConfigurationOptionsResponse>” tag
       responseList = responseList.Where(item =>
           TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetVideoSourceConfigurationOptionsResponse")).ToList();

       BeginStep("Device response contains <GetVideoSourceConfigurationOptionsResponse> tag", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("<GetVideoSourceConfigurationOptionsResponse> tag is missing");

       StepCompleted();

       List<RequestResponsePair> setVideoSourceConfigurationList = filteredList.Where(item =>
             TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetVideoSourceConfiguration")).ToList();
       AffectedPairs.AddRange(setVideoSourceConfigurationList);

       //[S10] Client request contains “<SetVideoSourceConfiguration>” tag after the “<Body>
       BeginStep("Client request contains <SetVideoSourceConfiguration> tag", setVideoSourceConfigurationList);

       if (0 == setVideoSourceConfigurationList.Count)
           throw new TestNotSupportedException("Conversation does not contain messages with <SetVideoSourceConfiguration> tag");

       StepCompleted();

       //[S11] “<SetVideoSourceConfiguration>” includes tag: “<Configuration>” with non-empty string value of “Token=*” parameter 
       List<RequestResponsePair> setVideoSourceConfigurationTokenList = setVideoSourceConfigurationList.Where(item =>
           TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetVideoSourceConfiguration", "Configuration")).ToList();

       var configurationTokenAttributesList = setVideoSourceConfigurationList.Select(item =>
            TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "Configuration")).ToList();


       BeginStep("<SetVideoSourceConfiguration>” includes tag: “<Configuration>” with non-empty string value of “Token=*” parameter", setVideoSourceConfigurationTokenList);

       if (0 == setVideoSourceConfigurationTokenList.Count)
           StepFailed("<Configuration> tag is missing");
       else if (!configurationTokenAttributesList.Any(item =>
       {
           var x = item.FirstOrDefault(attr => "token" == attr.Name);
           return null != x && !String.IsNullOrEmpty(x.Value);
       }))
           StepFailed("Value of “token” parameter tag is empty"); //StepFailed("<Configuration> tag is missing");


       StepCompleted();

       //[S12] Is not implemented yet
       //[S13] “<SetVideoSourceConfiguration>” includes tag: “<ForcePersistence>” with either “TRUE” OR “FALSE”
       List<RequestResponsePair> setVideoSourceConfigurationForcePersistence = setVideoSourceConfigurationList.Where(item =>
           TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetVideoSourceConfiguration", "ForcePersistence")).ToList();
       List<String> valuesList5 = setVideoSourceConfigurationForcePersistence.Select(item =>
           TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ForcePersistence")).ToList();

       valuesList5.Where(item =>
       {
           bool bTrue = item.Trim().ToUpper().Equals("TRUE");
           bool bFalse = item.Trim().ToUpper().Equals("FALSE");
           return (bFalse || bTrue);
       }).ToList();

       BeginStep("<SetVideoSourceConfiguration> includes tag: <ForcePersistence> with either “TRUE” OR “FALSE” value", setVideoSourceConfigurationTokenList);

       if (0 == setVideoSourceConfigurationForcePersistence.Count)
           StepFailed("<ForcePersistence> tag is missing");
       else if (valuesList5.Count == 0)
           StepFailed("Value of <ForcePersistence> tag is not “TRUE” neither “FALSE”");

       StepCompleted();

       //[S14] Device response contains “HTTP/* 200 OK”
       responseList = setVideoSourceConfigurationList.Where(
           item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

       BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("Response does not contain 200 OK");

       StepCompleted();

       //[S15] Device response contains “<SetVideoSourceConfigurationResponse>” tag
       responseList = responseList.Where(item =>
           TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetVideoSourceConfigurationResponse")).ToList();

       BeginStep("Device response contains <SetVideoSourceConfigurationResponse> tag", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("<SetVideoSourceConfigurationResponse> tag is missing");

       StepCompleted();

       List<RequestResponsePair> getVideoSourceConfigurationList = filteredList.Where(item =>
             TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoSourceConfiguration")).ToList();
       AffectedPairs.AddRange(getVideoSourceConfigurationList);

       //[S16] Client request contains “<GetVideoSourceConfiguration>” tag after the “<Body>
       BeginStep("Client request contains <GetVideoSourceConfiguration> tag", getVideoSourceConfigurationList);

       if (0 == getVideoSourceConfigurationList.Count)
           throw new TestNotSupportedException("Conversation does not contain messages with <GetVideoSourceConfiguration> tag");

       StepCompleted();

       //[S17] “<GetVideoSourceConfiguration>” includes tag: “<ConfigurationToken>” with non-empty string value of specific token
       List<RequestResponsePair> configurationTokenList2 = getVideoSourceConfigurationList.Where(
           item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetVideoSourceConfiguration", "ConfigurationToken")).ToList();
       List<string> valuesList6 = configurationTokenList2.Select(item =>
           TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ConfigurationToken")).ToList();

       BeginStep("<GetVideoSourceConfiguration> includes tag: <ConfigurationToken> with non-empty string value", configurationTokenList2);

       if (0 == configurationTokenList2.Count)
           StepFailed("<ConfigurationToken> tag is missing");
       else if (valuesList6.All(String.IsNullOrEmpty))
           StepFailed("Value of <ConfigurationToken> tag is empty");

       StepCompleted();

       //[S18] Device response contains “HTTP/* 200 OK”
       responseList = getVideoSourceConfigurationList.Where(
           item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

       BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("Response does not contain 200 OK");

       StepCompleted();

       //[S19] Device response contains “<GetVideoSourceConfigurationResponse>” tag
       responseList = responseList.Where(item =>
           TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetVideoSourceConfigurationResponse")).ToList();

       BeginStep("Device response contains <GetVideoSourceConfigurationResponse> tag", responseList, MessageType.Response);

       if (0 == responseList.Count)
           StepFailed("<GetVideoSourceConfigurationResponse> tag is missing");

       StepCompleted();

   }
  }
}

