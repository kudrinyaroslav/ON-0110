///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Conversations;
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

namespace ClientTestTool.Tests.Definitions.TestCases.MediaSearch
{
  [Test(
  Name             = "Find Events With Search Filters",
  Category         = Category.ProfileG,
  Id               = "6",
  FeatureUnderTest = Feature.EventSearchFilter
  )]
  public class FindEventsWithSearchFilters : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var findEventsList = conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpMessage>().ContainsTag("FindEvents"))
          .ToList();
      
      AffectedPairs.AddRange(findEventsList);

      BeginStep("Client request contains <FindEvents> tag", findEventsList);

      if (0 == findEventsList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <FindEvents> tag");

      StepCompleted();

      var startPointList = findEventsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "FindEvents", "StartPoint")).ToList();

      BeginStep("<FindEvents> includes tag: <StartPoint>", startPointList);

      if (0 == startPointList.Count)
        StepFailed("<StartPoint> tag is not present");

      StepCompleted();

      var scopeList = findEventsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "FindEvents", "Scope")).ToList();

      BeginStep("<FindEvents> includes tag: <Scope>", scopeList);

      if (0 == scopeList.Count)
        StepFailed("<Scope> tag is not present");

      StepCompleted();

      var searchFilterList = findEventsList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "FindEvents", "SearchFilter"))
        .ToList();

      BeginStep("<FindEvents> includes tag: <SearchFilter>", searchFilterList);

      if (0 == searchFilterList.Count)
        StepFailed("<SearchFilter> tag is not present");

      StepCompleted();

      searchFilterList = searchFilterList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "SearchFilter"))).ToList();

      BeginStep("<SearchFilter> contains any XPath expression", searchFilterList);

      if (0 == searchFilterList.Count)
          throw new TestNotSupportedException("<SearchFilter> does not contain any value");
      
      StepCompleted();

      var includeStartStateList = findEventsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "FindEvents", "IncludeStartState")).ToList();

      BeginStep("<FindEvents> includes tag: <IncludeStartState>", includeStartStateList);

      if (0 == includeStartStateList.Count)
        StepFailed("<IncludeStartState> tag is not present");

      StepCompleted();

      var keepAliveTimeList = findEventsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "FindEvents", "KeepAliveTime")).ToList();

      BeginStep("<FindEvents>” includes tag: <KeepAliveTime>", findEventsList);

      if (0 == keepAliveTimeList.Count)
        StepFailed("<KeepAliveTime> tag is not present");

      StepCompleted();

      var responseList = findEventsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("FindEventsResponse")).ToList();

      BeginStep("Device response contains “<FindEventsResponse>” tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<FindEventsResponse> tag is not present");

      StepCompleted();
    }
  }
}
