///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Enums;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.Capabilities
{
  [Test(
    Name              = "Get Services",
    Category          = Category.Core,
    Id                = "1",
    FeatureUnderTest  = Feature.GetServices
  )]
  public class GetServicesTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getServicesList = conversation.GetMessages(ContentType.Http);
      getServicesList = getServicesList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetServices")).ToList();

      AffectedPairs.AddRange(getServicesList);

      BeginStep("Client request contains <GetServices> tag", getServicesList);

      if (!getServicesList.Any())
        throw new TestNotSupportedException("Conversation does not contain requests with <GetServices> tag");

      StepCompleted();

      var includeCapabilityList =
        conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpMessage>().ContainsTag("IncludeCapability")).ToList();

      if (!includeCapabilityList.Any())
      {
        BeginStep("Client request does not contain <IncludeCapability> tag", includeCapabilityList);
        StepCompleted();
      }
      else
      {
        BeginStep("<GetServices> includes tag: <IncludeCapability> with either TRUE OR FALSE values", includeCapabilityList);

        if (!includeCapabilityList.Intersect(getServicesList)
            .Any(item =>
                 TestUtil.ValueOf(item.GetRequest<HttpRequest>(), "GetServices").ToUpper().Contains("TRUE") ||
                 TestUtil.ValueOf(item.GetRequest<HttpRequest>(), "GetServices").ToUpper().Contains("FALSE")))
          StepFailed("Valid value of <IncludeCapability> tag is not present");

        StepCompleted();
      }

      var responseList = getServicesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("GetServicesResponse")).ToList();

      BeginStep("Device response contains <GetServicesResponse> tag", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("GetServicesResponse tag missing");

      StepCompleted();
    }
  }
}
