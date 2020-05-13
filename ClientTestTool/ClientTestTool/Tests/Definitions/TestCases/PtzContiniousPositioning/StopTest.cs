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

namespace ClientTestTool.Tests.Definitions.TestCases.PtzContiniousPositioning
{
  [Test(
    Name             = "Stop",
    Id               = "3",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.Stop
  )]
  public class StopTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
        if (CheckStopPartPrivately(conversation))
            ShowStopPartFacade(conversation);
        else if (CheckContinuosMovePartPrivately(conversation))
            ShowContinuosMovePartFacade(conversation);
        else 
            ExecuteStopPart(conversation);
    }

    private void ExecuteContinuousMovePart(Conversation conversation)
    { 
    }

    private bool CheckContinuosMovePartPrivately(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var continuousMoveList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove")).ToList();

      if (0 == continuousMoveList.Count)
          return false;

      var profileTokenList = continuousMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      //BeginStep("<ContinuousMove> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        return false;//StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
          return false;// StepFailed("Value of <ProfileToken> tag is empty");

      //StepCompleted();

      var velocityList = continuousMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove", "Velocity")).ToList();
      //AffectedPairs.AddRange(velocityList);

      //BeginStep("<ContinuousMove> includes tag: <Velocity>", velocityList);

      if (0 == velocityList.Count)
          return false;//StepFailed("<Velocity> tag is missing");

      //StepCompleted();

      var panTiltList = velocityList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Velocity", "PanTilt")).ToList();
      //AffectedPairs.AddRange(panTiltList);

      var panTiltAttributesList = panTiltList.Select(item => TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "PanTilt")).ToList();
      //BeginStep("<PanTilt> tag contains attribute: “x=0” ", panTiltList);

      bool b = panTiltAttributesList.Any(item =>
      {
        var x = item.FirstOrDefault(attr => "x" == attr.Name);
        return null != x && ("0" == (x.Value));
      }) || panTiltAttributesList.Any(item =>
      {
        var y = item.FirstOrDefault(attr => "y" == attr.Name);
        return null != y && (y.Value == "0");
      });

      //StepCompleted();

      //BeginStep("<PanTilt> tag contains attribute: “y=0” ", panTiltList);

      //StepCompleted();

      var zoomList = velocityList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Velocity", "Zoom")).ToList();
      //AffectedPairs.AddRange(zoomList);

      var zoomAttributesList = zoomList.Select(item => TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "Zoom")).ToList();
      //BeginStep("<Zoom> tag contains attribute: “x=0” ", zoomList);

      if (zoomAttributesList.Any(item =>
        {
          var x = item.FirstOrDefault(attr => "x" == attr.Name);
          return null != x && (x.Value == "0");
        }))
        b = true;//StepFailed("<Zoom> tag does not contain attribute: \"x=0\" ");

      //StepCompleted();
      if (!b)
          return false;


      var responseList = continuousMoveList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
          return false;// StepFailed("Response does not contain 200 OK");

      //StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "ContinuousMoveResponse")).ToList();

      //BeginStep("Device response contains <ContinuousMoveResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
          return false;// StepFailed("<ContinuousMoveResponse> tag is missing");

      //StepCompleted();

      return true;
    }

    private void ShowContinuosMovePartFacade(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var continuousMoveList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove")).ToList();
      AffectedPairs.AddRange(continuousMoveList);

      BeginStep("Client request contains <ContinuousMove> tag", continuousMoveList);

      if (0 == continuousMoveList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <ContinuousMove> tag");

      StepCompleted();

      var profileTokenList = continuousMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<ContinuousMove> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      var velocityList = continuousMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove", "Velocity")).ToList();
      AffectedPairs.AddRange(velocityList);

      BeginStep("<ContinuousMove> includes tag: <Velocity>", velocityList);

      if (0 == velocityList.Count)
        StepFailed("<Velocity> tag is missing");

      StepCompleted();

      var panTiltList = velocityList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Velocity", "PanTilt")).ToList();
      AffectedPairs.AddRange(panTiltList);

      var panTiltAttributesList = panTiltList.Select(item => TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "PanTilt")).ToList();

      bool bPanTiltX0 = true;
      //BeginStep("<PanTilt> tag contains attribute: “x=0” ", panTiltList);

      if (!panTiltAttributesList.Any(item =>
          {
              var x = item.FirstOrDefault(attr => "x" == attr.Name);
              return null != x && ("0" == (x.Value));
          }))
          bPanTiltX0 = false;//StepFailed("<PanTilt> tag does not contain attribute: \"x=0\" ");

      //StepCompleted();
      if (bPanTiltX0)
      {
          BeginStep("<PanTilt> tag contains attribute: “x=0” ", panTiltList);
          StepCompleted();
      }
      else
      {
          bool bPanTiltY0 = true;
          //BeginStep("<PanTilt> tag contains attribute: “y=0” ", panTiltList);

          if (!panTiltAttributesList.Any(item =>
          {
              var y = item.FirstOrDefault(attr => "y" == attr.Name);
              return null != y && (y.Value == "0");
          }))
              bPanTiltY0 = false; //StepFailed("<PanTilt> tag does not contain attribute: \"y=0\"");

          //StepCompleted();
          if (bPanTiltY0)
          {
              BeginStep("<PanTilt> tag contains attribute: “y=0” ", panTiltList);
              StepCompleted();
          }
          else
          {
              var zoomList = velocityList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Velocity", "Zoom")).ToList();
              AffectedPairs.AddRange(zoomList);

              var zoomAttributesList = zoomList.Select(item => TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "Zoom")).ToList();
              bool bZoom0 = true;
              //BeginStep("<Zoom> tag contains attribute: “x=0” ", zoomList);

              if (!zoomAttributesList.Any(item =>
              {
                  var x = item.FirstOrDefault(attr => "x" == attr.Name);
                  return null != x && (x.Value == "0");
              }))
                  bZoom0 = false;//StepFailed("<Zoom> tag does not contain attribute: \"x=0\" ");

              //StepCompleted();
              if (bZoom0)
              {
                  BeginStep("<Zoom> tag contains attribute: “x=0” ", zoomList);
                  StepCompleted();
              }
          }
      }  

      var responseList = continuousMoveList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "ContinuousMoveResponse")).ToList();

      BeginStep("Device response contains <ContinuousMoveResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<ContinuousMoveResponse> tag is missing");

      StepCompleted();
    }           

    private void ExecuteStopPart(Conversation conversation)
    {
        var filteredList = conversation.GetMessages(ContentType.Http);

        var stopList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Stop")).ToList();
        AffectedPairs.AddRange(stopList);

        BeginStep("Client request contains <Stop> tag", stopList);

        if (0 == stopList.Count)
            throw new TestNotSupportedException("Conversation does not contain messages with <Stop> tag");

        StepCompleted();

        var profileTokenList = stopList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Stop", "ProfileToken")).ToList();
        var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

        BeginStep("<Stop> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

        if (0 == profileTokenList.Count)
            StepFailed("<ProfileToken> tag is missing");
        else if (valuesList.All(String.IsNullOrEmpty))
            StepFailed("Value of <ProfileToken> tag is empty");

        StepCompleted();

        var responseList = stopList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode)
                         .ToList();

        BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("Response does not contain 200 OK");

        StepCompleted();

        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "StopResponse")).ToList();

        BeginStep("Device response contains <StopResponse> tag", responseList, MessageType.Response);

        if (0 == responseList.Count)
            StepFailed("<StopResponse> tag is missing");

        StepCompleted();
    }

    private void ShowStopPartFacade(Conversation conversation)
    {
        var filteredList = conversation.GetMessages(ContentType.Http);

        var stopList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Stop")).ToList();
        AffectedPairs.AddRange(stopList);
        BeginStep("Client request contains <Stop> tag", stopList);
        StepCompleted();

        var profileTokenList = stopList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Stop", "ProfileToken")).ToList();
        BeginStep("<Stop> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);
        StepCompleted();

        var responseList = stopList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode)
                         .ToList();
        BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);
        StepCompleted();

        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "StopResponse")).ToList();
        BeginStep("Device response contains <StopResponse> tag", responseList, MessageType.Response);
        StepCompleted();
    }


    private bool CheckStopPartPrivately(Conversation conversation)
    {
        bool bRet = true;

        var filteredList = conversation.GetMessages(ContentType.Http);

        var stopList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Stop")).ToList();
        //AffectedPairs.AddRange(stopList);

        if (0 == stopList.Count)
            return false;

        var profileTokenList = stopList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Stop", "ProfileToken")).ToList();
        var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

        if (0 == profileTokenList.Count)
            return false;
        else if (valuesList.All(String.IsNullOrEmpty))
            return false;

        var responseList = stopList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode)
                         .ToList();

        if (0 == responseList.Count)
            return false;

        responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "StopResponse")).ToList();

        if (0 == responseList.Count)
            return false;

        return bRet;
    }
  }
}
