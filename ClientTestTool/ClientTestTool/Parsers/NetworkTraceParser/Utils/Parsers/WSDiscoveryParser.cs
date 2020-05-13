///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Discovery;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Utils;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Parsers.NetworkTraceParser.Extensions;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.Decoders;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query;
using TSharkHelperTool.TShark;

namespace ClientTestTool.Parsers.NetworkTraceParser.Utils.Parsers
{
  internal class WSDiscoveryParser
  {
    public WSDiscoveryParser(NTParser parser)
    {
      if (null == parser)
        throw new ArgumentNullException();

      mParser = parser;
    }

    public void Parse()
    {
      var frameList = new List<Frame>();
      using (var process = new TSharkProcess(new FrameListQuery(mParser.NetworkTrace.FullName, new[] { WSD_REQUEST_FILTER, WSD_RESPONSE_FILTER })))
      {
        frameList.AddRange(process.StandartOutput.ReadAllLines()
                 .Select(line => line.Split('\t'))
                 .Select(parts =>
                 new Frame(int.Parse(parts[0]), mParser.NetworkTrace, parts[1], parts[2], String.Empty, String.Empty, "ws-discovery"))
                 .Where(item => null != UnitSet.GetUnit(item.SourceMac)));
      }

      if (0 == frameList.Count)
        return;

      var frames = new List<String>();
      using (var process = new TSharkProcess(new WSDiscoveryQuery(frameList.Select(item => item.Number), mParser.NetworkTrace.FullName)))
      {
        frames.AddRange(process.StandartOutput.ReadAllLines().Select(HexToASCIIConverter.Convert));
      }

      var xmlList   = BuildXmlList(ref frames);
      var requests  = xmlList.Where(item => null != item.GetElementWithName("Probe")).ToList();
      var responses = xmlList.Where(item => null != item.GetElementWithName("ProbeMatches")).Distinct(new ProbeMatchesEqualityComparer()).ToList();

      var pairs = BuildPairs(frameList, xmlList, requests, responses);

      ProcessPairs(frameList, xmlList, pairs);
    }

    #region Helpers

    private List<XElement> BuildXmlList(ref List<String> frames)
    {
      var xmlList = new List<XElement>();

      foreach (var frame in frames.ToList())
      {
        try
        {
          var doc = XElement.Load(new StringReader(frame));

          xmlList.Add(doc);
        }
        catch (XmlException e)
        {
          frames.Remove(frame);
          Logger.LogException("WSDiscoveryParser met exception while loading xml", e);
        }
      }

      return xmlList;
    }

    private IEnumerable<Tuple<XElement, XElement>> BuildPairs(List<Frame> frameList, List<XElement> xmlList, List<XElement> requests, List<XElement> responses)
    {
      var pairs = new List<Tuple<XElement, XElement>>();

      foreach (var response in responses)
      {
        var responseFrameNumber = frameList[xmlList.IndexOf(response)].Number;

        var requestPredicate = new Func<XElement, bool>(item =>
        {
          var messageID = item.GetElementWithName("MessageID");

          if (null == messageID)
            return false;

          var relatesToID = response.GetElementWithName("RelatesTo");

          if (null == relatesToID)
            return false;

          int requestFrameNumber = frameList[xmlList.IndexOf(item)].Number;

          return requestFrameNumber < responseFrameNumber && relatesToID.Value == messageID.Value;
        });

        var request = requests.FirstOrDefault(requestPredicate);

        if (request == null)
          continue;

        pairs.Add(Tuple.Create(request, response));
      }

      return pairs;
    }

    private void ProcessPairs(List<Frame> frameList, List<XElement> xmlList, IEnumerable<Tuple<XElement, XElement>> pairs)
    {
      foreach (var pair in pairs)
      {
        var requestFrame = frameList[xmlList.IndexOf(pair.Item1)];
        var responseFrame = frameList[xmlList.IndexOf(pair.Item2)];

        var client = UnitSet.GetUnit(responseFrame.DestinationMac);
        var device = UnitSet.GetUnit(responseFrame.SourceMac);

        var conversation = ConversationList.Find(client, device);

        if (null == conversation)
          continue;

        var request = new DiscoveryMessage(requestFrame, conversation, MessageType.Request);
        PackXml(pair.Item1, conversation, requestFrame, MessageType.Request);

        var response = new DiscoveryMessage(responseFrame, conversation, MessageType.Response);
        PackXml(pair.Item2, conversation, responseFrame, MessageType.Response);

        conversation.Add(new RequestResponsePair(request, response, mParser.NetworkTrace, conversation,
          ContentType.WSDiscovery));
      }
    }

    private void PackXml(XElement doc, Conversation conversation, Frame frame, MessageType type)
    {
      String xmlFilename = FrameHelper.GetFrameFilename(conversation, frame, type);
      File.WriteAllText(xmlFilename, doc.ToString());
    }

    #endregion

    private readonly NTParser mParser;

    private const string WSD_REQUEST_FILTER  = "udp.dstport eq 3702";
    private const string WSD_RESPONSE_FILTER = "udp.srcport eq 3702";
  }

  class ProbeMatchesEqualityComparer : IEqualityComparer<XElement>
  {
    public bool Equals(XElement x, XElement y)
    {
      var xMessageNumber = GetMessageNumber(x);
      var yMessageNumber = GetMessageNumber(y);

      return -1 != xMessageNumber && xMessageNumber == yMessageNumber;
    }

    public int GetHashCode(XElement obj)
    {
      return GetMessageNumber(obj).GetHashCode();
    }

    private int GetMessageNumber(XElement doc)
    {
      XElement appSequence = doc.GetElementWithName("AppSequence");

      if (null == appSequence)
        return -1;

      XAttribute messageNumberAttribute = appSequence.Attribute("MessageNumber");

      if (null == messageNumberAttribute)
        return -1;

      return int.Parse(messageNumberAttribute.Value);
    }
  }
}

