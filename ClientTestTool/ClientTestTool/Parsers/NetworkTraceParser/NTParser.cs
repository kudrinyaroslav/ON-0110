///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.Data.Utils;
using ClientTestTool.Parsers.NetworkTraceParser.Base;
using ClientTestTool.Parsers.NetworkTraceParser.Extensions;
using ClientTestTool.Parsers.NetworkTraceParser.Utils;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.Packer;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.Parsers;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Query;
using ClientTestTool.Parsers.NetworkTraceParser.Utils.TShark.Utils;
using TSharkHelperTool.TShark;
using Device = ClientTestTool.Data.Definitions.Devices.Device;

namespace ClientTestTool.Parsers.NetworkTraceParser
{
  /// <summary>
  /// Network Trace files parser
  /// </summary>
  public class NTParser : BaseNTParser
  {
    /// <summary>
    /// ctor
    /// </summary>
    public NTParser(String[] protocols, NetworkTraceInfo networkTrace) : base(networkTrace, ApplicationState.ParserRunning)
    {
      mProtocols = protocols;
      CreateOutputDirectory();
    }

    #region Parsing

    protected override void Parse()
    {
      SetStatusMessage("Collecting frames...");
      ParseFrameList();
      
      SetStatusMessage("Searching for conversations...");
      ParseConversations();

      ParseFrames();

      SetStatusMessage("Searching for WS-Discovery messages");
      new WSDiscoveryParser(this).Parse();

      Progress = 95;

      SetStatusMessage("Obtaining device information");
      foreach (Unit unit in UnitSet.GetUnits(NetworkTrace))
        UnitAnalyzer.ParseUnitInfo(unit);

      SetStatusMessage("Serializing Frame List");
      SaveFrameList();
    }

    #endregion

    #region FrameList

    protected override void ParseFrameList()
    {
      using (var process = new TSharkProcess(new FrameListQuery(NetworkTrace.FullName, mProtocols)))
      {
        CreateFrameList(process.StandartOutput.ReadAllLines());
      }
    }
    
    #endregion

    #region Conversations

    private void ParseConversations()
    {
      String args = String.Format("{0} \"{1}\" {2} {3} {4} {3} {5} {3} {6} {3} {7} {8} \"{9}.{10}\"",
                                  TSharkHelper.ARG_READ_FILE,
                                  NetworkTrace.FullName,
                                  TSharkHelper.FILTER_FIELDS,
                                  TSharkHelper.FIELD,
                                  TSharkHelper.FIELD_REQUEST_IN,
                                  TSharkHelper.FIELD_FRAME_NUMBER,
                                  TSharkHelper.FIELD_MAC_DST,
                                  TSharkHelper.FIELD_MAC_SRC,
                                  TSharkHelper.ARG_DISPLAY_FILTER,
                                  CTTSettings.PROTOCOL_HTTP,
                                  CTTSettings.FILTER_RESPONSE);

      String output;

      if (!TSharkHelper.RunTShark(args, out output))
        return;

      var detectedDevices = FrameList.GetDevices();

      String[] lines = output.SplitToLines();
      foreach (String[] parts in lines.Select(line => line.Split(TSharkHelper.OUTPUT_SEPARATOR)))
      {
        int requestFrameNumber;
        int responseFrameNumber;

        int.TryParse(parts[0], out requestFrameNumber); // 0 as default value if TryParse failed

        if (0 == requestFrameNumber)
          continue;

        int.TryParse(parts[1], out responseFrameNumber);

        String clientMac = parts[2];
        String deviceMac = parts[3];

        Unit client = UnitSet.GetUnit(clientMac);

        if (null == client)
        {
          var macIpPair = detectedDevices.First(item => item.Item1 == clientMac);
          client = new Client(NetworkTrace, macIpPair.Item1, macIpPair.Item2);
          UnitSet.Add(client);
        }
        else
          client.FoundInTraces.Add(NetworkTrace);

        Unit device = UnitSet.GetUnit(deviceMac);

        if (null == device)
        {
          var macIpPair = detectedDevices.First(item => item.Item1 == deviceMac);
          device = new Device(NetworkTrace, macIpPair.Item1, macIpPair.Item2);
          UnitSet.Add(device);
        }
        else
          device.FoundInTraces.Add(NetworkTrace);

        var conversation = ConversationList.Find(client, device);

        if (null == conversation)
        {
          conversation = new Conversation(NetworkTrace, client, device);
          ConversationList.Add(conversation);
        }

        conversation.FoundInTraces.Add(NetworkTrace);
        Directory.CreateDirectory(ConversationHelper.GetConversationFolder(NetworkTrace, conversation));

        var request  = new HttpRequest (FrameList.GetFrame(requestFrameNumber) , conversation);
        var response = new HttpResponse(FrameList.GetFrame(responseFrameNumber), conversation);

        conversation.Add(new RequestResponsePair(request, response, NetworkTrace, conversation, ContentType.Http));
      }
    }

    #endregion

    #region Frames

    private void ParseFrames()
    {
      using (var process = new TSharkProcess(new FramesQuery(NetworkTrace.FullName, mProtocols)))
      {
        var frames = process.StandartOutput.ReadAllFrames();
        var packer = new FramePacker(FrameList);

        int frameIndex = 0;
        int frameListLength = FrameList.Count;

        foreach (var frame in frames)
        {
          if (frameIndex < frameListLength - 1)
            ++frameIndex;

          SetStatusMessage(String.Format("Processing frame {0}/{1}", frameIndex, frameListLength));

          bool isPacked = packer.PackFrame(frame); 
          //TODO remove frame from the FrameList if not packed

          Progress = frameIndex * 90 / frameListLength;
        }
      }
    }

    #endregion

    private readonly String[] mProtocols;

  }
}