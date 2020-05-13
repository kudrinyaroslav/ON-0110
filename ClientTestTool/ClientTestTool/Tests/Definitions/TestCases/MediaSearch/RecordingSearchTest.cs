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
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.MediaSearch
{
  [Test(
    Name             = "Recording Search",
    Category         = Category.ProfileG,
    Id               = "1",
    FeatureUnderTest = Feature.RecordingSearch
  )]
  public class RecordingSearchTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var findRecordingsList = conversation.GetMessages(ContentType.Http).Where(item => item.GetRequest<HttpMessage>().ContainsTag("FindRecordings")).ToList();
      
      AffectedPairs.AddRange(findRecordingsList);

      BeginStep("Client request contains <FindRecordings> tag", findRecordingsList);

      if (0 == findRecordingsList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <FindRecordings> tag");

      StepCompleted();

      var scopeList = findRecordingsList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("FindRecordings", "Scope")).ToList();

      BeginStep("<FindRecordings> includes tag: <Scope>", scopeList);

      if (0 == scopeList.Count)
        StepFailed("<FindRecordings> does not include tag: <Scope>");

      StepCompleted();

      var keepAliveTimeList = findRecordingsList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("FindRecordings", "KeepAliveTime")).ToList();

      BeginStep("<FindRecordings> includes tag: <KeepAliveTime>", keepAliveTimeList);

      if (0 == keepAliveTimeList.Count)
        StepFailed("<FindRecordings> does not include tag: <KeepAliveTime>");

      StepCompleted();

      var responseList = findRecordingsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("FindRecordingsResponse")).ToList();

      BeginStep("Device response contains “<FindRecordingsResponse>” tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<FindRecordingsResponse> tag is not present");

      StepCompleted();

      var getRecordingSearchResultsList = conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetRecordingSearchResults"))
          .ToList();

      AffectedPairs.AddRange(getRecordingSearchResultsList);

      BeginStep("Client request contains <GetRecordingSearchResults> tag", getRecordingSearchResultsList);
        
      if (0 == getRecordingSearchResultsList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetRecordingSearchResults> tag");

      StepCompleted();

      var searchTokenList = getRecordingSearchResultsList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetRecordingSearchResults", "SearchToken")).ToList();

      BeginStep("<GetRecordingSearchResults> includes tag: <SearchToken>", searchTokenList);

      if (0 == searchTokenList.Count)
        StepFailed("<GetRecordingSearchResults> does not include tag: <SearchToken>");

      StepCompleted();

      responseList = getRecordingSearchResultsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("GetRecordingSearchResultsResponse")).ToList();

      BeginStep("Device response contains <GetRecordingSearchResultsResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetRecordingSearchResultsResponse> tag is not present");

      StepCompleted();
    }
  }
}
