///
/// @Author Matthew Tuusberg
///

using System;
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

namespace ClientTestTool.Tests.Definitions.TestCases.ZeroConfiguration
{
  [Test(
    Name             = "Set Zero Configuration",
    Id               = "2",
    Category         = Category.Core,
    FeatureUnderTest = Feature.SetZeroConfiguration
  )]
  public class SetZeroConfigurationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetZeroConfiguration")).ToList();

      AffectedPairs.AddRange(filteredList);

      //S1
      BeginStep("Client request contains <SetZeroConfiguration> tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <SetZeroConfiguration> tag");

      StepCompleted();

      var tokenList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetZeroConfiguration", "InterfaceToken")).ToList();
      var validTokenList = tokenList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "InterfaceToken"))).ToList();

      //S2
      BeginStep("<SetZeroConfiguration>” includes tag: <InterfaceToken> with non-empty string value of specific token", tokenList);

      if (0 == tokenList.Count)
        StepFailed("<InterfaceToken> tag is missing");
      else if (0 == validTokenList.Count)
        StepFailed("Value of <InterfaceToken> tag is empty");

      StepCompleted();


      var enabledList = tokenList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetZeroConfiguration", "Enabled")).ToList();
      var validEnabledList = enabledList.Where(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Enabled");

        return null != value && (value.ToUpper().Contains("TRUE") || value.ToUpper().Contains("FALSE"));
      }).ToList();

      //S3
      BeginStep("<SetZeroConfiguration>” includes tag: “<Enabled>” with “TRUE” OR ”FALSE” value", enabledList);

      if (0 == enabledList.Count)
        StepFailed("<InterfaceToken> tag is missing");
      else if (0 == validEnabledList.Count)
        StepFailed("Value of <Enabled> tag is not valid");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S4
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetZeroConfigurationResponse")).ToList();

      //S5
      BeginStep("Device response contains <SetZeroConfigurationResponse>", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<SetZeroConfigurationResponse> tag is missing");

      StepCompleted();
    }

  }

}

